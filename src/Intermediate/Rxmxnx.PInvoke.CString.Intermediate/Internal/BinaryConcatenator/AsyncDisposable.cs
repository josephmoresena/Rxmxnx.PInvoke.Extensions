#if NETSTANDARD2_1 || NETCOREAPP2_0_OR_GREATER || NET461_OR_GREATER || UAP10_0_16299
namespace Rxmxnx.PInvoke.Internal;

/// <summary>
/// A helper class for concatenating UTF-8 text.
/// </summary>
internal abstract partial class BinaryConcatenator<T> : IAsyncDisposable
{
	/// <inheritdoc/>
	public async ValueTask DisposeAsync()
	{
		await this.DisposeAsync(true);
		GC.SuppressFinalize(this);
	}

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
				if (this.Stream is IAsyncDisposable ad)
					await ad.DisposeAsync();
				else
					this.Stream.Dispose();
			}
#endif
			this._disposedValue = true;
		}
	}
}
#endif