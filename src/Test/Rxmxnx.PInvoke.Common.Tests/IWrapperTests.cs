namespace Rxmxnx.PInvoke.Tests;

[ExcludeFromCodeCoverage]
[SuppressMessage("csharpsquid", "S2699")]
public sealed class IWrapperTests
{
	private static readonly IFixture fixture = new Fixture();

	[Fact]
	internal Task BooleanTestAsync() => IWrapperTests.TestAsync<Boolean>();
	[Fact]
	internal Task ByteTestAsync() => IWrapperTests.TestAsync<Byte>();
	[Fact]
	internal Task Int16TestAsync() => IWrapperTests.TestAsync<Int16>();
	[Fact]
	internal Task CharTestAsync() => IWrapperTests.TestAsync<Char>();
	[Fact]
	internal Task Int32TestAsync() => IWrapperTests.TestAsync<Int32>();
	[Fact]
	internal Task Int64TestAsync() => IWrapperTests.TestAsync<Int64>();
	[Fact]
	internal Task Int128TestAsync() => IWrapperTests.TestAsync<Int128>();
	[Fact]
	internal Task GuidTestAsync() => IWrapperTests.TestAsync<Guid>();
	[Fact]
	internal Task SingleTestAsync() => IWrapperTests.TestAsync<Single>();
	[Fact]
	internal Task HalfTestAsync() => IWrapperTests.TestAsync<Half>();
	[Fact]
	internal Task DoubleTestAsync() => IWrapperTests.TestAsync<Double>();
	[Fact]
	internal Task DecimalTestAsync() => IWrapperTests.TestAsync<Decimal>();
	[Fact]
	internal Task DateTimeTestAsync() => IWrapperTests.TestAsync<DateTime>();
	[Fact]
	internal Task TimeOnlyTestAsync() => IWrapperTests.TestAsync<TimeOnly>();
	[Fact]
	internal Task TimeSpanTestAsync() => IWrapperTests.TestAsync<TimeSpan>();

	private static async Task TestAsync<T>() where T : unmanaged
	{
		Task inputTest = Task.Run(IWrapperTests.Value<T>);
		Task nullTest1 = Task.Run(() => IWrapperTests.Nullable<T>(false));
		Task nullTest2 = Task.Run(() => IWrapperTests.Nullable<T>(true));
		Task objectTest = Task.Run(IWrapperTests.Object<T>);

		await Task.WhenAll(inputTest, nullTest1, nullTest2, objectTest);
	}
	private static void Value<T>() where T : unmanaged
	{
		T value = IWrapperTests.fixture.Create<T>();
		T value2 = IWrapperTests.fixture.Create<T>();
		IWrapper<T> result = IWrapper.Create(value);
		Assert.NotNull(result);
		Assert.Equal(value, result.Value);
		Assert.True(result.Equals(value));
		Assert.Equal(Equals(value, value2), result.Equals(value2));
	}
	private static void Nullable<T>(Boolean nullInput) where T : unmanaged
	{
		T? value = !nullInput ? IWrapperTests.fixture.Create<T>() : null;
		T? value2 = IWrapperTests.fixture.Create<Boolean>() ? IWrapperTests.fixture.Create<T>() : null;
		IWrapper<T?> result = IWrapper.CreateNullable(value);
		Assert.NotNull(result);
		Assert.Equal(value, result.Value);
		Assert.Equal(Equals(value, value2), result.Equals(value2));
	}
	private static void Object<T>() where T : unmanaged
	{
		T[] array = IWrapperTests.fixture.CreateMany<T>().ToArray();
		T[] array2 = IWrapperTests.fixture.CreateMany<T>().ToArray();
		IWrapper<T[]> result = IWrapper.CreateObject(array);
		Assert.NotNull(result);
		Assert.Equal(array, result.Value);
		Assert.True(result.Equals(array));
		Assert.Equal(Equals(array, array2), result.Equals(array2));
	}
}