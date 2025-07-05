namespace Rxmxnx.PInvoke.Tests.NativeUtilitiesTests;

[ExcludeFromCodeCoverage]
[SuppressMessage("csharpsquid", "S2699")]
public sealed class CreateArrayTest
{
	private static readonly IFixture fixture = new Fixture();

	[Fact]
	internal void NormalTest()
	{
		CreateArrayTest.CreateTest<Boolean>();
		CreateArrayTest.CreateTest<Byte>();
		CreateArrayTest.CreateTest<Int16>();
		CreateArrayTest.CreateTest<Int32>();
		CreateArrayTest.CreateTest<Int64>();
		CreateArrayTest.CreateTest<Single>();
		CreateArrayTest.CreateTest<Double>();
		CreateArrayTest.CreateTest<Decimal>();
	}

	private static void CreateTest<T>() where T : unmanaged
	{
		Int32 length = Random.Shared.Next(0, 129);
		List<T> list = [];
		T[] arr = NativeUtilities.CreateArray<T, IList<T>>(length, list, (span, _) =>
		{
			foreach (ref T value in span)
			{
				value = CreateArrayTest.fixture.Create<T>();
				list.Add(value);
			}
		});

		Assert.Equal(list, arr);
	}
#if !NET6_0_OR_GREATER
	private static class Random
	{
		public static readonly System.Random Shared = new();
	}
#endif
}