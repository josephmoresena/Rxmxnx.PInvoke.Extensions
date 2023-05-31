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
    /// Throws an exception if <paramref name="index"/> is invalid for a list of <paramref name="count"/> elements.
    /// </summary>
    /// <param name="index">Requested index.</param>
    /// <param name="count">Total number of elements in the list.</param>
    /// <param name="nameofIndex">Name of the index parameter.</param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ThrowIfInvalidListIndex(Int32 index, Int32 count, [CallerArgumentExpression(nameof(index))] String nameofIndex = emptyString)
    {
        if (index < 0 || index >= count)
            throw new ArgumentOutOfRangeException(nameofIndex, "Index was outside the bounds of the list.");
    }

    /// <summary>
    /// Throws an exception if <paramref name="index"/> is invalid for a sequence of <paramref name="count"/> elements.
    /// </summary>
    /// <param name="index">Requested index.</param>
    /// <param name="count">Total number of elements in the sequence.</param>
    /// <param name="nameofIndex">Name of the index parameter.</param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ThrowIfInvalidSequenceIndex(Int32 index, Int32 count, [CallerArgumentExpression(nameof(index))] String nameofIndex = emptyString)
    {
        if (index < 0 || index >= count)
            throw new ArgumentOutOfRangeException(nameofIndex, "Index was outside the bounds of the sequence.");
    }

    /// <summary>
    /// Throws an exception if the fixed memory pointer instance is invalid.
    /// </summary>
    /// <param name="isValid">Indicates whether the fixed pointer instance is invalid.</param>
    /// <exception cref="InvalidOperationException"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ThrowIfInvalidPointer(IWrapper<Boolean> isValid)
    {
        if (!isValid.Value)
            throw new InvalidOperationException("The current instance is not valid.");
    }

    /// <summary>
    /// Throws an exception if the fixed memory pointer instance is not a function.
    /// </summary>
    /// <param name="isFunction">Indicates whether the fixed pointer instance is a function.</param>
    /// <exception cref="InvalidOperationException"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ThrowIfNotFunctionPointer(Boolean isFunction)
    {
        if (!isFunction)
            throw new InvalidOperationException("The current instance is not a function.");
    }

    /// <summary>
    /// Throws an exception if the fixed memory pointer instance is a function.
    /// </summary>
    /// <param name="isFunction">Indicates whether the fixed pointer instance is a function.</param>
    /// <exception cref="InvalidOperationException"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ThrowIfFunctionPointer(Boolean isFunction)
    {
        if (isFunction)
            throw new InvalidOperationException("The current instance is a function.");
    }

    /// <summary>
    /// Throws an exception if the fixed memory pointer instance is read-only and the operation is not read-only.
    /// </summary>
    /// <param name="isReadOnlyOperation">Indicates whether the operation is read-only.</param>
    /// <param name="isReadOnly">Indicates whether the fixed pointer instance is read-only.</param>
    /// <exception cref="InvalidOperationException"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ThrowIfReadOnlyPointer(Boolean isReadOnlyOperation, Boolean isReadOnly)
    {
        if (!isReadOnlyOperation && isReadOnly)
            throw new InvalidOperationException("The current instance is read-only.");
    }

    /// <summary>
    /// Throws an exception if the size of the fixed memory pointer instance is insufficent to contain a
    /// <typeparamref name="TValue"/> value.
    /// </summary>
    /// <typeparam name="TValue">Type of the referenced value.</typeparam>
    /// <param name="binaryLength">Size of the fixed memory pointer instance.</param>
    /// <exception cref="InsufficientMemoryException"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe void ThrowIfInvalidRefTypePointer<TValue>(Int32 binaryLength)
        where TValue : unmanaged
    {
        if (binaryLength < sizeof(TValue))
            throw new InsufficientMemoryException($"The current instance is insufficent to contain a value of {typeof(TValue)} type.");
    }

    /// <summary>
    /// Throws an exception if the size of the binary span is insufficent to contain a <typeparamref name="TValue"/> value.
    /// </summary>
    /// <typeparam name="TValue">Type of the referenced value.</typeparam>
    /// <param name="span">Binary span.</param>
    /// <param name="nameofSpan">Name of the span.</param>
    /// <exception cref="InsufficientMemoryException"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe void ThrowIfInvalidBinarySpanSize<TValue>(ReadOnlySpan<Byte> span, [CallerArgumentExpression(nameof(span))] String nameofSpan = emptyString)
        where TValue : unmanaged
    {
        Int32 typeSize = sizeof(TValue);
        if (span.Length != typeSize)
            throw new InsufficientMemoryException($"The length of parameter {nameofSpan} must be equals to {typeSize}.");
    }

    /// <summary>
    /// Throws an exception if <paramref name="obj"/> is not a <typeparamref name="T"/> instance.
    /// </summary>
    /// <typeparam name="T">Type of the casted object.</typeparam>
    /// <param name="obj">Managed object.</param>
    /// <param name="typeName">Type name.</param>
    /// <param name="result">Output. Casted value.</param>
    /// <param name="nameofObj">Name of the object.</param>
    /// <exception cref="ArgumentException"/>
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
            throw new ArgumentException($"Insufficient available size on {nameof(destination)} to copy {nameof(value)}.");
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