namespace Rxmxnx.PInvoke.Tests;

[ExcludeFromCodeCoverage]
[SuppressMessage("csharpsquid", "S2699")]
public sealed class IReferenceableWrapperTests
{
	private static readonly IFixture fixture = new Fixture();

	[Fact]
	internal Task BooleanTestAsync() => IReferenceableWrapperTests.TestAsync<Boolean>();
	[Fact]
	internal Task ByteTestAsync() => IReferenceableWrapperTests.TestAsync<Byte>();
	[Fact]
	internal Task Int16TestAsync() => IReferenceableWrapperTests.TestAsync<Int16>();
	[Fact]
	internal Task CharTestAsync() => IReferenceableWrapperTests.TestAsync<Char>();
	[Fact]
	internal Task Int32TestAsync() => IReferenceableWrapperTests.TestAsync<Int32>();
	[Fact]
	internal Task Int64TestAsync() => IReferenceableWrapperTests.TestAsync<Int64>();
	[Fact]
	internal Task Int128TestAsync() => IReferenceableWrapperTests.TestAsync<Int128>();
	[Fact]
	internal Task GuidTestAsync() => IReferenceableWrapperTests.TestAsync<Guid>();
	[Fact]
	internal Task SingleTestAsync() => IReferenceableWrapperTests.TestAsync<Single>();
	[Fact]
	internal Task HalfTestAsync() => IReferenceableWrapperTests.TestAsync<Half>();
	[Fact]
	internal Task DoubleTestAsync() => IReferenceableWrapperTests.TestAsync<Double>();
	[Fact]
	internal Task DecimalTestAsync() => IReferenceableWrapperTests.TestAsync<Decimal>();
	[Fact]
	internal Task DateTimeTestAsync() => IReferenceableWrapperTests.TestAsync<DateTime>();
	[Fact]
	internal Task TimeOnlyTestAsync() => IReferenceableWrapperTests.TestAsync<TimeOnly>();
	[Fact]
	internal Task TimeSpanTestAsync() => IReferenceableWrapperTests.TestAsync<TimeSpan>();

	private static async Task TestAsync<T>() where T : unmanaged
	{
		Task inputTest = Task.Run(IReferenceableWrapperTests.Value<T>);
		Task nullTest1 = Task.Run(() => IReferenceableWrapperTests.Nullable<T>(false));
		Task nullTest2 = Task.Run(() => IReferenceableWrapperTests.Nullable<T>(true));
		Task objectTest = Task.Run(IReferenceableWrapperTests.Object<T>);

		await Task.WhenAll(inputTest, nullTest1, nullTest2, objectTest);
	}
	private static void Value<T>() where T : unmanaged
	{
		T value = IReferenceableWrapperTests.fixture.Create<T>();
		T value2 = IReferenceableWrapperTests.fixture.Create<T>();
		IReferenceableWrapper<T>? result = IReferenceableWrapper.Create(value);
		IReferenceableWrapper<T> result2 = IReferenceableWrapper.Create(value);
		ReferenceableWrapper<T> result3 = new(result);
		ref readonly T refValue = ref result.Reference;
		ref T mutableValueRef = ref Unsafe.AsRef(result.Reference);
		Assert.NotNull(result);
		Assert.Equal(value, result.Value);
		Assert.Equal(value, refValue);
		Assert.True(result.Equals(value));
		Assert.Equal(Object.Equals(value, value2), result.Equals(value2));
		Assert.True(Unsafe.AreSame(ref Unsafe.AsRef(result.Reference), ref mutableValueRef));
		Assert.False(Unsafe.AreSame(ref Unsafe.AsRef(result.Reference), ref value));
		Assert.False(result.Equals(result2));
		Assert.True(result.Equals(result3));
		Assert.False(result.Equals(default(IReferenceable<T>)));
	}
	private static void Nullable<T>(Boolean nullInput) where T : unmanaged
	{
		T? value = !nullInput ? IReferenceableWrapperTests.fixture.Create<T>() : null;
		T? value2 = IReferenceableWrapperTests.fixture.Create<Boolean>() ?
			IReferenceableWrapperTests.fixture.Create<T>() :
			null;
		IReferenceableWrapper<T?>? result = IReferenceableWrapper.CreateNullable(value);
		IReferenceableWrapper<T?> result2 = IReferenceableWrapper.CreateNullable(value);
		ReferenceableWrapper<T?> result3 = new(result);
		ref readonly T? refValue = ref result.Reference;
		ref T? mutableValueRef = ref Unsafe.AsRef(result.Reference);
		Assert.NotNull(result);
		Assert.Equal(value, refValue);
		Assert.Equal(value, result.Value);
		Assert.Equal(Object.Equals(value, value2), result.Equals(value2));
		Assert.True(Unsafe.AreSame(ref Unsafe.AsRef(result.Reference), ref mutableValueRef));
		Assert.False(Unsafe.AreSame(ref Unsafe.AsRef(result.Reference), ref value));
		Assert.False(result.Equals(result2));
		Assert.True(result.Equals(result3));
		Assert.False(result.Equals(default(IReferenceable<T?>)));
	}
	private static void Object<T>() where T : unmanaged
	{
		T[] array = IReferenceableWrapperTests.fixture.CreateMany<T>().ToArray();
		T[] array2 = IReferenceableWrapperTests.fixture.CreateMany<T>().ToArray();
		IReferenceableWrapper<T[]>? result = IReferenceableWrapper.CreateObject(array);
		IReferenceableWrapper<T[]> result2 = IReferenceableWrapper.CreateObject(array);
		ReferenceableWrapper<T[]> result3 = new(result);
		ref readonly T[] refValue = ref result.Reference;
		ref T[] mutableValueRef = ref Unsafe.AsRef(result.Reference);
		Assert.NotNull(result);
		Assert.Equal(array, result.Value);
		Assert.Equal(array, refValue);
		Assert.True(result.Equals(array));
		Assert.Equal(Object.Equals(array, array2), result.Equals(array2));
		Assert.True(Unsafe.AreSame(ref Unsafe.AsRef(result.Reference), ref mutableValueRef));
		Assert.False(Unsafe.AreSame(ref Unsafe.AsRef(result.Reference), ref array));
		Assert.False(result.Equals(result2));
		Assert.True(result.Equals(result3));
		Assert.False(result.Equals(default(IReferenceable<T>)));
	}
}