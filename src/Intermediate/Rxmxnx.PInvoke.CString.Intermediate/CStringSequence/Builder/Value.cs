#if !NET9_0_OR_GREATER
using IUtf8Buffer = Rxmxnx.PInvoke.CStringBuilder.IUnsafe;

#else
using IUtf8Buffer = Rxmxnx.PInvoke.CStringBuilder.IUnsafe<System.Threading.Lock>;
#endif

namespace Rxmxnx.PInvoke;

public partial class CStringSequence
{
	public readonly partial struct Builder
	{
		/// <summary>
		/// Internal <see cref="Builder"/> value type.
		/// </summary>
		/// <typeparam name="T">A <see cref="CStringBuilder.IUnsafe"/> type.</typeparam>
		/// <param name="lengths">Lengths list.</param>
		/// <param name="charBuffer">Characters buffer.</param>
		private readonly struct Value<T>(List<Int32?> lengths, T charBuffer) where T : IUtf8Buffer
		{
			/// <summary>
			/// Internal lengths list.
			/// </summary>
			private readonly List<Int32?> _lengths = lengths;
			/// <summary>
			/// Internal characters buffer.
			/// </summary>
			private readonly T _charBuffer = charBuffer;

			/// <summary>
			/// Indicates whether the current building is empty.
			/// </summary>
			public Boolean IsEmpty
			{
				get
				{
#if NET9_0_OR_GREATER
					using (this._charBuffer.Lock.EnterScope())
#else
					lock (this._charBuffer.Lock)
#endif
						return this._lengths.Count == 0;
				}
			}
			/// <summary>
			/// Gets the number of items contained in the builder.
			/// </summary>
			public Int32 Count
			{
				get
				{
#if NET9_0_OR_GREATER
					using (this._charBuffer.Lock.EnterScope())
#else
					lock (this._charBuffer.Lock)
#endif
						return this._lengths.Count;
				}
			}

			/// <summary>
			/// Retrieves the sequence buffer and the lengths array from the current build.
			/// </summary>
			/// <param name="lengths">Output. Lengths array.</param>
			/// <returns>The sequence buffer.</returns>
			public String BuildState(out Int32?[] lengths)
			{
#if NET9_0_OR_GREATER
				using (this._charBuffer.Lock.EnterScope())
#else
				lock (this._charBuffer.Lock)
#endif
				{
#if !NET5_0_OR_GREATER
					lengths = new Int32?[this._lengths.Count];

					Span<Int32?>.Enumerator arrEnumerator = lengths.AsSpan().GetEnumerator();
					using List<Int32?>.Enumerator lstEnumerator = this._lengths.GetEnumerator();
#else
					lengths = GC.AllocateUninitializedArray<Int32?>(this._lengths.Count);

#if !NET10_0_OR_GREATER
					Span<Int32?>.Enumerator arrEnumerator = lengths.AsSpan().GetEnumerator();
					Span<Int32?>.Enumerator lstEnumerator = CollectionsMarshal.AsSpan(this._lengths).GetEnumerator();
#else
					using Span<Int32?>.Enumerator arrEnumerator = lengths.AsSpan().GetEnumerator();
					using Span<Int32?>.Enumerator lstEnumerator =
						CollectionsMarshal.AsSpan(this._lengths).GetEnumerator();
#endif
#endif
					Int32 totalLength = 0;
					while (arrEnumerator.MoveNext() && lstEnumerator.MoveNext())
					{
						Int32 itemLength = lstEnumerator.Current.GetValueOrDefault();
						arrEnumerator.Current = lstEnumerator.Current;
						if (itemLength > 0)
							totalLength += itemLength + 1;
					}
					if (totalLength == 0)
						return CStringSequence.Empty._value;

					Int32 bufferLength = totalLength / sizeof(Char) + totalLength % sizeof(Char);
					return String.Create(bufferLength, this._charBuffer, Value<T>.CopyChars);
				}
			}

			/// <summary>
			/// Appends a null UTF-8 text at the current position.
			/// </summary>
			public void AppendNull()
			{
#if NET9_0_OR_GREATER
				using (this._charBuffer.Lock.EnterScope())
#else
				lock (this._charBuffer.Lock)
#endif
					this._lengths.Add(null);
			}
			/// <summary>
			/// Inserts a null UTF-8 text at <paramref name="index"/>.
			/// </summary>
			/// <param name="index">The zero-based index at which null UTF-8 text should be inserted.</param>
			public void InsertNull(Int32 index)
			{
#if NET9_0_OR_GREATER
				using (this._charBuffer.Lock.EnterScope())
#else
				lock (this._charBuffer.Lock)
#endif
					this._lengths.Insert(index, null);
			}
			/// <summary>
			/// Appends the specified UTF-8 text span at the current position.
			/// </summary>
			/// <param name="utf8Text">The UTF-8 text to append.</param>
			public void Append(ReadOnlySpan<Byte> utf8Text)
			{
#if NET9_0_OR_GREATER
				using (this._charBuffer.Lock.EnterScope())
#else
				lock (this._charBuffer.Lock)
#endif
				{
					this._charBuffer.Append(utf8Text);
					this._lengths.Add(utf8Text.Length);
					if (utf8Text.Length > 0)
						this._charBuffer.Append(Builder.NullChar);
				}
			}
			/// <summary>
			/// Appends the specified UTF-8 text sequence at the current position.
			/// </summary>
			/// <param name="utf8Text">The UTF-8 text to append.</param>
			public void Append(ReadOnlySequence<Byte> utf8Text)
			{
				Int32 utf8Length = (Int32)utf8Text.Length;
#if NET9_0_OR_GREATER
				using (this._charBuffer.Lock.EnterScope())
#else
				lock (this._charBuffer.Lock)
#endif
				{
					this._charBuffer.Append(utf8Text);
					this._lengths.Add(utf8Length);
					if (utf8Length > 0)
						this._charBuffer.Append(Builder.NullChar);
				}
			}
			/// <summary>
			/// Appends the specified UTF-16 text span at the current position.
			/// </summary>
			/// <param name="utf16Text">The UTF-16 text to append.</param>
			public void Append(ReadOnlySpan<Char> utf16Text)
			{
				Int32 utf8Length = Encoding.UTF8.GetByteCount(utf16Text);
#if NET9_0_OR_GREATER
				using (this._charBuffer.Lock.EnterScope())
#else
				lock (this._charBuffer.Lock)
#endif
				{
					this._charBuffer.Append(utf16Text);
					this._lengths.Add(utf8Length);
					if (utf8Length > 0)
						this._charBuffer.Append(Builder.NullChar);
				}
			}

			/// <summary>
			/// Inserts a UTF-8 text at <paramref name="index"/>.
			/// </summary>
			/// <param name="index">The zero-based index at which UTF-8 text should be inserted.</param>
			/// <param name="utf8Text">The UTF-8 text to insert.</param>
			public void Insert(Int32 index, ReadOnlySpan<Byte> utf8Text)
			{
#if NET9_0_OR_GREATER
				using (this._charBuffer.Lock.EnterScope())
#else
				lock (this._charBuffer.Lock)
#endif
				{
					Int32 charIndex = this._lengths.Take(index).Sum(l => l is > 0 ? l.Value + 1 : 0);
					this._lengths.Insert(index, utf8Text.Length);
					if (utf8Text.Length <= 0) return;
					this._charBuffer.Insert(charIndex, Builder.NullChar);
					this._charBuffer.Insert(charIndex, utf8Text);
				}
			}
			/// <summary>
			/// Inserts UTF-8 representation of the characters in the specified read-only span into this instance at the
			/// specified UTF-8 unit position.
			/// </summary>
			/// <param name="index">The zero-based index at which UTF-8 text should be inserted.</param>
			/// <param name="utf16Text">The UTF-16 text to insert.</param>
			public void Insert(Int32 index, ReadOnlySpan<Char> utf16Text)
			{
				Int32 utf8Length = Encoding.UTF8.GetByteCount(utf16Text);
#if NET9_0_OR_GREATER
				using (this._charBuffer.Lock.EnterScope())
#else
				lock (this._charBuffer.Lock)
#endif
				{
					Int32 charIndex = this._lengths.Take(index).Sum(l => l is > 0 ? l.Value + 1 : 0);
					this._lengths.Insert(index, utf8Length);
					if (utf8Length <= 0) return;
					this._charBuffer.Insert(charIndex, Builder.NullChar);
					this._charBuffer.Insert(charIndex, utf16Text);
				}
			}
			/// <summary>
			/// Removes the item at the specified index.
			/// </summary>
			/// <param name="index">The zero-based index of the item to remove.</param>
			public void RemoveAt(Int32 index)
			{
#if NET9_0_OR_GREATER
				using (this._charBuffer.Lock.EnterScope())
#else
				lock (this._charBuffer.Lock)
#endif
				{
					Int32? length = this._lengths[index];
					this._lengths.RemoveAt(index);
					if (length is not > 0) return;

					Int32 charIndex = this._lengths.Take(index).Sum(l => l is > 0 ? l.Value + 1 : 0);
					this._charBuffer.Remove(charIndex, length.Value + 1);
				}
			}
			/// <summary>
			/// Removes all items from the current instance.
			/// </summary>
			public void Reset()
			{
#if NET9_0_OR_GREATER
				using (this._charBuffer.Lock.EnterScope())
#else
				lock (this._charBuffer.Lock)
#endif
				{
					this._charBuffer.Clear();
					this._lengths.Clear();
				}
			}
			/// <summary>
			/// Creates an array of <see cref="CString"/> from current instance.
			/// </summary>
#if !PACKAGE
			[ExcludeFromCodeCoverage]
#endif
			public CString[] ToArray()
			{
				ReadOnlySpan<Int32?> lengthsSpan;
				CString value;
#if NET9_0_OR_GREATER
				using (this._charBuffer.Lock.EnterScope())
#else
				lock (this._charBuffer.Lock)
#endif
				{
					Byte[] array = CString.CreateByteArray(this._charBuffer.Count);

					this._charBuffer.CopyTo(array);
					lengthsSpan = this._lengths.ToArray();
					value = CString.Create(array);
				}

				CString[] result = new CString[lengthsSpan.Length];
				Int32 offset = 0;
				for (Int32 i = 0; i < lengthsSpan.Length; i++)
				{
					if (lengthsSpan[i].GetValueOrDefault() == 0)
					{
						result[i] = lengthsSpan[i].HasValue ? CString.Empty : CString.Zero;
						continue;
					}
					result[i] = value.Slice(offset, lengthsSpan[i].GetValueOrDefault());
					offset += result[i].Length + 1;
				}

				return result;
			}

			/// <summary>
			/// Copies the content of <paramref name="builder"/> to <paramref name="span"/>.
			/// </summary>
			/// <param name="span">Destination UTF-16 character buffer.</param>
			/// <param name="builder">A <see cref="CStringBuilder.IUnsafe"/> instance.</param>
			private static void CopyChars(Span<Char> span, T builder)
			{
				span[^1] = default;
				builder.CopyTo(MemoryMarshal.AsBytes(span));
			}
		}
	}
}