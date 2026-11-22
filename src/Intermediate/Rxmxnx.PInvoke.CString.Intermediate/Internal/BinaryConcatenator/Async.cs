#if NETSTANDARD2_1 || NETCOREAPP2_1_OR_GREATER
namespace Rxmxnx.PInvoke.Internal;

/// <summary>
/// A helper class for concatenating UTF-8 text.
/// </summary>
internal abstract partial class BinaryConcatenator<T>
{
	/// <summary>
	/// Gets the cancellation token to monitor for cancellation requests.
	/// </summary>
	protected CancellationToken CancellationToken { get; }

	/// <summary>
	/// Parameterless constructor.
	/// </summary>
	protected BinaryConcatenator() : this(default, default) { }
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

	/// <summary>
	/// Asynchronously writes the given <paramref name="value"/> into the current instance.
	/// </summary>
	/// <param name="value">The value of type T to be written.</param>
	/// <returns>A task that represents the asynchronous write operation.</returns>
	public Task WriteAsync(T? value) => this._writeAsync(this, value);
	/// <summary>
	/// Asynchronously writes the given <paramref name="value"/> into the current instance.
	/// </summary>
	/// <param name="value">The value of type T to be written.</param>
	/// <returns>A task that represents the asynchronous write operation.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	protected abstract Task WriteValueAsync(T value);
}
#endif