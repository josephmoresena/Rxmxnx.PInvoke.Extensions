namespace Rxmxnx.PInvoke.Internal;

/// <summary>
/// A helper class for concatenating UTF-8 text.
/// </summary>
internal abstract partial class BinaryConcatenator<T> : IDisposable
{
	/// <summary>
	/// Gets the current instance's memory stream.
	/// </summary>
	protected MemoryStream Stream { get; }

#if !PACKAGE
	/// <summary>
	/// Gets the separator instance.
	/// </summary>
	public T? Separator => this._separator;
#endif

#if !NETSTANDARD2_1 && !NETCOREAPP2_1_OR_GREATER
	/// <summary>
	/// Parameterless constructor.
	/// </summary>
	protected BinaryConcatenator() : this(default) { }
	/// <summary>
	/// Constructs a new instance of the BinaryConcatenator class.
	/// </summary>
	/// <param name="separator">The separator for the concatenation.</param>
	protected BinaryConcatenator(T? separator)
	{
		this.Stream = new();
		this._separator = separator;
		this.InitializeDelegates();
	}
#endif

	/// <inheritdoc/>
	public void Dispose()
	{
		// Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
		this.Dispose(true);
		GC.SuppressFinalize(this);
	}

	/// <summary>
	/// Writes the given <paramref name="value"/> into the current instance.
	/// </summary>
	/// <param name="value">A read-only span of UTF-8 bytes to be written.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	public void Write(ReadOnlySpan<Byte> value) => this._binaryWrite(this, value);
	/// <summary>
	/// Writes the given <paramref name="value"/> into the current instance.
	/// </summary>
	/// <param name="value">The value of type T to be written.</param>
	public void Write(T? value) => this._write(this, value);
	/// <summary>
	/// Creates a <see cref="CString"/> instance from the UTF-8 encoded text stored in the
	/// current instance.
	/// </summary>
	/// <returns>
	/// A <see cref="CString"/> instance representing the UTF-8 concatenation of the text.
	/// </returns>
	public CString ToCString() => this.ToArray(true) ?? CString.Empty;

	/// <summary>
	/// Writes the given <paramref name="value"/> into the current instance.
	/// </summary>
	/// <param name="value">The value of type T to be written.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	protected abstract void WriteValue(T value);
	/// <summary>
	/// Determines whether the given <paramref name="value"/> is empty.
	/// </summary>
	/// <param name="value">The value of type T to be checked.</param>
	/// <returns>
	/// <see langword="true"/> if the <paramref name="value"/> is empty; otherwise,
	/// <see langword="false"/>.
	/// </returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	protected abstract Boolean IsEmpty([NotNullWhen(false)] T? value);

	/// <summary>
	/// Determines whether the given <paramref name="value"/> of type
	/// <see cref="ReadOnlySpan{Byte}"/> is empty.
	/// </summary>
	/// <param name="value">The <see cref="ReadOnlySpan{Byte}"/> to be checked.</param>
	/// <returns>
	/// <see langword="true"/> if the <paramref name="value"/> is empty; otherwise,
	/// <see langword="false"/>.
	/// </returns>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	protected virtual Boolean IsEmpty(ReadOnlySpan<Byte> value) => value.IsEmpty;
	/// <summary>
	/// Releases the unmanaged resources used by the <see cref="IDisposable"/> current instance,
	/// and optionally releases the managed resources.
	/// </summary>
	/// <param name="disposing">
	/// <see langword="true"/> to release both managed and unmanaged resources; <see langword="false"/> to release only
	/// unmanaged resources.
	/// </param>
	protected virtual void Dispose(Boolean disposing)
	{
		if (this._disposedValue) return;
		if (disposing)
			this.Stream.Dispose();
		this._disposedValue = true;
	}

	/// <summary>
	/// Retrieves the binary data of the UTF-8 text stored in the current instance.
	/// </summary>
	/// <param name="nullTerminated">
	/// Indicates whether the returned array should end with a null (zero) UTF-8 byte,
	/// often used to indicate the end of a string.
	/// </param>
	/// <returns>
	/// The binary data of the UTF-8 text as a <see cref="Byte"/> array.
	/// If the text length is 0, returns <see langword="null"/>.
	/// </returns>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	protected Byte[]? ToArray(Boolean nullTerminated = false)
	{
		Byte[]? result = default;
		if (this.Stream.Length <= 0) return result;
#if NETSTANDARD2_0_OR_GREATER || NETCOREAPP || NETFRAMEWORK || UAP10_0_16299
		ReadOnlySpan<Byte> span = BinaryConcatenator<T>.PrepareUtf8Text(this.Stream.GetBuffer());
#else
		ReadOnlySpan<Byte> span = BinaryConcatenator<T>.PrepareUtf8Text(this.Stream.ToArray());
#endif
		if (span.IsEmpty) return result;
		Int32 resultLength = span.Length + (nullTerminated ? 1 : 0);
		result = CString.CreateByteArray(resultLength);
		span.CopyTo(result);
		if (result.Length > span.Length) result.AsSpan()[span.Length..].Clear();
		return result;
	}
}