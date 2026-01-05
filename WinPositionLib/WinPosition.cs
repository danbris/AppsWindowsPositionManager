using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Utils.Extensions;

namespace WinPositionLib
{
	/// <summary>
	/// Manages window positions across different monitor configurations.
	/// </summary>
	public class WinPosition
	{
		private const uint MONITOR_DEFAULTTONEAREST = 0x00000002;
		private const int MINIMUM_MONITOR_COUNT = 1;
		
		private int _previousMonitorCount = 0;
		private readonly Dictionary<IntPtr, ScreenDetail> _screenConfigs = new Dictionary<IntPtr, ScreenDetail>();
		private readonly Dictionary<int, List<WindowInfo>> _screenWindowLayout = new Dictionary<int, List<WindowInfo>>();

		/// <summary>
		/// Gets all currently open and visible windows.
		/// </summary>
		/// <returns>A dictionary mapping window handles to window titles.</returns>
		public IDictionary<IntPtr, string> GetOpenWindows()
		{
			var shellWindow = User32Wrapper.GetShellWindow();
			var windows = new Dictionary<IntPtr, string>();

			User32Wrapper.EnumWindows(delegate (IntPtr hWnd, int lParam)
			{
				if (hWnd == shellWindow) return true;
				if (!User32Wrapper.IsWindowVisible(hWnd)) return true;

				var length = User32Wrapper.GetWindowTextLength(hWnd);
				if (length == 0) return true;

				var builder = new StringBuilder(length);
				User32Wrapper.GetWindowText(hWnd, builder, length + 1);

				windows[hWnd] = builder.ToString();
				return true;
			}, 0);

			return windows;
		}

		/// <summary>
		/// Gets the current monitor count by enumerating all display monitors.
		/// </summary>
		/// <returns>The number of monitors detected.</returns>
		public int GetMonitorCount()
		{
			_screenConfigs.Clear();
			User32Wrapper.EnumDisplayMonitors(IntPtr.Zero, IntPtr.Zero, MonitorEnum, 0);
			return _screenConfigs.Count;
		}

		/// <summary>
		/// Callback function for monitor enumeration.
		/// </summary>
		private bool MonitorEnum(IntPtr hMonitor, IntPtr hdcMonitor, ref Rectangle lprcMonitor, IntPtr dwData)
		{
			var mi = new ScreenInfo();
			mi.Size = (uint)Marshal.SizeOf(mi);
			User32Wrapper.GetMonitorInfo(hMonitor, ref mi);

			var screenDetail = new ScreenDetail { Handle = hMonitor, Info = mi };
			_screenConfigs.SafeAdd(hMonitor, screenDetail);

			return true;
		}

		/// <summary>
		/// Checks current monitor configuration and saves/restores window positions as needed.
		/// </summary>
		public void RefreshWindowsState()
		{
			var monitorsCount = GetMonitorCount();
			
			// Restore windows when more monitors are detected after having multiple monitors previously
			if (monitorsCount > _previousMonitorCount && _previousMonitorCount > MINIMUM_MONITOR_COUNT)
			{
				RestoreWindowsPositions();
			}
			// Save current layout when transitioning from single to multiple monitors
			else if (_previousMonitorCount <= MINIMUM_MONITOR_COUNT && monitorsCount > MINIMUM_MONITOR_COUNT)
			{
				SaveWindowsPositions();
			}

			_previousMonitorCount = monitorsCount;
		}

		/// <summary>
		/// Saves the current positions of all open windows.
		/// </summary>
		private void SaveWindowsPositions()
		{
			var screenCount = GetMonitorCount();
			var windowConfiguration = new List<WindowInfo>();
			
			foreach (var window in GetOpenWindows())
			{
				var handle = window.Key;
				var windowRect = new Rectangle();
				
				if (User32Wrapper.GetWindowRect(handle, ref windowRect))
				{
					windowConfiguration.Add(new WindowInfo { WHandle = handle, Position = windowRect });
				}
			}

			_screenWindowLayout.SafeAdd(screenCount, windowConfiguration);
		}

		/// <summary>
		/// Restores window positions for the current monitor configuration.
		/// </summary>
		private void RestoreWindowsPositions()
		{
			var screenCount = GetMonitorCount();
			if (!_screenWindowLayout.ContainsKey(screenCount)) return;

			foreach (var windowsPosition in _screenWindowLayout[screenCount])
			{
				// Validate that the window handle is still valid
				if (!User32Wrapper.IsWindowVisible(windowsPosition.WHandle))
				{
					continue;
				}

				var windowRect = windowsPosition.Position;
				User32Wrapper.SetWindowPos(
					windowsPosition.WHandle,
					(IntPtr)SpecialWindowHandles.HWND_NOTOPMOST,
					windowRect.Left,
					windowRect.Top,
					windowRect.Right - windowRect.Left,
					windowRect.Bottom - windowRect.Top,
					SetWindowPosFlags.ShowWindow);
			}
		}


	}
}
