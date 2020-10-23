namespace WinPositionLib
{
    public struct Rectangle
	{
		public int Left { get; set; }
		public int Top { get; set; }
		public int Right { get; set; }
		public int Bottom { get; set; }

		public override string ToString()
		{
			return $"{Left}:{Top}:{Right}:{Bottom}";
		}
	}
}
