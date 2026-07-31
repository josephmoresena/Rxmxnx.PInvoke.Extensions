namespace Rxmxnx.PInvoke;

public sealed partial class CStringBuilder
{
	/// <inheritdoc cref="CStringBuilder.AppendLine()"/>
	/// <remarks>This operation is thread-safe.</remarks>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public CStringBuilder ConcurrentAppendLine() => new Concurrent(this.GetLock(), this).Append(CString.NewLine);
	/// <inheritdoc cref="CStringBuilder.AppendLine(String)"/>
	/// <remarks>This operation is thread-safe.</remarks>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public CStringBuilder ConcurrentAppendLine(String? value) => new Concurrent(this.GetLock(), this).AppendLine(value);
	/// <inheritdoc cref="CStringBuilder.AppendLine(Char[])"/>
	/// <remarks>This operation is thread-safe.</remarks>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public CStringBuilder ConcurrentAppendLine(Char[]? value)
		=> new Concurrent(this.GetLock(), this).AppendLine(value.AsSpan());
	/// <inheritdoc cref="CStringBuilder.AppendLine(CString)"/>
	/// <remarks>This operation is thread-safe.</remarks>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public CStringBuilder ConcurrentAppendLine(CString? value)
		=> new Concurrent(this.GetLock(), this).AppendLine(value);
	/// <inheritdoc cref="CStringBuilder.AppendLine(Byte[])"/>
	/// <remarks>This operation is thread-safe.</remarks>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public CStringBuilder ConcurrentAppendLine(Byte[]? value)
		=> new Concurrent(this.GetLock(), this).AppendLine(value.AsSpan());
	/// <inheritdoc cref="CStringBuilder.AppendLine(ReadOnlySpan{Char})"/>
	/// <remarks>This operation is thread-safe.</remarks>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public CStringBuilder ConcurrentAppendLine(ReadOnlySpan<Char> value)
		=> new Concurrent(this.GetLock(), this).AppendLine(value);
	/// <inheritdoc cref="CStringBuilder.AppendLine(ReadOnlySpan{Byte})"/>
	/// <remarks>This operation is thread-safe.</remarks>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public CStringBuilder ConcurrentAppendLine(ReadOnlySpan<Byte> value)
		=> new Concurrent(this.GetLock(), this).AppendLine(value);
	/// <inheritdoc cref="CStringBuilder.AppendLine(ReadOnlySequence{Byte})"/>
	/// <remarks>This operation is thread-safe.</remarks>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public CStringBuilder ConcurrentAppendLine(ReadOnlySequence<Byte> value)
		=> new Concurrent(this.GetLock(), this).AppendLine(value);
}