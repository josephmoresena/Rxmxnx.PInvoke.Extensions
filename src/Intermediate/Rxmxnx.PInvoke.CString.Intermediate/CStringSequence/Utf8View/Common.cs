namespace Rxmxnx.PInvoke;

public partial class CStringSequence
{
#pragma warning disable CS0282
	public readonly ref partial struct Utf8View
	{
		/// <summary>
		/// Enumerates the UTF-8 segments within a <see cref="CStringSequence"/>.
		/// </summary>
		[Preserve(AllMembers = true, Conditional = true)]
		public ref partial struct Enumerator
		{
			/// <summary>
			/// Indicates whether the current enumeration includes empty items.
			/// </summary>
			private readonly Boolean _excludeEmptyItems;
			/// <summary>
			/// Internal instance.
			/// </summary>
			private readonly CStringSequence? _instance;

			/// <summary>
			/// Indicates whether the current instance is active.
			/// </summary>
			private Boolean _active;

			/// <summary>
			/// Gets the element in the sequence at the current position of the enumerator.
			/// </summary>
			public partial ReadOnlySpan<Byte> Current { get; }

			/// <summary>
			/// Constructor.
			/// </summary>
			/// <param name="instance">A <see cref="CStringSequence"/> instance.</param>
			/// <param name="excludeEmptyItems">Indicates whether the current enumerator is only for non-empty items.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
			[SecuritySafeCritical]
#endif
			internal Enumerator(CStringSequence? instance, Boolean excludeEmptyItems)
			{
				this._excludeEmptyItems = excludeEmptyItems;
				this._instance = instance;
				this.Reset();
			}

			/// <summary>
			/// Advances the enumerator to the next element of the enumeration.
			/// </summary>
			/// <returns>
			/// <see langword="true"/> if the enumerator was successfully advanced to the next element;
			/// <see langword="false"/> if the enumerator has passed the end of the enumeration.
			/// </returns>
			public partial Boolean MoveNext();

			/// <summary>
			/// Resets the enumerator to the beginning of the enumeration, starting over.
			/// </summary>
			public partial void Reset();
			
			/// <summary>
			/// Retrieves the current instance span.
			/// </summary>
			/// <returns>Current <see cref="CStringSequence"/> UTF-8 buffer.</returns>
#if NETFRAMEWORK || NETSTANDARD2_0
			[SecuritySafeCritical]
#endif
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			private ReadOnlySpan<Byte> GetInstanceBuffer() => MemoryMarshal.AsBytes<Char>(this._instance?._value);
		}
#pragma warning restore CS0282
	}
}