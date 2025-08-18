#if !NETCOREAPP
using Fact = NUnit.Framework.TestAttribute;
#endif

namespace Rxmxnx.PInvoke.Tests;

[TestFixture]
[ExcludeFromCodeCoverage]
[SuppressMessage("csharpsquid", "S2699")]
public sealed class ReferenceableWrapperTests
{
	private static readonly IFixture fixture = new Fixture();

	[Fact]
	public Task BooleanTestAsync() => ReferenceableWrapperTests.TestAsync<Boolean>();
	[Fact]
	public Task ByteTestAsync() => ReferenceableWrapperTests.TestAsync<Byte>();
	[Fact]
	public Task Int16TestAsync() => ReferenceableWrapperTests.TestAsync<Int16>();
	[Fact]
	public Task CharTestAsync() => ReferenceableWrapperTests.TestAsync<Char>();
	[Fact]
	public Task Int32TestAsync() => ReferenceableWrapperTests.TestAsync<Int32>();
	[Fact]
	public Task Int64TestAsync() => ReferenceableWrapperTests.TestAsync<Int64>();
#if NET7_0_OR_GREATER
	[Fact]
	internal Task Int128TestAsync() => ReferenceableWrapperTests.TestAsync<Int128>();
#endif
	[Fact]
	public Task GuidTestAsync() => ReferenceableWrapperTests.TestAsync<Guid>();
	[Fact]
	public Task SingleTestAsync() => ReferenceableWrapperTests.TestAsync<Single>();
#if NET5_0_OR_GREATER
	[Fact]
	internal Task HalfTestAsync() => ReferenceableWrapperTests.TestAsync<Half>();
#endif
	[Fact]
	public Task DoubleTestAsync() => ReferenceableWrapperTests.TestAsync<Double>();
	[Fact]
	public Task DecimalTestAsync() => ReferenceableWrapperTests.TestAsync<Decimal>();
	[Fact]
	public Task DateTimeTestAsync() => ReferenceableWrapperTests.TestAsync<DateTime>();
#if NET6_0_OR_GREATER
	[Fact]
	internal Task TimeOnlyTestAsync() => ReferenceableWrapperTests.TestAsync<TimeOnly>();
#endif
	[Fact]
	public Task TimeSpanTestAsync() => ReferenceableWrapperTests.TestAsync<TimeSpan>();

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
		PInvokeAssert.NotNull(result);
		PInvokeAssert.Equal(value, result.Value);
		PInvokeAssert.Equal(value, refValue);
		PInvokeAssert.True(result.Equals(value));
		PInvokeAssert.Equal(Object.Equals(value, value2), result.Equals(value2));
#if NET8_0_OR_GREATER
		Assert.True(Unsafe.AreSame(in result.Reference, ref mutableValueRef));
		Assert.False(Unsafe.AreSame(in result.Reference, ref value));
#else
		PInvokeAssert.True(Unsafe.AreSame(ref Unsafe.AsRef(in result.Reference), ref mutableValueRef));
		PInvokeAssert.False(Unsafe.AreSame(ref Unsafe.AsRef(in result.Reference), ref value));
#endif
		PInvokeAssert.False(result.Equals(result2));
		PInvokeAssert.True(result.Equals(result3));
		PInvokeAssert.False(result.Equals(default(IReferenceable<T>)!));
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
		PInvokeAssert.NotNull(result);
		PInvokeAssert.Equal(value, refValue);
		PInvokeAssert.Equal(value, result.Value);
		PInvokeAssert.Equal(Object.Equals(value, value2), result.Equals(value2));
#if NET8_0_OR_GREATER
		Assert.True(Unsafe.AreSame(in result.Reference, ref mutableValueRef));
		Assert.False(Unsafe.AreSame(in result.Reference, ref value));
#else
		PInvokeAssert.True(Unsafe.AreSame(ref Unsafe.AsRef(in result.Reference), ref mutableValueRef));
		PInvokeAssert.False(Unsafe.AreSame(ref Unsafe.AsRef(in result.Reference), ref value));
#endif
		PInvokeAssert.False(result.Equals(result2));
		PInvokeAssert.True(result.Equals(result3));
		PInvokeAssert.False(result.Equals(default(IReferenceable<T>)));
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
		PInvokeAssert.NotNull(result);
		PInvokeAssert.Equal(array, result.Value);
		PInvokeAssert.Equal(array, refValue);
		PInvokeAssert.True(result.Equals(array));
		PInvokeAssert.Equal(Object.Equals(array, array2), result.Equals(array2));
#if NET8_0_OR_GREATER
		Assert.True(Unsafe.AreSame(in result.Reference, ref mutableValueRef));
		Assert.False(Unsafe.AreSame(in result.Reference, ref array));
#else
		PInvokeAssert.True(Unsafe.AreSame(ref Unsafe.AsRef(in result.Reference), ref mutableValueRef));
		PInvokeAssert.False(Unsafe.AreSame(ref Unsafe.AsRef(in result.Reference), ref array));
#endif
		PInvokeAssert.False(result.Equals(result2));
		PInvokeAssert.True(result.Equals(result3));
		PInvokeAssert.False(result.Equals(default(IReferenceable<T>)));
	}
}