namespace Rxmxnx.PInvoke.Internal;

/// <summary>
/// Helper class for managing fixed memory pointer blocks.
/// </summary>
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
internal abstract unsafe partial class FixedPointer : IFixedPointer
{
#pragma warning disable CS8500
	/// <summary>
	/// Size of the memory block in bytes.
	/// </summary>
	private readonly Int32 _binaryLength;
	/// <summary>
	/// Indicates whether the current instance is still valid.
	/// </summary>
	private readonly IMutableWrapper<Boolean> _isValid;
	/// <summary>
	/// Pointer to the fixed memory block.
	/// </summary>
	private readonly void* _ptr;

	/// <summary>
	/// Indicates whether current memory block is unmanaged.
	/// </summary>
	public abstract Boolean IsUnmanaged { get; }
	/// <summary>
	/// The type of memory.
	/// </summary>
	public abstract Type? Type { get; }
	/// <summary>
	/// The offset of the memory.
	/// </summary>
	public abstract Int32 BinaryOffset { get; }
	/// <summary>
	/// Indicates whether the current instance is a function pointer.
	/// </summary>
	public abstract Boolean IsFunction { get; }
	/// <summary>
	/// Indicates whether current memory block is null-referenced or empty.
	/// </summary>
	public Boolean IsNullOrEmpty => this._ptr == IntPtr.Zero.ToPointer() || this._binaryLength - this.BinaryOffset == 0;

	/// <summary>
	/// Size of the memory block in bytes.
	/// </summary>
	public Int32 BinaryLength => this._binaryLength - this.BinaryOffset;
	/// <summary>
	/// Indicates whether the current instance is read-only.
	/// </summary>
	public Boolean IsReadOnly { get; }
	/// <summary>
	/// Indicates whether the current instance is still valid.
	/// </summary>
	public Boolean IsValid => this._isValid.Value;

	/// <summary>
	/// Constructs a new FixedPointer instance pointing to a fixed memory block.
	/// </summary>
	/// <param name="ptr">The pointer to a fixed memory block.</param>
	/// <param name="binaryLength">The size of the memory block in bytes.</param>
	/// <param name="isReadOnly">A Boolean value indicating whether the memory block is read-only.</param>
	/// <remarks>
	/// This constructor sets the validity of the instance to true by default.
	/// </remarks>
	protected FixedPointer(void* ptr, Int32 binaryLength, Boolean isReadOnly)
	{
		this._ptr = ptr;
		this._binaryLength = binaryLength;
		this._isValid = new MutableWrapper<Boolean>(true);
		this.IsReadOnly = isReadOnly;
	}
	/// <summary>
	/// Constructs a new FixedPointer instance pointing to a fixed memory block, with specified validity.
	/// </summary>
	/// <param name="ptr">The pointer to a fixed memory block.</param>
	/// <param name="binaryLength">The size of the memory block in bytes.</param>
	/// <param name="isReadOnly">A Boolean value indicating whether the memory block is read-only.</param>
	/// <param name="isValid">
	/// A mutable wrapper containing a Boolean value indicating whether the current instance
	/// remains valid.
	/// </param>
	/// <remarks>
	/// This constructor allows to set the validity of the instance during the construction of the object.
	/// </remarks>
	protected FixedPointer(void* ptr, Int32 binaryLength, Boolean isReadOnly, IMutableWrapper<Boolean> isValid)
	{
		this._ptr = ptr;
		this._binaryLength = binaryLength;
		this._isValid = isValid;
		this.IsReadOnly = isReadOnly;
	}
	/// <summary>
	/// Constructs a new <see cref="FixedPointer"/> instance using another instance as a template.
	/// </summary>
	/// <param name="pointer">The <see cref="FixedPointer"/> instance to copy data from.</param>
	/// <remarks>
	/// This constructor will copy all the properties of the provided <see cref="FixedPointer"/> instance to the
	/// new instance.
	/// </remarks>
	protected FixedPointer(FixedPointer pointer)
	{
		this._ptr = pointer._ptr;
		this._binaryLength = pointer._binaryLength;
		this._isValid = pointer._isValid;
		this.IsReadOnly = pointer.IsReadOnly;
	}
	/// <summary>
	/// Constructs a new <see cref="FixedPointer"/> instance using another instance as a template and specifying a memory
	/// offset.
	/// </summary>
	/// <param name="pointer">The <see cref="FixedPointer"/> instance to copy data from.</param>
	/// <param name="offset">The offset to be added to the pointer to the memory block.</param>
	/// <remarks>
	/// This constructor will copy all the properties of the provided <see cref="FixedPointer"/> instance to the new instance
	/// and
	/// adjust the pointer and block size according to the provided offset.
	/// </remarks>
	protected FixedPointer(FixedPointer pointer, Int32 offset)
	{
		this._ptr = ((IntPtr)pointer._ptr + offset).ToPointer();
		this._binaryLength = pointer._binaryLength - offset;
		this._isValid = pointer._isValid;
		this.IsReadOnly = pointer.IsReadOnly;
	}

	IntPtr IFixedPointer.Pointer => (IntPtr)this.GetMemoryOffset();

	/// <summary>
	/// Creates a reference of a <typeparamref name="T"/> value over the memory block.
	/// </summary>
	/// <typeparam name="T">Type of the referenced value.</typeparam>
	/// <returns>
	/// A reference to a <typeparamref name="T"/> value over the memory block.
	/// </returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public ref T CreateReference<T>()
#if NET9_0_OR_GREATER
		where T : allows ref struct
#endif
	{
		this.ValidateOperation();
		this.ValidateReferenceSize(typeof(T), sizeof(T));
		this.ValidateTransformation(typeof(T), !RuntimeHelpers.IsReferenceOrContainsReferences<T>());
		return ref Unsafe.AsRef<T>(this._ptr);
	}
	/// <summary>
	/// Creates a read-only reference of a <typeparamref name="T"/> value over
	/// the memory block.
	/// </summary>
	/// <typeparam name="T">Type of the referenced value.</typeparam>
	/// <returns>
	/// A read-only reference to a <typeparamref name="T"/> value over the memory
	/// block.
	/// </returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public ref readonly T CreateReadOnlyReference<T>()
#if NET9_0_OR_GREATER
		where T : allows ref struct
#endif
	{
		this.ValidateOperation(true);
		this.ValidateReferenceSize(typeof(T), sizeof(T));
		this.ValidateTransformation(typeof(T), !RuntimeHelpers.IsReferenceOrContainsReferences<T>());
		return ref Unsafe.AsRef<T>(this._ptr);
	}
	/// <summary>
	/// Creates a <see cref="Span{TValue}"/> instance over the memory block whose
	/// length is <paramref name="length"/>.
	/// </summary>
	/// <typeparam name="TValue">The type of the objects in the span.</typeparam>
	/// <param name="length">Span length.</param>
	/// <returns>A <see cref="Span{TValue}"/> instance over the memory block.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Span<TValue> CreateSpan<TValue>(Int32 length)
	{
		this.ValidateOperation();
		ref TValue refValue = ref Unsafe.AsRef<TValue>(this._ptr);
		return MemoryMarshal.CreateSpan(ref refValue, length);
	}
	/// <summary>
	/// Creates a <see cref="ReadOnlySpan{TValue}"/> instance over the memory block whose
	/// length is <paramref name="length"/>.
	/// </summary>
	/// <typeparam name="TValue">The type of the objects in the read-only span.</typeparam>
	/// <param name="length">Span length.</param>
	/// <returns>A <see cref="ReadOnlySpan{TValue}"/> instance over the memory block.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public ReadOnlySpan<TValue> CreateReadOnlySpan<TValue>(Int32 length)
	{
		this.ValidateOperation(true);
		ref TValue refValue = ref Unsafe.AsRef<TValue>(this._ptr);
		return MemoryMarshal.CreateReadOnlySpan(ref refValue, length);
	}
	/// <summary>
	/// Creates a <see cref="Span{Byte}"/> instance over the memory block.
	/// </summary>
	/// <returns>A <see cref="Span{TValue}"/> instance over the memory block.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Span<Byte> CreateBinarySpan()
	{
		this.ValidateOperation();
		if (!this.IsUnmanaged) return default;
		void* ptr = this.GetMemoryOffset();
		return new(ptr, this.BinaryLength);
	}
	/// <summary>
	/// Creates a <see cref="Span{Object}"/> instance over the memory block.
	/// </summary>
	/// <returns>A <see cref="Span{Object}"/> instance over the memory block.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Span<Object> CreateObjectSpan()
	{
		this.ValidateOperation();
		if (this.Type is null || this.Type.IsValueType) return default;
		void* ptr = this.GetMemoryOffset();
		ref Object refObject = ref Unsafe.AsRef<Object>(ptr);
		return MemoryMarshal.CreateSpan(ref refObject, this.BinaryLength / sizeof(IntPtr));
	}
	/// <summary>
	/// Creates a <see cref="ReadOnlySpan{Byte}"/> instance over the memory block.
	/// </summary>
	/// <returns>A <see cref="ReadOnlySpan{TValue}"/> instance over the memory block.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public ReadOnlySpan<Byte> CreateReadOnlyBinarySpan()
	{
		this.ValidateOperation(true);
		if (!this.IsUnmanaged) return default;
		void* ptr = this.GetMemoryOffset();
		return new(ptr, this.BinaryLength);
	}
	/// <summary>
	/// Creates a <see cref="ReadOnlySpan{Object}"/> instance over the memory block.
	/// </summary>
	/// <returns>A <see cref="ReadOnlySpan{Object}"/> instance over the memory block.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public ReadOnlySpan<Object> CreateReadOnlyObjectSpan()
	{
		this.ValidateOperation(true);
		if (this.Type is null || this.Type.IsValueType) return default;
		void* ptr = this.GetMemoryOffset();
		ref Object refObject = ref Unsafe.AsRef<Object>(ptr);
		return MemoryMarshal.CreateReadOnlySpan(ref refObject, this.BinaryLength / sizeof(IntPtr));
	}
	/// <summary>
	/// Creates a <typeparamref name="TDelegate"/> instance over the memory block.
	/// </summary>
	/// <typeparam name="TDelegate">A <see cref="Delegate"/> type.</typeparam>
	/// <returns>A <typeparamref name="TDelegate"/> instance over the memory block.</returns>
	public TDelegate CreateDelegate<TDelegate>() where TDelegate : Delegate
	{
		this.ValidateFunctionOperation();
		return Marshal.GetDelegateForFunctionPointer<TDelegate>(new(this._ptr));
	}

	/// <summary>
	/// Invalidates current context.
	/// </summary>
	public virtual void Unload()
	{
		if (this._ptr == default && this._binaryLength == 0) return;
		this._isValid.Value = false;
	}

	/// <summary>
	/// Validates any operation over the fixed memory block.
	/// </summary>
	/// <param name="isReadOnly">Indicates whether current operation is read-only one.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	protected void ValidateOperation(Boolean isReadOnly = false)
	{
		ValidationUtilities.ThrowIfFunctionPointer(this.IsFunction);
		ValidationUtilities.ThrowIfInvalidPointer(this._isValid);
		ValidationUtilities.ThrowIfReadOnlyPointer(isReadOnly, this.IsReadOnly);
	}
	/// <summary>
	/// Validates any unmanaged operation over the fixed value type memory block.
	/// </summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	protected void ValidateUnmanagedOperation()
	{
		ValidationUtilities.ThrowIfNotUnmanagedType(this.Type, this.IsUnmanaged);
	}
	/// <summary>
	/// Validates any managed object operation over the fixed value type memory block.
	/// </summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	protected void ValidateReferenceOperation() { ValidationUtilities.ThrowIfNotReferenceType(this.Type); }
	/// <summary>
	/// Validates any transformation operation over the fixed memory block.
	/// </summary>
	/// <param name="type">Destination type.</param>
	/// <param name="unmanagedType">Indicates whether <paramref name="type"/> is unmanaged.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	protected void ValidateTransformation(Type type, Boolean unmanagedType)
	{
		if (type == this.Type) return;
		ValidationUtilities.ThrowIfInvalidTransformation(this.Type, this.IsUnmanaged, type, unmanagedType);
	}
	/// <summary>
	/// Retrieves the number of <paramref name="sizeOf"/> items that can be
	/// referenced into the fixed memory block.
	/// </summary>
	/// <param name="sizeOf">Type size in bytes.</param>
	/// <returns>
	/// The number of <paramref name="sizeOf"/> items that can be referenced into the
	/// fixed memory block.
	/// </returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	protected Int32 GetCount(Int32 sizeOf) => this._binaryLength / sizeOf;
	/// <summary>
	/// Validates the size of the referenced value type from current instance.
	/// </summary>
	/// <param name="typeOf">CLR Type.</param>
	/// <param name="sizeOf">Type size in bytes.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	protected void ValidateReferenceSize(Type typeOf, Int32 sizeOf)
		=> ValidationUtilities.ThrowIfInvalidRefTypePointer(this._binaryLength, typeOf, sizeOf);

	/// <summary>
	/// Validates any operation over the fixed function pointer.
	/// </summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void ValidateFunctionOperation()
	{
		ValidationUtilities.ThrowIfNotFunctionPointer(this.IsFunction);
		ValidationUtilities.ThrowIfInvalidPointer(this._isValid);
	}
	/// <summary>
	/// Retrieves the memory offset for current instance.
	/// </summary>
	/// <returns>Pointer to offset memory.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void* GetMemoryOffset()
	{
		IntPtr ptr = new(this._ptr);
		IntPtr result = ptr + this.BinaryOffset;
		return result.ToPointer();
	}
#pragma warning restore CS8500
}