using System;
using System.Collections.Generic;
using System.IO;
using Sokoban.controllers;
using Sokoban.controllers.exceptions;
using Sokoban.models;
using Sokoban.solver;
using Sokoban.views;

namespace Sokoban
{
	class Program
	{
		static void Main(string[] args)
		{
			if (args.Length == 0) {
				throw new ArgumentException("Filename should be passed as first argument");
			}

			var fileName = args[0];
			if (!File.Exists(fileName)) {
				throw new ArgumentException($"File not found: {fileName}");
			}

			bool restart;
			do {
				var sokoban = SokobanFactory.FromFilePath(fileName);
				var view = new ConsoleView(sokoban);
				// var controller = new SolverController(sokoban, view, 500, new BfSolver());
				var controller = new SolverController(sokoban, view, 500, new AStarSolver());
				// var controller = new ConsoleController(sokoban, view);

				restart = false;
				try {
					controller.Run();
				} catch (RestartRequiredException) {
					restart = true;
				}
			} while (restart);
		}
	}
}
