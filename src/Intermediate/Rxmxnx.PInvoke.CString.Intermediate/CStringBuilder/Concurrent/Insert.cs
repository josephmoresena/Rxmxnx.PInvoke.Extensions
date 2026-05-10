namespace Rxmxnx.PInvoke;

public partial class CStringBuilder
{
	private readonly partial struct Concurrent
	{
		/// <summary>
		/// Inserts UTF-8 representation of the characters in the specified read-only span into this instance at the
		/// specified UTF-8 unit position.
		/// </summary>
		/// <param name="index">The position in this instance where insertion begins.</param>
		/// <param name="value">The read-only span of characters to insert.</param>
		/// <returns>A reference to this instance after the insert operation has completed.</returns>
		public CStringBuilder Insert(Int32 index, ReadOnlySpan<Char> value)
		{
			if (value.IsEmpty) return builder;
#if NET9_0_OR_GREATER
			using (lockObj.EnterScope())
#else
			lock (lockObj)
#endif
				builder._chunk.Insert(index, value);
			return builder;
		}
		/// <summary>
		/// Inserts the specified UTF-8 units read-only span into this instance at the specified UTF-8 unit position.
		/// </summary>
		/// <param name="index">The position in this instance where insertion begins.</param>
		/// <param name="value">The read-only span of characters to insert.</param>
		/// <returns>A reference to this instance after the insert operation has completed.</returns>
		public CStringBuilder Insert(Int32 index, ReadOnlySpan<Byte> value)
		{
			if (value.IsEmpty) return builder;
#if NET9_0_OR_GREATER
			using (lockObj.EnterScope())
#else
			lock (lockObj)
#endif
				builder._chunk.Insert(index, value);
			return builder;
		}
		/// <summary>
		/// Inserts the UTF-8 representation of the specified Boolean value into this instance at the specified UTF-8
		/// unit position.
		/// </summary>
		/// <param name="index">The position in this instance where insertion begins.</param>
		/// <param name="value">The value to insert.</param>
		/// <returns>A reference to this instance after the insert operation has completed.</returns>
		public CStringBuilder Insert(Int32 index, Boolean value)
			=> this.Insert(index, value.ToString(CultureInfo.CurrentCulture));
		/// <summary>
		/// Inserts the UTF-8 representation of the specified character into this instance at the specified
		/// UTF-8 unit position.
		/// </summary>
		/// <param name="index">The position in this instance where insertion begins.</param>
		/// <param name="value">The value to insert.</param>
		/// <returns>A reference to this instance after the insert operation has completed.</returns>
		public CStringBuilder Insert(Int32 index, Char value)
		{
#if NET8_0_OR_GREATER
			if (Concurrent.InsertUtf8(this, index, value) is { } result8)
				return result8;
#endif
#if NET6_0_OR_GREATER
			if (Concurrent.InsertUtf16(this, index, value) is { } result16)
				return result16;
#endif
			return this.Insert(index, value.ToString(CultureInfo.CurrentCulture));
		}
		/// <summary>
		/// Inserts the UTF-8 representation of the specified decimal number into this instance at the specified UTF-8
		/// unit position.
		/// </summary>
		/// <param name="index">The position in this instance where insertion begins.</param>
		/// <param name="value">The value to insert.</param>
		/// <returns>A reference to this instance after the insert operation has completed.</returns>
		public CStringBuilder Insert(Int32 index, Decimal value)
		{
#if NET8_0_OR_GREATER
			if (Concurrent.InsertUtf8(this, index, value) is { } result8)
				return result8;
#endif
#if NET6_0_OR_GREATER
			if (Concurrent.InsertUtf16(this, index, value) is { } result16)
				return result16;
#endif
			return this.Insert(index, value.ToString(CultureInfo.CurrentCulture));
		}
		/// <summary>
		/// Inserts the UTF-8 representation of the specified double-precision floating-point number into this instance
		/// at the specified UTF-8 unit position.
		/// </summary>
		/// <param name="index">The position in this instance where insertion begins.</param>
		/// <param name="value">The value to insert.</param>
		/// <returns>A reference to this instance after the insert operation has completed.</returns>
		public CStringBuilder Insert(Int32 index, Double value)
		{
#if NET8_0_OR_GREATER
			if (Concurrent.InsertUtf8(this, index, value) is { } result8)
				return result8;
#endif
#if NET6_0_OR_GREATER
			if (Concurrent.InsertUtf16(this, index, value) is { } result16)
				return result16;
#endif
			return this.Insert(index, value.ToString(CultureInfo.CurrentCulture));
		}
		/// <summary>
		/// Inserts the UTF-8 representation of the specified 16-bit signed integer into this instance at the specified
		/// UTF-8 unit position.
		/// </summary>
		/// <param name="index">The position in this instance where insertion begins.</param>
		/// <param name="value">The value to insert.</param>
		/// <returns>A reference to this instance after the insert operation has completed.</returns>
		public CStringBuilder Insert(Int32 index, Int16 value)
		{
#if NET8_0_OR_GREATER
			if (Concurrent.InsertUtf8(this, index, value) is { } result8)
				return result8;
#endif
#if NET6_0_OR_GREATER
			if (Concurrent.InsertUtf16(this, index, value) is { } result16)
				return result16;
#endif
			return this.Insert(index, value.ToString(CultureInfo.CurrentCulture));
		}
		/// <summary>
		/// Inserts the UTF-8 representation of the specified 32-bit signed integer into this instance at the specified
		/// UTF-8 unit position.
		/// </summary>
		/// <param name="index">The position in this instance where insertion begins.</param>
		/// <param name="value">The value to insert.</param>
		/// <returns>A reference to this instance after the insert operation has completed.</returns>
		public CStringBuilder Insert(Int32 index, Int32 value)
		{
#if NET8_0_OR_GREATER
			if (Concurrent.InsertUtf8(this, index, value) is { } result8)
				return result8;
#endif
#if NET6_0_OR_GREATER
			if (Concurrent.InsertUtf16(this, index, value) is { } result16)
				return result16;
#endif
			return this.Insert(index, value.ToString(CultureInfo.CurrentCulture));
		}
		/// <summary>
		/// Inserts the UTF-8 representation of the specified 64-bit signed integer into this instance at the specified
		/// UTF-8 unit position.
		/// </summary>
		/// <param name="index">The position in this instance where insertion begins.</param>
		/// <param name="value">The value to insert.</param>
		/// <returns>A reference to this instance after the insert operation has completed.</returns>
		public CStringBuilder Insert(Int32 index, Int64 value)
		{
#if NET8_0_OR_GREATER
			if (Concurrent.InsertUtf8(this, index, value) is { } result8)
				return result8;
#endif
#if NET6_0_OR_GREATER
			if (Concurrent.InsertUtf16(this, index, value) is { } result16)
				return result16;
#endif
			return this.Insert(index, value.ToString(CultureInfo.CurrentCulture));
		}
		/// <summary>
		/// Inserts the UTF-8 representation of the specified 8-bit signed integer into this instance at the specified
		/// UTF-8 unit position.
		/// </summary>
		/// <param name="index">The position in this instance where insertion begins.</param>
		/// <param name="value">The value to insert.</param>
		/// <returns>A reference to this instance after the insert operation has completed.</returns>
		public CStringBuilder Insert(Int32 index, SByte value)
		{
#if NET8_0_OR_GREATER
			if (Concurrent.InsertUtf8(this, index, value) is { } result8)
				return result8;
#endif
#if NET6_0_OR_GREATER
			if (Concurrent.InsertUtf16(this, index, value) is { } result16)
				return result16;
#endif
			return this.Insert(index, value.ToString(CultureInfo.CurrentCulture));
		}
		/// <summary>
		/// Inserts the UTF-8 representation of the specified single-precision floating point number into this instance
		/// at the specified UTF-8 unit position.
		/// </summary>
		/// <param name="index">The position in this instance where insertion begins.</param>
		/// <param name="value">The value to insert.</param>
		/// <returns>A reference to this instance after the insert operation has completed.</returns>
		public CStringBuilder Insert(Int32 index, Single value)
		{
#if NET8_0_OR_GREATER
			if (Concurrent.InsertUtf8(this, index, value) is { } result8)
				return result8;
#endif
#if NET6_0_OR_GREATER
			if (Concurrent.InsertUtf16(this, index, value) is { } result16)
				return result16;
#endif
			return this.Insert(index, value.ToString(CultureInfo.CurrentCulture));
		}
		/// <summary>
		/// Inserts the UTF-8 representation of the specified 16-bit unsigned integer into this instance at the
		/// specified UTF-8 unit position.
		/// </summary>
		/// <param name="index">The position in this instance where insertion begins.</param>
		/// <param name="value">The value to insert.</param>
		/// <returns>A reference to this instance after the insert operation has completed.</returns>
		public CStringBuilder Insert(Int32 index, UInt16 value)
		{
#if NET8_0_OR_GREATER
			if (Concurrent.InsertUtf8(this, index, value) is { } result8)
				return result8;
#endif
#if NET6_0_OR_GREATER
			if (Concurrent.InsertUtf16(this, index, value) is { } result16)
				return result16;
#endif
			return this.Insert(index, value.ToString(CultureInfo.CurrentCulture));
		}
		/// <summary>
		/// Inserts the UTF-8 representation of the specified 32-bit unsigned integer into this instance at the
		/// specified UTF-8 unit position.
		/// </summary>
		/// <param name="index">The position in this instance where insertion begins.</param>
		/// <param name="value">The value to insert.</param>
		/// <returns>A reference to this instance after the insert operation has completed.</returns>
		public CStringBuilder Insert(Int32 index, UInt32 value)
		{
#if NET8_0_OR_GREATER
			if (Concurrent.InsertUtf8(this, index, value) is { } result8)
				return result8;
#endif
#if NET6_0_OR_GREATER
			if (Concurrent.InsertUtf16(this, index, value) is { } result16)
				return result16;
#endif
			return this.Insert(index, value.ToString(CultureInfo.CurrentCulture));
		}
		/// <summary>
		/// Inserts the UTF-8 representation of the specified 64-bit unsigned integer into this instance at the
		/// specified UTF-8 unit position.
		/// </summary>
		/// <param name="index">The position in this instance where insertion begins.</param>
		/// <param name="value">The value to insert.</param>
		/// <returns>A reference to this instance after the insert operation has completed.</returns>
		public CStringBuilder Insert(Int32 index, UInt64 value)
		{
#if NET8_0_OR_GREATER
			if (Concurrent.InsertUtf8(this, index, value) is { } result8)
				return result8;
#endif
#if NET6_0_OR_GREATER
			if (Concurrent.InsertUtf16(this, index, value) is { } result16)
				return result16;
#endif
			return this.Insert(index, value.ToString(CultureInfo.CurrentCulture));
		}
		/// <summary>
		/// Inserts the UTF-8 unit or the UTF-8 representation of the specified 8-bit unsigned integer to this
		/// instance at the specified UTF-8 unit position.
		/// </summary>
		/// <param name="index">The position in this instance where insertion begins.</param>
		/// <param name="value">The value to insert.</param>
		/// <param name="asNumber">
		/// Indicates whether <paramref name="value"/> should be treated as a number instead of UTF-8 unit.
		/// </param>
		/// <returns>A reference to this instance after the insert operation has completed.</returns>
		public CStringBuilder Insert(Int32 index, Byte value, Boolean asNumber = false)
		{
			// ReSharper disable once ConvertIfStatementToReturnStatement
			if (!asNumber)
				return this.Insert(index, [value,]);
#if NET8_0_OR_GREATER
			if (Concurrent.InsertUtf8(this, index, value) is { } result8)
				return result8;
#endif
#if NET6_0_OR_GREATER
			if (Concurrent.InsertUtf16(this, index, value) is { } result16)
				return result16;
#endif
			return this.Insert(index, value.ToString(CultureInfo.CurrentCulture));
		}

#if NET8_0_OR_GREATER
		/// <summary>
		/// Inserts the UTF-8 representation of the specified <typeparamref name="T"/> value into
		/// <paramref name="concurrent"/> at the specified UTF-8 unit position.
		/// </summary>
		/// <typeparam name="T">A <see cref="IUtf8SpanFormattable"/> instance.</typeparam>
		/// <param name="concurrent">A <see cref="Concurrent"/> instance.</param>
		/// <param name="index">The position in this instance where insertion begins.</param>
		/// <param name="value">The value to insert.</param>
		/// <returns>A reference to this instance after the insert operation has completed.</returns>
		[SkipLocalsInit]
#if !PACKAGE
		[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS3218)]
#endif
		private static CStringBuilder? InsertUtf8<T>(Concurrent concurrent, Int32 index, T value)
			where T : IUtf8SpanFormattable
		{
			Span<Byte> span = stackalloc Byte[StackAllocationHelper.StackallocByteThreshold];
			return value.TryFormat(span, out Int32 count, default, default) ?
				concurrent.Insert(index, span[..count]) :
				default;
		}
#endif
#if NET6_0_OR_GREATER
		/// <summary>
		/// Inserts the UTF-8 representation of the specified <typeparamref name="T"/> value into
		/// <paramref name="concurrent"/> at the specified UTF-8 unit position.
		/// </summary>
		/// <typeparam name="T">A <see cref="ISpanFormattable"/> instance.</typeparam>
		/// <param name="concurrent">A <see cref="Concurrent"/> instance.</param>
		/// <param name="index">The position in this instance where insertion begins.</param>
		/// <param name="value">The value to insert.</param>
		/// <returns>A reference to this instance after the insert operation has completed.</returns>
		[SkipLocalsInit]
#if !PACKAGE && NET8_0_OR_GREATER
		[ExcludeFromCodeCoverage]
#endif
#if !PACKAGE
		[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS3218)]
#endif
		private static CStringBuilder? InsertUtf16<T>(Concurrent concurrent, Int32 index, T value)
			where T : ISpanFormattable
		{
			Span<Char> span = stackalloc Char[StackAllocationHelper.StackallocByteThreshold];
			return value.TryFormat(span, out Int32 count, default, default) ?
				concurrent.Insert(index, span[..count]) :
				default;
		}
#endif
	}
}