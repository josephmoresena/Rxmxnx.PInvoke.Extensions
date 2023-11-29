namespace Rxmxnx.PInvoke;

/// <summary>
/// A platform-specific type that is used to represent a pointer to a read-only <typeparamref name="T"/> value.
/// </summary>
/// <typeparam name="T">A <see langword="unmanaged"/> <see cref="ValueType"/>.</typeparam>
[Serializable]
[StructLayout(LayoutKind.Sequential)]
public readonly unsafe struct ReadOnlyValPtr<T> : IEquatable<ReadOnlyValPtr<T>>, IComparable,
	IComparable<ReadOnlyValPtr<T>>, ISpanFormattable, ISerializable where T : unmanaged
{
	/// <summary>
	/// A read-only field that represents a pointer that has been initialized to zero.
	/// </summary>
	public static readonly ReadOnlyValPtr<T> Zero = default;

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
	/// <inheritdoc cref="IReadOnlyReferenceable{T}.Reference"/>
	public ref readonly T Reference
	{
		get
		{
			if (!this.IsZero)
				return ref Unsafe.AsRef<T>(this._value);
			throw new NullReferenceException();
		}
	}

	/// <summary>
	/// Private constructor.
	/// </summary>
	/// <param name="value">Unsafe pointer.</param>
	private ReadOnlyValPtr(void* value) => this._value = value;
	/// <summary>
	/// Serialization constructor.
	/// </summary>
	/// <param name="info">A <see cref="SerializationInfo"/> instance.</param>
	/// <param name="context">A <see cref="StreamingContext"/> instance.</param>
	/// <exception cref="ArgumentException">If invalid pointer value.</exception>
	[ExcludeFromCodeCoverage]
	private ReadOnlyValPtr(SerializationInfo info, StreamingContext context)
	{
		Int64 l = info.GetInt64("value");
		if (IntPtr.Size == 4 && l is > Int32.MaxValue or < Int32.MinValue)
			throw new ArgumentException(
				"An IntPtr or UIntPtr with an eight byte value cannot be deserialized on a machine with a four byte word size.");
		this._value = (void*)l;
	}

	[ExcludeFromCodeCoverage]
	void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
	{
		if (info == null)
			throw new ArgumentNullException(nameof(info));

		info.AddValue("value", (Int64)this._value);
	}

	/// <inheritdoc/>
	public Int32 CompareTo(Object? value)
		=> value switch
		{
			null => 1,
			ReadOnlyValPtr<T> r => this.Pointer.CompareTo(r.Pointer),
			ValPtr<T> v => this.Pointer.CompareTo(v.Pointer),
			_ => throw new ArgumentException($"Object must be of type {nameof(ReadOnlyValPtr<T>)}."),
		};
	/// <inheritdoc/>
	public Int32 CompareTo(ReadOnlyValPtr<T> value) => this.Pointer.CompareTo(value.Pointer);
	/// <inheritdoc/>
	public Boolean Equals(ReadOnlyValPtr<T> other) => this.Pointer == other.Pointer;

	/// <inheritdoc/>
	public override Boolean Equals([NotNullWhen(true)] Object? obj)
		=> obj switch
		{
			ReadOnlyValPtr<T> r => this._value == r._value,
			ValPtr<T> v => this.Pointer == v.Pointer,
			_ => false,
		};
	/// <inheritdoc/>
	public override Int32 GetHashCode() => new IntPtr(this._value).GetHashCode();
	/// <inheritdoc/>
	public override String ToString() => this.Pointer.ToString();

	/// <summary>
	/// Retrieves an <see langword="unsafe"/> <see cref="IReadOnlyFixedReference{T}.IDisposable"/> instance from
	/// current read-only reference pointer.
	/// </summary>
	/// <param name="disposable">Object to dispose in order to free <see langword="unmanaged"/> resources.</param>
	/// <returns>A <see cref="IReadOnlyFixedReference{T}.IDisposable"/> instance.</returns>
	/// <remarks>
	/// The instance obtained is "unsafe" as it doesn't guarantee that the referenced value
	/// won't be moved or collected by garbage collector.
	/// </remarks>
	public IReadOnlyFixedReference<T>.IDisposable GetUnsafeFixedReference(IDisposable? disposable = default)
		=> new ReadOnlyFixedReference<T>(this._value).ToDisposable(disposable);
	/// <summary>
	/// Retrieves an <see langword="unsafe"/> <see cref="IReadOnlyFixedContext{T}.IDisposable"/> instance from
	/// current read-only reference pointer.
	/// </summary>
	/// <param name="count">The number of items of type <typeparamref name="T"/> in the memory block.</param>
	/// <param name="disposable">Object to dispose in order to free <see langword="unmanaged"/> resources.</param>
	/// <returns>A <see cref="IReadOnlyFixedContext{T}.IDisposable"/> instance.</returns>
	/// <remarks>
	/// The instance obtained is "unsafe" as it doesn't guarantee that the referenced values
	/// won't be moved or collected by garbage collector.
	/// </remarks>
	public IReadOnlyFixedContext<T>.IDisposable GetUnsafeFixedContext(Int32 count, IDisposable? disposable = default)
		=> new ReadOnlyFixedContext<T>(this._value, count).ToDisposable(disposable);

	/// <summary>
	/// Converts the numeric value of the current <see cref="ReadOnlyValPtr{T}"/> object to its equivalent
	/// <see cref="String"/> representation.
	/// </summary>
	/// <param name="format">
	/// A format specification that governs how the current <see cref="ReadOnlyValPtr{T}"/> object is converted.
	/// </param>
	/// <returns>
	/// The <see cref="String"/> representation of the value of the current <see cref="ReadOnlyValPtr{T}"/> object.
	/// </returns>
	/// <exception cref="FormatException"><paramref name="format"/> is invalid or not supported.</exception>
	public String ToString(String? format) => this.Pointer.ToString(format);
	/// <inheritdoc cref="IntPtr.ToString(IFormatProvider?)"/>
	public String ToString(IFormatProvider? provider) => this.Pointer.ToString(provider);
	/// <inheritdoc/>
	public String ToString(String? format, IFormatProvider? provider) => this.Pointer.ToString(format, provider);
	/// <inheritdoc/>
	public Boolean TryFormat(Span<Char> destination, out Int32 charsWritten, ReadOnlySpan<Char> format = default,
		IFormatProvider? provider = default)
		=> this.Pointer.TryFormat(destination, out charsWritten, format, provider);

	/// <summary>
	/// Defines an explicit conversion of a given <see cref="IntPtr"/> to a read-only value pointer.
	/// </summary>
	/// <param name="ptr">A <see cref="IntPtr"/> to explicitly convert.</param>
	public static explicit operator ReadOnlyValPtr<T>(IntPtr ptr) => new(ptr.ToPointer());
	/// <summary>
	/// Defines an implicit conversion of a given <see cref="ReadOnlyValPtr{T}"/> to a pointer.
	/// </summary>
	/// <param name="valPtr">A <see cref="ReadOnlyValPtr{T}"/> to implicitly convert.</param>
	public static implicit operator IntPtr(ReadOnlyValPtr<T> valPtr) => new(valPtr._value);

	/// <summary>
	/// Determines whether two specified instances of <see cref="ReadOnlyValPtr{T}"/> are equal.
	/// </summary>
	/// <param name="value1">The first pointer to compare.</param>
	/// <param name="value2">The second pointer to compare.</param>
	/// <returns>
	/// <see langword="true"/> if <paramref name="value1"/> equals <paramref name="value2"/>;
	/// otherwise, <see langword="false"/>.
	/// </returns>
	public static Boolean operator ==(ReadOnlyValPtr<T> value1, ReadOnlyValPtr<T> value2)
		=> value1._value == value2._value;
	/// <summary>
	/// Determines whether two specified instances of <see cref="ReadOnlyValPtr{T}"/> are not equal.
	/// </summary>
	/// <param name="value1">The first pointer to compare.</param>
	/// <param name="value2">The second pointer to compare.</param>
	/// <returns>
	/// <see langword="true"/> if <paramref name="value1"/> does not equal <paramref name="value2"/>;
	/// otherwise, <see langword="false"/>.
	/// </returns>
	/// <inheritdoc cref="IntPtr.op_Inequality(IntPtr, IntPtr)"/>
	public static Boolean operator !=(ReadOnlyValPtr<T> value1, ReadOnlyValPtr<T> value2)
		=> value1._value != value2._value;
	/// <summary>
	/// Adds an offset in <typeparamref name="T"/> units to the value of a pointer.
	/// </summary>
	/// <param name="pointer">The pointer to add the offset to.</param>
	/// <param name="offset">The offset in <typeparamref name="T"/> units to add.</param>
	public static ReadOnlyValPtr<T> operator +(ReadOnlyValPtr<T> pointer, Int32 offset)
		=> (ReadOnlyValPtr<T>)(pointer.Pointer + offset * sizeof(T));
	/// <summary>
	/// Adds an offset of one <typeparamref name="T"/> unit to the value of a pointer.
	/// </summary>
	/// <param name="pointer">The pointer to add the offset to.</param>
	public static ReadOnlyValPtr<T> operator ++(ReadOnlyValPtr<T> pointer)
		=> (ReadOnlyValPtr<T>)(pointer.Pointer + sizeof(T));
	/// <summary>
	/// Subtracts an offset in <typeparamref name="T"/> units from the value of a pointer.
	/// </summary>
	/// <param name="pointer">The pointer to subtract the offset form.</param>
	/// <param name="offset">The offset in <typeparamref name="T"/> units to subtract.</param>
	public static ReadOnlyValPtr<T> operator -(ReadOnlyValPtr<T> pointer, Int32 offset)
		=> (ReadOnlyValPtr<T>)(pointer.Pointer - offset * sizeof(T));
	/// <summary>
	/// Subtracts an offset of one <typeparamref name="T"/> unit from the value of a pointer.
	/// </summary>
	/// <param name="pointer">The pointer to subtract the offset form.</param>
	public static ReadOnlyValPtr<T> operator --(ReadOnlyValPtr<T> pointer)
		=> (ReadOnlyValPtr<T>)(pointer.Pointer - sizeof(T));

	/// <summary>
	/// Adds an offset in <typeparamref name="T"/> units to the value of a pointer.
	/// </summary>
	/// <param name="pointer">The pointer to add the offset to.</param>
	/// <param name="offset">The offset in <typeparamref name="T"/> units to add.</param>
	public static ReadOnlyValPtr<T> Add(ReadOnlyValPtr<T> pointer, Int32 offset)
		=> (ReadOnlyValPtr<T>)(pointer.Pointer + offset * sizeof(T));
	/// <summary>
	/// Subtracts an offset in <typeparamref name="T"/> units to the value of a pointer.
	/// </summary>
	/// <param name="pointer">The pointer to subtract the offset to.</param>
	/// <param name="offset">The offset in <typeparamref name="T"/> units to subtract.</param>
	public static ReadOnlyValPtr<T> Subtract(ReadOnlyValPtr<T> pointer, Int32 offset)
		=> (ReadOnlyValPtr<T>)(pointer.Pointer - offset * sizeof(T));
	
	/// <inheritdoc cref="IntPtr.Parse(String)"/>
	[ExcludeFromCodeCoverage]
	public static ReadOnlyValPtr<T> Parse(String s) => (ReadOnlyValPtr<T>)IntPtr.Parse(s);
	/// <inheritdoc cref="IntPtr.Parse(String, NumberStyles)"/>
	[ExcludeFromCodeCoverage]
	public static ReadOnlyValPtr<T> Parse(String s, NumberStyles style) => (ReadOnlyValPtr<T>)IntPtr.Parse(s, style);
	/// <inheritdoc cref="IntPtr.Parse(String, IFormatProvider)"/>
	[ExcludeFromCodeCoverage]
	public static ReadOnlyValPtr<T> Parse(String s, IFormatProvider? provider)
		=> (ReadOnlyValPtr<T>)IntPtr.Parse(s, provider);
	/// <inheritdoc cref="IntPtr.Parse(String, NumberStyles, IFormatProvider)"/>
	[ExcludeFromCodeCoverage]
	public static ReadOnlyValPtr<T> Parse(String s, NumberStyles style, IFormatProvider? provider)
		=> (ReadOnlyValPtr<T>)IntPtr.Parse(s, style, provider);
	/// <inheritdoc cref="IntPtr.Parse(ReadOnlySpan{Char}, NumberStyles, IFormatProvider)"/>
	[ExcludeFromCodeCoverage]
	public static ReadOnlyValPtr<T> Parse(ReadOnlySpan<Char> s, NumberStyles style = NumberStyles.Integer,
		IFormatProvider? provider = null)
		=> (ReadOnlyValPtr<T>)IntPtr.Parse(s, style, provider);

	/// <inheritdoc cref="IntPtr.TryParse(String?, out IntPtr)"/>
	[ExcludeFromCodeCoverage]
	public static Boolean TryParse([NotNullWhen(true)] String? s, out ReadOnlyValPtr<T> result)
	{
		Unsafe.SkipInit(out result);
		return IntPtr.TryParse(s, out Unsafe.As<ReadOnlyValPtr<T>, IntPtr>(ref result));
	}
	/// <inheritdoc cref="IntPtr.TryParse(String?, NumberStyles, IFormatProvider?, out IntPtr)"/>
	[ExcludeFromCodeCoverage]
	public static Boolean TryParse([NotNullWhen(true)] String? s, NumberStyles style, IFormatProvider? provider,
		out ReadOnlyValPtr<T> result)
	{
		Unsafe.SkipInit(out result);
		return IntPtr.TryParse(s, style, provider, out Unsafe.As<ReadOnlyValPtr<T>, IntPtr>(ref result));
	}
	/// <inheritdoc cref="IntPtr.TryParse(ReadOnlySpan{Char}, out IntPtr)"/>
	[ExcludeFromCodeCoverage]
	public static Boolean TryParse(ReadOnlySpan<Char> s, out ReadOnlyValPtr<T> result)
	{
		Unsafe.SkipInit(out result);
		return IntPtr.TryParse(s, out Unsafe.As<ReadOnlyValPtr<T>, IntPtr>(ref result));
	}
	/// <inheritdoc cref="IntPtr.TryParse(ReadOnlySpan{Char}, NumberStyles, IFormatProvider?, out IntPtr)"/>
	[ExcludeFromCodeCoverage]
	public static Boolean TryParse(ReadOnlySpan<Char> s, NumberStyles style, IFormatProvider? provider,
		out ReadOnlyValPtr<T> result)
	{
		Unsafe.SkipInit(out result);
		return IntPtr.TryParse(s, style, provider, out Unsafe.As<ReadOnlyValPtr<T>, IntPtr>(ref result));
	}
}