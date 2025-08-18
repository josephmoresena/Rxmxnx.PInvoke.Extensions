#if !NETCOREAPP
using Fact = NUnit.Framework.TestAttribute;
#endif

namespace Rxmxnx.PInvoke.Tests.Internal.FixedContextTests;

[TestFixture]
[ExcludeFromCodeCoverage]
[SuppressMessage("csharpsquid", "S2699")]
public sealed class CreateReadOnlySpanTest : FixedContextTestsBase
{
	[Fact]
	public void BooleanTest() => CreateReadOnlySpanTest.Test<Boolean>();
	[Fact]
	public void ByteTest() => CreateReadOnlySpanTest.Test<Byte>();
	[Fact]
	public void Int16Test() => CreateReadOnlySpanTest.Test<Int16>();
	[Fact]
	public void CharTest() => CreateReadOnlySpanTest.Test<Char>();
	[Fact]
	public void Int32Test() => CreateReadOnlySpanTest.Test<Int32>();
	[Fact]
	public void Int64Test() => CreateReadOnlySpanTest.Test<Int64>();
#if NET7_0_OR_GREATER
	[Fact]
	internal void Int128Test() => CreateReadOnlySpanTest.Test<Int128>();
#endif
	[Fact]
	public void GuidTest() => CreateReadOnlySpanTest.Test<Guid>();
	[Fact]
	public void SingleTest() => CreateReadOnlySpanTest.Test<Single>();
#if NET5_0_OR_GREATER
	[Fact]
	internal void HalfTest() => CreateReadOnlySpanTest.Test<Half>();
#endif
	[Fact]
	public void DoubleTest() => CreateReadOnlySpanTest.Test<Double>();
	[Fact]
	public void DecimalTest() => CreateReadOnlySpanTest.Test<Decimal>();
	[Fact]
	public void DateTimeTest() => CreateReadOnlySpanTest.Test<DateTime>();
#if NET6_0_OR_GREATER
	[Fact]
	internal void TimeOnlyTest() => CreateReadOnlySpanTest.Test<TimeOnly>();
#endif
	[Fact]
	public void TimeSpanTest() => CreateReadOnlySpanTest.Test<TimeSpan>();
	[Fact]
	public void ManagedStructTest() => CreateReadOnlySpanTest.Test<ManagedStruct>();
	[Fact]
	public void StringTest() => CreateReadOnlySpanTest.Test<String>();

	private static void Test<T>()
	{
		T[] values = FixedMemoryTestsBase.Fixture.CreateMany<T>().ToArray();
		FixedContextTestsBase.WithFixed(values, CreateReadOnlySpanTest.Test);
		FixedContextTestsBase.WithFixed(values, CreateReadOnlySpanTest.ReadOnlyTest);
	}

	private static void Test<T>(FixedContext<T> ctx, T[] values)
	{
		ReadOnlySpan<T> span = ctx.CreateReadOnlySpan<T>(values.Length);
		PInvokeAssert.Equal(values.Length, span.Length);
		PInvokeAssert.Equal(values.Length, ctx.Count);
#if NET8_0_OR_GREATER
		Assert.True(Unsafe.AreSame(ref values[0], in span[0]));
#else
		PInvokeAssert.True(Unsafe.AreSame(ref values[0], ref Unsafe.AsRef(in span[0])));
#endif
		PInvokeAssert.False(ctx.IsFunction);

		Exception functionException = PInvokeAssert.Throws<InvalidOperationException>(ctx.CreateDelegate<Action>);
		PInvokeAssert.Equal(FixedMemoryTestsBase.IsNotFunction, functionException.Message);

		ctx.Unload();
		Exception invalid = PInvokeAssert.Throws<InvalidOperationException>(() => ctx.CreateSpan<T>(values.Length));
		PInvokeAssert.Equal(FixedMemoryTestsBase.InvalidError, invalid.Message);

		Exception functionException2 = PInvokeAssert.Throws<InvalidOperationException>(ctx.CreateDelegate<Action>);
		PInvokeAssert.Equal(FixedMemoryTestsBase.IsNotFunction, functionException2.Message);
	}

	private static void ReadOnlyTest<T>(ReadOnlyFixedContext<T> ctx, T[] values)
	{
		ReadOnlySpan<T> span = ctx.CreateReadOnlySpan<T>(values.Length);
		PInvokeAssert.Equal(values.Length, span.Length);
		PInvokeAssert.Equal(values.Length, ctx.Count);
#if NET8_0_OR_GREATER
		Assert.True(Unsafe.AreSame(ref values[0], in span[0]));
#else
		PInvokeAssert.True(Unsafe.AreSame(ref values[0], ref Unsafe.AsRef(in span[0])));
#endif
		PInvokeAssert.False(ctx.IsFunction);

		Exception functionException = PInvokeAssert.Throws<InvalidOperationException>(ctx.CreateDelegate<Action>);
		PInvokeAssert.Equal(FixedMemoryTestsBase.IsNotFunction, functionException.Message);

		ctx.Unload();
		Exception invalid = PInvokeAssert.Throws<InvalidOperationException>(() => ctx.CreateSpan<T>(values.Length));
		PInvokeAssert.Equal(FixedMemoryTestsBase.InvalidError, invalid.Message);

		Exception functionException2 = PInvokeAssert.Throws<InvalidOperationException>(ctx.CreateDelegate<Action>);
		PInvokeAssert.Equal(FixedMemoryTestsBase.IsNotFunction, functionException2.Message);
	}
}