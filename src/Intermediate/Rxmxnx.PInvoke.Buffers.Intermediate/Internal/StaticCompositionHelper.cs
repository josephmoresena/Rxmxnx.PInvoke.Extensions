namespace Rxmxnx.PInvoke.Internal;

internal sealed class StaticCompositionHelper<T>(UInt16 size) : IDisposable
{
	/// <summary>
	/// Internal flags.
	/// </summary>
	private readonly BufferTypeMetadata<T>?[] _arr = ArrayPool<BufferTypeMetadata<T>?>.Shared.Rent(size + 1);

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
		Int32 index = bufferTypeMetadata.Size - size;
		if (this._arr[index] is not null) return false;
		this._arr[index] = bufferTypeMetadata;
		return true;
	}
	/// <summary>
	/// Indicates whether at least one composition is required.
	/// </summary>
	/// <param name="aSize">Lower component size.</param>
	/// <param name="bSize">Upper pure component size.</param>
	/// <returns>
	/// <see langword="true"/> if at least one append is required; otherwise,
	/// <see langword="false"/>.
	/// </returns>
	public Boolean IsCompositionRequired(UInt16 aSize, UInt16 bSize)
	{
		UInt16 acc = 0;
		aSize /= 2;
		while (aSize > 0)
		{
			if (this._arr[bSize + aSize - 2] is null) return true;
			acc += aSize;
			if (acc != aSize && this._arr[bSize + acc - 2] is null) return true;
			aSize /= 2;
		}
		return false;
	}

	/// <summary>
	/// Appends to <paramref name="components"/> all compositions.
	/// </summary>
	/// <param name="components">A dictionary of components.</param>
	public void Append(IDictionary<UInt16, BufferTypeMetadata<T>> components)
	{
		for (Int32 i = size; i > 0; i--)
		{
			BufferTypeMetadata<T>? bufferMetadata = this._arr[i - 1];
			if (bufferMetadata is null) continue; // TODO: Remove when 2^15-1
			components.TryAdd(bufferMetadata.Size, bufferMetadata);
			components.TryAdd(bufferMetadata.Components[0].Size, bufferMetadata.Components[0]);
		}
	}
}