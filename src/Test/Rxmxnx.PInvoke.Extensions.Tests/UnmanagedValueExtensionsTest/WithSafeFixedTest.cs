namespace Rxmxnx.PInvoke.Tests.UnmanagedValueExtensionsTest;

[TestFixture]
[ExcludeFromCodeCoverage]
[SuppressMessage("csharpsquid", "S2699")]
public sealed class WithSafeFixedTest
{
	private static readonly IFixture fixture = new Fixture();
	private Array? _array;

#pragma warning disable CS0612
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
#pragma warning restore CS0612

#if NETSTANDARD2_1 && !LEGACY || NETCOREAPP3_0_OR_GREATER
	[Obsolete]
#endif
	private void Test<T>() where T : unmanaged
	{
		T[]? values = WithSafeFixedTest.fixture.CreateMany<T>(10).ToArray();

		this._array = values;
#if NETSTANDARD2_1 && !LEGACY || NETCOREAPP3_0_OR_GREATER
		values.WithSafeFixed(this.ActionTest);
		values.WithSafeFixed(this.ActionReadOnlyTest);

		values.WithSafeFixed(this, WithSafeFixedTest.ActionTest);
		values.WithSafeFixed(this, WithSafeFixedTest.ActionReadOnlyTest);

		PInvokeAssert.Equal(values, values.WithSafeFixed(this.FuncTest));
		PInvokeAssert.Equal(values, values.WithSafeFixed(this.FuncReadOnlyTest));

		PInvokeAssert.Equal(values, values.WithSafeFixed(this, WithSafeFixedTest.FuncTest));
		PInvokeAssert.Equal(values, values.WithSafeFixed(this, WithSafeFixedTest.FuncReadOnlyTest));
#endif
		values.WithSafeFixed(new FixedAction<T>(this._array));
		values.WithSafeFixed(new ReadOnlyFixedAction<T>(this._array));
		values.WithSafeFixed(new FixedFunction<T>(this._array), out T[]? result);
		PInvokeAssert.Equal(values, result);
		values.WithSafeFixed(new ReadOnlyFixedFunction<T>(this._array), out result);
		PInvokeAssert.Equal(values, result);
#if !NETFRAMEWORK || NET46_OR_GREATER
		values = Array.Empty<T>();
#else
		values = ArrayCompat.Empty<T>();
#endif
		this._array = values;
#if NETSTANDARD2_1 && !LEGACY || NETCOREAPP3_0_OR_GREATER
		values.WithSafeFixed(this.ActionTest);
		values.WithSafeFixed(this.ActionReadOnlyTest);

		values.WithSafeFixed(this, WithSafeFixedTest.ActionTest);
		values.WithSafeFixed(this, WithSafeFixedTest.ActionReadOnlyTest);

		PInvokeAssert.Equal(values, values.WithSafeFixed(this.FuncTest));
		PInvokeAssert.Equal(values, values.WithSafeFixed(this.FuncReadOnlyTest));

		PInvokeAssert.Equal(values, values.WithSafeFixed(this, WithSafeFixedTest.FuncTest));
		PInvokeAssert.Equal(values, values.WithSafeFixed(this, WithSafeFixedTest.FuncReadOnlyTest));
#endif
		values.WithSafeFixed(new FixedAction<T>(this._array));
		values.WithSafeFixed(new ReadOnlyFixedAction<T>(this._array));
		values.WithSafeFixed(new FixedFunction<T>(this._array), out result);
		PInvokeAssert.Equal(values, result);
		values.WithSafeFixed(new ReadOnlyFixedFunction<T>(this._array), out result);
		PInvokeAssert.Equal(values, result);
		values = default;
		this._array = values;
#if NETSTANDARD2_1 && !LEGACY || NETCOREAPP3_0_OR_GREATER
		values.WithSafeFixed(this.NullActionTest);
		values.WithSafeFixed(this.NullActionReadOnlyTest);

		values.WithSafeFixed(this, WithSafeFixedTest.NullActionTest);
		values.WithSafeFixed(this, WithSafeFixedTest.NullActionReadOnlyTest);

		PInvokeAssert.Equal(values.WithSafeFixed(this.NullFuncTest), values.WithSafeFixed(this.NullFuncReadOnlyTest));
		PInvokeAssert.Equal(values.WithSafeFixed(this, WithSafeFixedTest.NullFuncTest),
		                    values.WithSafeFixed(this, WithSafeFixedTest.NullFuncReadOnlyTest));
#endif
		values.WithSafeFixed(new FixedAction<T>(this._array));
		values.WithSafeFixed(new ReadOnlyFixedAction<T>(this._array));
		values.WithSafeFixed(new FixedFunction<T>(this._array), out result);
		PInvokeAssert.Equal(values, result);
		values.WithSafeFixed(new ReadOnlyFixedFunction<T>(this._array), out result);
	}
#if NETSTANDARD2_1 && !LEGACY || NETCOREAPP3_0_OR_GREATER
	[Obsolete]
	private void ActionTest<T>(in IFixedContext<T> ctx) where T : unmanaged
	{
		IFixedContext<Byte> bctx = ctx.AsBinaryContext();
		T[] arr = (T[])this._array!;

		PInvokeAssert.Equal(ctx.Pointer, bctx.Pointer);
		PInvokeAssert.Equal(bctx.Bytes.ToArray(), ctx.Bytes.ToArray());
		PInvokeAssert.Equal(arr, ctx.Values.ToArray());
		PInvokeAssert.True(Unsafe.AreSame(ref MemoryMarshal.GetReference(ctx.Bytes),
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
	[Obsolete]
	private void ActionReadOnlyTest<T>(in IReadOnlyFixedContext<T> ctx) where T : unmanaged
		=> this.ActionReadOnlyTest(ctx);
	[Obsolete]
	private void ActionReadOnlyTest<T>(IReadOnlyFixedContext<T> ctx) where T : unmanaged
	{
		IReadOnlyFixedContext<Byte> bctx = ctx.AsBinaryContext();
		T[] arr = (T[])this._array!;

		PInvokeAssert.Equal(ctx.Pointer, bctx.Pointer);
		PInvokeAssert.Equal(bctx, ctx.AsBinaryContext());
		PInvokeAssert.Equal(bctx.Bytes.ToArray(), ctx.Bytes.ToArray());
		PInvokeAssert.Equal(arr, ctx.Values.ToArray());
		PInvokeAssert.True(Unsafe.AreSame(ref MemoryMarshal.GetReference(ctx.Bytes),
		                                  ref MemoryMarshal.GetReference(bctx.Values)));
		PInvokeAssert.True(Unsafe.AreSame(ref MemoryMarshal.GetReference(arr.AsSpan()),
		                                  ref MemoryMarshal.GetReference(ctx.Values)));

		IFixedContext<Byte> bctx2 = (IFixedContext<Byte>)ctx.AsBinaryContext();
		IFixedContext<T> ctx2 = (IFixedContext<T>)ctx;
		PInvokeAssert.Equal(bctx2, ctx2.AsBinaryContext());
		PInvokeAssert.Equal(ctx.Bytes.ToArray(), ctx2.Bytes.ToArray());
		PInvokeAssert.Equal(arr, ctx2.Values.ToArray());
		PInvokeAssert.True(Unsafe.AreSame(ref MemoryMarshal.GetReference(ctx.Bytes),
		                                  ref MemoryMarshal.GetReference(bctx2.Values)));
		PInvokeAssert.True(Unsafe.AreSame(ref MemoryMarshal.GetReference(arr.AsSpan()),
		                                  ref MemoryMarshal.GetReference(ctx2.Values)));

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
	[Obsolete]
	private T[] FuncTest<T>(in IFixedContext<T> ctx) where T : unmanaged
	{
		this.ActionTest(ctx);
		return ctx.Values.ToArray();
	}
	[Obsolete]
	private T[] FuncReadOnlyTest<T>(in IReadOnlyFixedContext<T> ctx) where T : unmanaged
	{
		this.ActionReadOnlyTest(ctx);
		return ctx.Values.ToArray();
	}

	[Obsolete]
	[SuppressMessage("Performance", "CA1822:Mark members as static")]
	private void NullActionTest<T>(in IFixedContext<T> ctx) where T : unmanaged
	{
		PInvokeAssert.Equal(0, ctx.Bytes.Length);
		PInvokeAssert.Equal(0, ctx.Values.Length);
		PInvokeAssert.Equal(IntPtr.Zero, ctx.Pointer);
	}
	[Obsolete]
	private void NullActionReadOnlyTest<T>(in IReadOnlyFixedContext<T> ctx) where T : unmanaged
		=> this.NullActionTest((IFixedContext<T>)ctx);
	[Obsolete]
	private IFixedContext<T> NullFuncTest<T>(in IFixedContext<T> ctx) where T : unmanaged
	{
		this.NullActionTest(ctx);
		return ctx;
	}
	[Obsolete]
	private IReadOnlyFixedContext<T> NullFuncReadOnlyTest<T>(in IReadOnlyFixedContext<T> ctx) where T : unmanaged
		=> this.NullFuncTest((IFixedContext<T>)ctx);

	[Obsolete]
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
	[Obsolete]
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
	[Obsolete]
	private static void ActionTest<T>(in IFixedContext<T> ctx, WithSafeFixedTest test) where T : unmanaged
		=> test.ActionTest(ctx);
	[Obsolete]
	private static void ActionReadOnlyTest<T>(in IReadOnlyFixedContext<T> ctx, WithSafeFixedTest test)
		where T : unmanaged
		=> test.ActionReadOnlyTest(ctx);
	[Obsolete]
	private static T[] FuncTest<T>(in IFixedContext<T> ctx, WithSafeFixedTest test) where T : unmanaged
		=> test.FuncTest(ctx);
	[Obsolete]
	private static T[] FuncReadOnlyTest<T>(in IReadOnlyFixedContext<T> ctx, WithSafeFixedTest test) where T : unmanaged
		=> test.FuncReadOnlyTest(ctx);

	[Obsolete]
	private static void NullActionTest<T>(in IFixedContext<T> ctx, WithSafeFixedTest test) where T : unmanaged
		=> test.NullActionTest(ctx);
	[Obsolete]
	private static void NullActionReadOnlyTest<T>(in IReadOnlyFixedContext<T> ctx, WithSafeFixedTest test)
		where T : unmanaged
		=> test.NullActionReadOnlyTest(ctx);
	[Obsolete]
	private static IFixedContext<T> NullFuncTest<T>(in IFixedContext<T> ctx, WithSafeFixedTest test) where T : unmanaged
		=> test.NullFuncTest(ctx);
	[Obsolete]
	private static IReadOnlyFixedContext<T> NullFuncReadOnlyTest<T>(in IReadOnlyFixedContext<T> ctx,
		WithSafeFixedTest test) where T : unmanaged
		=> test.NullFuncReadOnlyTest(ctx);
#endif
	private readonly struct FixedAction<T>(Array? array) : IFixedContextAction<T> where T : unmanaged
	{
		public void Accept(scoped FixedContextValue<T> ctx)
		{
			if (array is null)
			{
				PInvokeAssert.Equal(0, ctx.Bytes.Length);
				PInvokeAssert.Equal(0, ctx.Values.Length);
				PInvokeAssert.Equal(IntPtr.Zero, ctx.Pointer);
				return;
			}

			PInvokeAssert.True(((FixedPointerValue)ctx).TryGetBinaryContext(out FixedContextValue<Byte> bctx));
			T[] arr = (T[])array;

			PInvokeAssert.Equal(ctx.Pointer, bctx.Pointer);
			PInvokeAssert.Equal(bctx.Bytes.ToArray(), ctx.Bytes.ToArray());
			PInvokeAssert.Equal(arr, ctx.Values.ToArray());
			PInvokeAssert.True(Unsafe.AreSame(ref MemoryMarshal.GetReference(ctx.Bytes),
			                                  ref MemoryMarshal.GetReference(bctx.Values)));
			PInvokeAssert.True(Unsafe.AreSame(ref MemoryMarshal.GetReference(arr.AsSpan()),
			                                  ref MemoryMarshal.GetReference(ctx.Values)));

			FixedContextValue<Byte> bctx2 = (FixedContextValue<Byte>)(FixedPointerValue)ctx;
			PInvokeAssert.Equal(ctx.Bytes.ToArray(), bctx2.Bytes.ToArray());
			PInvokeAssert.True(Unsafe.AreSame(ref MemoryMarshal.GetReference(ctx.Bytes),
			                                  ref MemoryMarshal.GetReference(bctx2.Values)));
			PInvokeAssert.True(Unsafe.AreSame(ref MemoryMarshal.GetReference(MemoryMarshal.AsBytes(arr.AsSpan())),
			                                  ref MemoryMarshal.GetReference(bctx2.Values)));

			FixedAction<T>.Test<Boolean>(ctx);
			FixedAction<T>.Test<Byte>(ctx);
			FixedAction<T>.Test<Char>(ctx);
			FixedAction<T>.Test<DateTime>(ctx);
			FixedAction<T>.Test<Decimal>(ctx);
			FixedAction<T>.Test<Double>(ctx);
			FixedAction<T>.Test<Guid>(ctx);
#if NET5_0_OR_GREATER
			FixedAction<T>.Test<Half>(ctx);
#endif
			FixedAction<T>.Test<Int16>(ctx);
			FixedAction<T>.Test<Int32>(ctx);
			FixedAction<T>.Test<Int64>(ctx);
			FixedAction<T>.Test<SByte>(ctx);
			FixedAction<T>.Test<Single>(ctx);
			FixedAction<T>.Test<UInt16>(ctx);
			FixedAction<T>.Test<UInt32>(ctx);
			FixedAction<T>.Test<UInt64>(ctx);
		}
		private static unsafe void Test<T2>(FixedContextValue<T> ctx) where T2 : unmanaged
		{
			FixedContextValue<T2> ctx2 = ctx.Transformation<T2>(out FixedPointerValue residual);
			Int32 offset = ctx2.Values.Length * sizeof(T2);

			PInvokeAssert.Equal(ctx.Pointer, ctx2.Pointer);
			PInvokeAssert.Equal(ctx.Bytes.Length / sizeof(T2), ctx2.Values.Length);
			PInvokeAssert.Equal(ctx.Bytes.Length, ctx2.Bytes.Length);
			PInvokeAssert.Equal(ctx.Bytes.Length - offset, ((FixedContextValue<Byte>)residual).Values.Length);
			PInvokeAssert.Equal(ctx.Pointer + offset, residual.Pointer);
		}
	}

	private readonly struct ReadOnlyFixedAction<T>(Array? array) : IReadOnlyFixedContextAction<T> where T : unmanaged
	{
		public void Accept(scoped ReadOnlyFixedContextValue<T> ctx)
		{
			if (array is null)
			{
				PInvokeAssert.Equal(0, ctx.Bytes.Length);
				PInvokeAssert.Equal(0, ctx.Values.Length);
				PInvokeAssert.Equal(IntPtr.Zero, ctx.Pointer);
				return;
			}

			PInvokeAssert.True(
				((FixedPointerValue)ctx).TryGetReadOnlyBinaryContext(out ReadOnlyFixedContextValue<Byte> bctx));
			T[] arr = (T[])array;

			PInvokeAssert.Equal(ctx.Pointer, bctx.Pointer);
			PInvokeAssert.Equal(bctx.Bytes.ToArray(), ctx.Bytes.ToArray());
			PInvokeAssert.Equal(arr, ctx.Values.ToArray());
			PInvokeAssert.True(Unsafe.AreSame(ref MemoryMarshal.GetReference(ctx.Bytes),
			                                  ref MemoryMarshal.GetReference(bctx.Values)));
			PInvokeAssert.True(Unsafe.AreSame(ref MemoryMarshal.GetReference(arr.AsSpan()),
			                                  ref MemoryMarshal.GetReference(ctx.Values)));

			ReadOnlyFixedContextValue<Byte> bctx2 = (ReadOnlyFixedContextValue<Byte>)(FixedPointerValue)ctx;
			PInvokeAssert.Equal(ctx.Bytes.ToArray(), bctx2.Bytes.ToArray());
			PInvokeAssert.True(Unsafe.AreSame(ref MemoryMarshal.GetReference(ctx.Bytes),
			                                  ref MemoryMarshal.GetReference(bctx2.Values)));
			PInvokeAssert.True(Unsafe.AreSame(ref MemoryMarshal.GetReference(MemoryMarshal.AsBytes(arr.AsSpan())),
			                                  ref MemoryMarshal.GetReference(bctx2.Values)));

			ReadOnlyFixedAction<T>.Test<Boolean>(ctx);
			ReadOnlyFixedAction<T>.Test<Byte>(ctx);
			ReadOnlyFixedAction<T>.Test<Char>(ctx);
			ReadOnlyFixedAction<T>.Test<DateTime>(ctx);
			ReadOnlyFixedAction<T>.Test<Decimal>(ctx);
			ReadOnlyFixedAction<T>.Test<Double>(ctx);
			ReadOnlyFixedAction<T>.Test<Guid>(ctx);
#if NET5_0_OR_GREATER
			ReadOnlyFixedAction<T>.Test<Half>(ctx);
#endif
			ReadOnlyFixedAction<T>.Test<Int16>(ctx);
			ReadOnlyFixedAction<T>.Test<Int32>(ctx);
			ReadOnlyFixedAction<T>.Test<Int64>(ctx);
			ReadOnlyFixedAction<T>.Test<SByte>(ctx);
			ReadOnlyFixedAction<T>.Test<Single>(ctx);
			ReadOnlyFixedAction<T>.Test<UInt16>(ctx);
			ReadOnlyFixedAction<T>.Test<UInt32>(ctx);
			ReadOnlyFixedAction<T>.Test<UInt64>(ctx);
		}
		private static unsafe void Test<T2>(ReadOnlyFixedContextValue<T> ctx) where T2 : unmanaged
		{
			ReadOnlyFixedContextValue<T2> ctx2 = ctx.Transformation<T2>(out FixedPointerValue residual);
			Int32 offset = ctx2.Values.Length * sizeof(T2);

			PInvokeAssert.Equal(ctx.Pointer, ctx2.Pointer);
			PInvokeAssert.Equal(ctx.Bytes.Length / sizeof(T2), ctx2.Values.Length);
			PInvokeAssert.Equal(ctx.Bytes.Length, ctx2.Bytes.Length);
			PInvokeAssert.Equal(ctx.Bytes.Length - offset, ((ReadOnlyFixedContextValue<Byte>)residual).Values.Length);
			PInvokeAssert.Equal(ctx.Pointer + offset, residual.Pointer);
		}
	}

	private readonly struct FixedFunction<T>(Array? array) : IFixedContextFunction<T, T[]?> where T : unmanaged
	{
		public T[]? Apply(scoped FixedContextValue<T> ctx)
		{
			new FixedAction<T>(array).Accept(ctx);
			return array is not null ? ctx.Values.ToArray() : default;
		}
	}

	private readonly struct ReadOnlyFixedFunction<T>(Array? array)
		: IReadOnlyFixedContextFunction<T, T[]?> where T : unmanaged
	{
		public T[]? Apply(scoped ReadOnlyFixedContextValue<T> ctx)
		{
			new ReadOnlyFixedAction<T>(array).Accept(ctx);
			return array is not null ? ctx.Values.ToArray() : default;
		}
	}
}