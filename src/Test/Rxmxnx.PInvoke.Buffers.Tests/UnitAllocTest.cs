namespace Rxmxnx.PInvoke.Tests;

[TestFixture]
[ExcludeFromCodeCoverage]
[SuppressMessage("csharpsquid", "S2699")]
public sealed unsafe class UnitAllocTest
{
	[Fact]
	public void BooleanTest() => UnitAllocTest.UnitAlloc<Boolean>();
	[Fact]
	public void ByteTest() => UnitAllocTest.UnitAlloc<Byte>();
	[Fact]
	public void Int16Test() => UnitAllocTest.UnitAlloc<Int16>();
	[Fact]
	public void Int32Test() => UnitAllocTest.UnitAlloc<Int32>();
	[Fact]
	public void Int64Test() => UnitAllocTest.UnitAlloc<Int64>();

	[Fact]
	public void NullableBooleanTest() => UnitAllocTest.UnitAlloc<Boolean?>();
	[Fact]
	public void NullableByteTest() => UnitAllocTest.UnitAlloc<Byte?>();
	[Fact]
	public void NullableInt16Test() => UnitAllocTest.UnitAlloc<Int16?>();
	[Fact]
	public void NullableInt32Test() => UnitAllocTest.UnitAlloc<Int32?>();
	[Fact]
	public void NullableInt64Test() => UnitAllocTest.UnitAlloc<Int64?>();

	[Fact]
	public void BooleanArrayTest() => UnitAllocTest.UnitAlloc<Boolean[]?>();
	[Fact]
	public void ByteArrayTest() => UnitAllocTest.UnitAlloc<Byte[]?>();
	[Fact]
	public void Int16ArrayTest() => UnitAllocTest.UnitAlloc<Int16[]?>();
	[Fact]
	public void Int32ArrayTest() => UnitAllocTest.UnitAlloc<Int32[]?>();
	[Fact]
	public void Int64ArrayTest() => UnitAllocTest.UnitAlloc<Int64[]?>();
	[Fact]
	public void StringTest() => UnitAllocTest.UnitAlloc<String?>();

	private static void UnitAlloc<T>()
	{
		Span<IntPtr> span0 = stackalloc IntPtr[5];
		span0[0] = (IntPtr)Unsafe.AsPointer(ref MemoryMarshal.GetReference(span0));
		ValPtr<IntPtr> ptrPtr = NativeUtilities.GetUnsafeValPtrFromRef(ref span0[0]);
		BufferManager.Alloc<T>(1, UnitAllocTest.Do);
		BufferManager.Alloc<T, ValPtr<IntPtr>>(1, ptrPtr, UnitAllocTest.Do);
		IntPtr addOfT = BufferManager.Alloc<T, IntPtr>(1, UnitAllocTest.Get);
		PInvokeAssert.Equal(default, BufferManager.Alloc<T, ValPtr<IntPtr>, T>(1, ptrPtr, UnitAllocTest.Get));

		PInvokeAssert.True(Math.Abs(addOfT.ToInt64() - span0[0].ToInt64()) < 0x10000);
	}
	private static void Do<T>(ScopedBuffer<T> buffer)
	{
		PInvokeAssert.True(buffer.InStack);
		PInvokeAssert.Equal(1, buffer.Span.Length);
		PInvokeAssert.Equal(1, buffer.FullLength);
		PInvokeAssert.Equal(default, buffer.Span[0]);
		if (!RuntimeHelpers.IsReferenceOrContainsReferences<T>())
		{
			PInvokeAssert.Null(buffer.BufferMetadata);
			return;
		}
		PInvokeAssert.NotNull(buffer.BufferMetadata);
		PInvokeAssert.Equal(typeof(T).IsValueType ? typeof(Atomic<T>) : typeof(Atomic<Object>),
		                    buffer.BufferMetadata.BufferType);
		PInvokeAssert.True(buffer.BufferMetadata.IsBinary);
		PInvokeAssert.Equal(1, buffer.BufferMetadata.Size);
		PInvokeAssert.Equal(0, buffer.BufferMetadata.ComponentCount);
	}
	private static void Do<T>(ScopedBuffer<T> buffer, ValPtr<IntPtr> ptrPtr)
	{
		UnitAllocTest.Do(buffer);
		PInvokeAssert.True(ptrPtr.Pointer == ptrPtr.Reference);
	}
	private static IntPtr Get<T>(ScopedBuffer<T> buffer)
	{
		UnitAllocTest.Do(buffer);
		return (IntPtr)Unsafe.AsPointer(ref buffer.Span[0]);
	}
	private static T Get<T>(ScopedBuffer<T> buffer, ValPtr<IntPtr> ptrPtr)
	{
		UnitAllocTest.Do(buffer, ptrPtr);
		return buffer.Span[0];
	}
}