using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Sokoban.models
{
	public class Sokoban
	{
		private readonly Field field;
		private Point player;
		private readonly List<Point> crates;
		private readonly List<Point> marks;

		public int MovesCount { get; private set; } = 0;

		public Sokoban(Field field, Point player, List<Point> crates, List<Point> marks)
		{
			this.field = field;
			this.player = player;
			this.crates = crates;
			this.marks = marks;
		}

		public int FieldHeight()
		{
			return field.Height;
		}

		public int FieldWidth()
		{
			return field.Width;
		}

		public Tiles GetTileAt(Point point)
		{
			
			if (player.Equals(point)) {
				return Tiles.Player;
			}

			if (crates.Contains(point)) {
				return Tiles.Crate;
			}

			return field.GetTileAt(point);
		}

		public bool IsMark(Point at)
		{
			return marks.Contains(at);
		}

		public bool IsSolved()
		{
			return crates.All(marks.Contains);
		}

		private bool CanMoveTo(Point delta)
		{
			var translated = player.Translate(delta);
			if (field.IsOutOfBounds(translated)) {
				return false;
			}

			var moveTo = GetTileAt(translated);
			if (moveTo.IsWalkable()) {
				return true;
			}

			if (moveTo == Tiles.Crate) {
				return GetTileAt(translated.Translate(delta)).IsWalkable();
			}

			return false;
		}

		public void Move(Point delta)
		{
			MovesCount++;
			if (!CanMoveTo(delta)) {
				return;
			}

			var translated = player.Translate(delta);
			var tileTo = GetTileAt(translated);
			if (tileTo.IsWalkable()) {
				player = translated;
				return;
			}

			if (tileTo != Tiles.Crate) {
				return;
			}

			var crateTo = translated.Translate(delta);
			if (GetTileAt(crateTo).IsWalkable()) {
				player = translated;
				MoveCrate(translated, crateTo);
			}
		}

		private void MoveCrate(Point from, Point to)
		{
			crates.Remove(from);
			crates.Add(to);
		}

		public Point GetPlayerPosition()
		{
			return player;
		}

		public bool IsOutOfBounds(Point point)
		{
			return field.IsOutOfBounds(point);
		}

		public bool IsWalkableTileAt(Point point)
		{
			return GetTileAt(point).IsWalkable();
		}

		public bool IsWallAt(Point point)
		{
			return field.GetTileAt(point) == Tiles.Wall;
		}

		public IEnumerable<Point> GetCrates()
		{
			return crates;
		}

		public Sokoban Extract(Point crateBefore, Point crateAfter)
		{
			var newCrates = new List<Point>(crates);
			newCrates.Remove(crateBefore);
			newCrates.Add(crateAfter);
			return new Sokoban(field, crateBefore, newCrates, marks);
		}

		public List<Point> GetMarks()
		{
			return marks;
		}

		public override bool Equals(object? obj)
		{
			if (!(obj is Sokoban sokoban)) {
				return false;
			}

			var otherCrates = sokoban.crates;
			if (crates.Count != otherCrates.Count) return false;
			for (var i = 0; i < crates.Count; i++) {
				if (!crates[i].Equals(otherCrates[i])) {
					return false;
				}
			}

			return true;
		}

		public override int GetHashCode()
		{
			int result = 0;
			foreach (var val in crates.Select((crate) => HashCode.Combine(crate)))
			{
				result ^= val;
			}
			return result;
		}
	}
}
