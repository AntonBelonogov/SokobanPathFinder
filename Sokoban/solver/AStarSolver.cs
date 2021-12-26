using System.Collections.Generic;
using System.Linq;
using Sokoban.common.heap;
using Sokoban.models;
using SokobanModel = Sokoban.models.Sokoban;

namespace Sokoban.solver
{
	public class AStarSolver: BfSolver
	{
		public override History Solve(SokobanModel sokoban)
		{
			var parents = new Dictionary<SokobanModel, KeyValuePair<SokobanModel, List<Direction>>>();
			var visited = new HashSet<SokobanModel>();
			var discovered = new BinaryHeap<Entry<SokobanModel>>();
			var gStore = new Dictionary<SokobanModel, int>();
			discovered.Insert(Entry<SokobanModel>.Of(0, sokoban));
			gStore.Add(sokoban, 0);
			var found = false;
			Entry<SokobanModel> last = null;

			while (!discovered.IsEmpty())
			{
				last = discovered.Remove();
				var parent = last.Value;
				if (parent.IsSolved()) {
					found = true;
					break;
				}

				visited.Add(parent);
				var gOfParent = gStore.GetValueOrDefault(parent, int.MaxValue);
				foreach (var (child, path) in ExtractChildren(parent))
				{
					var testGScore = gOfParent + path.Count;
					var childGScore = gStore.GetValueOrDefault(child, int.MaxValue);
					if (testGScore >= childGScore) {
						continue;
					}

					gStore[child] = testGScore;
					parents[child] = new KeyValuePair<SokobanModel, List<Direction>>(parent, path);
					if (visited.Contains(child)) {
						continue;
					}

					var score = childGScore + H(child);
					discovered.Insert(Entry<SokobanModel>.Of(score, child));
				}
			}

			return found ? Unwind(last.Value, parents) : new History();
		}

		private static int H(SokobanModel model)
		{
			var result = 0;
			foreach (var crate in model.GetCrates()) {
				result += model.GetMarks().Select(point => crate.Distance(point)).Min();
			}
			return result;
		}
	}
}
