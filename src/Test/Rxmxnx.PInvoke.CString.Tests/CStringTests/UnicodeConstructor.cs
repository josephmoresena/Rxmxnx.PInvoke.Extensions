namespace Rxmxnx.PInvoke.Tests.CStringTests;

[TestFixture]
[ExcludeFromCodeCoverage]
[SuppressMessage("csharpsquid", "S2699")]
public sealed class UnicodeConstructor
{
	[Theory]
	[InlineData("Hello")]
	[InlineData("Привет")]
	[InlineData("こんにちは")]
	public void SpanTest(String text)
	{
		CString value = new(text);
		PInvokeAssert.Equal(text, value.ToString());
	}
	[Theory]
	[InlineData("He")]
	[InlineData("Пр")]
	[InlineData("こん")]
	public void CharTest(String text)
	{
		for (Int32 i = 0; i < 10; i++)
		{
			CString c0 = new(text[0], i);
			CString c1 = new(text[0], text[1], i);

			PInvokeAssert.Equal(new(Enumerable.Repeat(text[0], i).ToArray()), c0.ToString());
			PInvokeAssert.Equal(String.Concat(Enumerable.Repeat($"{text[0]}{text[1]}", i)), c1.ToString());
		}
	}
}