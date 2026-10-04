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
		StandardBackend standard = default;

		ref BufferTypeMetadata<StandardValue>? value = ref standard.GetBinaryReference<StandardValue>(1);
		ref BufferTypeMetadata<StandardValue>? neighbour = ref standard.GetBinaryReference<StandardValue>(2);
		ref BufferTypeMetadata<StandardValue>? slot = ref standard.GetBinaryReference<StandardValue>(256);
		Assert.Equal(StandardBackendTest.ContiguousOffset(1, 2),
		             StandardBackendTest.Distance(ref value, ref neighbour));
		Assert.NotEqual(StandardBackendTest.ContiguousOffset(1, 256),
		                StandardBackendTest.Distance(ref value, ref slot));

		ref BufferTypeMetadata<StandardObject>? item = ref standard.GetBinaryReference<StandardObject>(1);
		ref BufferTypeMetadata<StandardObject>? itemNeighbour = ref standard.GetBinaryReference<StandardObject>(2);
		ref BufferTypeMetadata<StandardObject>? itemSlot = ref standard.GetBinaryReference<StandardObject>(2048);
		Assert.Equal(StandardBackendTest.ContiguousOffset(1, 2),
		             StandardBackendTest.Distance(ref item, ref itemNeighbour));
		Assert.NotEqual(StandardBackendTest.ContiguousOffset(1, 2048),
		                StandardBackendTest.Distance(ref item, ref itemSlot));
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