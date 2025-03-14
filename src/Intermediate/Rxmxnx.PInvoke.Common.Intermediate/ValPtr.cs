namespace Rxmxnx.PInvoke;

/// <summary>
/// Represents a platform-specific type used to manage a pointer to a mutable value of type <typeparamref name="T"/>.
/// </summary>
/// <typeparam name="T">Type of pointer.</typeparam>
[Serializable]
[StructLayout(LayoutKind.Sequential)]
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
public readonly unsafe partial struct ValPtr<T> : IWrapper<IntPtr>, IEquatable<ValPtr<T>>, ISerializable
#if NET9_0_OR_GREATER
	where T : allows ref struct
#endif
{
#pragma warning disable CS8500
	/// <summary>
	/// A read-only field that represents a pointer that has been initialized to zero.
	/// </summary>
	public static readonly ValPtr<T> Zero = default;

	/// <summary>
	/// Indicates if <typeparamref name="T"/> is an <see langword="unmanaged"/> type.
	/// </summary>
	public static Boolean IsUnmanaged => ReadOnlyValPtr<T>.IsUnmanaged;

	/// <summary>
	/// The internal pointer value.
	/// </summary>
	private readonly void* _value;

	/// <summary>
	/// Internal pointer as an <see cref="IntPtr"/>.
	/// </summary>
	public IntPtr Pointer => new(this._value);
	/// <summary>
	/// Indicates whether the current pointer is <see langword="null"/>.
	/// </summary>
	public Boolean IsZero => IntPtr.Zero == (IntPtr)this._value;
	/// <summary>
	/// A reference to the value pointed to by this instance.
	/// </summary>
	public ref T Reference => ref Unsafe.AsRef<T>(this._value);

	/// <summary>
	/// Private constructor.
	/// </summary>
	/// <param name="value">Unsafe pointer.</param>
	internal ValPtr(void* value) => this._value = value;

	/// <summary>
	/// Serialization constructor.
	/// </summary>
	/// <param name="info">A <see cref="SerializationInfo"/> instance.</param>
	/// <param name="context">A <see cref="StreamingContext"/> instance.</param>
	/// <exception cref="ArgumentException">If invalid pointer value.</exception>
	[ExcludeFromCodeCoverage]
	private ValPtr(SerializationInfo info, StreamingContext context)
		=> this._value = ValidationUtilities.ThrowIfInvalidPointer(info);

	IntPtr IWrapper<IntPtr>.Value => this.Pointer;

	[ExcludeFromCodeCoverage]
	void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		=> ValidationUtilities.ThrowIfInvalidSerialization(info, this._value);

	/// <inheritdoc/>
	public Boolean Equals(ValPtr<T> other) => this.Pointer == other.Pointer;

	/// <inheritdoc/>
	public override Boolean Equals([NotNullWhen(true)] Object? obj)
		=> obj switch
		{
			ReadOnlyValPtr<T> r => this.Pointer == r.Pointer,
			ValPtr<T> v => this._value == v._value,
			_ => false,
		};
	/// <inheritdoc/>
	public override Int32 GetHashCode() => new IntPtr(this._value).GetHashCode();
	/// <inheritdoc/>
	public override String ToString() => this.Pointer.ToString();

	/// <summary>
	/// Converts the numeric value of the current <see cref="ValPtr{T}"/> object to its equivalent
	/// <see cref="String"/> representation.
	/// </summary>
	/// <param name="format">
	/// A format specification that governs how the current <see cref="ValPtr{T}"/> object is converted.
	/// </param>
	/// <returns>
	/// The <see cref="String"/> representation of the value of the current <see cref="ValPtr{T}"/> object.
	/// </returns>
	/// <exception cref="FormatException"><paramref name="format"/> is invalid or not supported.</exception>
	public String ToString(String? format) => this.Pointer.ToString(format);

	/// <summary>
	/// Retrieves an <see langword="unsafe"/> <see cref="IFixedReference{T}.IDisposable"/> instance from
	/// current reference pointer.
	/// </summary>
	/// <param name="disposable">Optional object to dispose in order to free unmanaged resources.</param>
	/// <returns>An <see cref="IFixedReference{T}.IDisposable"/> instance representing a fixed reference.</returns>
	/// <remarks>
	/// The instance obtained is "unsafe" as it doesn't guarantee that the referenced value
	/// won't be moved or collected by garbage collector.
	/// The <paramref name="disposable"/> parameter allows for custom management of resource cleanup.
	/// If provided, this object will be disposed of when the fixed reference is disposed.
	/// </remarks>
	public IFixedReference<T>.IDisposable GetUnsafeFixedReference(IDisposable? disposable = default)
		=> new FixedReference<T>(this._value).ToDisposable(disposable);
	/// <summary>
	/// Retrieves an <see langword="unsafe"/> <see cref="IFixedContext{T}.IDisposable"/> instance from
	/// current reference pointer.
	/// </summary>
	/// <param name="count">The number of items of type <typeparamref name="T"/> in the memory block.</param>
	/// <param name="disposable">Optional object to dispose in order to free unmanaged resources.</param>
	/// <returns>An <see cref="IFixedContext{T}.IDisposable"/> instance representing a fixed reference.</returns>
	/// <remarks>
	/// The instance obtained is "unsafe" as it doesn't guarantee that the referenced values
	/// won't be moved or collected by garbage collector.
	/// The <paramref name="disposable"/> parameter allows for custom management of resource cleanup.
	/// If provided, this object will be disposed of when the fixed reference is disposed.
	/// </remarks>
	public IFixedContext<T>.IDisposable GetUnsafeFixedContext(Int32 count, IDisposable? disposable = default)
		=> new FixedContext<T>(this._value, count).ToDisposable(disposable);

	/// <summary>
	/// Defines an explicit conversion of a given <see cref="IntPtr"/> to a value pointer.
	/// </summary>
	/// <param name="ptr">An <see cref="IntPtr"/> to explicitly convert.</param>
	public static explicit operator ValPtr<T>(IntPtr ptr) => new(ptr.ToPointer());
	/// <summary>
	/// Defines an explicit conversion of a given pointer to a value pointer.
	/// </summary>
	/// <param name="ptr">A pointer to explicitly convert.</param>
	public static explicit operator ValPtr<T>(void* ptr) => new(ptr);
	/// <summary>
	/// Defines an implicit conversion of a given pointer to a value pointer.
	/// </summary>
	/// <param name="ptr">A pointer to implicitly convert.</param>
	public static implicit operator ValPtr<T>(T* ptr) => new(ptr);
	/// <summary>
	/// Defines an implicit conversion of a given <see cref="ValPtr{T}"/> to a pointer.
	/// </summary>
	/// <param name="valPtr">A <see cref="ValPtr{T}"/> to implicitly convert.</param>
	public static implicit operator void*(ValPtr<T> valPtr) => valPtr._value;
	/// <summary>
	/// Defines an implicit conversion of a given <see cref="ValPtr{T}"/> to a pointer.
	/// </summary>
	/// <param name="valPtr">A <see cref="ValPtr{T}"/> to implicitly convert.</param>
	public static implicit operator T*(ValPtr<T> valPtr) => (T*)valPtr._value;
	/// <summary>
	/// Defines an implicit conversion of a given <see cref="ValPtr{T}"/> to a pointer.
	/// </summary>
	/// <param name="valPtr">A <see cref="ValPtr{T}"/> to implicitly convert.</param>
	public static implicit operator IntPtr(ValPtr<T> valPtr) => new(valPtr._value);
	/// <summary>
	/// Defines an implicit conversion of a given <see cref="ValPtr{T}"/> to a read-only pointer.
	/// </summary>
	/// <param name="valPtr">A <see cref="ValPtr{T}"/> to implicitly convert.</param>
	public static implicit operator ReadOnlyValPtr<T>(ValPtr<T> valPtr) => (ReadOnlyValPtr<T>)valPtr.Pointer;

	/// <summary>
	/// Determines whether two specified instances of <see cref="ValPtr{T}"/> are equal.
	/// </summary>
	/// <param name="value1">The first pointer to compare.</param>
	/// <param name="value2">The second pointer to compare.</param>
	/// <returns>
	/// <see langword="true"/> if <paramref name="value1"/> equals <paramref name="value2"/>;
	/// otherwise, <see langword="false"/>.
	/// </returns>
	public static Boolean operator ==(ValPtr<T> value1, ValPtr<T> value2) => value1._value == value2._value;
	/// <summary>
	/// Determines whether two specified instances of <see cref="ValPtr{T}"/> are not equal.
	/// </summary>
	/// <param name="value1">The first pointer to compare.</param>
	/// <param name="value2">The second pointer to compare.</param>
	/// <returns>
	/// <see langword="true"/> if <paramref name="value1"/> does not equal <paramref name="value2"/>;
	/// otherwise, <see langword="false"/>.
	/// </returns>
	/// <inheritdoc cref="IntPtr.op_Inequality(IntPtr, IntPtr)"/>
	public static Boolean operator !=(ValPtr<T> value1, ValPtr<T> value2) => value1._value != value2._value;
	/// <summary>
	/// Adds an offset in <typeparamref name="T"/> units to the value of a pointer.
	/// </summary>
	/// <param name="pointer">The pointer to add the offset to.</param>
	/// <param name="offset">The offset in <typeparamref name="T"/> units to add.</param>
	public static ValPtr<T> operator +(ValPtr<T> pointer, Int32 offset)
		=> (ValPtr<T>)(pointer.Pointer + offset * sizeof(T));
	/// <summary>
	/// Adds an offset of one <typeparamref name="T"/> unit to the value of a pointer.
	/// </summary>
	/// <param name="pointer">The pointer to add the offset to.</param>
	public static ValPtr<T> operator ++(ValPtr<T> pointer) => (ValPtr<T>)(pointer.Pointer + sizeof(T));
	/// <summary>
	/// Subtracts an offset in <typeparamref name="T"/> units from the value of a pointer.
	/// </summary>
	/// <param name="pointer">The pointer to subtract the offset form.</param>
	/// <param name="offset">The offset in <typeparamref name="T"/> units to subtract.</param>
	public static ValPtr<T> operator -(ValPtr<T> pointer, Int32 offset)
		=> (ValPtr<T>)(pointer.Pointer - offset * sizeof(T));
	/// <summary>
	/// Subtracts an offset of one <typeparamref name="T"/> unit from the value of a pointer.
	/// </summary>
	/// <param name="pointer">The pointer to subtract the offset form.</param>
	public static ValPtr<T> operator --(ValPtr<T> pointer) => (ValPtr<T>)(pointer.Pointer - sizeof(T));

	/// <summary>
	/// Adds an offset in <typeparamref name="T"/> units to the value of a pointer.
	/// </summary>
	/// <param name="pointer">The pointer to add the offset to.</param>
	/// <param name="offset">The offset in <typeparamref name="T"/> units to add.</param>
	public static ValPtr<T> Add(ValPtr<T> pointer, Int32 offset) => (ValPtr<T>)(pointer.Pointer + offset * sizeof(T));
	/// <summary>
	/// Subtracts an offset in <typeparamref name="T"/> units to the value of a pointer.
	/// </summary>
	/// <param name="pointer">The pointer to subtract the offset to.</param>
	/// <param name="offset">The offset in <typeparamref name="T"/> units to subtract.</param>
	public static ValPtr<T> Subtract(ValPtr<T> pointer, Int32 offset)
		=> (ValPtr<T>)(pointer.Pointer - offset * sizeof(T));
#pragma warning restore CS8500
}