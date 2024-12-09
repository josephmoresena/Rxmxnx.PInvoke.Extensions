namespace Rxmxnx.PInvoke.Tests.Buffers;

[ExcludeFromCodeCoverage]
[SuppressMessage("csharpsquid", "S2699")]
public sealed class NonBinarySpaceTests
{
	[Fact]
	internal void BooleanTest() => NonBinarySpaceTests.Test<Boolean>();
	[Fact]
	internal void ByteTest() => NonBinarySpaceTests.Test<Byte>();
	[Fact]
	internal void Int16Test() => NonBinarySpaceTests.Test<Int16>();
	[Fact]
	internal void Int32Test() => NonBinarySpaceTests.Test<Int32>();
	[Fact]
	internal void Int64Test() => NonBinarySpaceTests.Test<Int64>();
	[Fact]
	internal void StringWrapperTest() => NonBinarySpaceTests.Test<WrapperStruct<String?>>();

	private static unsafe void Test<T>() where T : struct
	{
		BufferManager
			.Register<WrapperStruct<WrapperStruct<WrapperStruct<T>>>, NonBinarySpace<NonBinaryBuffer<T>,
				WrapperStruct<WrapperStruct<WrapperStruct<T>>>>>();

		BufferTypeMetadata<WrapperStruct<WrapperStruct<WrapperStruct<T>>>> atomicMetadata =
			IManagedBuffer<WrapperStruct<WrapperStruct<WrapperStruct<T>>>>
				.GetMetadata<Atomic<WrapperStruct<WrapperStruct<WrapperStruct<T>>>>>();
		BufferTypeMetadata<WrapperStruct<WrapperStruct<WrapperStruct<T>>>> typeMetadata =
			IManagedBuffer<WrapperStruct<WrapperStruct<WrapperStruct<T>>>>
				.GetMetadata<NonBinarySpace<NonBinaryBuffer<T>, WrapperStruct<WrapperStruct<WrapperStruct<T>>>>>();

		Assert.Empty(typeMetadata);
		Assert.Empty(typeMetadata.Components);
		Assert.False(typeMetadata.IsBinary);
		Assert.Equal(100, typeMetadata.Size);
		Assert.Null(typeMetadata.Double());
		Assert.Null(typeMetadata.Compose(atomicMetadata));
		Assert.Null(atomicMetadata.Compose(typeMetadata));
		Assert.Equal(typeof(NonBinarySpace<NonBinaryBuffer<T>, WrapperStruct<WrapperStruct<WrapperStruct<T>>>>),
		             typeMetadata.BufferType);

		Span<IntPtr> span0 = stackalloc IntPtr[5];
		span0[0] = (IntPtr)Unsafe.AsPointer(ref MemoryMarshal.GetReference(span0));
		ValPtr<IntPtr> ptrPtr = NativeUtilities.GetUnsafeValPtrFromRef(ref span0[0]);
		BufferManager.Alloc<WrapperStruct<WrapperStruct<WrapperStruct<T>>>>(100, NonBinarySpaceTests.Do);
		BufferManager.Alloc<WrapperStruct<WrapperStruct<WrapperStruct<T>>>, ValPtr<IntPtr>>(
			100, ptrPtr, NonBinarySpaceTests.Do);
		Boolean inStack =
			BufferManager.Alloc<WrapperStruct<WrapperStruct<WrapperStruct<T>>>, Boolean>(100, NonBinarySpaceTests.Get);
		Assert.Equal(
			default,
			BufferManager.Alloc<WrapperStruct<WrapperStruct<WrapperStruct<T>>>, ValPtr<IntPtr>, T>(
				100, ptrPtr, NonBinarySpaceTests.Get));
		Assert.True(inStack);
	}

	private static void Do<T>(ScopedBuffer<WrapperStruct<WrapperStruct<WrapperStruct<T>>>> buffer)
	{
		Assert.True(buffer.InStack);
		Assert.Equal(100, buffer.Span.Length);
		Assert.Equal(buffer.FullLength, buffer.Span.Length);
		Assert.Equal(default, buffer.Span[0]);
		Assert.NotNull(buffer.BufferMetadata);
		Assert.False(buffer.BufferMetadata.IsBinary);
		Assert.Equal(buffer.Span.Length, buffer.BufferMetadata.Size);
		Assert.Equal(0, buffer.BufferMetadata.ComponentCount);
		Assert.Equal(typeof(NonBinarySpace<NonBinaryBuffer<T>, WrapperStruct<WrapperStruct<WrapperStruct<T>>>>),
		             buffer.BufferMetadata.BufferType);
	}
	private static void Do<T>(ScopedBuffer<WrapperStruct<WrapperStruct<WrapperStruct<T>>>> buffer,
		ValPtr<IntPtr> ptrPtr)
	{
		NonBinarySpaceTests.Do(buffer);
		Assert.True(ptrPtr.Pointer == ptrPtr.Reference);
	}
	private static Boolean Get<T>(ScopedBuffer<WrapperStruct<WrapperStruct<WrapperStruct<T>>>> buffer)
	{
		NonBinarySpaceTests.Do(buffer);
		return buffer.InStack;
	}
	private static T Get<T>(ScopedBuffer<WrapperStruct<WrapperStruct<WrapperStruct<T>>>> buffer, ValPtr<IntPtr> ptrPtr)
	{
		NonBinarySpaceTests.Do(buffer, ptrPtr);
		return buffer.Span[0].Value.Value.Value;
	}

	[InlineArray(100)]
	private struct NonBinaryBuffer<T>
	{
		private T _val;
	}
}