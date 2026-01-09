namespace Rxmxnx.PInvoke.Tests.Buffers.Composite;

[TestFixture]
[ExcludeFromCodeCoverage]
[SuppressMessage("csharpsquid", "S2699")]
public sealed class BinaryBufferCompositeTest
{
	private static readonly Type compositeType = typeof(Composite<,,>);

	[Fact]
	public void BooleanTest() => BinaryBufferCompositeTest.StructTest<Boolean>();
	[Fact]
	public void ByteTest() => BinaryBufferCompositeTest.StructTest<Byte>();
	[Fact]
	public void Int16Test() => BinaryBufferCompositeTest.StructTest<Int16>();
	[Fact]
	public void Int32Test() => BinaryBufferCompositeTest.StructTest<Int32>();
	[Fact]
	public void Int64Test() => BinaryBufferCompositeTest.StructTest<Int64>();
	[Fact]
	public void StringWrapperTest() => BinaryBufferCompositeTest.StructTest<WrapperStruct<String?>>();

	private static void StructTest<T>() where T : struct
	{
		BinaryBufferCompositeTest.Test<WrapperStruct<WrapperStruct<T>>>();
		BinaryBufferCompositeTest.Test<WrapperStruct<WrapperStruct<T>>?>();
	}
	private static void Test<T>()
	{
		Type typeofT = typeof(T);
		Type typeofAtomic = typeof(Atomic<T>);
		Type typeofComposite2 = typeof(Composite<Atomic<T>, Atomic<T>, T>);
		BufferTypeMetadata<T> atomicMetadata = BinaryBufferCompositeTest.GetMetadata<Atomic<T>, T>();
		BufferTypeMetadata<T> composite2Metadata =
			BinaryBufferCompositeTest.GetMetadata<Composite<Atomic<T>, Atomic<T>, T>, T>();

		Type binary = BinaryBufferCompositeTest.compositeType.MakeGenericType(typeofAtomic, typeofComposite2, typeofT);
		Type nonBinary1 =
			BinaryBufferCompositeTest.compositeType.MakeGenericType(typeofComposite2, typeofAtomic, typeofT);
		Type nonBinary2 = BinaryBufferCompositeTest.compositeType.MakeGenericType(typeofAtomic, binary, typeofT);
		Type nonBinary3 = BinaryBufferCompositeTest.compositeType.MakeGenericType(binary, binary, typeofT);
		Type nonBinary4 = BinaryBufferCompositeTest.compositeType.MakeGenericType(nonBinary3, typeofAtomic, typeofT);
		Type nonBinary5 = BinaryBufferCompositeTest.compositeType.MakeGenericType(typeofAtomic, nonBinary3, typeofT);

		BufferTypeMetadata<T> binaryMetadata = ((IManagedBinaryBuffer<T>)Activator.CreateInstance(binary)!).Metadata;
		BufferTypeMetadata<T> nonBinaryMetadata1 =
			((IManagedBinaryBuffer<T>)Activator.CreateInstance(nonBinary1)!).Metadata;
		BufferTypeMetadata<T> nonBinaryMetadata2 =
			((IManagedBinaryBuffer<T>)Activator.CreateInstance(nonBinary2)!).Metadata;
		BufferTypeMetadata<T> nonBinaryMetadata3 =
			((IManagedBinaryBuffer<T>)Activator.CreateInstance(nonBinary3)!).Metadata;
		BufferTypeMetadata<T> nonBinaryMetadata4 =
			((IManagedBinaryBuffer<T>)Activator.CreateInstance(nonBinary4)!).Metadata;
		BufferTypeMetadata<T> nonBinaryMetadata5 =
			((IManagedBinaryBuffer<T>)Activator.CreateInstance(nonBinary5)!).Metadata;

		PInvokeAssert.Equal(composite2Metadata, atomicMetadata.Double());
		PInvokeAssert.Equal(binaryMetadata, composite2Metadata.Compose(atomicMetadata));
		PInvokeAssert.Equal(nonBinaryMetadata1, atomicMetadata.Compose(composite2Metadata));
		PInvokeAssert.Equal(nonBinaryMetadata2, binaryMetadata.Compose(atomicMetadata));
		PInvokeAssert.Equal(nonBinaryMetadata3, binaryMetadata.Double());
		PInvokeAssert.Null(atomicMetadata.Compose(nonBinaryMetadata3));
		PInvokeAssert.Null(nonBinaryMetadata3.Compose(atomicMetadata));

		PInvokeAssert.True(binaryMetadata.IsBinary);
		PInvokeAssert.Equal(2, binaryMetadata.ComponentCount);
		PInvokeAssert.Equal(atomicMetadata.Size + composite2Metadata.Size, binaryMetadata.Size);
		PInvokeAssert.Equal(atomicMetadata, binaryMetadata[0]);
		PInvokeAssert.Equal(composite2Metadata, binaryMetadata[1]);

		PInvokeAssert.False(nonBinaryMetadata1.IsBinary);
		PInvokeAssert.Equal(2, nonBinaryMetadata1.ComponentCount);
		PInvokeAssert.Equal(atomicMetadata.Size + composite2Metadata.Size, nonBinaryMetadata1.Size);
		PInvokeAssert.Equal(composite2Metadata, nonBinaryMetadata1[0]);
		PInvokeAssert.Equal(atomicMetadata, nonBinaryMetadata1[1]);

		PInvokeAssert.False(nonBinaryMetadata2.IsBinary);
		PInvokeAssert.Equal(2, nonBinaryMetadata2.ComponentCount);
		PInvokeAssert.Equal(2 * atomicMetadata.Size + composite2Metadata.Size, nonBinaryMetadata2.Size);
		PInvokeAssert.Equal(atomicMetadata, nonBinaryMetadata2[0]);
		PInvokeAssert.Equal(binaryMetadata, nonBinaryMetadata2[1]);

		PInvokeAssert.False(nonBinaryMetadata3.IsBinary);
		PInvokeAssert.Equal(2, nonBinaryMetadata3.ComponentCount);
		PInvokeAssert.Equal(2 * binaryMetadata.Size, nonBinaryMetadata3.Size);
		PInvokeAssert.Equal(binaryMetadata, nonBinaryMetadata3[0]);
		PInvokeAssert.Equal(binaryMetadata, nonBinaryMetadata3[1]);

		PInvokeAssert.False(nonBinaryMetadata4.IsBinary);
		PInvokeAssert.Equal(2, nonBinaryMetadata4.ComponentCount);
		PInvokeAssert.Equal(atomicMetadata.Size + nonBinaryMetadata3.Size, nonBinaryMetadata4.Size);
		PInvokeAssert.Equal(nonBinaryMetadata3, nonBinaryMetadata4[0]);
		PInvokeAssert.Equal(atomicMetadata, nonBinaryMetadata4[1]);

		PInvokeAssert.False(nonBinaryMetadata5.IsBinary);
		PInvokeAssert.Equal(2, nonBinaryMetadata5.ComponentCount);
		PInvokeAssert.Equal(atomicMetadata.Size + nonBinaryMetadata3.Size, nonBinaryMetadata5.Size);
		PInvokeAssert.Equal(atomicMetadata, nonBinaryMetadata5[0]);
		PInvokeAssert.Equal(nonBinaryMetadata3, nonBinaryMetadata5[1]);

		PInvokeAssert.Equal(atomicMetadata, BufferManager.MetadataManager<T>.GetMetadata(typeofAtomic));
		PInvokeAssert.Equal(composite2Metadata, BufferManager.MetadataManager<T>.GetMetadata(typeofComposite2));
		PInvokeAssert.Equal(binaryMetadata, BufferManager.MetadataManager<T>.GetMetadata(binaryMetadata.BufferType));
		PInvokeAssert.Equal(binaryMetadata.Components.ToArray(), [
			BufferManager.MetadataManager<T>.GetMetadata(typeofAtomic),
			BufferManager.MetadataManager<T>.GetMetadata(typeofComposite2),
		]);
		foreach (BufferTypeMetadata<T> metadata in binaryMetadata.Components.Span)
			PInvokeAssert.Equal(metadata, BufferManager.MetadataManager<T>.GetMetadata(metadata.BufferType));
	}
	private static BufferTypeMetadata<T> GetMetadata<TBuffer, T>()
		where TBuffer : struct, IManagedBinaryBuffer<TBuffer, T>
	{
		const BindingFlags getMetadataFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;
		Type typeofInterface = typeof(IManagedBuffer<T>);
		MethodInfo? getMetadata =
			typeofInterface.GetMethod(nameof(BufferManager.MetadataManager<T>.GetMetadata), getMetadataFlags);
		if (getMetadata is not null)
			return (BufferTypeMetadata<T>)getMetadata.MakeGenericMethod(typeof(TBuffer)).Invoke(null, [])!;
		return (BufferTypeMetadata<T>)typeof(TBuffer).GetField(
			nameof(Composite<Atomic<Object>, Atomic<Object>, Object>.TypeMetadata), getMetadataFlags)!.GetValue(null)!;
	}
}