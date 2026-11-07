namespace Rxmxnx.PInvoke.Tests;

[TestFixture]
[ExcludeFromCodeCoverage]
public sealed class ClassAllocTest
{
	[Fact]
	public void ActionTest()
	{
		ClassAction action = new();
		BufferManager<String?>.Alloc(action);
		PInvokeAssert.True(action.Accepted);
	}
	[Fact]
	public void FunctionTest()
	{
		BufferManager<String?>.Alloc(new ClassFunction(), out Int32 length);
		PInvokeAssert.Equal(1, length);
	}

	private sealed class ClassAction : IScopedBufferAction<String?>
	{
		public Boolean Accepted { get; private set; }
		public Boolean IsMinimalCount => false;
		public UInt16 Count => 1;
		public void Accept(scoped ScopedBuffer<String?> buffer)
		{
			PInvokeAssert.True(buffer.InStack);
			PInvokeAssert.Equal(1, buffer.Span.Length);
			PInvokeAssert.Equal(1, buffer.FullLength);
			PInvokeAssert.NotNull(buffer.BufferMetadata);
			this.Accepted = true;
		}
	}

	private sealed class ClassFunction : IScopedBufferFunction<String?, Int32>
	{
		public Boolean IsMinimalCount => false;
		public UInt16 Count => 1;
		public Int32 Apply(scoped ScopedBuffer<String?> buffer)
		{
			PInvokeAssert.True(buffer.InStack);
			PInvokeAssert.Equal(1, buffer.Span.Length);
			PInvokeAssert.NotNull(buffer.BufferMetadata);
			return buffer.Span.Length;
		}
	}
}
