namespace Rxmxnx.PInvoke.Tests.Internal;

[ExcludeFromCodeCoverage]
public sealed class ArgumentNullTest
{
	private static readonly IFixture fixture = new Fixture();

	[Fact]
	internal void Test()
	{
		String[] values = ArgumentNullTest.fixture.CreateMany<String>(10).ToArray();
		Object? nullValue = null;
		ArgumentNullException ex1 =
			Assert.Throws<ArgumentNullException>(() => ArgumentNullException.ThrowIfNull(nullValue));
		ArgumentNullException ex2 =
			Assert.Throws<ArgumentNullException>(() => ArgumentNullExceptionCompat.ThrowIfNull(nullValue));

		Assert.Equal(ex1.Message, ex2.Message);

		foreach (String value in values)
		{
			ex1 = Assert.Throws<ArgumentNullException>(() => ArgumentNullException.ThrowIfNull(nullValue, value));
			ex2 = Assert.Throws<ArgumentNullException>(() => ArgumentNullExceptionCompat.ThrowIfNull(nullValue, value));

			Assert.Equal(ex1.Message, ex2.Message);
		}
	}
}