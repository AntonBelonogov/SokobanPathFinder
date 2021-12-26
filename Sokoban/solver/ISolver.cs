using Sokoban.models;

namespace Sokoban.solver
{
	public interface ISolver
	{
		public History Solve(models.Sokoban sokoban);
	}
}