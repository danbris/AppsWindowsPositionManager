namespace WinPositionLib
{
    /// <summary>
    /// Represents a rectangle with integer coordinates.
    /// </summary>
    public struct Rectangle
	{
		/// <summary>
		/// Gets or sets the x-coordinate of the left edge.
		/// </summary>
		public int Left { get; set; }
		
		/// <summary>
		/// Gets or sets the y-coordinate of the top edge.
		/// </summary>
		public int Top { get; set; }
		
		/// <summary>
		/// Gets or sets the x-coordinate of the right edge.
		/// </summary>
		public int Right { get; set; }
		
		/// <summary>
		/// Gets or sets the y-coordinate of the bottom edge.
		/// </summary>
		public int Bottom { get; set; }

		/// <summary>
		/// Returns a string representation of the rectangle.
		/// </summary>
		public override string ToString()
		{
			return $"{Left}:{Top}:{Right}:{Bottom}";
		}
	}
}
