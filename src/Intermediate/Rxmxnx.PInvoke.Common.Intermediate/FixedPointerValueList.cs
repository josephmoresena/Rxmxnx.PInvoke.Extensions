namespace Rxmxnx.PInvoke;

/// <summary>
/// Represents a list of <see cref="FixedPointerValue"/> instances.
/// </summary>
[Preserve(AllMembers = true)]
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
	internal ReadOnlySpan<FixedPointerInfo> Information
	{
#if NETFRAMEWORK || NETSTANDARD2_0
		[SecuritySafeCritical]
#endif
		get;
#if NETFRAMEWORK || NETSTANDARD2_0
		[SecuritySafeCritical]
#endif
		init;
	}
	/// <summary>
	/// Span of <see cref="ReadOnlyFixedMemory"/> instances.
	/// </summary>
	internal Span<ReadOnlyFixedMemory?> Instances
	{
#if NETFRAMEWORK || NETSTANDARD2_0
		[SecuritySafeCritical]
#endif
		get;
#if NETFRAMEWORK || NETSTANDARD2_0
		[SecuritySafeCritical]
#endif
		init;
	}
	/// <summary>
	/// Indicates whether the current list is for read-only memory blocks.
	/// </summary>
	public Boolean IsReadOnly { get; init; }
	/// <summary>
	/// Gets the <see cref="ItemValue"/> at the specified index.
	/// </summary>
	/// <param name="index">The zero-based index of the element to get.</param>
	/// <returns>The <see cref="ItemValue"/> at the specified index.</returns>
	/// <exception cref="IndexOutOfRangeException">
	/// Thrown when the <paramref name="index"/> is out of the range of the list elements.
	/// </exception>
	[IndexerName("Item")]
	public ItemValue this[Int32 index]
	{
		get
		{
			ValidationUtilities.ThrowIfInvalidListIndex(index, this.Information.Length);
			FixedPointerInfo info = this.Information[index];
			return new()
			{
				Value = info.GetValue(this.IsReadOnly, this.Handle), Count = info.Count, SizeOf = info.SizeOf,
			};
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
	/// Returns an enumerator that iterates through the <see cref="FixedPointerValueList"/>.
	/// </summary>
	/// <returns>An enumerator for the current <see cref="FixedPointerValueList"/> instance.</returns>
#if NETSTANDARD2_0_OR_GREATER || NETCOREAPP || NETFRAMEWORK || UAP10_0_16299
	[Browsable(false)]
#endif
	[EditorBrowsable(EditorBrowsableState.Never)]
	public Enumerator GetEnumerator() => new(this);

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
	/// <see cref="FixedPointerValueList"/> item value.
	/// </summary>
	[Preserve(AllMembers = true, Conditional = true)]
	public readonly ref struct ItemValue
#if NET9_0_OR_GREATER
		: IFixedPointer, IWrapper.IBase<FixedPointerValue>
#endif
	{
		/// <summary>
		/// Number of elements on the memory block.
		/// </summary>
		public Int32 Count { get; internal init; }
		/// <summary>
		/// Size of the element type on the memory block.
		/// </summary>
		public Int32 SizeOf { get; internal init; }
		/// <summary>
		/// Current fixed value pointer.
		/// </summary>
		public FixedPointerValue Value { get; internal init; }
		/// <inheritdoc cref="IFixedPointer.Pointer"/>
		public IntPtr Pointer => this.Value.Pointer;
		/// <summary>
		/// The type of memory block.
		/// </summary>
		public Type Type => this.Value.Type ?? typeof(Byte);
		/// <summary>
		/// Indicates whether current memory block is unmanaged.
		/// </summary>
		public Boolean IsUnmanaged => this.Value.Type is null || this.Value.IsUnmanaged;
		/// <summary>
		/// Indicates whether the current instance is read-only.
		/// </summary>
		public Boolean IsReadOnly => this.Value.IsReadOnly;
	}

	/// <summary>
	/// Enumerates the elements of a <see cref="FixedPointerValueList"/>.
	/// </summary>
	[Preserve(AllMembers = true, Conditional = true)]
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
#if NETFRAMEWORK || NETSTANDARD2_0
		[SecuritySafeCritical]
#endif
		private ReadOnlySpan<FixedPointerInfo>.Enumerator _enumerator;

		/// <summary>
		/// Gets the element at the current position of the enumerator.
		/// </summary>
		/// <value>The element in the list at the current position of the enumerator.</value>
		public ItemValue Current
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				FixedPointerInfo info = this._enumerator.Current;
				return new()
				{
					Value = info.GetValue(this._isReadOnly, this._handle), Count = info.Count, SizeOf = info.SizeOf,
				};
			}
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