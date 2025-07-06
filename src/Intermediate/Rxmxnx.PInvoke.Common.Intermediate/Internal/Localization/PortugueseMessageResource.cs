namespace Rxmxnx.PInvoke.Internal.Localization;

/// <summary>
/// Portuguese (Português) message resource.
/// </summary>
#if !PACKAGE
[ExcludeFromCodeCoverage]
#endif
internal sealed class PortugueseMessageResource : IMessageResource
{
	/// <summary>
	/// Current instance.
	/// </summary>
	public static readonly PortugueseMessageResource Instance = new();

	/// <summary>
	/// Private constructor.
	/// </summary>
	private PortugueseMessageResource() { }

	String IMessageResource.InvalidListIndexMessage => "O índice estava fora dos limites da lista.";
	String IMessageResource.InvalidSequenceIndex => "O índice estava fora dos limites da sequência.";
	String IMessageResource.InvalidInstance => "A instância atual não é válida.";
	String IMessageResource.InvalidPointerSerialization
		=> "Um IntPtr ou UIntPtr com um valor de oito bytes não pode ser desserializado em uma máquina com tamanho de palavra de quatro bytes.";
	String IMessageResource.IsNotFunction => "A instância atual não é uma função.";
	String IMessageResource.IsFunction => "A instância atual é uma função.";
	String IMessageResource.ReadOnlyInstance => "A instância atual é somente leitura.";
	String IMessageResource.InvalidUnmanagedCast
		=> "Os tamanhos dos tipos não gerenciados de origem e destino devem ser iguais.";
	String IMessageResource.NotStartedEnumerable => "A enumeração não foi iniciada. Chame MoveNext.";
	String IMessageResource.FinishedEnumerable => "A enumeração já foi concluída.";
	String IMessageResource.LessThanZero => "Não pode ser menor que zero.";
	String IMessageResource.LargerThanRegionLength => "Não pode ser maior que o comprimento da região.";
	String IMessageResource.IndexOutOfRegion
		=> "O índice e o comprimento devem se referir a uma posição dentro da região.";
	String IMessageResource.LargerThanStringLength => "Não pode ser maior que o comprimento da string.";
	String IMessageResource.IndexOutOfString
		=> "O índice e o comprimento devem se referir a uma posição dentro da string.";
	String IMessageResource.LargerThanSequenceLength => "Não pode ser maior que o comprimento da sequência.";
	String IMessageResource.IndexOutOfSequence
		=> "O índice e o comprimento devem se referir a uma posição dentro da sequência.";
	String IMessageResource.MissingMemoryInspector => "A inspeção de memória não é suportada na plataforma atual.";
	String IMessageResource.ReflectionDisabled => "Este recurso requer o modo de reflexão completa.";

	String IMessageResource.InvalidType(String requiredTypeName) => $"O objeto deve ser do tipo {requiredTypeName}.";
	String IMessageResource.InvalidRefTypePointer(Type typeOf)
		=> $"A instância atual é insuficiente para conter um valor do tipo {typeOf}.";
	String IMessageResource.InvalidBinarySpanSize(String nameofSpan, Int32 sizeOf)
		=> $"O comprimento do parâmetro {nameofSpan} deve ser igual a {sizeOf}.";
	String IMessageResource.InvalidCopyUnmanagedType(String nameofSpan, String nameofValue)
		=> $"Espaço insuficiente disponível em {nameofSpan} para copiar {nameofValue}.";
	String IMessageResource.InvalidLength(String nameofLength)
		=> $"O parâmetro {nameofLength} deve ser zero ou um número inteiro positivo.";
	String IMessageResource.InvalidUtf8Region(String nameofRegion) => $"{nameofRegion} não contém texto UTF-8.";
	String IMessageResource.NotUnmanagedType(Type type) => $"{type} não é um tipo não gerenciado.";
	String IMessageResource.NotValueType(Type type) => $"{type} não é um tipo de valor.";
	String IMessageResource.NotReferenceType(Type type) => $"{type} não é um tipo de referência.";
	String IMessageResource.NotType(Type sourceType, Type destinationType) => $"{sourceType} não é {destinationType}.";
	String IMessageResource.ContainsReferencesButUnmanaged(Type itemType, Type arrayType)
		=> $"{itemType} contém referências, mas {arrayType} é um tipo não gerenciado.";
	String IMessageResource.ReferencesTypeButUnmanaged(Type itemType, Type arrayType)
		=> $"{itemType} é um tipo de referência, mas {arrayType} é um tipo não gerenciado.";
	String IMessageResource.UnmanagedTypeButContainsReferences(Type itemType, Type arrayType)
		=> $"{itemType} é um tipo não gerenciado, mas {arrayType} contém referências.";
	String IMessageResource.MissingBufferMetadataException(Type bufferType)
		=> $"Não foi possível obter os metadados para o buffer {bufferType}.";
	String IMessageResource.MissingBufferMetadataException(Type itemType, UInt16 size)
		=> $"Não é possível criar um buffer para {itemType} com {size} itens.";
#if !PACKAGE || NETCOREAPP
	String IMessageResource.InvalidToken(String currentToken, String expectedToken)
		=> $"Tipo de token inesperado: {currentToken}. Tipo de token esperado: {expectedToken}.";
#endif
#if NET9_0_OR_GREATER
	String IMessageResource.NotObjectType(Type type) => $"{type} é uma ref struct; o boxing de objeto não é permitido.";
#endif
}