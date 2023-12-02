namespace Rxmxnx.PInvoke;

/// <summary>
/// Represents a platform-specific type used to manage a pointer to a mutable value of type <typeparamref name="T"/>.
/// </summary>
/// <typeparam name="T">An <see langword="unmanaged"/> <see cref="ValueType"/>.</typeparam>
[Serializable]
[StructLayout(LayoutKind.Sequential)]
[SuppressMessage("csharpsquid", "S6640")]
public readonly unsafe struct ValPtr<T> : IWrapper<IntPtr>, IEquatable<ValPtr<T>>, IComparable, IComparable<ValPtr<T>>,
	ISpanFormattable, ISerializable where T : unmanaged
{
	/// <summary>
	/// A read-only field that represents a pointer that has been initialized to zero.
	/// </summary>
	public static readonly ValPtr<T> Zero = default;

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
	private ValPtr(void* value) => this._value = value;
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
	public Int32 CompareTo(Object? obj)
		=> ValidationUtilities.ThrowIfInvalidValuePointer<T>(obj, this.Pointer, nameof(ValPtr<T>));
	/// <inheritdoc/>
	public Int32 CompareTo(ValPtr<T> value) => this.Pointer.CompareTo(value.Pointer);
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
	/// <inheritdoc cref="IntPtr.ToString(IFormatProvider?)"/>
	public String ToString(IFormatProvider? formatProvider) => this.Pointer.ToString(formatProvider);
	/// <inheritdoc/>
	public String ToString(String? format, IFormatProvider? formatProvider)
		=> this.Pointer.ToString(format, formatProvider);
	/// <inheritdoc/>
	[SuppressMessage("csharpsquid", "S1006")]
	public Boolean TryFormat(Span<Char> destination, out Int32 charsWritten, ReadOnlySpan<Char> format = default,
		IFormatProvider? provider = default)
		=> this.Pointer.TryFormat(destination, out charsWritten, format, provider);

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
	/// Defines an explicit conversion of a given <see cref="IntPtr"/> to a read-only value pointer.
	/// </summary>
	/// <param name="ptr">A <see cref="IntPtr"/> to explicitly convert.</param>
	public static explicit operator ValPtr<T>(IntPtr ptr) => new(ptr.ToPointer());
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
	/// <summary>Compares two values to determine which is less.</summary>
	/// <param name="left">The value to compare with <paramref name="right"/>.</param>
	/// <param name="right">The value to compare with <paramref name="left"/>.</param>
	/// <returns><c>true</c> if <paramref name="left"/> is less than <paramref name="right"/>; otherwise, <c>false</c>.</returns>
	public static Boolean operator <(ValPtr<T> left, ValPtr<T> right) => left.CompareTo(right) < 0;
	/// <summary>Compares two values to determine which is less or equal.</summary>
	/// <param name="left">The value to compare with <paramref name="right"/>.</param>
	/// <param name="right">The value to compare with <paramref name="left"/>.</param>
	/// <returns>
	/// <c>true</c> if <paramref name="left"/> is less than or equal to <paramref name="right"/>; otherwise,
	/// <c>false</c>.
	/// </returns>
	public static Boolean operator <=(ValPtr<T> left, ValPtr<T> right) => left.CompareTo(right) <= 0;
	/// <summary>Compares two values to determine which is greater.</summary>
	/// <param name="left">The value to compare with <paramref name="right"/>.</param>
	/// <param name="right">The value to compare with <paramref name="left"/>.</param>
	/// <returns><c>true</c> if <paramref name="left"/> is greater than <paramref name="right"/>; otherwise, <c>false</c>.</returns>
	public static Boolean operator >(ValPtr<T> left, ValPtr<T> right) => left.CompareTo(right) > 0;
	/// <summary>Compares two values to determine which is greater or equal.</summary>
	/// <param name="left">The value to compare with <paramref name="right"/>.</param>
	/// <param name="right">The value to compare with <paramref name="left"/>.</param>
	/// <returns>
	/// <c>true</c> if <paramref name="left"/> is greater than or equal to <paramref name="right"/>; otherwise,
	/// <c>false</c>.
	/// </returns>
	public static Boolean operator >=(ValPtr<T> left, ValPtr<T> right) => left.CompareTo(right) >= 0;

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

	/// <inheritdoc cref="IntPtr.Parse(String)"/>
	[ExcludeFromCodeCoverage]
	public static ValPtr<T> Parse(String s) => (ValPtr<T>)IntPtr.Parse(s);
	/// <inheritdoc cref="IntPtr.Parse(String, NumberStyles)"/>
	[ExcludeFromCodeCoverage]
	public static ValPtr<T> Parse(String s, NumberStyles style) => (ValPtr<T>)IntPtr.Parse(s, style);
	/// <inheritdoc cref="IntPtr.Parse(String, IFormatProvider)"/>
	[ExcludeFromCodeCoverage]
	public static ValPtr<T> Parse(String s, IFormatProvider? provider) => (ValPtr<T>)IntPtr.Parse(s, provider);
	/// <inheritdoc cref="IntPtr.Parse(String, NumberStyles, IFormatProvider)"/>
	[ExcludeFromCodeCoverage]
	public static ValPtr<T> Parse(String s, NumberStyles style, IFormatProvider? provider)
		=> (ValPtr<T>)IntPtr.Parse(s, style, provider);
	/// <inheritdoc cref="IntPtr.Parse(ReadOnlySpan{Char}, NumberStyles, IFormatProvider)"/>
	[ExcludeFromCodeCoverage]
	public static ValPtr<T> Parse(ReadOnlySpan<Char> s, NumberStyles style = NumberStyles.Integer,
		IFormatProvider? provider = default)
		=> (ValPtr<T>)IntPtr.Parse(s, style, provider);

	/// <inheritdoc cref="IntPtr.TryParse(String?, out IntPtr)"/>
	[ExcludeFromCodeCoverage]
	public static Boolean TryParse([NotNullWhen(true)] String? s, out ValPtr<T> result)
	{
		Unsafe.SkipInit(out result);
		return IntPtr.TryParse(s, out Unsafe.As<ValPtr<T>, IntPtr>(ref result));
	}
	/// <inheritdoc cref="IntPtr.TryParse(String?, NumberStyles, IFormatProvider?, out IntPtr)"/>
	[ExcludeFromCodeCoverage]
	public static Boolean TryParse([NotNullWhen(true)] String? s, NumberStyles style, IFormatProvider? provider,
		out ValPtr<T> result)
	{
		Unsafe.SkipInit(out result);
		return IntPtr.TryParse(s, style, provider, out Unsafe.As<ValPtr<T>, IntPtr>(ref result));
	}
	/// <inheritdoc cref="IntPtr.TryParse(ReadOnlySpan{Char}, out IntPtr)"/>
	[ExcludeFromCodeCoverage]
	public static Boolean TryParse(ReadOnlySpan<Char> s, out ValPtr<T> result)
	{
		Unsafe.SkipInit(out result);
		return IntPtr.TryParse(s, out Unsafe.As<ValPtr<T>, IntPtr>(ref result));
	}
	/// <inheritdoc cref="IntPtr.TryParse(ReadOnlySpan{Char}, NumberStyles, IFormatProvider?, out IntPtr)"/>
	[ExcludeFromCodeCoverage]
	public static Boolean TryParse(ReadOnlySpan<Char> s, NumberStyles style, IFormatProvider? provider,
		out ValPtr<T> result)
	{
		Unsafe.SkipInit(out result);
		return IntPtr.TryParse(s, style, provider, out Unsafe.As<ValPtr<T>, IntPtr>(ref result));
	}
}