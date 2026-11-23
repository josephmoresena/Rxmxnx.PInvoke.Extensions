namespace Rxmxnx.PInvoke.VisualBasic;

/// <summary>
/// Ordinary struct holding the address of a <see cref="FixedPointerValueList"/> variable.
/// </summary>
/// <remarks>
/// A caller that can store the list creates the pointer and passes it to a callee that cannot store a ref struct.
/// Each call rebuilds the list value. Span results use the same transformation checks as a value context.
/// </remarks>
#if NET9_0_OR_GREATER
[Browsable(false)]
[EditorBrowsable(EditorBrowsableState.Never)]
#endif
#if !NET5_0_OR_GREATER
[Preserve(AllMembers = true)]
#endif
[StructLayout(LayoutKind.Sequential)]
#if !PACKAGE
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
#endif
public readonly unsafe struct FixedPointerValueListPtr
{
	/// <summary>
	/// Address of the referenced <see cref="FixedPointerValueList"/>.
	/// </summary>
	private readonly void* _ptr;

	/// <summary>
	/// Constructor.
	/// </summary>
	/// <param name="refList">A managed <see cref="FixedPointerValueList"/> reference.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private FixedPointerValueListPtr(scoped ref FixedPointerValueList refList)
	{
#pragma warning disable CS8500
		fixed (void* ptr = &refList)
			this._ptr = ptr;
#pragma warning restore CS8500
	}

	/// <summary>
	/// Gets the number of blocks in the list.
	/// </summary>
	public Int32 Count => this.GetList().Count;
	/// <summary>
	/// Indicates whether the list is empty.
	/// </summary>
	public Boolean IsEmpty => this.Count == 0;
	/// <summary>
	/// Indicates whether the blocks are read-only.
	/// </summary>
	public Boolean IsReadOnly => this.GetList().IsReadOnly;

	/// <summary>
	/// Creates an unsafe pointer to <paramref name="refList"/>.
	/// </summary>
	/// <param name="refList">The list variable to address.</param>
	/// <returns>A pointer to <paramref name="refList"/>.</returns>
	/// <remarks>The list variable must stay alive for as long as the pointer is used.</remarks>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static FixedPointerValueListPtr CreateUnsafePtr(scoped ref FixedPointerValueList refList)
		=> new(ref refList);

	/// <summary>
	/// Gets the <see cref="FixedPointerValueList.ItemValue"/> at the specified index.
	/// </summary>
	/// <param name="index">The zero-based index of the element to get.</param>
	/// <returns>The <see cref="FixedPointerValueList.ItemValue"/> at the specified index.</returns>
	/// <exception cref="ArgumentOutOfRangeException">
	/// Thrown when <paramref name="index"/> is outside the list.
	/// </exception>
	[IndexerName("Item")]
	public FixedPointerValueList.ItemValue this[Int32 index]
	{
#if NETFRAMEWORK || NETSTANDARD2_0
		[SecuritySafeCritical]
#endif
		get
		{
			FixedPointerValueList list = this.GetList();
			ValidationUtilities.ThrowIfInvalidListIndex(index, list.Count);
			return list[index];
		}
	}

	/// <summary>
	/// Returns the first <see cref="ReadOnlyValPtr{T}"/> value of the block at <paramref name="index"/>.
	/// </summary>
	/// <typeparam name="T">Type of the value to read.</typeparam>
	/// <param name="index">The zero-based block index.</param>
	/// <returns>The first read-only value, or the default value when the block is empty or incompatible.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public ReadOnlyValPtr<T> GetReadOnlyValuePointer<T>(Int32 index)
	{
		if (this._ptr == default && (index <= 0 || index >= this.Count))
			return default;
		ReadOnlyFixedReferenceValue<T> result = default;
		FixedPointerValueList.ItemValue item = this[index];
		if (item.Value.IsValidValueTransformation<T>())
			result = (ReadOnlyFixedReferenceValue<T>)item.Value;
		return result.ValuePointer;
	}
	/// <summary>
	/// Returns the first <see cref="ValPtr{T}"/> value of the block at <paramref name="index"/>.
	/// </summary>
	/// <typeparam name="T">Type of the value to read.</typeparam>
	/// <param name="index">The zero-based block index.</param>
	/// <returns>The first value pointer, or the default value when the block is empty, read-only or incompatible.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public ValPtr<T> GetValuePointer<T>(Int32 index)
	{
		if (this._ptr == default && (index <= 0 || index >= this.Count))
			return default;
		FixedReferenceValue<T> result = default;
		FixedPointerValueList.ItemValue item = this[index];
		if (item.Value.IsValidValueTransformation<T>(false))
			result = (FixedReferenceValue<T>)item.Value;
		return result.ValuePointer;
	}
	/// <summary>
	/// Returns a read-only <typeparamref name="T"/> span over the block at <paramref name="index"/>.
	/// </summary>
	/// <typeparam name="T">Type of the span elements.</typeparam>
	/// <param name="index">The zero-based block index.</param>
	/// <returns>The read-only span, or the default value when the pointer is zero.</returns>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	public ReadOnlySpan<T> GetReadOnlySpan<T>(Int32 index)
		=> this._ptr == default ? default : this[index].GetReadOnlySpan<T>();
	/// <summary>
	/// Returns a <typeparamref name="T"/> span over the block at <paramref name="index"/>.
	/// </summary>
	/// <typeparam name="T">Type of the span elements.</typeparam>
	/// <param name="index">The zero-based block index.</param>
	/// <returns>
	/// The span, or the default value when the pointer is zero or the block cannot be written as
	/// <typeparamref name="T"/>.
	/// </returns>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	public Span<T> GetSpan<T>(Int32 index) => this._ptr == default ? default : this[index].GetSpan<T>();

	/// <summary>
	/// Returns the list addressed by this pointer, or the default value when the pointer is zero.
	/// </summary>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private FixedPointerValueList GetList()
	{
		if (this._ptr == default) return default;
#pragma warning disable CS8500
		return *(FixedPointerValueList*)this._ptr;
#pragma warning restore CS8500
	}
}