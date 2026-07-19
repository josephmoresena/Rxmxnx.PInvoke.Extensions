namespace Rxmxnx.PInvoke.Internal;

/// <summary>
/// Represents a list of <see cref="FixedPointerValue"/> instances.
/// </summary>
#if !PACKAGE
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
#endif
internal readonly ref struct FixedPointerValueList
{
	/// <summary>
	/// Indicates whether the current list is for read-only memory blocks.
	/// </summary>
	public Boolean IsReadOnly { get; init; }
	/// <summary>
	/// Current list handle.
	/// </summary>
	public FixedValueHandle? Handle { get; init; }
	/// <summary>
	/// Read-only span with fixed pointer information.
	/// </summary>
	public ReadOnlySpan<FixedPointerInfo> Information { get; init; }
	/// <summary>
	/// Span of <see cref="ReadOnlyFixedMemory"/> instances.
	/// </summary>
	public Span<ReadOnlyFixedMemory?> Instances { get; init; }

	/// <summary>
	/// Gets the value at the specified index.
	/// </summary>
	/// <param name="index">The zero-based index of the value to get.</param>
	/// <returns>The value at the specified index.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public FixedPointerValue GetValue(Int32 index)
	{
		ValidationUtilities.ThrowIfInvalidListIndex(index, this.Information.Length);
		return this.Information[index].GetValue(this.IsReadOnly, this.Handle!);
	}
	/// <summary>
	/// Gets the element at the specified index.
	/// </summary>
	/// <param name="index">The zero-based index of the element to get.</param>
	/// <returns>The element at the specified index.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public ReadOnlyFixedMemory GetInstance(Int32 index)
	{
		ValidationUtilities.ThrowIfInvalidListIndex(index, this.Instances.Length);
		return (this.Instances[index] ??= this.Information[index].CreateContext(this.Handle!))!;
	}

	/// <summary>
	/// Initializes <see cref="Instances"/> span.
	/// </summary>
	public void InitializeInstances()
	{
		for (Int32 i = 0; i < this.Instances.Length; i++)
		{
			if (this.Instances[i] is not null)
				continue;
			this.Instances[i] = this.Information[i].CreateContext(this.Handle!);
		}
	}
}