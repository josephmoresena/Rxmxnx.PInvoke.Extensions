using Rxmxnx.PInvoke.RefStruct;

namespace Rxmxnx.PInvoke.Tests.RefStruct;

[TestFixture]
[ExcludeFromCodeCoverage]
public sealed class SpanPtrTest
{
	[Fact]
	public unsafe void RoundtripTest()
	{
		Int32[] first = [1, 2, 3,];
		Int32[] second = [7, 8,];
		fixed (Int32* _ = first)
		fixed (Int32* __ = second)
		{
			Span<Int32> span = first;
			SpanPtr<Int32> pointer = SpanPtr<Int32>.CreateUnsafePtr(ref span);
			span[1] = 9;
			PInvokeAssert.Equal(9, pointer.GetSpan()[1]);
			span = second;
			PInvokeAssert.Equal(2, pointer.GetSpan().Length);
			PInvokeAssert.Equal(7, pointer.GetSpan()[0]);

			ReadOnlySpan<Int32> readOnly = first;
			ReadOnlySpanPtr<Int32> readOnlyPointer = ReadOnlySpanPtr<Int32>.CreateUnsafePtr(ref readOnly);
			readOnly = second;
			PInvokeAssert.Equal(2, readOnlyPointer.GetSpan().Length);
			PInvokeAssert.Equal(8, readOnlyPointer.GetSpan()[1]);
		}
	}
#if NET9_0_OR_GREATER
	[Fact]
	public void ValPtrRoundtripTest()
	{
		Span<Int32> values = [4, 5,];
		Span<Int32> span = values;
		SpanPtr<Int32> pointer = SpanPtr<Int32>.CreateUnsafePtr(ref span);
		ValPtr<Span<Int32>> valPtr = pointer;
		SpanPtr<Int32> restored = (SpanPtr<Int32>)valPtr;
		PInvokeAssert.Equal(2, restored.GetSpan().Length);
		PInvokeAssert.Equal(4, restored.GetSpan()[0]);

		ReadOnlySpan<Int32> readOnly = values;
		ReadOnlySpanPtr<Int32> readOnlyPointer = ReadOnlySpanPtr<Int32>.CreateUnsafePtr(ref readOnly);
		ReadOnlyValPtr<ReadOnlySpan<Int32>> readOnlyValPtr = readOnlyPointer;
		PInvokeAssert.Equal(5, ((ReadOnlySpanPtr<Int32>)readOnlyValPtr).GetSpan()[1]);
	}
#endif
	[Fact]
	public void NullTest()
	{
		PInvokeAssert.Equal(0, default(SpanPtr<Int32>).GetSpan().Length);
		PInvokeAssert.Equal(0, default(ReadOnlySpanPtr<Int32>).GetSpan().Length);
	}
}