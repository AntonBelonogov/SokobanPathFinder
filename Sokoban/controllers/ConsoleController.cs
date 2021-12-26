using System;
using Sokoban.controllers.exceptions;
using Sokoban.models;
using Sokoban.views;
using SokobanModel = Sokoban.models.Sokoban;

namespace Sokoban.controllers
{
	public class ConsoleController: IController
	{
		private readonly SokobanModel sokoban;
		private readonly ConsoleView view;

		public ConsoleController(SokobanModel sokoban, ConsoleView view)
		{
			this.sokoban = sokoban;
			this.view = view;
		}

		public void Run()
		{
			view.WriteLn("Sokoban Starts");
			view.Render();

			var history = new History();

			var symbol = ConsoleKey.Spacebar;
			while (symbol != ConsoleKey.Q)
			{
				symbol = Console.ReadKey(true).Key;

				if (symbol == ConsoleKey.R) {
					throw new RestartRequiredException();
				}

				var move = Move.Resolve(symbol);
				if (move == null) {
					continue;
				}

				history.Add(move);
				move.Perform(sokoban);
				view.Render();

				if (sokoban.IsSolved()) {
					view.WriteLn("YOU WIN");
					break;
				}
			}

			view.WriteLn($"Your moves: {history}");
		}
	}
}
