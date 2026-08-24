namespace Rxmxnx.PInvoke.Tests.NativeUtilitiesTests;

[TestFixture]
[ExcludeFromCodeCoverage]
[SuppressMessage("csharpsquid", "S2699")]
public sealed class HeapAllocTest
{
#pragma warning disable CS0612
	[Fact]
	public void BooleanTest()
	{
		HeapAllocTest.EmptyTest<Boolean>();
		HeapAllocTest.InvalidCountTest<Boolean>();
		HeapAllocTest.BasicTest(static i => i % 2 == 0);
	}
	[Fact]
	public void ByteTest()
	{
		HeapAllocTest.EmptyTest<Byte>();
		HeapAllocTest.InvalidCountTest<Byte>();
		HeapAllocTest.BasicTest(static i => (Byte)~i);
	}
	[Fact]
	public void CharTest()
	{
		HeapAllocTest.EmptyTest<Char>();
		HeapAllocTest.InvalidCountTest<Char>();
		HeapAllocTest.BasicTest(static i => (Char)(0x0100 + i));
	}
	[Fact]
	public void Int16Test()
	{
		HeapAllocTest.EmptyTest<UInt16>();
		HeapAllocTest.InvalidCountTest<UInt16>();
		HeapAllocTest.BasicTest(static i => i);
	}
	[Fact]
	public void Int32Test()
	{
		HeapAllocTest.EmptyTest<UInt16>();
		HeapAllocTest.InvalidCountTest<UInt16>();
		HeapAllocTest.BasicTest(static i => -i);
	}
	[Fact]
	public void Int64Test()
	{
		HeapAllocTest.EmptyTest<UInt16>();
		HeapAllocTest.InvalidCountTest<UInt16>();
		HeapAllocTest.BasicTest(static i => i ^ 0x7);
	}
#pragma warning restore CS0612

	[Obsolete]
	private static void EmptyTest<T>() where T : unmanaged
	{
#if NETSTANDARD2_1 && !LEGACY || NETCOREAPP3_0_OR_GREATER
		using (IFixedContext<T>.IDisposable ctx = NativeUtilities.HeapAlloc<T>(0))
		{
			PInvokeAssert.True(ctx.IsNullOrEmpty);
			PInvokeAssert.Equal(IntPtr.Zero, ctx.Pointer);
			PInvokeAssert.Equal(0, ctx.Values.Length);
		}
#endif
		using IDisposable _ = NativeUtilities.HeapAlloc(0, out FixedContextValue<T> ctxValue);
		PInvokeAssert.True(ctxValue.IsNullOrEmpty);
		PInvokeAssert.Equal(IntPtr.Zero, ctxValue.Pointer);
		PInvokeAssert.Equal(0, ctxValue.Values.Length);
	}
	[Obsolete]
	private static void InvalidCountTest<T>() where T : unmanaged
	{
#if NETSTANDARD2_1 && !LEGACY || NETCOREAPP3_0_OR_GREATER
		PInvokeAssert.Throws<ArgumentOutOfRangeException>(() => NativeUtilities.HeapAlloc<T>(-1));
#endif
		if (Unsafe.SizeOf<T>() == sizeof(Byte)) return;
#if NETSTANDARD2_1 && !LEGACY || NETCOREAPP3_0_OR_GREATER
		PInvokeAssert.Throws<OverflowException>(() => NativeUtilities.HeapAlloc<T>(Int32.MaxValue));
#endif
		PInvokeAssert.Throws<ArgumentOutOfRangeException>(() => NativeUtilities.HeapAlloc<T>(-1, out _));
	}
	[Obsolete]
	private static void BasicTest<T>(Func<Int32, T> factory) where T : unmanaged
	{
		Int32 count = PInvokeRandom.Shared.Next(1, Byte.MaxValue);
#if NETSTANDARD2_1 && !LEGACY || NETCOREAPP3_0_OR_GREATER
		HeapAllocTest.BasicDisposable(factory, count);
#endif
		HeapAllocTest.BasicValue(factory, count);
	}
	private static void BasicValue<T>(Func<Int32, T> factory, Int32 count) where T : unmanaged
	{
		IDisposable disposable = NativeUtilities.HeapAlloc(count, out FixedContextValue<T> ctx);
		List<T> list = [];
		try
		{
			PInvokeAssert.False(ctx.IsNullOrEmpty);
			PInvokeAssert.NotEqual(IntPtr.Zero, ctx.Pointer);
			PInvokeAssert.Equal(count, ctx.Values.Length);
			PInvokeAssert.Equal(count * NativeUtilities.SizeOf<T>(), ctx.Bytes.Length);
			for (Int32 i = 0; i < ctx.Values.Length; i++)
			{
				T newValue = factory(i);
				ctx.Values[i] = newValue;
				list.Add(newValue);
			}
			PInvokeAssert.Equal(list, ctx.Values.ToArray());
		}
		finally
		{
			disposable.Dispose();
		}

		// ReSharper disable once AccessToDisposedClosure
#pragma warning disable CS8500
		ref FixedContextValue<T> refCtx = ref ctx;
		unsafe
		{
			fixed (void* ptr = &refCtx)
			{
				IntPtr intPtr = new(ptr);
				PInvokeAssert.Throws<InvalidOperationException>(() => _ = ((FixedContextValue<T>*)intPtr.ToPointer())[0]
				                                                          .Values.Length);
			}
		}
#pragma warning restore CS8500
		disposable.Dispose();
	}
#if NETSTANDARD2_1 && !LEGACY || NETCOREAPP3_0_OR_GREATER
	[Obsolete]
	private static void BasicDisposable<T>(Func<Int32, T> factory, Int32 count) where T : unmanaged
	{
		IFixedContext<T>.IDisposable ctx = NativeUtilities.HeapAlloc<T>(count);
		List<T> list = [];
		try
		{
			PInvokeAssert.False(ctx.IsNullOrEmpty);
			PInvokeAssert.NotEqual(IntPtr.Zero, ctx.Pointer);
			PInvokeAssert.Equal(count, ctx.Values.Length);
			PInvokeAssert.Equal(count * NativeUtilities.SizeOf<T>(), ctx.Bytes.Length);
			for (Int32 i = 0; i < ctx.Values.Length; i++)
			{
				T newValue = factory(i);
				ctx.Values[i] = newValue;
				list.Add(newValue);
			}
			PInvokeAssert.Equal(list, ctx.Values.ToArray());
		}
		finally
		{
			ctx.Dispose();
		}

		// ReSharper disable once AccessToDisposedClosure
		PInvokeAssert.Throws<InvalidOperationException>(() => _ = ctx.Values.Length);
		ctx.Dispose();
	}
#endif
}