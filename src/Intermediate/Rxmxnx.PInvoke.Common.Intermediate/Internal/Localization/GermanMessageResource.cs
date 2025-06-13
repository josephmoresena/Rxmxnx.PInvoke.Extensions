namespace Rxmxnx.PInvoke.Internal.Localization;

/// <summary>
/// German (Deutsch) message resource.
/// </summary>
#if !PACKAGE
[ExcludeFromCodeCoverage]
#endif
internal sealed class GermanMessageResource : IMessageResource
{
	/// <summary>
	/// Current instance.
	/// </summary>
	public static readonly GermanMessageResource Instance = new();

	/// <summary>
	/// Private constructor.
	/// </summary>
	private GermanMessageResource() { }

	String IMessageResource.InvalidListIndexMessage => "Index lag außerhalb der Grenzen der Liste.";
	String IMessageResource.InvalidSequenceIndex => "Index lag außerhalb der Grenzen der Sequenz.";
	String IMessageResource.InvalidInstance => "Die aktuelle Instanz ist ungültig.";
	String IMessageResource.InvalidPointerSerialization
		=> "Ein IntPtr oder UIntPtr mit einem Wert von acht Bytes kann auf einer Maschine mit einer Wortgröße von vier Bytes nicht deserialisiert werden.";
	String IMessageResource.IsNotFunction => "Die aktuelle Instanz ist keine Funktion.";
	String IMessageResource.IsFunction => "Die aktuelle Instanz ist eine Funktion.";
	String IMessageResource.ReadOnlyInstance => "Die aktuelle Instanz ist schreibgeschützt.";
	String IMessageResource.InvalidUnmanagedCast => "Die Größen von Quell- und Zieltypen müssen gleich sein.";
	String IMessageResource.NotStartedEnumerable => "Die Enumeration wurde nicht gestartet. Rufen Sie MoveNext auf.";
	String IMessageResource.FinishedEnumerable => "Die Enumeration ist bereits abgeschlossen.";
	String IMessageResource.LessThanZero => "Darf nicht kleiner als null sein.";
	String IMessageResource.LargerThanRegionLength => "Darf nicht größer als die Länge der Region sein.";
	String IMessageResource.IndexOutOfRegion
		=> "Index und Länge müssen sich auf eine Position innerhalb der Region beziehen.";
	String IMessageResource.LargerThanStringLength => "Darf nicht größer als die Länge der Zeichenfolge sein.";
	String IMessageResource.IndexOutOfString
		=> "Index und Länge müssen sich auf eine Position innerhalb der Zeichenfolge beziehen.";
	String IMessageResource.LargerThanSequenceLength => "Darf nicht größer als die Länge der Sequenz sein.";
	String IMessageResource.IndexOutOfSequence
		=> "Index und Länge müssen sich auf eine Position innerhalb der Sequenz beziehen.";
	String IMessageResource.MissingMemoryInspector
		=> "Die Speicherinspektion wird auf der aktuellen Plattform nicht unterstützt.";
	String IMessageResource.ReflectionDisabled => "Diese Funktion erfordert den vollständigen Reflexionsmodus.";

	String IMessageResource.InvalidType(String requiredTypeName) => $"Das Objekt muss vom Typ {requiredTypeName} sein.";
	String IMessageResource.InvalidRefTypePointer(Type typeOf)
		=> $"Die aktuelle Instanz reicht nicht aus, um einen Wert des Typs {typeOf} zu enthalten.";
	String IMessageResource.InvalidBinarySpanSize(String nameofSpan, Int32 sizeOf)
		=> $"Die Länge des Parameters {nameofSpan} muss gleich {sizeOf} sein.";
	String IMessageResource.InvalidCopyUnmanagedType(String nameofSpan, String nameofValue)
		=> $"Nicht genügend Platz verfügbar auf {nameofSpan}, um {nameofValue} zu kopieren.";
	String IMessageResource.InvalidLength(String nameofLength)
		=> $"Der Parameter {nameofLength} muss null oder eine positive ganze Zahl sein.";
	String IMessageResource.InvalidUtf8Region(String nameofRegion) => $"{nameofRegion} enthält keinen UTF-8-Text.";
	String IMessageResource.NotUnmanagedType(Type type) => $"{type} ist kein nicht verwalteter Typ.";
	String IMessageResource.NotValueType(Type type) => $"{type} ist kein Werttyp.";
	String IMessageResource.NotReferenceType(Type type) => $"{type} ist kein Referenztyp.";
	String IMessageResource.NotType(Type sourceType, Type destinationType)
		=> $"{sourceType} ist nicht {destinationType}.";
	String IMessageResource.ContainsReferencesButUnmanaged(Type itemType, Type arrayType)
		=> $"{itemType} enthält Verweise, aber {arrayType} ist ein nicht verwalteter Typ.";
	String IMessageResource.ReferencesTypeButUnmanaged(Type itemType, Type arrayType)
		=> $"{itemType} ist ein Referenztyp, aber {arrayType} ist ein nicht verwalteter Typ.";
	String IMessageResource.UnmanagedTypeButContainsReferences(Type itemType, Type arrayType)
		=> $"{itemType} ist ein nicht verwalteter Typ, aber {arrayType} enthält Verweise.";
#if !PACKAGE || !NET6_0_OR_GREATER
	String IMessageResource.MissingBufferMetadataException(Type bufferType)
		=> $"Metadaten für den Puffer {bufferType} konnten nicht abgerufen werden.";
#endif
	String IMessageResource.MissingBufferMetadataException(Type itemType, UInt16 size)
		=> $"Es ist nicht möglich, einen Puffer für {itemType} mit {size} Elementen zu erstellen.";
#if !PACKAGE || NETCOREAPP
	String IMessageResource.InvalidToken(String currentToken, String expectedToken)
		=> $"Unerwarteter Token-Typ: {currentToken}. Erwarteter Token-Typ: {expectedToken}.";
#endif
}