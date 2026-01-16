namespace Rxmxnx.PInvoke.Tests.Internal.BinaryConcatenatorTests;

[TestFixture]
[ExcludeFromCodeCoverage]
public sealed class EmptyTest
{
	private readonly IFixture _fixture = new Fixture();

	[Theory]
	[InlineData(true)]
	[InlineData(false)]
	public void Test(Boolean nullSeparator)
	{
		Byte? separator = !nullSeparator ? this._fixture.Create<Byte>() : default(Byte?);
		using BinaryConcatenator helper = separator.HasValue ? new(separator.Value) : new();
		PInvokeAssert.Null(helper.ToArray(false));
		PInvokeAssert.Null(helper.ToArray(true));
	}

	[Theory]
	[InlineData(true)]
	[InlineData(false)]
	public async Task TestAsync(Boolean nullSeparator)
	{
		Byte? separator = !nullSeparator ? this._fixture.Create<Byte>() : default(Byte?);
		BinaryConcatenator helper = separator.HasValue ? new(separator.Value) : new();
		try
		{
			PInvokeAssert.Null(helper.ToArray(false));
			PInvokeAssert.Null(helper.ToArray(true));
		}
		finally
		{
			await helper.DisposeAsync();
		}
	}
}