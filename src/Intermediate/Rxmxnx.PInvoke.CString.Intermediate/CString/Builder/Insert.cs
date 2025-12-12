namespace Rxmxnx.PInvoke;

public partial class CString
{
	public sealed partial class Builder
	{
		/// <summary>
		/// Inserts the UTF-8 representation of the specified string into this instance at the specified UTF-8 unit
		/// position.
		/// </summary>
		/// <param name="index">The position in this instance where insertion begins.</param>
		/// <param name="value">The string to insert.</param>
		/// <returns>A reference to this instance after the insert operation has completed.</returns>
		public Builder Insert(Int32 index, String? value)
			=> !String.IsNullOrEmpty(value) ? this.Insert(index, value.AsSpan()) : this;
		/// <summary>
		/// Inserts the specified UTF-8 text into this instance at the specified UTF-8 unit position.
		/// </summary>
		/// <param name="index">The position in this instance where insertion begins.</param>
		/// <param name="value">The UTF-8 text to insert.</param>
		/// <returns>A reference to this instance after the insert operation has completed.</returns>
		public Builder Insert(Int32 index, CString? value)
			=> !CString.IsNullOrEmpty(value) ? this.Insert(index, value.AsSpan()) : this;
		/// <summary>
		/// Inserts UTF-8 representation of the characters in the specified array into this instance at the specified
		/// UTF-8 unit position.
		/// </summary>
		/// <param name="index">The position in this instance where insertion begins.</param>
		/// <param name="value">The character array to insert.</param>
		/// <returns>A reference to this instance after the insert operation has completed.</returns>
		public Builder Insert(Int32 index, Char[]? value)
			=> value is null || value.Length == 0 ? this.Insert(index, value.AsSpan()) : this;
		/// <summary>
		/// Inserts the specified UTF-8 units array into this instance at the specified UTF-8 unit position.
		/// </summary>
		/// <param name="index">The position in this instance where insertion begins.</param>
		/// <param name="value">The array of UTF-8 units to insert.</param>
		/// <returns>A reference to this instance after the insert operation has completed.</returns>
		public Builder Insert(Int32 index, Byte[]? value)
			=> value is null || value.Length == 0 ? this.Insert(index, value.AsSpan()) : this;
		/// <summary>
		/// Inserts UTF-8 representation of the characters in the specified read-only span into this instance at the
		/// specified UTF-8 unit position.
		/// </summary>
		/// <param name="index">The position in this instance where insertion begins.</param>
		/// <param name="value">The read-only span of characters to insert.</param>
		/// <returns>A reference to this instance after the insert operation has completed.</returns>
		public Builder Insert(Int32 index, ReadOnlySpan<Char> value)
		{
			if (value.IsEmpty) return this;
#if NET9_0_OR_GREATER
			using (this._lock.EnterScope())
#else
			lock (this._lock)
#endif
				this._chunk.Insert(index, value);
			return this;
		}
		/// <summary>
		/// Inserts the specified UTF-8 units read-only span into this instance at the specified UTF-8 unit position.
		/// </summary>
		/// <param name="index">The position in this instance where insertion begins.</param>
		/// <param name="value">The read-only span of characters to insert.</param>
		/// <returns>A reference to this instance after the insert operation has completed.</returns>
		public Builder Insert(Int32 index, ReadOnlySpan<Byte> value)
		{
			if (value.IsEmpty) return this;
#if NET9_0_OR_GREATER
			using (this._lock.EnterScope())
#else
			lock (this._lock)
#endif
				this._chunk.Insert(index, value);
			return this;
		}

#if NET8_0_OR_GREATER
		/// <summary>
		/// Inserts the UTF-8 representation of the specified <typeparamref name="T"/> value into
		/// <paramref name="builder"/> at the specified UTF-8 unit position.
		/// </summary>
		/// <typeparam name="T">A <see cref="IUtf8SpanFormattable"/> instance.</typeparam>
		/// <param name="builder">A <see cref="Builder"/> instance.</param>
		/// <param name="index">The position in this instance where insertion begins.</param>
		/// <param name="value">The value to insert.</param>
		/// <returns>A reference to this instance after the insert operation has completed.</returns>
		private static Builder? InsertUtf8<T>(Builder builder, Int32 index, T value) where T : IUtf8SpanFormattable
		{
			Span<Byte> span = stackalloc Byte[CString.stackallocByteThreshold];
			return value.TryFormat(span, out Int32 count, default, default) ?
				builder.Insert(index, span[..count]) :
				default;
		}
#endif
#if NET6_0_OR_GREATER
		/// <summary>
		/// Inserts the UTF-8 representation of the specified <typeparamref name="T"/> value into
		/// <paramref name="builder"/> at the specified UTF-8 unit position.
		/// </summary>
		/// <typeparam name="T">A <see cref="ISpanFormattable"/> instance.</typeparam>
		/// <param name="builder">A <see cref="Builder"/> instance.</param>
		/// <param name="index">The position in this instance where insertion begins.</param>
		/// <param name="value">The value to insert.</param>
		/// <returns>A reference to this instance after the insert operation has completed.</returns>
		private static Builder? InsertUtf16<T>(Builder builder, Int32 index, T value) where T : ISpanFormattable
		{
			Span<Char> span = stackalloc Char[CString.stackallocByteThreshold / 2];
			return value.TryFormat(span, out Int32 count, default, default) ?
				builder.Insert(index, span[..count]) :
				default;
		}
#endif
	}
}