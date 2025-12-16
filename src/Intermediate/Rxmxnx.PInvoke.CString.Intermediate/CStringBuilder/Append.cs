namespace Rxmxnx.PInvoke;

public sealed partial class CStringBuilder
{
	/// <summary>
	/// Appends each non-empty UTF-8 text in <paramref name="sequence"/>.
	/// </summary>
	/// <param name="sequence">A UTF-8 text sequence to append.</param>
	/// <returns>A reference to this instance after the append operation has completed.</returns>
	public CStringBuilder Append(CStringSequence sequence)
	{
		CStringSequence.Utf8View view = new(sequence, false);
#if NET9_0_OR_GREATER
		using (this._lock.EnterScope())
#else
		lock (this._lock)
#endif
		{
			foreach (ReadOnlySpan<Byte> value in view)
				this._chunk = this._chunk.Append(value);
		}
		return this;
	}
	/// <summary>
	/// Appends the UTF-8 text to this instance.
	/// </summary>
	/// <param name="value">The UTF-8 text to append.</param>
	/// <returns>A reference to this instance after the append operation has completed.</returns>
	public CStringBuilder Append(CString? value) => !CString.IsNullOrEmpty(value) ? this.Append(value.AsSpan()) : this;
	/// <summary>
	/// Appends the UTF-8 representation of the specified string.
	/// </summary>
	/// <param name="value">The read-only character span to append.</param>
	/// <returns>A reference to this instance after the append operation has completed.</returns>
	public CStringBuilder Append(String? value) => !String.IsNullOrEmpty(value) ? this.Append(value.AsSpan()) : this;
	/// <summary>
	/// Appends the specified UTF-8 units read-only span to this instance.
	/// </summary>
	/// <param name="value">The UTF-8 units read-only span to append.</param>
	/// <returns>A reference to this instance after the append operation has completed.</returns>
	public CStringBuilder Append(ReadOnlySpan<Byte> value)
	{
		if (value.IsEmpty) return this;
#if NET9_0_OR_GREATER
		using (this._lock.EnterScope())
#else
		lock (this._lock)
#endif
			this._chunk = this._chunk.Append(value);
		return this;
	}
	/// <summary>
	/// Appends the UTF-8 representation of the characters in the specified array to this instance.
	/// </summary>
	/// <param name="value">The array of characters to append.</param>
	/// <returns>A reference to this instance after the append operation has completed.</returns>
	public CStringBuilder Append(Char[]? value)
	{
		if (value is null || value.Length == 0) return this;
		return this.Append(value.AsSpan());
	}
	/// <summary>
	/// Appends the specified UTF-8 units array to this instance.
	/// </summary>
	/// <param name="value">The array of UTF-8 units to append.</param>
	/// <returns>A reference to this instance after the append operation has completed.</returns>
	public CStringBuilder Append(Byte[]? value)
	{
		if (value is null || value.Length == 0) return this;
		return this.Append(value.AsSpan());
	}
	/// <summary>
	/// Appends the UTF-8 representation of the characters in the specified read-only span to this instance.
	/// </summary>
	/// <param name="value">The read-only span of characters to append.</param>
	/// <returns>A reference to this instance after the append operation has completed.</returns>
	public CStringBuilder Append(ReadOnlySpan<Char> value)
	{
		if (value.IsEmpty) return this;
#if NET9_0_OR_GREATER
		using (this._lock.EnterScope())
#else
		lock (this._lock)
#endif
			this._chunk = this._chunk.Append(value);
		return this;
	}
	/// <summary>
	/// Appends the UTF-8 representation of the specified Boolean value to this instance.
	/// </summary>
	/// <param name="value">The Boolean value to append.</param>
	/// <returns>A reference to this instance after the append operation has completed.</returns>
	public CStringBuilder Append(Boolean value)
	{
#if NET9_0_OR_GREATER
		using (this._lock.EnterScope())
#else
		lock (this._lock)
#endif
			this._chunk = this._chunk.Append(value.ToString(CultureInfo.CurrentCulture));
		return this;
	}
	/// <summary>
	/// Appends the UTF-8 representation of the specified Boolean value to this instance.
	/// </summary>
	/// <param name="value">The Boolean value to append.</param>
	/// <returns>A reference to this instance after the append operation has completed.</returns>
	public CStringBuilder Append(Boolean? value) => value.HasValue ? this.Append(value.Value) : this;
	/// <summary>
	/// Appends the UTF-8 representation of the specified <see cref="Char"/> object to this instance.
	/// </summary>
	/// <param name="value">The UTF-16-encoded code char to append.</param>
	/// <returns>A reference to this instance after the append operation has completed.</returns>
	public CStringBuilder Append(Char value)
	{
#if NET9_0_OR_GREATER
		using (this._lock.EnterScope())
#else
		lock (this._lock)
#endif
#if NET8_0_OR_GREATER
			this._chunk = this._chunk.AppendUtf8(value);
#elif NET6_0_OR_GREATER
			this._chunk = this._chunk.AppendUtf16(value);
#else
			this._chunk = this._chunk.Append(value.ToString(CultureInfo.CurrentCulture));
#endif
		return this;
	}
	/// <summary>
	/// Appends the UTF-8 representation of the specified <see cref="Char"/> object to this instance.
	/// </summary>
	/// <param name="value">The UTF-16-encoded code char to append.</param>
	/// <returns>A reference to this instance after the append operation has completed.</returns>
	public CStringBuilder Append(Char? value) => value.HasValue ? this.Append(value.Value) : this;
	/// <summary>
	/// Appends the UTF-8 representation of the specified decimal number to this instance.
	/// </summary>
	/// <param name="value">The value to append.</param>
	/// <returns>A reference to this instance after the append operation has completed.</returns>
	public CStringBuilder Append(Decimal value)
	{
#if NET9_0_OR_GREATER
		using (this._lock.EnterScope())
#else
		lock (this._lock)
#endif
#if NET8_0_OR_GREATER
			this._chunk = this._chunk.AppendUtf8(value);
#elif NET6_0_OR_GREATER
			this._chunk = this._chunk.AppendUtf16(value);
#else
			this._chunk = this._chunk.Append(value.ToString(CultureInfo.CurrentCulture));
#endif
		return this;
	}
	/// <summary>
	/// Appends the UTF-8 representation of the specified double-precision floating-point number to this instance.
	/// </summary>
	/// <param name="value">The value to append.</param>
	/// <returns>A reference to this instance after the append operation has completed.</returns>
	public CStringBuilder Append(Double value)
	{
#if NET9_0_OR_GREATER
		using (this._lock.EnterScope())
#else
		lock (this._lock)
#endif
#if NET8_0_OR_GREATER
			this._chunk = this._chunk.AppendUtf8(value);
#elif NET6_0_OR_GREATER
			this._chunk = this._chunk.AppendUtf16(value);
#else
			this._chunk = this._chunk.Append(value.ToString(CultureInfo.CurrentCulture));
#endif
		return this;
	}
	/// <summary>
	/// Appends the UTF-8 representation of the specified double-precision floating-point number to this instance.
	/// </summary>
	/// <param name="value">The value to append.</param>
	/// <returns>A reference to this instance after the append operation has completed.</returns>
	public CStringBuilder Append(Double? value) => value.HasValue ? this.Append(value.Value) : this;
	/// <summary>
	/// Appends the UTF-8 representation of the specified 16-bit signed integer to this instance.
	/// </summary>
	/// <param name="value">The value to append.</param>
	/// <returns>A reference to this instance after the append operation has completed.</returns>
	public CStringBuilder Append(Int16 value)
	{
		if (value < 0) throw new ArgumentOutOfRangeException(nameof(value));
#if NET9_0_OR_GREATER
		using (this._lock.EnterScope())
#else
		lock (this._lock)
#endif
#if NET8_0_OR_GREATER
			this._chunk = this._chunk.AppendUtf8(value);
#elif NET6_0_OR_GREATER
			this._chunk = this._chunk.AppendUtf16(value);
#else
			this._chunk = this._chunk.Append(value.ToString(CultureInfo.CurrentCulture));
#endif
		return this;
	}
	/// <summary>
	/// Appends the UTF-8 representation of the specified 16-bit signed integer to this instance.
	/// </summary>
	/// <param name="value">The value to append.</param>
	/// <returns>A reference to this instance after the append operation has completed.</returns>
	public CStringBuilder Append(Int16? value) => value.HasValue ? this.Append(value.Value) : this;
	/// <summary>
	/// Appends the UTF-8 representation of the specified 32-bit signed integer to this instance.
	/// </summary>
	/// <param name="value">The value to append.</param>
	/// <returns>A reference to this instance after the append operation has completed.</returns>
	public CStringBuilder Append(Int32 value)
	{
#if NET9_0_OR_GREATER
		using (this._lock.EnterScope())
#else
		lock (this._lock)
#endif
#if NET8_0_OR_GREATER
			this._chunk = this._chunk.AppendUtf8(value);
#elif NET6_0_OR_GREATER
			this._chunk = this._chunk.AppendUtf16(value);
#else
			this._chunk = this._chunk.Append(value.ToString(CultureInfo.CurrentCulture));
#endif
		return this;
	}
	/// <summary>
	/// Appends the UTF-8 representation of the specified 32-bit signed integer to this instance.
	/// </summary>
	/// <param name="value">The value to append.</param>
	/// <returns>A reference to this instance after the append operation has completed.</returns>
	public CStringBuilder Append(Int32? value) => value.HasValue ? this.Append(value.Value) : this;
	/// <summary>
	/// Appends the UTF-8 representation of the specified 64-bit signed integer to this instance.
	/// </summary>
	/// <param name="value">The value to append.</param>
	/// <returns>A reference to this instance after the append operation has completed.</returns>
	public CStringBuilder Append(Int64 value)
	{
#if NET9_0_OR_GREATER
		using (this._lock.EnterScope())
#else
		lock (this._lock)
#endif
#if NET8_0_OR_GREATER
			this._chunk = this._chunk.AppendUtf8(value);
#elif NET6_0_OR_GREATER
			this._chunk = this._chunk.AppendUtf16(value);
#else
			this._chunk = this._chunk.Append(value.ToString(CultureInfo.CurrentCulture));
#endif
		return this;
	}
	/// <summary>
	/// Appends the UTF-8 representation of the specified 64-bit signed integer to this instance.
	/// </summary>
	/// <param name="value">The value to append.</param>
	/// <returns>A reference to this instance after the append operation has completed.</returns>
	public CStringBuilder Append(Int64? value) => value.HasValue ? this.Append(value.Value) : this;
	/// <summary>
	/// Appends the UTF-8 representation of the specified 8-bit signed integer to this instance.
	/// </summary>
	/// <param name="value">The value to append.</param>
	/// <returns>A reference to this instance after the append operation has completed.</returns>
	public CStringBuilder Append(SByte value)
	{
#if NET9_0_OR_GREATER
		using (this._lock.EnterScope())
#else
		lock (this._lock)
#endif
#if NET8_0_OR_GREATER
			this._chunk = this._chunk.AppendUtf8(value);
#elif NET6_0_OR_GREATER
			this._chunk = this._chunk.AppendUtf16(value);
#else
			this._chunk = this._chunk.Append(value.ToString(CultureInfo.CurrentCulture));
#endif
		return this;
	}
	/// <summary>
	/// Appends the UTF-8 representation of the specified 8-bit signed integer to this instance.
	/// </summary>
	/// <param name="value">The value to append.</param>
	/// <returns>A reference to this instance after the append operation has completed.</returns>
	public CStringBuilder Append(SByte? value) => value.HasValue ? this.Append(value.Value) : this;
	/// <summary>
	/// Appends the UTF-8 representation of the specified single-precision floating-point number to this instance.
	/// </summary>
	/// <param name="value">The value to append.</param>
	/// <returns>A reference to this instance after the append operation has completed.</returns>
	public CStringBuilder Append(Single value)
	{
#if NET9_0_OR_GREATER
		using (this._lock.EnterScope())
#else
		lock (this._lock)
#endif
#if NET8_0_OR_GREATER
			this._chunk = this._chunk.AppendUtf8(value);
#elif NET6_0_OR_GREATER
			this._chunk = this._chunk.AppendUtf16(value);
#else
			this._chunk = this._chunk.Append(value.ToString(CultureInfo.CurrentCulture));
#endif
		return this;
	}
	/// <summary>
	/// Appends the UTF-8 representation of the specified single-precision floating-point number to this instance.
	/// </summary>
	/// <param name="value">The value to append.</param>
	/// <returns>A reference to this instance after the append operation has completed.</returns>
	public CStringBuilder Append(Single? value) => value.HasValue ? this.Append(value.Value) : this;
	/// <summary>
	/// Appends the UTF-8 representation of the specified 16-bit unsigned integer to this instance.
	/// </summary>
	/// <param name="value">The value to append.</param>
	/// <returns>A reference to this instance after the append operation has completed.</returns>
	public CStringBuilder Append(UInt16 value)
	{
#if NET9_0_OR_GREATER
		using (this._lock.EnterScope())
#else
		lock (this._lock)
#endif
#if NET8_0_OR_GREATER
			this._chunk = this._chunk.AppendUtf8(value);
#elif NET6_0_OR_GREATER
			this._chunk = this._chunk.AppendUtf16(value);
#else
			this._chunk = this._chunk.Append(value.ToString(CultureInfo.CurrentCulture));
#endif
		return this;
	}
	/// <summary>
	/// Appends the UTF-8 representation of the specified 16-bit unsigned integer to this instance.
	/// </summary>
	/// <param name="value">The value to append.</param>
	/// <returns>A reference to this instance after the append operation has completed.</returns>
	public CStringBuilder Append(UInt16? value) => value.HasValue ? this.Append(value.Value) : this;
	/// <summary>
	/// Appends the UTF-8 representation of the specified 32-bit unsigned integer to this instance.
	/// </summary>
	/// <param name="value">The value to append.</param>
	/// <returns>A reference to this instance after the append operation has completed.</returns>
	public CStringBuilder Append(UInt32 value)
	{
#if NET9_0_OR_GREATER
		using (this._lock.EnterScope())
#else
		lock (this._lock)
#endif
#if NET8_0_OR_GREATER
			this._chunk = this._chunk.AppendUtf8(value);
#elif NET6_0_OR_GREATER
			this._chunk = this._chunk.AppendUtf16(value);
#else
			this._chunk = this._chunk.Append(value.ToString(CultureInfo.CurrentCulture));
#endif
		return this;
	}
	/// <summary>
	/// Appends the UTF-8 representation of the specified 32-bit unsigned integer to this instance.
	/// </summary>
	/// <param name="value">The value to append.</param>
	/// <returns>A reference to this instance after the append operation has completed.</returns>
	public CStringBuilder Append(UInt32? value) => value.HasValue ? this.Append(value.Value) : this;
	/// <summary>
	/// Appends the UTF-8 representation of the specified 64-bit unsigned integer to this instance.
	/// </summary>
	/// <param name="value">The value to append.</param>
	/// <returns>A reference to this instance after the append operation has completed.</returns>
	public CStringBuilder Append(UInt64 value)
	{
#if NET9_0_OR_GREATER
		using (this._lock.EnterScope())
#else
		lock (this._lock)
#endif
#if NET8_0_OR_GREATER
			this._chunk = this._chunk.AppendUtf8(value);
#elif NET6_0_OR_GREATER
			this._chunk = this._chunk.AppendUtf16(value);
#else
			this._chunk = this._chunk.Append(value.ToString(CultureInfo.CurrentCulture));
#endif
		return this;
	}
	/// <summary>
	/// Appends the UTF-8 representation of the specified 64-bit unsigned integer to this instance.
	/// </summary>
	/// <param name="value">The value to append.</param>
	/// <returns>A reference to this instance after the append operation has completed.</returns>
	public CStringBuilder Append(UInt64? value) => value.HasValue ? this.Append(value.Value) : this;
	/// <summary>
	/// Appends the UTF-8 unit or the UTF-8 representation of the specified 8-bit unsigned integer to this instance.
	/// </summary>
	/// <param name="value">The value to append.</param>
	/// <param name="asNumber">
	/// Indicates whether <paramref name="value"/> should be treated as a number instead of UTF-8 unit.
	/// </param>
	/// <returns>A reference to this instance after the append operation has completed.</returns>
	public CStringBuilder Append(Byte value, Boolean asNumber = false)
	{
#if NET9_0_OR_GREATER
		using (this._lock.EnterScope())
#else
		lock (this._lock)
#endif
#if NET8_0_OR_GREATER
			this._chunk = asNumber ? this._chunk.AppendUtf8(value) : this._chunk.Append([value,]);
#elif NET6_0_OR_GREATER
			this._chunk = asNumber ? this._chunk.AppendUtf16(value) : this._chunk.Append([value,]);
#else
		{
			this._chunk = asNumber ?
				this._chunk.Append(value.ToString(CultureInfo.CurrentCulture)) :
				this._chunk.Append([value,]);
		}
#endif
		return this;
	}
	/// <summary>
	/// Appends the UTF-8 unit or the UTF-8 representation of the specified 8-bit unsigned integer to this instance.
	/// </summary>
	/// <param name="value">The value to append.</param>
	/// <param name="asNumber">
	/// Indicates whether <paramref name="value"/> should be treated as a number instead of UTF-8 unit.
	/// </param>
	/// <returns>A reference to this instance after the append operation has completed.</returns>
	public CStringBuilder Append(Byte? value, Boolean asNumber = false)
		=> value.HasValue ? this.Append(value.Value, asNumber) : this;
}