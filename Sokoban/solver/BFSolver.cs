using System.Collections.Generic;
using System.Linq;
using Sokoban.models;
using SokobanModel = Sokoban.models.Sokoban;

namespace Sokoban.solver
{
	public class BfSolver: ISolver
	{
		public virtual History Solve(SokobanModel sokoban)
		{
			var structure = new Dictionary<SokobanModel, KeyValuePair<SokobanModel, List<Direction>>>();
			var visited = new HashSet<SokobanModel>();
			var queue = new Queue<SokobanModel>();
			queue.Enqueue(sokoban);
			var found = false;
			SokobanModel parent = null;

			while (queue.Count > 0)
			{
				parent = queue.Dequeue();
				if (parent.IsSolved()) {
					found = true;
					break;
				}
				
				visited.Add(parent);
				foreach (var pair in ExtractChildren(parent))
				{
					if (!visited.Contains(pair.Key)) {
						structure.Add(pair.Key, new KeyValuePair<SokobanModel, List<Direction>>(parent, pair.Value));
						queue.Enqueue(pair.Key);
					}
				}
			}

			return found ? Unwind(parent, structure) : new History();
		}

		private static Dictionary<Point, Point> ShortestPathsFromPlayer(SokobanModel model)
		{
			var result = new Dictionary<Point, Point>();
			var queue = new Queue<Point>();
			var visited = new HashSet<Point>();
			queue.Enqueue(model.GetPlayerPosition());

			while (queue.Count > 0)
			{
				var parent = queue.Dequeue();
				visited.Add(parent);
				foreach (var neighbor in parent.Neighbors())
				{
					if (model.IsOutOfBounds(neighbor)
					    || !model.IsWalkableTileAt(neighbor)
					    || visited.Contains(neighbor)
					) {
						continue;
					}

					if (!result.ContainsKey(neighbor)) {
						result.Add(neighbor, parent);
					}

					queue.Enqueue(neighbor);
				}
			}

			return result;
		}

		private static bool CanMoveCrate(
			SokobanModel model,
			Point playerWillStand,
			Point crateWillGo,
			Dictionary<Point, Point> walkablePoints
		) {
			if (model.IsOutOfBounds(playerWillStand) || model.IsOutOfBounds(crateWillGo)) {
				return false;
			}

			if (!model.IsWalkableTileAt(playerWillStand) || !model.IsWalkableTileAt(crateWillGo)) {
				return false;
			}

			return model.GetPlayerPosition().Equals(playerWillStand)
			       || walkablePoints.ContainsKey(playerWillStand);
		}

		private static bool IsDeadPosition(SokobanModel model, Point crateWillGo)
		{
			var blockHor = model.IsWallAt(crateWillGo.Left()) || model.IsWallAt(crateWillGo.Right());
			var blockVert = model.IsWallAt(crateWillGo.Up()) || model.IsWallAt(crateWillGo.Down());
			var notAtMark = !model.IsMark(crateWillGo);
			return blockHor && blockVert && notAtMark;
		}

		private static List<Direction> UnwindWalk(IReadOnlyDictionary<Point, Point> walkablePoints, Point playerWillStand)
		{
			var result = new LinkedList<Direction>();
			var key = playerWillStand;

			while (walkablePoints.ContainsKey(key))
			{
				var before = walkablePoints[key];
				result = new LinkedList<Direction>(result.Prepend(before.Extract(key)));
				key = before;
			}

			return result.ToList();
		}

		protected static IEnumerable<KeyValuePair<SokobanModel, List<Direction>>> ExtractChildren(SokobanModel model)
		{
			var walkablePoints = ShortestPathsFromPlayer(model);
			var result = new List<KeyValuePair<SokobanModel, List<Direction>>>();

			foreach (var crate in model.GetCrates())
			{
				var pairs = new [] {
					new KeyValuePair<Point, Point>(crate.Left(), crate.Right()),
					new KeyValuePair<Point, Point>(crate.Right(), crate.Left()),
					new KeyValuePair<Point, Point>(crate.Up(), crate.Down()),
					new KeyValuePair<Point, Point>(crate.Down(), crate.Up()),
				};

				foreach (var pair in pairs)
				{
					if (!CanMoveCrate(model, pair.Key, pair.Value, walkablePoints)) {
						continue;
					}

					if (IsDeadPosition(model, pair.Value)) {
						continue;
					}

					var pathToChild = UnwindWalk(walkablePoints, pair.Key);
					pathToChild.Add(crate.Extract(pair.Value));
					var child = model.Extract(crate, pair.Value);
					result.Add(new KeyValuePair<SokobanModel, List<Direction>>(child, pathToChild));
				}
			}

			return result;
		}

		protected static History Unwind(
			SokobanModel root,
			Dictionary<SokobanModel, KeyValuePair<SokobanModel, List<Direction>>> structure
		) {
			var result = new History();
			var child = root;
			var moves = new LinkedList<List<Move>>();
			while (structure.ContainsKey(child))
			{
				var pair = structure[child];
				var parent = pair.Key;
				var fromParentToChild = pair.Value;
				moves.AddFirst(fromParentToChild.Select(dir => new Move(dir)).ToList());
				child = parent;
			}

			foreach (var move in moves) {
				result.AddRange(move);
			}

			return result;
		}
	}
}
