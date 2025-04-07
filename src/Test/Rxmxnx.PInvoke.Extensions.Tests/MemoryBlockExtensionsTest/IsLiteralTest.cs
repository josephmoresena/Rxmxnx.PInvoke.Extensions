namespace Rxmxnx.PInvoke.Tests.MemoryBlockExtensionsTest;

[ExcludeFromCodeCoverage]
[SuppressMessage("csharpsquid", "S2699")]
public sealed class IsLiteralTest
{
	private const Int32 IntegerValue = 49;
	private const String ConstantString = "CONST_STRING_VALUE";

	private static readonly String constantStringField = "CONST_STRING_FIELD";

	private static String ConstantStringProperty => "CONST_STRING_PROPERTY";
	private static ReadOnlySpan<Char> ConstantStringSpan => "CONST_STRING_SPAN";
	private static ReadOnlySpan<Char> ConstantCharSpan
		=> ['C', 'O', 'N', 'S', 'T', '_', 'S', 'T', 'R', 'I', 'N', 'G', '_', 'S', 'P', 'A', 'N', '\0',];
	private static ReadOnlySpan<Byte> ConstantByteSpan => "CONST_BYTE_SPAN"u8;

	[Fact]
	internal void Test()
	{
		const String constValue = "LOCAL_STRING_VALUE";
		const Int32 constIntValue = 90;

		Int32 value = Random.Shared.Next();

		Assert.False(MemoryMarshal.CreateReadOnlySpan(ref value, 1).IsLiteral());
		Assert.False(MemoryMarshal.CreateReadOnlySpan(ref UnsafeLegacy.AsRef(constIntValue), 1).IsLiteral());
		Assert.False(
			MemoryMarshal.CreateReadOnlySpan(ref UnsafeLegacy.AsRef(IsLiteralTest.IntegerValue), 1).IsLiteral());

		Assert.False(constValue.AsSpan().IsLiteral());
		Assert.False(IsLiteralTest.constantStringField.AsSpan().IsLiteral());
		Assert.False(IsLiteralTest.ConstantString.AsSpan().IsLiteral());
		Assert.False(IsLiteralTest.ConstantStringProperty.AsSpan().IsLiteral());
		Assert.False(IsLiteralTest.ConstantStringSpan.IsLiteral());

		Assert.True(IsLiteralTest.ConstantCharSpan.IsLiteral());
		Assert.True(IsLiteralTest.ConstantByteSpan.IsLiteral());
		Assert.True("LITERAL_BYTE_SPAN"u8.IsLiteral());
	}
}