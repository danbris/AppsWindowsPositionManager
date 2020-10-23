using System;
using System.Collections.Generic;
using System.Diagnostics;
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
		
		public MainWindow()
		{
			InitializeComponent();
		}

		private void BtnGetOpenedWindows_OnClick(object sender, RoutedEventArgs e)
		{
			_monitorCheckTimer.Elapsed += OnMonitorCheck;
			_monitorCheckTimer.Start();

			//return;
			_winPosition.GetMonitorCount();
			foreach (KeyValuePair<IntPtr, string> window in _winPosition.GetOpenWindows())
			{
				IntPtr handle = window.Key;
				string title = window.Value;

				var windowRect = new Rectangle();
				User32Wrapper.GetWindowRect(handle, ref windowRect);

				lstBox.Items.Add($"{handle}: {title} : {windowRect}");
				if (title.Contains("iTunes"))
				{
					User32Wrapper.SetWindowPos(handle, (IntPtr) SpecialWindowHandles.HWND_TOP, windowRect.Left + 1000,
						windowRect.Top + 100, windowRect.Right - windowRect.Left, windowRect.Bottom - windowRect.Top,
						SetWindowPosFlags.ShowWindow);
				}
			}
		}

		private void OnMonitorCheck(object sender, ElapsedEventArgs e)
		{
			try
			{
				_winPosition.RefreshWindowsState();
			}
			catch(Exception ex)
			{
				Debug.WriteLine(ex);
			}
		}
	}
}
