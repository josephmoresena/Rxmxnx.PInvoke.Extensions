namespace Rxmxnx.PInvoke;

/// <summary>
/// Represents a list of <see cref="FixedPointerValue"/> instances.
/// </summary>
#if !PACKAGE
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
#endif
public readonly ref struct FixedPointerValueList
{
	/// <summary>
	/// Current list handle.
	/// </summary>
	internal FixedValueHandle? Handle { get; init; }
	/// <summary>
	/// Read-only span with fixed pointer information.
	/// </summary>
	internal ReadOnlySpan<FixedPointerInfo> Information { get; init; }
	/// <summary>
	/// Span of <see cref="ReadOnlyFixedMemory"/> instances.
	/// </summary>
	internal Span<ReadOnlyFixedMemory?> Instances { get; init; }
	/// <summary>
	/// Indicates whether the current list is for read-only memory blocks.
	/// </summary>
	public Boolean IsReadOnly { get; init; }
	/// <summary>
	/// Gets the <see cref="FixedPointerValue"/> at the specified index.
	/// </summary>
	/// <param name="index">The zero-based index of the element to get.</param>
	/// <returns>The <see cref="FixedPointerValue"/> at the specified index.</returns>
	/// <exception cref="IndexOutOfRangeException">
	/// Thrown when the <paramref name="index"/> is out of the range of the list elements.
	/// </exception>
	[IndexerName("Item")]
	public FixedPointerValue this[Int32 index]
	{
		get
		{
			ValidationUtilities.ThrowIfInvalidListIndex(index, this.Information.Length);
			return this.Information[index].GetValue(this.IsReadOnly, this.Handle);
		}
	}
	/// <summary>
	/// Gets the total number of elements in the list.
	/// </summary>
	/// <value>The total number of elements in the list.</value>
	public Int32 Count => this.Information.Length;
	/// <summary>
	/// Indicates whether the current list is empty.
	/// </summary>
	/// <value><see langword="true"/> if the list is empty; otherwise, <see langword="false"/>.</value>
	public Boolean IsEmpty => this.Count == 0;

	/// <summary>
	/// Gets the value at the specified index.
	/// </summary>
	/// <param name="index">The zero-based index of the value to get.</param>
	/// <returns>The value at the specified index.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal FixedPointerValue GetValue(Int32 index)
	{
		ValidationUtilities.ThrowIfInvalidListIndex(index, this.Information.Length);
		return this.Information[index].GetValue(this.IsReadOnly, this.Handle);
	}
#if NETSTANDARD2_1 || NETCOREAPP3_0_OR_GREATER
	/// <summary>
	/// Gets the element at the specified index.
	/// </summary>
	/// <param name="index">The zero-based index of the element to get.</param>
	/// <returns>The element at the specified index.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal ReadOnlyFixedMemory GetInstance(Int32 index)
	{
		ValidationUtilities.ThrowIfInvalidListIndex(index, this.Instances.Length);
		return (this.Instances[index] ??= this.Information[index].CreateContext(this.Handle))!;
	}

	/// <summary>
	/// Initializes <see cref="Instances"/> span.
	/// </summary>
	internal void InitializeInstances()
	{
		for (Int32 i = 0; i < this.Instances.Length; i++)
		{
			if (this.Instances[i] is not null)
				continue;
			this.Instances[i] = this.Information[i].CreateContext(this.Handle!);
		}
	}
#endif

	/// <summary>
	/// Enumerates the elements of a <see cref="FixedPointerValueList"/>.
	/// </summary>
	public ref struct Enumerator
	{
		/// <summary>
		/// Indicates whether the current list is for read-only memory blocks.
		/// </summary>
		private readonly Boolean _isReadOnly;
		/// <summary>
		/// Current list handle.
		/// </summary>
		private readonly FixedValueHandle? _handle;
		/// <summary>
		/// Internal enumerator.
		/// </summary>
		private ReadOnlySpan<FixedPointerInfo>.Enumerator _enumerator;

		/// <summary>
		/// Gets the element at the current position of the enumerator.
		/// </summary>
		/// <value>The element in the list at the current position of the enumerator.</value>
		public FixedPointerValue Current
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => this._enumerator.Current.GetValue(this._isReadOnly, this._handle);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Enumerator"/> structure.
		/// </summary>
		/// <param name="valueList">A <see cref="FixedPointerValueList"/> instance.</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal Enumerator(FixedPointerValueList valueList)
		{
			this._isReadOnly = valueList.IsReadOnly;
			this._handle = valueList.Handle;
			this._enumerator = valueList.Information.GetEnumerator();
		}

		/// <summary>
		/// Advances the enumerator to the next element of the <see cref="FixedPointerValue"/>.
		/// </summary>
		/// <returns>
		/// <see langword="true"/> if the enumerator was successfully advanced to the next element;
		/// <see langword="false"/> if the enumerator has passed the end of the list.
		/// </returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Boolean MoveNext() => this._enumerator.MoveNext();
	}
}