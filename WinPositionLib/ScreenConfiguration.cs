using System;
using System.Runtime.InteropServices;

namespace WinPositionLib
{
	public class ScreenConfiguration
	{
		public int NoOfScreens { get; set; }

	}

	public class ScreenDetail
	{
		public IntPtr Handle { get; set; }
		public ScreenInfo Info { get; set; }
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct ScreenInfo
	{
		public uint Size;
		public Rectangle Monitor;
		public Rectangle Work;
		public uint Flags;
	}
}
