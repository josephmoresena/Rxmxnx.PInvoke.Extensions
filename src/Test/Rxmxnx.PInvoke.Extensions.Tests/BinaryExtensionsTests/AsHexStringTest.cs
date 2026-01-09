namespace Rxmxnx.PInvoke.Tests.BinaryExtensionsTests;

[TestFixture]
[ExcludeFromCodeCoverage]
public sealed class AsHexStringTest
{
	private static readonly IFixture fixture = new Fixture();

	[Fact]
	public void NormalTest()
	{
		Byte[] input = AsHexStringTest.fixture.Create<Byte[]>();
		StringBuilder strBuild = new();
		foreach (Byte value in input)
		{
			PInvokeAssert.Equal(value.ToString("X2").ToLowerInvariant(), value.AsHexString());
			strBuild.Append(value.ToString("X2"));
		}
		PInvokeAssert.Equal(strBuild.ToString().ToLowerInvariant(), input.AsHexString());
	}
}