namespace Rxmxnx.PInvoke;

public partial class CStringSequence
{
	/// <summary>
	/// A stack-only view over the UTF-8 items on a <see cref="CStringSequence"/>, with control over empty entries.
	/// </summary>
	[DebuggerDisplay("Count = {Count}")]
	[DebuggerTypeProxy(typeof(CStringSequenceDebugView))]
	[StructLayout(LayoutKind.Sequential)]
#if NET7_0_OR_GREATER
	[NativeMarshalling(typeof(InputMarshaller))]
#endif
	public readonly ref struct Utf8View
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
		/// Enumeration source sequence.
		/// </summary>
		public CStringSequence? Source => this._instance;
		/// <summary>
		/// Indicates whether current enumeration includes empty items from the source sequence.
		/// </summary>
		public Boolean EmptyItemsIncluded => !this._excludeEmptyItems;
		/// <summary>
		/// Gets the number of elements in the current enumeration.
		/// </summary>
		public Int32 Count
			=> (!this._excludeEmptyItems ? this._instance?.Count : this._instance?.NonEmptyCount).GetValueOrDefault();

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="instance">A <see cref="CStringSequence"/> instance.</param>
		/// <param name="includeEmptyItems">Specifies whether empty items should be included in the enumeration.</param>
		internal Utf8View(CStringSequence? instance, Boolean includeEmptyItems)
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
		/// <inheritdoc/>
		public override Boolean Equals([NotNullWhen(true)] Object? obj) => Object.Equals(obj, this._instance);

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
		/// Determines whether two specified <see cref="Utf8View"/> instances have the same value.
		/// </summary>
		/// <param name="left">The first <see cref="Utf8View"/> to compare, or <see langword="null"/>.</param>
		/// <param name="right">The second <see cref="Utf8View"/> to compare, or <see langword="null"/>.</param>
		/// <returns>
		/// <see langword="true"/> if the value of <paramref name="left"/> is the same as the value
		/// of <paramref name="right"/>; otherwise, <see langword="false"/>.
		/// </returns>
		public static Boolean operator ==(Utf8View left, Utf8View right)
			=> Object.Equals(left._instance, right._instance) && left._excludeEmptyItems == right._excludeEmptyItems;
		/// <summary>
		/// Determines whether two specified <see cref="Utf8View"/> instances have different values.
		/// </summary>
		/// <param name="left">The first <see cref="Utf8View"/> to compare, or <see langword="null"/>.</param>
		/// <param name="right">The second <see cref="Utf8View"/> to compare, or <see langword="null"/>.</param>
		/// <returns>
		/// <see langword="true"/> if the value of <paramref name="left"/> is different from the value
		/// of <paramref name="right"/>; otherwise, <see langword="false"/>.
		/// </returns>
		public static Boolean operator !=(Utf8View left, Utf8View right) => !(left == right);

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