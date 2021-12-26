using System.Collections.Generic;
using System.Text;

namespace Sokoban.models
{
	public class History: List<Move>
	{
		public override string ToString()
		{
			var sb = new StringBuilder();
			var moves = ConvertAll(move => move.ToString());

			var prevMove = "";
			var count = 0;
			foreach (var move in moves) {
				if (move == prevMove) {
					count++;
					continue;
				}

				if (count > 0) {
					if (count > 1) {
						sb.Append(count);
					}

					sb.Append(prevMove);
					sb.Append(" ");
				}

				count = 1;
				prevMove = move;
			}

			if (count > 1) {
				sb.Append(count);
			}

			sb.Append(prevMove);

			return sb.ToString();
		}
	}
}