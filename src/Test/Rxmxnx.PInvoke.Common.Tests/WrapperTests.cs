namespace Rxmxnx.PInvoke.Tests;

[ExcludeFromCodeCoverage]
[SuppressMessage("csharpsquid", "S2699")]
public sealed class WrapperTests
{
	private static readonly IFixture fixture = new Fixture();

	[Fact]
	internal Task BooleanTestAsync() => WrapperTests.TestAsync<Boolean>();
	[Fact]
	internal Task ByteTestAsync() => WrapperTests.TestAsync<Byte>();
	[Fact]
	internal Task Int16TestAsync() => WrapperTests.TestAsync<Int16>();
	[Fact]
	internal Task CharTestAsync() => WrapperTests.TestAsync<Char>();
	[Fact]
	internal Task Int32TestAsync() => WrapperTests.TestAsync<Int32>();
	[Fact]
	internal Task Int64TestAsync() => WrapperTests.TestAsync<Int64>();
#if NET7_0_OR_GREATER
	[Fact]
	internal Task Int128TestAsync() => WrapperTests.TestAsync<Int128>();
#endif
	[Fact]
	internal Task GuidTestAsync() => WrapperTests.TestAsync<Guid>();
	[Fact]
	internal Task SingleTestAsync() => WrapperTests.TestAsync<Single>();
#if NET5_0_OR_GREATER
	[Fact]
	internal Task HalfTestAsync() => WrapperTests.TestAsync<Half>();
#endif
	[Fact]
	internal Task DoubleTestAsync() => WrapperTests.TestAsync<Double>();
	[Fact]
	internal Task DecimalTestAsync() => WrapperTests.TestAsync<Decimal>();
	[Fact]
	internal Task DateTimeTestAsync() => WrapperTests.TestAsync<DateTime>();
#if NET6_0_OR_GREATER
	[Fact]
	internal Task TimeOnlyTestAsync() => WrapperTests.TestAsync<TimeOnly>();
#endif
	[Fact]
	internal Task TimeSpanTestAsync() => WrapperTests.TestAsync<TimeSpan>();

	private static async Task TestAsync<T>() where T : unmanaged
	{
		Task inputTest = Task.Run(WrapperTests.Value<T>);
		Task nullTest1 = Task.Run(() => WrapperTests.Nullable<T>(false));
		Task nullTest2 = Task.Run(() => WrapperTests.Nullable<T>(true));
		Task objectTest = Task.Run(WrapperTests.ObjectTest<T>);

		await Task.WhenAll(inputTest, nullTest1, nullTest2, objectTest);
	}
	private static void Value<T>() where T : unmanaged
	{
		T value = WrapperTests.fixture.Create<T>();
		T value2 = WrapperTests.fixture.Create<T>();
		IWrapper<T> result = IWrapper.Create(value);
		Assert.NotNull(result);
		Assert.Equal(value, result.Value);
		Assert.True(result.Equals(value));
		Assert.Equal(Object.Equals(value, value2), result.Equals(value2));
	}
	private static void Nullable<T>(Boolean nullInput) where T : unmanaged
	{
		T? value = !nullInput ? WrapperTests.fixture.Create<T>() : null;
		T? value2 = WrapperTests.fixture.Create<Boolean>() ? WrapperTests.fixture.Create<T>() : null;
		IWrapper<T?> result = IWrapper.CreateNullable(value);
		Assert.NotNull(result);
		Assert.Equal(value, result.Value);
		Assert.Equal(Object.Equals(value, value2), result.Equals(value2));
	}
	private static void ObjectTest<T>() where T : unmanaged
	{
		T[] array = WrapperTests.fixture.CreateMany<T>().ToArray();
		T[] array2 = WrapperTests.fixture.CreateMany<T>().ToArray();
		IWrapper<T[]> result = IWrapper.CreateObject(array);
		Assert.NotNull(result);
		Assert.Equal(array, result.Value);
		Assert.True(result.Equals(array));
		Assert.Equal(Object.Equals(array, array2), result.Equals(array2));
	}
}