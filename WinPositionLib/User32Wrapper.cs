using System;
using System.Runtime.InteropServices;
using System.Text;

namespace WinPositionLib
{
    /// <summary>
    /// Wrapper class for User32.dll Win32 API functions.
    /// </summary>
    public class User32Wrapper
	{
		/// <summary>
		/// Retrieves a handle to a window whose class name and window name match the specified strings.
		/// </summary>
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr FindWindow(string strClassName, string strWindowName);

		/// <summary>
		/// Retrieves the dimensions of the bounding rectangle of the specified window.
		/// </summary>
		[DllImport("user32.dll")]
		public static extern bool GetWindowRect(IntPtr hwnd, ref Rectangle rectangle);

		/// <summary>
		/// Enumerates all top-level windows on the screen.
		/// </summary>
		[DllImport("user32.dll")]
		public static extern bool EnumWindows(EnumWindowsProc enumWindowsProc, int lParam);
		
		/// <summary>
		/// Delegate for EnumWindows callback.
		/// </summary>
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
		/// <returns>True to continue enumeration, false to stop.</returns>
		public delegate bool MonitorEnumProc(IntPtr hDesktop, IntPtr hdc, ref Rectangle pRect, IntPtr dwData);

		/// <summary>
		/// Determines whether the specified window handle identifies an existing window.
		/// </summary>
		[DllImport("USER32.DLL")]
		public static extern bool IsWindowVisible(IntPtr hWnd);

		/// <summary>
		/// Copies the text of the specified window's title bar into a buffer.
		/// </summary>
		[DllImport("USER32.DLL")]
		public static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

		/// <summary>
		/// Retrieves the length of the specified window's title bar text.
		/// </summary>
		[DllImport("USER32.DLL")]
		public static extern int GetWindowTextLength(IntPtr hWnd);

		/// <summary>
		/// Retrieves a handle to the Shell's desktop window.
		/// </summary>
		[DllImport("USER32.DLL")]
		public static extern IntPtr GetShellWindow();

		/// <summary>
		/// Changes the size, position, and Z order of a window.
		/// </summary>
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
