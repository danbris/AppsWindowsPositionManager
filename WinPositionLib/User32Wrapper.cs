using System;
using System.Runtime.InteropServices;
using System.Text;

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


		/// <summary>
		/// Enumerates through the display monitors.
		/// </summary>
		/// <param name="hdc">A handle to a display device context that defines the visible region of interest.</param>
		/// <param name="lprcClip">A pointer to a RECT structure that specifies a clipping rectangle.</param>
		/// <param name="lpfnEnum">A pointer to a MonitorEnumProc application-defined callback function.</param>
		/// <param name="dwData">Application-defined data that EnumDisplayMonitors passes directly to the MonitorEnumProc function.</param>
		/// <returns></returns>
		[DllImport("user32")]
		public static extern bool EnumDisplayMonitors(IntPtr hdc, IntPtr lpRect, MonitorEnumProc callback, int dwData);
		/// <summary>
		/// Monitor Enum Delegate
		/// </summary>
		/// <param name="hDesktop">A handle to the display monitor.</param>
		/// <param name="hdc">A handle to a device context.</param>
		/// <param name="pRect">A pointer to a RECT structure.</param>
		/// <param name="dwData">Application-defined data that EnumDisplayMonitors passes directly to the enumeration function.</param>
		/// <returns></returns>
		public delegate bool MonitorEnumProc(IntPtr hDesktop, IntPtr hdc, ref Rectangle pRect, IntPtr dwData);

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

		/// <summary>
		/// Gets the monitor information.
		/// </summary>
		/// <param name="hmon">A handle to the display monitor of interest.</param>
		/// <param name="mi">A pointer to a MONITORINFO instance created by this method.</param>
		/// <returns></returns>
		[DllImport("user32.dll")]
		public static extern bool GetMonitorInfo(IntPtr hmon, ref ScreenInfo mi);

		/// <summary>
		/// Monitors from rect.
		/// </summary>
		/// <param name="rectPointer">The Rectangle pointer.</param>
		/// <param name="flags">The flags.</param>
		/// <returns></returns>
		[DllImport("user32.dll")]
		public static extern IntPtr MonitorFromRect([In] ref Rectangle rectPointer, uint flags);
	}
}
