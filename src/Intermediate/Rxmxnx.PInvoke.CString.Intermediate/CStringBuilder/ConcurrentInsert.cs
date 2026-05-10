namespace Rxmxnx.PInvoke;

public sealed partial class CStringBuilder
{
	/// <inheritdoc cref="CStringBuilder.Insert(Int32, String)"/>
	/// <remarks>This operation is thread-safe.</remarks>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public CStringBuilder ConcurrentInsert(Int32 index, String? value)
		=> String.IsNullOrEmpty(value) ? this : new Concurrent(this.GetLock(), this).Insert(index, value.AsSpan());
	/// <inheritdoc cref="CStringBuilder.Insert(Int32, CString)"/>
	/// <remarks>This operation is thread-safe.</remarks>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public CStringBuilder ConcurrentInsert(Int32 index, CString? value)
		=> CString.IsNullOrEmpty(value) ? this : new Concurrent(this.GetLock(), this).Insert(index, value.AsSpan());
	/// <inheritdoc cref="CStringBuilder.Insert(Int32, Byte[])"/>
	/// <remarks>This operation is thread-safe.</remarks>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public CStringBuilder ConcurrentInsert(Int32 index, Byte[]? value)
		=> value is null || value.Length == 0 ?
			this :
			new Concurrent(this.GetLock(), this).Insert(index, value.AsSpan());
	/// <inheritdoc cref="CStringBuilder.Insert(Int32, ReadOnlySpan{Char})"/>
	/// <remarks>This operation is thread-safe.</remarks>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public CStringBuilder ConcurrentInsert(Int32 index, ReadOnlySpan<Char> value)
		=> value.IsEmpty ? this : new Concurrent(this.GetLock(), this).Insert(index, value);
	/// <inheritdoc cref="CStringBuilder.Insert(Int32, ReadOnlySpan{Byte})"/>
	/// <remarks>This operation is thread-safe.</remarks>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public CStringBuilder ConcurrentInsert(Int32 index, ReadOnlySpan<Byte> value)
		=> value.IsEmpty ? this : new Concurrent(this.GetLock(), this).Insert(index, value);
	/// <inheritdoc cref="CStringBuilder.Insert(Int32, Boolean)"/>
	/// <remarks>This operation is thread-safe.</remarks>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public CStringBuilder ConcurrentInsert(Int32 index, Boolean value)
		=> new Concurrent(this.GetLock(), this).Insert(index, value);
	/// <inheritdoc cref="CStringBuilder.Insert(Int32, Nullable{Boolean})"/>
	/// <remarks>This operation is thread-safe.</remarks>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public CStringBuilder ConcurrentInsert(Int32 index, Boolean? value)
		=> !value.HasValue ? this : new Concurrent(this.GetLock(), this).Insert(index, value.Value);
	/// <inheritdoc cref="CStringBuilder.Insert(Int32, Char)"/>
	/// <remarks>This operation is thread-safe.</remarks>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public CStringBuilder ConcurrentInsert(Int32 index, Char value)
		=> new Concurrent(this.GetLock(), this).Insert(index, value);
	/// <inheritdoc cref="CStringBuilder.Insert(Int32, Nullable{Char})"/>
	/// <remarks>This operation is thread-safe.</remarks>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public CStringBuilder ConcurrentInsert(Int32 index, Char? value)
		=> !value.HasValue ? this : new Concurrent(this.GetLock(), this).Insert(index, value.Value);
	/// <inheritdoc cref="CStringBuilder.Insert(Int32, Decimal)"/>
	/// <remarks>This operation is thread-safe.</remarks>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public CStringBuilder ConcurrentInsert(Int32 index, Decimal value)
		=> new Concurrent(this.GetLock(), this).Insert(index, value);
	/// <inheritdoc cref="CStringBuilder.Insert(Int32, Nullable{Decimal})"/>
	/// <remarks>This operation is thread-safe.</remarks>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public CStringBuilder ConcurrentInsert(Int32 index, Decimal? value)
		=> !value.HasValue ? this : new Concurrent(this.GetLock(), this).Insert(index, value.Value);
	/// <inheritdoc cref="CStringBuilder.Insert(Int32, Double)"/>
	/// <remarks>This operation is thread-safe.</remarks>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public CStringBuilder ConcurrentInsert(Int32 index, Double value)
		=> new Concurrent(this.GetLock(), this).Insert(index, value);
	/// <inheritdoc cref="CStringBuilder.Insert(Int32, Nullable{Double})"/>
	/// <remarks>This operation is thread-safe.</remarks>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public CStringBuilder ConcurrentInsert(Int32 index, Double? value)
		=> !value.HasValue ? this : new Concurrent(this.GetLock(), this).Insert(index, value.Value);
	/// <inheritdoc cref="CStringBuilder.Insert(Int32, Int16)"/>
	/// <remarks>This operation is thread-safe.</remarks>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public CStringBuilder ConcurrentInsert(Int32 index, Int16 value)
		=> new Concurrent(this.GetLock(), this).Insert(index, value);
	/// <inheritdoc cref="CStringBuilder.Insert(Int32, Nullable{Int16})"/>
	/// <remarks>This operation is thread-safe.</remarks>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public CStringBuilder ConcurrentInsert(Int32 index, Int16? value)
		=> !value.HasValue ? this : new Concurrent(this.GetLock(), this).Insert(index, value.Value);
	/// <inheritdoc cref="CStringBuilder.Insert(Int32, Int32)"/>
	/// <remarks>This operation is thread-safe.</remarks>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public CStringBuilder ConcurrentInsert(Int32 index, Int32 value)
		=> new Concurrent(this.GetLock(), this).Insert(index, value);
	/// <inheritdoc cref="CStringBuilder.Insert(Int32, Nullable{Int32})"/>
	/// <remarks>This operation is thread-safe.</remarks>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public CStringBuilder ConcurrentInsert(Int32 index, Int32? value)
		=> !value.HasValue ? this : new Concurrent(this.GetLock(), this).Insert(index, value.Value);
	/// <inheritdoc cref="CStringBuilder.Insert(Int32, Int64)"/>
	/// <remarks>This operation is thread-safe.</remarks>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public CStringBuilder ConcurrentInsert(Int32 index, Int64 value)
		=> new Concurrent(this.GetLock(), this).Insert(index, value);
	/// <inheritdoc cref="CStringBuilder.Insert(Int32, Nullable{Int64})"/>
	/// <remarks>This operation is thread-safe.</remarks>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public CStringBuilder ConcurrentInsert(Int32 index, Int64? value)
		=> !value.HasValue ? this : new Concurrent(this.GetLock(), this).Insert(index, value.Value);
	/// <inheritdoc cref="CStringBuilder.Insert(Int32, SByte)"/>
	/// <remarks>This operation is thread-safe.</remarks>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public CStringBuilder ConcurrentInsert(Int32 index, SByte value)
		=> new Concurrent(this.GetLock(), this).Insert(index, value);
	/// <inheritdoc cref="CStringBuilder.Insert(Int32, Nullable{SByte})"/>
	/// <remarks>This operation is thread-safe.</remarks>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public CStringBuilder ConcurrentInsert(Int32 index, SByte? value)
		=> !value.HasValue ? this : new Concurrent(this.GetLock(), this).Insert(index, value.Value);
	/// <inheritdoc cref="CStringBuilder.Insert(Int32, Single)"/>
	/// <remarks>This operation is thread-safe.</remarks>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public CStringBuilder ConcurrentInsert(Int32 index, Single value)
		=> new Concurrent(this.GetLock(), this).Insert(index, value);
	/// <inheritdoc cref="CStringBuilder.Insert(Int32, Nullable{Single})"/>
	/// <remarks>This operation is thread-safe.</remarks>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public CStringBuilder ConcurrentInsert(Int32 index, Single? value)
		=> !value.HasValue ? this : new Concurrent(this.GetLock(), this).Insert(index, value.Value);
	/// <inheritdoc cref="CStringBuilder.Insert(Int32, UInt16)"/>
	/// <remarks>This operation is thread-safe.</remarks>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public CStringBuilder ConcurrentInsert(Int32 index, UInt16 value)
		=> new Concurrent(this.GetLock(), this).Insert(index, value);
	/// <inheritdoc cref="CStringBuilder.Insert(Int32, Nullable{UInt16})"/>
	/// <remarks>This operation is thread-safe.</remarks>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public CStringBuilder ConcurrentInsert(Int32 index, UInt16? value)
		=> !value.HasValue ? this : new Concurrent(this.GetLock(), this).Insert(index, value.Value);
	/// <inheritdoc cref="CStringBuilder.Insert(Int32, UInt32)"/>
	/// <remarks>This operation is thread-safe.</remarks>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public CStringBuilder ConcurrentInsert(Int32 index, UInt32 value)
		=> new Concurrent(this.GetLock(), this).Insert(index, value);
	/// <inheritdoc cref="CStringBuilder.Insert(Int32, Nullable{UInt32})"/>
	/// <remarks>This operation is thread-safe.</remarks>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public CStringBuilder ConcurrentInsert(Int32 index, UInt32? value)
		=> !value.HasValue ? this : new Concurrent(this.GetLock(), this).Insert(index, value.Value);
	/// <inheritdoc cref="CStringBuilder.Insert(Int32, UInt64)"/>
	/// <remarks>This operation is thread-safe.</remarks>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public CStringBuilder ConcurrentInsert(Int32 index, UInt64 value)
		=> new Concurrent(this.GetLock(), this).Insert(index, value);
	/// <inheritdoc cref="CStringBuilder.Insert(Int32, Nullable{UInt64})"/>
	/// <remarks>This operation is thread-safe.</remarks>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public CStringBuilder ConcurrentInsert(Int32 index, UInt64? value)
		=> !value.HasValue ? this : new Concurrent(this.GetLock(), this).Insert(index, value.Value);
	/// <inheritdoc cref="CStringBuilder.Insert(Int32, Byte, Boolean)"/>
	/// <remarks>This operation is thread-safe.</remarks>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public CStringBuilder ConcurrentInsert(Int32 index, Byte value, Boolean asNumber = false)
		=> new Concurrent(this.GetLock(), this).Insert(index, value, asNumber);
	/// <inheritdoc cref="CStringBuilder.Insert(Int32, Nullable{Byte}, Boolean)"/>
	/// <remarks>This operation is thread-safe.</remarks>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public CStringBuilder ConcurrentInsert(Int32 index, Byte? value, Boolean asNumber = false)
		=> !value.HasValue ? this : new Concurrent(this.GetLock(), this).Insert(index, value.Value, asNumber);
}