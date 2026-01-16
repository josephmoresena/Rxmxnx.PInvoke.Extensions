namespace Rxmxnx.PInvoke.Tests.BinaryExtensionsTests;

[TestFixture]
[ExcludeFromCodeCoverage]
[SuppressMessage("csharpsquid", "S2699")]
public sealed class WithSafeFixedTest
{
	private static readonly IFixture fixture = new Fixture();

	private Array? _array;

	[Fact]
	public void ByteTest() => this.Test<Byte>();
	[Fact]
	public void CharTest() => this.Test<Char>();
	[Fact]
	public void DateTimeTest() => this.Test<DateTime>();
	[Fact]
	public void DecimalTest() => this.Test<Decimal>();
	[Fact]
	public void DoubleTest() => this.Test<Double>();
	[Fact]
	public void GuidTest() => this.Test<Guid>();
#if NET5_0_OR_GREATER
	[Fact]
	internal void HalfTest() => this.Test<Half>();
#endif
	[Fact]
	public void Int16Test() => this.Test<Int16>();
	[Fact]
	public void Int32Test() => this.Test<Int32>();
	[Fact]
	public void Int64Test() => this.Test<Int64>();
	[Fact]
	public void SByteTest() => this.Test<SByte>();
	[Fact]
	public void SingleTest() => this.Test<Single>();
	[Fact]
	public void UInt16Test() => this.Test<UInt16>();
	[Fact]
	public void UInt32Test() => this.Test<UInt32>();
	[Fact]
	public void UInt64Test() => this.Test<UInt64>();

	private void Test<T>() where T : unmanaged
	{
		T[] values = WithSafeFixedTest.fixture.CreateMany<T>(10).ToArray();
		Span<Byte> span = MemoryMarshal.AsBytes(values.AsSpan());
		ReadOnlySpan<Byte> readOnlySpan = span;

		this._array = values;
		span.WithSafeFixed(this.ActionTest<T>);
		span.WithSafeFixed(this.ActionReadOnlyTest<T>);
		readOnlySpan.WithSafeFixed(this.ReadOnlyActionReadOnlyTest<T>);

		span.WithSafeFixed(this, WithSafeFixedTest.ActionTest<T>);
		span.WithSafeFixed(this, WithSafeFixedTest.ActionReadOnlyTest<T>);
		readOnlySpan.WithSafeFixed(this, WithSafeFixedTest.ReadOnlyActionReadOnlyTest<T>);

		PInvokeAssert.Equal(span.ToArray(), span.WithSafeFixed(this.FuncTest<T>));
		PInvokeAssert.Equal(span.ToArray(), span.WithSafeFixed(this.FuncReadOnlyTest<T>));
		PInvokeAssert.Equal(span.ToArray(), readOnlySpan.WithSafeFixed(this.ReadOnlyFuncReadOnlyTest<T>));

		PInvokeAssert.Equal(span.ToArray(), span.WithSafeFixed(this, WithSafeFixedTest.FuncTest<T>));
		PInvokeAssert.Equal(span.ToArray(), span.WithSafeFixed(this, WithSafeFixedTest.FuncReadOnlyTest<T>));
		PInvokeAssert.Equal(span.ToArray(),
		                    readOnlySpan.WithSafeFixed(this, WithSafeFixedTest.ReadOnlyFuncReadOnlyTest<T>));
	}
	private void ActionTest<T>(in IFixedMemory mem) where T : unmanaged
	{
		IFixedContext<Byte> bctx = mem.AsBinaryContext();
		IFixedContext<T> ctx = bctx.Transformation<T>(out IReadOnlyFixedMemory _);
		T[] arr = (T[])this._array!;

		PInvokeAssert.Equal(mem.Pointer, bctx.Pointer);
		PInvokeAssert.Equal(mem.Pointer, ctx.Pointer);
		PInvokeAssert.Equal(bctx, ctx.AsBinaryContext());
		PInvokeAssert.Equal(mem.Bytes.ToArray(), ctx.Bytes.ToArray());
		PInvokeAssert.Equal(arr, ctx.Values.ToArray());
		PInvokeAssert.True(Unsafe.AreSame(ref MemoryMarshal.GetReference(mem.Bytes),
		                                  ref MemoryMarshal.GetReference(bctx.Values)));
		PInvokeAssert.True(Unsafe.AreSame(ref MemoryMarshal.GetReference(arr.AsSpan()),
		                                  ref MemoryMarshal.GetReference(ctx.Values)));

		WithSafeFixedTest.Test<T, Boolean>(ctx);
		WithSafeFixedTest.Test<T, Byte>(ctx);
		WithSafeFixedTest.Test<T, Char>(ctx);
		WithSafeFixedTest.Test<T, DateTime>(ctx);
		WithSafeFixedTest.Test<T, Decimal>(ctx);
		WithSafeFixedTest.Test<T, Double>(ctx);
		WithSafeFixedTest.Test<T, Guid>(ctx);
#if NET5_0_OR_GREATER
		WithSafeFixedTest.Test<T, Half>(ctx);
#endif
		WithSafeFixedTest.Test<T, Int16>(ctx);
		WithSafeFixedTest.Test<T, Int32>(ctx);
		WithSafeFixedTest.Test<T, Int64>(ctx);
		WithSafeFixedTest.Test<T, SByte>(ctx);
		WithSafeFixedTest.Test<T, Single>(ctx);
		WithSafeFixedTest.Test<T, UInt16>(ctx);
		WithSafeFixedTest.Test<T, UInt32>(ctx);
		WithSafeFixedTest.Test<T, UInt64>(ctx);
	}
	private void ActionReadOnlyTest<T>(in IReadOnlyFixedMemory mem) where T : unmanaged
		=> this.ActionReadOnlyTest<T>(mem, false);
	private void ReadOnlyActionReadOnlyTest<T>(in IReadOnlyFixedMemory mem) where T : unmanaged
		=> this.ActionReadOnlyTest<T>(mem, true);
	private void ActionReadOnlyTest<T>(IReadOnlyFixedMemory mem, Boolean readOnly) where T : unmanaged
	{
		IReadOnlyFixedContext<Byte> bctx = mem.AsBinaryContext();
		IReadOnlyFixedContext<T> ctx = bctx.Transformation<T>(out IReadOnlyFixedMemory _);
		T[] arr = (T[])this._array!;

		PInvokeAssert.Equal(mem.Pointer, bctx.Pointer);
		PInvokeAssert.Equal(mem.Pointer, ctx.Pointer);
		PInvokeAssert.Equal(bctx, ctx.AsBinaryContext());
		PInvokeAssert.Equal(mem.Bytes.ToArray(), ctx.Bytes.ToArray());
		PInvokeAssert.Equal(arr, ctx.Values.ToArray());
		PInvokeAssert.True(Unsafe.AreSame(ref MemoryMarshal.GetReference(mem.Bytes),
		                                  ref MemoryMarshal.GetReference(bctx.Values)));
		PInvokeAssert.True(Unsafe.AreSame(ref MemoryMarshal.GetReference(arr.AsSpan()),
		                                  ref MemoryMarshal.GetReference(ctx.Values)));

		if (!readOnly)
		{
			IFixedContext<Byte> bctx2 = (IFixedContext<Byte>)mem.AsBinaryContext();
			IFixedContext<T> ctx2 = (IFixedContext<T>)ctx;
			PInvokeAssert.Equal(bctx2, ctx2.AsBinaryContext());
			PInvokeAssert.Equal(mem.Bytes.ToArray(), ctx2.Bytes.ToArray());
			PInvokeAssert.Equal(arr, ctx2.Values.ToArray());
			PInvokeAssert.True(Unsafe.AreSame(ref MemoryMarshal.GetReference(mem.Bytes),
			                                  ref MemoryMarshal.GetReference(bctx2.Values)));
			PInvokeAssert.True(Unsafe.AreSame(ref MemoryMarshal.GetReference(arr.AsSpan()),
			                                  ref MemoryMarshal.GetReference(ctx2.Values)));
		}
		else
		{
			PInvokeAssert.Throws<InvalidCastException>(() => (IFixedContext<Byte>)mem.AsBinaryContext());
		}

		WithSafeFixedTest.Test<T, Boolean>(ctx);
		WithSafeFixedTest.Test<T, Byte>(ctx);
		WithSafeFixedTest.Test<T, Char>(ctx);
		WithSafeFixedTest.Test<T, DateTime>(ctx);
		WithSafeFixedTest.Test<T, Decimal>(ctx);
		WithSafeFixedTest.Test<T, Double>(ctx);
		WithSafeFixedTest.Test<T, Guid>(ctx);
#if NET5_0_OR_GREATER
		WithSafeFixedTest.Test<T, Half>(ctx);
#endif
		WithSafeFixedTest.Test<T, Int16>(ctx);
		WithSafeFixedTest.Test<T, Int32>(ctx);
		WithSafeFixedTest.Test<T, Int64>(ctx);
		WithSafeFixedTest.Test<T, SByte>(ctx);
		WithSafeFixedTest.Test<T, Single>(ctx);
		WithSafeFixedTest.Test<T, UInt16>(ctx);
		WithSafeFixedTest.Test<T, UInt32>(ctx);
		WithSafeFixedTest.Test<T, UInt64>(ctx);
	}
	private Byte[] FuncTest<T>(in IFixedMemory mem) where T : unmanaged
	{
		this.ActionTest<T>(mem);
		return mem.Bytes.ToArray();
	}
	private Byte[] FuncReadOnlyTest<T>(in IReadOnlyFixedMemory mem) where T : unmanaged
	{
		this.ActionReadOnlyTest<T>(mem);
		return mem.Bytes.ToArray();
	}
	private Byte[] ReadOnlyFuncReadOnlyTest<T>(in IReadOnlyFixedMemory mem) where T : unmanaged
	{
		this.ReadOnlyActionReadOnlyTest<T>(mem);
		return mem.Bytes.ToArray();
	}

	private static unsafe void Test<T, T2>(IFixedContext<T> ctx) where T : unmanaged where T2 : unmanaged
	{
		IFixedContext<T2> ctx2 = ctx.Transformation<T2>(out IFixedMemory residual);
		IFixedContext<Byte> bctx = residual.AsBinaryContext();
		Int32 offset = ctx2.Values.Length * sizeof(T2);

		PInvokeAssert.Equal(ctx2, ctx.Transformation<T2>(out IReadOnlyFixedMemory residualR));
		PInvokeAssert.Equal(residual, residualR);

		PInvokeAssert.Equal(ctx.Pointer, ctx2.Pointer);
		PInvokeAssert.Equal(ctx.Bytes.Length / sizeof(T2), ctx2.Values.Length);
		PInvokeAssert.Equal(ctx.Bytes.Length, ctx2.Bytes.Length);
		PInvokeAssert.Equal(ctx.Bytes.Length - offset, residual.Bytes.Length);
		PInvokeAssert.Equal(ctx.Bytes.Length - offset, residualR.Bytes.Length);
		PInvokeAssert.Equal(ctx.Bytes.Length - offset, bctx.Bytes.Length);
		PInvokeAssert.Equal(ctx.Pointer + offset, residual.Pointer);
		PInvokeAssert.Equal(ctx.Pointer + offset, residualR.Pointer);
		PInvokeAssert.Equal(ctx.Pointer + offset, bctx.Pointer);
	}
	private static unsafe void Test<T, T2>(IReadOnlyFixedContext<T> ctx) where T : unmanaged where T2 : unmanaged
	{
		IReadOnlyFixedContext<T2> ctx2 = ctx.Transformation<T2>(out IReadOnlyFixedMemory residual);
		Int32 offset = ctx2.Values.Length * sizeof(T2);

		PInvokeAssert.Equal(ctx.Pointer, ctx2.Pointer);
		PInvokeAssert.Equal(ctx.Bytes.Length / sizeof(T2), ctx2.Values.Length);
		PInvokeAssert.Equal(ctx.Bytes.Length, ctx2.Bytes.Length);
		PInvokeAssert.Equal(ctx.Bytes.Length - offset, residual.Bytes.Length);
		PInvokeAssert.Equal(ctx.Pointer + offset, residual.Pointer);
	}
	private static void ActionTest<T>(in IFixedMemory mem, WithSafeFixedTest test) where T : unmanaged
		=> test.ActionTest<T>(mem);
	private static void ActionReadOnlyTest<T>(in IReadOnlyFixedMemory mem, WithSafeFixedTest test) where T : unmanaged
		=> test.ActionReadOnlyTest<T>(mem);
	private static void ReadOnlyActionReadOnlyTest<T>(in IReadOnlyFixedMemory mem, WithSafeFixedTest test)
		where T : unmanaged
		=> test.ReadOnlyActionReadOnlyTest<T>(mem);
	private static Byte[] FuncTest<T>(in IFixedMemory mem, WithSafeFixedTest test) where T : unmanaged
		=> test.FuncTest<T>(mem);
	private static Byte[] FuncReadOnlyTest<T>(in IReadOnlyFixedMemory mem, WithSafeFixedTest test) where T : unmanaged
		=> test.FuncReadOnlyTest<T>(mem);
	private static Byte[] ReadOnlyFuncReadOnlyTest<T>(in IReadOnlyFixedMemory mem, WithSafeFixedTest test)
		where T : unmanaged
		=> test.ReadOnlyFuncReadOnlyTest<T>(mem);
}