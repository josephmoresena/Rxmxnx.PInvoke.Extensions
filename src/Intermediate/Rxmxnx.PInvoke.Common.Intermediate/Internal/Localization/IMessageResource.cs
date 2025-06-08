namespace Rxmxnx.PInvoke.Internal.Localization;

/// <summary>
/// This interface exposes a message resource type.
/// </summary>
internal partial interface IMessageResource
{
	/// <summary>
	/// Message for invalid list index exception.
	/// </summary>
	String InvalidListIndexMessage { get; }
	/// <summary>
	/// Message for invalid sequence index exception.
	/// </summary>
	String InvalidSequenceIndex { get; }
	/// <summary>
	/// Message for invalid instance exception.
	/// </summary>
	String InvalidInstance { get; }
	/// <summary>
	/// Message for invalid pointer serialization exception.
	/// </summary>
	String InvalidPointerSerialization { get; }
	/// <summary>
	/// Message for not function exception.
	/// </summary>
	String IsNotFunction { get; }
	/// <summary>
	/// Message for function exception.
	/// </summary>
	String IsFunction { get; }
	/// <summary>
	/// Message for read-only instance exception.
	/// </summary>
	String ReadOnlyInstance { get; }
	/// <summary>
	/// Message for invalid unmanaged cast exception.
	/// </summary>
	String InvalidUnmanagedCast { get; }
	/// <summary>
	/// Message for not started enumerable exception.
	/// </summary>
	String NotStartedEnumerable { get; }
	/// <summary>
	/// Message for finished enumerable exception.
	/// </summary>
	String FinishedEnumerable { get; }
	/// <summary>
	/// Message for less than zero exception.
	/// </summary>
	String LessThanZero { get; }
	/// <summary>
	/// Message for larger than region length exception.
	/// </summary>
	String LargerThanRegionLength { get; }
	/// <summary>
	/// Message for index out of region exception.
	/// </summary>
	String IndexOutOfRegion { get; }
	/// <summary>
	/// Message for larger than string length exception.
	/// </summary>
	String LargerThanStringLength { get; }
	/// <summary>
	/// Message for index out of string exception.
	/// </summary>
	String IndexOutOfString { get; }
	/// <summary>
	/// Message for larger than sequence length exception.
	/// </summary>
	String LargerThanSequenceLength { get; }
	/// <summary>
	/// Message for index out of sequence exception.
	/// </summary>
	String IndexOutOfSequence { get; }
	/// <summary>
	/// Message for missing memory inspector.
	/// </summary>
	String MissingMemoryInspector { get; }
	/// <summary>
	/// Message for reflection disabled.
	/// </summary>
	String ReflectionDisabled { get; }

	/// <summary>
	/// Message for invalid pointer value exception.
	/// </summary>
	String InvalidType(String requiredTypeName);
	/// <summary>
	/// Message for invalid ref type pointer exception.
	/// </summary>
	String InvalidRefTypePointer(Type typeOf);
	/// <summary>
	/// Message for invalid binary span size exception.
	/// </summary>
	String InvalidBinarySpanSize(String nameofSpan, Int32 sizeOf);
	/// <summary>
	/// Message for invalid copy unmanaged type exception.
	/// </summary>
	String InvalidCopyUnmanagedType(String nameofSpan, String nameofValue);
	/// <summary>
	/// Message for invalid length exception.
	/// </summary>
	String InvalidLength(String nameofLength);
	/// <summary>
	/// Message for invalid UTF-8 region exception.
	/// </summary>
	String InvalidUtf8Region(String nameofRegion);
	/// <summary>
	/// Message for not unmanaged type exception.
	/// </summary>
	String NotUnmanagedType(Type type);
	/// <summary>
	/// Message for not value type exception.
	/// </summary>
	String NotValueType(Type type);
	/// <summary>
	/// Message for not reference type exception.
	/// </summary>
	String NotReferenceType(Type type);
	/// <summary>
	/// Message for not type equality exception.
	/// </summary>
	String NotType(Type sourceType, Type destinationType);
	/// <summary>
	/// Message for containing item references but unmanaged array exception.
	/// </summary>
	String ContainsReferencesButUnmanaged(Type itemType, Type arrayType);
	/// <summary>
	/// Message for reference type item but unmanaged array exception.
	/// </summary>
	String ReferencesTypeButUnmanaged(Type itemType, Type arrayType);
	/// <summary>
	/// Message for unmanaged type item but containing array references exception.
	/// </summary>
	String UnmanagedTypeButContainsReferences(Type itemType, Type arrayType);
	/// <summary>
	/// Message for missing buffer metadata exception.
	/// </summary>
	String MissingBufferMetadataException(Type itemType, UInt16 size);
	/// <summary>
	/// Message for invalid string token exception.
	/// </summary>
	String InvalidToken(String currentToken, String expectedToken);
}