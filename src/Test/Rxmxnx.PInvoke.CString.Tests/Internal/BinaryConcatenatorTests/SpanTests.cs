namespace Rxmxnx.PInvoke.Tests.Internal.BinaryConcatenatorTests;

[TestFixture]
[ExcludeFromCodeCoverage]
public sealed class SpanTests
{
	private readonly IFixture _fixture = new Fixture();

	[Theory]
	[InlineData(true)]
	[InlineData(false)]
	public void SingleSpanTest(Boolean nullSeparator)
	{
		Byte[] sourceBytes = Encoding.UTF8.GetBytes(this._fixture.Create<String>());
		Byte? separator = !nullSeparator ? this._fixture.Create<Byte>() : default(Byte?);
		using BinaryConcatenator helper = separator.HasValue ? new(separator.Value) : new();

		helper.Write(sourceBytes);
		SpanTests.SingleSpanAssert(helper, sourceBytes);
	}

	[Theory]
	[InlineData(true)]
	[InlineData(false)]
	public void MultipleSpanTest(Boolean nullSeparator)
	{
		Byte[] sourceBytes1 = Encoding.UTF8.GetBytes(this._fixture.Create<String>());
		Byte[] sourceBytes2 = Encoding.UTF8.GetBytes(this._fixture.Create<String>());
		Byte? separator = !nullSeparator ? this._fixture.Create<Byte>() : default(Byte?);
		using BinaryConcatenator helper = separator.HasValue ? new(separator.Value) : new();

		helper.Write(sourceBytes1);
		helper.Write(sourceBytes2);
		SpanTests.MultipleSpanAssert(sourceBytes1, sourceBytes2, helper);
	}

	private static void SingleSpanAssert(BinaryConcatenator helper, Byte[] sourceBytes)
	{
		Boolean separatorInBytes = sourceBytes.Any(b => b == helper.Separator);
		Byte[]? outputBytes1 = helper.ToArray(false);
		Byte[]? outputBytes2 = helper.ToArray(true);

		PInvokeAssert.NotNull(outputBytes1);
		PInvokeAssert.NotNull(outputBytes2);

		PInvokeAssert.Equal(sourceBytes, outputBytes1);
		PInvokeAssert.Equal(sourceBytes, outputBytes2[..sourceBytes.Length]);
		PInvokeAssert.Equal(outputBytes1.Length, outputBytes2.Length - 1);
		PInvokeAssert.Equal(default, outputBytes2.Last());

		PInvokeAssert.Equal(separatorInBytes, outputBytes1.Any(b => b == helper.Separator));
	}

	private static void MultipleSpanAssert(Byte[] sourceBytes1, Byte[] sourceBytes2, BinaryConcatenator helper)
	{
		Int32 output2Start = sourceBytes1.Length + (helper.Separator.HasValue ? 1 : 0);
		Int32 finalLength = output2Start + sourceBytes2.Length;
		Byte[]? outputBytes1 = helper.ToArray(false);
		Byte[]? outputBytes2 = helper.ToArray(true);

		PInvokeAssert.NotNull(outputBytes1);
		PInvokeAssert.NotNull(outputBytes2);
		PInvokeAssert.Equal(sourceBytes1, outputBytes1[..sourceBytes1.Length]);
		if (helper.Separator.HasValue)
			PInvokeAssert.Equal(helper.Separator, outputBytes1[sourceBytes1.Length]);
		PInvokeAssert.Equal(sourceBytes2, outputBytes1[output2Start..]);
		if (helper.Separator.HasValue)
			PInvokeAssert.Contains(helper.Separator.Value, outputBytes1);

		PInvokeAssert.Equal(outputBytes1, outputBytes2[..finalLength]);
		PInvokeAssert.Equal(outputBytes1.Length, outputBytes2.Length - 1);
		PInvokeAssert.Equal(default, outputBytes2.Last());
	}
}