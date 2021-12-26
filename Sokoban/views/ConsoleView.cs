using System;
using System.Collections.Generic;
using Sokoban.models;
using SokobanModel = Sokoban.models.Sokoban;

namespace Sokoban.views
{
	public class ConsoleView: IView
	{
		private readonly SokobanModel sokoban;

		private static readonly Dictionary<Tiles, ConsoleColor> TileColors = new Dictionary<Tiles, ConsoleColor> {
			{Tiles.Wall,        ConsoleColor.DarkGray},
			{Tiles.Grass,       ConsoleColor.DarkGreen},
			{Tiles.Crate,       ConsoleColor.White},
			{Tiles.Mark,        ConsoleColor.Red},
			{Tiles.CrateOnMark, ConsoleColor.Yellow},
			{Tiles.Player,      ConsoleColor.Blue},
		};
		private static readonly Dictionary<Tiles, ConsoleColor> TileBackgrounds = new Dictionary<Tiles, ConsoleColor> {
			{Tiles.Wall,        ConsoleColor.DarkGray},
			{Tiles.Grass,       ConsoleColor.DarkGreen},
			{Tiles.Crate,       ConsoleColor.DarkGreen},
			{Tiles.Mark,        ConsoleColor.DarkGreen},
			{Tiles.CrateOnMark, ConsoleColor.DarkGreen},
			{Tiles.Player,      ConsoleColor.DarkGreen},
		};

		private const ConsoleColor FieldBgColor = ConsoleColor.Gray;
		private const ConsoleColor TextBgColor = ConsoleColor.Black;
		private const ConsoleColor TextFgColor = ConsoleColor.Yellow;

		public ConsoleView(SokobanModel sokoban)
		{
			this.sokoban = sokoban;
		}

		public void WriteLn(string str)
		{
			var bg = Console.BackgroundColor;
			var fg = Console.ForegroundColor;
			Console.BackgroundColor = TextBgColor;
			Console.ForegroundColor = TextFgColor;
			Console.WriteLine(str);
			Console.BackgroundColor = bg;
			Console.ForegroundColor = fg;
		}

		public void Render()
		{
			var bg = Console.BackgroundColor;
			var fg = Console.ForegroundColor;
			Console.Clear();
			Console.BackgroundColor = TextBgColor;
			Console.ForegroundColor = TextFgColor;
			Console.WriteLine($"Moves count: {sokoban.MovesCount.ToString()}");
			Console.WriteLine();

			Console.BackgroundColor = FieldBgColor;
			for (var y = 0; y < sokoban.FieldHeight(); y++)
			{
				for (var x = 0; x < sokoban.FieldWidth(); x++)
				{
					var at = new Point(x, y);
					var tile = sokoban.GetTileAt(at);
					if (tile == Tiles.Crate && sokoban.IsMark(at)) {
						tile = Tiles.CrateOnMark;
					}

					RenderTile(tile);
				}
				Console.WriteLine();
			}

			Console.BackgroundColor = TextBgColor;
			Console.WriteLine();
			Console.BackgroundColor = bg;
			Console.ForegroundColor = fg;
		}

		private static void RenderTile(Tiles tile)
		{
			Console.ForegroundColor = TileColors[tile];
			Console.BackgroundColor = TileBackgrounds[tile];
			Console.Write((char) tile);
		}

		private static ConsoleColor GetTileColor(Tiles tile)
		{
			return TileColors[tile];
		}
	}
}
