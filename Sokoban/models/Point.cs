using System;
using System.Collections.Generic;
using System.Linq;

namespace Sokoban.models
{
	public class Point
	{
		public int Col { get; }
		public int Row { get; }

		public Point(int col, int row)
		{
			Col = col;
			Row = row;
		}

		public Point Translate(Point delta)
		{
			return new Point(Col + delta.Col, Row + delta.Row);
		}

		protected bool Equals(Point other)
		{
			return Col == other.Col && Row == other.Row;
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != GetType()) return false;
			return Equals((Point) obj);
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(typeof(Point), Col, Row);
		}

		public List<Point> Neighbors()
		{
			return Move.Directions.Select(dir => Translate(dir.Delta)).ToList();
		}

		public Point Left()
		{
			return Translate(new Point(-1, 0));
		}

		public Point Right()
		{
			return Translate(new Point(1, 0));
		}

		public Point Up()
		{
			return Translate(new Point(0, -1));
		}

		public Point Down()
		{
			return Translate(new Point(0, 1));
		}

		public Direction Extract(Point point)
		{
			var delta = new Point(point.Col - Col, point.Row - Row);

			foreach (var direction in Move.Directions)
			{
				if (direction.Delta.Equals(delta)) {
					return direction;
				}
			}

			throw new ArgumentException($"{this} to {point}");
		}

		public int Distance(Point point)
		{
			return Math.Abs(Row - point.Row) + Math.Abs(Col - point.Col);
		}
	}
}
