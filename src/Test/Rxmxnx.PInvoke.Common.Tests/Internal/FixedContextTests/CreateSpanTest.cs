namespace Rxmxnx.PInvoke.Tests.Internal.FixedContextTests;

[ExcludeFromCodeCoverage]
[SuppressMessage("csharpsquid", "S2699")]
public sealed class CreateSpanTest : FixedContextTestsBase
{
	[Fact]
	internal void BooleanTest() => this.Test<Boolean>();
	[Fact]
	internal void ByteTest() => this.Test<Byte>();
	[Fact]
	internal void Int16Test() => this.Test<Int16>();
	[Fact]
	internal void CharTest() => this.Test<Char>();
	[Fact]
	internal void Int32Test() => this.Test<Int32>();
	[Fact]
	internal void Int64Test() => this.Test<Int64>();
	[Fact]
	internal void Int128Test() => this.Test<Int128>();
	[Fact]
	internal void GuidTest() => this.Test<Guid>();
	[Fact]
	internal void SingleTest() => this.Test<Single>();
	[Fact]
	internal void HalfTest() => this.Test<Half>();
	[Fact]
	internal void DoubleTest() => this.Test<Double>();
	[Fact]
	internal void DecimalTest() => this.Test<Decimal>();
	[Fact]
	internal void DateTimeTest() => this.Test<DateTime>();
	[Fact]
	internal void TimeOnlyTest() => this.Test<TimeOnly>();
	[Fact]
	internal void TimeSpanTest() => this.Test<TimeSpan>();

	private void Test<T>() where T : unmanaged
	{
		T[] values = FixedMemoryTestsBase.fixture.CreateMany<T>().ToArray();
		this.WithFixed(values, CreateSpanTest.Test);
		Exception readOnly =
			Assert.Throws<InvalidOperationException>(() => this.WithFixed(values, CreateSpanTest.ReadOnlyTest));
		Assert.Equal(FixedMemoryTestsBase.ReadOnlyError, readOnly.Message);
	}

	private static void Test<T>(FixedContext<T> ctx, T[] values) where T : unmanaged
	{
		Span<T> span = ctx.CreateSpan<T>(values.Length);
		Assert.Equal(values.Length, span.Length);
		Assert.Equal(values.Length, ctx.Count);
		Assert.True(Unsafe.AreSame(ref Unsafe.AsRef(values[0]), ref span[0]));
		Assert.False(ctx.IsFunction);

		Exception functionException = Assert.Throws<InvalidOperationException>(() => ctx.CreateDelegate<Action>());
		Assert.Equal(FixedMemoryTestsBase.IsNotFunction, functionException.Message);

		ctx.Unload();
		Exception invalid = Assert.Throws<InvalidOperationException>(() => ctx.CreateSpan<T>(values.Length));
		Assert.Equal(FixedMemoryTestsBase.InvalidError, invalid.Message);

		Exception functionException2 = Assert.Throws<InvalidOperationException>(() => ctx.CreateDelegate<Action>());
		Assert.Equal(FixedMemoryTestsBase.IsNotFunction, functionException2.Message);
	}

	private static void ReadOnlyTest<T>(ReadOnlyFixedContext<T> ctx, T[] values) where T : unmanaged
	{
		_ = ctx.CreateSpan<T>(values.Length);
	}
}