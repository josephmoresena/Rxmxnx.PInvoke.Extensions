namespace Rxmxnx.PInvoke.Tests.NativeUtilitiesTests;

[TestFixture]
[ExcludeFromCodeCoverage]
[SuppressMessage("csharpsquid", "S2699")]
public sealed class HeapAllocTest
{
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

	private static void EmptyTest<T>() where T : unmanaged
	{
		using IFixedContext<T>.IDisposable ctx = NativeUtilities.HeapAlloc<T>(0);

		PInvokeAssert.True(ctx.IsNullOrEmpty);
		PInvokeAssert.Equal(IntPtr.Zero, ctx.Pointer);
		PInvokeAssert.Equal(0, ctx.Values.Length);
	}
	private static void InvalidCountTest<T>() where T : unmanaged
	{
		PInvokeAssert.Throws<ArgumentOutOfRangeException>(() => NativeUtilities.HeapAlloc<T>(-1));
		if (Unsafe.SizeOf<T>() == sizeof(Byte)) return;
		PInvokeAssert.Throws<OverflowException>(() => NativeUtilities.HeapAlloc<T>(Int32.MaxValue));
	}
	private static void BasicTest<T>(Func<Int32, T> factory) where T : unmanaged
	{
		Int32 count = PInvokeRandom.Shared.Next(0, Byte.MaxValue);
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
}