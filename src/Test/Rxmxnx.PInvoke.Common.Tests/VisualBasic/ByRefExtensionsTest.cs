using Rxmxnx.PInvoke.VisualBasic;

namespace Rxmxnx.PInvoke.Tests.VisualBasic;

[TestFixture]
[ExcludeFromCodeCoverage]
public sealed class ByRefExtensionsTest
{
	[Fact]
	public void SpanGetSetTest() => ByRefExtensionsTest.ExerciseSpan();
	[Fact]
	public void SpanOutOfRangeTest()
	{
		PInvokeAssert.Throws<IndexOutOfRangeException>(ByRefExtensionsTest.ReadPastEnd);
		PInvokeAssert.Throws<IndexOutOfRangeException>(ByRefExtensionsTest.WritePastEnd);
	}
	[Fact]
	public void ValuePointerTest()
	{
		IDisposable disposable = NativeUtilities.UnsafeHeapAlloc(3, out ValPtr<Int32> valPtr);
		try
		{
			PInvokeAssert.Equal(4, valPtr.SetUnsafe(4));
			PInvokeAssert.Equal(4, valPtr.GetUnsafe());
			PInvokeAssert.Equal(4, ((ReadOnlyValPtr<Int32>)valPtr).GetUnsafe());
		}
		finally
		{
			disposable.Dispose();
		}
	}
	[Fact]
	public void NullValuePointerTest()
	{
		ValPtr<Int32> valPtr = default;
		ReadOnlyValPtr<Int32> readOnly = default;
		PInvokeAssert.Throws<NullReferenceException>(() => valPtr.GetUnsafe());
		PInvokeAssert.Throws<NullReferenceException>(() => valPtr.SetUnsafe(1));
		PInvokeAssert.Throws<NullReferenceException>(() => readOnly.GetUnsafe());
	}
	[Fact]
	public unsafe void UnsafeCopyToTest()
	{
		Int32[] source = [1, 2, 3,];
		IDisposable disposable = NativeUtilities.UnsafeHeapAlloc(source.Length, out ValPtr<Int32> valPtr);
		try
		{
			source.AsSpan().UnsafeCopyTo(valPtr);
			Span<Int32> written = new(valPtr.Pointer.ToPointer(), source.Length);
			PInvokeAssert.Equal(1, valPtr.GetUnsafe());
			PInvokeAssert.Equal(1, written[0]);
			PInvokeAssert.Equal(2, written[1]);
			PInvokeAssert.Equal(3, written[2]);

			ReadOnlySpan<Int32> readOnly = [9, 8,];
			readOnly.UnsafeCopyTo(valPtr);
			PInvokeAssert.Equal(9, valPtr.GetUnsafe());
			PInvokeAssert.Equal(8, written[1]);
		}
		finally
		{
			disposable.Dispose();
		}
	}
	[Fact]
	public void ReferenceUnsafeCopyToTest()
	{
		String[] destination = new String[3];
		ValPtr<String> valPtr = NativeUtilities.GetUnsafeValPtrFromRef(ref destination[0]);
		ReadOnlySpan<String> source = ["a", "b", "c",];
		source.UnsafeCopyTo(valPtr);
		PInvokeAssert.Equal("a", valPtr.GetUnsafe());
		PInvokeAssert.Equal("b", destination[1]);
		PInvokeAssert.Equal("c", destination[2]);
	}
	[Fact]
	public void UnsafeCopyFromTest()
	{
		Int32[] source = [1, 2, 3,];
		IDisposable disposable = NativeUtilities.UnsafeHeapAlloc(source.Length, out ValPtr<Int32> valPtr);
		try
		{
			source.AsSpan().UnsafeCopyTo(valPtr);
			Int32[] destination = new Int32[source.Length];
			destination.AsSpan().UnsafeCopyFrom(valPtr);
			PInvokeAssert.Equal(source, destination);

			Int32[] head = new Int32[2];
			head.AsSpan().UnsafeCopyFrom(valPtr);
			PInvokeAssert.Equal(1, head[0]);
			PInvokeAssert.Equal(2, head[1]);
		}
		finally
		{
			disposable.Dispose();
		}
	}
	[Fact]
	public void ReferenceUnsafeCopyFromTest()
	{
		String[] source = ["a", "b", "c",];
		ValPtr<String> valPtr = NativeUtilities.GetUnsafeValPtrFromRef(ref source[0]);
		String[] destination = new String[source.Length];
		destination.AsSpan().UnsafeCopyFrom(valPtr);
		PInvokeAssert.Equal("a", destination[0]);
		PInvokeAssert.Equal("b", destination[1]);
		PInvokeAssert.Equal("c", destination[2]);
	}
	[Fact]
	public void EmptyUnsafeCopyFromTest()
	{
		Span<Int32> empty = [];
		empty.UnsafeCopyFrom(default);
	}
	[Fact]
	public void EmptyUnsafeCopyToTest()
	{
		ReadOnlySpan<Int32> empty = default;
		empty.UnsafeCopyTo(default);
		Span<Int32> mutable = [];
		mutable.UnsafeCopyTo(default);
	}

	private static void ExerciseSpan()
	{
		Int32[] values = [1, 2, 3,];
		Span<Int32> span = values;
		ReadOnlySpan<Int32> readOnly = span;
		PInvokeAssert.Equal(2, span.GetUnsafe(1));
		PInvokeAssert.Equal(2, readOnly.GetUnsafe(1));
		PInvokeAssert.Equal(5, span.SetUnsafe(1, 5));
		PInvokeAssert.Equal(5, values[1]);
		PInvokeAssert.Equal(5, readOnly.GetUnsafe(1));
	}
	private static void ReadPastEnd()
	{
		Span<Int32> span = stackalloc Int32[2];
		_ = span.GetUnsafe(2);
	}
	private static void WritePastEnd()
	{
		Span<Int32> span = stackalloc Int32[2];
		span.SetUnsafe(2, 1);
	}
}