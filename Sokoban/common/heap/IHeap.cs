using System;

namespace Sokoban.common.heap
{
	public interface IHeap<T>
	{
		T Top();

		void Insert(T value);

		int Size();

		T Remove();

		bool IsEmpty();

		void Elevate(int index, T value);
	}
}
