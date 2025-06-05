namespace Rxmxnx.PInvoke.Internal.Localization;

/// <summary>
/// Spanish message resource.
/// </summary>
#if !PACKAGE
[ExcludeFromCodeCoverage]
#endif
internal sealed class SpanishMessageResource : IMessageResource
{
	/// <summary>
	/// Current instance.
	/// </summary>
	public static readonly SpanishMessageResource Instance = new();

	/// <summary>
	/// Private constructor.
	/// </summary>
	private SpanishMessageResource() { }

	String IMessageResource.InvalidListIndexMessage => "El índice estaba fuera de los límites de la lista.";
	String IMessageResource.InvalidSequenceIndex => "El índice estaba fuera de los límites de la secuencia.";
	String IMessageResource.InvalidInstance => "La instancia actual no es válida.";
	String IMessageResource.InvalidPointerSerialization
		=> "Un IntPtr o UIntPtr con un valor de ocho bytes no se puede deserializar en una máquina con un tamaño de palabra de cuatro bytes.";
	String IMessageResource.IsNotFunction => "La instancia actual no es una función.";
	String IMessageResource.IsFunction => "La instancia actual es una función.";
	String IMessageResource.ReadOnlyInstance => "La instancia actual es de sólo lectura.";
	String IMessageResource.InvalidUnmanagedCast
		=> "Los tamaños de los tipos no administrados de origen y destino deben ser iguales.";
	String IMessageResource.NotStartedEnumerable => "La enumeración no ha comenzado. Llamar a MoveNext.";
	String IMessageResource.FinishedEnumerable => "La enumeración ya ha finalizado.";
	String IMessageResource.LessThanZero => "No puede ser menor que cero.";
	String IMessageResource.LargerThanRegionLength => "No puede ser mayor que la longitud de la región.";
	String IMessageResource.IndexOutOfRegion
		=> "El índice y la longitud deben referirse a una ubicación dentro de la región.";
	String IMessageResource.LargerThanStringLength => "No puede ser mayor que la longitud de la cadena.";
	String IMessageResource.IndexOutOfString
		=> "El índice y la longitud deben hacer referencia a una ubicación dentro de la cadena.";
	String IMessageResource.LargerThanSequenceLength => "No puede ser mayor que la longitud de la secuencia.";
	String IMessageResource.IndexOutOfSequence
		=> "El índice y la longitud deben hacer referencia a una ubicación dentro de la secuencia.";
	String IMessageResource.MissingMemoryInspector
		=> "La inspección de memoria no es compatible con la plataforma actual.";

	String IMessageResource.InvalidType(String requiredTypeName) => $"El objeto debe ser del tipo {requiredTypeName}.";
	String IMessageResource.InvalidRefTypePointer(Type typeOf)
		=> $"La instancia actual es insuficiente para contener un valor del tipo {typeOf}.";
	String IMessageResource.InvalidBinarySpanSize(String nameofSpan, Int32 sizeOf)
		=> $"La longitud del parámetro {nameofSpan} debe ser igual a {sizeOf}.";
	String IMessageResource.InvalidCopyUnmanagedType(String nameofSpan, String nameofValue)
		=> $"Tamaño disponible insuficiente en {nameofSpan} para copiar {nameofValue}.";
	String IMessageResource.InvalidLength(String nameofLength)
		=> $"El parámetro {nameofLength} debe ser cero o un entero positivo.";
	String IMessageResource.InvalidUtf8Region(String nameofRegion) => $"{nameofRegion} no contiene texto UTF-8.";
	String IMessageResource.NotUnmanagedType(Type type) => $"{type} no es un tipo no administrado.";
	String IMessageResource.NotValueType(Type type) => $"{type} no es un tipo de valor.";
	String IMessageResource.NotReferenceType(Type type) => $"{type} no es un tipo de referencia.";
	String IMessageResource.NotType(Type sourceType, Type destinationType) => $"{sourceType} no es {destinationType}.";
	String IMessageResource.ContainsReferencesButUnmanaged(Type itemType, Type arrayType)
		=> $"{itemType} contiene referencias pero {arrayType} es un tipo no administrado.";
	String IMessageResource.ReferencesTypeButUnmanaged(Type itemType, Type arrayType)
		=> $"{itemType} es un tipo de referencia pero {arrayType} es un tipo no administrado.";
	String IMessageResource.UnmanagedTypeButContainsReferences(Type itemType, Type arrayType)
		=> $"{itemType} es un tipo no administrado pero {arrayType} contiene referencias.";
	String IMessageResource.MissingBufferMetadataException(Type itemType, UInt16 size)
		=> $"No se puede crear un búfer para {itemType} con {size} elementos.";
	String IMessageResource.InvalidToken(String currentToken, String expectedToken)
		=> $"Tipo de token inesperado: {currentToken}. Se esperaba el tipo de token: {expectedToken}.";
}