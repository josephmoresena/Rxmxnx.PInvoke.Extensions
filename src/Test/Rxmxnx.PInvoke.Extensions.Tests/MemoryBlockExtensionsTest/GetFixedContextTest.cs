#if !NETCOREAPP
using Fact = NUnit.Framework.TestAttribute;
#endif

namespace Rxmxnx.PInvoke.Tests.MemoryBlockExtensionsTest;

[TestFixture]
[ExcludeFromCodeCoverage]
[SuppressMessage("csharpsquid", "S2699")]
public sealed class GetFixedContextTest
{
	private static readonly IFixture fixture = new Fixture();

	[Fact]
	public void StringTest()
	{
		String value = GetFixedContextTest.fixture.Create<String>();
		GetFixedContextTest.ReadOnlyTest(value.AsMemory());
	}
	[Fact]
	public void ByteTest() => GetFixedContextTest.ArrayTest<Byte>();
	[Fact]
	public void CharTest() => GetFixedContextTest.ArrayTest<Char>();
#if NET7_0_OR_GREATER
	[Fact]
	internal void DateTimeTest() => GetFixedContextTest.ArrayTest<DateTime>();
#endif
	[Fact]
	public void DecimalTest() => GetFixedContextTest.ArrayTest<Decimal>();
	[Fact]
	public void DoubleTest() => GetFixedContextTest.ArrayTest<Double>();
	[Fact]
	public void GuidTest() => GetFixedContextTest.ArrayTest<Guid>();
#if NET5_0_OR_GREATER
	[Fact]
	internal void HalfTest() => GetFixedContextTest.ArrayTest<Half>();
#endif
	[Fact]
	public void Int16Test() => GetFixedContextTest.ArrayTest<Int16>();
	[Fact]
	public void Int32Test() => GetFixedContextTest.ArrayTest<Int32>();
	[Fact]
	public void Int64Test() => GetFixedContextTest.ArrayTest<Int64>();
	[Fact]
	public void SByteTest() => GetFixedContextTest.ArrayTest<SByte>();
	[Fact]
	public void SingleTest() => GetFixedContextTest.ArrayTest<Single>();
	[Fact]
	public void UInt16Test() => GetFixedContextTest.ArrayTest<UInt16>();
	[Fact]
	public void UInt32Test() => GetFixedContextTest.ArrayTest<UInt32>();
	[Fact]
	public void UInt64Test() => GetFixedContextTest.ArrayTest<UInt64>();

	private static void ArrayTest<T>() where T : unmanaged
	{
		T[] arr = GetFixedContextTest.fixture.CreateMany<T>(10).ToArray();
		T[] arr2 = GetFixedContextTest.fixture.CreateMany<T>(arr.Length).ToArray();
		GetFixedContextTest.ReadOnlyTest<T>(arr.AsMemory());
		GetFixedContextTest.Test(arr.AsMemory(), arr2);
		PInvokeAssert.Equal(arr, arr2);
	}
	private static unsafe void ReadOnlyTest<T>(ReadOnlyMemory<T> mem) where T : unmanaged
	{
		using IReadOnlyFixedContext<T>.IDisposable ctx = mem.GetFixedContext();
		for (Int32 i = 0; i < mem.Length; i++)
		{
#if NET8_0_OR_GREATER
			Assert.True(Unsafe.AreSame(in mem.Span[i], in ctx.Values[i]));
#else
			PInvokeAssert.True(Unsafe.AreSame(ref Unsafe.AsRef(in mem.Span[i]), ref Unsafe.AsRef(in ctx.Values[i])));
#endif
			PInvokeAssert.Equal(mem.Span[i], ctx.Bytes.Slice(i * sizeof(T), sizeof(T)).ToValue<T>());
		}
		GetFixedContextTest.ReadOnlyTransformTest<T, Byte>(ctx);
		GetFixedContextTest.ReadOnlyTransformTest<T, Char>(ctx);
		GetFixedContextTest.ReadOnlyTransformTest<T, Int16>(ctx);
		GetFixedContextTest.ReadOnlyTransformTest<T, Int32>(ctx);
		GetFixedContextTest.ReadOnlyTransformTest<T, Int64>(ctx);
		GetFixedContextTest.ReadOnlyTransformTest<T, SByte>(ctx);
		GetFixedContextTest.ReadOnlyTransformTest<T, UInt16>(ctx);
		GetFixedContextTest.ReadOnlyTransformTest<T, UInt32>(ctx);
		GetFixedContextTest.ReadOnlyTransformTest<T, UInt64>(ctx);
		PInvokeAssert.Equal(
			!MemoryMarshal.TryGetMemoryManager<T, MemoryManager<T>>(mem, out _) &&
			MemoryMarshal.TryGetArray(mem, out _), ctx is IFixedContext<T>);
	}
	private static unsafe void ReadOnlyTransformTest<T, TDestination>(IReadOnlyFixedContext<T> ctx)
		where T : unmanaged where TDestination : unmanaged
	{
		IReadOnlyFixedContext<TDestination> ctx2 = ctx.Transformation<TDestination>(out IReadOnlyFixedMemory residual);
		Int32 offset = ctx2.Values.Length * sizeof(TDestination);

		PInvokeAssert.Equal(ctx.Pointer, ctx2.Pointer);
		PInvokeAssert.Equal(ctx.Bytes.Length / sizeof(TDestination), ctx2.Values.Length);
		PInvokeAssert.Equal(ctx.Bytes.Length, ctx2.Bytes.Length);
		PInvokeAssert.Equal(ctx.Bytes.Length - offset, residual.Bytes.Length);
		PInvokeAssert.Equal(ctx.Pointer + offset, residual.Pointer);
		PInvokeAssert.Equal(ctx.Bytes.Length - offset, residual.AsBinaryContext().Bytes.Length);
		PInvokeAssert.Equal(ctx.Bytes.Length - offset == 0, residual.IsNullOrEmpty);
		PInvokeAssert.True(ctx.Objects.IsEmpty);
		PInvokeAssert.True(residual.Objects.IsEmpty);
		PInvokeAssert.Equal(ctx.Bytes.Length == 0, ctx.IsNullOrEmpty);
		PInvokeAssert.Throws<InvalidOperationException>(() => residual.AsObjectContext());
	}
	private static unsafe void Test<T>(Memory<T> mem, T[] arr2) where T : unmanaged
	{
		using IFixedContext<T>.IDisposable ctx = mem.GetFixedContext();
		for (Int32 i = 0; i < mem.Length; i++)
		{
			PInvokeAssert.True(Unsafe.AreSame(ref mem.Span[i], ref ctx.Values[i]));
			PInvokeAssert.Equal(mem.Span[i], ctx.Bytes.Slice(i * sizeof(T), sizeof(T)).ToValue<T>());
			ctx.Values[i] = arr2[i];
		}
		GetFixedContextTest.TransformTest<T, Byte>(ctx);
		GetFixedContextTest.TransformTest<T, Char>(ctx);
		GetFixedContextTest.TransformTest<T, Int16>(ctx);
		GetFixedContextTest.TransformTest<T, Int32>(ctx);
		GetFixedContextTest.TransformTest<T, Int64>(ctx);
		GetFixedContextTest.TransformTest<T, SByte>(ctx);
		GetFixedContextTest.TransformTest<T, UInt16>(ctx);
		GetFixedContextTest.TransformTest<T, UInt32>(ctx);
		GetFixedContextTest.TransformTest<T, UInt64>(ctx);
	}
	private static unsafe void TransformTest<T, TDestination>(IFixedContext<T> ctx)
		where T : unmanaged where TDestination : unmanaged
	{
		IFixedContext<TDestination> ctx2 = ctx.Transformation<TDestination>(out IFixedMemory residual);
		Int32 offset = ctx2.Values.Length * sizeof(TDestination);

		PInvokeAssert.Equal(ctx.Pointer, ctx2.Pointer);
		PInvokeAssert.Equal(ctx.Bytes.Length / sizeof(TDestination), ctx2.Values.Length);
		PInvokeAssert.Equal(ctx.Bytes.Length, ctx2.Bytes.Length);
		PInvokeAssert.Equal(ctx.Bytes.Length - offset, residual.Bytes.Length);
		PInvokeAssert.Equal(ctx.Pointer + offset, residual.Pointer);
		PInvokeAssert.Equal(ctx.Bytes.Length - offset, residual.AsBinaryContext().Bytes.Length);
		PInvokeAssert.Equal(ctx.Bytes.Length - offset == 0, residual.IsNullOrEmpty);
		PInvokeAssert.True(ctx.Objects.IsEmpty);
		PInvokeAssert.True(residual.Objects.IsEmpty);
		PInvokeAssert.Equal(ctx.Bytes.Length == 0, ctx.IsNullOrEmpty);
		PInvokeAssert.Throws<InvalidOperationException>(() => residual.AsObjectContext());
	}
}