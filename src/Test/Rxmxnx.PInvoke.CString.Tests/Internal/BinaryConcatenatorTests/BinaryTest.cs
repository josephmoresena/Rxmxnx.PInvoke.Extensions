#if !NETCOREAPP
using InlineData = NUnit.Framework.TestCaseAttribute;
#endif

namespace Rxmxnx.PInvoke.Tests.Internal.BinaryConcatenatorTests;

[TestFixture]
[ExcludeFromCodeCoverage]
public sealed class BinaryTest
{
	private readonly IFixture _fixture = new Fixture();

	[Theory]
	[InlineData(true)]
	[InlineData(false)]
	public void Test(Boolean nullSeparator)
	{
		Byte[] sourceBytes = Encoding.UTF8.GetBytes(this._fixture.Create<String>());
		Byte? separator = !nullSeparator ? this._fixture.Create<Byte>() : default(Byte?);
		using BinaryConcatenator helper = separator.HasValue ? new(separator.Value) : new();

		foreach (Byte b in sourceBytes)
			helper.Write(b);

		BinaryTest.AssertTest(helper, sourceBytes);
	}

	[Theory]
	[InlineData(true)]
	[InlineData(false)]
	public async Task TestAsync(Boolean nullSeparator)
	{
		Byte[] sourceBytes = Encoding.UTF8.GetBytes(this._fixture.Create<String>());
		Byte? separator = !nullSeparator ? this._fixture.Create<Byte>() : default(Byte?);
		await using BinaryConcatenator helper = separator.HasValue ? new(separator.Value) : new();
		foreach (Byte b in sourceBytes)
			await helper.WriteAsync(b);

		BinaryTest.AssertTest(helper, sourceBytes);
	}

	private static void AssertTest(BinaryConcatenator helper, Byte[] sourceBytes)
	{
		Byte[]? outputBytes1 = helper.ToArray(false);
		Byte[]? outputBytes2 = helper.ToArray(true);

		PInvokeAssert.NotNull(outputBytes1);
		PInvokeAssert.NotNull(outputBytes2);
		if (!helper.Separator.HasValue)
			PInvokeAssert.Equal(sourceBytes, outputBytes1);
		else
			BinaryTest.AssertWithSeparator(helper, sourceBytes, outputBytes1);
		PInvokeAssert.Equal(outputBytes1, outputBytes2[..outputBytes1.Length]);
		PInvokeAssert.Equal(outputBytes1.Length, outputBytes2.Length - 1);
		PInvokeAssert.Equal(default, outputBytes2.Last());
	}
	private static void AssertWithSeparator(BinaryConcatenator helper, Byte[] sourceBytes, Byte[] outputBytes1)
	{
		for (Int32 i = 0; i < sourceBytes.Length; i++)
		{
			Int32 iByte = i * 2;
			Int32 iSeparator = iByte + 1;

			PInvokeAssert.Equal(sourceBytes[i], outputBytes1[iByte]);
			if (iSeparator < outputBytes1.Length)
				PInvokeAssert.Equal(helper.Separator, outputBytes1[iSeparator]);
		}
	}
}