namespace Rxmxnx.PInvoke.Tests.StringExtensionsTests;

[TestFixture]
[ExcludeFromCodeCoverage]
public sealed class WithSafeFixedTest
{
	private static readonly IFixture fixture = new Fixture();

	[Theory]
#if NETSTANDARD2_1 && !LEGACY || NETCOREAPP3_0_OR_GREATER
	[Obsolete]
#endif
	[InlineData(true)]
	[InlineData(false)]
	public void EmptyTest(Boolean nullInput)
	{
		String? value = !nullInput ? String.Empty : default;
#if NETSTANDARD2_1 && !LEGACY || NETCOREAPP3_0_OR_GREATER
		value.WithSafeFixed(WithSafeFixedTest.EmptyActionTest);
		value.WithSafeFixed(value, WithSafeFixedTest.EmptyActionTest);

		PInvokeAssert.Equal(value, value.WithSafeFixed(WithSafeFixedTest.EmptyFuncTest));
		PInvokeAssert.Equal(value, value.WithSafeFixed(value, WithSafeFixedTest.EmptyFuncTest));
#endif
		value.WithSafeFixed(new ReadOnlyFixedAction(value));
		value.WithSafeFixed(new ReadOnlyFixedFunction(value), out String? result);
		PInvokeAssert.Equal(value, result);
	}

	[Fact]
#if NETSTANDARD2_1 && !LEGACY || NETCOREAPP3_0_OR_GREATER
	[Obsolete]
#endif
	public void NormalTest()
	{
		String value = WithSafeFixedTest.fixture.Create<String>();
#if NETSTANDARD2_1 && !LEGACY || NETCOREAPP3_0_OR_GREATER
		value.WithSafeFixed(value, WithSafeFixedTest.ActionTest);
		PInvokeAssert.Equal(value, value.WithSafeFixed(WithSafeFixedTest.FuncTest));
#endif
		value.WithSafeFixed(new ReadOnlyFixedAction(value));
		value.WithSafeFixed(new ReadOnlyFixedFunction(value), out String? result);
		PInvokeAssert.Equal(value, result);
	}

#if NETSTANDARD2_1 && !LEGACY || NETCOREAPP3_0_OR_GREATER
	[Obsolete]
	private static unsafe void EmptyActionTest(in IReadOnlyFixedContext<Char> ctx)
	{
		PInvokeAssert.Equal(0, ctx.Bytes.Length);
		if (ctx.Pointer != IntPtr.Zero)
			fixed (Char* ptr = String.Empty)
				PInvokeAssert.Equal(new(ptr), ctx.Pointer);
	}
	[Obsolete]
	private static unsafe void EmptyActionTest(in IReadOnlyFixedContext<Char> ctx, String? value)
	{
		WithSafeFixedTest.EmptyActionTest(ctx);
		if (value is null)
			PInvokeAssert.Equal(IntPtr.Zero, ctx.Pointer);
		else
			fixed (Char* ptr = String.Empty)
				PInvokeAssert.Equal(new(ptr), ctx.Pointer);
	}
	[Obsolete]
	private static String? EmptyFuncTest(in IReadOnlyFixedContext<Char> ctx)
	{
		WithSafeFixedTest.EmptyActionTest(ctx);
		return ctx.Pointer != IntPtr.Zero ? String.Empty : default;
	}
	[Obsolete]
	private static String? EmptyFuncTest(in IReadOnlyFixedContext<Char> ctx, String? value)
	{
		WithSafeFixedTest.EmptyActionTest(ctx, value);
		return value;
	}

	[Obsolete]
	private static void ActionTest(in IReadOnlyFixedContext<Char> ctx, String value)
	{
		PInvokeAssert.Equal(value.Length, ctx.Values.Length);
		PInvokeAssert.Equal(value.Length * sizeof(Char), ctx.Bytes.Length);
		PInvokeAssert.Equal(value, new(ctx.Values));
	}
	[Obsolete]
	private static String FuncTest(in IReadOnlyFixedContext<Char> ctx) => new(ctx.Values);
#endif

	private readonly struct ReadOnlyFixedAction(String? value) : IReadOnlyFixedContextAction<Char>
	{
		public unsafe void Accept(scoped ReadOnlyFixedContextValue<Char> ctx)
		{
			if (String.IsNullOrEmpty(value))
			{
				PInvokeAssert.Equal(0, ctx.Bytes.Length);
				if (ctx.Pointer != IntPtr.Zero)
					fixed (Char* ptr = String.Empty)
						PInvokeAssert.Equal(new(ptr), ctx.Pointer);
				if (value is null)
					PInvokeAssert.Equal(IntPtr.Zero, ctx.Pointer);
				else
					fixed (Char* ptr = String.Empty)
						PInvokeAssert.Equal(new(ptr), ctx.Pointer);
				return;
			}
			PInvokeAssert.Equal(value!.Length, ctx.Values.Length);
			PInvokeAssert.Equal(value.Length * sizeof(Char), ctx.Bytes.Length);
			PInvokeAssert.Equal(value, ctx.Values.ToString());
		}
	}

	private readonly struct ReadOnlyFixedFunction(String? value) : IReadOnlyFixedContextFunction<Char, String?>
	{
		public String? Apply(scoped ReadOnlyFixedContextValue<Char> ctx)
		{
			new ReadOnlyFixedAction(value).Accept(ctx);
			return !String.IsNullOrEmpty(value) ? ctx.Values.ToString() : value;
		}
	}
}