namespace Rxmxnx.PInvoke.Tests;

[ExcludeFromCodeCoverage]
[SuppressMessage("csharpsquid", "S2699")]
public sealed unsafe class UnitAllocTest
{
	[Fact]
	internal void BooleanTest() => UnitAllocTest.UnitAlloc<Boolean>();
	[Fact]
	internal void ByteTest() => UnitAllocTest.UnitAlloc<Byte>();
	[Fact]
	internal void Int16Test() => UnitAllocTest.UnitAlloc<Int16>();
	[Fact]
	internal void Int32Test() => UnitAllocTest.UnitAlloc<Int32>();
	[Fact]
	internal void Int64Test() => UnitAllocTest.UnitAlloc<Int64>();

	[Fact]
	internal void NullableBooleanTest() => UnitAllocTest.UnitAlloc<Boolean?>();
	[Fact]
	internal void NullableByteTest() => UnitAllocTest.UnitAlloc<Byte?>();
	[Fact]
	internal void NullableInt16Test() => UnitAllocTest.UnitAlloc<Int16?>();
	[Fact]
	internal void NullableInt32Test() => UnitAllocTest.UnitAlloc<Int32?>();
	[Fact]
	internal void NullableInt64Test() => UnitAllocTest.UnitAlloc<Int64?>();

	[Fact]
	internal void BooleanArrayTest() => UnitAllocTest.UnitAlloc<Boolean[]?>();
	[Fact]
	internal void ByteArrayTest() => UnitAllocTest.UnitAlloc<Byte[]?>();
	[Fact]
	internal void Int16ArrayTest() => UnitAllocTest.UnitAlloc<Int16[]?>();
	[Fact]
	internal void Int32ArrayTest() => UnitAllocTest.UnitAlloc<Int32[]?>();
	[Fact]
	internal void Int64ArrayTest() => UnitAllocTest.UnitAlloc<Int64[]?>();
	[Fact]
	internal void StringTest() => UnitAllocTest.UnitAlloc<String?>();

	private static void UnitAlloc<T>()
	{
		Span<IntPtr> span0 = stackalloc IntPtr[5];
		span0[0] = (IntPtr)Unsafe.AsPointer(ref MemoryMarshal.GetReference(span0));
		BufferManager.Alloc<T>(1, UnitAllocTest.Do);
		BufferManager.Alloc<T, IntPtr>(1, in span0[0], UnitAllocTest.Do);
		IntPtr addOfT = BufferManager.Alloc<T, IntPtr>(1, UnitAllocTest.Get);
		Assert.Equal(default, BufferManager.Alloc<T, IntPtr, T>(1, in span0[0], UnitAllocTest.Get));

		Assert.True(Math.Abs(addOfT.ToInt64() - span0[0].ToInt64()) < 0x10000);
	}
	private static void Do<T>(ScopedBuffer<T> buffer)
	{
		Assert.True(buffer.InStack);
		Assert.Equal(1, buffer.Span.Length);
		Assert.Equal(1, buffer.FullLength);
		Assert.Equal(default, buffer.Span[0]);
	}
	private static void Do<T>(ScopedBuffer<T> buffer, in IntPtr ptr)
	{
		Assert.True(buffer.InStack);
		Assert.Equal(1, buffer.Span.Length);
		Assert.Equal(1, buffer.FullLength);
		Assert.Equal(default, buffer.Span[0]);
		Assert.True(Unsafe.AsPointer(ref UnsafeLegacy.AsRef(in ptr)) == ptr.ToPointer());
	}
	private static IntPtr Get<T>(ScopedBuffer<T> buffer)
	{
		Assert.True(buffer.InStack);
		Assert.Equal(1, buffer.Span.Length);
		Assert.Equal(1, buffer.FullLength);
		Assert.Equal(default, buffer.Span[0]);
		return (IntPtr)Unsafe.AsPointer(ref buffer.Span[0]);
	}
	private static T Get<T>(ScopedBuffer<T> buffer, in IntPtr ptr)
	{
		UnitAllocTest.Do(buffer, in ptr);
		return buffer.Span[0];
	}
}