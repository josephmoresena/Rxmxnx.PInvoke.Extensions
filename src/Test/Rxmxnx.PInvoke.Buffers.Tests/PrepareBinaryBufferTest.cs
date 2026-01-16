namespace Rxmxnx.PInvoke.Tests;

[TestFixture]
[ExcludeFromCodeCoverage]
[SuppressMessage("csharpsquid", "S2699")]
public sealed class PrepareBinaryBufferTest
{
	[Fact]
	public void AssertConstants()
	{
		PInvokeAssert.Equal(typeof(Composite<,,>), BufferManager.TypeofComposite);
		PInvokeAssert.Equal("TypeMetadata", BufferManager.TypeMetadataName);
		PInvokeAssert.True(BufferManager.GetMetadataFlags.HasFlag(BindingFlags.Public));
		PInvokeAssert.True(BufferManager.GetMetadataFlags.HasFlag(BindingFlags.NonPublic));
		PInvokeAssert.True(BufferManager.GetMetadataFlags.HasFlag(BindingFlags.Static));
	}

	[Fact]
	public void Test()
	{
		BufferManager.PrepareBinaryBuffer(5);
		BufferManager.PrepareBinaryBuffer<ValueTuple<Int32, ValueTuple<Int32, Int32>>>(100);
		BufferManager.PrepareBinaryBuffer<ValueTuple<String, ValueTuple<String, ValueTuple<Int32, Int32>>>>(14);
		BufferManager.PrepareBinaryBuffer<ValueTuple<Int32, ValueTuple<String, ValueTuple<Int32, Int32>>>>(15);

		BufferManager.PrepareBinaryBufferNullable<ValueTuple<Int32, ValueTuple<Int32, Int32>>>(100);
		BufferManager.PrepareBinaryBufferNullable<ValueTuple<String, ValueTuple<String, ValueTuple<Int32, Int32>>>>(14);
		BufferManager.PrepareBinaryBufferNullable<ValueTuple<Int32, ValueTuple<String, ValueTuple<Int32, Int32>>>>(15);
	}
}