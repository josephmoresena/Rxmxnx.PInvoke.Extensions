namespace Rxmxnx.PInvoke.Tests;

[TestFixture]
[ExcludeFromCodeCoverage]
[SuppressMessage("csharpsquid", "S2699")]
public sealed class MutableReferenceTests
{
	private static readonly IFixture fixture = new Fixture();

	[Fact]
	public Task BooleanTestAsync() => MutableReferenceTests.TestAsync<Boolean>();
	[Fact]
	public Task ByteTestAsync() => MutableReferenceTests.TestAsync<Byte>();
	[Fact]
	public Task Int16TestAsync() => MutableReferenceTests.TestAsync<Int16>();
	[Fact]
	public Task CharTestAsync() => MutableReferenceTests.TestAsync<Char>();
	[Fact]
	public Task Int32TestAsync() => MutableReferenceTests.TestAsync<Int32>();
	[Fact]
	public Task Int64TestAsync() => MutableReferenceTests.TestAsync<Int64>();
#if NET7_0_OR_GREATER
	[Fact]
	internal Task Int128TestAsync() => MutableReferenceTests.TestAsync<Int128>();
#endif
	[Fact]
	public Task GuidTestAsync() => MutableReferenceTests.TestAsync<Guid>();
	[Fact]
	public Task SingleTestAsync() => MutableReferenceTests.TestAsync<Single>();
#if NET5_0_OR_GREATER
	[Fact]
	internal Task HalfTestAsync() => MutableReferenceTests.TestAsync<Half>();
#endif
	[Fact]
	public Task DoubleTestAsync() => MutableReferenceTests.TestAsync<Double>();
	[Fact]
	public Task DecimalTestAsync() => MutableReferenceTests.TestAsync<Decimal>();
	[Fact]
	public Task DateTimeTestAsync() => MutableReferenceTests.TestAsync<DateTime>();
#if NET6_0_OR_GREATER
	[Fact]
	internal Task TimeOnlyTestAsync() => MutableReferenceTests.TestAsync<TimeOnly>();
#endif
	[Fact]
	public Task TimeSpanTestAsync() => MutableReferenceTests.TestAsync<TimeSpan>();

	private static async Task TestAsync<T>() where T : unmanaged
	{
		Task inputTest = Task.Run(MutableReferenceTests.Value<T>);
		Task nullTest1 = Task.Run(() => MutableReferenceTests.Nullable<T>(false));
		Task nullTest2 = Task.Run(() => MutableReferenceTests.Nullable<T>(true));
		Task objectTest = Task.Run(MutableReferenceTests.ObjectTest<T>);

		await Task.WhenAll(inputTest, nullTest1, nullTest2, objectTest);
	}
	private static void Value<T>() where T : unmanaged
	{
		T value = MutableReferenceTests.fixture.Create<T>();
		T value2 = MutableReferenceTests.fixture.Create<T>();
		T value3 = MutableReferenceTests.fixture.Create<T>();
		IMutableReference<T> result = IMutableReference.Create(value);
		IReferenceableWrapper<T> result2 = IReferenceableWrapper.Create(value);
		ReferenceableWrapper<T> result3 = new(result);
		ref readonly T refValue = ref result.Reference;
		ref T mutableValueRef = ref result.Reference;
		PInvokeAssert.NotNull(result);
		PInvokeAssert.Equal(value, result.Value);
		PInvokeAssert.Equal(value, refValue);
		PInvokeAssert.True(result.Equals(value));
		PInvokeAssert.Equal(Object.Equals(value, value2), result.Equals(value2));
		PInvokeAssert.True(Unsafe.AreSame(ref result.Reference, ref mutableValueRef));
		PInvokeAssert.False(Unsafe.AreSame(ref result.Reference, ref value));
		PInvokeAssert.False(result.Equals(result2));
		PInvokeAssert.True(result.Equals(result3));
		PInvokeAssert.True((result as IReadOnlyReferenceable<T>).Equals(result3));

		result.Value = value2;
		PInvokeAssert.Equal(value2, result.Value);
		PInvokeAssert.Equal(value2, refValue);
		PInvokeAssert.True(result.Equals(value2));
		PInvokeAssert.Equal(Object.Equals(value2, value), result.Equals(value));
		PInvokeAssert.True(Unsafe.AreSame(ref result.Reference, ref mutableValueRef));
		PInvokeAssert.False(Unsafe.AreSame(ref result.Reference, ref value2));
		PInvokeAssert.False(result.Equals(result2));
		PInvokeAssert.True(result.Equals(result3));
		PInvokeAssert.True((result as IReadOnlyReferenceable<T>).Equals(result3));

		result.Reference = value3;
		PInvokeAssert.Equal(value3, result.Value);
		PInvokeAssert.Equal(value3, refValue);
	}
	private static void Nullable<T>(Boolean nullInput) where T : unmanaged
	{
		T? value = !nullInput ? MutableReferenceTests.fixture.Create<T>() : null;
		T? value2 = MutableReferenceTests.fixture.Create<Boolean>() ? MutableReferenceTests.fixture.Create<T>() : null;
		T? value3 = MutableReferenceTests.fixture.Create<Boolean>() ? MutableReferenceTests.fixture.Create<T>() : null;
		IMutableReference<T?> result = IMutableReference.CreateNullable(value);
		IReferenceableWrapper<T?> result2 = IReferenceableWrapper.CreateNullable(value);
		ReferenceableWrapper<T?> result3 = new(result);
		ref readonly T? refValue = ref result.Reference;
		ref T? mutableValueRef = ref result.Reference;
		PInvokeAssert.NotNull(result);
		PInvokeAssert.Equal(value, refValue);
		PInvokeAssert.Equal(value, result.Value);
		PInvokeAssert.Equal(Object.Equals(value, value2), result.Equals(value2));
		PInvokeAssert.True(Unsafe.AreSame(ref result.Reference, ref mutableValueRef));
		PInvokeAssert.False(Unsafe.AreSame(ref result.Reference, ref value));
		PInvokeAssert.False(result.Equals(result2));
		PInvokeAssert.True(result.Equals(result3));

		result.Value = value2;
		PInvokeAssert.Equal(value2, result.Value);
		PInvokeAssert.Equal(value2, refValue);
		PInvokeAssert.True(result.Equals(value2));
		PInvokeAssert.Equal(Object.Equals(value2, value), result.Equals(value));
		PInvokeAssert.True(Unsafe.AreSame(ref result.Reference, ref mutableValueRef));
		PInvokeAssert.False(Unsafe.AreSame(ref result.Reference, ref value2));
		PInvokeAssert.False(result.Equals(result2));
		PInvokeAssert.True(result.Equals(result3));

		result.Reference = value3;
		PInvokeAssert.Equal(value3, result.Value);
		PInvokeAssert.Equal(value3, refValue);
	}
	private static void ObjectTest<T>() where T : unmanaged
	{
		T[] array = MutableReferenceTests.fixture.CreateMany<T>().ToArray();
		T[] array2 = MutableReferenceTests.fixture.CreateMany<T>().ToArray();
		T[] array3 = MutableReferenceTests.fixture.CreateMany<T>().ToArray();
		IMutableReference<T[]> result = IMutableReference.CreateObject(array);
		IReferenceableWrapper<T[]> result2 = IReferenceableWrapper.CreateObject(array);
		ReferenceableWrapper<T[]> result3 = new(result);
		ref readonly T[] refValue = ref result.Reference;
		ref T[] mutableValueRef = ref result.Reference;
		PInvokeAssert.NotNull(result);
		PInvokeAssert.Equal(array, result.Value);
		PInvokeAssert.Equal(array, refValue);
		PInvokeAssert.True(result.Equals(array));
		PInvokeAssert.Equal(Object.Equals(array, array2), result.Equals(array2));
		PInvokeAssert.True(Unsafe.AreSame(ref result.Reference, ref mutableValueRef));
		PInvokeAssert.False(Unsafe.AreSame(ref result.Reference, ref array));
		PInvokeAssert.False(result.Equals(result2));
		PInvokeAssert.True(result.Equals(result3));
		PInvokeAssert.True((result as IReadOnlyReferenceable<T[]>).Equals(result3));

		result.Value = array2;
		PInvokeAssert.Equal(array2, result.Value);
		PInvokeAssert.Equal(array2, refValue);
		PInvokeAssert.True(result.Equals(array2));
		PInvokeAssert.Equal(Object.Equals(array2, array), result.Equals(array));
		PInvokeAssert.True(Unsafe.AreSame(ref result.Reference, ref mutableValueRef));
		PInvokeAssert.False(Unsafe.AreSame(ref result.Reference, ref array2));
		PInvokeAssert.False(result.Equals(result2));
		PInvokeAssert.True(result.Equals(result3));
		PInvokeAssert.True((result as IReadOnlyReferenceable<T[]>).Equals(result3));

		result.Reference = array3;
		PInvokeAssert.Equal(array3, result.Value);
		PInvokeAssert.Equal(array3, refValue);
	}
}