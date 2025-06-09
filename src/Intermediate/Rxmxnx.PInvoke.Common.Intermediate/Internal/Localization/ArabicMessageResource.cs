namespace Rxmxnx.PInvoke.Internal.Localization;

/// <summary>
/// Arabic (العربية) message resource.
/// </summary>
#if !PACKAGE
[ExcludeFromCodeCoverage]
#endif
internal sealed class ArabicMessageResource : IMessageResource
{
	/// <summary>
	/// Current instance.
	/// </summary>
	public static readonly ArabicMessageResource Instance = new();

	/// <summary>
	/// Private constructor.
	/// </summary>
	private ArabicMessageResource() { }

	String IMessageResource.InvalidListIndexMessage => "الفهرس خارج حدود القائمة.";
	String IMessageResource.InvalidSequenceIndex => "الفهرس خارج حدود التسلسل.";
	String IMessageResource.InvalidInstance => "المثال الحالي غير صالح.";
	String IMessageResource.InvalidPointerSerialization
		=> "لا يمكن إلغاء تسلسل IntPtr أو UIntPtr بقيمة ثمانية بايتات على جهاز بحجم كلمة أربعة بايتات.";
	String IMessageResource.IsNotFunction => "المثال الحالي ليس دالة.";
	String IMessageResource.IsFunction => "المثال الحالي هو دالة.";
	String IMessageResource.ReadOnlyInstance => "المثال الحالي للقراءة فقط.";
	String IMessageResource.InvalidUnmanagedCast => "يجب أن تكون أحجام النوعين غير المُدار المصدر والوجهة متساوية.";
	String IMessageResource.NotStartedEnumerable => "لم يتم بدء التعداد. اتصل بـ MoveNext.";
	String IMessageResource.FinishedEnumerable => "تم الانتهاء من التعداد بالفعل.";
	String IMessageResource.LessThanZero => "لا يمكن أن يكون أقل من الصفر.";
	String IMessageResource.LargerThanRegionLength => "لا يمكن أن يكون أكبر من طول المنطقة.";
	String IMessageResource.IndexOutOfRegion => "يجب أن يشير الفهرس والطول إلى موقع داخل المنطقة.";
	String IMessageResource.LargerThanStringLength => "لا يمكن أن يكون أكبر من طول النص.";
	String IMessageResource.IndexOutOfString => "يجب أن يشير الفهرس والطول إلى موقع داخل النص.";
	String IMessageResource.LargerThanSequenceLength => "لا يمكن أن يكون أكبر من طول التسلسل.";
	String IMessageResource.IndexOutOfSequence => "يجب أن يشير الفهرس والطول إلى موقع داخل التسلسل.";
	String IMessageResource.MissingMemoryInspector => "لا يتم دعم فحص الذاكرة على النظام الأساسي الحالي.";
	String IMessageResource.ReflectionDisabled => "تتطلب هذه الميزة وضع الانعكاس الكامل.";

	String IMessageResource.InvalidType(String requiredTypeName) => $"يجب أن يكون الكائن من النوع {requiredTypeName}.";
	String IMessageResource.InvalidRefTypePointer(Type typeOf)
		=> $"المثال الحالي غير كافٍ لاحتواء قيمة من النوع {typeOf}.";
	String IMessageResource.InvalidBinarySpanSize(String nameofSpan, Int32 sizeOf)
		=> $"يجب أن يكون طول المعامل {nameofSpan} مساويًا لـ {sizeOf}.";
	String IMessageResource.InvalidCopyUnmanagedType(String nameofSpan, String nameofValue)
		=> $"المساحة المتوفرة على {nameofSpan} غير كافية لنسخ {nameofValue}.";
	String IMessageResource.InvalidLength(String nameofLength)
		=> $"يجب أن يكون المعامل {nameofLength} صفرًا أو عددًا صحيحًا موجبًا.";
	String IMessageResource.InvalidUtf8Region(String nameofRegion) => $"{nameofRegion} لا يحتوي على نص UTF-8.";
	String IMessageResource.NotUnmanagedType(Type type) => $"{type} ليس نوعًا غير مُدار.";
	String IMessageResource.NotValueType(Type type) => $"{type} ليس نوعًا قيمًا.";
	String IMessageResource.NotReferenceType(Type type) => $"{type} ليس نوعًا مرجعيًا.";
	String IMessageResource.NotType(Type sourceType, Type destinationType) => $"{sourceType} ليس {destinationType}.";
	String IMessageResource.ContainsReferencesButUnmanaged(Type itemType, Type arrayType)
		=> $"{itemType} يحتوي على مراجع لكن {arrayType} هو نوع غير مُدار.";
	String IMessageResource.ReferencesTypeButUnmanaged(Type itemType, Type arrayType)
		=> $"{itemType} هو نوع مرجعي لكن {arrayType} هو نوع غير مُدار.";
	String IMessageResource.UnmanagedTypeButContainsReferences(Type itemType, Type arrayType)
		=> $"{itemType} هو نوع غير مُدار لكن {arrayType} يحتوي على مراجع.";
#if !PACKAGE || !NET6_0_OR_GREATER
	String IMessageResource.MissingBufferMetadataException(Type bufferType)
		=> $"تعذّر استرداد البيانات الوصفية لمخزن {bufferType}.";
#endif
	String IMessageResource.MissingBufferMetadataException(Type itemType, UInt16 size)
		=> $"تعذّر إنشاء مخزن مؤقت لـ {itemType} يضم {size} عناصر.";
	String IMessageResource.InvalidToken(String currentToken, String expectedToken)
		=> $"نوع الرمز غير المتوقع: {currentToken}. نوع الرمز المتوقع: {expectedToken}.";
}