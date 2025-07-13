namespace Rxmxnx.PInvoke.Tests;

[ExcludeFromCodeCoverage]
[SuppressMessage("csharpsquid", "S2699")]
public sealed class MutableWrapperTests
{
	private static readonly IFixture fixture = new Fixture();

	[Fact]
	internal void InterfaceTest()
	{
		IMutableWrapper<String> instance = new MutableInstance<String>();
		instance.Value = MutableWrapperTests.fixture.Create<String>();
		Assert.Equal(instance.Value, (instance as IWrapper<String>).Value);
	}

	[Fact]
	internal Task BooleanTestAsync() => MutableWrapperTests.TestAsync<Boolean>();
	[Fact]
	internal Task ByteTestAsync() => MutableWrapperTests.TestAsync<Byte>();
	[Fact]
	internal Task Int16TestAsync() => MutableWrapperTests.TestAsync<Int16>();
	[Fact]
	internal Task CharTestAsync() => MutableWrapperTests.TestAsync<Char>();
	[Fact]
	internal Task Int32TestAsync() => MutableWrapperTests.TestAsync<Int32>();
	[Fact]
	internal Task Int64TestAsync() => MutableWrapperTests.TestAsync<Int64>();
#if NET7_0_OR_GREATER
	[Fact]
	internal Task Int128TestAsync() => MutableWrapperTests.TestAsync<Int128>();
#endif
	[Fact]
	internal Task GuidTestAsync() => MutableWrapperTests.TestAsync<Guid>();
	[Fact]
	internal Task SingleTestAsync() => MutableWrapperTests.TestAsync<Single>();
#if NET5_0_OR_GREATER
	[Fact]
	internal Task HalfTestAsync() => MutableWrapperTests.TestAsync<Half>();
#endif
	[Fact]
	internal Task DoubleTestAsync() => MutableWrapperTests.TestAsync<Double>();
	[Fact]
	internal Task DecimalTestAsync() => MutableWrapperTests.TestAsync<Decimal>();
	[Fact]
	internal Task DateTimeTestAsync() => MutableWrapperTests.TestAsync<DateTime>();
#if NET6_0_OR_GREATER
	[Fact]
	internal Task TimeOnlyTestAsync() => MutableWrapperTests.TestAsync<TimeOnly>();
#endif
	[Fact]
	internal Task TimeSpanTestAsync() => MutableWrapperTests.TestAsync<TimeSpan>();

	private static async Task TestAsync<T>() where T : unmanaged
	{
		Task inputTest = Task.Run(MutableWrapperTests.Value<T>);
		Task nullTest1 = Task.Run(() => MutableWrapperTests.Nullable<T>(false));
		Task nullTest2 = Task.Run(() => MutableWrapperTests.Nullable<T>(true));
		Task objectTest = Task.Run(MutableWrapperTests.ObjectTest<T>);

		await Task.WhenAll(inputTest, nullTest1, nullTest2, objectTest);
	}

	private static void Value<T>() where T : unmanaged
	{
		T value = MutableWrapperTests.fixture.Create<T>();
		T value2 = MutableWrapperTests.fixture.Create<T>();
		IMutableWrapper<T> result = IMutableWrapper.Create(value);
		IWrapper<T> wrapper = result;

		Assert.NotNull(result);
		Assert.Equal(value, result.Value);
		Assert.True(result.Equals(value));
		Assert.Equal(Object.Equals(value, value2), result.Equals(value2));
		Assert.NotNull(wrapper);
		Assert.Equal(value, wrapper.Value);
		Assert.True(wrapper.Equals(value));
		Assert.Equal(Object.Equals(value, value2), wrapper.Equals(value2));

		result.Value = value2;
		Assert.Equal(value2, result.Value);
		Assert.True(result.Equals(value2));
		Assert.Equal(Object.Equals(value2, value), result.Equals(value));
		Assert.Equal(value2, wrapper.Value);
		Assert.True(wrapper.Equals(value2));
		Assert.Equal(Object.Equals(value2, value), wrapper.Equals(value));
	}

	private static void Nullable<T>(Boolean nullInput) where T : unmanaged
	{
		T? value = !nullInput ? MutableWrapperTests.fixture.Create<T>() : null;
		T? value2 = MutableWrapperTests.fixture.Create<Boolean>() ? MutableWrapperTests.fixture.Create<T>() : null;
		IMutableWrapper<T?> result = IMutableWrapper.CreateNullable(value);
		IWrapper<T?> wrapper = result;

		Assert.NotNull(result);
		Assert.Equal(value, result.Value);
		Assert.Equal(Object.Equals(value, value2), result.Equals(value2));
		Assert.NotNull(wrapper);
		Assert.Equal(value, wrapper.Value);
		Assert.True(wrapper.Equals(value));
		Assert.Equal(Object.Equals(value, value2), wrapper.Equals(value2));

		result.Value = value2;
		Assert.Equal(value2, result.Value);
		Assert.True(result.Equals(value2));
		Assert.Equal(Object.Equals(value2, value), result.Equals(value));
		Assert.Equal(value2, wrapper.Value);
		Assert.True(wrapper.Equals(value2));
		Assert.Equal(Object.Equals(value2, value), wrapper.Equals(value));
	}

	private static void ObjectTest<T>() where T : unmanaged
	{
		T[] array = MutableWrapperTests.fixture.CreateMany<T>().ToArray();
		T[] array2 = MutableWrapperTests.fixture.CreateMany<T>().ToArray();
		IMutableWrapper<T[]> result = IMutableWrapper.CreateObject(array);
		IWrapper<T[]> wrapper = result;

		Assert.NotNull(result);
		Assert.Equal(array, result.Value);
		Assert.True(result.Equals(array));
		Assert.Equal(Object.Equals(array, array2), result.Equals(array2));
		Assert.NotNull(wrapper);
		Assert.Equal(array, wrapper.Value);
		Assert.True(wrapper.Equals(array));
		Assert.Equal(Object.Equals(array, array2), wrapper.Equals(array2));

		result.Value = array2;
		Assert.Equal(array2, result.Value);
		Assert.True(result.Equals(array2));
		Assert.Equal(Object.Equals(array2, array), result.Equals(array));
		Assert.Equal(array2, wrapper.Value);
		Assert.True(wrapper.Equals(array2));
		Assert.Equal(Object.Equals(array2, array), wrapper.Equals(array));
	}
}