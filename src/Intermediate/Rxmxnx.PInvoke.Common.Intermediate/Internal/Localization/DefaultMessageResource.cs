namespace Rxmxnx.PInvoke.Internal.Localization;

/// <summary>
/// Default (English) message resource.
/// </summary>
#if !PACKAGE
[ExcludeFromCodeCoverage]
#endif
internal sealed class DefaultMessageResource : IMessageResource
{
	/// <summary>
	/// Current instance.
	/// </summary>
	public static readonly DefaultMessageResource Instance = new();

	/// <summary>
	/// Private constructor.
	/// </summary>
	private DefaultMessageResource() { }

	String IMessageResource.InvalidListIndexMessage => "Index was outside the bounds of the list.";
	String IMessageResource.InvalidSequenceIndex => "Index was outside the bounds of the sequence.";
	String IMessageResource.InvalidInstance => "The current instance is not valid.";
	String IMessageResource.InvalidPointerSerialization
		=> "An IntPtr or UIntPtr with an eight byte value cannot be deserialized on a machine with a four byte word size.";
	String IMessageResource.IsNotFunction => "The current instance is not a function.";
	String IMessageResource.IsFunction => "The current instance is a function.";
	String IMessageResource.ReadOnlyInstance => "The current instance is read-only.";
	String IMessageResource.InvalidUnmanagedCast
		=> "The sizes of both source and destination unmanaged types must be equal.";
	String IMessageResource.NotStartedEnumerable => "Enumeration has not started. Call MoveNext.";
	String IMessageResource.FinishedEnumerable => "Enumeration already finished.";
	String IMessageResource.LessThanZero => "Cannot be less than zero.";
	String IMessageResource.LargerThanRegionLength => "Cannot be larger than length of region.";
	String IMessageResource.IndexOutOfRegion => "Index and length must refer to a location within the region.";
	String IMessageResource.LargerThanStringLength => "Cannot be larger than length of string.";
	String IMessageResource.IndexOutOfString => "Index and length must refer to a location within the string.";
	String IMessageResource.LargerThanSequenceLength => "Cannot be larger than length of sequence.";
	String IMessageResource.IndexOutOfSequence => "Index and length must refer to a location within the sequence.";
	String IMessageResource.MissingMemoryInspector => "Memory inspection is not supported on the current platform.";
	String IMessageResource.ReflectionDisabled => "This feature requires reflection-full mode";

	String IMessageResource.InvalidType(String requiredTypeName) => $"Object must be of type {requiredTypeName}.";
	String IMessageResource.InvalidRefTypePointer(Type typeOf)
		=> $"The current instance is insufficient to contain a value of {typeOf} type.";
	String IMessageResource.InvalidBinarySpanSize(String nameofSpan, Int32 sizeOf)
		=> $"The length of parameter {nameofSpan} must be equals to {sizeOf}.";
	String IMessageResource.InvalidCopyUnmanagedType(String nameofSpan, String nameofValue)
		=> $"Insufficient available size on {nameofSpan} to copy {nameofValue}.";
	String IMessageResource.InvalidLength(String nameofLength)
		=> $"The parameter {nameofLength} must be zero or positive integer.";
	String IMessageResource.InvalidUtf8Region(String nameofRegion)
		=> $"{nameofRegion} does not contains the UTF-8 text.";
	String IMessageResource.NotUnmanagedType(Type type) => $"{type} is not an unmanaged type.";
	String IMessageResource.NotValueType(Type type) => $"{type} is not a value type.";
	String IMessageResource.NotReferenceType(Type type) => $"{type} is not a reference type.";
	String IMessageResource.NotType(Type sourceType, Type destinationType) => $"{sourceType} is not {destinationType}.";
	String IMessageResource.ContainsReferencesButUnmanaged(Type itemType, Type arrayType)
		=> $"{itemType} contains references but {arrayType} is unmanaged type.";
	String IMessageResource.ReferencesTypeButUnmanaged(Type itemType, Type arrayType)
		=> $"{itemType} is a reference type but {arrayType} is unmanaged type.";
	String IMessageResource.UnmanagedTypeButContainsReferences(Type itemType, Type arrayType)
		=> $"{itemType} is an unmanaged type but {arrayType} contains references.";
#if !PACKAGE || !NET6_0_OR_GREATER
	String IMessageResource.MissingBufferMetadataException(Type bufferType)
		=> $"Unable to retrieve metadata for {bufferType} buffer.";
#endif
	String IMessageResource.MissingBufferMetadataException(Type itemType, UInt16 size)
		=> $"Unable to create buffer for {itemType} {size} items.";
#if !PACKAGE || NETCOREAPP
	String IMessageResource.InvalidToken(String currentToken, String expectedToken)
		=> $"Unexpected token type: {currentToken}. Expected token type: {expectedToken}.";
#endif
}