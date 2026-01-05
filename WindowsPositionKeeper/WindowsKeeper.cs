using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Timers;
using WinPositionLib;

namespace WindowsPositionKeeper
{
	public partial class WindowsKeeper : ServiceBase
	{
		private const int MonitorCheckIntervalMs = 10000;
		
		private readonly Timer _monitorCheckTimer = new Timer(MonitorCheckIntervalMs);
		private readonly WinPosition _winPosition = new WinPosition();

		public WindowsKeeper()
		{
			InitializeComponent();
		}

		protected override void OnStart(string[] args)
		{
			_monitorCheckTimer.Elapsed += OnMonitorCheck;
			_monitorCheckTimer.Start();
		}

		private void OnMonitorCheck(object sender, ElapsedEventArgs e)
		{
			_winPosition.RefreshWindowsState();
		}

		protected override void OnStop()
		{
			_monitorCheckTimer.Stop();
			_monitorCheckTimer.Elapsed -= OnMonitorCheck;
			_monitorCheckTimer.Dispose();
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				_monitorCheckTimer?.Dispose();
			}
			base.Dispose(disposing);
		}
	}
}
