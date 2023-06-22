namespace Rxmxnx.PInvoke.Tests.UnmanagedValueExtensionsTest;

[ExcludeFromCodeCoverage]
[SuppressMessage("csharpsquid", "S2699")]
public sealed class WithSafeFixedTest
{
    private static readonly IFixture fixture = new Fixture();

    private Array? _array = default;

    [Fact]
    internal void ByteTest() => this.Test<Byte>();
    [Fact]
    internal void CharTest() => this.Test<Char>();
    [Fact]
    internal void DateTimeTest() => this.Test<DateTime>();
    [Fact]
    internal void DecimalTest() => this.Test<Decimal>();
    [Fact]
    internal void DoubleTest() => this.Test<Double>();
    [Fact]
    internal void GuidTest() => this.Test<Guid>();
    [Fact]
    internal void HalfTest() => this.Test<Half>();
    [Fact]
    internal void Int16Test() => this.Test<Int16>();
    [Fact]
    internal void Int32Test() => this.Test<Int32>();
    [Fact]
    internal void Int64Test() => this.Test<Int64>();
    [Fact]
    internal void SByteTest() => this.Test<SByte>();
    [Fact]
    internal void SingleTest() => this.Test<Single>();
    [Fact]
    internal void UInt16Test() => this.Test<UInt16>();
    [Fact]
    internal void UInt32Test() => this.Test<UInt32>();
    [Fact]
    internal void UInt64Test() => this.Test<UInt64>();

    private void Test<T>() where T : unmanaged
    {
        T[]? values = fixture.CreateMany<T>(10).ToArray();

        _array = values;
        values.WithSafeFixed(this.ActionTest);
        values.WithSafeFixed(this.ActionReadOnlyTest);

        values.WithSafeFixed(this, ActionTest);
        values.WithSafeFixed(this, ActionReadOnlyTest);

        Assert.Equal(values, values.WithSafeFixed(this.FuncTest));
        Assert.Equal(values, values.WithSafeFixed(this.FuncReadOnlyTest));

        Assert.Equal(values, values.WithSafeFixed(this, FuncTest));
        Assert.Equal(values, values.WithSafeFixed(this, FuncReadOnlyTest));

        values = default;

        values.WithSafeFixed(this.NullActionTest);
        values.WithSafeFixed(this.NullActionReadOnlyTest);

        values.WithSafeFixed(this, NullActionTest);
        values.WithSafeFixed(this, NullActionReadOnlyTest);

        Assert.Equal(values.WithSafeFixed(this.NullFuncTest), values.WithSafeFixed(this.NullFuncReadOnlyTest));
        Assert.Equal(values.WithSafeFixed(this, NullFuncTest), values.WithSafeFixed(this, NullFuncReadOnlyTest));
    }
    private void ActionTest<T>(in IFixedContext<T> ctx) where T : unmanaged
    {
        IFixedContext<Byte> bctx = ctx.AsBinaryContext();
        T[] arr = (T[])_array!;

        Assert.Equal(ctx.Pointer, bctx.Pointer);
        Assert.Equal(bctx.Bytes.ToArray(), ctx.Bytes.ToArray());
        Assert.Equal(arr, ctx.Values.ToArray());
        Assert.True(Unsafe.AreSame(
            ref MemoryMarshal.GetReference(ctx.Bytes),
            ref MemoryMarshal.GetReference(bctx.Values)));
        Assert.True(Unsafe.AreSame(
            ref MemoryMarshal.GetReference(arr.AsSpan()),
            ref MemoryMarshal.GetReference(ctx.Values)));

        Test<T, Boolean>(ctx);
        Test<T, Byte>(ctx);
        Test<T, Char>(ctx);
        Test<T, DateTime>(ctx);
        Test<T, Decimal>(ctx);
        Test<T, Double>(ctx);
        Test<T, Guid>(ctx);
        Test<T, Half>(ctx);
        Test<T, Int16>(ctx);
        Test<T, Int32>(ctx);
        Test<T, Int64>(ctx);
        Test<T, SByte>(ctx);
        Test<T, Single>(ctx);
        Test<T, UInt16>(ctx);
        Test<T, UInt32>(ctx);
        Test<T, UInt64>(ctx);
    }
    private void ActionReadOnlyTest<T>(in IReadOnlyFixedContext<T> ctx) where T : unmanaged
        => ActionReadOnlyTest(ctx);
    private void ActionReadOnlyTest<T>(IReadOnlyFixedContext<T> ctx) where T : unmanaged
    {
        IReadOnlyFixedContext<Byte> bctx = ctx.AsBinaryContext();
        T[] arr = (T[])_array!;

        Assert.Equal(ctx.Pointer, bctx.Pointer);
        Assert.Equal(bctx, ctx.AsBinaryContext());
        Assert.Equal(bctx.Bytes.ToArray(), ctx.Bytes.ToArray());
        Assert.Equal(arr, ctx.Values.ToArray());
        Assert.True(Unsafe.AreSame(
            ref MemoryMarshal.GetReference(ctx.Bytes),
            ref MemoryMarshal.GetReference(bctx.Values)));
        Assert.True(Unsafe.AreSame(
            ref MemoryMarshal.GetReference(arr.AsSpan()),
            ref MemoryMarshal.GetReference(ctx.Values)));

        IFixedContext<Byte> bctx2 = (IFixedContext<Byte>)ctx.AsBinaryContext();
        IFixedContext<T> ctx2 = (IFixedContext<T>)ctx;
        Assert.Equal(bctx2, ctx2.AsBinaryContext());
        Assert.Equal(ctx.Bytes.ToArray(), ctx2.Bytes.ToArray());
        Assert.Equal(arr, ctx2.Values.ToArray());
        Assert.True(Unsafe.AreSame(
            ref MemoryMarshal.GetReference(ctx.Bytes),
            ref MemoryMarshal.GetReference(bctx2.Values)));
        Assert.True(Unsafe.AreSame(
            ref MemoryMarshal.GetReference(arr.AsSpan()),
            ref MemoryMarshal.GetReference(ctx2.Values)));

        Test<T, Boolean>(ctx);
        Test<T, Byte>(ctx);
        Test<T, Char>(ctx);
        Test<T, DateTime>(ctx);
        Test<T, Decimal>(ctx);
        Test<T, Double>(ctx);
        Test<T, Guid>(ctx);
        Test<T, Half>(ctx);
        Test<T, Int16>(ctx);
        Test<T, Int32>(ctx);
        Test<T, Int64>(ctx);
        Test<T, SByte>(ctx);
        Test<T, Single>(ctx);
        Test<T, UInt16>(ctx);
        Test<T, UInt32>(ctx);
        Test<T, UInt64>(ctx);
    }
    private T[] FuncTest<T>(in IFixedContext<T> ctx) where T : unmanaged
    {
        this.ActionTest<T>(ctx);
        return ctx.Values.ToArray();
    }
    private T[] FuncReadOnlyTest<T>(in IReadOnlyFixedContext<T> ctx) where T : unmanaged
    {
        this.ActionReadOnlyTest<T>(ctx);
        return ctx.Values.ToArray();
    }

    private void NullActionTest<T>(in IFixedContext<T> ctx) where T : unmanaged
    {
        Assert.Equal(0, ctx.Bytes.Length);
        Assert.Equal(0, ctx.Values.Length);
        Assert.Equal(IntPtr.Zero, ctx.Pointer);
    }
    private void NullActionReadOnlyTest<T>(in IReadOnlyFixedContext<T> ctx) where T : unmanaged
        => NullActionTest((IFixedContext<T>)ctx);
    private IFixedContext<T> NullFuncTest<T>(in IFixedContext<T> ctx) where T : unmanaged
    {
        this.NullActionTest(ctx);
        return ctx;
    }
    private IReadOnlyFixedContext<T> NullFuncReadOnlyTest<T>(in IReadOnlyFixedContext<T> ctx) where T : unmanaged
        => NullFuncTest((IFixedContext<T>)ctx);

    private static unsafe void Test<T, T2>(IFixedContext<T> ctx)
        where T : unmanaged where T2 : unmanaged
    {
        IFixedContext<T2> ctx2 = ctx.Transformation<T2>(out IFixedMemory residual);
        IFixedContext<Byte> bctx = residual.AsBinaryContext();
        Int32 offset = ctx2.Values.Length * sizeof(T2);

        Assert.Equal(ctx2, ctx.Transformation<T2>(out IReadOnlyFixedMemory residualR));
        Assert.Equal(residual, residualR);

        Assert.Equal(ctx.Pointer, ctx2.Pointer);
        Assert.Equal(ctx.Bytes.Length / sizeof(T2), ctx2.Values.Length);
        Assert.Equal(ctx.Bytes.Length, ctx2.Bytes.Length);
        Assert.Equal(ctx.Bytes.Length - offset, residual.Bytes.Length);
        Assert.Equal(ctx.Bytes.Length - offset, residualR.Bytes.Length);
        Assert.Equal(ctx.Bytes.Length - offset, bctx.Bytes.Length);
        Assert.Equal(ctx.Pointer + offset, residual.Pointer);
        Assert.Equal(ctx.Pointer + offset, residualR.Pointer);
        Assert.Equal(ctx.Pointer + offset, bctx.Pointer);
    }
    private static unsafe void Test<T, T2>(IReadOnlyFixedContext<T> ctx)
        where T : unmanaged where T2 : unmanaged
    {
        IReadOnlyFixedContext<T2> ctx2 = ctx.Transformation<T2>(out IReadOnlyFixedMemory residual);
        Int32 offset = ctx2.Values.Length * sizeof(T2);

        Assert.Equal(ctx.Pointer, ctx2.Pointer);
        Assert.Equal(ctx.Bytes.Length / sizeof(T2), ctx2.Values.Length);
        Assert.Equal(ctx.Bytes.Length, ctx2.Bytes.Length);
        Assert.Equal(ctx.Bytes.Length - offset, residual.Bytes.Length);
        Assert.Equal(ctx.Pointer + offset, residual.Pointer);
    }
    private static void ActionTest<T>(in IFixedContext<T> ctx, WithSafeFixedTest test) where T : unmanaged
        => test.ActionTest(ctx);
    private static void ActionReadOnlyTest<T>(in IReadOnlyFixedContext<T> ctx, WithSafeFixedTest test) where T : unmanaged
        => test.ActionReadOnlyTest(ctx);
    private static T[] FuncTest<T>(in IFixedContext<T> ctx, WithSafeFixedTest test) where T : unmanaged
        => test.FuncTest(ctx);
    private static T[] FuncReadOnlyTest<T>(in IReadOnlyFixedContext<T> ctx, WithSafeFixedTest test) where T : unmanaged
        => test.FuncReadOnlyTest(ctx);

    private static void NullActionTest<T>(in IFixedContext<T> ctx, WithSafeFixedTest test) where T : unmanaged
        => test.NullActionTest(ctx);
    private static void NullActionReadOnlyTest<T>(in IReadOnlyFixedContext<T> ctx, WithSafeFixedTest test) where T : unmanaged
        => test.NullActionReadOnlyTest(ctx);
    private static IFixedContext<T> NullFuncTest<T>(in IFixedContext<T> ctx, WithSafeFixedTest test) where T : unmanaged
        => test.NullFuncTest(ctx);
    private static IReadOnlyFixedContext<T> NullFuncReadOnlyTest<T>(in IReadOnlyFixedContext<T> ctx, WithSafeFixedTest test) where T : unmanaged
        => test.NullFuncReadOnlyTest(ctx);
}
