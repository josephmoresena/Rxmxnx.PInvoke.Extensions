namespace Rxmxnx.PInvoke.Internal;

/// <summary>
/// Utility class for argument validation.
/// </summary>
public static class ValidationUtilities
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
    public static void ThrowIfInvalidListIndex(Int32 index, Int32 count, [CallerArgumentExpression(nameof(index))] String nameofIndex = emptyString)
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
    public static void ThrowIfInvalidSequenceIndex(Int32 index, Int32 count, [CallerArgumentExpression(nameof(index))] String nameofIndex = emptyString)
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
    /// value of type <typeparamref name="TValue"/>.
    /// </summary>
    /// <typeparam name="TValue">Type of the referenced value.</typeparam>
    /// <param name="binaryLength">Binary size of the fixed memory pointer instance.</param>
    /// <exception cref="InsufficientMemoryException">
    /// Thrown if the binary size of the fixed memory pointer instance is insufficient to contain at least one 
    /// value of type <typeparamref name="TValue"/>.
    /// </exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe void ThrowIfInvalidRefTypePointer<TValue>(Int32 binaryLength)
        where TValue : unmanaged
    {
        if (binaryLength < sizeof(TValue))
            throw new InsufficientMemoryException($"The current instance is insufficent to contain a value of {typeof(TValue)} type.");
    }

    /// <summary>
    /// Validates that the size of the binary span exactly matches the size of the type <typeparamref name="TValue"/>.
    /// </summary>
    /// <typeparam name="TValue">Type of the referenced value.</typeparam>
    /// <param name="span">Binary span.</param>
    /// <param name="nameofSpan">Name of the span.</param>
    /// <exception cref="InsufficientMemoryException">
    /// Thrown if the size of the binary span is less than the size of the type <typeparamref name="TValue"/>.
    /// </exception>
    /// <exception cref="InvalidCastException">
    /// Thrown if the size of the binary span is greater than the size of the type <typeparamref name="TValue"/>.
    /// </exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe void ThrowIfInvalidBinarySpanSize<TValue>(ReadOnlySpan<Byte> span, [CallerArgumentExpression(nameof(span))] String nameofSpan = emptyString)
        where TValue : unmanaged
    {
        Int32 typeSize = sizeof(TValue);
        String message = $"The length of parameter {nameofSpan} must be equals to {typeSize}.";
        if (span.Length < typeSize)
            throw new InsufficientMemoryException(message);
        else if (span.Length > typeSize)
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
        [CallerArgumentExpression(nameof(obj))] String nameofObj = emptyString)
    {
        if (obj is not T value)
            throw new ArgumentException($"Object must be of type {typeName}.", nameofObj);
        result = value;
    }

    /// <summary>
    /// Throws an exception if memory reference <typeparamref name="TSource"/> cannot be converted to
    /// reference <typeparamref name="TDestination"/>.
    /// </summary>
    /// <exception cref="InvalidOperationException"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe void ThrowIfInvalidCastType<TSource, TDestination>()
        where TSource : unmanaged
        where TDestination : unmanaged
    {
        if (sizeof(TDestination) != sizeof(TSource))
            throw new InvalidOperationException("The sizes of both source and destination unmanaged types must be equal.");
    }

    /// <summary>
    /// Throws an exception if <paramref name="destination"/> is insufficent to contain the binary information of
    /// <paramref name="value"/>.
    /// </summary>
    /// <typeparam name="TValue">Type of the copiable value.</typeparam>
    /// <param name="value">Value to copy.</param>
    /// <param name="destination">Binary span destination.</param>
    /// <param name="offset">Offset of copy.</param>
    /// <param name="bytes">Output. <paramref name="value"/> as binary span.</param>
    /// <exception cref="ArgumentException"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ThrowIfInvalidCopyType<TValue>(in TValue value, Span<Byte> destination, Int32 offset, out ReadOnlySpan<Byte> bytes)
        where TValue : unmanaged
    {
        ref TValue refValue = ref Unsafe.AsRef(value);
        ReadOnlySpan<TValue> intermediateSpan = MemoryMarshal.CreateReadOnlySpan(ref refValue, 1);
        bytes = MemoryMarshal.AsBytes(intermediateSpan);
        if (destination.Length - offset < bytes.Length)
            throw new InsufficientMemoryException($"Insufficient available size on {nameof(destination)} to copy {nameof(value)}.");
    }

    /// <summary>
    /// Throws an exception if the memory length is invalid.
    /// </summary>
    /// <param name="length">Memory length value.</param>
    /// <param name="nameofLength">Name of the memory length parameter.</param>
    /// <exception cref="ArgumentException"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ThrowIfInvalidMemoryLength(Int32 length, [CallerArgumentExpression(nameof(length))] String nameofLength = emptyString)
    {
        if (length < 0)
            throw new ArgumentException($"The parameter {nameofLength} must be zero or positive integer.");
    }

    /// <summary>
    /// Throws an exception if <paramref name="index"/> is invalid in an iteration of an enumerable of
    /// <paramref name="enumerationSize"/> elements.
    /// </summary>
    /// <param name="index">Iteration current index.</param>
    /// <param name="enumerationSize">Enumeration size.</param>
    /// <exception cref="InvalidOperationException"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ThrowIfInvalidIndexEnumerator(Int32 index, Int32 enumerationSize)
    {
        if (index < 0)
            throw new InvalidOperationException("Enumeration has not started. Call MoveNext.");
        if (index >= enumerationSize)
            throw new InvalidOperationException("Enumeration already finished.");
    }

    /// <summary>
    /// Throws an exception if <paramref name="region"/> is not a local UTF-8 string.
    /// </summary>
    /// <param name="region">A <see cref="ValueRegion{Byte}"/> instance.</param>
    /// <param name="nameofRegion">Name of region.</param>
    /// <param name="result">Output. Binary information in <paramref name="region"/>.</param>
    /// <exception cref="InvalidOperationException"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ThrowIfInvalidUtf8Region(ValueRegion<Byte> region, String nameofRegion, out Byte[] result)
    {
        result = (Byte[])region!;
        if (result is null)
            throw new InvalidOperationException($"{nameofRegion} does not contains the UTF-8 text.");
    }

    /// <summary>
    /// Throws an exception if the subregion parameters are invalid.
    /// </summary>
    /// <param name="regionLength">Length of the region.</param>
    /// <param name="startIndex">
    /// The zero-based starting item position of a subregion in the region.
    /// </param>
    /// <param name="length">The number of items in the subregion.</param>
    /// <param name="nameofStartIndex">Name of the start index parameter.</param>
    /// <param name="nameofLength">Name of the length parameter.</param>
    /// <exception cref="ArgumentOutOfRangeException"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ThrowIfInvalidSubregion(Int32 regionLength, Int32 startIndex, Int32 length,
        [CallerArgumentExpression(nameof(startIndex))] String nameofStartIndex = emptyString,
        [CallerArgumentExpression(nameof(length))] String nameofLength = emptyString)
    {
        if (startIndex < 0)
            throw new ArgumentOutOfRangeException(nameofStartIndex, "StartIndex cannot be less than zero.");

        if (startIndex > regionLength)
            throw new ArgumentOutOfRangeException(nameofStartIndex, $"{nameofStartIndex} cannot be larger than length of region.");

        if (length < 0)
            throw new ArgumentOutOfRangeException(nameofLength, "Length cannot be less than zero.");

        if (startIndex > regionLength - length)
            throw new ArgumentOutOfRangeException(nameofLength, "Index and length must refer to a location within the region.");
    }

    /// <summary>
    /// Throws an exception if the substring parameters are invalid.
    /// </summary>
    /// <param name="stringLength">Length of the string.</param>
    /// <param name="startIndex">
    /// The zero-based starting item position of a substring in the region.
    /// </param>
    /// <param name="length">The number of items in the substring.</param>
    /// <param name="nameofStartIndex">Name of the start index parameter.</param>
    /// <param name="nameofLength">Name of the length parameter.</param>
    /// <exception cref="ArgumentOutOfRangeException"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ThrowIfInvalidSubstring(Int32 stringLength, Int32 startIndex, Int32 length,
        [CallerArgumentExpression(nameof(startIndex))] String nameofStartIndex = emptyString,
        [CallerArgumentExpression(nameof(length))] String nameofLength = emptyString)
    {
        if (startIndex < 0)
            throw new ArgumentOutOfRangeException(nameofStartIndex, "StartIndex cannot be less than zero.");

        if (startIndex > stringLength)
            throw new ArgumentOutOfRangeException(nameofStartIndex, $"{nameofStartIndex} cannot be larger than length of string.");

        if (length < 0)
            throw new ArgumentOutOfRangeException(nameofLength, "Length cannot be less than zero.");

        if (startIndex > stringLength - length)
            throw new ArgumentOutOfRangeException(nameofLength, "Index and length must refer to a location within the string.");
    }

    /// <summary>
    /// Throws an exception if the subsequence parameters are invalid.
    /// </summary>
    /// <param name="sequenceLength">Length of the subsequence.</param>
    /// <param name="startIndex">
    /// The zero-based starting UTF-8 string position of a subsequence in this instance.
    /// </param>
    /// <param name="length">The number of UTF-8 strings in the subsequence.</param>
    /// <param name="nameofStartIndex">Name of the start index parameter.</param>
    /// <param name="nameofLength">Name of the length parameter.</param>
    /// <exception cref="ArgumentOutOfRangeException"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ThrowIfInvalidSubsequence(Int32 sequenceLength, Int32 startIndex, Int32 length,
        [CallerArgumentExpression(nameof(startIndex))] String nameofStartIndex = emptyString,
        [CallerArgumentExpression(nameof(length))] String nameofLength = emptyString)
    {
        if (startIndex < 0)
            throw new ArgumentOutOfRangeException(nameofStartIndex, "StartIndex cannot be less than zero.");

        if (startIndex > sequenceLength)
            throw new ArgumentOutOfRangeException(nameofStartIndex, $"{nameofStartIndex} cannot be larger than length of sequence.");

        if (length < 0)
            throw new ArgumentOutOfRangeException(nameofLength, "Length cannot be less than zero.");

        if (startIndex > sequenceLength - length)
            throw new ArgumentOutOfRangeException(nameofLength, "Index and length must refer to a location within the sequence.");
    }
}