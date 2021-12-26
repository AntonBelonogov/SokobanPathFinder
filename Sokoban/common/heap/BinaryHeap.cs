using System;
using System.Collections.Generic;

namespace Sokoban.common.heap
{
	public class BinaryHeap<T>: IHeap<T> where T: IComparable<T>
	{
		private readonly List<T> heap;

		public BinaryHeap()
		{
			heap = new List<T>();
		}

		public T Top()
		{
			return IsEmpty() ? default : heap[0];
		}

		public void Insert(T value)
		{
			heap.Add(value);
			Promote(Size() - 1);
		}

		public int Size()
		{
			return heap.Count;
		}

		public T Remove()
		{
			if (IsEmpty()) {
				return default;
			}
			var root = At(0);
			var last = At(Size() - 1);
			heap[0] = last;
			heap.RemoveAt(Size() - 1);
			EnsureConsistency(0);
			return root;
		}

		public bool IsEmpty()
		{
			return Size() == 0;
		}

		public void Elevate(int index, T value)
		{
			if (OutOfBounds(index)) {
				throw new ArgumentException($"Index is out of bounds: {index}");
			}

			var current = heap[index];
			if (GreaterOrEqual(current, value)) {
				throw new ArgumentException("New value is less then old");
			}

			heap[index] = value;
			Promote(index);
		}

		private bool OutOfBounds(int index)
		{
			return index < 0 || index >= Size();
		}

		private bool InBounds(int index)
		{
			return !OutOfBounds(index);
		}

		private static int Parent(int i)
		{
			return ((i + 1) >> 1) - 1;
		}

		private static int Left(int i)
		{
			return ((i + 1) << 1) - 1;
		}

		private static int Right(int i)
		{
			return (i + 1) << 1;
		}

		private void Promote(int index)
		{
			var i = index;
			while (i > 0 && (Less(At(Parent(i)), At(i))))
			{
				Swap(i, Parent(i));
				i = Parent(i);
			}
		}

		private T At(int index)
		{
			return heap[index];
		}

		private void Swap(int i1, int i2)
		{
			var tmp = heap[i1];
			heap[i1] = heap[i2];
			heap[i2] = tmp;
		}

		private void EnsureConsistency(int badIndex)
		{
			while (true)
			{
				var left = Left(badIndex);
				int greatest;
				if (InBounds(left) && Greater(At(left), At(badIndex))) {
					greatest = left;
				} else {
					greatest = badIndex;
				}

				var right = Right(badIndex);
				if (InBounds(right) && Greater(At(right), At(greatest))) {
					greatest = right;
				}

				if (greatest != badIndex) {
					Swap(badIndex, greatest);
					badIndex = greatest;
					continue;
				}

				break;
			}
		}

		private static bool Less(T first, T second)
		{
			return Compare(first, second) < 0;
		}

		private static bool Greater(T first, T second)
		{
			return Compare(first, second) > 0;
		}

		private static bool GreaterOrEqual(T first, T second)
		{
			return Compare(first, second) >= 0;
		}

		private static int Compare(T first, T second)
		{
			return first.CompareTo(second);
		}
	}
}
