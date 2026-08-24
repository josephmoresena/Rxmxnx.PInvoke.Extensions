namespace Rxmxnx.PInvoke.Internal;

/// <summary>
/// A helper class for concatenating UTF-8 text.
/// </summary>
internal abstract partial class BinaryConcatenator<T> : IDisposable
#if NETSTANDARD2_1 || NETCOREAPP2_0_OR_GREATER || NET461_OR_GREATER || UAP10_0_16299
	, IAsyncDisposable
#endif
{
	/// <summary>
	/// Gets the current instance's memory stream.
	/// </summary>
	protected MemoryStream Stream { get; }
	/// <summary>
	/// Gets the cancellation token to monitor for cancellation requests.
	/// </summary>
	protected CancellationToken CancellationToken { get; }

	/// <summary>
	/// Gets the separator instance.
	/// </summary>
	public T? Separator => this._separator;
	/// <summary>
	/// Constructs a new instance of the BinaryConcatenator class.
	/// </summary>
	/// <param name="separator">The separator for the concatenation.</param>
	/// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
	protected BinaryConcatenator(T? separator, CancellationToken cancellationToken)
	{
		this.Stream = new();
		this._separator = separator;
		this.CancellationToken = cancellationToken;
		this.InitializeDelegates();
	}
#if NETSTANDARD2_1 || NETCOREAPP2_0_OR_GREATER || NET461_OR_GREATER || UAP10_0_16299
	/// <inheritdoc/>
	public async ValueTask DisposeAsync()
	{
		await this.DisposeAsync(true);
		GC.SuppressFinalize(this);
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
	public void Write(ReadOnlySpan<Byte> value) => this._binaryWrite(this, value);
	/// <summary>
	/// Writes the given <paramref name="value"/> into the current instance.
	/// </summary>
	/// <param name="value">The value of type T to be written.</param>
	public void Write(T? value) => this._write(this, value);
#if NETSTANDARD2_1 || NETCOREAPP2_1_OR_GREATER
	/// <summary>
	/// Asynchronously writes the given <paramref name="value"/> into the current instance.
	/// </summary>
	/// <param name="value">The value of type T to be written.</param>
	/// <returns>A task that represents the asynchronous write operation.</returns>
	public Task WriteAsync(T? value) => this._writeAsync(this, value);
#endif
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
#if NETSTANDARD2_1 || NETCOREAPP2_1_OR_GREATER
	/// <summary>
	/// Asynchronously writes the given <paramref name="value"/> into the current instance.
	/// </summary>
	/// <param name="value">The value of type T to be written.</param>
	/// <returns>A task that represents the asynchronous write operation.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	protected abstract Task WriteValueAsync(T value);
#endif
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
#if NETSTANDARD2_1 || NETCOREAPP2_0_OR_GREATER || NET461_OR_GREATER || UAP10_0_16299
	/// <summary>
	/// Asynchronously releases the unmanaged resources used by the <see cref="IDisposable"/>
	/// current instance, and optionally releases the managed resources.
	/// </summary>
	/// <param name="disposing">
	/// <see langword="true"/> to release both managed and unmanaged resources;
	/// <see langword="false"/> to release only unmanaged resources.
	/// </param>
	/// <returns>A task that represents the asynchronous dispose operation.</returns>
	protected virtual async ValueTask DisposeAsync(Boolean disposing)
	{
		if (!this._disposedValue)
		{
			if (disposing)
#if NETSTANDARD2_1 || NETCOREAPP3_0_OR_GREATER
				await this.Stream.DisposeAsync();
#else
			{
				if (this.Stream is IAsyncDisposable { } ad)
					await ad.DisposeAsync();
				else
					this.Stream.Dispose();
			}
#endif
			this._disposedValue = true;
		}
	}
#endif

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