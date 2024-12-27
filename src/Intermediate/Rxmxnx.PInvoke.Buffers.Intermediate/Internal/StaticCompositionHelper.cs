namespace Rxmxnx.PInvoke.Internal;

#if BINARY_SPACES
/// <summary>
/// Helper class to compose statically managed spaces metadata.
/// </summary>
/// <typeparam name="T">The type of items in the buffer.</typeparam>
[ExcludeFromCodeCoverage]
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS3881)]
internal abstract partial class StaticCompositionHelper<T> : IDisposable
{
	/// <summary>
	/// Internal flags.
	/// </summary>
	private readonly BufferTypeMetadata<T>?[] _arr;
	/// <summary>
	/// Space size.
	/// </summary>
	private readonly UInt16 _size;

	/// <summary>
	/// Helper class to compose statically managed spaces metadata.
	/// </summary>
	/// <param name="size">Size of space.</param>
	private StaticCompositionHelper(UInt16 size)
	{
		this._size = size;
		this._arr = ArrayPool<BufferTypeMetadata<T>?>.Shared.Rent(size + 1);
	}

	/// <inheritdoc/>
	public void Dispose() { ArrayPool<BufferTypeMetadata<T>?>.Shared.Return(this._arr, true); }

	/// <summary>
	/// Adds <paramref name="bufferTypeMetadata"/> to current composition.
	/// </summary>
	/// <param name="bufferTypeMetadata">A <see cref="BufferTypeMetadata{T}"/> instance.</param>
	/// <returns>
	/// <see langword="true"/> if current composition was added; otherwise, <see langword="false"/>.
	/// </returns>
	public Boolean Add(BufferTypeMetadata<T> bufferTypeMetadata)
	{
		Int32 index = bufferTypeMetadata.Size - this._size;
		if (this._arr[index] is not null) return false;
		this._arr[index] = bufferTypeMetadata;
		return true;
	}

	/// <summary>
	/// Appends to <paramref name="components"/> all compositions.
	/// </summary>
	/// <param name="components">A dictionary of components.</param>
	public void Append(IDictionary<UInt16, BufferTypeMetadata<T>> components)
	{
		for (Int32 i = this._size; i > 0; i--)
		{
			BufferTypeMetadata<T>? bufferMetadata = this._arr[i - 1];
			if (bufferMetadata is null) continue;
			components.TryAdd(bufferMetadata.Size, bufferMetadata);
			components.TryAdd(bufferMetadata.Components[0].Size, bufferMetadata.Components[0]);
		}
	}
}
#endif