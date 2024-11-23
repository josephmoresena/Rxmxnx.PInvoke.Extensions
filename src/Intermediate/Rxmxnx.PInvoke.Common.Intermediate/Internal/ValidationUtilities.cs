namespace Rxmxnx.PInvoke.Internal;

/// <summary>
/// Utility class for argument validation.
/// </summary>
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
internal static unsafe class ValidationUtilities
{
	/// <summary>
	/// Empty <see cref="String"/> for <see cref="CallerArgumentExpressionAttribute"/> default value.
	/// </summary>
	private const String emptyString = "";

	/// <summary>
	/// Validates that an index is valid for a list of a specific size.
	/// </summary>
	/// <param name="index">The index to validate.</param>
	/// <param name="count">The total number of elements in the list.</param>
	/// <param name="nameofIndex">
	/// The name of the index parameter. Used in the exception message if the index is invalid.
	/// </param>
	/// <exception cref="ArgumentOutOfRangeException">
	/// Thrown if the index is less than zero or greater than or equal to the count.
	/// </exception>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void ThrowIfInvalidListIndex(Int32 index, Int32 count,
		[CallerArgumentExpression(nameof(index))] String nameofIndex = ValidationUtilities.emptyString)
	{
		if (index < 0 || index >= count)
			throw new ArgumentOutOfRangeException(nameofIndex, "Index was outside the bounds of the list.");
	}

	/// <summary>
	/// Validates if the sequence index is valid for a sequence of a specific size.
	/// </summary>
	/// <param name="index">The index to validate.</param>
	/// <param name="count">The total number of elements in the sequence.</param>
	/// <param name="nameofIndex">
	/// The name of the index parameter. Used in the exception message if the index is invalid.
	/// </param>
	/// <exception cref="ArgumentOutOfRangeException">
	/// Thrown if the index is less than zero or greater than or equal to the count.
	/// </exception>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void ThrowIfInvalidSequenceIndex(Int32 index, Int32 count,
		[CallerArgumentExpression(nameof(index))] String nameofIndex = ValidationUtilities.emptyString)
	{
		if (index < 0 || index >= count)
			throw new ArgumentOutOfRangeException(nameofIndex, "Index was outside the bounds of the sequence.");
	}

	/// <summary>
	/// Validates if a pointer is fixed in memory and safe to use.
	/// </summary>
	/// <param name="isValid">A wrapper that indicates whether the fixed pointer instance is valid.</param>
	/// <exception cref="InvalidOperationException">
	/// Thrown if the fixed pointer instance is not guaranteed to be safe.
	/// </exception>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void ThrowIfInvalidPointer(IWrapper<Boolean> isValid)
	{
		if (!isValid.Value)
			throw new InvalidOperationException("The current instance is not valid.");
	}

	/// <summary>
	/// Throws an exception if <paramref name="info"/> contains an invalid <see langword="unmanaged"/> pointer.
	/// </summary>
	/// <param name="info">A <see cref="SerializationInfo"/> instance.</param>
	/// <returns>Deserialized <see langword="unmanaged"/> pointer.</returns>
	/// <exception cref="ArgumentException">
	/// Throws an exception if <paramref name="info"/> contains an invalid <see langword="unmanaged"/> pointer.
	/// </exception>
	[ExcludeFromCodeCoverage]
	public static void* ThrowIfInvalidPointer(SerializationInfo info)
	{
		Int64 l = info.GetInt64("value");
		if (IntPtr.Size == 4 && l is > Int32.MaxValue or < Int32.MinValue)
			throw new ArgumentException(
				"An IntPtr or UIntPtr with an eight byte value cannot be deserialized on a machine with a four byte word size.");
		return (void*)l;
	}
	/// <summary>
	/// Throws an exception if <paramref name="info"/> is <see langword="null"/>.
	/// </summary>
	/// <param name="info">A <see cref="SerializationInfo"/> instance.</param>
	/// <param name="ptr">An <see langword="unmanaged"/> pointer.</param>
	/// <exception cref="ArgumentNullException">
	/// Throws an exception if <paramref name="info"/> is <see langword="null"/>.
	/// </exception>
	[ExcludeFromCodeCoverage]
	public static void ThrowIfInvalidSerialization(SerializationInfo info, void* ptr)
	{
		if (info == null)
			throw new ArgumentNullException(nameof(info));
		info.AddValue("value", (Int64)ptr);
	}
	/// <summary>
	/// Throws an exception if <paramref name="obj"/> is not a value pointer.
	/// </summary>
	/// <param name="obj">A <see cref="Object"/> instance.</param>
	/// <param name="ptr">A <see cref="IntPtr"/> value.</param>
	/// <param name="nameofPtr">Name of value pointer type.</param>
	/// <typeparam name="T">Type of referenced value.</typeparam>
	/// <returns>A <see cref="Int32"/> value that indicates the relative order of the objects being compared.</returns>
	/// <exception cref="ArgumentException">Throws an exception if <paramref name="obj"/> is not a value pointer.</exception>
	public static Int32 ThrowIfInvalidValuePointer<T>(Object? obj, IntPtr ptr, String nameofPtr)
		=> obj switch
		{
			null => 1,
			ValPtr<T> v => ptr.CompareTo(v.Pointer),
			ReadOnlyValPtr<T> r => ptr.CompareTo(r.Pointer),
			_ => throw new ArgumentException($"Object must be of type {nameofPtr}."),
		};

	/// <summary>
	/// Validates if the fixed memory pointer instance is a function.
	/// </summary>
	/// <param name="isFunction">Indicates whether the fixed pointer instance is a function.</param>
	/// <exception cref="InvalidOperationException">
	/// Thrown if the fixed pointer instance is not a function.
	/// </exception>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void ThrowIfNotFunctionPointer(Boolean isFunction)
	{
		if (!isFunction)
			throw new InvalidOperationException("The current instance is not a function.");
	}

	/// <summary>
	/// Validates if the fixed memory pointer instance is not a function.
	/// </summary>
	/// <param name="isFunction">Indicates whether the fixed pointer instance is a function.</param>
	/// <exception cref="InvalidOperationException">
	/// Thrown if the fixed pointer instance is a function.
	/// </exception>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void ThrowIfFunctionPointer(Boolean isFunction)
	{
		if (isFunction)
			throw new InvalidOperationException("The current instance is a function.");
	}

	/// <summary>
	/// Validates that a non-read-only operation is not attempted on a fixed memory pointer instance that is read-only.
	/// </summary>
	/// <param name="isReadOnlyOperation">Indicates whether the operation to be performed is read-only.</param>
	/// <param name="isReadOnly">Indicates whether the fixed pointer instance is read-only.</param>
	/// <exception cref="InvalidOperationException">
	/// Thrown if a non-read-only operation is attempted on a fixed memory pointer instance that is read-only.
	/// </exception>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void ThrowIfReadOnlyPointer(Boolean isReadOnlyOperation, Boolean isReadOnly)
	{
		if (!isReadOnlyOperation && isReadOnly)
			throw new InvalidOperationException("The current instance is read-only.");
	}

	/// <summary>
	/// Validates that the binary size of the fixed memory pointer instance is sufficient to contain at least one
	/// <paramref name="typeOf"/> value.
	/// </summary>
	/// <param name="binaryLength">Binary size of the fixed memory pointer instance.</param>
	/// <param name="typeOf">CLR Type.</param>
	/// <param name="sizeOf">Type size in bytes.</param>
	/// <exception cref="InsufficientMemoryException">
	/// Thrown if the binary size of the fixed memory pointer instance is insufficient to contain at least one
	/// <paramref name="typeOf"/> value.
	/// </exception>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void ThrowIfInvalidRefTypePointer(Int32 binaryLength, Type typeOf, Int32 sizeOf)
	{
		if (binaryLength < sizeOf)
			throw new InsufficientMemoryException(
				$"The current instance is insufficient to contain a value of {typeOf} type.");
	}

	/// <summary>
	/// Validates that the size of the binary span exactly matches with <paramref name="sizeOf"/>.
	/// </summary>
	/// <param name="span">Binary span.</param>
	/// <param name="nameofSpan">Name of the span.</param>
	/// <param name="sizeOf">Type size in bytes.</param>
	/// <exception cref="InsufficientMemoryException">
	/// Thrown if the size of the binary span is less than <paramref name="sizeOf"/>.
	/// </exception>
	/// <exception cref="InvalidCastException">
	/// Thrown if the size of the binary span is greater than <paramref name="sizeOf"/>.
	/// </exception>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void ThrowIfInvalidBinarySpanSize(ReadOnlySpan<Byte> span, Int32 sizeOf,
		[CallerArgumentExpression(nameof(span))] String nameofSpan = ValidationUtilities.emptyString)
	{
		String message = $"The length of parameter {nameofSpan} must be equals to {sizeOf}.";
		if (span.Length < sizeOf)
			throw new InsufficientMemoryException(message);
		if (span.Length > sizeOf)
			throw new InvalidCastException(message);
	}

	/// <summary>
	/// Validates if the object <paramref name="obj"/> is of type <typeparamref name="T"/> and assigns it to the output
	/// parameter <paramref name="result"/>.
	/// </summary>
	/// <typeparam name="T">The expected type of the object.</typeparam>
	/// <param name="obj">The object to check and cast.</param>
	/// <param name="typeName">The name of the expected type.</param>
	/// <param name="result">
	/// Output parameter. If the object is of type <typeparamref name="T"/>, this will contain the casted object.
	/// </param>
	/// <param name="nameofObj">
	/// The name of the object parameter. Used in the exception message if the object is not of type <typeparamref name="T"/>.
	/// </param>
	/// <exception cref="ArgumentException">
	/// Thrown if the object <paramref name="obj"/> is not of type <typeparamref name="T"/>.
	/// </exception>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void ThrowIfInvalidCastType<T>(Object? obj, String typeName, out T result,
		[CallerArgumentExpression(nameof(obj))] String nameofObj = ValidationUtilities.emptyString)
	{
		if (obj is not T value)
			throw new ArgumentException($"Object must be of type {typeName}.", nameofObj);
		result = value;
	}

	/// <summary>
	/// Validates if <see langword="unmanaged"/> memory reference of <paramref name="sourceSize"/> can be safely
	/// converted to <paramref name="destinationSize"/> reference.
	/// </summary>
	/// <param name="destinationSize">Source type size.</param>
	/// <param name="sourceSize">Destination type size.</param>
	/// <exception cref="InvalidOperationException">
	/// Thrown if the sizes of both source and destination <see langword="unmanaged"/> types are not equal.
	/// </exception>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void ThrowIfInvalidCastType(Int32 destinationSize, Int32 sourceSize)
	{
		if (destinationSize != sourceSize)
			throw new InvalidOperationException(
				"The sizes of both source and destination unmanaged types must be equal.");
	}

	/// <summary>
	/// Validates if the binary span <paramref name="destination"/> is sufficient to contain the binary
	/// information of <paramref name="value"/>.
	/// Outputs the value as binary span.
	/// </summary>
	/// <typeparam name="TValue">Type of the copiable <see langword="unmanaged"/> value.</typeparam>
	/// <param name="value">Value to copy.</param>
	/// <param name="destination">Binary span destination.</param>
	/// <param name="offset">Offset of copy.</param>
	/// <param name="bytes">Output parameter. The binary representation of <paramref name="value"/>.</param>
	/// <exception cref="InsufficientMemoryException">
	/// Thrown if the destination span does not have enough space to contain the binary representation of the
	/// <see langword="unmanaged"/> value.
	/// </exception>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void ThrowIfInvalidCopyType<TValue>(in TValue value, Span<Byte> destination, Int32 offset,
		out ReadOnlySpan<Byte> bytes) where TValue : unmanaged
	{
		ref TValue refValue = ref Unsafe.AsRef(in value);
		ReadOnlySpan<TValue> intermediateSpan = MemoryMarshal.CreateReadOnlySpan(ref refValue, 1);
		bytes = MemoryMarshal.AsBytes(intermediateSpan);
		if (destination.Length - offset < bytes.Length)
			throw new InsufficientMemoryException(
				$"Insufficient available size on {nameof(destination)} to copy {nameof(value)}.");
	}

	/// <summary>
	/// Validates the memory length.
	/// </summary>
	/// <param name="length">Memory length value.</param>
	/// <param name="nameofLength">Name of the memory length parameter.</param>
	/// <exception cref="ArgumentException">Thrown if <paramref name="length"/> is less than zero.</exception>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void ThrowIfInvalidMemoryLength(Int32 length,
		[CallerArgumentExpression(nameof(length))] String nameofLength = ValidationUtilities.emptyString)
	{
		if (length < 0)
			throw new ArgumentException($"The parameter {nameofLength} must be zero or positive integer.");
	}

	/// <summary>
	/// Validates the current index in an iteration of an enumeration.
	/// </summary>
	/// <param name="index">Current index of iteration.</param>
	/// <param name="enumerationSize">Total size of the enumeration.</param>
	/// <exception cref="InvalidOperationException">
	/// Thrown if <paramref name="index"/> is less than zero or greater than or equal to <paramref name="enumerationSize"/>.
	/// </exception>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void ThrowIfInvalidIndexEnumerator(Int32 index, Int32 enumerationSize)
	{
		if (index < 0)
			throw new InvalidOperationException("Enumeration has not started. Call MoveNext.");
		if (index >= enumerationSize)
			throw new InvalidOperationException("Enumeration already finished.");
	}

	/// <summary>
	/// Validates if the <paramref name="region"/> is a local UTF-8 string.
	/// </summary>
	/// <param name="region">A <see cref="ValueRegion{Byte}"/> instance.</param>
	/// <param name="nameofRegion">Name of the region parameter.</param>
	/// <param name="result">Output. Binary information in <paramref name="region"/>.</param>
	/// <exception cref="InvalidOperationException">
	/// Thrown if <paramref name="region"/> does not contain UTF-8 text.
	/// </exception>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void ThrowIfInvalidUtf8Region(ValueRegion<Byte> region, String nameofRegion, out Byte[] result)
	{
		result = (Byte[])region!;
		if (result is null)
			throw new InvalidOperationException($"{nameofRegion} does not contains the UTF-8 text.");
	}

	/// <summary>
	/// Validates the parameters of a subregion.
	/// </summary>
	/// <param name="regionLength">Length of the region.</param>
	/// <param name="startIndex">
	/// The zero-based starting item position of a subregion in the region.
	/// </param>
	/// <param name="length">The number of items in the subregion.</param>
	/// <param name="nameofStartIndex">Name of the start index parameter.</param>
	/// <param name="nameofLength">Name of the length parameter.</param>
	/// <exception cref="ArgumentOutOfRangeException">
	/// Thrown if <paramref name="startIndex"/> or <paramref name="length"/> are out of range.
	/// </exception>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void ThrowIfInvalidSubregion(Int32 regionLength, Int32 startIndex, Int32 length,
		[CallerArgumentExpression(nameof(startIndex))] String nameofStartIndex = ValidationUtilities.emptyString,
		[CallerArgumentExpression(nameof(length))] String nameofLength = ValidationUtilities.emptyString)
	{
		if (startIndex < 0)
			throw new ArgumentOutOfRangeException(nameofStartIndex, "StartIndex cannot be less than zero.");

		if (startIndex > regionLength)
			throw new ArgumentOutOfRangeException(nameofStartIndex,
			                                      $"{nameofStartIndex} cannot be larger than length of region.");

		if (length < 0)
			throw new ArgumentOutOfRangeException(nameofLength, "Length cannot be less than zero.");

		if (startIndex > regionLength - length)
			throw new ArgumentOutOfRangeException(nameofLength,
			                                      "Index and length must refer to a location within the region.");
	}

	/// <summary>
	/// Validates the parameters of a substring.
	/// </summary>
	/// <param name="stringLength">Length of the string.</param>
	/// <param name="startIndex">
	/// The zero-based starting item position of a substring in the region.
	/// </param>
	/// <param name="length">The number of items in the substring.</param>
	/// <param name="nameofStartIndex">Name of the start index parameter.</param>
	/// <param name="nameofLength">Name of the length parameter.</param>
	/// <exception cref="ArgumentOutOfRangeException">
	/// Thrown if <paramref name="startIndex"/> or <paramref name="length"/> are out of range.
	/// </exception>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void ThrowIfInvalidSubstring(Int32 stringLength, Int32 startIndex, Int32 length,
		[CallerArgumentExpression(nameof(startIndex))] String nameofStartIndex = ValidationUtilities.emptyString,
		[CallerArgumentExpression(nameof(length))] String nameofLength = ValidationUtilities.emptyString)
	{
		if (startIndex < 0)
			throw new ArgumentOutOfRangeException(nameofStartIndex, "StartIndex cannot be less than zero.");

		if (startIndex > stringLength)
			throw new ArgumentOutOfRangeException(nameofStartIndex,
			                                      $"{nameofStartIndex} cannot be larger than length of string.");

		if (length < 0)
			throw new ArgumentOutOfRangeException(nameofLength, "Length cannot be less than zero.");

		if (startIndex > stringLength - length)
			throw new ArgumentOutOfRangeException(nameofLength,
			                                      "Index and length must refer to a location within the string.");
	}

	/// <summary>
	/// Validates the parameters of a subsequence.
	/// </summary>
	/// <param name="sequenceLength">Length of the subsequence.</param>
	/// <param name="startIndex">
	/// The zero-based starting UTF-8 string position of a subsequence in this instance.
	/// </param>
	/// <param name="length">The number of UTF-8 strings in the subsequence.</param>
	/// <param name="nameofStartIndex">Name of the start index parameter.</param>
	/// <param name="nameofLength">Name of the length parameter.</param>
	/// <exception cref="ArgumentOutOfRangeException">
	/// Thrown if <paramref name="startIndex"/> or <paramref name="length"/> are out of range.
	/// </exception>
	/// <exception cref="ArgumentOutOfRangeException"/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void ThrowIfInvalidSubsequence(Int32 sequenceLength, Int32 startIndex, Int32 length,
		[CallerArgumentExpression(nameof(startIndex))] String nameofStartIndex = ValidationUtilities.emptyString,
		[CallerArgumentExpression(nameof(length))] String nameofLength = ValidationUtilities.emptyString)
	{
		if (startIndex < 0)
			throw new ArgumentOutOfRangeException(nameofStartIndex, "StartIndex cannot be less than zero.");

		if (startIndex > sequenceLength)
			throw new ArgumentOutOfRangeException(nameofStartIndex,
			                                      $"{nameofStartIndex} cannot be larger than length of sequence.");

		if (length < 0)
			throw new ArgumentOutOfRangeException(nameofLength, "Length cannot be less than zero.");

		if (startIndex > sequenceLength - length)
			throw new ArgumentOutOfRangeException(nameofLength,
			                                      "Index and length must refer to a location within the sequence.");
	}
	/// <summary>
	/// Throws an exception if buffer is not a space.
	/// </summary>
	/// <param name="isBinary">Indicates whether buffer is binary.</param>
	/// <param name="bufferSize">Buffer sizes.</param>
	/// <param name="type">CLR type of buffer.</param>
	/// <exception cref="InvalidOperationException">Throws an exception if buffer is not a space.</exception>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void ThrowIfNotSpace(Boolean isBinary, Span<UInt16> bufferSize, Type type)
	{
		if (!isBinary || bufferSize[0] + 1 != 2 * bufferSize[1] || bufferSize[1] != 1 + bufferSize[2])
			throw new InvalidOperationException(
				$"{type} is not an space. Size: {bufferSize[0]} ({bufferSize[2]}, {bufferSize[1]}).");
	}
	/// <summary>
	/// Throws an exception if <paramref name="type"/> is not unmanaged.
	/// </summary>
	/// <param name="type">CLR type.</param>
	/// <param name="isUnmanaged">Indicates whether <paramref name="type"/> is unmanaged.</param>
	/// <exception cref="InvalidOperationException">
	/// Throws an exception if <paramref name="type"/> is not a value type.
	/// </exception>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void ThrowIfNotUnmanagedType(Type? type, Boolean isUnmanaged)
	{
		if (type is not null && !type.IsValueType)
			throw new InvalidOperationException($"{type} is not a value type.");
	}
	/// <summary>
	/// Throws an exception if transformation from <paramref name="sourceType"/> to
	/// <paramref name="destinationType"/> is not allowed.
	/// </summary>
	/// <param name="sourceType">Source type.</param>
	/// <param name="unmanagedSource">Indicates whether <paramref name="sourceType"/> is unmanaged.</param>
	/// <param name="destinationType">Destination type.</param>
	/// <param name="unmanagedDestination">Indicates whether <paramref name="destinationType"/> is unmanaged.</param>
	/// <exception cref="InvalidOperationException">
	/// Throws an exception if transformation from <paramref name="sourceType"/> to
	/// <paramref name="destinationType"/> is not allowed.
	/// </exception>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void ThrowIfInvalidTransformation(Type? sourceType, Boolean unmanagedSource, Type destinationType,
		Boolean unmanagedDestination)
	{
		if (!destinationType.IsValueType)
		{
			if (sourceType is null || destinationType.IsValueType)
				throw new InvalidOperationException($"{destinationType} is not a value type.");
		}
		else if (!unmanagedSource)
		{
			throw new InvalidOperationException($"{sourceType ?? typeof(Byte)} is not a unmanaged type.");
		}
		else if (!unmanagedDestination)
		{
			throw new InvalidOperationException($"{destinationType} is not a unmanaged type.");
		}
	}
}