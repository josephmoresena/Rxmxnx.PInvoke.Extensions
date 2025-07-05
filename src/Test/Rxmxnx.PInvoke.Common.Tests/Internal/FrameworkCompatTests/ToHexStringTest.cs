#if NET5_0_OR_GREATER
namespace Rxmxnx.PInvoke.Tests.Internal;

[ExcludeFromCodeCoverage]
public sealed class ToHexStringTest
{
	private static readonly IFixture fixture = new Fixture();

	[Fact]
	internal void Test()
	{
		Byte[][] values = ToHexStringTest.fixture.CreateMany<String>(10).Select(s => Encoding.UTF8.GetBytes(s))
		                                 .ToArray();
		foreach (Byte[] value in values)
			Assert.Equal(Convert.ToHexString(value), ConvertCompat.ToHexString(value));
	}
}
#endif