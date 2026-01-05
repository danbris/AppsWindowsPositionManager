using System;

namespace WinPositionLib
{
    /// <summary>
    /// Represents information about a window's handle and position.
    /// </summary>
    public class WindowInfo
    {
        /// <summary>
        /// Gets or sets the window handle.
        /// </summary>
        public IntPtr WHandle { get; set; }
        
        /// <summary>
        /// Gets or sets the window's rectangle position.
        /// </summary>
        public Rectangle Position { get; set; }
    }
}
