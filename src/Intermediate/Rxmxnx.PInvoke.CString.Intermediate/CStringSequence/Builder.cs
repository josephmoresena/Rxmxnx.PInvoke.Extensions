namespace Rxmxnx.PInvoke;

public partial class CStringSequence
{
	/// <summary>
	/// A thread-safe builder structure for <see cref="CStringSequence"/> instances.
	/// </summary>
	[DebuggerDisplay("Count = {Count}")]
	[DebuggerTypeProxy(typeof(CStringSequenceDebugView))]
	public readonly partial struct Builder
	{
		/// <summary>
		/// UTF-8 null character span.
		/// </summary>
		private static ReadOnlySpan<Byte> NullChar => "\0"u8;

		/// <summary>
		/// Internal lengths list.
		/// </summary>
		private readonly Value<CStringBuilder> _value;

		/// <inheritdoc cref="Value{TBuilder}.Count"/>
		public Int32 Count => this._value.Count;

		/// <summary>
		/// Parameterless constructor.
		/// </summary>
		public Builder() => this._value = new([], new());

		/// <summary>
		/// Appends the specified UTF-8 text.
		/// </summary>
		/// <param name="value">The UTF-8 text to append.</param>
		/// <returns>The current instance after the append operation has completed.</returns>
		public Builder Append(CString? value)
		{
			if (value is null || value.IsZero)
				this._value.AppendNull();
			else
				this._value.Append(value.AsSpan());
			return this;
		}
		/// <summary>
		/// Appends the UTF-8 representation of the specified string.
		/// </summary>
		/// <param name="value">The string to append.</param>
		/// <returns>The current instance after the append operation has completed.</returns>
		public Builder Append(String? value)
		{
			if (value is null)
				this._value.AppendNull();
			else
				this._value.Append(value);
			return this;
		}
		/// <summary>
		/// Appends the specified UTF-8 text.
		/// </summary>
		/// <param name="value">The UTF-8 text to append.</param>
		/// <returns>The current instance after the append operation has completed.</returns>
		public Builder Append(ReadOnlySpan<Byte> value)
		{
			this._value.Append(value);
			return this;
		}
		/// <summary>
		/// Appends the specified UTF-8 text.
		/// </summary>
		/// <param name="value">The UTF-8 text to append.</param>
		/// <returns>The current instance after the append operation has completed.</returns>
		public Builder Append(ReadOnlySequence<Byte> value)
		{
			this._value.Append(value);
			return this;
		}
		/// <summary>
		/// Appends the UTF-8 representation of <paramref name="value"/> at the specified position.
		/// </summary>
		/// <param name="value">The UTF-16 text to append.</param>
		/// <returns>The current instance after the append operation has completed.</returns>
		public Builder Append(ReadOnlySpan<Char> value)
		{
			this._value.Append(value);
			return this;
		}
		/// <summary>
		/// Appends the UTF-8 unescaped representation of <paramref name="escaped"/>.
		/// </summary>
		/// <param name="escaped">The UTF-8 escaped text to append.</param>
		/// <returns>The current instance after the append operation has completed.</returns>
		public Builder AppendEscaped(ReadOnlySpan<Byte> escaped)
		{
			if (escaped.IsEmpty)
			{
				this._value.Append(ReadOnlySpan<Byte>.Empty);
				return this;
			}

			Byte[]? byteArray = default;
			try
			{
				Span<Byte> bytes = StackAllocationHelper.HasStackBytes(escaped.Length) ?
					stackalloc Byte[escaped.Length] :
					StackAllocationHelper.RentArray(escaped.Length, out byteArray, false);

				escaped.CopyTo(bytes);
				this._value.Append(bytes[..^TextUnescape.Unescape(bytes)]);
				return this;
			}
			finally
			{
				StackAllocationHelper.ReturnArray(byteArray);
			}
		}
		/// <summary>
		/// Appends the UTF-8 unescaped representation of <paramref name="escaped"/>.
		/// </summary>
		/// <param name="escaped">The UTF-8 escaped text to append.</param>
		/// <returns>The current instance after the append operation has completed.</returns>
		public Builder AppendEscaped(ReadOnlySequence<Byte> escaped)
		{
			if (escaped.IsEmpty)
			{
				this._value.Append(ReadOnlySpan<Byte>.Empty);
				return this;
			}

			Byte[]? byteArray = default;
			Int32 utf8Length = (Int32)escaped.Length;
			try
			{
				Span<Byte> bytes = StackAllocationHelper.HasStackBytes(utf8Length) ?
					stackalloc Byte[utf8Length] :
					StackAllocationHelper.RentArray(utf8Length, out byteArray, false);

				escaped.CopyTo(bytes);
				this._value.Append(bytes[..^TextUnescape.Unescape(bytes)]);
				return this;
			}
			finally
			{
				StackAllocationHelper.ReturnArray(byteArray);
			}
		}
		/// <summary>
		/// Inserts the specified UTF-8 text at the specified position.
		/// </summary>
		/// <param name="index">The zero-based index at which item should be inserted.</param>
		/// <param name="value">The UTF-8 text to insert.</param>
		/// <returns>The current instance after the insert operation has completed.</returns>
		public Builder Insert(Int32 index, CString? value)
		{
			if (value is null || value.IsZero)
				this._value.InsertNull(index);
			else
				this._value.Insert(index, value.AsSpan());
			return this;
		}
		/// <summary>
		/// Inserts the UTF-8 representation of the specified string at the specified position.
		/// </summary>
		/// <param name="index">The zero-based index at which item should be inserted.</param>
		/// <param name="value">The string to insert.</param>
		/// <returns>The current instance after the insert operation has completed.</returns>
		public Builder Insert(Int32 index, String? value)
		{
			if (value is null)
				this._value.InsertNull(index);
			else
				this._value.Insert(index, value.AsSpan());
			return this;
		}
		/// <summary>
		/// Inserts the specified UTF-8 text at the specified position.
		/// </summary>
		/// <param name="index">The zero-based index at which item should be inserted.</param>
		/// <param name="value">The UTF-8 text to insert.</param>
		/// <returns>The current instance after the insert operation has completed.</returns>
		public Builder Insert(Int32 index, ReadOnlySpan<Byte> value)
		{
			this._value.Insert(index, value);
			return this;
		}
		/// <summary>
		/// Inserts the UTF-8 representation of <paramref name="value"/> at the specified position.
		/// </summary>
		/// <param name="index">The zero-based index at which item should be inserted.</param>
		/// <param name="value">The UTF-16 text to insert.</param>
		/// <returns>The current instance after the insert operation has completed.</returns>
		public Builder Insert(Int32 index, ReadOnlySpan<Char> value)
		{
			this._value.Insert(index, value);
			return this;
		}
		/// <inheritdoc cref="CStringSequence.Builder.Value{T}.RemoveAt(Int32)"/>
		/// <returns>The current instance after the remove operation has completed.</returns>
		public Builder RemoveAt(Int32 index)
		{
			this._value.RemoveAt(index);
			return this;
		}
		/// <summary>
		/// Removes all items from the current instance.
		/// </summary>
		/// <returns>The current instance after the clear operation has completed.</returns>
		public Builder Clear()
		{
			this._value.Reset();
			return this;
		}
		/// <summary>
		/// Creates a new <see cref="CStringSequence"/> instance using the current builder state.
		/// </summary>
		/// <returns>A new <see cref="CStringSequence"/> instance.</returns>
		public CStringSequence Build()
			=> !this._value.IsEmpty ?
				new(this._value.BuildState(out Int32?[] lengths), lengths) :
				CStringSequence.Empty;

		/// <summary>
		/// Creates an array of <see cref="CString"/> from current instance.
		/// </summary>
#if !PACKAGE
		[ExcludeFromCodeCoverage]
#endif
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal CString[] ToArray() => this._value.ToArray();
	}
}