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
		private readonly Timer _monitorCheckTimer = new Timer(10000);
		private readonly WinPosition _winPosition = new WinPosition();

		public WindowsKeeper()
		{
			InitializeComponent();
			//ServiceName = "Windows position keeper";
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
		}
	}
}
