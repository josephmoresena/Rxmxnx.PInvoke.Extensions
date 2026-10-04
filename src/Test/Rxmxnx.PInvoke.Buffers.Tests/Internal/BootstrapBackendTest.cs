#if NET8_0_OR_GREATER
using Rxmxnx.PInvoke.Buffers.Storage;
using Rxmxnx.PInvoke.Buffers.Storage.Bootstrap;

namespace Rxmxnx.PInvoke.Tests.Internal;

[TestFixture]
[ExcludeFromCodeCoverage]
public sealed class BootstrapBackendTest
{
	[Fact]
	public void FixedLimitTest()
	{
		BootstrapBackendTest.AssertShared<BootstrapBackend31, Object31>(32);
		BootstrapBackendTest.AssertShared<BootstrapBackend31, Value31>(32);
		BootstrapBackendTest.AssertShared<BootstrapBackend127, Object127>(128);
		BootstrapBackendTest.AssertShared<BootstrapBackend127, Value127>(128);
	}
	[Fact]
	public void SpaceLimitTest()
	{
		BootstrapBackendTest.AssertShared<BootstrapBackend<Space11>, Object11>(2048);
		BootstrapBackendTest.AssertShared<BootstrapBackend<Space11>, Value11>(2048);
		BootstrapBackendTest.AssertShared<BootstrapBackend<Space16>, Object16>(default);
		BootstrapBackendTest.AssertShared<BootstrapBackend<Space16>, Value16>(default);
	}
	[Fact]
	public void SharedSegmentTest()
	{
		BootstrapBackend<Space11> narrow = default;
		BootstrapBackend<Space16> wide = default;

		ref BufferTypeMetadata<SharedValue>? narrowValue = ref narrow.GetBinaryReference<SharedValue>(1);
		ref BufferTypeMetadata<SharedValue>? wideValue = ref wide.GetBinaryReference<SharedValue>(1);
		ref BufferTypeMetadata<SharedValue>? wideNeighbour = ref wide.GetBinaryReference<SharedValue>(2);
		ref BufferTypeMetadata<SharedValue>? wideSlot = ref wide.GetBinaryReference<SharedValue>(256);
		Assert.Equal(0, BootstrapBackendTest.Distance(ref narrowValue, ref wideValue));
		Assert.Equal(BootstrapBackendTest.ContiguousOffset(1, 2),
		             BootstrapBackendTest.Distance(ref wideValue, ref wideNeighbour));
		Assert.NotEqual(BootstrapBackendTest.ContiguousOffset(1, 256),
		                BootstrapBackendTest.Distance(ref wideValue, ref wideSlot));

		ref BufferTypeMetadata<SharedObject>? narrowObject = ref narrow.GetBinaryReference<SharedObject>(1);
		ref BufferTypeMetadata<SharedObject>? wideObject = ref wide.GetBinaryReference<SharedObject>(1);
		ref BufferTypeMetadata<SharedObject>? wideObjectNeighbour = ref wide.GetBinaryReference<SharedObject>(2);
		ref BufferTypeMetadata<SharedObject>? wideObjectSlot = ref wide.GetBinaryReference<SharedObject>(2048);
		Assert.Equal(0, BootstrapBackendTest.Distance(ref narrowObject, ref wideObject));
		Assert.Equal(BootstrapBackendTest.ContiguousOffset(1, 2),
		             BootstrapBackendTest.Distance(ref wideObject, ref wideObjectNeighbour));
		Assert.NotEqual(BootstrapBackendTest.ContiguousOffset(1, 2048),
		                BootstrapBackendTest.Distance(ref wideObject, ref wideObjectSlot));
	}

	private static void AssertShared<TBackend, T>(UInt16 beyond) where TBackend : struct, IMetadataStorageBackend
	{
		MetadataStorage left = new MetadataStorage<TBackend>();
		MetadataStorage right = new MetadataStorage<TBackend>();
		BufferTypeMetadata<T>? fromLeft = left.GetMetadata<T>(1);
		BufferTypeMetadata<T>? fromRight = right.GetMetadata<T>(1);

		Assert.NotNull(fromLeft);
		Assert.Equal((UInt16)1, fromLeft.Size);
		Assert.Same(fromLeft, fromRight);
		Assert.Same(fromLeft, left.GetMetadata<T>(1));
		if (beyond == 0) return;

		Assert.Null(left.GetMetadata<T>(beyond));
		Assert.Null(right.GetMetadata<T>(beyond));
	}

	private static Int64 ContiguousOffset(Int32 fromSize, Int32 toSize) => (toSize - fromSize) * (Int64)IntPtr.Size;
	private static Int64 Distance<T>(ref BufferTypeMetadata<T>? origin, ref BufferTypeMetadata<T>? target)
		=> Unsafe.ByteOffset(ref origin, ref target).ToInt64();

	private sealed class Object31;
	private sealed class Object127;
	private sealed class Object11;
	private sealed class Object16;

	private struct Value31
	{
		public String? Text { get; set; }
	}

	private struct Value127
	{
		public String? Text { get; set; }
	}

	private struct Value11
	{
		public String? Text { get; set; }
	}

	private struct Value16
	{
		public String? Text { get; set; }
	}

	private struct SharedValue
	{
		public String? Text { get; set; }
	}

	private sealed class SharedObject;
}
#endif