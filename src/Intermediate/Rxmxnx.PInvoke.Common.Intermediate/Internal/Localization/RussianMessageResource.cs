namespace Rxmxnx.PInvoke.Internal.Localization;

/// <summary>
/// Russian (Русский) message resource.
/// </summary>
#if !PACKAGE
[ExcludeFromCodeCoverage]
#endif
internal sealed class RussianMessageResource : IMessageResource
{
	/// <summary>
	/// Current instance.
	/// </summary>
	public static readonly RussianMessageResource Instance = new();

	/// <summary>
	/// Private constructor.
	/// </summary>
	private RussianMessageResource() { }

	String IMessageResource.InvalidListIndexMessage => "Индекс вышел за пределы списка.";
	String IMessageResource.InvalidSequenceIndex => "Индекс вышел за пределы последовательности.";
	String IMessageResource.InvalidInstance => "Текущий экземпляр недействителен.";
	String IMessageResource.InvalidPointerSerialization
		=> "IntPtr или UIntPtr с восьмибайтовым значением не может быть десериализован на машине с четырехбайтовым размером слова.";
	String IMessageResource.IsNotFunction => "Текущий экземпляр не является функцией.";
	String IMessageResource.IsFunction => "Текущий экземпляр является функцией.";
	String IMessageResource.ReadOnlyInstance => "Текущий экземпляр доступен только для чтения.";
	String IMessageResource.InvalidUnmanagedCast
		=> "Размеры исходного и целевого неуправляемых типов должны быть равны.";
	String IMessageResource.NotStartedEnumerable => "Перечисление не началось. Вызовите MoveNext.";
	String IMessageResource.FinishedEnumerable => "Перечисление уже завершено.";
	String IMessageResource.LessThanZero => "Не может быть меньше нуля.";
	String IMessageResource.LargerThanRegionLength => "Не может быть больше длины региона.";
	String IMessageResource.IndexOutOfRegion => "Индекс и длина должны указывать на местоположение внутри региона.";
	String IMessageResource.LargerThanStringLength => "Не может быть больше длины строки.";
	String IMessageResource.IndexOutOfString => "Индекс и длина должны указывать на местоположение внутри строки.";
	String IMessageResource.LargerThanSequenceLength => "Не может быть больше длины последовательности.";
	String IMessageResource.IndexOutOfSequence
		=> "Индекс и длина должны указывать на местоположение внутри последовательности.";
	String IMessageResource.MissingMemoryInspector => "Проверка памяти не поддерживается на текущей платформе.";
	String IMessageResource.ReflectionDisabled => "Эта функция требует режима полной рефлексии.";

	String IMessageResource.InvalidType(String requiredTypeName) => $"Объект должен быть типа {requiredTypeName}.";
	String IMessageResource.InvalidRefTypePointer(Type typeOf)
		=> $"Текущий экземпляр недостаточен для значения типа {typeOf}.";
	String IMessageResource.InvalidBinarySpanSize(String nameofSpan, Int32 sizeOf)
		=> $"Длина параметра {nameofSpan} должна быть равна {sizeOf}.";
	String IMessageResource.InvalidCopyUnmanagedType(String nameofSpan, String nameofValue)
		=> $"Недостаточно доступного места в {nameofSpan} для копирования {nameofValue}.";
	String IMessageResource.InvalidLength(String nameofLength)
		=> $"Параметр {nameofLength} должен быть равен нулю или положительному целому числу.";
	String IMessageResource.InvalidUtf8Region(String nameofRegion)
		=> $"{nameofRegion} не содержит текста в формате UTF-8.";
	String IMessageResource.NotUnmanagedType(Type type) => $"{type} не является неуправляемым типом.";
	String IMessageResource.NotValueType(Type type) => $"{type} не является значимым типом.";
	String IMessageResource.NotReferenceType(Type type) => $"{type} не является ссылочным типом.";
	String IMessageResource.NotType(Type sourceType, Type destinationType)
		=> $"{sourceType} не является {destinationType}.";
	String IMessageResource.ContainsReferencesButUnmanaged(Type itemType, Type arrayType)
		=> $"{itemType} содержит ссылки, но {arrayType} является неуправляемым типом.";
	String IMessageResource.ReferencesTypeButUnmanaged(Type itemType, Type arrayType)
		=> $"{itemType} является ссылочным типом, но {arrayType} является неуправляемым типом.";
	String IMessageResource.UnmanagedTypeButContainsReferences(Type itemType, Type arrayType)
		=> $"{itemType} является неуправляемым типом, но {arrayType} содержит ссылки.";
#if !PACKAGE || !NET6_0_OR_GREATER
	String IMessageResource.MissingBufferMetadataException(Type bufferType)
		=> $"Не удалось получить метаданные для буфера {bufferType}.";
#endif
	String IMessageResource.MissingBufferMetadataException(Type itemType, UInt16 size)
		=> $"Невозможно создать буфер для {itemType} с {size} элементами.";
#if !PACKAGE || NETCOREAPP
	String IMessageResource.InvalidToken(String currentToken, String expectedToken)
		=> $"Неожиданный тип токена: {currentToken}. Ожидаемый тип токена: {expectedToken}.";
#endif
#if NET9_0_OR_GREATER
	String IMessageResource.NotObjectType(Type type) => $"{type} — это ref struct; упаковка объекта не допускается.";
#endif
}