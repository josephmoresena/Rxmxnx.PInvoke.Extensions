using System;

namespace Rxmxnx.PInvoke.ApplicationTest
{
	internal sealed class ArrayWrapper<T> : IEnumerableSequence<T>, IMutableWrapper<T[]>
	{
		public Int32 GetSize() => this.Value.Length;
		public T GetItem(Int32 index) => this.Value[index];
		public T[] Value { get; set; } = Array.Empty<T>();
	}
}