namespace Rxmxnx.PInvoke.Internal.Localization;

/// <summary>
/// Japanese (日本語) message resource.
/// </summary>
#if !PACKAGE
[ExcludeFromCodeCoverage]
#endif
internal sealed class JapaneseMessageResource : IMessageResource
{
	/// <summary>
	/// Current instance.
	/// </summary>
	public static readonly JapaneseMessageResource Instance = new();

	/// <summary>
	/// Private constructor.
	/// </summary>
	private JapaneseMessageResource() { }

	String IMessageResource.InvalidListIndexMessage => "インデックスがリストの範囲外です。";
	String IMessageResource.InvalidSequenceIndex => "インデックスがシーケンスの範囲外です。";
	String IMessageResource.InvalidInstance => "現在のインスタンスは無効です。";
	String IMessageResource.InvalidPointerSerialization
		=> "8バイトの値を持つ IntPtr または UIntPtr は、4バイトのワードサイズのマシンでデシリアライズできません。";
	String IMessageResource.IsNotFunction => "現在のインスタンスは関数ではありません。";
	String IMessageResource.IsFunction => "現在のインスタンスは関数です。";
	String IMessageResource.ReadOnlyInstance => "現在のインスタンスは読み取り専用です。";
	String IMessageResource.InvalidUnmanagedCast => "ソースおよびターゲットの非管理型のサイズは等しくなければなりません。";
	String IMessageResource.NotStartedEnumerable => "列挙が開始されていません。MoveNext を呼び出してください。";
	String IMessageResource.FinishedEnumerable => "列挙はすでに終了しています。";
	String IMessageResource.LessThanZero => "ゼロ未満にはできません。";
	String IMessageResource.LargerThanRegionLength => "領域の長さを超えることはできません。";
	String IMessageResource.IndexOutOfRegion => "インデックスと長さは領域内の位置を参照する必要があります。";
	String IMessageResource.LargerThanStringLength => "文字列の長さを超えることはできません。";
	String IMessageResource.IndexOutOfString => "インデックスと長さは文字列内の位置を参照する必要があります。";
	String IMessageResource.LargerThanSequenceLength => "シーケンスの長さを超えることはできません。";
	String IMessageResource.IndexOutOfSequence => "インデックスと長さはシーケンス内の位置を参照する必要があります。";
	String IMessageResource.MissingMemoryInspector => "現在のプラットフォームではメモリ検査はサポートされていません。";
	String IMessageResource.ReflectionDisabled => "この機能を使用するにはフルリフレクションモードが必要です。";

	String IMessageResource.InvalidType(String requiredTypeName) => $"オブジェクトは {requiredTypeName} 型である必要があります。";
	String IMessageResource.InvalidRefTypePointer(Type typeOf) => $"現在のインスタンスは、{typeOf} 型の値を含むには不十分です。";
	String IMessageResource.InvalidBinarySpanSize(String nameofSpan, Int32 sizeOf)
		=> $"パラメータ {nameofSpan} の長さは {sizeOf} である必要があります。";
	String IMessageResource.InvalidCopyUnmanagedType(String nameofSpan, String nameofValue)
		=> $"{nameofSpan} に {nameofValue} をコピーするには十分な空きサイズがありません。";
	String IMessageResource.InvalidLength(String nameofLength) => $"パラメータ {nameofLength} はゼロまたは正の整数である必要があります。";
	String IMessageResource.InvalidUtf8Region(String nameofRegion) => $"{nameofRegion} は UTF-8 テキストを含んでいません。";
	String IMessageResource.NotUnmanagedType(Type type) => $"{type} は非管理型ではありません。";
	String IMessageResource.NotValueType(Type type) => $"{type} は値型ではありません。";
	String IMessageResource.NotReferenceType(Type type) => $"{type} は参照型ではありません。";
	String IMessageResource.NotType(Type sourceType, Type destinationType)
		=> $"{sourceType} は {destinationType} ではありません。";
	String IMessageResource.ContainsReferencesButUnmanaged(Type itemType, Type arrayType)
		=> $"{itemType} は参照を含みますが、{arrayType} は非管理型です。";
	String IMessageResource.ReferencesTypeButUnmanaged(Type itemType, Type arrayType)
		=> $"{itemType} は参照型ですが、{arrayType} は非管理型です。";
	String IMessageResource.UnmanagedTypeButContainsReferences(Type itemType, Type arrayType)
		=> $"{itemType} は非管理型ですが、{arrayType} は参照を含んでいます。";
#if !PACKAGE || !NET6_0_OR_GREATER
	String IMessageResource.MissingBufferMetadataException(Type bufferType) => $"{bufferType} バッファーのメタデータを取得できません。";
#endif
	String IMessageResource.MissingBufferMetadataException(Type itemType, UInt16 size)
		=> $"{itemType} の {size} アイテム用のバッファを作成できません。";
	String IMessageResource.InvalidToken(String currentToken, String expectedToken)
		=> $"予期しないトークンの種類: {currentToken}。期待されるトークンの種類: {expectedToken}。";
}