namespace Rxmxnx.PInvoke;

/// <summary>
/// Ref-struct representing a pointer to a fixed block of memory.
/// </summary>
public readonly ref struct FixedPointerValue
#if NET9_0_OR_GREATER
	: IFixedPointer
#endif
{
	/// <summary>
	/// Internal pointer.
	/// </summary>
	private readonly IntPtr _ptr;
	/// <summary>
	/// The offset of the memory.
	/// </summary>
	private readonly Int32 _offset;
	/// <summary>
	/// Internal byte count.
	/// </summary>
	private readonly Int32 _byteCount;

	/// <inheritdoc cref="IFixedPointer.Pointer"/>
	public IntPtr Pointer
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			this.ValidateOperation();
			return this._ptr + this._offset;
		}
	}

	/// <summary>
	/// Size of the memory block.
	/// </summary>
	internal Int32 Size => this._byteCount - this._offset;
	/// <summary>
	/// Indicates whether current memory block is null-referenced or empty.
	/// </summary>
	internal Boolean IsNullOrEmpty => this._ptr == IntPtr.Zero || this.Size == 0;
	/// <summary>
	/// Current memory block handle.
	/// </summary>
	internal FixedValueHandle? Handle { get; init; }
	/// <summary>
	/// The type of memory block.
	/// </summary>
	internal Type? Type { get; init; }
	/// <summary>
	/// Indicates whether current memory block is unmanaged.
	/// </summary>
	internal Boolean IsUnmanaged { get; init; }
	/// <summary>
	/// Indicates whether the current instance is read-only.
	/// </summary>
	internal Boolean IsReadOnly { get; init; }

	/// <summary>
	/// Constructor.
	/// </summary>
	/// <param name="ptr">Unmanaged fixed pointer.</param>
	/// <param name="byteCount">Memory block byte count.</param>
	internal FixedPointerValue(IntPtr ptr, Int32 byteCount)
	{
		this._ptr = ptr;
		this._byteCount = byteCount;
	}

	/// <summary>
	/// Constructor.
	/// </summary>
	/// <param name="value">Original <see cref="FixedPointerValue"/> instance.</param>
	/// <param name="offset">Memory offset to apply.</param>
	private FixedPointerValue(FixedPointerValue value, Int32 offset)
	{
		this._ptr = value._ptr;
		this._byteCount = value._byteCount;
		this._offset = offset;

		this.Handle = value.Handle;
		this.Type = value.Type;
		this.IsUnmanaged = value.IsUnmanaged;
		this.IsReadOnly = value.IsReadOnly;
	}

	/// <summary>
	/// Attempts to create a read-only binary context value from the current instance.
	/// </summary>
	/// <param name="binaryContext">Output. Created instance.</param>
	/// <returns>
	/// <see langword="true"/> if the current instance was successfully created; otherwise, <see langword="false"/>.
	/// </returns>
	public Boolean TryGetReadOnlyBinaryContext(out ReadOnlyFixedContextValue<Byte> binaryContext)
	{
		if (!this.IsNullOrEmpty && this.IsUnmanaged && this.Type is not { IsValueType: false, })
		{
			binaryContext = new(this);
			return true;
		}
		binaryContext = default;
		return false;
	}
	/// <summary>
	/// Attempts to create a read-only object context value from the current instance.
	/// </summary>
	/// <param name="objectContext">Output. Created instance.</param>
	/// <returns>
	/// <see langword="true"/> if the current instance was successfully created; otherwise, <see langword="false"/>.
	/// </returns>
	public Boolean TryGetReadOnlyObjectContext(out ReadOnlyFixedContextValue<Object> objectContext)
	{
		if (!this.IsNullOrEmpty && this.IsUnmanaged && this.Type is { IsValueType: true, })
		{
			objectContext = new(this);
			return true;
		}
		objectContext = default;
		return false;
	}
	/// <summary>
	/// Attempts to create a read-only binary context value from the current instance.
	/// </summary>
	/// <param name="binaryContext">Output. Created instance.</param>
	/// <returns>
	/// <see langword="true"/> if the current instance was successfully created; otherwise, <see langword="false"/>.
	/// </returns>
	public Boolean TryBinaryContext(out FixedContextValue<Byte> binaryContext)
	{
		if (!this.IsNullOrEmpty && !this.IsReadOnly && this.IsUnmanaged && this.Type is not { IsValueType: false, })
		{
			binaryContext = new(this);
			return true;
		}
		binaryContext = default;
		return false;
	}
	/// <summary>
	/// Attempts to create a read-only object context value from the current instance.
	/// </summary>
	/// <param name="objectContext">Output. Created instance.</param>
	/// <returns>
	/// <see langword="true"/> if the current instance was successfully created; otherwise, <see langword="false"/>.
	/// </returns>
	public Boolean TryObjectContext(out FixedContextValue<Object> objectContext)
	{
		if (!this.IsNullOrEmpty && !this.IsReadOnly && this.IsUnmanaged && this.Type is { IsValueType: true, })
		{
			objectContext = new(this);
			return true;
		}
		objectContext = default;
		return false;
	}

	/// <summary>
	/// Creates a new <see cref="FixedPointerValue"/> from current instance applying <paramref name="offset"/>.
	/// </summary>
	/// <param name="offset">The memory offset to apply.</param>
	/// <returns>A new <see cref="FixedPointerValue"/> instance.</returns>
	internal FixedPointerValue CreateOffset(Int32 offset) => new(this, offset);
	/// <summary>
	/// Validates any operation over the fixed memory block.
	/// </summary>
	/// <param name="isReadOnly">Indicates whether current operation is read-only one.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal void ValidateOperation(Boolean isReadOnly = false)
	{
		if (this.Handle is not null)
			ValidationUtilities.ThrowIfInvalidPointer(this.Handle);
		ValidationUtilities.ThrowIfReadOnlyPointer(isReadOnly, this.IsReadOnly);
	}
	/// <summary>
	/// Validates the size of the referenced value type from current instance.
	/// </summary>
	/// <param name="typeOf">CLR Type.</param>
	/// <param name="sizeOf">Type size in bytes.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal void ValidateReferenceSize(Type typeOf, Int32 sizeOf)
		=> ValidationUtilities.ThrowIfInvalidRefTypePointer(this.Size, typeOf, sizeOf);
	/// <summary>
	/// Validates any transformation operation over the fixed memory block.
	/// </summary>
	/// <param name="type">Destination type.</param>
	/// <param name="unmanagedType">Indicates whether <paramref name="type"/> is unmanaged.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal void ValidateTransformation(Type type, Boolean unmanagedType)
	{
		if (type == this.Type) return;
		ValidationUtilities.ThrowIfInvalidTransformation(this.Type, this.IsUnmanaged, type, unmanagedType);
	}

#if NET9_0_OR_GREATER
	/// <summary>
	/// Retrieves the <see cref="IMutableWrapper{Boolean}"/> instance for <see cref="FixedPointer"/> instances.
	/// </summary>
	/// <typeparam name="TPointer">Type of the <see cref="IFixedPointer"/>.</typeparam>
	/// <returns>The <see cref="IMutableWrapper{Boolean}"/> instance for <see cref="FixedPointer"/> instances.</returns>
	internal static IMutableWrapper<Boolean> GetValidationObject<TPointer>(TPointer pointer)
		where TPointer : struct, IFixedPointerOperators<TPointer>, allows ref struct
	{
		FixedPointerValue value = pointer;
		if (value.Handle is null)
			ValidationUtilities.ThrowIfNotObject(typeof(TPointer));
		return value.Handle!;
	}
#endif
}