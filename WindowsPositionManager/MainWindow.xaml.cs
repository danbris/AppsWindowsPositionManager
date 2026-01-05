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
		private const int MonitorCheckIntervalMs = 10000;
		
		private readonly Timer _monitorCheckTimer = new Timer(MonitorCheckIntervalMs);
		private readonly WinPosition _winPosition = new WinPosition();
		
		public MainWindow()
		{
			InitializeComponent();
		}

		private void BtnGetOpenedWindows_OnClick(object sender, RoutedEventArgs e)
		{
			_monitorCheckTimer.Elapsed += OnMonitorCheck;
			_monitorCheckTimer.Start();

			var monitorCount = _winPosition.GetMonitorCount();
			lstBox.Items.Add($"Monitor Count: {monitorCount}");
			
			foreach (KeyValuePair<IntPtr, string> window in _winPosition.GetOpenWindows())
			{
				IntPtr handle = window.Key;
				string title = window.Value;

				var windowRect = new Rectangle();
				if (User32Wrapper.GetWindowRect(handle, ref windowRect))
				{
					lstBox.Items.Add($"{handle}: {title} : {windowRect}");
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
