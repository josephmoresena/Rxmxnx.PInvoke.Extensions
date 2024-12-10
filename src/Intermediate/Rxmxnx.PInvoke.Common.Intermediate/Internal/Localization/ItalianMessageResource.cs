namespace Rxmxnx.PInvoke.Internal.Localization;

/// <summary>
/// Italian (Italiano) message resource.
/// </summary>
[ExcludeFromCodeCoverage]
internal sealed class ItalianMessageResource : IMessageResource
{
	/// <inheritdoc cref="IMessageResource.Instance"/>
	private static readonly ItalianMessageResource instance = new();

#if NET6_0
	[RequiresPreviewFeatures]
#endif
	static IMessageResource IMessageResource.Instance => ItalianMessageResource.instance;

	/// <summary>
	/// Private constructor.
	/// </summary>
	private ItalianMessageResource() { }

	String IMessageResource.InvalidListIndexMessage => "L'indice era fuori dai limiti della lista.";
	String IMessageResource.InvalidSequenceIndex => "L'indice era fuori dai limiti della sequenza.";
	String IMessageResource.InvalidInstance => "L'istanza corrente non è valida.";
	String IMessageResource.InvalidPointerSerialization
		=> "Un IntPtr o UIntPtr con un valore di otto byte non può essere deserializzato su una macchina con una dimensione di parola di quattro byte.";
	String IMessageResource.IsNotFunction => "L'istanza corrente non è una funzione.";
	String IMessageResource.IsFunction => "L'istanza corrente è una funzione.";
	String IMessageResource.ReadOnlyInstance => "L'istanza corrente è di sola lettura.";
	String IMessageResource.InvalidUnmanagedCast
		=> "Le dimensioni dei tipi non gestiti di origine e destinazione devono essere uguali.";
	String IMessageResource.NotStartedEnumerable => "La enumerazione non è iniziata. Chiama MoveNext.";
	String IMessageResource.FinishedEnumerable => "La enumerazione è già terminata.";
	String IMessageResource.LessThanZero => "Non può essere inferiore a zero.";
	String IMessageResource.LargerThanRegionLength => "Non può essere maggiore della lunghezza della regione.";
	String IMessageResource.IndexOutOfRegion
		=> "Indice e lunghezza devono fare riferimento a una posizione all'interno della regione.";
	String IMessageResource.LargerThanStringLength => "Non può essere maggiore della lunghezza della stringa.";
	String IMessageResource.IndexOutOfString
		=> "Indice e lunghezza devono fare riferimento a una posizione all'interno della stringa.";
	String IMessageResource.LargerThanSequenceLength => "Non può essere maggiore della lunghezza della sequenza.";
	String IMessageResource.IndexOutOfSequence
		=> "Indice e lunghezza devono fare riferimento a una posizione all'interno della sequenza.";

	String IMessageResource.InvalidType(String requiredTypeName)
		=> $"L'oggetto deve essere di tipo {requiredTypeName}.";
	String IMessageResource.InvalidRefTypePointer(Type typeOf)
		=> $"L'istanza corrente non è sufficiente per contenere un valore del tipo {typeOf}.";
	String IMessageResource.InvalidBinarySpanSize(String nameofSpan, Int32 sizeOf)
		=> $"La lunghezza del parametro {nameofSpan} deve essere uguale a {sizeOf}.";
	String IMessageResource.InvalidCopyUnmanagedType(String nameofSpan, String nameofValue)
		=> $"Spazio insufficiente disponibile in {nameofSpan} per copiare {nameofValue}.";
	String IMessageResource.InvalidLength(String nameofLength)
		=> $"Il parametro {nameofLength} deve essere zero o un numero intero positivo.";
	String IMessageResource.InvalidUtf8Region(String nameofRegion) => $"{nameofRegion} non contiene testo UTF-8.";
	String IMessageResource.NotUnmanagedType(Type type) => $"{type} non è un tipo non gestito.";
	String IMessageResource.NotValueType(Type type) => $"{type} non è un tipo valore.";
	String IMessageResource.NotType(Type sourceType, Type destinationType) => $"{sourceType} non è {destinationType}.";
	String IMessageResource.ContainsReferencesButUnmanaged(Type itemType, Type arrayType)
		=> $"{itemType} contiene riferimenti ma {arrayType} è un tipo non gestito.";
	String IMessageResource.ReferencesTypeButUnmanaged(Type itemType, Type arrayType)
		=> $"{itemType} è un tipo di riferimento ma {arrayType} è un tipo non gestito.";
	String IMessageResource.UnmanagedTypeButContainsReferences(Type itemType, Type arrayType)
		=> $"{itemType} è un tipo non gestito ma {arrayType} contiene riferimenti.";
}