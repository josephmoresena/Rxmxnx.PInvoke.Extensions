#if NET9_0_OR_GREATER
using IEnumerator = System.Collections.IEnumerator;
#endif

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
#if NET9_0_OR_GREATER
		: IEquatable<Utf8View>
#endif
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
		// ReSharper disable once ConvertToAutoPropertyWhenPossible
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
#if NET9_0_OR_GREATER
		/// <inheritdoc/>
#if !PACKAGE
		[ExcludeFromCodeCoverage]
#endif
		public Boolean Equals(Utf8View other) => this == other;
#endif
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
			ref CString item = ref MemoryMarshal.GetReference(result.AsSpan());
			for (Int32 index = 0; index < this._instance._lengths.Length; index++)
			{
				Int32 length = this._instance._lengths[index];
				if (length <= 0) continue;
				item = this._instance[index];
				item = ref Unsafe.Add(ref item, 1);
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
			/// Remaining lengths.
			/// </summary>
			private ReadOnlySpan<Int32> _remaining;
			/// <summary>
			/// Remaining UTF-8 buffer.
			/// </summary>
			private ReadOnlySpan<Byte> _buffer;
			/// <summary>
			/// Current item length.
			/// </summary>
			private Int32 _currentLength;

			/// <summary>
			/// Gets the element in the sequence at the current position of the enumerator.
			/// </summary>
			public ReadOnlySpan<Byte> Current
			{
				get
				{
					ValidationUtilities.ThrowIfInvalidEnumerator(this._instance is null, this._currentLength == -1,
					                                             this._remaining.IsEmpty);
					return this._currentLength switch
					{
						0 => CString.Empty.AsSpan(),
						> 0 => this._buffer[..this._currentLength],
						_ => ReadOnlySpan<Byte>.Empty,
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
			public Boolean MoveNext()
			{
				if (this._currentLength > 0)
					this._buffer = this._buffer[(this._currentLength + 1)..];

				while (!this._remaining.IsEmpty)
				{
					this._currentLength = this._remaining[0];
					this._remaining = this._remaining[1..];

					if (!this._excludeEmptyItems || this._currentLength > 0) return true;
				}

				this._currentLength = -1;
				return false;
			}
			/// <summary>
			/// Resets the enumerator to the beginning of the enumeration, starting over.
			/// </summary>
			public void Reset()
			{
				this._remaining = this._instance?._lengths;
				this._buffer = MemoryMarshal.AsBytes<Char>(this._instance?._value);
				this._currentLength = -1;
			}
#if NET9_0_OR_GREATER
			Object? IEnumerator.Current => null;
			void IDisposable.Dispose() { }
#endif
		}
	}
}