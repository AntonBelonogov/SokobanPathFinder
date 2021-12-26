using System;
using System.Collections.Generic;

namespace Sokoban.models
{
	public enum Tiles
	{
		Wall        = '#',
		Grass       = '.',
		Crate       = 'O',
		Mark        = 'X',
		CrateOnMark = 'G',
		Player      = '@',
	}

	public static class TilesHelper
	{
		private static readonly List<Tiles> WalkableTiles = new List<Tiles> {
			Tiles.Grass,
			Tiles.Mark,
			Tiles.Player,
		};

		public static Tiles FromSymbol(char symbol)
		{
			foreach (Tiles tile in typeof(Tiles).GetEnumValues())
			{
				if (symbol == (char) tile) {
					return tile;
				}
			}

			throw new ArgumentException($"Unknown symbol: {symbol}");
		}

		public static bool IsWalkable(this Tiles t)
		{
			return WalkableTiles.Contains(t);
		}
	}
}
