namespace Rxmxnx.PInvoke.Internal;

/// <summary>
/// Helper class for managing fixed memory pointer blocks.
/// </summary>
internal abstract unsafe partial class FixedPointer : IFixedPointer, IEquatable<FixedPointer>
{
	/// <summary>
	/// Size of the memory block in bytes.
	/// </summary>
	private readonly Int32 _binaryLength;
	/// <summary>
	/// Indicates whether the memory block is read-only.
	/// </summary>
	private readonly Boolean _isReadOnly;
	/// <summary>
	/// Indicates whether the current instance is still valid.
	/// </summary>
	private readonly IMutableWrapper<Boolean> _isValid;
	/// <summary>
	/// Pointer to the fixed memory block.
	/// </summary>
	private readonly void* _ptr;

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
	/// Size of the memory block in bytes.
	/// </summary>
	public Int32 BinaryLength => this._binaryLength - this.BinaryOffset;
	/// <summary>
	/// Indicates whether the current instance is read-only.
	/// </summary>
	public Boolean IsReadOnly => this._isReadOnly;
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
		this._isReadOnly = isReadOnly;
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
		this._isReadOnly = isReadOnly;
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
		this._isReadOnly = pointer._isReadOnly;
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
		this._isReadOnly = pointer._isReadOnly;
	}
	/// <inheritdoc/>
	public virtual Boolean Equals(FixedPointer? other)
		=> other is not null && this.GetMemoryOffset() == other.GetMemoryOffset() &&
			this.BinaryLength == other.BinaryLength && this._isReadOnly == other._isReadOnly;

	IntPtr IFixedPointer.Pointer => (IntPtr)this.GetMemoryOffset();

	/// <summary>
	/// Creates a reference of a <typeparamref name="TValue"/> value over the memory block.
	/// </summary>
	/// <typeparam name="TValue">Type of the referenced value.</typeparam>
	/// <returns>
	/// A reference to a <typeparamref name="TValue"/> value over the memory block.
	/// </returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public ref TValue CreateReference<TValue>() where TValue : unmanaged
	{
		this.ValidateOperation();
		this.ValidateReferenceSize<TValue>();
		return ref Unsafe.AsRef<TValue>(this._ptr);
	}
	/// <summary>
	/// Creates a read-only reference of a <typeparamref name="TValue"/> value over
	/// the memory block.
	/// </summary>
	/// <typeparam name="TValue">Type of the referenced value.</typeparam>
	/// <returns>
	/// A read-only reference to a <typeparamref name="TValue"/> value over the memory
	/// block.
	/// </returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public ref readonly TValue CreateReadOnlyReference<TValue>() where TValue : unmanaged
	{
		this.ValidateOperation(true);
		ValidationUtilities.ThrowIfInvalidRefTypePointer<TValue>(this._binaryLength);
		this.ValidateReferenceSize<TValue>();
		return ref Unsafe.AsRef<TValue>(this._ptr);
	}
	/// <summary>
	/// Creates a <see cref="Span{TValue}"/> instance over the memory block whose
	/// length is <paramref name="length"/>.
	/// </summary>
	/// <typeparam name="TValue">The type of the objects in the span.</typeparam>
	/// <param name="length">Span length.</param>
	/// <returns>A <see cref="Span{TValue}"/> instance over the memory block.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Span<TValue> CreateSpan<TValue>(Int32 length) where TValue : unmanaged
	{
		this.ValidateOperation();
		return new(this._ptr, length);
	}
	/// <summary>
	/// Creates a <see cref="ReadOnlySpan{TValue}"/> instance over the memory block whose
	/// length is <paramref name="length"/>.
	/// </summary>
	/// <typeparam name="TValue">The type of the objects in the read-only span.</typeparam>
	/// <param name="length">Span length.</param>
	/// <returns>A <see cref="ReadOnlySpan{TValue}"/> instance over the memory block.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public ReadOnlySpan<TValue> CreateReadOnlySpan<TValue>(Int32 length) where TValue : unmanaged
	{
		this.ValidateOperation(true);
		return new(this._ptr, length);
	}
	/// <summary>
	/// Creates a <see cref="Span{Byte}"/> instance over the memory block.
	/// </summary>
	/// <returns>A <see cref="Span{TValue}"/> instance over the memory block.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Span<Byte> CreateBinarySpan()
	{
		this.ValidateOperation();
		void* ptr = this.GetMemoryOffset();
		return new(ptr, this.BinaryLength);
	}
	/// <summary>
	/// Creates a <see cref="ReadOnlySpan{Byte}"/> instance over the memory block.
	/// </summary>
	/// <returns>A <see cref="ReadOnlySpan{TValue}"/> instance over the memory block.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public ReadOnlySpan<Byte> CreateReadOnlyBinarySpan()
	{
		this.ValidateOperation(true);
		void* ptr = this.GetMemoryOffset();
		return new(ptr, this.BinaryLength);
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
	public virtual void Unload() => this._isValid.Value = false;

	/// <inheritdoc/>
	[ExcludeFromCodeCoverage]
	public override Boolean Equals(Object? obj) => this.Equals(obj as FixedPointer);
	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override Int32 GetHashCode()
	{
		HashCode result = new();
		result.Add(new IntPtr(this._ptr));
		result.Add(this.BinaryOffset);
		result.Add(this._binaryLength);
		result.Add(this._isReadOnly);
		if (this.Type is not null)
			result.Add(this.Type);
		return result.ToHashCode();
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
		ValidationUtilities.ThrowIfReadOnlyPointer(isReadOnly, this._isReadOnly);
	}
	/// <summary>
	/// Retrieves the number of <typeparamref name="TValue"/> items that can be
	/// referenced into the fixed memory block.
	/// </summary>
	/// <typeparam name="TValue">The type of the objects referenced.</typeparam>
	/// <returns>
	/// The number of <typeparamref name="TValue"/> items that can be referenced into the
	/// fixed memory block.
	/// </returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	protected Int32 GetCount<TValue>() where TValue : unmanaged => this._binaryLength / sizeof(TValue);
	/// <summary>
	/// Validates the size of the referenced value type from current instance.
	/// </summary>
	/// <typeparam name="TValue">Type of the referenced value.</typeparam>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	protected void ValidateReferenceSize<TValue>() where TValue : unmanaged
		=> ValidationUtilities.ThrowIfInvalidRefTypePointer<TValue>(this._binaryLength);

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
}