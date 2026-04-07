namespace Rxmxnx.PInvoke;

public sealed partial class CStringBuilder
{
	/// <inheritdoc cref="CStringBuilder.Append(CStringSequence?)"/>
	/// <remarks>This operation is thread-safe.</remarks>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public CStringBuilder ConcurrentAppend(CStringSequence? sequence)
	{
		if (sequence is null || sequence.NonEmptyCount == 0) return this;
		CStringSequence.Utf8View view = new(sequence, false);
		return new Concurrent(this.GetLock(), this).Append(view);
	}
	/// <inheritdoc cref="CStringBuilder.Append(CString?)"/>
	/// <remarks>This operation is thread-safe.</remarks>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public CStringBuilder ConcurrentAppend(CString? value)
		=> CString.IsNullOrEmpty(value) ? this : new Concurrent(this.GetLock(), this).Append(value.AsSpan());
	/// <inheritdoc cref="CStringBuilder.Append(String?)"/>
	/// <remarks>This operation is thread-safe.</remarks>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public CStringBuilder ConcurrentAppend(String? value)
		=> String.IsNullOrEmpty(value) ? this : new Concurrent(this.GetLock(), this).Append(value.AsSpan());
	/// <inheritdoc cref="CStringBuilder.Append(ReadOnlySpan{Byte})"/>
	/// <remarks>This operation is thread-safe.</remarks>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public CStringBuilder ConcurrentAppend(ReadOnlySpan<Byte> value)
		=> value.IsEmpty ? this : new Concurrent(this.GetLock(), this).Append(value);
	/// <inheritdoc cref="CStringBuilder.Append(ReadOnlySequence{Byte})"/>
	/// <remarks>This operation is thread-safe.</remarks>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public CStringBuilder ConcurrentAppend(ReadOnlySequence<Byte> value)
		=> value.IsEmpty ? this : new Concurrent(this.GetLock(), this).Append(value);
	/// <inheritdoc cref="CStringBuilder.Append(Char[])"/>
	/// <remarks>This operation is thread-safe.</remarks>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public CStringBuilder ConcurrentAppend(Char[]? value)
		=> value is null || value.Length == 0 ? this : new Concurrent(this.GetLock(), this).Append(value.AsSpan());
	/// <inheritdoc cref="CStringBuilder.Append(Byte[])"/>
	/// <remarks>This operation is thread-safe.</remarks>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public CStringBuilder ConcurrentAppend(Byte[]? value)
		=> value is null || value.Length == 0 ? this : new Concurrent(this.GetLock(), this).Append(value.AsSpan());
	/// <inheritdoc cref="CStringBuilder.Append(ReadOnlySpan{Char})"/>
	/// <remarks>This operation is thread-safe.</remarks>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public CStringBuilder ConcurrentAppend(ReadOnlySpan<Char> value)
		=> value.IsEmpty ? this : new Concurrent(this.GetLock(), this).Append(value);
	/// <inheritdoc cref="CStringBuilder.Append(Boolean)"/>
	/// <remarks>This operation is thread-safe.</remarks>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public CStringBuilder ConcurrentAppend(Boolean value) => new Concurrent(this.GetLock(), this).Append(value);
	/// <inheritdoc cref="CStringBuilder.Append(Nullable{Boolean})"/>
	/// <remarks>This operation is thread-safe.</remarks>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public CStringBuilder ConcurrentAppend(Boolean? value)
		=> !value.HasValue ? this : new Concurrent(this.GetLock(), this).Append(value.Value);
	/// <inheritdoc cref="CStringBuilder.Append(Char)"/>
	/// <remarks>This operation is thread-safe.</remarks>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public CStringBuilder ConcurrentAppend(Char value) => new Concurrent(this.GetLock(), this).Append(value);
	/// <inheritdoc cref="CStringBuilder.Append(Nullable{Char})"/>
	/// <remarks>This operation is thread-safe.</remarks>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public CStringBuilder ConcurrentAppend(Char? value)
		=> !value.HasValue ? this : new Concurrent(this.GetLock(), this).Append(value.Value);
	/// <inheritdoc cref="CStringBuilder.Append(Decimal)"/>
	/// <remarks>This operation is thread-safe.</remarks>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public CStringBuilder ConcurrentAppend(Decimal value) => new Concurrent(this.GetLock(), this).Append(value);
	/// <inheritdoc cref="CStringBuilder.Append(Nullable{Decimal})"/>
	/// <remarks>This operation is thread-safe.</remarks>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public CStringBuilder ConcurrentAppend(Decimal? value)
		=> !value.HasValue ? this : new Concurrent(this.GetLock(), this).Append(value.Value);
	/// <inheritdoc cref="CStringBuilder.Append(Double)"/>
	/// <remarks>This operation is thread-safe.</remarks>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public CStringBuilder ConcurrentAppend(Double value) => new Concurrent(this.GetLock(), this).Append(value);
	/// <inheritdoc cref="CStringBuilder.Append(Nullable{Double})"/>
	/// <remarks>This operation is thread-safe.</remarks>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public CStringBuilder ConcurrentAppend(Double? value)
		=> !value.HasValue ? this : new Concurrent(this.GetLock(), this).Append(value.Value);
	/// <inheritdoc cref="CStringBuilder.Append(Int16)"/>
	/// <remarks>This operation is thread-safe.</remarks>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public CStringBuilder ConcurrentAppend(Int16 value) => new Concurrent(this.GetLock(), this).Append(value);
	/// <inheritdoc cref="CStringBuilder.Append(Nullable{Int16})"/>
	/// <remarks>This operation is thread-safe.</remarks>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public CStringBuilder ConcurrentAppend(Int16? value)
		=> !value.HasValue ? this : new Concurrent(this.GetLock(), this).Append(value.Value);
	/// <inheritdoc cref="CStringBuilder.Append(Int32)"/>
	/// <remarks>This operation is thread-safe.</remarks>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public CStringBuilder ConcurrentAppend(Int32 value) => new Concurrent(this.GetLock(), this).Append(value);
	/// <inheritdoc cref="CStringBuilder.Append(Nullable{Int32})"/>
	/// <remarks>This operation is thread-safe.</remarks>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public CStringBuilder ConcurrentAppend(Int32? value)
		=> !value.HasValue ? this : new Concurrent(this.GetLock(), this).Append(value.Value);
	/// <inheritdoc cref="CStringBuilder.Append(Int64)"/>
	/// <remarks>This operation is thread-safe.</remarks>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public CStringBuilder ConcurrentAppend(Int64 value) => new Concurrent(this.GetLock(), this).Append(value);
	/// <inheritdoc cref="CStringBuilder.Append(Nullable{Int64})"/>
	/// <remarks>This operation is thread-safe.</remarks>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public CStringBuilder ConcurrentAppend(Int64? value)
		=> !value.HasValue ? this : new Concurrent(this.GetLock(), this).Append(value.Value);
	/// <inheritdoc cref="CStringBuilder.Append(SByte)"/>
	/// <remarks>This operation is thread-safe.</remarks>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public CStringBuilder ConcurrentAppend(SByte value) => new Concurrent(this.GetLock(), this).Append(value);
	/// <inheritdoc cref="CStringBuilder.Append(Nullable{SByte})"/>
	/// <remarks>This operation is thread-safe.</remarks>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public CStringBuilder ConcurrentAppend(SByte? value)
		=> !value.HasValue ? this : new Concurrent(this.GetLock(), this).Append(value.Value);
	/// <inheritdoc cref="CStringBuilder.Append(Single)"/>
	/// <remarks>This operation is thread-safe.</remarks>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public CStringBuilder ConcurrentAppend(Single value) => new Concurrent(this.GetLock(), this).Append(value);
	/// <inheritdoc cref="CStringBuilder.Append(Nullable{Single})"/>
	/// <remarks>This operation is thread-safe.</remarks>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public CStringBuilder ConcurrentAppend(Single? value)
		=> !value.HasValue ? this : new Concurrent(this.GetLock(), this).Append(value.Value);
	/// <inheritdoc cref="CStringBuilder.Append(UInt16)"/>
	/// <remarks>This operation is thread-safe.</remarks>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public CStringBuilder ConcurrentAppend(UInt16 value) => new Concurrent(this.GetLock(), this).Append(value);
	/// <inheritdoc cref="CStringBuilder.Append(Nullable{UInt16})"/>
	/// <remarks>This operation is thread-safe.</remarks>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public CStringBuilder ConcurrentAppend(UInt16? value)
		=> !value.HasValue ? this : new Concurrent(this.GetLock(), this).Append(value.Value);
	/// <inheritdoc cref="CStringBuilder.Append(UInt32)"/>
	/// <remarks>This operation is thread-safe.</remarks>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public CStringBuilder ConcurrentAppend(UInt32 value) => new Concurrent(this.GetLock(), this).Append(value);
	/// <inheritdoc cref="CStringBuilder.Append(Nullable{UInt32})"/>
	/// <remarks>This operation is thread-safe.</remarks>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public CStringBuilder ConcurrentAppend(UInt32? value)
		=> !value.HasValue ? this : new Concurrent(this.GetLock(), this).Append(value.Value);
	/// <inheritdoc cref="CStringBuilder.Append(UInt64)"/>
	/// <remarks>This operation is thread-safe.</remarks>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public CStringBuilder ConcurrentAppend(UInt64 value) => new Concurrent(this.GetLock(), this).Append(value);
	/// <inheritdoc cref="CStringBuilder.Append(Nullable{UInt64})"/>
	/// <remarks>This operation is thread-safe.</remarks>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public CStringBuilder ConcurrentAppend(UInt64? value)
		=> !value.HasValue ? this : new Concurrent(this.GetLock(), this).Append(value.Value);
	/// <inheritdoc cref="CStringBuilder.Append(Byte, Boolean)"/>
	/// <remarks>This operation is thread-safe.</remarks>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public CStringBuilder ConcurrentAppend(Byte value, Boolean asNumber = false)
		=> new Concurrent(this.GetLock(), this).Append(value, asNumber);
	/// <inheritdoc cref="CStringBuilder.Append(Byte, Boolean)"/>
	/// <remarks>This operation is thread-safe.</remarks>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public CStringBuilder ConcurrentAppend(Byte? value, Boolean asNumber = false)
		=> !value.HasValue ? this : new Concurrent(this.GetLock(), this).Append(value.Value, asNumber);
#if NETCOREAPP
	/// <inheritdoc cref="CStringBuilder.Append(Rune)"/>
	/// <remarks>This operation is thread-safe.</remarks>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public CStringBuilder ConcurrentAppend(Rune value) => new Concurrent(this.GetLock(), this).Append(value);
	/// <inheritdoc cref="CStringBuilder.Append(Nullable{Rune})"/>
	/// <remarks>This operation is thread-safe.</remarks>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public CStringBuilder ConcurrentAppend(Rune? value)
		=> !value.HasValue ? this : new Concurrent(this.GetLock(), this).Append(value.Value);
#endif
}