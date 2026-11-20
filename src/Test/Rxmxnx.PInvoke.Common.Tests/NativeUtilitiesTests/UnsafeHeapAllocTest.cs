namespace Rxmxnx.PInvoke.Tests.NativeUtilitiesTests;

[TestFixture]
[ExcludeFromCodeCoverage]
[SuppressMessage("csharpsquid", "S2699")]
public sealed class UnsafeHeapAlloc
{
	[Obsolete]
	private static void EmptyTest<T>() where T : unmanaged
	{
		using IDisposable _ = NativeUtilities.UnsafeHeapAlloc(0, out ValPtr<T> ptr);
		PInvokeAssert.Same(_, ptr.GetUnsafeFixedContext(0, _, out FixedContextValue<T> ctxValue));
		PInvokeAssert.True(ctxValue.IsNullOrEmpty);
		PInvokeAssert.Equal(IntPtr.Zero, ctxValue.Pointer);
		PInvokeAssert.Equal(0, ctxValue.Values.Length);
	}
	[Obsolete]
	private static void BasicTest<T>(Func<Int32, T> factory) where T : unmanaged
	{
		Int32 count = PInvokeRandom.Shared.Next(1, Byte.MaxValue);
		UnsafeHeapAlloc.BasicValue(factory, count);
	}
	private static void BasicValue<T>(Func<Int32, T> factory, Int32 count) where T : unmanaged
	{
		IDisposable disposable = NativeUtilities.UnsafeHeapAlloc(count, out ValPtr<T> valPtr);
		PInvokeAssert.Same(disposable, valPtr.GetUnsafeFixedContext(count, disposable, out FixedContextValue<T> ctx));
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
#pragma warning disable CS0612
	[Fact]
	public void BooleanTest()
	{
		UnsafeHeapAlloc.EmptyTest<Boolean>();
		UnsafeHeapAlloc.BasicTest(static i => i % 2 == 0);
	}
	[Fact]
	public void ByteTest()
	{
		UnsafeHeapAlloc.EmptyTest<Byte>();
		UnsafeHeapAlloc.BasicTest(static i => (Byte)~i);
	}
	[Fact]
	public void CharTest()
	{
		UnsafeHeapAlloc.EmptyTest<Char>();
		UnsafeHeapAlloc.BasicTest(static i => (Char)(0x0100 + i));
	}
	[Fact]
	public void Int16Test()
	{
		UnsafeHeapAlloc.EmptyTest<UInt16>();
		UnsafeHeapAlloc.BasicTest(static i => i);
	}
	[Fact]
	public void Int32Test()
	{
		UnsafeHeapAlloc.EmptyTest<UInt16>();
		UnsafeHeapAlloc.BasicTest(static i => -i);
	}
	[Fact]
	public void Int64Test()
	{
		UnsafeHeapAlloc.EmptyTest<UInt16>();
		UnsafeHeapAlloc.BasicTest(static i => i ^ 0x7);
	}
#pragma warning restore CS0612
}