namespace Rxmxnx.PInvoke;

/// <summary>
/// A platform-specific type that is used to represent a pointer to a read-only <typeparamref name="T"/> value.
/// </summary>
/// <typeparam name="T">A <see langword="unmanaged"/> <see cref="ValueType"/>.</typeparam>
[Serializable]
[StructLayout(LayoutKind.Sequential)]
public readonly unsafe struct FuncPtr<T> : IWrapper<IntPtr>, IEquatable<FuncPtr<T>>, ISpanFormattable, ISerializable
	where T : Delegate
{
	/// <summary>
	/// A read-only field that represents a pointer that has been initialized to zero.
	/// </summary>
	public static readonly FuncPtr<T> Zero = default;

	/// <summary>
	/// Internal pointer.
	/// </summary>
	private readonly void* _value;

	/// <summary>
	/// Internal pointer.
	/// </summary>
	public IntPtr Pointer => new(this._value);
	/// <summary>
	/// Indicates whether current pointer is zero.
	/// </summary>
	public Boolean IsZero => IntPtr.Zero == (IntPtr)this._value;

	/// <inheritdoc cref="IFixedMethod{T}.Method"/>
	public T Invoke => !this.IsZero ? Marshal.GetDelegateForFunctionPointer<T>(this.Pointer) : default!;

	/// <summary>
	/// Private constructor.
	/// </summary>
	/// <param name="value">Unsafe pointer.</param>
	private FuncPtr(void* value) => this._value = value;
	/// <summary>
	/// Serialization constructor.
	/// </summary>
	/// <param name="info">A <see cref="SerializationInfo"/> instance.</param>
	/// <param name="context">A <see cref="StreamingContext"/> instance.</param>
	/// <exception cref="ArgumentException">If invalid pointer value.</exception>
	[ExcludeFromCodeCoverage]
	private FuncPtr(SerializationInfo info, StreamingContext context)
	{
		Int64 l = info.GetInt64("value");
		if (IntPtr.Size == 4 && l is > Int32.MaxValue or < Int32.MinValue)
			throw new ArgumentException(
				"An IntPtr or UIntPtr with an eight byte value cannot be deserialized on a machine with a four byte word size.");
		this._value = (void*)l;
	}

	IntPtr IWrapper<IntPtr>.Value => this.Pointer;

	[ExcludeFromCodeCoverage]
	void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
	{
		if (info == null)
			throw new ArgumentNullException(nameof(info));

		info.AddValue("value", (Int64)this._value);
	}

	/// <inheritdoc/>
	public Boolean Equals(FuncPtr<T> other) => this.Pointer == other.Pointer;

	/// <inheritdoc/>
	public override Boolean Equals([NotNullWhen(true)] Object? obj)
		=> obj is FuncPtr<T> other && this._value == other._value;
	/// <inheritdoc/>
	public override Int32 GetHashCode() => new IntPtr(this._value).GetHashCode();
	/// <inheritdoc/>
	public override String ToString() => this.Pointer.ToString();

	/// <summary>
	/// Converts the numeric value of the current <see cref="FuncPtr{T}"/> object to its equivalent
	/// <see cref="String"/> representation.
	/// </summary>
	/// <param name="format">
	/// A format specification that governs how the current <see cref="FuncPtr{T}"/> object is converted.
	/// </param>
	/// <returns>
	/// The <see cref="String"/> representation of the value of the current <see cref="FuncPtr{T}"/> object.
	/// </returns>
	/// <exception cref="FormatException"><paramref name="format"/> is invalid or not supported.</exception>
	public String ToString(String? format) => this.Pointer.ToString(format);
	/// <inheritdoc cref="IntPtr.ToString(IFormatProvider?)"/>
	public String ToString(IFormatProvider? formatProvider) => this.Pointer.ToString(formatProvider);
	/// <inheritdoc/>
	public String ToString(String? format, IFormatProvider? formatProvider) => this.Pointer.ToString(format, formatProvider);
	/// <inheritdoc/>
	public Boolean TryFormat(Span<Char> destination, out Int32 charsWritten, ReadOnlySpan<Char> format = default,
		IFormatProvider? provider = default)
		=> this.Pointer.TryFormat(destination, out charsWritten, format, provider);

	/// <summary>
	/// Defines an explicit conversion of a given <see cref="IntPtr"/> to a read-only value pointer.
	/// </summary>
	/// <param name="ptr">A <see cref="IntPtr"/> to explicitly convert.</param>
	public static explicit operator FuncPtr<T>(IntPtr ptr) => new(ptr.ToPointer());
	/// <summary>
	/// Defines an implicit conversion of a given <see cref="FuncPtr{T}"/> to a pointer.
	/// </summary>
	/// <param name="valPtr">A <see cref="FuncPtr{T}"/> to implicitly convert.</param>
	public static implicit operator IntPtr(FuncPtr<T> valPtr) => new(valPtr._value);

	/// <summary>
	/// Determines whether two specified instances of <see cref="FuncPtr{T}"/> are equal.
	/// </summary>
	/// <param name="value1">The first pointer to compare.</param>
	/// <param name="value2">The second pointer to compare.</param>
	/// <returns>
	/// <see langword="true"/> if <paramref name="value1"/> equals <paramref name="value2"/>;
	/// otherwise, <see langword="false"/>.
	/// </returns>
	public static Boolean operator ==(FuncPtr<T> value1, FuncPtr<T> value2) => value1._value == value2._value;
	/// <summary>
	/// Determines whether two specified instances of <see cref="FuncPtr{T}"/> are not equal.
	/// </summary>
	/// <param name="value1">The first pointer to compare.</param>
	/// <param name="value2">The second pointer to compare.</param>
	/// <returns>
	/// <see langword="true"/> if <paramref name="value1"/> does not equal <paramref name="value2"/>;
	/// otherwise, <see langword="false"/>.
	/// </returns>
	/// <inheritdoc cref="IntPtr.op_Inequality(IntPtr, IntPtr)"/>
	public static Boolean operator !=(FuncPtr<T> value1, FuncPtr<T> value2) => value1._value != value2._value;

	/// <inheritdoc cref="IntPtr.Parse(String)"/>
	[ExcludeFromCodeCoverage]
	public static FuncPtr<T> Parse(String s) => (FuncPtr<T>)IntPtr.Parse(s);
	/// <inheritdoc cref="IntPtr.Parse(String, NumberStyles)"/>
	[ExcludeFromCodeCoverage]
	public static FuncPtr<T> Parse(String s, NumberStyles style) => (FuncPtr<T>)IntPtr.Parse(s, style);
	/// <inheritdoc cref="IntPtr.Parse(String, IFormatProvider)"/>
	[ExcludeFromCodeCoverage]
	public static FuncPtr<T> Parse(String s, IFormatProvider? formatProvider) => (FuncPtr<T>)IntPtr.Parse(s, formatProvider);
	/// <inheritdoc cref="IntPtr.Parse(String, NumberStyles, IFormatProvider)"/>
	[ExcludeFromCodeCoverage]
	public static FuncPtr<T> Parse(String s, NumberStyles style, IFormatProvider? provider)
		=> (FuncPtr<T>)IntPtr.Parse(s, style, provider);
	/// <inheritdoc cref="IntPtr.Parse(ReadOnlySpan{Char}, NumberStyles, IFormatProvider)"/>
	[ExcludeFromCodeCoverage]
	public static FuncPtr<T> Parse(ReadOnlySpan<Char> s, NumberStyles style = NumberStyles.Integer,
		IFormatProvider? provider = null)
		=> (FuncPtr<T>)IntPtr.Parse(s, style, provider);

	/// <inheritdoc cref="IntPtr.TryParse(String?, out IntPtr)"/>
	[ExcludeFromCodeCoverage]
	public static Boolean TryParse([NotNullWhen(true)] String? s, out FuncPtr<T> result)
	{
		Unsafe.SkipInit(out result);
		return IntPtr.TryParse(s, out Unsafe.As<FuncPtr<T>, IntPtr>(ref result));
	}
	/// <inheritdoc cref="IntPtr.TryParse(String?, NumberStyles, IFormatProvider?, out IntPtr)"/>
	[ExcludeFromCodeCoverage]
	public static Boolean TryParse([NotNullWhen(true)] String? s, NumberStyles style, IFormatProvider? provider,
		out FuncPtr<T> result)
	{
		Unsafe.SkipInit(out result);
		return IntPtr.TryParse(s, style, provider, out Unsafe.As<FuncPtr<T>, IntPtr>(ref result));
	}
	/// <inheritdoc cref="IntPtr.TryParse(ReadOnlySpan{Char}, out IntPtr)"/>
	[ExcludeFromCodeCoverage]
	public static Boolean TryParse(ReadOnlySpan<Char> s, out FuncPtr<T> result)
	{
		Unsafe.SkipInit(out result);
		return IntPtr.TryParse(s, out Unsafe.As<FuncPtr<T>, IntPtr>(ref result));
	}
	/// <inheritdoc cref="IntPtr.TryParse(ReadOnlySpan{Char}, NumberStyles, IFormatProvider?, out IntPtr)"/>
	[ExcludeFromCodeCoverage]
	public static Boolean TryParse(ReadOnlySpan<Char> s, NumberStyles style, IFormatProvider? provider,
		out FuncPtr<T> result)
	{
		Unsafe.SkipInit(out result);
		return IntPtr.TryParse(s, style, provider, out Unsafe.As<FuncPtr<T>, IntPtr>(ref result));
	}
}