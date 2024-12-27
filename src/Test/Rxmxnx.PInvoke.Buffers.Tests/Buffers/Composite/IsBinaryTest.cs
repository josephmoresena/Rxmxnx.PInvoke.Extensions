namespace Rxmxnx.PInvoke.Tests.Buffers.Composite;

[ExcludeFromCodeCoverage]
[SuppressMessage("csharpsquid", "S2699")]
public sealed class IsBinaryTest
{
	private static readonly Type compositeType = typeof(Composite<,,>);

	[Fact]
	internal void BooleanTest() => IsBinaryTest.StructTest<Boolean>();
	[Fact]
	internal void ByteTest() => IsBinaryTest.StructTest<Byte>();
	[Fact]
	internal void Int16Test() => IsBinaryTest.StructTest<Int16>();
	[Fact]
	internal void Int32Test() => IsBinaryTest.StructTest<Int32>();
	[Fact]
	internal void Int64Test() => IsBinaryTest.StructTest<Int64>();
	[Fact]
	internal void StringWrapperTest() => IsBinaryTest.StructTest<WrapperStruct<String?>>();

	private static void StructTest<T>() where T : struct
	{
		IsBinaryTest.Test<WrapperStruct<WrapperStruct<T>>>();
		IsBinaryTest.Test<WrapperStruct<WrapperStruct<T>>?>();
	}
	private static void Test<T>()
	{
		Type typeofT = typeof(T);
		Type typeofAtomic = typeof(Atomic<T>);
		Type typeofComposite2 = typeof(Composite<Atomic<T>, Atomic<T>, T>);
		BufferTypeMetadata<T> atomicMetadata = IManagedBuffer<T>.GetMetadata<Atomic<T>>();
		BufferTypeMetadata<T> composite2Metadata = IManagedBuffer<T>.GetMetadata<Composite<Atomic<T>, Atomic<T>, T>>();

		Type binary = IsBinaryTest.compositeType.MakeGenericType(typeofAtomic, typeofComposite2, typeofT);
		Type nonBinary1 = IsBinaryTest.compositeType.MakeGenericType(typeofComposite2, typeofAtomic, typeofT);
		Type nonBinary2 = IsBinaryTest.compositeType.MakeGenericType(typeofAtomic, binary, typeofT);
		Type nonBinary3 = IsBinaryTest.compositeType.MakeGenericType(binary, binary, typeofT);
		Type nonBinary4 = IsBinaryTest.compositeType.MakeGenericType(nonBinary3, typeofAtomic, typeofT);
		Type nonBinary5 = IsBinaryTest.compositeType.MakeGenericType(typeofAtomic, nonBinary3, typeofT);

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
	}
}