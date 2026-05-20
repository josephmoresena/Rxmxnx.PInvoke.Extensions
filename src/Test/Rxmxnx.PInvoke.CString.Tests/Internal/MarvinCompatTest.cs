namespace Rxmxnx.PInvoke.Tests.Internal;

[TestFixture]
[ExcludeFromCodeCoverage]
public sealed class MarvinCompatTest
{
	private readonly Boolean _hasMarvin = MarvinCompat.DefaultSeed.HasValue;

	[Fact]
	public void SpanTest()
	{
		if (!this._hasMarvin) return;
		foreach (String text in TestSet.Utf16Text)
		{
			ReadOnlySpan<Char> chars = text;
			PInvokeAssert.Equal(text.GetHashCode(), MarvinCompat.GetHashCode(chars));
		}
	}
	[Fact]
	public void Utf8Test()
	{
		if (!this._hasMarvin) return;
		for (Int32 index = 0; index < TestSet.Utf16Text.Count; index++)
		{
			String text = TestSet.Utf16Text[index];
			ReadOnlySpan<Byte> bytes = TestSet.Utf8Text[index]();
			PInvokeAssert.Equal(text.GetHashCode(), MarvinCompat.GetHashCode(bytes));
		}
	}
}