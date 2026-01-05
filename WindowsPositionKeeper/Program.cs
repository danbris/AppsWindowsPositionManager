using System.ServiceProcess;

namespace WindowsPositionKeeper
{
	/// <summary>
	/// Main program entry point for the Windows Position Keeper service.
	/// </summary>
	internal static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		private static void Main()
		{
			var servicesToRun = new ServiceBase[]
			{
				new WindowsKeeper()
			};
			ServiceBase.Run(servicesToRun);
		}
	}
}
