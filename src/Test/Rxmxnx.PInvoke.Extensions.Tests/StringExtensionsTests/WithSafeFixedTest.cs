namespace Rxmxnx.PInvoke.Tests.StringExtensionsTests;

[ExcludeFromCodeCoverage]
public sealed class WithSafeFixedTest
{
    private static readonly IFixture fixture = new Fixture();

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    internal void EmptyTest(Boolean nullInput)
    {
        String? value = !nullInput ? String.Empty : default;
        value.WithSafeFixed(EmptyActionTest);
        value.WithSafeFixed(value, EmptyActionTest);

        Assert.Equal(value, value.WithSafeFixed(EmptyFuncTest));
        Assert.Equal(value, value.WithSafeFixed(value, EmptyFuncTest));
    }

    [Fact]
    internal void NormalTest()
    {
        String value = fixture.Create<String>();
        value.WithSafeFixed(value, ActionTest);
        Assert.Equal(value, value.WithSafeFixed(FuncTest));
    }


    private static unsafe void EmptyActionTest(in IReadOnlyFixedContext<Char> ctx)
    {
        Assert.Equal(0, ctx.Bytes.Length);
        if (ctx.Pointer != IntPtr.Zero)
            fixed (Char* ptr = String.Empty)
                Assert.Equal(new(ptr), ctx.Pointer);
    }
    private static unsafe void EmptyActionTest(in IReadOnlyFixedContext<Char> ctx, String? value)
    {
        EmptyActionTest(ctx);
        if (value is null)
            Assert.Equal(IntPtr.Zero, ctx.Pointer);
        else
            fixed (Char* ptr = String.Empty)
                Assert.Equal(new(ptr), ctx.Pointer);
    }
    private static String? EmptyFuncTest(in IReadOnlyFixedContext<Char> ctx)
    {
        EmptyActionTest(ctx);
        if (ctx.Pointer != IntPtr.Zero)
            return String.Empty;
        else
            return default;
    }
    private static String? EmptyFuncTest(in IReadOnlyFixedContext<Char> ctx, String? value)
    {
        EmptyActionTest(ctx, value);
        return value;
    }

    private static unsafe void ActionTest(in IReadOnlyFixedContext<Char> ctx, String value)
    {
        Assert.Equal(value.Length, ctx.Values.Length);
        Assert.Equal(value.Length * sizeof(Char), ctx.Bytes.Length);
        Assert.Equal(value, new(ctx.Values));
    }
    private static String FuncTest(in IReadOnlyFixedContext<Char> ctx)
        => new(ctx.Values);
}

