namespace Rxmxnx.PInvoke.Internal.Localization;

/// <summary>
/// French (français) message resource.
/// </summary>
#if !PACKAGE
[ExcludeFromCodeCoverage]
#endif
internal sealed class FrenchMessageResource : IMessageResource
{
	/// <summary>
	/// Current instance.
	/// </summary>
	public static readonly FrenchMessageResource Instance = new();

	/// <summary>
	/// Private constructor.
	/// </summary>
	private FrenchMessageResource() { }

	String IMessageResource.InvalidListIndexMessage => "L'index était hors des limites de la liste.";
	String IMessageResource.InvalidSequenceIndex => "L'index était hors des limites de la séquence.";
	String IMessageResource.InvalidInstance => "L'instance actuelle n'est pas valide.";
	String IMessageResource.InvalidPointerSerialization
		=> "Un IntPtr ou UIntPtr avec une valeur de huit octets ne peut pas être désérialisé sur une machine avec une taille de mot de quatre octets.";
	String IMessageResource.IsNotFunction => "L'instance actuelle n'est pas une fonction.";
	String IMessageResource.IsFunction => "L'instance actuelle est une fonction.";
	String IMessageResource.ReadOnlyInstance => "L'instance actuelle est en lecture seule.";
	String IMessageResource.InvalidUnmanagedCast
		=> "Les tailles des types non gérés source et destination doivent être égales.";
	String IMessageResource.NotStartedEnumerable => "L'énumération n'a pas commencé. Appelez MoveNext.";
	String IMessageResource.FinishedEnumerable => "L'énumération est déjà terminée.";
	String IMessageResource.LessThanZero => "Ne peut pas être inférieur à zéro.";
	String IMessageResource.LargerThanRegionLength => "Ne peut pas être plus grand que la longueur de la région.";
	String IMessageResource.IndexOutOfRegion
		=> "L'index et la longueur doivent faire référence à un emplacement dans la région.";
	String IMessageResource.LargerThanStringLength => "Ne peut pas être plus grand que la longueur de la chaîne.";
	String IMessageResource.IndexOutOfString
		=> "L'index et la longueur doivent faire référence à un emplacement dans la chaîne.";
	String IMessageResource.LargerThanSequenceLength => "Ne peut pas être plus grand que la longueur de la séquence.";
	String IMessageResource.IndexOutOfSequence
		=> "L'index et la longueur doivent faire référence à un emplacement dans la séquence.";
	String IMessageResource.MissingMemoryInspector
		=> "L'inspection de la mémoire n'est pas prise en charge sur la plateforme actuelle.";
	String IMessageResource.ReflectionDisabled => "Cette fonctionnalité nécessite le mode réflexion complète.";

	String IMessageResource.InvalidType(String requiredTypeName) => $"L'objet doit être de type {requiredTypeName}.";
	String IMessageResource.InvalidRefTypePointer(Type typeOf)
		=> $"L'instance actuelle est insuffisante pour contenir une valeur de type {typeOf}.";
	String IMessageResource.InvalidBinarySpanSize(String nameofSpan, Int32 sizeOf)
		=> $"La longueur du paramètre {nameofSpan} doit être égale à {sizeOf}.";
	String IMessageResource.InvalidCopyUnmanagedType(String nameofSpan, String nameofValue)
		=> $"Taille insuffisante disponible sur {nameofSpan} pour copier {nameofValue}.";
	String IMessageResource.InvalidLength(String nameofLength)
		=> $"Le paramètre {nameofLength} doit être zéro ou un entier positif.";
	String IMessageResource.InvalidUtf8Region(String nameofRegion) => $"{nameofRegion} ne contient pas de texte UTF-8.";
	String IMessageResource.NotUnmanagedType(Type type) => $"{type} n'est pas un type non géré.";
	String IMessageResource.NotValueType(Type type) => $"{type} n'est pas un type valeur.";
	String IMessageResource.NotReferenceType(Type type) => $"{type} n'est pas un type de référence.";
	String IMessageResource.NotType(Type sourceType, Type destinationType)
		=> $"{sourceType} n'est pas {destinationType}.";
	String IMessageResource.ContainsReferencesButUnmanaged(Type itemType, Type arrayType)
		=> $"{itemType} contient des références mais {arrayType} est un type non géré.";
	String IMessageResource.ReferencesTypeButUnmanaged(Type itemType, Type arrayType)
		=> $"{itemType} est un type de référence mais {arrayType} est un type non géré.";
	String IMessageResource.UnmanagedTypeButContainsReferences(Type itemType, Type arrayType)
		=> $"{itemType} est un type non géré mais {arrayType} contient des références.";
	String IMessageResource.MissingBufferMetadataException(Type itemType, UInt16 size)
		=> $"Impossible de créer un tampon pour {itemType} comportant {size} éléments.";
	String IMessageResource.InvalidToken(String currentToken, String expectedToken)
		=> $"Type de jeton inattendu : {currentToken}. Type de jeton attendu : {expectedToken}.";
}