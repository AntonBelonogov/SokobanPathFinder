using System;
using System.Collections.Generic;

namespace Sokoban.models
{
	public class Move
	{
		private readonly Direction dir;

		public static readonly List<Direction> Directions = new List<Direction>() {
			new Direction(ConsoleKey.UpArrow, 0, -1),
			new Direction(ConsoleKey.DownArrow, 0, 1),
			new Direction(ConsoleKey.LeftArrow, -1, 0),
			new Direction(ConsoleKey.RightArrow, 1, 0),
		};

		private static readonly Dictionary<ConsoleKey, string> DirectionsNames = new Dictionary<ConsoleKey, string> {
			{ConsoleKey.UpArrow,    "U"},
			{ConsoleKey.DownArrow,  "D"},
			{ConsoleKey.LeftArrow,  "L"},
			{ConsoleKey.RightArrow, "R"},
		};

		public Move(Direction dir)
		{
			this.dir = dir;
		}

		public static Move Resolve(ConsoleKey key)
		{
			var direction = Directions.Find(dir => dir.Key == key);
			return direction != null ? new Move(direction) :  null;
		}

		public void Perform(Sokoban sokoban)
		{
			sokoban.Move(dir.Delta);
		}

		public override string ToString()
		{
			return DirectionsNames[dir.Key] ?? "";
		}
	}
}
