namespace Sokoban.models
{
	public class Field
	{
		private readonly Tiles[,] tiles;

		public readonly int Width;
		public readonly int Height;

		public Field(Tiles[,] tiles)
		{
			this.tiles = tiles;
			Width = tiles.GetLength(1);
			Height = tiles.GetLength(0);
		}

		public Tiles GetTileAt(Point at)
		{
			return tiles[at.Row, at.Col];
		}

		public bool IsOutOfBounds(Point translated)
		{
			if (translated.Col < 0) {
				return true;
			}

			if (translated.Col >= Width) {
				return true;
			}

			if (translated.Row < 0) {
				return true;
			}

			if (translated.Row >= Height) {
				return true;
			}

			return false;
		}
	}
}
