using System;
using System.Runtime.InteropServices;

namespace WinPositionLib
{
	/// <summary>
	/// Represents a screen configuration with number of screens.
	/// </summary>
	public class ScreenConfiguration
	{
		/// <summary>
		/// Gets or sets the number of screens in the configuration.
		/// </summary>
		public int NoOfScreens { get; set; }
	}

	/// <summary>
	/// Represents details about a specific screen monitor.
	/// </summary>
	public class ScreenDetail
	{
		/// <summary>
		/// Gets or sets the monitor handle.
		/// </summary>
		public IntPtr Handle { get; set; }
		
		/// <summary>
		/// Gets or sets the screen information.
		/// </summary>
		public ScreenInfo Info { get; set; }
	}

	/// <summary>
	/// Contains information about a monitor's dimensions and properties.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct ScreenInfo
	{
		/// <summary>
		/// Size of the structure in bytes.
		/// </summary>
		public uint Size;
		
		/// <summary>
		/// The display monitor rectangle.
		/// </summary>
		public Rectangle Monitor;
		
		/// <summary>
		/// The work area rectangle.
		/// </summary>
		public Rectangle Work;
		
		/// <summary>
		/// Monitor flags.
		/// </summary>
		public uint Flags;
	}
}
