namespace Rxmxnx.PInvoke;

public partial class CStringBuilder
{
	private readonly partial struct Concurrent
	{
		/// <summary>
		/// Appends each non-empty UTF-8 text in <paramref name="view"/>.
		/// </summary>
		/// <param name="view">A UTF-8 text sequence to append.</param>
		/// <returns>A reference to this instance after the append operation has completed.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public CStringBuilder Append(CStringSequence.Utf8View view)
		{
#if NET9_0_OR_GREATER
			using (lockObj.EnterScope())
#else
			lock (lockObj)
#endif
			{
				foreach (ReadOnlySpan<Byte> value in view)
					builder._chunk = builder._chunk.Append(value);
			}
			return builder;
		}
		/// <summary>
		/// Appends the specified UTF-8 units read-only span to this instance.
		/// </summary>
		/// <param name="value">The UTF-8 units read-only span to append.</param>
		/// <returns>A reference to this instance after the append operation has completed.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public CStringBuilder Append(ReadOnlySpan<Byte> value)
		{
#if NET9_0_OR_GREATER
			using (lockObj.EnterScope())
#else
			lock (lockObj)
#endif
				builder._chunk = builder._chunk.Append(value);
			return builder;
		}
		/// <summary>
		/// Appends the specified UTF-8 units read-only sequence to this instance.
		/// </summary>
		/// <param name="value">The UTF-8 units read-only sequence to append.</param>
		/// <returns>A reference to this instance after the append operation has completed.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public CStringBuilder Append(ReadOnlySequence<Byte> value)
		{
#if NET9_0_OR_GREATER
			using (lockObj.EnterScope())
#else
			lock (lockObj)
#endif
				builder._chunk = builder._chunk.Append(value);
			return builder;
		}
		/// <summary>
		/// Appends the UTF-8 representation of the characters in the specified read-only span to this instance.
		/// </summary>
		/// <param name="value">The read-only span of characters to append.</param>
		/// <returns>A reference to this instance after the append operation has completed.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public CStringBuilder Append(ReadOnlySpan<Char> value)
		{
#if NET9_0_OR_GREATER
			using (lockObj.EnterScope())
#else
			lock (lockObj)
#endif
				builder._chunk = builder._chunk.Append(value);
			return builder;
		}
		/// <summary>
		/// Appends the UTF-8 representation of the specified Boolean value to this instance.
		/// </summary>
		/// <param name="value">The Boolean value to append.</param>
		/// <returns>A reference to this instance after the append operation has completed.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public CStringBuilder Append(Boolean value)
		{
#if NET9_0_OR_GREATER
			using (lockObj.EnterScope())
#else
			lock (lockObj)
#endif
				builder._chunk = builder._chunk.Append(value.ToString(CultureInfo.CurrentCulture));
			return builder;
		}
		/// <summary>
		/// Appends the UTF-8 representation of the specified <see cref="Char"/> object to this instance.
		/// </summary>
		/// <param name="value">The UTF-16-encoded code char to append.</param>
		/// <returns>A reference to this instance after the append operation has completed.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public CStringBuilder Append(Char value)
		{
#if NET9_0_OR_GREATER
			using (lockObj.EnterScope())
#else
			lock (lockObj)
#endif
#if NET8_0_OR_GREATER
				builder._chunk = builder._chunk.AppendUtf8(value);
#elif NET6_0_OR_GREATER
				builder._chunk = builder._chunk.AppendUtf16(value);
#else
				builder._chunk = builder._chunk.Append(value.ToString(CultureInfo.CurrentCulture));
#endif
			return builder;
		}
		/// <summary>
		/// Appends the UTF-8 representation of the specified decimal number to this instance.
		/// </summary>
		/// <param name="value">The value to append.</param>
		/// <returns>A reference to this instance after the append operation has completed.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public CStringBuilder Append(Decimal value)
		{
#if NET9_0_OR_GREATER
			using (lockObj.EnterScope())
#else
			lock (lockObj)
#endif
#if NET8_0_OR_GREATER
				builder._chunk = builder._chunk.AppendUtf8(value);
#elif NET6_0_OR_GREATER
				builder._chunk = builder._chunk.AppendUtf16(value);
#else
				builder._chunk = builder._chunk.Append(value.ToString(CultureInfo.CurrentCulture));
#endif
			return builder;
		}
		/// <summary>
		/// Appends the UTF-8 representation of the specified double-precision floating-point number to this instance.
		/// </summary>
		/// <param name="value">The value to append.</param>
		/// <returns>A reference to this instance after the append operation has completed.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public CStringBuilder Append(Double value)
		{
#if NET9_0_OR_GREATER
			using (lockObj.EnterScope())
#else
			lock (lockObj)
#endif
#if NET8_0_OR_GREATER
				builder._chunk = builder._chunk.AppendUtf8(value);
#elif NET6_0_OR_GREATER
				builder._chunk = builder._chunk.AppendUtf16(value);
#else
				builder._chunk = builder._chunk.Append(value.ToString(CultureInfo.CurrentCulture));
#endif
			return builder;
		}
		/// <summary>
		/// Appends the UTF-8 representation of the specified 16-bit signed integer to this instance.
		/// </summary>
		/// <param name="value">The value to append.</param>
		/// <returns>A reference to this instance after the append operation has completed.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public CStringBuilder Append(Int16 value)
		{
#if NET9_0_OR_GREATER
			using (lockObj.EnterScope())
#else
			lock (lockObj)
#endif
#if NET8_0_OR_GREATER
				builder._chunk = builder._chunk.AppendUtf8(value);
#elif NET6_0_OR_GREATER
				builder._chunk = builder._chunk.AppendUtf16(value);
#else
				builder._chunk = builder._chunk.Append(value.ToString(CultureInfo.CurrentCulture));
#endif
			return builder;
		}
		/// <summary>
		/// Appends the UTF-8 representation of the specified 32-bit signed integer to this instance.
		/// </summary>
		/// <param name="value">The value to append.</param>
		/// <returns>A reference to this instance after the append operation has completed.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public CStringBuilder Append(Int32 value)
		{
#if NET9_0_OR_GREATER
			using (lockObj.EnterScope())
#else
			lock (lockObj)
#endif
#if NET8_0_OR_GREATER
				builder._chunk = builder._chunk.AppendUtf8(value);
#elif NET6_0_OR_GREATER
				builder._chunk = builder._chunk.AppendUtf16(value);
#else
				builder._chunk = builder._chunk.Append(value.ToString(CultureInfo.CurrentCulture));
#endif
			return builder;
		}
		/// <summary>
		/// Appends the UTF-8 representation of the specified 64-bit signed integer to this instance.
		/// </summary>
		/// <param name="value">The value to append.</param>
		/// <returns>A reference to this instance after the append operation has completed.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public CStringBuilder Append(Int64 value)
		{
#if NET9_0_OR_GREATER
			using (lockObj.EnterScope())
#else
			lock (lockObj)
#endif
#if NET8_0_OR_GREATER
				builder._chunk = builder._chunk.AppendUtf8(value);
#elif NET6_0_OR_GREATER
				builder._chunk = builder._chunk.AppendUtf16(value);
#else
				builder._chunk = builder._chunk.Append(value.ToString(CultureInfo.CurrentCulture));
#endif
			return builder;
		}
		/// <summary>
		/// Appends the UTF-8 representation of the specified 8-bit signed integer to this instance.
		/// </summary>
		/// <param name="value">The value to append.</param>
		/// <returns>A reference to this instance after the append operation has completed.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public CStringBuilder Append(SByte value)
		{
#if NET9_0_OR_GREATER
			using (lockObj.EnterScope())
#else
			lock (lockObj)
#endif
#if NET8_0_OR_GREATER
				builder._chunk = builder._chunk.AppendUtf8(value);
#elif NET6_0_OR_GREATER
				builder._chunk = builder._chunk.AppendUtf16(value);
#else
				builder._chunk = builder._chunk.Append(value.ToString(CultureInfo.CurrentCulture));
#endif
			return builder;
		}
		/// <summary>
		/// Appends the UTF-8 representation of the specified single-precision floating-point number to this instance.
		/// </summary>
		/// <param name="value">The value to append.</param>
		/// <returns>A reference to this instance after the append operation has completed.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public CStringBuilder Append(Single value)
		{
#if NET9_0_OR_GREATER
			using (lockObj.EnterScope())
#else
			lock (lockObj)
#endif
#if NET8_0_OR_GREATER
				builder._chunk = builder._chunk.AppendUtf8(value);
#elif NET6_0_OR_GREATER
				builder._chunk = builder._chunk.AppendUtf16(value);
#else
				builder._chunk = builder._chunk.Append(value.ToString(CultureInfo.CurrentCulture));
#endif
			return builder;
		}
		/// <summary>
		/// Appends the UTF-8 representation of the specified 16-bit unsigned integer to this instance.
		/// </summary>
		/// <param name="value">The value to append.</param>
		/// <returns>A reference to this instance after the append operation has completed.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public CStringBuilder Append(UInt16 value)
		{
#if NET9_0_OR_GREATER
			using (lockObj.EnterScope())
#else
			lock (lockObj)
#endif
#if NET8_0_OR_GREATER
				builder._chunk = builder._chunk.AppendUtf8(value);
#elif NET6_0_OR_GREATER
				builder._chunk = builder._chunk.AppendUtf16(value);
#else
				builder._chunk = builder._chunk.Append(value.ToString(CultureInfo.CurrentCulture));
#endif
			return builder;
		}
		/// <summary>
		/// Appends the UTF-8 representation of the specified 32-bit unsigned integer to this instance.
		/// </summary>
		/// <param name="value">The value to append.</param>
		/// <returns>A reference to this instance after the append operation has completed.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public CStringBuilder Append(UInt32 value)
		{
#if NET9_0_OR_GREATER
			using (lockObj.EnterScope())
#else
			lock (lockObj)
#endif
#if NET8_0_OR_GREATER
				builder._chunk = builder._chunk.AppendUtf8(value);
#elif NET6_0_OR_GREATER
			builder._chunk = builder._chunk.AppendUtf16(value);
#else
				builder._chunk = builder._chunk.Append(value.ToString(CultureInfo.CurrentCulture));
#endif
			return builder;
		}
		/// <summary>
		/// Appends the UTF-8 representation of the specified 64-bit unsigned integer to this instance.
		/// </summary>
		/// <param name="value">The value to append.</param>
		/// <returns>A reference to this instance after the append operation has completed.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public CStringBuilder Append(UInt64 value)
		{
#if NET9_0_OR_GREATER
			using (lockObj.EnterScope())
#else
			lock (lockObj)
#endif
#if NET8_0_OR_GREATER
				builder._chunk = builder._chunk.AppendUtf8(value);
#elif NET6_0_OR_GREATER
				builder._chunk = builder._chunk.AppendUtf16(value);
#else
				builder._chunk = builder._chunk.Append(value.ToString(CultureInfo.CurrentCulture));
#endif
			return builder;
		}
		/// <summary>
		/// Appends the UTF-8 unit or the UTF-8 representation of the specified 8-bit unsigned integer to this instance.
		/// </summary>
		/// <param name="value">The value to append.</param>
		/// <param name="asNumber">
		/// Indicates whether <paramref name="value"/> should be treated as a number instead of UTF-8 unit.
		/// </param>
		/// <returns>A reference to this instance after the append operation has completed.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public CStringBuilder Append(Byte value, Boolean asNumber = false)
		{
#if NET9_0_OR_GREATER
			using (lockObj.EnterScope())
#else
			lock (lockObj)
#endif
#if NET8_0_OR_GREATER
				builder._chunk = asNumber ? builder._chunk.AppendUtf8(value) : builder._chunk.Append([value,]);
#elif NET6_0_OR_GREATER
				builder._chunk = asNumber ? builder._chunk.AppendUtf16(value) : builder._chunk.Append([value,]);
#else
			{
				builder._chunk = asNumber ?
					builder._chunk.Append(value.ToString(CultureInfo.CurrentCulture)) :
					builder._chunk.Append([value,]);
			}
#endif
			return builder;
		}
#if NETCOREAPP
		/// <summary>
		/// Appends the UTF-8 representation of the specified rune to this instance.
		/// </summary>
		/// <param name="value">The value to append.</param>
		/// <returns>A reference to this instance after the append operation has completed.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public CStringBuilder Append(Rune value)
		{
			Span<Byte> utf8Text = stackalloc Byte[value.Utf8SequenceLength];
			value.EncodeToUtf8(utf8Text);
			return this.Append(utf8Text);
		}
#endif
	}
}