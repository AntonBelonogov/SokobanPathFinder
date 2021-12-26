using System.Collections.Generic;
using System.IO;

namespace Sokoban.models
{
	public class SokobanFactory
	{
		private readonly string[] lines;

		public static Sokoban FromFilePath(string filename)
		{
			return new SokobanFactory(File.ReadAllLines(filename)).Build();
		}

		public SokobanFactory(string[] lines)
		{
			this.lines = lines;
		}

		public Sokoban Build()
		{
			var tiles = new Tiles[lines.Length, lines[0].Length];
			Point player = null;
			var crates = new List<Point>();
			var marks = new List<Point>();

			var atRow = 0;
			foreach (var line in lines)
			{
				var atCol = 0;

				foreach (var symbol in line)
				{
					var tile = TilesHelper.FromSymbol(symbol);
					var at = new Point(atCol++, atRow);

					switch (tile)
					{
						case Tiles.Player:
							player = at;
							tiles[at.Row, at.Col] = Tiles.Grass;
							break;

						case Tiles.Crate:
							crates.Add(at);
							tiles[at.Row, at.Col] = Tiles.Grass;
							break;

						case Tiles.CrateOnMark:
							crates.Add(at);
							marks.Add(at);
							tiles[at.Row, at.Col] = Tiles.Crate;
							break;

						case Tiles.Mark:
							marks.Add(at);
							tiles[at.Row, at.Col] = Tiles.Mark;
							break;

						default:
							tiles[at.Row, at.Col] = tile;
							break;
					}
				}

				atRow++;
			}

			return new Sokoban(new Field(tiles), player, crates, marks);
		}
	}
}
