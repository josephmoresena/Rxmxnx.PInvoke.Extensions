#if NETFRAMEWORK && !NET46_OR_GREATER
using Array = Rxmxnx.PInvoke.Internal.FrameworkCompat.ArrayCompat;
#endif

using Rxmxnx.PInvoke.Buffers.Storage;

namespace Rxmxnx.PInvoke.Tests.Internal;

[TestFixture]
[ExcludeFromCodeCoverage]
public sealed class StandardBackendTest
{
#if NET5_0_OR_GREATER
	[Fact]
	public void SlotSegmentTest()
	{
		StandardBackendTest.AssertPages<StandardValue>(255, 8);
		StandardBackendTest.AssertPages<StandardObject>(2047, 5);
	}

	private static void AssertPages<T>(UInt16 inlineLength, Int32 slotCount)
	{
		StandardBackend backend = default;
		StandardBackendTest.AssertSamePage<T>(ref backend, 1, inlineLength);
		UInt32 start = (UInt32)inlineLength + 1;
		for (Int32 slot = 0; slot < slotCount; slot++)
		{
			UInt32 end = start * 2 - 1;
			StandardBackendTest.AssertDifferentPage<T>(ref backend, (UInt16)(start - 1), (UInt16)start);
			StandardBackendTest.AssertSamePage<T>(ref backend, (UInt16)start, (UInt16)end);
			start = end + 1;
		}
	}

	private static void AssertSamePage<T>(ref StandardBackend backend, UInt16 first, UInt16 last)
	{
		ref BufferTypeMetadata<T>? origin = ref backend.GetBinaryReference<T>(first);
		ref BufferTypeMetadata<T>? target = ref backend.GetBinaryReference<T>(last);
		Assert.Equal(StandardBackendTest.ContiguousOffset(first, last),
		             StandardBackendTest.Distance(ref origin, ref target));
	}
	private static void AssertDifferentPage<T>(ref StandardBackend backend, UInt16 first, UInt16 last)
	{
		ref BufferTypeMetadata<T>? origin = ref backend.GetBinaryReference<T>(first);
		ref BufferTypeMetadata<T>? target = ref backend.GetBinaryReference<T>(last);
		Assert.NotEqual(StandardBackendTest.ContiguousOffset(first, last),
		                StandardBackendTest.Distance(ref origin, ref target));
	}

	private static Int64 ContiguousOffset(Int32 fromSize, Int32 toSize) => (toSize - fromSize) * (Int64)IntPtr.Size;
	private static Int64 Distance<T>(ref BufferTypeMetadata<T>? origin, ref BufferTypeMetadata<T>? target)
		=> Unsafe.ByteOffset(ref origin, ref target).ToInt64();
#endif

	[Fact]
	public void SlotStoreTest()
	{
		StandardBackendTest.AssertSlot<StandardValue>(256);
		StandardBackendTest.AssertSlot<StandardObject>(2048);
	}

	private static void AssertSlot<T>(UInt16 slotSize)
	{
		StandardBackend left = default;
		StandardBackend right = default;
		BufferTypeMetadata<T> unit = Atomic<T>.TypeMetadata;
		BufferTypeMetadata<T> slot =
			new BufferTypeMetadata<Atomic<T>, T>(slotSize, Array.Empty<BufferTypeMetadata<T>>(), false);

		left.TryAdd(unit);
		PInvokeAssert.False(right.TryAdd(unit));
		PInvokeAssert.Same(unit, left.GetBinaryValue<T>(1));
		PInvokeAssert.Same(unit, right.GetBinaryValue<T>(1));

		PInvokeAssert.Same(slot, left.SetBinaryValue(slot));
		PInvokeAssert.Same(slot, right.GetBinaryValue<T>(slotSize));
		PInvokeAssert.NotSame(left.GetBinaryValue<T>(1), left.GetBinaryValue<T>(slotSize));
	}

	private struct StandardValue
	{
		public String? Text { get; set; }
	}

	// ReSharper disable once ClassNeverInstantiated.Local
	private sealed class StandardObject;
}