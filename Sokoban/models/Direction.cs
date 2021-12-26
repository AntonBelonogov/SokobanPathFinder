using System;

namespace Sokoban.models
{
	public class Direction
	{
		public readonly ConsoleKey Key;
		public readonly Point Delta;

		public Direction(ConsoleKey key, int deltaX, int deltaY)
		{
			Key = key;
			Delta = new Point(deltaX, deltaY);
		}
	}
}
