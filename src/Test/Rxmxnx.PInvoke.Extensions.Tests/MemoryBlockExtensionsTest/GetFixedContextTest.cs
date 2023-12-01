namespace Rxmxnx.PInvoke.Tests.MemoryBlockExtensionsTest;

[ExcludeFromCodeCoverage]
[SuppressMessage("csharpsquid", "S2699")]
public sealed class GetFixedContextTest
{
	private static readonly IFixture fixture = new Fixture();

	[Fact]
	internal void StringTest()
	{
		String value = GetFixedContextTest.fixture.Create<String>();
		GetFixedContextTest.ReadOnlyTest(value.AsMemory());
		GetFixedContextTest.ReadOnlyTransformDisposeTest<Char, Byte>(value.AsMemory().GetFixedContext());
		GetFixedContextTest.ReadOnlyTransformDisposeTest<Char, Int16>(value.AsMemory().GetFixedContext());
		GetFixedContextTest.ReadOnlyTransformDisposeTest<Char, Int32>(value.AsMemory().GetFixedContext());
		GetFixedContextTest.ReadOnlyTransformDisposeTest<Char, Int64>(value.AsMemory().GetFixedContext());
	}
	[Fact]
	internal void ByteTest() => GetFixedContextTest.ArrayTest<Byte>();
	[Fact]
	internal void CharTest() => GetFixedContextTest.ArrayTest<Char>();
	[Fact]
	internal void DateTimeTest() => GetFixedContextTest.ArrayTest<DateTime>();
	[Fact]
	internal void DecimalTest() => GetFixedContextTest.ArrayTest<Decimal>();
	[Fact]
	internal void DoubleTest() => GetFixedContextTest.ArrayTest<Double>();
	[Fact]
	internal void GuidTest() => GetFixedContextTest.ArrayTest<Guid>();
	[Fact]
	internal void HalfTest() => GetFixedContextTest.ArrayTest<Half>();
	[Fact]
	internal void Int16Test() => GetFixedContextTest.ArrayTest<Int16>();
	[Fact]
	internal void Int32Test() => GetFixedContextTest.ArrayTest<Int32>();
	[Fact]
	internal void Int64Test() => GetFixedContextTest.ArrayTest<Int64>();
	[Fact]
	internal void SByteTest() => GetFixedContextTest.ArrayTest<SByte>();
	[Fact]
	internal void SingleTest() => GetFixedContextTest.ArrayTest<Single>();
	[Fact]
	internal void UInt16Test() => GetFixedContextTest.ArrayTest<UInt16>();
	[Fact]
	internal void UInt32Test() => GetFixedContextTest.ArrayTest<UInt32>();
	[Fact]
	internal void UInt64Test() => GetFixedContextTest.ArrayTest<UInt64>();

	private static void ArrayTest<T>() where T : unmanaged
	{
		T[] arr = GetFixedContextTest.fixture.CreateMany<T>(10).ToArray();
		T[] arr2 = GetFixedContextTest.fixture.CreateMany<T>(arr.Length).ToArray();
		GetFixedContextTest.ReadOnlyTest<T>(arr.AsMemory());
		GetFixedContextTest.Test(arr.AsMemory(), arr2);
		Assert.Equal(arr, arr2);

		GetFixedContextTest.TransformDisposeTest<T, Byte>(arr2.AsMemory().GetFixedContext());
		GetFixedContextTest.TransformDisposeTest<T, Int16>(arr2.AsMemory().GetFixedContext());
		GetFixedContextTest.TransformDisposeTest<T, Int32>(arr2.AsMemory().GetFixedContext());
		GetFixedContextTest.TransformDisposeTest<T, Int64>(arr2.AsMemory().GetFixedContext());
	}
	private static unsafe void ReadOnlyTest<T>(ReadOnlyMemory<T> mem) where T : unmanaged
	{
		using IReadOnlyFixedContext<T>.IDisposable ctx = mem.GetFixedContext();
		for (Int32 i = 0; i < mem.Length; i++)
		{
			Assert.True(
				Unsafe.AreSame(ref UnsafeLegacy.AsRef(in mem.Span[i]), ref UnsafeLegacy.AsRef(in ctx.Values[i])));
			Assert.Equal(mem.Span[i], ctx.Bytes.Slice(i * sizeof(T), sizeof(T)).ToValue<T>());
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
		Assert.Equal(
			!MemoryMarshal.TryGetMemoryManager<T, MemoryManager<T>>(mem, out _) &&
			MemoryMarshal.TryGetArray(mem, out _), ctx is IFixedContext<T>);
	}
	private static unsafe void ReadOnlyTransformTest<T, TDestination>(IReadOnlyFixedContext<T> ctx)
		where T : unmanaged where TDestination : unmanaged
	{
		IReadOnlyFixedContext<TDestination> ctx2 = ctx.Transformation<TDestination>(out IReadOnlyFixedMemory residual);
		Int32 offset = ctx2.Values.Length * sizeof(TDestination);

		Assert.Equal(ctx.Pointer, ctx2.Pointer);
		Assert.Equal(ctx.Bytes.Length / sizeof(TDestination), ctx2.Values.Length);
		Assert.Equal(ctx.Bytes.Length, ctx2.Bytes.Length);
		Assert.Equal(ctx.Bytes.Length - offset, residual.Bytes.Length);
		Assert.Equal(ctx.Pointer + offset, residual.Pointer);
		Assert.Equal(ctx.Bytes.Length - offset, residual.AsBinaryContext().Bytes.Length);
	}
	private static unsafe void Test<T>(Memory<T> mem, T[] arr2) where T : unmanaged
	{
		using IFixedContext<T>.IDisposable ctx = mem.GetFixedContext();
		for (Int32 i = 0; i < mem.Length; i++)
		{
			Assert.True(Unsafe.AreSame(ref mem.Span[i], ref ctx.Values[i]));
			Assert.Equal(mem.Span[i], ctx.Bytes.Slice(i * sizeof(T), sizeof(T)).ToValue<T>());
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

		Assert.Equal(ctx.Pointer, ctx2.Pointer);
		Assert.Equal(ctx.Bytes.Length / sizeof(TDestination), ctx2.Values.Length);
		Assert.Equal(ctx.Bytes.Length, ctx2.Bytes.Length);
		Assert.Equal(ctx.Bytes.Length - offset, residual.Bytes.Length);
		Assert.Equal(ctx.Pointer + offset, residual.Pointer);
		Assert.Equal(ctx.Bytes.Length - offset, residual.AsBinaryContext().Bytes.Length);
	}
	private static void ReadOnlyTransformDisposeTest<T, TDestination>(IReadOnlyFixedContext<T>.IDisposable ctx)
		where T : unmanaged where TDestination : unmanaged
	{
		IReadOnlyFixedContext<TDestination> ctx2 =
			ctx.Transformation<TDestination>(out IReadOnlyFixedMemory.IDisposable residual);
		residual.Dispose();
		Assert.Throws<InvalidOperationException>(() => ctx.Values.Length);
		Assert.Throws<InvalidOperationException>(() => ctx2.Values.Length);
		Assert.Throws<InvalidOperationException>(() => residual.Bytes.Length);
		Assert.Throws<InvalidOperationException>(() => residual.AsBinaryContext().Bytes.Length);
	}
	private static void TransformDisposeTest<T, TDestination>(IFixedContext<T>.IDisposable ctx)
		where T : unmanaged where TDestination : unmanaged
	{
		IFixedContext<TDestination> ctx2 =
			ctx.Transformation<TDestination>(out IReadOnlyFixedMemory.IDisposable residualR);
		_ = ctx.Transformation<TDestination>(out IFixedMemory.IDisposable residual);

		Assert.Equal(residualR.Pointer, residual.Pointer);
		Assert.True(residualR.Bytes.SequenceEqual(residual.Bytes));
		residualR.Dispose();

		Assert.Throws<InvalidOperationException>(() => ctx.Values.Length);
		Assert.Throws<InvalidOperationException>(() => ctx2.Values.Length);
		Assert.Throws<InvalidOperationException>(() => residual.Bytes.Length);
		Assert.Throws<InvalidOperationException>(() => residual.AsBinaryContext().Bytes.Length);

		residual.Dispose();
	}
}