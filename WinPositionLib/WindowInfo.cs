using System;

namespace WinPositionLib
{
    public class WindowInfo
    {
        public string Title { get; set; }
        public IntPtr WHandle { get; set; }
        public Rectangle Position { get; set; }
    }
}
