using System;
using System.Collections.Generic;
using System.Text;

namespace WinPositionLib
{
	public class WinPosition
    {
	    private int _previousMonitorCount = 0;
	    private readonly Dictionary<IntPtr, Rectangle> _windowsPositions = new Dictionary<IntPtr, Rectangle>();

	    public static IDictionary<IntPtr, string> GetOpenWindows()
	    {
		    var shellWindow = User32Wrapper.GetShellWindow();
		    var windows = new Dictionary<IntPtr, string>();

		    User32Wrapper.EnumWindows(delegate (IntPtr hWnd, int lParam)
		    {
			    if (hWnd == shellWindow) return true;
			    if (!User32Wrapper.IsWindowVisible(hWnd)) return true;

			    var length = User32Wrapper.GetWindowTextLength(hWnd);
			    if (length == 0) return true;

			    var builder = new StringBuilder(length);
			    User32Wrapper.GetWindowText(hWnd, builder, length + 1);

			    windows[hWnd] = builder.ToString();
			    return true;

		    }, 0);

		    return windows;
	    }

	    private static int GetMonitorCount()
	    {
		    var monCount = 0;
		    bool Callback(IntPtr hDesktop, IntPtr hdc, ref Rectangle prect, int d) => ++monCount > 0;
			User32Wrapper.EnumDisplayMonitors(IntPtr.Zero, IntPtr.Zero, Callback, 0);
			return monCount;
	    }

	    public void RefreshWindowsState()
	    {
		    var monitorsCount = GetMonitorCount();
		    if (monitorsCount > _previousMonitorCount)
		    {
			    //restore windows to original position when there were more monitors
			    RestoreWindowsPositons();
		    }
		    else if (monitorsCount > 1)
		    {
			    //save current windows positions
			    SaveWindowsPositions();
		    }

		    _previousMonitorCount = monitorsCount;
	    }

	    private void SaveWindowsPositions()
	    {
		    _windowsPositions.Clear();
		    foreach (var window in GetOpenWindows())
		    {
			    var handle = window.Key;
			    var windowRect = new Rectangle();
			    User32Wrapper.GetWindowRect(handle, ref windowRect);
				_windowsPositions.Add(handle, windowRect);
		    }
	    }

	    private void RestoreWindowsPositons()
	    {
		    foreach (var windowsPosition in _windowsPositions)
		    {// skip minimized windows

			    var windowRect = windowsPosition.Value;
			    User32Wrapper.SetWindowPos(windowsPosition.Key, (IntPtr)SpecialWindowHandles.HWND_NOTOPMOST, windowRect.Left,
				    windowRect.Top, windowRect.Right - windowRect.Left, windowRect.Bottom - windowRect.Top,
				    SetWindowPosFlags.ShowWindow);
		    }
	    }
	}
}
