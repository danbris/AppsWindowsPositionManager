using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinPositionLib
{
	public class ScreenConfiguration
	{
		public int NoOfScreens { get; set; }

	}

	public class ScreenDetail
	{
		public IntPtr ScreenHandle { get; set; }
		public Rectangle Rectangle { get; set; }
	}
}
