namespace Rxmxnx.PInvoke.Tests.StringExtensionsTests;

[TestFixture]
[ExcludeFromCodeCoverage]
public sealed class WithSafeFixedTest
{
	private static readonly IFixture fixture = new Fixture();

	[Theory]
	[InlineData(true)]
	[InlineData(false)]
	public void EmptyTest(Boolean nullInput)
	{
		String? value = !nullInput ? String.Empty : default;
		value.WithSafeFixed(WithSafeFixedTest.EmptyActionTest);
		value.WithSafeFixed(value, WithSafeFixedTest.EmptyActionTest);

		PInvokeAssert.Equal(value, value.WithSafeFixed(WithSafeFixedTest.EmptyFuncTest));
		PInvokeAssert.Equal(value, value.WithSafeFixed(value, WithSafeFixedTest.EmptyFuncTest));
	}

	[Fact]
	public void NormalTest()
	{
		String value = WithSafeFixedTest.fixture.Create<String>();
		value.WithSafeFixed(value, WithSafeFixedTest.ActionTest);
		PInvokeAssert.Equal(value, value.WithSafeFixed(WithSafeFixedTest.FuncTest));
	}

	private static unsafe void EmptyActionTest(in IReadOnlyFixedContext<Char> ctx)
	{
		PInvokeAssert.Equal(0, ctx.Bytes.Length);
		if (ctx.Pointer != IntPtr.Zero)
			fixed (Char* ptr = String.Empty)
				PInvokeAssert.Equal(new(ptr), ctx.Pointer);
	}
	private static unsafe void EmptyActionTest(in IReadOnlyFixedContext<Char> ctx, String? value)
	{
		WithSafeFixedTest.EmptyActionTest(ctx);
		if (value is null)
			PInvokeAssert.Equal(IntPtr.Zero, ctx.Pointer);
		else
			fixed (Char* ptr = String.Empty)
				PInvokeAssert.Equal(new(ptr), ctx.Pointer);
	}
	private static String? EmptyFuncTest(in IReadOnlyFixedContext<Char> ctx)
	{
		WithSafeFixedTest.EmptyActionTest(ctx);
		return ctx.Pointer != IntPtr.Zero ? String.Empty : default;
	}
	private static String? EmptyFuncTest(in IReadOnlyFixedContext<Char> ctx, String? value)
	{
		WithSafeFixedTest.EmptyActionTest(ctx, value);
		return value;
	}

	private static void ActionTest(in IReadOnlyFixedContext<Char> ctx, String value)
	{
		PInvokeAssert.Equal(value.Length, ctx.Values.Length);
		PInvokeAssert.Equal(value.Length * sizeof(Char), ctx.Bytes.Length);
		PInvokeAssert.Equal(value, new(ctx.Values));
	}
	private static String FuncTest(in IReadOnlyFixedContext<Char> ctx) => new(ctx.Values);
}