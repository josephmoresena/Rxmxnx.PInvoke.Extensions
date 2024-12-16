namespace Rxmxnx.PInvoke;

/// <summary>
/// Represents a platform-specific type used to handle a pointer to a method of type <typeparamref name="TDelegate"/>.
/// </summary>
/// <typeparam name="TDelegate">The type of delegate that the method pointer represents.</typeparam>
[Serializable]
[StructLayout(LayoutKind.Sequential)]
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
public readonly unsafe struct FuncPtr<TDelegate> : IWrapper<IntPtr>, IEquatable<FuncPtr<TDelegate>>, ISpanFormattable,
	ISerializable where TDelegate : Delegate
{
	/// <summary>
	/// A read-only field representing a null-initialized function pointer.
	/// </summary>
	public static readonly FuncPtr<TDelegate> Zero = default;

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
	/// A managed delegate using the method address pointed to by this instance.
	/// </summary>
	public TDelegate Invoke => !this.IsZero ? Marshal.GetDelegateForFunctionPointer<TDelegate>(this.Pointer) : default!;

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
		=> this._value = ValidationUtilities.ThrowIfInvalidPointer(info);

	IntPtr IWrapper<IntPtr>.Value => this.Pointer;

	[ExcludeFromCodeCoverage]
	void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		=> ValidationUtilities.ThrowIfInvalidSerialization(info, this._value);

	/// <inheritdoc/>
	public Boolean Equals(FuncPtr<TDelegate> other) => this.Pointer == other.Pointer;

	/// <inheritdoc/>
	public override Boolean Equals([NotNullWhen(true)] Object? obj)
		=> obj is FuncPtr<TDelegate> other && this._value == other._value;
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
	public String ToString(String? format, IFormatProvider? formatProvider)
		=> this.Pointer.ToString(format, formatProvider);
	/// <inheritdoc/>
	[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS1006)]
	public Boolean TryFormat(Span<Char> destination, out Int32 charsWritten, ReadOnlySpan<Char> format = default,
		IFormatProvider? provider = default)
		=> this.Pointer.TryFormat(destination, out charsWritten, format, provider);

	/// <summary>
	/// Defines an explicit conversion of a given <see cref="IntPtr"/> to a read-only value pointer.
	/// </summary>
	/// <param name="ptr">A <see cref="IntPtr"/> to explicitly convert.</param>
	public static explicit operator FuncPtr<TDelegate>(IntPtr ptr) => new(ptr.ToPointer());
	/// <summary>
	/// Defines an explicit conversion of a given <see cref="IntPtr"/> to a read-only value pointer.
	/// </summary>
	/// <param name="ptr">A pointer to explicitly convert.</param>
	public static explicit operator FuncPtr<TDelegate>(void* ptr) => new(ptr);
	/// <summary>
	/// Defines an implicit conversion of a given <see cref="FuncPtr{T}"/> to a pointer.
	/// </summary>
	/// <param name="valPtr">A <see cref="FuncPtr{T}"/> to implicitly convert.</param>
	public static implicit operator IntPtr(FuncPtr<TDelegate> valPtr) => new(valPtr._value);
	/// <summary>
	/// Defines an implicit conversion of a given <see cref="FuncPtr{T}"/> to a pointer.
	/// </summary>
	/// <param name="valPtr">A <see cref="FuncPtr{T}"/> to implicitly convert.</param>
	public static implicit operator void*(FuncPtr<TDelegate> valPtr) => valPtr._value;

	/// <summary>
	/// Determines whether two specified instances of <see cref="FuncPtr{T}"/> are equal.
	/// </summary>
	/// <param name="value1">The first pointer to compare.</param>
	/// <param name="value2">The second pointer to compare.</param>
	/// <returns>
	/// <see langword="true"/> if <paramref name="value1"/> equals <paramref name="value2"/>;
	/// otherwise, <see langword="false"/>.
	/// </returns>
	public static Boolean operator ==(FuncPtr<TDelegate> value1, FuncPtr<TDelegate> value2)
		=> value1._value == value2._value;
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
	public static Boolean operator !=(FuncPtr<TDelegate> value1, FuncPtr<TDelegate> value2)
		=> value1._value != value2._value;

	/// <inheritdoc cref="IntPtr.Parse(String)"/>
	[ExcludeFromCodeCoverage]
	public static FuncPtr<TDelegate> Parse(String s) => (FuncPtr<TDelegate>)IntPtr.Parse(s);
	/// <inheritdoc cref="IntPtr.Parse(String, NumberStyles)"/>
	[ExcludeFromCodeCoverage]
	public static FuncPtr<TDelegate> Parse(String s, NumberStyles style) => (FuncPtr<TDelegate>)IntPtr.Parse(s, style);
	/// <inheritdoc cref="IntPtr.Parse(String, IFormatProvider)"/>
	[ExcludeFromCodeCoverage]
	public static FuncPtr<TDelegate> Parse(String s, IFormatProvider? formatProvider)
		=> (FuncPtr<TDelegate>)IntPtr.Parse(s, formatProvider);
	/// <inheritdoc cref="IntPtr.Parse(String, NumberStyles, IFormatProvider)"/>
	[ExcludeFromCodeCoverage]
	public static FuncPtr<TDelegate> Parse(String s, NumberStyles style, IFormatProvider? provider)
		=> (FuncPtr<TDelegate>)IntPtr.Parse(s, style, provider);
	/// <inheritdoc cref="IntPtr.Parse(ReadOnlySpan{Char}, NumberStyles, IFormatProvider)"/>
	[ExcludeFromCodeCoverage]
	public static FuncPtr<TDelegate> Parse(ReadOnlySpan<Char> s, NumberStyles style = NumberStyles.Integer,
		IFormatProvider? provider = default)
		=> (FuncPtr<TDelegate>)IntPtr.Parse(s, style, provider);

	/// <inheritdoc cref="IntPtr.TryParse(String?, out IntPtr)"/>
	[ExcludeFromCodeCoverage]
	public static Boolean TryParse([NotNullWhen(true)] String? s, out FuncPtr<TDelegate> result)
	{
		Unsafe.SkipInit(out result);
		return IntPtr.TryParse(s, out Unsafe.As<FuncPtr<TDelegate>, IntPtr>(ref result));
	}
	/// <inheritdoc cref="IntPtr.TryParse(String?, NumberStyles, IFormatProvider?, out IntPtr)"/>
	[ExcludeFromCodeCoverage]
	public static Boolean TryParse([NotNullWhen(true)] String? s, NumberStyles style, IFormatProvider? provider,
		out FuncPtr<TDelegate> result)
	{
		Unsafe.SkipInit(out result);
		return IntPtr.TryParse(s, style, provider, out Unsafe.As<FuncPtr<TDelegate>, IntPtr>(ref result));
	}
	/// <inheritdoc cref="IntPtr.TryParse(ReadOnlySpan{Char}, out IntPtr)"/>
	[ExcludeFromCodeCoverage]
	public static Boolean TryParse(ReadOnlySpan<Char> s, out FuncPtr<TDelegate> result)
	{
		Unsafe.SkipInit(out result);
		return IntPtr.TryParse(s, out Unsafe.As<FuncPtr<TDelegate>, IntPtr>(ref result));
	}
	/// <inheritdoc cref="IntPtr.TryParse(ReadOnlySpan{Char}, NumberStyles, IFormatProvider?, out IntPtr)"/>
	[ExcludeFromCodeCoverage]
	public static Boolean TryParse(ReadOnlySpan<Char> s, NumberStyles style, IFormatProvider? provider,
		out FuncPtr<TDelegate> result)
	{
		Unsafe.SkipInit(out result);
		return IntPtr.TryParse(s, style, provider, out Unsafe.As<FuncPtr<TDelegate>, IntPtr>(ref result));
	}
}