#if !NETSTANDARD2_1 && !NETCOREAPP2_0_OR_GREATER
using RuntimeHelpers = Rxmxnx.PInvoke.Internal.FrameworkCompat.RuntimeHelpersCompat;
#endif

namespace Rxmxnx.PInvoke.Tests;

[TestFixture]
[ExcludeFromCodeCoverage]
[SuppressMessage("csharpsquid", "S2699")]
public sealed unsafe class MultipleAllocTest
{
#pragma warning disable CS0612
	[Fact]
	public void BooleanTest() => MultipleAllocTest.MultipleAlloc<Boolean>();
	[Fact]
	public void ByteTest() => MultipleAllocTest.MultipleAlloc<Byte>();
	[Fact]
	public void Int16Test() => MultipleAllocTest.MultipleAlloc<Int16>();
	[Fact]
	public void Int32Test() => MultipleAllocTest.MultipleAlloc<Int32>();
	[Fact]
	public void Int64Test() => MultipleAllocTest.MultipleAlloc<Int64>();

	[Fact]
	public void NullableBooleanTest() => MultipleAllocTest.MultipleAlloc<Boolean?>();
	[Fact]
	public void NullableByteTest() => MultipleAllocTest.MultipleAlloc<Byte?>();
	[Fact]
	public void NullableInt16Test() => MultipleAllocTest.MultipleAlloc<Int16?>();
	[Fact]
	public void NullableInt32Test() => MultipleAllocTest.MultipleAlloc<Int32?>();
	[Fact]
	public void NullableInt64Test() => MultipleAllocTest.MultipleAlloc<Int64?>();

	[Fact]
	public void BooleanArrayTest() => MultipleAllocTest.MultipleAlloc<Boolean[]?>();
	[Fact]
	public void ByteArrayTest() => MultipleAllocTest.MultipleAlloc<Byte[]?>();
	[Fact]
	public void Int16ArrayTest() => MultipleAllocTest.MultipleAlloc<Int16[]?>();
	[Fact]
	public void Int32ArrayTest() => MultipleAllocTest.MultipleAlloc<Int32[]?>();
	[Fact]
	public void Int64ArrayTest() => MultipleAllocTest.MultipleAlloc<Int64[]?>();
	[Fact]
	public void StringTest() => MultipleAllocTest.MultipleAlloc<String?>();

	[Fact]
	public void BooleanWrapperTest() => MultipleAllocTest.MultipleAlloc<WrapperStruct<Boolean>>();
	[Fact]
	public void ByteWrapperTest() => MultipleAllocTest.MultipleAlloc<WrapperStruct<Byte>>();
	[Fact]
	public void Int16WrapperTest() => MultipleAllocTest.MultipleAlloc<WrapperStruct<Int16>>();
	[Fact]
	public void Int32WrapperTest() => MultipleAllocTest.MultipleAlloc<WrapperStruct<Int32>>();
	[Fact]
	public void Int64WrapperTest() => MultipleAllocTest.MultipleAlloc<WrapperStruct<Int64>>();

	[Fact]
	public void NullableBooleanWrapperTest() => MultipleAllocTest.MultipleAlloc<WrapperStruct<Boolean?>>();
	[Fact]
	public void NullableByteWrapperTest() => MultipleAllocTest.MultipleAlloc<WrapperStruct<Byte?>>();
	[Fact]
	public void NullableInt16WrapperTest() => MultipleAllocTest.MultipleAlloc<WrapperStruct<Int16?>>();
	[Fact]
	public void NullableInt32WrapperTest() => MultipleAllocTest.MultipleAlloc<WrapperStruct<Int32?>>();
	[Fact]
	public void NullableInt64WrapperTest() => MultipleAllocTest.MultipleAlloc<WrapperStruct<Int64?>>();

	[Fact]
	public void BooleanArrayWrapperTest() => MultipleAllocTest.MultipleAlloc<WrapperStruct<Boolean[]?>>();
	[Fact]
	public void ByteArrayWrapperTest() => MultipleAllocTest.MultipleAlloc<WrapperStruct<Byte[]?>>();
	[Fact]
	public void Int16ArrayWrapperTest() => MultipleAllocTest.MultipleAlloc<WrapperStruct<Int16[]?>>();
	[Fact]
	public void Int32ArrayWrapperTest() => MultipleAllocTest.MultipleAlloc<WrapperStruct<Int32[]?>>();
	[Fact]
	public void Int64ArrayWrapperTest() => MultipleAllocTest.MultipleAlloc<WrapperStruct<Int64[]?>>();
	[Fact]
	public void StringWrapperTest() => MultipleAllocTest.MultipleAlloc<WrapperStruct<String?>>();
#pragma warning restore CS0612

#if NETSTANDARD2_1 && !LEGACY || NETCOREAPP3_0_OR_GREATER
	[Obsolete]
#endif
	private static void MultipleAlloc<T>()
	{
		UInt16 count = (UInt16)(Math.Pow(2, PInvokeRandom.Shared.Next(2, 4)) - 1);
		Span<IntPtr> span0 = stackalloc IntPtr[5];
		Exception exception = new();
		// ReSharper disable once InlineOutVariableDeclaration
		// ReSharper disable once JoinDeclarationAndInitializer
		Boolean inStack;
		span0[0] = (IntPtr)Unsafe.AsPointer(ref MemoryMarshal.GetReference(span0));
#if NETSTANDARD2_1 && !LEGACY || NETCOREAPP3_0_OR_GREATER
		ValPtr<IntPtr> ptrPtr = NativeUtilities.GetUnsafeValPtrFromRef(ref span0[0]);
		BufferManager.Alloc<T>(count, MultipleAllocTest.Do);
		BufferManager.Alloc<T, ValPtr<IntPtr>>(count, ptrPtr, MultipleAllocTest.Do);
		inStack = BufferManager.Alloc<T, Boolean>(count, MultipleAllocTest.Get);
		PInvokeAssert.Equal(default, BufferManager.Alloc<T, ValPtr<IntPtr>, T>(count, ptrPtr, MultipleAllocTest.Get));
		PInvokeAssert.True(inStack);

		PInvokeAssert.Equal(
			exception, PInvokeAssert.ThrowsAny<Exception>(() => BufferManager.Alloc<T>(count, ThrowDo)));
		PInvokeAssert.Equal(
			exception,
			PInvokeAssert.ThrowsAny<Exception>(() => BufferManager.Alloc<T, Exception>(count, exception, ThrowDoEx)));
		PInvokeAssert.Equal(
			exception, PInvokeAssert.ThrowsAny<Exception>(() => BufferManager.Alloc<T, T>(count, ThrowGet)));
		PInvokeAssert.Equal(
			exception,
			PInvokeAssert.ThrowsAny<Exception>(() => BufferManager.Alloc<T, Exception, T>(
				                                   count, exception, ThrowGetEx)));
#endif
		BufferManager<T>.Alloc(new ScopedBufferAction<T>(count));
		BufferManager<T>.Alloc(new ScopedBufferFunction<T>(count), out inStack);
		PInvokeAssert.True(inStack);
		PInvokeAssert.Equal(
			exception,
			PInvokeAssert.ThrowsAny<Exception>(() => BufferManager<T>.Alloc(
				                                   new ScopedBufferAction<T>(count, exception))));
		PInvokeAssert.Equal(
			exception,
			PInvokeAssert.ThrowsAny<Exception>(() => BufferManager<T>.Alloc(
				                                   new ScopedBufferFunction<T>(count, exception), out inStack)));
#if NETSTANDARD2_1 && !LEGACY || NETCOREAPP3_0_OR_GREATER
		return;

		void ThrowDo(ScopedBuffer<T> buffer) => throw exception;
		void ThrowDoEx(ScopedBuffer<T> buffer, Exception ex) => throw ex;
		T ThrowGet(ScopedBuffer<T> buffer) => throw exception;
		T ThrowGetEx(ScopedBuffer<T> buffer, Exception ex) => throw ex;
#endif
	}
	private static void Do<T>(ScopedBuffer<T> buffer)
	{
		PInvokeAssert.True(buffer.InStack);
		PInvokeAssert.InRange(buffer.Span.Length, 2, Math.Pow(2, 4));
		PInvokeAssert.Equal(buffer.FullLength, buffer.Span.Length);
		PInvokeAssert.Equal(default, buffer.Span[0]);
		if (!RuntimeHelpers.IsReferenceOrContainsReferences<T>())
		{
			PInvokeAssert.Null(buffer.BufferMetadata);
			return;
		}

		PInvokeAssert.NotNull(buffer.BufferMetadata);
		if (typeof(T).IsValueType)
			PInvokeAssert.IsType<BufferTypeMetadata<T>>(buffer.BufferMetadata, false);
		else
			PInvokeAssert.IsType<BufferTypeMetadata<Object>>(buffer.BufferMetadata, false);
		PInvokeAssert.True(buffer.BufferMetadata.IsBinary);
		PInvokeAssert.Equal(buffer.Span.Length, buffer.BufferMetadata.Size);
		PInvokeAssert.InRange(buffer.BufferMetadata.ComponentCount, 0, 2);
	}
#if NETSTANDARD2_1 && !LEGACY || NETCOREAPP3_0_OR_GREATER
	private static void Do<T>(ScopedBuffer<T> buffer, ValPtr<IntPtr> ptrPtr)
	{
		MultipleAllocTest.Do(buffer);
		PInvokeAssert.True(ptrPtr.Pointer == ptrPtr.Reference);
	}
#endif
	private static Boolean Get<T>(ScopedBuffer<T> buffer)
	{
		MultipleAllocTest.Do(buffer);
		return buffer.InStack;
	}
#if NETSTANDARD2_1 && !LEGACY || NETCOREAPP3_0_OR_GREATER
	private static T Get<T>(ScopedBuffer<T> buffer, ValPtr<IntPtr> ptrPtr)
	{
		MultipleAllocTest.Do(buffer, ptrPtr);
		return buffer.Span[0];
	}
#endif

	private readonly struct ScopedBufferAction<T>(UInt16 count, Exception? exception = default) : IScopedBufferAction<T>
	{
		public Boolean IsMinimalCount => false;
		public UInt16 Count => count;
		public void Accept(scoped ScopedBuffer<T> buffer)
		{
			if (exception is not null) throw exception;
			MultipleAllocTest.Do(buffer);
		}
	}

	private readonly struct ScopedBufferFunction<T>(UInt16 count, Exception? exception = default)
		: IScopedBufferFunction<T, Boolean>
	{
		public Boolean IsMinimalCount => false;
		public UInt16 Count => count;
		public Boolean Apply(scoped ScopedBuffer<T> buffer)
			=> exception is null ? MultipleAllocTest.Get(buffer) : throw exception;
	}
}