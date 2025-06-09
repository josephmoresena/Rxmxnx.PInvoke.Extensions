#if !NET5_0_OR_GREATER
using Enum = Rxmxnx.PInvoke.Internal.FrameworkCompat.EnumCompat;
#endif

namespace Rxmxnx.PInvoke.Internal;

/// <summary>
/// Utility class for argument validation.
/// </summary>
#if !PACKAGE
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
#endif
internal static unsafe class ValidationUtilities
{
	/// <summary>
	/// Empty <see cref="String"/> for <see cref="CallerArgumentExpressionAttribute"/> default value.
	/// </summary>
	private const String EmptyString = "";

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
		[CallerArgumentExpression(nameof(index))] String nameofIndex = ValidationUtilities.EmptyString)
	{
		if (index >= 0 && index < count) return;
		String message = IMessageResource.GetInstance().InvalidListIndexMessage;
		throw new ArgumentOutOfRangeException(nameofIndex, message);
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
		[CallerArgumentExpression(nameof(index))] String nameofIndex = ValidationUtilities.EmptyString)
	{
		if (index >= 0 && index < count) return;
		String message = IMessageResource.GetInstance().InvalidSequenceIndex;
		throw new ArgumentOutOfRangeException(nameofIndex, message);
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
		if (isValid.Value) return;
		String message = IMessageResource.GetInstance().InvalidInstance;
		throw new InvalidOperationException(message);
	}

	/// <summary>
	/// Throws an exception if <paramref name="info"/> contains an invalid <see langword="unmanaged"/> pointer.
	/// </summary>
	/// <param name="info">A <see cref="SerializationInfo"/> instance.</param>
	/// <returns>Deserialized <see langword="unmanaged"/> pointer.</returns>
	/// <exception cref="ArgumentException">
	/// Throws an exception if <paramref name="info"/> contains an invalid <see langword="unmanaged"/> pointer.
	/// </exception>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static void* ThrowIfInvalidPointer(SerializationInfo info)
	{
		Int64 l = info.GetInt64("value");
		if (IntPtr.Size != 4 || l is <= Int32.MaxValue and >= Int32.MinValue) return (void*)l;
		String message = IMessageResource.GetInstance().InvalidPointerSerialization;
		throw new ArgumentException(message);
	}
	/// <summary>
	/// Throws an exception if <paramref name="info"/> is <see langword="null"/>.
	/// </summary>
	/// <param name="info">A <see cref="SerializationInfo"/> instance.</param>
	/// <param name="ptr">An <see langword="unmanaged"/> pointer.</param>
	/// <exception cref="ArgumentNullException">
	/// Throws an exception if <paramref name="info"/> is <see langword="null"/>.
	/// </exception>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
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
	/// <param name="ptr">An <see cref="IntPtr"/> value.</param>
	/// <param name="nameofPtr">Name of value pointer type.</param>
	/// <typeparam name="T">Type of referenced value.</typeparam>
	/// <returns>A <see cref="Int32"/> value that indicates the relative order of the objects being compared.</returns>
	/// <exception cref="ArgumentException">Throws an exception if <paramref name="obj"/> is not a value pointer.</exception>
	public static Int32 ThrowIfInvalidValuePointer<T>(Object? obj, IntPtr ptr, String nameofPtr)
#if NET9_0_OR_GREATER
		where T : allows ref struct
#endif
		=> obj switch
		{
			null => 1,
			ValPtr<T> v => ((Int64)ptr).CompareTo((Int64)v.Pointer),
			ReadOnlyValPtr<T> r => ((Int64)ptr).CompareTo((Int64)r.Pointer),
			_ => throw new ArgumentException(IMessageResource.GetInstance().InvalidType(nameofPtr)),
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
		if (isFunction) return;
		String message = IMessageResource.GetInstance().IsNotFunction;
		throw new InvalidOperationException(message);
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
		if (!isFunction) return;
		String message = IMessageResource.GetInstance().IsFunction;
		throw new InvalidOperationException(message);
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
		if (isReadOnlyOperation || !isReadOnly) return;
		String message = IMessageResource.GetInstance().ReadOnlyInstance;
		throw new InvalidOperationException(message);
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
		if (binaryLength >= sizeOf) return;
		String message = IMessageResource.GetInstance().InvalidRefTypePointer(typeOf);
		throw new InsufficientMemoryException(message);
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
		[CallerArgumentExpression(nameof(span))] String nameofSpan = ValidationUtilities.EmptyString)
	{
		String message = IMessageResource.GetInstance().InvalidBinarySpanSize(nameofSpan, sizeOf);
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
		[CallerArgumentExpression(nameof(obj))] String nameofObj = ValidationUtilities.EmptyString)
	{
		if (obj is not T value)
			throw new ArgumentException(IMessageResource.GetInstance().InvalidType(typeName), nameofObj);
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
		if (destinationSize == sourceSize) return;
		String message = IMessageResource.GetInstance().InvalidUnmanagedCast;
		throw new InvalidOperationException(message);
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
		if (destination.Length - offset >= bytes.Length) return;
		String message = IMessageResource.GetInstance().InvalidCopyUnmanagedType(nameof(destination), nameof(value));
		throw new InsufficientMemoryException(message);
	}

	/// <summary>
	/// Validates the memory length.
	/// </summary>
	/// <param name="length">Memory length value.</param>
	/// <param name="nameofLength">Name of the memory length parameter.</param>
	/// <exception cref="ArgumentException">Thrown if <paramref name="length"/> is less than zero.</exception>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void ThrowIfInvalidMemoryLength(Int32 length,
		[CallerArgumentExpression(nameof(length))] String nameofLength = ValidationUtilities.EmptyString)
	{
		if (length >= 0) return;
		String message = IMessageResource.GetInstance().InvalidLength(nameofLength);
		throw new ArgumentException(message);
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
		IMessageResource resource = IMessageResource.GetInstance();
		if (index < 0)
			throw new InvalidOperationException(resource.NotStartedEnumerable);
		if (index >= enumerationSize)
			throw new InvalidOperationException(resource.FinishedEnumerable);
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
		{
			String message = IMessageResource.GetInstance().InvalidUtf8Region(nameofRegion);
			throw new InvalidOperationException(message);
		}
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
		[CallerArgumentExpression(nameof(startIndex))] String nameofStartIndex = ValidationUtilities.EmptyString,
		[CallerArgumentExpression(nameof(length))] String nameofLength = ValidationUtilities.EmptyString)
	{
		IMessageResource resource = IMessageResource.GetInstance();
		if (startIndex < 0)
			throw new ArgumentOutOfRangeException(nameofStartIndex, resource.LessThanZero);

		if (startIndex > regionLength)
			throw new ArgumentOutOfRangeException(nameofStartIndex, resource.LargerThanRegionLength);

		if (length < 0)
			throw new ArgumentOutOfRangeException(nameofLength, resource.LessThanZero);

		if (startIndex > regionLength - length)
			throw new ArgumentOutOfRangeException(nameofLength, resource.IndexOutOfRegion);
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
		[CallerArgumentExpression(nameof(startIndex))] String nameofStartIndex = ValidationUtilities.EmptyString,
		[CallerArgumentExpression(nameof(length))] String nameofLength = ValidationUtilities.EmptyString)
	{
		IMessageResource resource = IMessageResource.GetInstance();
		if (startIndex < 0)
			throw new ArgumentOutOfRangeException(nameofStartIndex, resource.LessThanZero);

		if (startIndex > stringLength)
			throw new ArgumentOutOfRangeException(nameofStartIndex, resource.LargerThanStringLength);

		if (length < 0)
			throw new ArgumentOutOfRangeException(nameofLength, resource.LessThanZero);

		if (startIndex > stringLength - length)
			throw new ArgumentOutOfRangeException(nameofLength, resource.IndexOutOfString);
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
		[CallerArgumentExpression(nameof(startIndex))] String nameofStartIndex = ValidationUtilities.EmptyString,
		[CallerArgumentExpression(nameof(length))] String nameofLength = ValidationUtilities.EmptyString)
	{
		IMessageResource resource = IMessageResource.GetInstance();
		if (startIndex < 0)
			throw new ArgumentOutOfRangeException(nameofStartIndex, resource.LessThanZero);

		if (startIndex > sequenceLength)
			throw new ArgumentOutOfRangeException(nameofStartIndex, resource.LargerThanSequenceLength);

		if (length < 0)
			throw new ArgumentOutOfRangeException(nameofLength, resource.LessThanZero);

		if (startIndex > sequenceLength - length)
			throw new ArgumentOutOfRangeException(nameofLength, resource.IndexOutOfSequence);
	}
	/// <summary>
	/// Throws an exception if <paramref name="type"/> is not unmanaged.
	/// </summary>
	/// <param name="type">CLR type.</param>
	/// <param name="isUnmanaged">Indicates whether <paramref name="type"/> is unmanaged.</param>
	/// <exception cref="InvalidOperationException">
	/// Throws an exception if <paramref name="type"/> is not a unmanaged type.
	/// </exception>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void ThrowIfNotUnmanagedType(Type? type, Boolean isUnmanaged)
	{
		if (type is null || (type.IsValueType && isUnmanaged)) return;
		String message = IMessageResource.GetInstance().NotUnmanagedType(type);
		throw new InvalidOperationException(message);
	}
	/// <summary>
	/// Throws an exception if <paramref name="type"/> is not reference type.
	/// </summary>
	/// <param name="type">CLR type.</param>
	/// <exception cref="InvalidOperationException">
	/// Throws an exception if <paramref name="type"/> is not a reference type.
	/// </exception>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void ThrowIfNotReferenceType(Type? type)
	{
		if (type is not null && !type.IsValueType) return;
		String message = IMessageResource.GetInstance().NotReferenceType(type ?? typeof(Byte));
		throw new InvalidOperationException(message);
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
		IMessageResource resource = IMessageResource.GetInstance();
		if (!destinationType.IsValueType)
		{
			if (sourceType is null || sourceType.IsValueType)
				throw new InvalidOperationException(resource.NotValueType(destinationType));
		}
		else if (!unmanagedSource)
		{
			sourceType ??= typeof(Byte);
			if (unmanagedDestination)
				throw new InvalidOperationException(resource.NotUnmanagedType(sourceType));
			if (destinationType != sourceType)
				throw new InvalidOperationException(resource.NotType(sourceType, destinationType));
		}
		else if (!unmanagedDestination)
		{
			throw new InvalidOperationException(resource.NotUnmanagedType(destinationType));
		}
	}
	/// <summary>
	/// Throws an exception if <paramref name="arrayType"/> can't hold <paramref name="itemType"/> instances.
	/// </summary>
	/// <param name="itemType">CLR item type.</param>
	/// <param name="isItemUnmanaged">Indicates whether <paramref name="itemType"/> is unmanaged.</param>
	/// <param name="arrayType">CLR array type.</param>
	/// <param name="isArrayUnmanaged">Indicates whether <paramref name="arrayType"/> is unmanaged.</param>
	/// <exception cref="InvalidOperationException">
	/// Throws an exception if <paramref name="arrayType"/> can't hold <paramref name="itemType"/> instances.
	/// </exception>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void ThrowIfInvalidBuffer(Type itemType, Boolean isItemUnmanaged, Type arrayType,
		Boolean isArrayUnmanaged)
	{
		IMessageResource resource = IMessageResource.GetInstance();
		String? message = isItemUnmanaged switch
		{
			false when isArrayUnmanaged => itemType.IsValueType ?
				resource.ContainsReferencesButUnmanaged(itemType, arrayType) :
				resource.ReferencesTypeButUnmanaged(itemType, arrayType),
			true when !isArrayUnmanaged => resource.UnmanagedTypeButContainsReferences(itemType, arrayType),
			_ => default,
		};
		if (!String.IsNullOrEmpty(message))
			throw new InvalidOperationException(message);
	}
#if !PACKAGE || !NET6_0_OR_GREATER
	/// <summary>
	/// Throws an exception if buffer metadata is null.
	/// </summary>
	/// <param name="bufferType">Buffer type.</param>
	/// <param name="isNull">Indicates whether buffer metadata is null.</param>
	/// <exception cref="InvalidOperationException">Throws an exception if buffer metadata is null.</exception>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void ThrowIfNullMetadata(Type bufferType, Boolean isNull)
	{
		if (!isNull) return;
		IMessageResource resource = IMessageResource.GetInstance();
		throw new InvalidOperationException(resource.MissingBufferMetadataException(bufferType));
	}
#endif
	/// <summary>
	/// Throws an exception if buffer metadata is null.
	/// </summary>
	/// <param name="itemType">CLR item type.</param>
	/// <param name="isNull">Indicates whether buffer metadata is null.</param>
	/// <param name="size">Buffer size.</param>
	/// <exception cref="InvalidOperationException">Throws an exception if buffer metadata is null.</exception>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void ThrowIfNullMetadata(Type itemType, UInt16 size, Boolean isNull)
	{
		if (!isNull) return;
		IMessageResource resource = IMessageResource.GetInstance();
		throw new InvalidOperationException(resource.MissingBufferMetadataException(itemType, size));
	}
	/// <summary>
	/// Throws an exception if <see cref="MemoryInspector"/> is not supported on the current platform.
	/// </summary>
	/// <param name="instance">Current platform <see cref="MemoryInspector"/> instance.</param>
	/// <returns></returns>
	/// <exception cref="PlatformNotSupportedException">
	/// Throws an exception if <see cref="MemoryInspector"/> is not supported on the current platform.
	/// </exception>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static MemoryInspector ThrowIfNotSupportedPlatform(MemoryInspector? instance)
	{
		if (instance is not null) return instance;
		IMessageResource resource = IMessageResource.GetInstance();
		throw new PlatformNotSupportedException(resource.MissingMemoryInspector);
	}
	/// <summary>
	/// Throws an exception if the current token type is invalid for string type.
	/// </summary>
	/// <param name="tokenType">A <see cref="JsonTokenType"/> value.</param>
	/// <exception cref="JsonException">Throws an exception if the current token type is invalid for string type.</exception>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void ThrowIfNotString(JsonTokenType tokenType)
	{
		if (tokenType is JsonTokenType.String or JsonTokenType.Null) return;
		String expectedTokenTypeName = Enum.GetName(JsonTokenType.String) ??
			nameof(JsonTokenType) + '.' + nameof(JsonTokenType.String);
		ValidationUtilities.ThrowIfInvalidToken(tokenType, expectedTokenTypeName);
	}
	/// <summary>
	/// Throws an exception if the current token type is invalid for array type.
	/// </summary>
	/// <param name="tokenType">A <see cref="JsonTokenType"/> value.</param>
	/// <exception cref="JsonException">Throws an exception if the current token type is invalid for array type.</exception>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void ThrowIfNotArray(JsonTokenType tokenType)
	{
		if (tokenType is JsonTokenType.StartArray or JsonTokenType.Null) return;
		String expectedTokenTypeName = Enum.GetName(JsonTokenType.StartArray) ??
			nameof(JsonTokenType) + '.' + nameof(JsonTokenType.StartArray);
		ValidationUtilities.ThrowIfInvalidToken(tokenType, expectedTokenTypeName);
	}
	/// <summary>
	/// Throw if reflection free-mode.
	/// </summary>
	/// <exception cref="NotImplementedException"></exception>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void ThrowIfNoReflection()
	{
		if (!AotInfo.IsReflectionDisabled) return;
		IMessageResource resource = IMessageResource.GetInstance();
		throw new PlatformNotSupportedException(resource.ReflectionDisabled);
	}
#if BINARY_SPACES
	/// <summary>
	/// Throws an exception if buffer is not a space.
	/// </summary>
	/// <param name="isBinary">Indicates whether buffer is binary.</param>
	/// <param name="bufferSize">Buffer sizes.</param>
	/// <param name="type">CLR type of buffer.</param>
	/// <exception cref="InvalidOperationException">Throws an exception if buffer is not a space.</exception>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void ThrowIfNotSpace(Boolean isBinary, Span<UInt16> bufferSize, Type type)
	{
		if (!isBinary || bufferSize[0] != 2 * bufferSize[1] || bufferSize[1] != bufferSize[2])
			throw new InvalidOperationException(
				$"{type} is not an space. Size: {bufferSize[0]} ({bufferSize[2]}, {bufferSize[1]}).");
	}
#endif

	/// <summary>
	/// Throws an exception if the current token type is invalid for expected type.
	/// </summary>
	/// <param name="tokenType">A <see cref="JsonTokenType"/> value.</param>
	/// <param name="expectedToken">Expected token type name.</param>
	/// <exception cref="JsonException">Throws an exception if the current token type is invalid for expeted type.</exception>
	private static void ThrowIfInvalidToken(JsonTokenType tokenType, String expectedToken)
	{
		IMessageResource resource = IMessageResource.GetInstance();
		String tokenTypeName = Enum.GetName(tokenType) ?? $"{tokenType}";
		String message = resource.InvalidToken(tokenTypeName, expectedToken);
		throw new JsonException(message);
	}
}