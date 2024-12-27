namespace Rxmxnx.PInvoke.Tests.Internal.FixedContextTests;

[ExcludeFromCodeCoverage]
[SuppressMessage("csharpsquid", "S2699")]
public sealed class CreateSpanTest : FixedContextTestsBase
{
	[Fact]
	internal void BooleanTest() => CreateSpanTest.Test<Boolean>();
	[Fact]
	internal void ByteTest() => CreateSpanTest.Test<Byte>();
	[Fact]
	internal void Int16Test() => CreateSpanTest.Test<Int16>();
	[Fact]
	internal void CharTest() => CreateSpanTest.Test<Char>();
	[Fact]
	internal void Int32Test() => CreateSpanTest.Test<Int32>();
	[Fact]
	internal void Int64Test() => CreateSpanTest.Test<Int64>();
	[Fact]
	internal void Int128Test() => CreateSpanTest.Test<Int128>();
	[Fact]
	internal void GuidTest() => CreateSpanTest.Test<Guid>();
	[Fact]
	internal void SingleTest() => CreateSpanTest.Test<Single>();
	[Fact]
	internal void HalfTest() => CreateSpanTest.Test<Half>();
	[Fact]
	internal void DoubleTest() => CreateSpanTest.Test<Double>();
	[Fact]
	internal void DecimalTest() => CreateSpanTest.Test<Decimal>();
	[Fact]
	internal void DateTimeTest() => CreateSpanTest.Test<DateTime>();
	[Fact]
	internal void TimeOnlyTest() => CreateSpanTest.Test<TimeOnly>();
	[Fact]
	internal void TimeSpanTest() => CreateSpanTest.Test<TimeSpan>();
	[Fact]
	internal void ManagedStructTest() => CreateSpanTest.Test<ManagedStruct>();
	[Fact]
	internal void StringTest() => CreateSpanTest.Test<String>();

	private static void Test<T>()
	{
		T[] values = FixedMemoryTestsBase.Fixture.CreateMany<T>().ToArray();
		FixedContextTestsBase.WithFixed(values, CreateSpanTest.Test);
		Exception readOnly =
			Assert.Throws<InvalidOperationException>(
				() => FixedContextTestsBase.WithFixed(values, CreateSpanTest.ReadOnlyTest));
		Assert.Equal(FixedMemoryTestsBase.ReadOnlyError, readOnly.Message);
	}

	private static void Test<T>(FixedContext<T> ctx, T[] values)
	{
		Span<T> span = ctx.CreateSpan<T>(values.Length);
		Assert.Equal(values.Length, span.Length);
		Assert.Equal(values.Length, ctx.Count);
		Assert.True(Unsafe.AreSame(ref values[0], ref span[0]));
		Assert.False(ctx.IsFunction);

		Exception functionException = Assert.Throws<InvalidOperationException>(ctx.CreateDelegate<Action>);
		Assert.Equal(FixedMemoryTestsBase.IsNotFunction, functionException.Message);

		ctx.Unload();
		Exception invalid = Assert.Throws<InvalidOperationException>(() => ctx.CreateSpan<T>(values.Length));
		Assert.Equal(FixedMemoryTestsBase.InvalidError, invalid.Message);

		Exception functionException2 = Assert.Throws<InvalidOperationException>(ctx.CreateDelegate<Action>);
		Assert.Equal(FixedMemoryTestsBase.IsNotFunction, functionException2.Message);
	}

	private static void ReadOnlyTest<T>(ReadOnlyFixedContext<T> ctx, T[] values)
	{
		_ = ctx.CreateSpan<T>(values.Length);
	}
}