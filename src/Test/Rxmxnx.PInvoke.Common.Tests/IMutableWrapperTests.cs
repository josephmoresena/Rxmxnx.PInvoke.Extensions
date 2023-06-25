namespace Rxmxnx.PInvoke.Tests;

[ExcludeFromCodeCoverage]
[SuppressMessage("csharpsquid", "S2699")]
public sealed class IMutableWrapperTests
{
	private static readonly IFixture fixture = new Fixture();

	[Fact]
	internal void InterfaceTest()
	{
		IMutableWrapper<String> instance = new MutableInstance<String>();
		instance.Value = IMutableWrapperTests.fixture.Create<String>();
		Assert.Equal(instance.Value, (instance as IWrapper<String>).Value);
	}

	[Fact]
	internal Task BooleanTestAsync() => IMutableWrapperTests.TestAsync<Boolean>();
	[Fact]
	internal Task ByteTestAsync() => IMutableWrapperTests.TestAsync<Byte>();
	[Fact]
	internal Task Int16TestAsync() => IMutableWrapperTests.TestAsync<Int16>();
	[Fact]
	internal Task CharTestAsync() => IMutableWrapperTests.TestAsync<Char>();
	[Fact]
	internal Task Int32TestAsync() => IMutableWrapperTests.TestAsync<Int32>();
	[Fact]
	internal Task Int64TestAsync() => IMutableWrapperTests.TestAsync<Int64>();
	[Fact]
	internal Task Int128TestAsync() => IMutableWrapperTests.TestAsync<Int128>();
	[Fact]
	internal Task GuidTestAsync() => IMutableWrapperTests.TestAsync<Guid>();
	[Fact]
	internal Task SingleTestAsync() => IMutableWrapperTests.TestAsync<Single>();
	[Fact]
	internal Task HalfTestAsync() => IMutableWrapperTests.TestAsync<Half>();
	[Fact]
	internal Task DoubleTestAsync() => IMutableWrapperTests.TestAsync<Double>();
	[Fact]
	internal Task DecimalTestAsync() => IMutableWrapperTests.TestAsync<Decimal>();
	[Fact]
	internal Task DateTimeTestAsync() => IMutableWrapperTests.TestAsync<DateTime>();
	[Fact]
	internal Task TimeOnlyTestAsync() => IMutableWrapperTests.TestAsync<TimeOnly>();
	[Fact]
	internal Task TimeSpanTestAsync() => IMutableWrapperTests.TestAsync<TimeSpan>();

	private static async Task TestAsync<T>() where T : unmanaged
	{
		Task inputTest = Task.Run(IMutableWrapperTests.Value<T>);
		Task nullTest1 = Task.Run(() => IMutableWrapperTests.Nullable<T>(false));
		Task nullTest2 = Task.Run(() => IMutableWrapperTests.Nullable<T>(true));
		Task objectTest = Task.Run(IMutableWrapperTests.Object<T>);

		await Task.WhenAll(inputTest, nullTest1, nullTest2, objectTest);
	}

	private static void Value<T>() where T : unmanaged
	{
		T value = IMutableWrapperTests.fixture.Create<T>();
		T value2 = IMutableWrapperTests.fixture.Create<T>();
		IMutableWrapper<T> result = IMutableWrapper.Create(value);
		IWrapper<T> wrapper = result;

		Assert.NotNull(result);
		Assert.Equal(value, result.Value);
		Assert.True(result.Equals(value));
		Assert.Equal(object.Equals(value, value2), result.Equals(value2));
		Assert.NotNull(wrapper);
		Assert.Equal(value, wrapper.Value);
		Assert.True(wrapper.Equals(value));
		Assert.Equal(object.Equals(value, value2), wrapper.Equals(value2));

		result.Value = value2;
		Assert.Equal(value2, result.Value);
		Assert.True(result.Equals(value2));
		Assert.Equal(object.Equals(value2, value), result.Equals(value));
		Assert.Equal(value2, wrapper.Value);
		Assert.True(wrapper.Equals(value2));
		Assert.Equal(object.Equals(value2, value), wrapper.Equals(value));
	}

	private static void Nullable<T>(Boolean nullInput) where T : unmanaged
	{
		T? value = !nullInput ? IMutableWrapperTests.fixture.Create<T>() : null;
		T? value2 = IMutableWrapperTests.fixture.Create<Boolean>() ? IMutableWrapperTests.fixture.Create<T>() : null;
		IMutableWrapper<T?> result = IMutableWrapper.CreateNullable(value);
		IWrapper<T?> wrapper = result;

		Assert.NotNull(result);
		Assert.Equal(value, result.Value);
		Assert.Equal(object.Equals(value, value2), result.Equals(value2));
		Assert.NotNull(wrapper);
		Assert.Equal(value, wrapper.Value);
		Assert.True(wrapper.Equals(value));
		Assert.Equal(object.Equals(value, value2), wrapper.Equals(value2));

		result.Value = value2;
		Assert.Equal(value2, result.Value);
		Assert.True(result.Equals(value2));
		Assert.Equal(object.Equals(value2, value), result.Equals(value));
		Assert.Equal(value2, wrapper.Value);
		Assert.True(wrapper.Equals(value2));
		Assert.Equal(object.Equals(value2, value), wrapper.Equals(value));
	}

	private static void Object<T>() where T : unmanaged
	{
		T[] array = IMutableWrapperTests.fixture.CreateMany<T>().ToArray();
		T[] array2 = IMutableWrapperTests.fixture.CreateMany<T>().ToArray();
		IMutableWrapper<T[]> result = IMutableWrapper.CreateObject(array);
		IWrapper<T[]> wrapper = result;

		Assert.NotNull(result);
		Assert.Equal(array, result.Value);
		Assert.True(result.Equals(array));
		Assert.Equal(object.Equals(array, array2), result.Equals(array2));
		Assert.NotNull(wrapper);
		Assert.Equal(array, wrapper.Value);
		Assert.True(wrapper.Equals(array));
		Assert.Equal(object.Equals(array, array2), wrapper.Equals(array2));

		result.Value = array2;
		Assert.Equal(array2, result.Value);
		Assert.True(result.Equals(array2));
		Assert.Equal(object.Equals(array2, array), result.Equals(array));
		Assert.Equal(array2, wrapper.Value);
		Assert.True(wrapper.Equals(array2));
		Assert.Equal(object.Equals(array2, array), wrapper.Equals(array));
	}
}