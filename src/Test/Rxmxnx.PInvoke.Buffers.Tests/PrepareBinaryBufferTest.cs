namespace Rxmxnx.PInvoke.Tests;

[ExcludeFromCodeCoverage]
[SuppressMessage("csharpsquid", "S2699")]
public sealed class PrepareBinaryBufferTest
{
	[Fact]
	internal void AssertConstants()
	{
		Assert.Equal(typeof(Composite<,,>), BufferManager.TypeofComposite);
		Assert.Equal("TypeMetadata", BufferManager.TypeMetadataName);
		Assert.True(BufferManager.GetMetadataFlags.HasFlag(BindingFlags.Public));
		Assert.True(BufferManager.GetMetadataFlags.HasFlag(BindingFlags.NonPublic));
		Assert.True(BufferManager.GetMetadataFlags.HasFlag(BindingFlags.Static));
	}

	[Fact]
	internal void Test()
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