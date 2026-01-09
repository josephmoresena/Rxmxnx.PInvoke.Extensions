namespace Rxmxnx.PInvoke.Tests.Internal.FixedContextTests;

[TestFixture]
[ExcludeFromCodeCoverage]
[SuppressMessage("csharpsquid", "S2699")]
public sealed class CreateSpanTest : FixedContextTestsBase
{
	[Fact]
	public void BooleanTest() => CreateSpanTest.Test<Boolean>();
	[Fact]
	public void ByteTest() => CreateSpanTest.Test<Byte>();
	[Fact]
	public void Int16Test() => CreateSpanTest.Test<Int16>();
	[Fact]
	public void CharTest() => CreateSpanTest.Test<Char>();
	[Fact]
	public void Int32Test() => CreateSpanTest.Test<Int32>();
	[Fact]
	public void Int64Test() => CreateSpanTest.Test<Int64>();
#if NET7_0_OR_GREATER
	[Fact]
	internal void Int128Test() => CreateSpanTest.Test<Int128>();
#endif
	[Fact]
	public void GuidTest() => CreateSpanTest.Test<Guid>();
	[Fact]
	public void SingleTest() => CreateSpanTest.Test<Single>();
#if NET5_0_OR_GREATER
	[Fact]
	internal void HalfTest() => CreateSpanTest.Test<Half>();
#endif
	[Fact]
	public void DoubleTest() => CreateSpanTest.Test<Double>();
	[Fact]
	public void DecimalTest() => CreateSpanTest.Test<Decimal>();
	[Fact]
	public void DateTimeTest() => CreateSpanTest.Test<DateTime>();
#if NET6_0_OR_GREATER
	[Fact]
	internal void TimeOnlyTest() => CreateSpanTest.Test<TimeOnly>();
#endif
	[Fact]
	public void TimeSpanTest() => CreateSpanTest.Test<TimeSpan>();
	[Fact]
	public void ManagedStructTest() => CreateSpanTest.Test<ManagedStruct>();
	[Fact]
	public void StringTest() => CreateSpanTest.Test<String>();

	private static void Test<T>()
	{
		T[] values = FixedMemoryTestsBase.Fixture.CreateMany<T>().ToArray();
		FixedContextTestsBase.WithFixed(values, CreateSpanTest.Test);
		Exception readOnly =
			PInvokeAssert.Throws<InvalidOperationException>(() => FixedContextTestsBase.WithFixed(
				                                                values, CreateSpanTest.ReadOnlyTest));
		PInvokeAssert.Equal(FixedMemoryTestsBase.ReadOnlyError, readOnly.Message);
	}

	private static void Test<T>(FixedContext<T> ctx, T[] values)
	{
		Span<T> span = ctx.CreateSpan<T>(values.Length);
		PInvokeAssert.Equal(values.Length, span.Length);
		PInvokeAssert.Equal(values.Length, ctx.Count);
		PInvokeAssert.True(Unsafe.AreSame(ref values[0], ref span[0]));
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
		_ = ctx.CreateSpan<T>(values.Length);
	}
}