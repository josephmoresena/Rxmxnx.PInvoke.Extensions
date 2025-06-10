namespace Rxmxnx.PInvoke.Tests.Buffers;

[ExcludeFromCodeCoverage]
[SuppressMessage("csharpsquid", "S2699")]
public sealed class NonBinarySpaceTests
{
	[Fact]
	internal void InvalidTest()
	{
		Assert.IsType<InvalidOperationException>(
			Assert.Throws<TypeInitializationException>(NonBinarySpaceTests
				                                           .GetMetadata<NonBinarySpace<InternalStruct<Object>, Int32>,
					                                           Int32>).InnerException);
		Assert.IsType<InvalidOperationException>(
			Assert.Throws<TypeInitializationException>(BufferManager
				                                           .Register<NonBinarySpace<InternalStruct<Int32>, Object>>)
			      .InnerException);
		Assert.IsType<InvalidOperationException>(Assert.Throws<TypeInitializationException>(
			                                         BufferManager
				                                         .Register<WrapperStruct<Object>,
					                                         NonBinarySpace<InternalStruct<Int32>,
						                                         WrapperStruct<Object>>>).InnerException);
	}

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

		BufferTypeMetadata<WrapperStruct<WrapperStruct<WrapperStruct<T>>>> atomicMetadata = NonBinarySpaceTests
			.GetMetadata<Atomic<WrapperStruct<WrapperStruct<WrapperStruct<T>>>>,
				WrapperStruct<WrapperStruct<WrapperStruct<T>>>>();
		BufferTypeMetadata<WrapperStruct<WrapperStruct<WrapperStruct<T>>>> typeMetadata = NonBinarySpaceTests
			.GetMetadata<NonBinarySpace<NonBinaryBuffer<T>, WrapperStruct<WrapperStruct<WrapperStruct<T>>>>,
				WrapperStruct<WrapperStruct<WrapperStruct<T>>>>();

		Assert.Empty(typeMetadata);
		Assert.Empty(typeMetadata.Components);
		Assert.False(typeMetadata.IsBinary);
		Assert.Equal(100, typeMetadata.Size);
		Assert.Null(typeMetadata.Double());
		Assert.Null(typeMetadata.Compose(atomicMetadata));
		Assert.Null(atomicMetadata.Compose(typeMetadata));
		Assert.Equal(typeof(NonBinarySpace<NonBinaryBuffer<T>, WrapperStruct<WrapperStruct<WrapperStruct<T>>>>),
		             typeMetadata.BufferType);
		Assert.Equal(atomicMetadata,
		             BufferManager.MetadataManager<WrapperStruct<WrapperStruct<WrapperStruct<T>>>>.GetMetadata(
			             atomicMetadata.BufferType));
		Assert.Equal(typeMetadata,
		             BufferManager.MetadataManager<WrapperStruct<WrapperStruct<WrapperStruct<T>>>>.GetMetadata(
			             typeMetadata.BufferType));

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
		if (!RuntimeHelpers.IsReferenceOrContainsReferences<T>())
		{
			Assert.Null(buffer.BufferMetadata);
			return;
		}

		Assert.NotNull(buffer.BufferMetadata);
		Assert.False(buffer.BufferMetadata.IsBinary);
		Assert.Equal(buffer.Span.Length, buffer.BufferMetadata.Size);
		Assert.Equal(0, buffer.BufferMetadata.ComponentCount);
		Assert.Equal(typeof(NonBinarySpace<NonBinaryBuffer<T>, WrapperStruct<WrapperStruct<WrapperStruct<T>>>>),
		             buffer.BufferMetadata.BufferType);
		Assert.Equal(buffer.BufferMetadata,
		             BufferManager.MetadataManager<WrapperStruct<WrapperStruct<WrapperStruct<T>>>>.GetMetadata(
			             buffer.BufferMetadata.BufferType));
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
	private static BufferTypeMetadata<T> GetMetadata<TBuffer, T>() where TBuffer : struct, IManagedBuffer<T>
	{
		const BindingFlags getMetadataFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;
		try
		{
			Type typeofInterface = typeof(IManagedBuffer<T>);
			MethodInfo? getMetadata =
				typeofInterface.GetMethod(nameof(BufferManager.MetadataManager<T>.GetMetadata), getMetadataFlags);
			if (getMetadata is not null)
				return (BufferTypeMetadata<T>)getMetadata.MakeGenericMethod(typeof(TBuffer)).Invoke(null, [])!;
			return (BufferTypeMetadata<T>)typeof(TBuffer).GetField(nameof(NonBinarySpace<Int32, Int32>.TypeMetadata),
			                                                       getMetadataFlags)!.GetValue(null)!;
		}
		catch (TargetInvocationException tie)
		{
			if (tie.InnerException is not null)
				throw tie.InnerException;
			throw;
		}
	}

	[InlineArray(10)]
	private struct InternalStruct<T>
	{
		private T _val;
	}

	[InlineArray(100)]
	private struct NonBinaryBuffer<T>
	{
		private T _val;
	}
}