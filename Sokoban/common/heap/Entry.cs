using System;
using System.Collections.Generic;

namespace Sokoban.common.heap
{
	public class Entry<T>: IComparable<Entry<T>>
	{
		public readonly int Weight;
		public readonly T Value;

		public static Entry<T> Of(int weight, T value)
		{
			return new Entry<T>(weight, value);
		}

		public Entry(int weight, T value)
		{
			Weight = weight;
			Value = value;
		}

		public int CompareTo(Entry<T> other)
		{
			if (ReferenceEquals(this, other)) return 0;
			if (ReferenceEquals(null, other)) return 1;
			return Weight.CompareTo(other.Weight);
		}
	}
}
