namespace Rxmxnx.PInvoke.Internal;

internal partial class BinaryConcatenator<T>
{
	/// <summary>
	/// Provides a token that allows for monitoring and responding to cancellation
	/// requests.
	/// </summary>
	private readonly CancellationToken _cancellationToken;

	/// <summary>
	/// The separator value to use between elements when writing.
	/// </summary>
	private readonly T? _separator;

	/// <summary>
	/// Delegate that handles the writing of UTF-8 bytes into the current instance.
	/// </summary>
	private BinaryWriteDelegate _binaryWrite = default!;
	/// <summary>
	/// Indicates whether the current instance has been disposed.
	/// </summary>
	private Boolean _disposedValue;
	/// <summary>
	/// Delegate that handles the writing of a specific value into the current
	/// instance.
	/// </summary>
	private WriteDelegate _write = default!;
	/// <summary>
	/// Delegate that handles the writing of a specific value into the current
	/// instance asynchronously.
	/// </summary>
	private WriteAsyncDelegate _writeAsync = default!;

	/// <summary>
	/// Initializes the delegates of the current instance based on the separator's
	/// presence.
	/// </summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void InitializeDelegates()
	{
		if (!this.IsEmpty(this._separator))
		{
			this._binaryWrite = this.InitialWrite;
			this._write = this.InitialWrite;
			this._writeAsync = this.InitialWriteAsync;
		}
		else
		{
			this._binaryWrite = this.FinalWrite;
			this._write = this.FinalWrite;
			this._writeAsync = this.FinalWriteAsync;
		}
	}
	/// <summary>
	/// Writes the <paramref name="value"/> to the current instance.
	/// </summary>
	/// <param name="value">The UTF-8 bytes to write.</param>
	private void WriteValue(ReadOnlySpan<Byte> value) => this.Stream.Write(value);
	/// <summary>
	/// Writes the <paramref name="value"/> to the current instance and updates the writing
	/// delegates for subsequent writes with a separator.
	/// </summary>
	/// <param name="value">The UTF-8 bytes to write.</param>
	/// <remarks>This method is used when the separator is not empty.</remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void InitialWrite(ReadOnlySpan<Byte> value)
	{
		this.WriteValue(value);
		this._binaryWrite = this.WriteWithSeparator;
		this._write = this.WriteWithSeparator;
		this._writeAsync = this.WriteWithSeparatorAsync;
	}
	/// <summary>
	/// Writes the <paramref name="value"/> to the current instance and updates the writing delegates
	/// for subsequent writes with a separator.
	/// </summary>
	/// <param name="value">The value to write.</param>
	/// <remarks>This method is used when the separator is not empty.</remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void InitialWrite(T? value)
	{
		this.WriteValue(value!);
		this._binaryWrite = this.WriteWithSeparator;
		this._write = this.WriteWithSeparator;
		this._writeAsync = this.WriteWithSeparatorAsync;
	}
	/// <summary>
	/// Writes the <paramref name="value"/> to the current instance preceded by the separator.
	/// This method is used when subsequent writes require a separator.
	/// </summary>
	/// <param name="value">The UTF-8 bytes to write.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void WriteWithSeparator(ReadOnlySpan<Byte> value)
	{
		this.WriteValue(this._separator!);
		this.WriteValue(value);
	}
	/// <summary>
	/// Writes the <paramref name="value"/> to the current instance, preceded by the separator.
	/// This method is used when subsequent writes require a separator.
	/// </summary>
	/// <param name="value">The value to write.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void WriteWithSeparator(T? value)
	{
		this.WriteValue(this._separator!);
		this.WriteValue(value!);
	}
	/// <summary>
	/// Writes the <paramref name="value"/> to the current instance.
	/// </summary>
	/// <param name="value">The UTF-8 bytes to write.</param>
	/// <remarks>
	/// This method is used when the value to write doesn't require a preceding separator.
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void FinalWrite(ReadOnlySpan<Byte> value)
	{
		if (!this.IsEmpty(value))
			this.WriteValue(value);
	}
	/// <summary>
	/// Writes the <paramref name="value"/> to the current instance.
	/// </summary>
	/// <param name="value">The value to write.</param>
	/// <remarks>
	/// This method is used when the value to write doesn't require a preceding separator.
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void FinalWrite(T? value)
	{
		if (!this.IsEmpty(value))
			this.WriteValue(value);
	}
	/// <summary>
	/// Asynchronously writes the <paramref name="value"/> to the current instance and
	/// sets the delegates for subsequent writes that require a separator.
	/// </summary>
	/// <param name="value">The value to write.</param>
	/// <returns>A <see cref="Task"/> that represents the asynchronous write operation.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private async Task InitialWriteAsync(T? value)
	{
		if (!this.IsEmpty(value))
		{
			await this.WriteValueAsync(value);
			this._binaryWrite = this.WriteWithSeparator;
			this._write = this.WriteWithSeparator;
			this._writeAsync = this.WriteWithSeparatorAsync;
		}
	}
	/// <summary>
	/// Asynchronously writes the <paramref name="value"/> to the current instance and
	/// sets the delegates for subsequent writes that require a separator.
	/// </summary>
	/// <param name="value">The value to write.</param>
	/// <returns>A <see cref="Task"/> that represents the asynchronous write operation.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private async Task WriteWithSeparatorAsync(T? value)
	{
		if (!this.IsEmpty(value))
		{
			await this.WriteValueAsync(this._separator!);
			await this.WriteValueAsync(value);
		}
	}
	/// <summary>
	/// Asynchronously writes the <paramref name="value"/> to the current instance.
	/// </summary>
	/// <param name="value">The value to write.</param>
	/// <returns>A <see cref="Task"/> that represents the asynchronous write operation.</returns>
	/// <remarks>
	/// This method is used when the value to write doesn't require a preceding separator.
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private async Task FinalWriteAsync(T? value)
	{
		if (!this.IsEmpty(value))
			await this.WriteValueAsync(value);
	}

	/// <summary>
	/// Delegate that defines a method to write UTF-8 bytes into the current instance.
	/// </summary>
	/// <param name="value">The span of UTF-8 bytes to write.</param>
	private delegate void BinaryWriteDelegate(ReadOnlySpan<Byte> value);

	/// <summary>
	/// Delegate that defines a method to write the UTF-8 bytes of a specific value into
	/// the current instance.
	/// </summary>
	/// <param name="value">The value to write its UTF-8 bytes representation.</param>
	private delegate void WriteDelegate(T? value);

	/// <summary>
	/// Delegate that defines a method to write the UTF-8 bytes of a specific value into the
	/// current instance asynchronously.
	/// </summary>
	/// <param name="value">The value to write its UTF-8 bytes representation.</param>
	/// <returns>A <see cref="Task"/> that represents the asynchronous write operation.</returns>
	private delegate Task WriteAsyncDelegate(T? value);
}