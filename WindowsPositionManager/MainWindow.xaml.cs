using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Timers;
using System.Windows;
using WinPositionLib;

namespace WindowsPositionManager
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private readonly Timer _monitorCheckTimer = new Timer(10000);
		private readonly WinPosition _winPosition = new WinPosition();

		private int _previousMonitorNo = 0;
		Dictionary<IntPtr, Rect> _windowsPositions = new Dictionary<IntPtr, Rect>();

		public MainWindow()
		{
			InitializeComponent();
		}

		

		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr FindWindow(string strClassName, string strWindowName);

		[DllImport("user32.dll")]
		public static extern bool GetWindowRect(IntPtr hwnd, ref Rect rectangle);

		[DllImport("user32.dll")]
		public static extern bool EnumWindows(EnumWindowsProc enumWindowsProc, int lParam);
		public delegate bool EnumWindowsProc(IntPtr hWnd, int lParam);

		[DllImport("user32")]
		private static extern bool EnumDisplayMonitors(IntPtr hdc, IntPtr lpRect, MonitorEnumProc callback, int dwData);
		private delegate bool MonitorEnumProc(IntPtr hDesktop, IntPtr hdc, ref Rect pRect, int dwData);


		[DllImport("USER32.DLL")]
		private static extern bool IsWindowVisible(IntPtr hWnd);

		[DllImport("USER32.DLL")]
		private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

		[DllImport("USER32.DLL")]
		private static extern int GetWindowTextLength(IntPtr hWnd);

		[DllImport("USER32.DLL")]
		private static extern IntPtr GetShellWindow();

		[DllImport("USER32.DLL")]
		static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy,
			SetWindowPosFlags uFlags);

		public struct Rect
		{
			public int Left { get; set; }
			public int Top { get; set; }
			public int Right { get; set; }
			public int Bottom { get; set; }

			public override string ToString()
			{
				return $"{Left}:{Top}:{Right}:{Bottom}";
			}
		}

		public static IDictionary<IntPtr, string> GetOpenWindows()
		{
			IntPtr shellWindow = GetShellWindow();
			Dictionary<IntPtr, string> windows = new Dictionary<IntPtr, string>();

			EnumWindows(delegate(IntPtr hWnd, int lParam)
			{
				if (hWnd == shellWindow) return true;
				if (!IsWindowVisible(hWnd)) return true;

				int length = GetWindowTextLength(hWnd);
				if (length == 0) return true;

				StringBuilder builder = new StringBuilder(length);
				GetWindowText(hWnd, builder, length + 1);

				windows[hWnd] = builder.ToString();
				return true;

			}, 0);

			return windows;
		}

		private void BtnGetOpenedWindows_OnClick(object sender, RoutedEventArgs e)
		{
			_monitorCheckTimer.Elapsed += OnMonitorCheck;
			_monitorCheckTimer.Start();

			return;
			var count = GetMonitorCount();
			foreach (KeyValuePair<IntPtr, string> window in GetOpenWindows())
			{
				IntPtr handle = window.Key;
				string title = window.Value;

				var windowRect = new Rect();
				GetWindowRect(handle, ref windowRect);

				lstBox.Items.Add($"{handle}: {title} : {windowRect}");
				if (title.Contains("iTunes"))
				{
					SetWindowPos(handle, (IntPtr) SpecialWindowHandles.HWND_TOP, windowRect.Left + 1000,
						windowRect.Top + 100, windowRect.Right - windowRect.Left, windowRect.Bottom - windowRect.Top,
						SetWindowPosFlags.ShowWindow);
				}
			}
		}

		private void OnMonitorCheck(object sender, ElapsedEventArgs e)
		{
			_winPosition.RefreshWindowsState();
		}

		private int GetMonitorCount()
		{
			int monCount = 0;
			bool Callback(IntPtr hDesktop, IntPtr hdc, ref Rect prect, int d) => ++monCount > 0;

			EnumDisplayMonitors(IntPtr.Zero, IntPtr.Zero, Callback, 0);

			return monCount;
		}

		private void CheckWindowsState()
		{
			var monitorsNo = GetMonitorCount();
			if (monitorsNo > _previousMonitorNo)
			{
				//restore windows to original position when there were 3 monitors
				RestoreWindowsPositons();
			}
			else if (monitorsNo == _previousMonitorNo && monitorsNo == 3)
			{
				//save current windows positions
				SaveWindowsPositions();
			}
			
			_previousMonitorNo = monitorsNo;
		}

		private void SaveWindowsPositions()
		{
			_windowsPositions.Clear();
			foreach (KeyValuePair<IntPtr, string> window in GetOpenWindows())
			{
				IntPtr handle = window.Key;
				var windowRect = new Rect();
				GetWindowRect(handle, ref windowRect);

				_windowsPositions.Add(handle, windowRect);

				//lstBox.Items.Add($"{handle}: {title} : {windowRect}");
				//if (title.Contains("iTunes"))
				//{
				//	SetWindowPos(handle, (IntPtr)SpecialWindowHandles.HWND_TOP, windowRect.Left + 1000,
				//		windowRect.Top + 100, windowRect.Right - windowRect.Left, windowRect.Bottom - windowRect.Top,
				//		SetWindowPosFlags.ShowWindow);
				//}
			}
		}

		private void RestoreWindowsPositons()
		{
			foreach (var windowsPosition in _windowsPositions)
			{
				var windowRect = windowsPosition.Value;
				SetWindowPos(windowsPosition.Key, (IntPtr)SpecialWindowHandles.HWND_NOTOPMOST, windowRect.Left,
						windowRect.Top, windowRect.Right - windowRect.Left, windowRect.Bottom - windowRect.Top,
						SetWindowPosFlags.ShowWindow);
			}
		}
	}
}
