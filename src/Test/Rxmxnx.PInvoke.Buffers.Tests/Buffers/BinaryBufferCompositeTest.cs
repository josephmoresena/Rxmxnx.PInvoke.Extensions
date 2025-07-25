namespace Rxmxnx.PInvoke.Tests.Buffers.Composite;

[ExcludeFromCodeCoverage]
[SuppressMessage("csharpsquid", "S2699")]
public sealed class BinaryBufferCompositeTest
{
	private static readonly Type compositeType = typeof(Composite<,,>);

	[Fact]
	internal void BooleanTest() => BinaryBufferCompositeTest.StructTest<Boolean>();
	[Fact]
	internal void ByteTest() => BinaryBufferCompositeTest.StructTest<Byte>();
	[Fact]
	internal void Int16Test() => BinaryBufferCompositeTest.StructTest<Int16>();
	[Fact]
	internal void Int32Test() => BinaryBufferCompositeTest.StructTest<Int32>();
	[Fact]
	internal void Int64Test() => BinaryBufferCompositeTest.StructTest<Int64>();
	[Fact]
	internal void StringWrapperTest() => BinaryBufferCompositeTest.StructTest<WrapperStruct<String?>>();

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

		Assert.Equal(composite2Metadata, atomicMetadata.Double());
		Assert.Equal(binaryMetadata, composite2Metadata.Compose(atomicMetadata));
		Assert.Equal(nonBinaryMetadata1, atomicMetadata.Compose(composite2Metadata));
		Assert.Equal(nonBinaryMetadata2, binaryMetadata.Compose(atomicMetadata));
		Assert.Equal(nonBinaryMetadata3, binaryMetadata.Double());
		Assert.Null(atomicMetadata.Compose(nonBinaryMetadata3));
		Assert.Null(nonBinaryMetadata3.Compose(atomicMetadata));

		Assert.True(binaryMetadata.IsBinary);
		Assert.Equal(2, binaryMetadata.ComponentCount);
		Assert.Equal(atomicMetadata.Size + composite2Metadata.Size, binaryMetadata.Size);
		Assert.Equal(atomicMetadata, binaryMetadata[0]);
		Assert.Equal(composite2Metadata, binaryMetadata[1]);

		Assert.False(nonBinaryMetadata1.IsBinary);
		Assert.Equal(2, nonBinaryMetadata1.ComponentCount);
		Assert.Equal(atomicMetadata.Size + composite2Metadata.Size, nonBinaryMetadata1.Size);
		Assert.Equal(composite2Metadata, nonBinaryMetadata1[0]);
		Assert.Equal(atomicMetadata, nonBinaryMetadata1[1]);

		Assert.False(nonBinaryMetadata2.IsBinary);
		Assert.Equal(2, nonBinaryMetadata2.ComponentCount);
		Assert.Equal(2 * atomicMetadata.Size + composite2Metadata.Size, nonBinaryMetadata2.Size);
		Assert.Equal(atomicMetadata, nonBinaryMetadata2[0]);
		Assert.Equal(binaryMetadata, nonBinaryMetadata2[1]);

		Assert.False(nonBinaryMetadata3.IsBinary);
		Assert.Equal(2, nonBinaryMetadata3.ComponentCount);
		Assert.Equal(2 * binaryMetadata.Size, nonBinaryMetadata3.Size);
		Assert.Equal(binaryMetadata, nonBinaryMetadata3[0]);
		Assert.Equal(binaryMetadata, nonBinaryMetadata3[1]);

		Assert.False(nonBinaryMetadata4.IsBinary);
		Assert.Equal(2, nonBinaryMetadata4.ComponentCount);
		Assert.Equal(atomicMetadata.Size + nonBinaryMetadata3.Size, nonBinaryMetadata4.Size);
		Assert.Equal(nonBinaryMetadata3, nonBinaryMetadata4[0]);
		Assert.Equal(atomicMetadata, nonBinaryMetadata4[1]);

		Assert.False(nonBinaryMetadata5.IsBinary);
		Assert.Equal(2, nonBinaryMetadata5.ComponentCount);
		Assert.Equal(atomicMetadata.Size + nonBinaryMetadata3.Size, nonBinaryMetadata5.Size);
		Assert.Equal(atomicMetadata, nonBinaryMetadata5[0]);
		Assert.Equal(nonBinaryMetadata3, nonBinaryMetadata5[1]);

		Assert.Equal(atomicMetadata, BufferManager.MetadataManager<T>.GetMetadata(typeofAtomic));
		Assert.Equal(composite2Metadata, BufferManager.MetadataManager<T>.GetMetadata(typeofComposite2));
		Assert.Equal(binaryMetadata, BufferManager.MetadataManager<T>.GetMetadata(binaryMetadata.BufferType));
		Assert.Equal(binaryMetadata.Components, [
			BufferManager.MetadataManager<T>.GetMetadata(typeofAtomic),
			BufferManager.MetadataManager<T>.GetMetadata(typeofComposite2),
		]);
		foreach (BufferTypeMetadata<T> metadata in binaryMetadata.Components.AsSpan())
			Assert.Equal(metadata, BufferManager.MetadataManager<T>.GetMetadata(metadata.BufferType));
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