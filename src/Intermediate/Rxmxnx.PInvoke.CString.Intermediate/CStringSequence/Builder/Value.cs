#if !NET9_0_OR_GREATER
using Lock = System.Object;
#endif

namespace Rxmxnx.PInvoke;

public partial class CStringSequence
{
	public readonly partial struct Builder
	{
		/// <summary>
		/// Internal <see cref="Builder"/> value type.
		/// </summary>
		/// <param name="lengths">Lengths list.</param>
		/// <param name="charBuffer">Characters buffer.</param>
		private readonly struct Value(List<Int32> lengths, CStringBuilder charBuffer)
		{
			/// <summary>
			/// Internal lengths list.
			/// </summary>
			private readonly List<Int32> _lengths = lengths;
			/// <summary>
			/// Internal characters buffer.
			/// </summary>
			private readonly CStringBuilder _charBuffer = charBuffer;

			/// <summary>
			/// Gets the number of items contained in the builder.
			/// </summary>
			public Int32 Count => this._lengths.Count;

			/// <summary>
			/// Retrieves the <see cref="CStringSequence"/> representation of current instance.
			/// </summary>
			/// <returns>A <see cref="CStringSequence"/> instance.</returns>
			public CStringSequence CreateSequence()
				=> this._lengths.Count != 0 ?
					new(this.BuildState(out Int32[] stateLengths), stateLengths) :
					CStringSequence.Empty;
			/// <summary>
			/// Appends a null UTF-8 text at the current position.
			/// </summary>
			public void AppendNull() => this._lengths.Add(CStringSequence.zeroItemLength);
			/// <summary>
			/// Inserts a null UTF-8 text at <paramref name="index"/>.
			/// </summary>
			/// <param name="index">The zero-based index at which null UTF-8 text should be inserted.</param>
			public void InsertNull(Int32 index) => this._lengths.Insert(index, CStringSequence.zeroItemLength);
			/// <summary>
			/// Appends the specified UTF-8 text span at the current position.
			/// </summary>
			/// <param name="utf8Text">The UTF-8 text to append.</param>
			public void Append(ReadOnlySpan<Byte> utf8Text)
			{
				this._charBuffer.Append(utf8Text);
				this._lengths.Add(utf8Text.Length);
				if (utf8Text.Length > 0)
					this._charBuffer.Append(Builder.NullChar);
			}
			/// <summary>
			/// Appends the specified UTF-8 text sequence at the current position.
			/// </summary>
			/// <param name="utf8Text">The UTF-8 text to append.</param>
			public void Append(ReadOnlySequence<Byte> utf8Text)
			{
				Int32 utf8Length = (Int32)utf8Text.Length;
				this._charBuffer.Append(utf8Text);
				this._lengths.Add(utf8Length);
				if (utf8Length > 0)
					this._charBuffer.Append(Builder.NullChar);
			}
			/// <summary>
			/// Appends the specified UTF-16 text span at the current position.
			/// </summary>
			/// <param name="utf16Text">The UTF-16 text to append.</param>
			/// <param name="utf8Length">The UTF-8 text length to append.</param>
			public void Append(ReadOnlySpan<Char> utf16Text, Int32 utf8Length)
			{
				this._charBuffer.Append(utf16Text);
				this._lengths.Add(utf8Length);
				if (utf8Length > 0)
					this._charBuffer.Append(Builder.NullChar);
			}
			/// <summary>
			/// Inserts a UTF-8 text at <paramref name="index"/>.
			/// </summary>
			/// <param name="index">The zero-based index at which UTF-8 text should be inserted.</param>
			/// <param name="utf8Text">The UTF-8 text to insert.</param>
			public void Insert(Int32 index, ReadOnlySpan<Byte> utf8Text)
			{
				Int32 charIndex = this._lengths.Take(index).Sum(l => l > 0 ? l + 1 : 0);
				this._lengths.Insert(index, utf8Text.Length);
				if (utf8Text.Length <= 0) return;
				this._charBuffer.Insert(charIndex, Builder.NullChar);
				this._charBuffer.Insert(charIndex, utf8Text);
			}
			/// <summary>
			/// Inserts UTF-8 representation of the characters in the specified read-only span into this instance at the
			/// specified UTF-8 unit position.
			/// </summary>
			/// <param name="index">The zero-based index at which UTF-8 text should be inserted.</param>
			/// <param name="utf16Text">The UTF-16 text to insert.</param>
			/// <param name="utf8Length">The UTF-8 text length to append.</param>
			public void Insert(Int32 index, ReadOnlySpan<Char> utf16Text, Int32 utf8Length)
			{
				Int32 charIndex = this._lengths.Take(index).Sum(l => l > 0 ? l + 1 : 0);
				this._lengths.Insert(index, utf8Length);
				if (utf8Length <= 0) return;
				this._charBuffer.Insert(charIndex, Builder.NullChar);
				this._charBuffer.Insert(charIndex, utf16Text);
			}
			/// <summary>
			/// Removes the item at the specified index.
			/// </summary>
			/// <param name="index">The zero-based index of the item to remove.</param>
			public void RemoveAt(Int32 index)
			{
				Int32 length = this._lengths[index];
				this._lengths.RemoveAt(index);
				if (length <= 0) return;

				Int32 charIndex = this._lengths.Take(index).Sum(l => l > 0 ? l + 1 : 0);
				this._charBuffer.Remove(charIndex, length + 1);
			}
			/// <summary>
			/// Removes all items from the current instance.
			/// </summary>
			public void Reset()
			{
				this._charBuffer.Clear();
				this._lengths.Clear();
			}
			/// <summary>
			/// Retrieves the UTF-8 value from current instance.
			/// </summary>
			/// <param name="lengths">Output. Item length span.</param>
			/// <returns>The UTF-8 value.</returns>
			public CString GetValue(out ReadOnlySpan<Int32> lengths)
			{
				Byte[] array = CString.CreateByteArray(this._charBuffer.Length);

				this._charBuffer.CopyTo(0, array);
				lengths = this._lengths.ToArray();
				return CString.Create(array);
			}
			/// <summary>
			/// Retrieves the lock object to concurrent operations.
			/// </summary>
			/// <returns>A <see cref="Lock"/> instance.</returns>
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public Lock GetLock() => this._charBuffer.GetLock();

			/// <summary>
			/// Retrieves the sequence buffer and the lengths array from the current build.
			/// </summary>
			/// <param name="lengths">Output. Lengths array.</param>
			/// <returns>The sequence buffer.</returns>
			private String BuildState(out Int32[] lengths)
			{
				lengths = CStringSequence.CreateIntArray(this._lengths.Count);
#if !NET5_0_OR_GREATER
				Span<Int32>.Enumerator arrEnumerator = lengths.AsSpan().GetEnumerator();
				using List<Int32>.Enumerator lstEnumerator = this._lengths.GetEnumerator();
#elif !NET10_0_OR_GREATER
				Span<Int32>.Enumerator arrEnumerator = lengths.AsSpan().GetEnumerator();
				Span<Int32>.Enumerator lstEnumerator = CollectionsMarshal.AsSpan(this._lengths).GetEnumerator();
#else
				using Span<Int32>.Enumerator arrEnumerator = lengths.AsSpan().GetEnumerator();
				using Span<Int32>.Enumerator lstEnumerator = CollectionsMarshal.AsSpan(this._lengths).GetEnumerator();
#endif
				Int32 totalLength = 0;
				while (arrEnumerator.MoveNext() && lstEnumerator.MoveNext())
				{
					Int32 itemLength = lstEnumerator.Current;
					arrEnumerator.Current = itemLength;
					if (itemLength > 0)
						totalLength += itemLength + 1;
				}
				if (totalLength == 0)
					return CStringSequence.Empty._value;

				Int32 bufferLength = totalLength / sizeof(Char) + totalLength % sizeof(Char);
				return String.Create(bufferLength, this._charBuffer, Value.CopyChars);
			}

			/// <summary>
			/// Copies the content of <paramref name="builder"/> to <paramref name="span"/>.
			/// </summary>
			/// <param name="span">Destination UTF-16 character buffer.</param>
			/// <param name="builder">A <see cref="CStringBuilder"/> instance.</param>
			private static void CopyChars(Span<Char> span, CStringBuilder builder)
			{
				span[^1] = default;
				builder.CopyTo(0, MemoryMarshal.AsBytes(span));
			}
		}
	}
}