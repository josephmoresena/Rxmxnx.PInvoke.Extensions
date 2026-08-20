using System;
#if !NETCOREAPP3_0_OR_GREATER && (NETCOREAPP || NET461_OR_GREATER || WINDOWS_UWP || LEGACY)
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
#endif

namespace Rxmxnx.PInvoke.ApplicationTest
{
	internal sealed class ArrayWrapper<T> : IEnumerableSequence<T>, IMutableWrapper<T[]>
	{
		public Int32 GetSize() => this.Value.Length;
		public T GetItem(Int32 index) => this.Value[index];
#if MONO || NET461_OR_GREATER
		public T[] Value { get; set; } = Array.Empty<T>();
#else
		public T[] Value { get; set; } = new T[0];
#endif
#if !NETCOREAPP3_0_OR_GREATER && (NETCOREAPP || NET461_OR_GREATER || WINDOWS_UWP || LEGACY)
		IEnumerator<T> IEnumerable<T>.GetEnumerator() => this.CreateDefaultEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => this.CreateDefaultEnumerator();
		Object? IStrongBox.Value
		{
			get => this.Value;
			set => this.Value = (T[])value!;
		}
#endif
	}
}