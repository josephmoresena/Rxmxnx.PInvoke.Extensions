namespace Rxmxnx.PInvoke;

public partial class CStringSequence
{
	/// <summary>
	/// Provides a stack-only, allocation-free enumerable view over the elements of a <see cref="CStringSequence"/>.
	/// </summary>
	[DebuggerDisplay("Count = {Count}")]
	[DebuggerTypeProxy(typeof(CStringSequenceDebugView))]
	[StructLayout(LayoutKind.Sequential)]
#if NET7_0_OR_GREATER
	[NativeMarshalling(typeof(InputMarshaller))]
#endif
	public readonly ref struct ValueSequence
	{
		/// <summary>
		/// Internal <see cref="CStringSequence"/> instance.
		/// </summary>
		private readonly CStringSequence? _instance;
		/// <summary>
		/// Indicates whether current enumeration is only for non-empty items.
		/// </summary>
		private readonly Boolean _excludeEmptyItems;

		/// <summary>
		/// Internal sequence instance.
		/// </summary>
		public CStringSequence? Sequence => this._instance;
		/// <summary>
		/// Indicates whether current enumeration includes empty items. 
		/// </summary>
		public Boolean IncludeEmptyItems => !this._excludeEmptyItems;
		/// <summary>
		/// Gets the number of elements in the enumeration.
		/// </summary>
		public Int32 Count
			=> (!this._excludeEmptyItems ? this._instance?.Count : this._instance?.NonEmptyCount).GetValueOrDefault();

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="instance">A <see cref="CStringSequence"/> instance.</param>
		/// <param name="includeEmptyItems">Specifies whether empty items should be included in the enumeration.</param>
		internal ValueSequence(CStringSequence? instance, Boolean includeEmptyItems)
		{
			this._instance = instance;
			this._excludeEmptyItems = !includeEmptyItems;
		}

		/// <inheritdoc cref="IEnumerable{T}.GetEnumerator"/>
		public Enumerator GetEnumerator()
			=> this._instance is not null ? new(this._instance, this._excludeEmptyItems) : default;
		/// <inheritdoc/>
		public override Int32 GetHashCode() => this._instance?.GetHashCode() ?? default;
		/// <inheritdoc/>
		public override String? ToString() => this._instance?._value;

		/// <summary>
		/// Creates an array of <see cref="CString"/> from current instance.
		/// </summary>
#if !PACKAGE
		[ExcludeFromCodeCoverage]
#endif
		internal CString[] ToArray()
		{
			if (this._instance is null or { Count: 0, }) return [];
			if (!this._excludeEmptyItems) return this._instance.ToArray();

			CString[] result = new CString[this._instance.NonEmptyCount];
			Span<CString> span = result;
			for (Int32 index = 0; index < this._instance._lengths.Length; index++)
			{
				Int32? length = this._instance._lengths[index];
				if (length is null or 0) continue;
				span[0] = this._instance[index];
				span = span[1..];
			}
			return result;
		}

		/// <summary>
		/// Enumerates the UTF-8 segments within a <see cref="CStringSequence"/>.
		/// </summary>
		public ref struct Enumerator
#if NET9_0_OR_GREATER
		: IEnumerator<ReadOnlySpan<Byte>>
#endif
		{
			/// <summary>
			/// Indicates whether current enumeration include empty items.
			/// </summary>
			private readonly Boolean _excludeEmptyItems;
			/// <summary>
			/// Internal instance.
			/// </summary>
			private readonly CStringSequence? _instance;
			/// <summary>
			/// The current position in the sequence.
			/// </summary>
			private Int32? _index; // Null by default.
			/// <summary>
			/// UTF-8 text buffer.
			/// </summary>
			private ReadOnlySpan<Byte> _buffer;

			/// <summary>
			/// Gets the element in the sequence at the current position of the enumerator.
			/// </summary>
			public ReadOnlySpan<Byte> Current
			{
				get
				{
					Int32 index = this._index ?? -1;
					ReadOnlySpan<Int32?> lengths = this._instance?._lengths;

					ValidationUtilities.ThrowIfInvalidIndexEnumerator(index, lengths.Length);
					Int32? currentLength = lengths[index];
					return currentLength switch
					{
						null => ReadOnlySpan<Byte>.Empty,
						0 => CString.Empty.AsSpan(),
						_ => this._buffer[..currentLength.Value],
					};
				}
			}

			/// <summary>
			/// Constructor.
			/// </summary>
			/// <param name="instance">A <see cref="CStringSequence"/> instance.</param>
			/// <param name="excludeEmptyItems">Indicates whether current enumerator is only for non-empty items.</param>
			internal Enumerator(CStringSequence? instance, Boolean excludeEmptyItems)
			{
				this._instance = instance;
				this._excludeEmptyItems = excludeEmptyItems;
				this.Reset();
			}

#if NET9_0_OR_GREATER
		Object? System.Collections.IEnumerator.Current => this._instance?[this._index.GetValueOrDefault()];
		
		void IDisposable.Dispose() { }
#endif

			/// <summary>
			/// Advances the enumerator to the next element of the enumeration.
			/// </summary>
			/// <returns>
			/// <see langword="true"/> if the enumerator was successfully advanced to the next element;
			/// <see langword="false"/> if the enumerator has passed the end of the enumeration.
			/// </returns>
			public Boolean MoveNext()
			{
				ReadOnlySpan<Int32?> lengths = this.GetLengthSpan(out Int32? currentLength);
				while (lengths.Length >= 0)
				{
					this.Advance(ref lengths, ref currentLength);
					if (currentLength < 0) return false;
					if (!this._excludeEmptyItems || currentLength > 0)
						return currentLength is null or 0 || currentLength < this._buffer.Length;
				}
				return false;
			}
			/// <summary>
			/// Resets the enumerator to the beginning of the enumeration, starting over.
			/// </summary>
			public void Reset()
			{
				this._buffer = MemoryMarshal.AsBytes<Char>(this._instance?._value);
				this._index = null;
			}

			/// <summary>
			/// Retrieves a span containing the lengths of the items.
			/// </summary>
			/// <param name="currentLength">Output. The length of the current item.</param>
			/// <returns>A read-only span containing the item lengths.</returns>
			private ReadOnlySpan<Int32?> GetLengthSpan(out Int32? currentLength)
			{
				ReadOnlySpan<Int32?> lengths = this._instance?._lengths;
				currentLength = default;
				if (lengths.IsEmpty) return lengths;

				if (this._index > 0)
					lengths = lengths[this._index.Value..];
				currentLength = !lengths.IsEmpty ? lengths[0] : -1;
				return lengths;
			}
			/// <summary>
			/// Advances the enumerator to the next item.
			/// </summary>
			/// <param name="lengths">A span containing the item lengths.</param>
			/// <param name="currentLength">Current item length.</param>
			private void Advance(ref ReadOnlySpan<Int32?> lengths, ref Int32? currentLength)
			{
				if (this._index.HasValue && currentLength > 0)
				{
					Int32 offset = currentLength.Value + 1;
					this._buffer = this._buffer[offset..]; // Advances to the next element in the buffer.
				}
				if (!this._index.HasValue)
				{
					// Starts enumerator instance.
					this._index = 0; // Start.
				}
				else
				{
					// Advances to the next element in the lengths.
					this._index++;
					if (this._index > 0 && !lengths.IsEmpty) lengths = lengths[1..];
				}
				currentLength = lengths.IsEmpty ? -1 : lengths[0];
			}
		}
	}
}