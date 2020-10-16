using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Utils.Extensions;

namespace WinPositionLib
{
	public class WinPosition
	{
		private const uint MONITOR_DEFAULTTONEAREST = 0x00000002;
		private int _previousMonitorCount = 0;
		private static readonly Dictionary<IntPtr, ScreenDetail> _screenConfigs = new Dictionary<IntPtr, ScreenDetail>();
		private readonly Dictionary<int, List<WindowInfo>> _screenWindowLayout = new Dictionary<int, List<WindowInfo>>();

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

		public static int GetMonitorCount()
		{
			//bool Callback(IntPtr hDesktop, IntPtr hdc, ref Rectangle prect, int d) => ++monCount > 0;
			//this will enumerate all displays and call MonitorEnum for each of them
			//=> _screenConfig will be updated
			_screenConfigs.Clear();
			User32Wrapper.EnumDisplayMonitors(IntPtr.Zero, IntPtr.Zero, MonitorEnum, 0);
			return _screenConfigs.Count;
		}

		private static bool MonitorEnum(IntPtr hMonitor, IntPtr hdcMonitor, ref Rectangle lprcMonitor, IntPtr dwData)
		{
			var mi = new ScreenInfo();
			mi.Size = (uint)Marshal.SizeOf(mi);
			User32Wrapper.GetMonitorInfo(hMonitor, ref mi);

			//think of _screenConfigs as a dictionary, where the key is the handle, so that 
			//it keeps a record of all screens, just in case it's used again in that session
			var screenDetail = new ScreenDetail { Handle = hMonitor, Info = mi };
			_screenConfigs.SafeAdd(hMonitor, screenDetail);

			return true;
		}

		public void RefreshWindowsState()
		{
			var monitorsCount = GetMonitorCount();
			// ignore 1 screen setup.
			if (monitorsCount > _previousMonitorCount && _previousMonitorCount > 0)
			{
				//restore windows to original position when there were more monitors
				RestoreWindowsPositions();
			}
			else if (_previousMonitorCount == 0 && monitorsCount > 1)
		    {
			    //save current windows positions
			    SaveWindowsPositions();
		    }

		    _previousMonitorCount = monitorsCount;
	    }

		private void SaveWindowsPositions()
		{
			var screenCount = GetMonitorCount();
			var windowConfiguration = new List<WindowInfo>();
			foreach (var window in GetOpenWindows())
			{
				var handle = window.Key;
				var windowRect = new Rectangle();
				User32Wrapper.GetWindowRect(handle, ref windowRect);
				//var monitorHandle = User32Wrapper.MonitorFromRect(ref windowRect, MONITOR_DEFAULTTONEAREST);

				windowConfiguration.Add(new WindowInfo { WHandle = handle, Position = windowRect });
			}

			_screenWindowLayout.SafeAdd(screenCount, windowConfiguration);
	    }

	    private void RestoreWindowsPositions()
	    {
			var screenCount = GetMonitorCount();
			if (!_screenWindowLayout.ContainsKey(screenCount)) return;
			foreach (var windowsPosition in _screenWindowLayout[screenCount])
		    {
				// skip minimized windows

			    var windowRect = windowsPosition.Position;
			    User32Wrapper.SetWindowPos(windowsPosition.WHandle, (IntPtr)SpecialWindowHandles.HWND_NOTOPMOST, windowRect.Left,
				    windowRect.Top, windowRect.Right - windowRect.Left, windowRect.Bottom - windowRect.Top,
				    SetWindowPosFlags.ShowWindow);
		    }
	    }


	}
}
