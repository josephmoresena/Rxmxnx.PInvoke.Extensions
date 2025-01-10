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
		ValPtr<IntPtr> ptrPtr = NativeUtilities.GetUnsafeValPtrFromRef(ref span0[0]);
		BufferManager.Alloc<T>(1, UnitAllocTest.Do);
		BufferManager.Alloc<T, ValPtr<IntPtr>>(1, ptrPtr, UnitAllocTest.Do);
		IntPtr addOfT = BufferManager.Alloc<T, IntPtr>(1, UnitAllocTest.Get);
		Assert.Equal(default, BufferManager.Alloc<T, ValPtr<IntPtr>, T>(1, ptrPtr, UnitAllocTest.Get));

		Assert.True(Math.Abs(addOfT.ToInt64() - span0[0].ToInt64()) < 0x10000);
	}
	private static void Do<T>(ScopedBuffer<T> buffer)
	{
		Assert.True(buffer.InStack);
		Assert.Equal(1, buffer.Span.Length);
		Assert.Equal(1, buffer.FullLength);
		Assert.Equal(default, buffer.Span[0]);
		if (!RuntimeHelpers.IsReferenceOrContainsReferences<T>())
		{
			Assert.Null(buffer.BufferMetadata);
			return;
		}
		Assert.NotNull(buffer.BufferMetadata);
		Assert.Equal(typeof(T).IsValueType ? typeof(Atomic<T>) : typeof(Atomic<Object>),
		             buffer.BufferMetadata.BufferType);
		Assert.True(buffer.BufferMetadata.IsBinary);
		Assert.Equal(1, buffer.BufferMetadata.Size);
		Assert.Equal(0, buffer.BufferMetadata.ComponentCount);
	}
	private static void Do<T>(ScopedBuffer<T> buffer, ValPtr<IntPtr> ptrPtr)
	{
		UnitAllocTest.Do(buffer);
		Assert.True(ptrPtr.Pointer == ptrPtr.Reference);
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