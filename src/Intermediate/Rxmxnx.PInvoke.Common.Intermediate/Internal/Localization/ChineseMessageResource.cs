namespace Rxmxnx.PInvoke.Internal.Localization;

/// <summary>
/// Chinese (中文) message resource.
/// </summary>
#if !PACKAGE
[ExcludeFromCodeCoverage]
#endif
internal sealed class ChineseMessageResource : IMessageResource
{
	/// <summary>
	/// Current instance.
	/// </summary>
	public static readonly ChineseMessageResource Instance = new();

	/// <summary>
	/// Private constructor.
	/// </summary>
	private ChineseMessageResource() { }

	String IMessageResource.InvalidListIndexMessage => "索引超出了列表的范围。";
	String IMessageResource.InvalidSequenceIndex => "索引超出了序列的范围。";
	String IMessageResource.InvalidInstance => "当前实例无效。";
	String IMessageResource.InvalidPointerSerialization => "具有八字节值的 IntPtr 或 UIntPtr 无法在四字节字大小的机器上反序列化。";
	String IMessageResource.IsNotFunction => "当前实例不是函数。";
	String IMessageResource.IsFunction => "当前实例是一个函数。";
	String IMessageResource.ReadOnlyInstance => "当前实例是只读的。";
	String IMessageResource.InvalidUnmanagedCast => "源和目标非托管类型的大小必须相等。";
	String IMessageResource.NotStartedEnumerable => "枚举尚未开始。调用 MoveNext。";
	String IMessageResource.FinishedEnumerable => "枚举已经完成。";
	String IMessageResource.LessThanZero => "不能小于零。";
	String IMessageResource.LargerThanRegionLength => "不能大于区域的长度。";
	String IMessageResource.IndexOutOfRegion => "索引和长度必须引用区域内的位置。";
	String IMessageResource.LargerThanStringLength => "不能大于字符串的长度。";
	String IMessageResource.IndexOutOfString => "索引和长度必须引用字符串内的位置。";
	String IMessageResource.LargerThanSequenceLength => "不能大于序列的长度。";
	String IMessageResource.IndexOutOfSequence => "索引和长度必须引用序列内的位置。";
	String IMessageResource.MissingMemoryInspector => "当前平台不支持内存检查。";
	String IMessageResource.ReflectionDisabled => "此功能需要启用完整反射模式。";

	String IMessageResource.InvalidType(String requiredTypeName) => $"对象必须是 {requiredTypeName} 类型。";
	String IMessageResource.InvalidRefTypePointer(Type typeOf) => $"当前实例不足以包含 {typeOf} 类型的值。";
	String IMessageResource.InvalidBinarySpanSize(String nameofSpan, Int32 sizeOf)
		=> $"参数 {nameofSpan} 的长度必须等于 {sizeOf}。";
	String IMessageResource.InvalidCopyUnmanagedType(String nameofSpan, String nameofValue)
		=> $"在 {nameofSpan} 上没有足够的可用空间来复制 {nameofValue}。";
	String IMessageResource.InvalidLength(String nameofLength) => $"参数 {nameofLength} 必须是零或正整数。";
	String IMessageResource.InvalidUtf8Region(String nameofRegion) => $"{nameofRegion} 不包含 UTF-8 文本。";
	String IMessageResource.NotUnmanagedType(Type type) => $"{type} 不是非托管类型。";
	String IMessageResource.NotValueType(Type type) => $"{type} 不是值类型。";
	String IMessageResource.NotReferenceType(Type type) => $"{type} 不是引用类型。";
	String IMessageResource.NotType(Type sourceType, Type destinationType) => $"{sourceType} 不是 {destinationType}。";
	String IMessageResource.ContainsReferencesButUnmanaged(Type itemType, Type arrayType)
		=> $"{itemType} 包含引用，但 {arrayType} 是非托管类型。";
	String IMessageResource.ReferencesTypeButUnmanaged(Type itemType, Type arrayType)
		=> $"{itemType} 是引用类型，但 {arrayType} 是非托管类型。";
	String IMessageResource.UnmanagedTypeButContainsReferences(Type itemType, Type arrayType)
		=> $"{itemType} 是非托管类型，但 {arrayType} 包含引用。";
#if !PACKAGE || !NET6_0_OR_GREATER
	String IMessageResource.MissingBufferMetadataException(Type bufferType) => $"无法获取 {bufferType} 缓冲区的元数据。";
#endif
	String IMessageResource.MissingBufferMetadataException(Type itemType, UInt16 size)
		=> $"无法为 {itemType} 创建包含 {size} 项的缓冲区。";
#if !PACKAGE || NETCOREAPP
	String IMessageResource.InvalidToken(String currentToken, String expectedToken)
		=> $"意外的标记类型：{currentToken}。预期的标记类型：{expectedToken}。";
#endif
#if NET9_0_OR_GREATER
	String IMessageResource.NotObjectType(Type type) => $"{type} 是 ref struct；不允许进行对象装箱。";
#endif
}