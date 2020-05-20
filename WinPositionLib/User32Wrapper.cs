using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WinPositionLib
{
	public class User32Wrapper
	{
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr FindWindow(string strClassName, string strWindowName);

		[DllImport("user32.dll")]
		public static extern bool GetWindowRect(IntPtr hwnd, ref Rectangle rectangle);

		[DllImport("user32.dll")]
		public static extern bool EnumWindows(EnumWindowsProc enumWindowsProc, int lParam);
		public delegate bool EnumWindowsProc(IntPtr hWnd, int lParam);

		[DllImport("user32")]
		public static extern bool EnumDisplayMonitors(IntPtr hdc, IntPtr lpRect, MonitorEnumProc callback, int dwData);
		public delegate bool MonitorEnumProc(IntPtr hDesktop, IntPtr hdc, ref Rectangle pRect, int dwData);


		[DllImport("USER32.DLL")]
		public static extern bool IsWindowVisible(IntPtr hWnd);

		[DllImport("USER32.DLL")]
		public static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

		[DllImport("USER32.DLL")]
		public static extern int GetWindowTextLength(IntPtr hWnd);

		[DllImport("USER32.DLL")]
		public static extern IntPtr GetShellWindow();

		[DllImport("USER32.DLL")]
		public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy,
			SetWindowPosFlags uFlags);
	}
}
