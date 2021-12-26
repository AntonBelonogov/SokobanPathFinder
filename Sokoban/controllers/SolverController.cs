using System.Threading;
using Sokoban.solver;
using Sokoban.views;

namespace Sokoban.controllers
{
	public class SolverController: IController
	{
		private readonly models.Sokoban model;
		private readonly IView view;
		private readonly int speed;
		private readonly ISolver solver;

		public SolverController(models.Sokoban model, IView view, int speed, ISolver solver)
		{
			this.model = model;
			this.view = view;
			this.speed = speed;
			this.solver = solver;
		}


		public void Run()
		{
			var moves = solver.Solve(model);
			if (moves.Count == 0) {
				view.WriteLn("Solution not found");
				return;
			}

			view.Render();
			foreach (var move in moves)
			{
				move.Perform(model);
				view.Render();
				Sleep();
			}

			view.WriteLn($"Done: {moves}");
		}

		private void Sleep()
		{
			Thread.Sleep(speed);
		}
	}
}