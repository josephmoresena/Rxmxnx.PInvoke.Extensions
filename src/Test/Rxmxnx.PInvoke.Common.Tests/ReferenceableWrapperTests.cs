namespace Rxmxnx.PInvoke.Tests;

[ExcludeFromCodeCoverage]
[SuppressMessage("csharpsquid", "S2699")]
public sealed class ReferenceableWrapperTests
{
	private static readonly IFixture fixture = new Fixture();

	[Fact]
	internal Task BooleanTestAsync() => ReferenceableWrapperTests.TestAsync<Boolean>();
	[Fact]
	internal Task ByteTestAsync() => ReferenceableWrapperTests.TestAsync<Byte>();
	[Fact]
	internal Task Int16TestAsync() => ReferenceableWrapperTests.TestAsync<Int16>();
	[Fact]
	internal Task CharTestAsync() => ReferenceableWrapperTests.TestAsync<Char>();
	[Fact]
	internal Task Int32TestAsync() => ReferenceableWrapperTests.TestAsync<Int32>();
	[Fact]
	internal Task Int64TestAsync() => ReferenceableWrapperTests.TestAsync<Int64>();
	[Fact]
	internal Task Int128TestAsync() => ReferenceableWrapperTests.TestAsync<Int128>();
	[Fact]
	internal Task GuidTestAsync() => ReferenceableWrapperTests.TestAsync<Guid>();
	[Fact]
	internal Task SingleTestAsync() => ReferenceableWrapperTests.TestAsync<Single>();
	[Fact]
	internal Task HalfTestAsync() => ReferenceableWrapperTests.TestAsync<Half>();
	[Fact]
	internal Task DoubleTestAsync() => ReferenceableWrapperTests.TestAsync<Double>();
	[Fact]
	internal Task DecimalTestAsync() => ReferenceableWrapperTests.TestAsync<Decimal>();
	[Fact]
	internal Task DateTimeTestAsync() => ReferenceableWrapperTests.TestAsync<DateTime>();
	[Fact]
	internal Task TimeOnlyTestAsync() => ReferenceableWrapperTests.TestAsync<TimeOnly>();
	[Fact]
	internal Task TimeSpanTestAsync() => ReferenceableWrapperTests.TestAsync<TimeSpan>();

	private static async Task TestAsync<T>() where T : unmanaged
	{
		Task inputTest = Task.Run(ReferenceableWrapperTests.Value<T>);
		Task nullTest1 = Task.Run(() => ReferenceableWrapperTests.Nullable<T>(false));
		Task nullTest2 = Task.Run(() => ReferenceableWrapperTests.Nullable<T>(true));
		Task objectTest = Task.Run(ReferenceableWrapperTests.ObjectTest<T>);

		await Task.WhenAll(inputTest, nullTest1, nullTest2, objectTest);
	}
	private static void Value<T>() where T : unmanaged
	{
		T value = ReferenceableWrapperTests.fixture.Create<T>();
		T value2 = ReferenceableWrapperTests.fixture.Create<T>();
		IReferenceableWrapper<T>? result = IReferenceableWrapper.Create(value);
		IReferenceableWrapper<T> result2 = IReferenceableWrapper.Create(value);
		ReferenceableWrapper<T> result3 = new(result);
		ref readonly T refValue = ref result.Reference;
		ref T mutableValueRef = ref Unsafe.AsRef(in result.Reference);
		Assert.NotNull(result);
		Assert.Equal(value, result.Value);
		Assert.Equal(value, refValue);
		Assert.True(result.Equals(value));
		Assert.Equal(Object.Equals(value, value2), result.Equals(value2));
		Assert.True(Unsafe.AreSame(in result.Reference, ref mutableValueRef));
		Assert.False(Unsafe.AreSame(in result.Reference, ref value));
		Assert.False(result.Equals(result2));
		Assert.True(result.Equals(result3));
		Assert.False(result.Equals(default(IReferenceable<T>)));
	}
	private static void Nullable<T>(Boolean nullInput) where T : unmanaged
	{
		T? value = !nullInput ? ReferenceableWrapperTests.fixture.Create<T>() : null;
		T? value2 = ReferenceableWrapperTests.fixture.Create<Boolean>() ?
			ReferenceableWrapperTests.fixture.Create<T>() :
			null;
		IReferenceableWrapper<T?>? result = IReferenceableWrapper.CreateNullable(value);
		IReferenceableWrapper<T?> result2 = IReferenceableWrapper.CreateNullable(value);
		ReferenceableWrapper<T?> result3 = new(result);
		ref readonly T? refValue = ref result.Reference;
		ref T? mutableValueRef = ref Unsafe.AsRef(in result.Reference);
		Assert.NotNull(result);
		Assert.Equal(value, refValue);
		Assert.Equal(value, result.Value);
		Assert.Equal(Object.Equals(value, value2), result.Equals(value2));
		Assert.True(Unsafe.AreSame(in result.Reference, ref mutableValueRef));
		Assert.False(Unsafe.AreSame(in result.Reference, ref value));
		Assert.False(result.Equals(result2));
		Assert.True(result.Equals(result3));
		Assert.False(result.Equals(default(IReferenceable<T?>)));
	}
	private static void ObjectTest<T>() where T : unmanaged
	{
		T[] array = ReferenceableWrapperTests.fixture.CreateMany<T>().ToArray();
		T[] array2 = ReferenceableWrapperTests.fixture.CreateMany<T>().ToArray();
		IReferenceableWrapper<T[]>? result = IReferenceableWrapper.CreateObject(array);
		IReferenceableWrapper<T[]> result2 = IReferenceableWrapper.CreateObject(array);
		ReferenceableWrapper<T[]> result3 = new(result);
		ref readonly T[] refValue = ref result.Reference;
		ref T[] mutableValueRef = ref Unsafe.AsRef(in result.Reference);
		Assert.NotNull(result);
		Assert.Equal(array, result.Value);
		Assert.Equal(array, refValue);
		Assert.True(result.Equals(array));
		Assert.Equal(Object.Equals(array, array2), result.Equals(array2));
		Assert.True(Unsafe.AreSame(in result.Reference, ref mutableValueRef));
		Assert.False(Unsafe.AreSame(in result.Reference, ref array));
		Assert.False(result.Equals(result2));
		Assert.True(result.Equals(result3));
		Assert.False(result.Equals(default(IReferenceable<T>)));
	}
}