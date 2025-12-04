#if !NET6_0_OR_GREATER
using MemoryMarshalCompat = Rxmxnx.PInvoke.Internal.FrameworkCompat.MemoryMarshalCompat;
#endif

#if !NET5_0_OR_GREATER
using Convert = Rxmxnx.PInvoke.Internal.FrameworkCompat.ConvertCompat;
#endif

namespace Rxmxnx.PInvoke;

/// <summary>
/// Represents a sequence of UTF-8 encoded characters.
/// </summary>
#if NET7_0_OR_GREATER
[NativeMarshalling(typeof(Marshaller))]
#endif
[DebuggerDisplay("{ToString()}")]
[DebuggerTypeProxy(typeof(CStringDebugView))]
#if !PACKAGE || NETCOREAPP
[JsonConverter(typeof(JsonConverter))]
#endif
public sealed partial class CString : ICloneable, IEquatable<CString>, IEquatable<String>
{
	/// <summary>
	/// Represents an empty UTF-8 string. This field is read-only.
	/// </summary>
	/// <remarks>This instance is a UTF-8 literal.</remarks>
	public static readonly CString Empty = new(ValueRegion<Byte>.Create(AotInfo.EmptyUt8Text), true, true, 0);
	/// <summary>
	/// Represents a null-pointer UTF-8 string. This field is read-only.
	/// </summary>
	public static readonly CString Zero = new(IntPtr.Zero, 0, true);

	/// <summary>
	/// Gets a value indicating whether the text in the current <see cref="CString"/> instance
	/// ends with a null-termination character.
	/// </summary>
	public Boolean IsNullTerminated => this._isNullTerminated;
	/// <summary>
	/// Gets a value indicating whether the UTF-8 text is referenced by, and not contained within,
	/// the current <see cref="CString"/> instance.
	/// </summary>
	public Boolean IsReference => !this._isLocal && !this.IsFunction;
	/// <summary>
	/// Gets a value indicating whether the current <see cref="CString"/> instance is a segment
	/// (or slice) of another <see cref="CString"/> instance.
	/// </summary>
	public Boolean IsSegmented => this._data.IsMemorySlice;
	/// <summary>
	/// Gets a value indicating whether the current <see cref="CString"/> instance is a function.
	/// </summary>
	public Boolean IsFunction { get; }
	/// <summary>
	/// Indicates whether the current <see cref="CString"/> instance is a null pointer.
	/// </summary>
	public Boolean IsZero => this.IsReference && Unsafe.IsNullRef(ref MemoryMarshal.GetReference(this._data.AsSpan()));

	/// <summary>
	/// Initializes a new instance of the <see cref="CString"/> class to the value indicated by a specified
	/// UTF-8 character repeated a specified number of times.
	/// </summary>
	/// <param name="c">A UTF-8 char.</param>
	/// <param name="count">The number of times <paramref name="c"/> is repeated to form the UTF-8 string.</param>
	public CString(Byte c, Int32 count) : this(CString.CreateRepeatedSequence([c,], count), true) { }
	/// <summary>
	/// Initializes a new instance of the <see cref="CString"/> class to the value indicated by a specified
	/// UTF-8 sequence repeated a specified number of times.
	/// </summary>
	/// <param name="u0">The first UTF-8 unit.</param>
	/// <param name="u1">The second UTF-8 unit.</param>
	/// <param name="count">The number of times the sequence is repeated to form the UTF-8 string.</param>
	public CString(Byte u0, Byte u1, Int32 count) : this(CString.CreateRepeatedSequence([u0, u1,], count), true) { }
	/// <summary>
	/// Initializes a new instance of the <see cref="CString"/> class to the value indicated by a specified
	/// UTF-8 sequence repeated a specified number of times.
	/// </summary>
	/// <param name="u0">The first UTF-8 unit.</param>
	/// <param name="u1">The second UTF-8 unit.</param>
	/// <param name="u2">The third UTF-8 unit.</param>
	/// <param name="count">The number of times the sequence is repeated to form the UTF-8 string.</param>
	public CString(Byte u0, Byte u1, Byte u2, Int32 count) : this(CString.CreateRepeatedSequence([u0, u1, u2,], count),
	                                                              true) { }
	/// <summary>
	/// Initializes a new instance of the <see cref="CString"/> class to the value indicated by a specified
	/// UTF-8 sequence repeated a specified number of times.
	/// </summary>
	/// <param name="u0">The first UTF-8 unit.</param>
	/// <param name="u1">The second UTF-8 unit.</param>
	/// <param name="u2">The third UTF-8 unit.</param>
	/// <param name="u3">The fourth UTF-8 unit.</param>
	/// <param name="count">The number of times the sequence is repeated to form the UTF-8 string.</param>
	public CString(Byte u0, Byte u1, Byte u2, Byte u3, Int32 count) : this(
		CString.CreateRepeatedSequence([u0, u1, u2, u3,], count), true) { }
	/// <summary>
	/// Initializes a new instance of the <see cref="CString"/> class to the value indicated by a specified
	/// UTF-16 character repeated a specified number of times.
	/// </summary>
	/// <param name="c">A UTF-16 char.</param>
	/// <param name="count">The number of times <paramref name="c"/> is repeated to form the UTF-8 string.</param>
	public CString(Char c, Int32 count) : this(CString.CreateRepeatedSequence([c,], count), true) { }
	/// <summary>
	/// Initializes a new instance of the <see cref="CString"/> class to the value indicated by a specified
	/// UTF-16 sequence repeated a specified number of times.
	/// </summary>
	/// <param name="u0">The first UTF-16 unit.</param>
	/// <param name="u1">The second UTF-16 unit.</param>
	/// <param name="count">The number of times the sequence is repeated to form the UTF-8 string.</param>
	public CString(Char u0, Char u1, Int32 count) : this(CString.CreateRepeatedSequence([u0, u1,], count), true) { }
	/// <summary>
	/// Initializes a new instance of the <see cref="CString"/> class using the UTF-8 characters
	/// indicated in the specified read-only span.
	/// </summary>
	/// <param name="source">A read-only span of UTF-8 characters to initialize the new instance.</param>
	public CString(ReadOnlySpan<Byte> source) : this(CString.CreateRepeatedSequence(source, 1)) { }
	/// <summary>
	/// Initializes a new instance of the <see cref="CString"/> class using the UTF-16 characters
	/// indicated in the specified read-only span.
	/// </summary>
	/// <param name="source">A read-only span of UTF-16 characters to initialize the new instance.</param>
	public CString(ReadOnlySpan<Char> source) : this(CString.CreateRepeatedSequence(source, 1)) { }
	/// <summary>
	/// Initializes a new instance of the <see cref="CString"/> class that contains the UTF-8 string
	/// returned by the specified <see cref="ReadOnlySpanFunc{Byte}"/>.
	/// </summary>
	/// <param name="func">
	/// <see cref="ReadOnlySpanFunc{Byte}"/> delegate that returns the UTF-8 string to initialize
	/// the new instance.
	/// </param>
	public CString(ReadOnlySpanFunc<Byte> func) : this(func, true) { }
	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Object Clone()
	{
		ReadOnlySpan<Byte> source = this;
		Byte[] bytes = new Byte[this._length + 1];
		source.CopyTo(bytes);
		return new CString(bytes, true);
	}

	/// <inheritdoc/>
	public Boolean Equals([NotNullWhen(true)] CString? other)
	{
		if (other is null) return false;
		if (other.IsZero) return this.IsZero;
		return !this.IsZero && CString.equals(this, other);
	}
	/// <summary>
	/// Determines whether the current <see cref="CString"/> and a specified <see cref="String"/>
	/// have the same value.
	/// </summary>
	/// <param name="other">The <see cref="String"/> to compare to the current instance.</param>
	/// <returns>
	/// <see langword="true"/> if the value of the <paramref name="other"/> parameter is the same
	/// as this <see cref="CString"/>, otherwise, <see langword="false"/>.
	/// </returns>
	public Boolean Equals([NotNullWhen(true)] String? other)
		=> other is not null && !this.IsZero && StringUtf8Comparator.OrdinalComparator.TextEquals(this, other);
	/// <summary>
	/// Determines whether the current <see cref="CString"/> and a specified <see cref="CString"/>
	/// have the same value.
	/// A parameter specifies the culture, case, and sort rules used in the comparison.
	/// </summary>
	/// <param name="value">The <see cref="CString"/> to compare to the current instance.</param>
	/// <param name="comparisonType">
	/// One of the enumeration values that specifies how the strings will be compared.
	/// </param>
	/// <returns>
	/// <see langword="true"/> if the value of the <paramref name="value"/> parameter is the same
	/// as this <see cref="CString"/>, otherwise, <see langword="false"/>.
	/// </returns>
	public Boolean Equals([NotNullWhen(true)] CString? value, StringComparison comparisonType)
	{
		if (value is null) return false;
		if (value.IsZero) return this.IsZero;
		if (this.IsZero) return false;
		return comparisonType is StringComparison.Ordinal ?
			CString.equals(this, value) :
			CStringUtf8Comparator.Create(comparisonType).TextEquals(this, value);
	}
	/// <summary>
	/// Determines whether the current <see cref="CString"/> and a specified <see cref="String"/>
	/// have the same value.
	/// A parameter specifies the culture, case, and sort rules used in the comparison.
	/// </summary>
	/// <param name="value">The <see cref="String"/> to compare to the current instance.</param>
	/// <param name="comparisonType">
	/// One of the enumeration values that specifies how the strings will be compared.
	/// </param>
	/// <returns>
	/// <see langword="true"/> if the value of the <paramref name="value"/> parameter is the same as this
	/// <see cref="CString"/>, otherwise, <see langword="false"/>.
	/// </returns>
	public Boolean Equals([NotNullWhen(true)] String? value, StringComparison comparisonType)
	{
		if (value is null) return false;
		StringUtf8Comparator comparator = comparisonType is StringComparison.Ordinal ?
			StringUtf8Comparator.OrdinalComparator :
			StringUtf8Comparator.Create(comparisonType);
		return comparator.TextEquals(this, value);
	}

	/// <inheritdoc/>
	public override Boolean Equals([NotNullWhen(true)] Object? obj)
		=> obj is String str ? this.Equals(str) : this.Equals(obj as CString);
	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override String ToString()
	{
		if (this._length == 0) return String.Empty;
		String result = Encoding.UTF8.GetString(this.AsSpan());
		return String.IsInterned(result) ?? result;
	}
	/// <inheritdoc/>
	public override Int32 GetHashCode() => this.ToString().GetHashCode();

	/// <summary>
	/// Returns a reference to the first UTF-8 unit of the <see cref="CString"/>.
	/// </summary>
	/// <returns>A reference to the first UTF-8 unit of the <see cref="CString"/>.</returns>
	/// <remarks>
	/// This method is used to support the use of a <see cref="CString"/> within a fixed statement.
	/// It should not be used in typical code.
	/// </remarks>
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public ref readonly Byte GetPinnableReference() => ref MemoryMarshal.GetReference(this._data.AsSpan());
	/// <summary>
	/// Copies the UTF-8 text of the current <see cref="CString"/> instance into a new byte array.
	/// </summary>
	/// <returns>
	/// A new <see cref="Byte"/> array containing the UTF-8 units of the current <see cref="CString"/>.
	/// </returns>
	public Byte[] ToArray() => this._data.ToArray()[..this._length];
	/// <summary>
	/// Retrieves the UTF-8 units of the current <see cref="CString"/> as a read-only span of bytes.
	/// </summary>
	/// <returns>
	/// A read-only span of bytes representing the UTF-8 units of the current <see cref="CString"/>.
	/// </returns>
	public ReadOnlySpan<Byte> AsSpan() => this._data.AsSpan()[..this._length];
	/// <summary>
	/// Returns a <see cref="String"/> that represents the current UTF-8 text as a hexadecimal value.
	/// </summary>
	/// <returns>
	/// A <see cref="String"/> that represents the hexadecimal value of the current UTF-8 text.
	/// </returns>
	public String ToHexString() => Convert.ToHexString(this).ToLowerInvariant();
	/// <summary>
	/// Returns an enumerator that iterates through the UTF-8 units of the current <see cref="CString"/>.
	/// </summary>
	/// <returns>
	/// An enumerator that can be used to iterate through the UTF-8 units of the current
	/// <see cref="CString"/>.
	/// </returns>
	public ReadOnlySpan<Byte>.Enumerator GetEnumerator() => this.AsSpan().GetEnumerator();

	/// <summary>
	/// Tries to pin the current UTF-8 memory block.
	/// </summary>
	/// <param name="pinned">Output. Indicates whether current instance was pinned.</param>
	/// <returns>A <see cref="MemoryHandle"/> instance.</returns>
	public unsafe MemoryHandle TryPin(out Boolean pinned)
	{
		if (this._data.GetPinnable(out Int32 index) is { } p)
		{
			pinned = true;
			return p.Pin(index);
		}

		fixed (void* ptr = &MemoryMarshal.GetReference(this._data.AsSpan()))
		{
			if (this.IsReference)
			{
				pinned = false;
				return new(ptr);
			}
			if (this._data.TryAlloc(GCHandleType.Pinned, out GCHandle handle))
			{
				pinned = true;
				return new(ptr, handle);
			}
		}

		pinned = false;
		return default;
	}

	/// <summary>
	/// Determines whether the specified <see cref="CString"/> is <see langword="null"/>
	/// or an empty UTF-8 string.
	/// </summary>
	/// <param name="value">The <see cref="CString"/> to test.</param>
	/// <returns>
	/// <see langword="true"/> if the value parameter is <see langword="null"/> or an empty UTF-8 string;
	/// otherwise, <see langword="false"/>.
	/// </returns>
	public static Boolean IsNullOrEmpty([NotNullWhen(false)] CString? value) => value is null || value._length == 0;
	/// <summary>
	/// Creates a new instance of the <see cref="CString"/> class using the UTF-8 characters provided
	/// in the specified read-only span.
	/// </summary>
	/// <param name="source">A read-only span of UTF-8 characters.</param>
	/// <returns>A new instance of the <see cref="CString"/> class.</returns>
	public static CString Create(ReadOnlySpan<Byte> source)
		=> source.Length == 0 ? CString.Empty : new(source.ToArray(), false);
	/// <summary>
	/// Creates a new instance of the <see cref="CString"/> class using the
	/// <see cref="ReadOnlySpanFunc{Byte}"/> delegate provided.
	/// </summary>
	/// <param name="func">
	/// A <see cref="ReadOnlySpanFunc{Byte}"/> delegate that returns a UTF-8 string non-literal.
	/// </param>
	/// <returns>
	/// A new instance of the <see cref="CString"/> class, if the func is not <see langword="null"/>;
	/// otherwise, <see langword="null"/>.
	/// </returns>
	[return: NotNullIfNotNull(nameof(func))]
	public static CString? Create(ReadOnlySpanFunc<Byte>? func) => func is not null ? new(func, false) : default;
	/// <summary>
	/// Creates a new instance of the <see cref="CString"/> class using the binary internal
	/// information provided.
	/// </summary>
	/// <param name="bytes">Binary internal information.</param>
	/// <returns>
	/// A new instance of the <see cref="CString"/> class, if the bytes are not <see langword="null"/>;
	/// otherwise, <see langword="null"/>.
	/// </returns>
	[return: NotNullIfNotNull(nameof(bytes))]
	public static CString? Create(Byte[]? bytes)
	{
		if (bytes is null) return default;
		return bytes.Length == 0 ? CString.Empty : new(bytes, false);
	}
	/// <summary>
	/// Creates a new instance of the <see cref="CString"/> class using a <typeparamref name="TState"/> instance.
	/// </summary>
	/// <typeparam name="TState">Type of the state object.</typeparam>
	/// <param name="state">Function state parameter.</param>
	/// <param name="getSpan">Function to retrieve utf-8 span from the state.</param>
	/// <param name="isNullTerminated">Indicates whether resulting UTF-8 text is null-terminated.</param>
	/// <param name="alloc">Function to allocate a <see cref="GCHandle"/> for the state.</param>
	/// <returns>
	/// A new instance of the <see cref="CString"/> class.
	/// </returns>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static CString Create<TState>(TState state, ReadOnlySpanFunc<Byte, TState> getSpan, Boolean isNullTerminated,
		Func<TState, GCHandleType, GCHandle>? alloc = default) where TState : struct
	{
		ValueRegion<Byte> data = ValueRegion<Byte>.Create(state, getSpan, alloc);
		return new(data, true, isNullTerminated, getSpan(state).Length);
	}
#if NET7_0_OR_GREATER
	/// <summary>
	/// Creates a new instance of the <see cref="CString"/> class using a <typeparamref name="TState"/> instance.
	/// </summary>
	/// <typeparam name="TState">A <see cref="IUtf8FunctionState{TState}"/> type.</typeparam>
	/// <param name="state">Function state parameter.</param>
	/// <returns>
	/// A new instance of the <see cref="CString"/> class.
	/// </returns>
	public static CString Create<TState>(TState state) where TState : struct, IUtf8FunctionState<TState>
	{
		ValueRegion<Byte> data = ValueRegion<Byte>.Create(state, TState.GetSpan, TState.Alloc);
		Int32 length = TState.GetLength(state);
		return new(data, true, state.IsNullTerminated, length);
	}
#endif
	/// <summary>
	/// Creates a new instance of the <see cref="CString"/> class using the pointer to a UTF-8
	/// character array and length provided.
	/// </summary>
	/// <param name="ptr">A pointer to an array of UTF-8 characters.</param>
	/// <param name="length">The number of <see cref="Byte"/> within value to use.</param>
	/// <param name="useFullLength">Indicates whether the total length should be used.</param>
	/// <returns>A new instance of the <see cref="CString"/> class.</returns>
	/// <remarks>
	/// The reliability of the returned <see cref="CString"/> depends on the lifetime and validity of the pointer.
	/// If the memory containing the UTF-8 text is moved or deallocated, accessing the span can cause unexpected behavior
	/// or application crashes.
	/// </remarks>
	public static CString CreateUnsafe(IntPtr ptr, Int32 length, Boolean useFullLength = false)
		=> ptr != IntPtr.Zero ? new(ptr, length, useFullLength) : CString.Zero;
	/// <summary>
	/// Creates a new instance of the <see cref="CString"/> class using the pointer to a UTF-8
	/// character array.
	/// </summary>
	/// <param name="ptr">A pointer to an array of UTF-8 characters.</param>
	/// <returns>A new instance of the <see cref="CString"/> class.</returns>
	/// <remarks>
	/// The reliability of the returned <see cref="CString"/> depends on the lifetime and validity of the pointer.
	/// If the memory containing the UTF-8 text is moved or deallocated, accessing the span can cause unexpected behavior
	/// or application crashes.
	/// </remarks>
	public static unsafe CString CreateNullTerminatedUnsafe(IntPtr ptr)
	{
		ReadOnlySpan<Byte> span =
#if NET6_0_OR_GREATER
			MemoryMarshal.CreateReadOnlySpanFromNullTerminated((Byte*)ptr);
#else
			MemoryMarshalCompat.CreateReadOnlySpanFromNullTerminated((Byte*)ptr);
#endif
		Int32 textLength = span.Length;
		Int32 regionLength = textLength + 1; // Region should include null-terminated char.
		return new(ValueRegion<Byte>.Create(ptr, regionLength), false, true, textLength);
	}
}