namespace Rxmxnx.PInvoke.Tests;

[TestFixture]
[ExcludeFromCodeCoverage]
[SuppressMessage("csharpsquid", "S2699")]
public sealed class WrapperTests
{
	private static readonly IFixture fixture = new Fixture();

	[Fact]
	public Task BooleanTestAsync() => WrapperTests.TestAsync<Boolean>();
	[Fact]
	public Task ByteTestAsync() => WrapperTests.TestAsync<Byte>();
	[Fact]
	public Task Int16TestAsync() => WrapperTests.TestAsync<Int16>();
	[Fact]
	public Task CharTestAsync() => WrapperTests.TestAsync<Char>();
	[Fact]
	public Task Int32TestAsync() => WrapperTests.TestAsync<Int32>();
	[Fact]
	public Task Int64TestAsync() => WrapperTests.TestAsync<Int64>();
#if NET7_0_OR_GREATER
	[Fact]
	internal Task Int128TestAsync() => WrapperTests.TestAsync<Int128>();
#endif
	[Fact]
	public Task GuidTestAsync() => WrapperTests.TestAsync<Guid>();
	[Fact]
	public Task SingleTestAsync() => WrapperTests.TestAsync<Single>();
#if NET5_0_OR_GREATER
	[Fact]
	internal Task HalfTestAsync() => WrapperTests.TestAsync<Half>();
#endif
	[Fact]
	public Task DoubleTestAsync() => WrapperTests.TestAsync<Double>();
	[Fact]
	public Task DecimalTestAsync() => WrapperTests.TestAsync<Decimal>();
	[Fact]
	public Task DateTimeTestAsync() => WrapperTests.TestAsync<DateTime>();
#if NET6_0_OR_GREATER
	[Fact]
	internal Task TimeOnlyTestAsync() => WrapperTests.TestAsync<TimeOnly>();
#endif
	[Fact]
	public Task TimeSpanTestAsync() => WrapperTests.TestAsync<TimeSpan>();

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
#if NETSTANDARD2_1 && !LEGACY || NETCOREAPP3_0_OR_GREATER
		IWrapper<T> result = IWrapper.Create(value);
#else
		IWrapper<T> result = WrapperFactory.Create(value);
#endif
		Boolean isEquatable = typeof(IWrapper<T>).GetInterfaces().Any(i => i == typeof(IEquatable<T>));
		PInvokeAssert.NotNull(result);
		PInvokeAssert.Equal(value, result.Value);
		PInvokeAssert.Equal(isEquatable, result.Equals(value));
		if (isEquatable)
			PInvokeAssert.Equal(Object.Equals(value, value2), result.Equals(value2));
	}
	private static void Nullable<T>(Boolean nullInput) where T : unmanaged
	{
		T? value = !nullInput ? WrapperTests.fixture.Create<T>() : null;
		T? value2 = WrapperTests.fixture.Create<Boolean>() ? WrapperTests.fixture.Create<T>() : null;
#if (NETSTANDARD2_1 || NETCOREAPP3_0_OR_GREATER) && !LEGACY
		IWrapper<T?> result = IWrapper.CreateNullable(value);
#else
		IWrapper<T?> result = WrapperFactory.CreateNullable(value);
#endif
		Boolean isEquatable = typeof(IWrapper<T[]>).GetInterfaces().Any(i => i == typeof(IEquatable<T[]>));
		PInvokeAssert.NotNull(result);
		PInvokeAssert.Equal(value, result.Value);
		if (isEquatable)
			PInvokeAssert.Equal(Object.Equals(value, value2), result.Equals(value2));
	}
	private static void ObjectTest<T>() where T : unmanaged
	{
		T[] array = WrapperTests.fixture.CreateMany<T>().ToArray();
		T[] array2 = WrapperTests.fixture.CreateMany<T>().ToArray();
#if NETSTANDARD2_1 && !LEGACY || NETCOREAPP3_0_OR_GREATER
		IWrapper<T[]> result = IWrapper.CreateObject(array);
#else
		IWrapper<T[]> result = WrapperFactory.CreateObject(array);
#endif
		Boolean isEquatable = typeof(IWrapper<T[]>).GetInterfaces().Any(i => i == typeof(IEquatable<T[]>));
		PInvokeAssert.NotNull(result);
		PInvokeAssert.Equal(array, result.Value);
		PInvokeAssert.Equal(isEquatable, result.Equals(array));
		if (isEquatable)
			PInvokeAssert.Equal(Object.Equals(array, array2), result.Equals(array2));
	}
}