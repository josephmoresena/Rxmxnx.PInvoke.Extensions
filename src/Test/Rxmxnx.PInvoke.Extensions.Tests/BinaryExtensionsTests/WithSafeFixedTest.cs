namespace Rxmxnx.PInvoke.Tests.BinaryExtensionsTests;

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
		T[] values = WithSafeFixedTest.fixture.CreateMany<T>(10).ToArray();
		Span<Byte> span = MemoryMarshal.AsBytes(values.AsSpan());
		ReadOnlySpan<Byte> readOnlySpan = span;

		this._array = values;
#if NETSTANDARD2_1 && !LEGACY || NETCOREAPP3_0_OR_GREATER
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
#endif
		span.WithSafeFixed(new FixedAction<T>(this._array, false));
		span.WithSafeFixed(new FixedAction<T>(this._array, true));
		readOnlySpan.WithSafeFixed(new FixedAction<T>(this._array, true));
		span.WithSafeFixed(new FixedFunction<T>(this._array, false), out Byte[] result);
		PInvokeAssert.True(span.SequenceEqual(result.AsSpan()));
		span.WithSafeFixed(new FixedFunction<T>(this._array, true), out result);
		PInvokeAssert.True(span.SequenceEqual(result.AsSpan()));
		readOnlySpan.WithSafeFixed(new FixedFunction<T>(this._array, true), out result);
		PInvokeAssert.True(readOnlySpan.SequenceEqual(result.AsSpan()));
	}
#if NETSTANDARD2_1 && !LEGACY || NETCOREAPP3_0_OR_GREATER
	[Obsolete]
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
#pragma warning disable CS0612
	private void ActionReadOnlyTest<T>(in IReadOnlyFixedMemory mem) where T : unmanaged
		=> this.ActionReadOnlyTest<T>(mem, false);
	private void ReadOnlyActionReadOnlyTest<T>(in IReadOnlyFixedMemory mem) where T : unmanaged
		=> this.ActionReadOnlyTest<T>(mem, true);
#pragma warning restore CS0612
	[Obsolete]
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
#pragma warning disable CS0612
		this.ActionTest<T>(mem);
#pragma warning restore CS0612
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
#pragma warning disable CS0612
	private static void ActionTest<T>(in IFixedMemory mem, WithSafeFixedTest test) where T : unmanaged
		=> test.ActionTest<T>(mem);
#pragma warning restore CS0612
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
#endif
	private readonly unsafe struct FixedAction<T>(Array array, Boolean isReadOnly) : IFixedAction where T : unmanaged
	{
		public void Accept(scoped FixedPointerValue fptr)
		{
			if (isReadOnly)
			{
				this.AcceptReadOnly(fptr);
				return;
			}
			PInvokeAssert.Equal(!fptr.IsNullOrEmpty, fptr.TryGetBinaryContext(out FixedContextValue<Byte> bctx));
			FixedContextValue<T> ctx = bctx.Transformation<T>(out _);
			T[] arr = (T[])array;

			PInvokeAssert.Equal(fptr.Pointer, bctx.Pointer);
			PInvokeAssert.Equal(fptr.Pointer, ctx.ValuePointer.Pointer);
			PInvokeAssert.True(fptr == ctx);
			PInvokeAssert.Equal(bctx.Bytes.ToArray(), ctx.Bytes.ToArray());
			PInvokeAssert.Equal(arr, ctx.Values.ToArray());
			PInvokeAssert.True(Unsafe.AreSame(ref MemoryMarshal.GetReference(bctx.Bytes),
			                                  ref MemoryMarshal.GetReference(bctx.Values)));
			PInvokeAssert.True(Unsafe.AreSame(ref MemoryMarshal.GetReference(arr.AsSpan()),
			                                  ref MemoryMarshal.GetReference(ctx.Values)));

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

		private void AcceptReadOnly(FixedPointerValue fptr)
		{
			PInvokeAssert.True(fptr.TryGetReadOnlyBinaryContext(out ReadOnlyFixedContextValue<Byte> bctx));
			ReadOnlyFixedContextValue<T> ctx = bctx.Transformation<T>(out _);
			T[] arr = (T[])array;
			PInvokeAssert.Equal(fptr.Pointer, bctx.Pointer);
			PInvokeAssert.Equal(fptr.Pointer, ctx.Pointer);
			PInvokeAssert.True(fptr == ctx);
			PInvokeAssert.Equal(bctx.Bytes.ToArray(), ctx.Bytes.ToArray());
			PInvokeAssert.Equal(arr, ctx.Values.ToArray());
			PInvokeAssert.True(Unsafe.AreSame(ref Unsafe.AsRef<Byte>(fptr.Pointer.ToPointer()),
			                                  ref MemoryMarshal.GetReference(bctx.Values)));
			PInvokeAssert.True(Unsafe.AreSame(ref MemoryMarshal.GetReference(arr.AsSpan()),
			                                  ref MemoryMarshal.GetReference(ctx.Values)));
			if (!((FixedPointerValue)ctx).IsReadOnly)
			{
				FixedContextValue<T> ctx2 = (FixedContextValue<T>)ctx;
				PInvokeAssert.Equal(ctx.Pointer, ctx2.Pointer);
				PInvokeAssert.Equal(ctx.ValuePointer, ctx2.ValuePointer);
				PInvokeAssert.Equal(ctx.Values.Length, ctx2.Values.Length);
				PInvokeAssert.True(Unsafe.AreSame(ref MemoryMarshal.GetReference(ctx.Values),
				                                  ref MemoryMarshal.GetReference(ctx2.Values)));
			}

			FixedAction<T>.ReadOnlyTest<Boolean>(ctx);
			FixedAction<T>.ReadOnlyTest<Byte>(ctx);
			FixedAction<T>.ReadOnlyTest<Char>(ctx);
			FixedAction<T>.ReadOnlyTest<DateTime>(ctx);
			FixedAction<T>.ReadOnlyTest<Decimal>(ctx);
			FixedAction<T>.ReadOnlyTest<Double>(ctx);
			FixedAction<T>.ReadOnlyTest<Guid>(ctx);
#if NET5_0_OR_GREATER
			FixedAction<T>.ReadOnlyTest<Half>(ctx);
#endif
			FixedAction<T>.ReadOnlyTest<Int16>(ctx);
			FixedAction<T>.ReadOnlyTest<Int32>(ctx);
			FixedAction<T>.ReadOnlyTest<Int64>(ctx);
			FixedAction<T>.ReadOnlyTest<SByte>(ctx);
			FixedAction<T>.ReadOnlyTest<Single>(ctx);
			FixedAction<T>.ReadOnlyTest<UInt16>(ctx);
			FixedAction<T>.ReadOnlyTest<UInt32>(ctx);
			FixedAction<T>.ReadOnlyTest<UInt64>(ctx);
		}
		private static void ReadOnlyTest<T2>(ReadOnlyFixedContextValue<T> ctx) where T2 : unmanaged
		{
			ReadOnlyFixedContextValue<T2> ctx2 = ctx.Transformation<T2>(out FixedPointerValue residual);
			Int32 offset = ctx2.Values.Length * sizeof(T2);

			PInvokeAssert.Equal(ctx.Pointer, ctx2.Pointer);
			PInvokeAssert.Equal(ctx.Bytes.Length / sizeof(T2), ctx2.Values.Length);
			PInvokeAssert.Equal(ctx.Bytes.Length, ctx2.Bytes.Length);
			PInvokeAssert.True(residual.TryGetReadOnlyBinaryContext(out ReadOnlyFixedContextValue<Byte> rb));
			PInvokeAssert.Equal(ctx.Bytes.Length - offset, rb.Bytes.Length);
			PInvokeAssert.Equal(ctx.Pointer + offset, residual.Pointer);
			PInvokeAssert.Equal(!residual.IsReadOnly, residual.TryGetBinaryContext(out _));
		}
		private static void Test<T2>(FixedContextValue<T> ctx) where T2 : unmanaged
		{
			FixedContextValue<T2> ctx2 = ctx.Transformation<T2>(out FixedPointerValue residual);
			Int32 offset = ctx2.Values.Length * sizeof(T2);

			PInvokeAssert.Equal(ctx.Pointer, ctx2.Pointer);
			PInvokeAssert.Equal(ctx.Bytes.Length / sizeof(T2), ctx2.Values.Length);
			PInvokeAssert.Equal(ctx.Bytes.Length, ctx2.Bytes.Length);
			PInvokeAssert.True(residual.TryGetBinaryContext(out FixedContextValue<Byte> rb));
			PInvokeAssert.Equal(ctx.Bytes.Length - offset, rb.Bytes.Length);
			PInvokeAssert.Equal(ctx.Pointer + offset, residual.Pointer);
		}
	}

	private readonly struct FixedFunction<T>(Array array, Boolean isReadOnly) : IFixedFunction<Byte[]>
		where T : unmanaged
	{
		public Byte[] Apply(scoped FixedPointerValue fptr)
		{
			new FixedAction<T>(array, isReadOnly).Accept(fptr);
			fptr.TryGetReadOnlyBinaryContext(out ReadOnlyFixedContextValue<Byte> ctx);
			return ctx.Values.ToArray();
		}
	}
}