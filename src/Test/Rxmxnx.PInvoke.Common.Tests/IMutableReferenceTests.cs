namespace Rxmxnx.PInvoke.Tests;

[ExcludeFromCodeCoverage]
[SuppressMessage("csharpsquid", "S2699")]
public sealed class IMutableReferenceTests
{
	private static readonly IFixture fixture = new Fixture();

	[Fact]
	internal Task BooleanTestAsync() => IMutableReferenceTests.TestAsync<Boolean>();
	[Fact]
	internal Task ByteTestAsync() => IMutableReferenceTests.TestAsync<Byte>();
	[Fact]
	internal Task Int16TestAsync() => IMutableReferenceTests.TestAsync<Int16>();
	[Fact]
	internal Task CharTestAsync() => IMutableReferenceTests.TestAsync<Char>();
	[Fact]
	internal Task Int32TestAsync() => IMutableReferenceTests.TestAsync<Int32>();
	[Fact]
	internal Task Int64TestAsync() => IMutableReferenceTests.TestAsync<Int64>();
	[Fact]
	internal Task Int128TestAsync() => IMutableReferenceTests.TestAsync<Int128>();
	[Fact]
	internal Task GuidTestAsync() => IMutableReferenceTests.TestAsync<Guid>();
	[Fact]
	internal Task SingleTestAsync() => IMutableReferenceTests.TestAsync<Single>();
	[Fact]
	internal Task HalfTestAsync() => IMutableReferenceTests.TestAsync<Half>();
	[Fact]
	internal Task DoubleTestAsync() => IMutableReferenceTests.TestAsync<Double>();
	[Fact]
	internal Task DecimalTestAsync() => IMutableReferenceTests.TestAsync<Decimal>();
	[Fact]
	internal Task DateTimeTestAsync() => IMutableReferenceTests.TestAsync<DateTime>();
	[Fact]
	internal Task TimeOnlyTestAsync() => IMutableReferenceTests.TestAsync<TimeOnly>();
	[Fact]
	internal Task TimeSpanTestAsync() => IMutableReferenceTests.TestAsync<TimeSpan>();

	private static async Task TestAsync<T>() where T : unmanaged
	{
		Task inputTest = Task.Run(IMutableReferenceTests.Value<T>);
		Task nullTest1 = Task.Run(() => IMutableReferenceTests.Nullable<T>(false));
		Task nullTest2 = Task.Run(() => IMutableReferenceTests.Nullable<T>(true));
		Task objectTest = Task.Run(IMutableReferenceTests.ObjectTest<T>);

		await Task.WhenAll(inputTest, nullTest1, nullTest2, objectTest);
	}
	private static void Value<T>() where T : unmanaged
	{
		T value = IMutableReferenceTests.fixture.Create<T>();
		T value2 = IMutableReferenceTests.fixture.Create<T>();
		T value3 = IMutableReferenceTests.fixture.Create<T>();
		IMutableReference<T> result = IMutableReference.Create(value);
		IReferenceableWrapper<T> result2 = IReferenceableWrapper.Create(value);
		ReferenceableWrapper<T> result3 = new(result);
		ref readonly T refValue = ref result.Reference;
		ref T mutableValueRef = ref result.Reference;
		Assert.NotNull(result);
		Assert.Equal(value, result.Value);
		Assert.Equal(value, refValue);
		Assert.True(result.Equals(value));
		Assert.Equal(Object.Equals(value, value2), result.Equals(value2));
		Assert.True(Unsafe.AreSame(ref Unsafe.AsRef(result.Reference), ref mutableValueRef));
		Assert.False(Unsafe.AreSame(ref Unsafe.AsRef(result.Reference), ref value));
		Assert.False(result.Equals(result2));
		Assert.True(result.Equals(result3));
		Assert.True((result as IReadOnlyReferenceable<T>).Equals(result3));

		result.Value = value2;
		Assert.Equal(value2, result.Value);
		Assert.Equal(value2, refValue);
		Assert.True(result.Equals(value2));
		Assert.Equal(Object.Equals(value2, value), result.Equals(value));
		Assert.True(Unsafe.AreSame(ref Unsafe.AsRef(result.Reference), ref mutableValueRef));
		Assert.False(Unsafe.AreSame(ref Unsafe.AsRef(result.Reference), ref value2));
		Assert.False(result.Equals(result2));
		Assert.True(result.Equals(result3));
		Assert.True((result as IReadOnlyReferenceable<T>).Equals(result3));

		result.Reference = value3;
		Assert.Equal(value3, result.Value);
		Assert.Equal(value3, refValue);
	}
	private static void Nullable<T>(Boolean nullInput) where T : unmanaged
	{
		T? value = !nullInput ? IMutableReferenceTests.fixture.Create<T>() : null;
		T? value2 = IMutableReferenceTests.fixture.Create<Boolean>() ?
			IMutableReferenceTests.fixture.Create<T>() :
			null;
		T? value3 = IMutableReferenceTests.fixture.Create<Boolean>() ?
			IMutableReferenceTests.fixture.Create<T>() :
			null;
		IMutableReference<T?> result = IMutableReference.CreateNullable(value);
		IReferenceableWrapper<T?> result2 = IReferenceableWrapper.CreateNullable(value);
		ReferenceableWrapper<T?> result3 = new(result);
		ref readonly T? refValue = ref result.Reference;
		ref T? mutableValueRef = ref result.Reference;
		Assert.NotNull(result);
		Assert.Equal(value, refValue);
		Assert.Equal(value, result.Value);
		Assert.Equal(Object.Equals(value, value2), result.Equals(value2));
		Assert.True(Unsafe.AreSame(ref Unsafe.AsRef(result.Reference), ref mutableValueRef));
		Assert.False(Unsafe.AreSame(ref Unsafe.AsRef(result.Reference), ref value));
		Assert.False(result.Equals(result2));
		Assert.True(result.Equals(result3));

		result.Value = value2;
		Assert.Equal(value2, result.Value);
		Assert.Equal(value2, refValue);
		Assert.True(result.Equals(value2));
		Assert.Equal(Object.Equals(value2, value), result.Equals(value));
		Assert.True(Unsafe.AreSame(ref Unsafe.AsRef(result.Reference), ref mutableValueRef));
		Assert.False(Unsafe.AreSame(ref Unsafe.AsRef(result.Reference), ref value2));
		Assert.False(result.Equals(result2));
		Assert.True(result.Equals(result3));

		result.Reference = value3;
		Assert.Equal(value3, result.Value);
		Assert.Equal(value3, refValue);
	}
	private static void ObjectTest<T>() where T : unmanaged
	{
		T[] array = IMutableReferenceTests.fixture.CreateMany<T>().ToArray();
		T[] array2 = IMutableReferenceTests.fixture.CreateMany<T>().ToArray();
		T[] array3 = IMutableReferenceTests.fixture.CreateMany<T>().ToArray();
		IMutableReference<T[]> result = IMutableReference.CreateObject(array);
		IReferenceableWrapper<T[]> result2 = IReferenceableWrapper.CreateObject(array);
		ReferenceableWrapper<T[]> result3 = new(result);
		ref readonly T[] refValue = ref result.Reference;
		ref T[] mutableValueRef = ref result.Reference;
		Assert.NotNull(result);
		Assert.Equal(array, result.Value);
		Assert.Equal(array, refValue);
		Assert.True(result.Equals(array));
		Assert.Equal(Object.Equals(array, array2), result.Equals(array2));
		Assert.True(Unsafe.AreSame(ref Unsafe.AsRef(result.Reference), ref mutableValueRef));
		Assert.False(Unsafe.AreSame(ref Unsafe.AsRef(result.Reference), ref array));
		Assert.False(result.Equals(result2));
		Assert.True(result.Equals(result3));
		Assert.True((result as IReadOnlyReferenceable<T[]>).Equals(result3));

		result.Value = array2;
		Assert.Equal(array2, result.Value);
		Assert.Equal(array2, refValue);
		Assert.True(result.Equals(array2));
		Assert.Equal(Object.Equals(array2, array), result.Equals(array));
		Assert.True(Unsafe.AreSame(ref Unsafe.AsRef(result.Reference), ref mutableValueRef));
		Assert.False(Unsafe.AreSame(ref Unsafe.AsRef(result.Reference), ref array2));
		Assert.False(result.Equals(result2));
		Assert.True(result.Equals(result3));
		Assert.True((result as IReadOnlyReferenceable<T[]>).Equals(result3));

		result.Reference = array3;
		Assert.Equal(array3, result.Value);
		Assert.Equal(array3, refValue);
	}
}