#if NET8_0_OR_GREATER
using Rxmxnx.PInvoke.Buffers.Storage;
using Rxmxnx.PInvoke.Buffers.Storage.Bootstrap;

namespace Rxmxnx.PInvoke.Tests.Internal;

using R32 =
	Composite<
		Composite<
			Composite<
				Composite<
					Composite<Atomic<BootstrapBackendTest.RegValue>, Atomic<BootstrapBackendTest.RegValue>,
						BootstrapBackendTest.RegValue>,
					Composite<Atomic<BootstrapBackendTest.RegValue>, Atomic<BootstrapBackendTest.RegValue>,
						BootstrapBackendTest.RegValue>, BootstrapBackendTest.RegValue>,
				Composite<
					Composite<Atomic<BootstrapBackendTest.RegValue>, Atomic<BootstrapBackendTest.RegValue>,
						BootstrapBackendTest.RegValue>,
					Composite<Atomic<BootstrapBackendTest.RegValue>, Atomic<BootstrapBackendTest.RegValue>,
						BootstrapBackendTest.RegValue>, BootstrapBackendTest.RegValue>, BootstrapBackendTest.RegValue>,
			Composite<
				Composite<
					Composite<Atomic<BootstrapBackendTest.RegValue>, Atomic<BootstrapBackendTest.RegValue>,
						BootstrapBackendTest.RegValue>,
					Composite<Atomic<BootstrapBackendTest.RegValue>, Atomic<BootstrapBackendTest.RegValue>,
						BootstrapBackendTest.RegValue>, BootstrapBackendTest.RegValue>,
				Composite<
					Composite<Atomic<BootstrapBackendTest.RegValue>, Atomic<BootstrapBackendTest.RegValue>,
						BootstrapBackendTest.RegValue>,
					Composite<Atomic<BootstrapBackendTest.RegValue>, Atomic<BootstrapBackendTest.RegValue>,
						BootstrapBackendTest.RegValue>, BootstrapBackendTest.RegValue>, BootstrapBackendTest.RegValue>,
			BootstrapBackendTest.RegValue>, Composite<
			Composite<
				Composite<
					Composite<Atomic<BootstrapBackendTest.RegValue>, Atomic<BootstrapBackendTest.RegValue>,
						BootstrapBackendTest.RegValue>,
					Composite<Atomic<BootstrapBackendTest.RegValue>, Atomic<BootstrapBackendTest.RegValue>,
						BootstrapBackendTest.RegValue>, BootstrapBackendTest.RegValue>,
				Composite<
					Composite<Atomic<BootstrapBackendTest.RegValue>, Atomic<BootstrapBackendTest.RegValue>,
						BootstrapBackendTest.RegValue>,
					Composite<Atomic<BootstrapBackendTest.RegValue>, Atomic<BootstrapBackendTest.RegValue>,
						BootstrapBackendTest.RegValue>, BootstrapBackendTest.RegValue>, BootstrapBackendTest.RegValue>,
			Composite<
				Composite<
					Composite<Atomic<BootstrapBackendTest.RegValue>, Atomic<BootstrapBackendTest.RegValue>,
						BootstrapBackendTest.RegValue>,
					Composite<Atomic<BootstrapBackendTest.RegValue>, Atomic<BootstrapBackendTest.RegValue>,
						BootstrapBackendTest.RegValue>, BootstrapBackendTest.RegValue>, Composite<
					Composite<Atomic<BootstrapBackendTest.RegValue>, Atomic<BootstrapBackendTest.RegValue>,
						BootstrapBackendTest.RegValue>,
					Composite<Atomic<BootstrapBackendTest.RegValue>, Atomic<BootstrapBackendTest.RegValue>,
						BootstrapBackendTest.RegValue>, BootstrapBackendTest.RegValue>, BootstrapBackendTest.RegValue>,
			BootstrapBackendTest.RegValue>, BootstrapBackendTest.RegValue>;

[TestFixture]
[ExcludeFromCodeCoverage]
public sealed class BootstrapBackendTest
{
	[Fact]
	public void FixedLimitTest()
	{
		BootstrapBackendTest.AssertPages<BootstrapBackend31, PageObject>(31, 0);
		BootstrapBackendTest.AssertPages<BootstrapBackend31, PageValue>(31, 0);
		BootstrapBackendTest.AssertPages<BootstrapBackend127, PageObject>(127, 0);
		BootstrapBackendTest.AssertPages<BootstrapBackend127, PageValue>(127, 0);
	}
	[Fact]
	public void SpaceLimitTest()
	{
		BootstrapBackendTest.AssertPages<BootstrapBackend<Space11>, PageObject>(2047, 0);
		BootstrapBackendTest.AssertPages<BootstrapBackend<Space11>, PageValue>(255, 3);
		BootstrapBackendTest.AssertPages<BootstrapBackend<Space16>, PageObject>(2047, 5);
		BootstrapBackendTest.AssertPages<BootstrapBackend<Space16>, PageValue>(255, 8);
	}
	[Fact]
	public void InvalidSizeTest()
	{
		BootstrapBackendTest.AssertZero<BootstrapBackend<Space11>, LimitValue>();
		BootstrapBackendTest.AssertStored<BootstrapBackend31, StoreValue>(32);
		BootstrapBackendTest.AssertStored<BootstrapBackend127, StoreValue>(128);
		BootstrapBackendTest.AssertStored<BootstrapBackend<Space11>, StoreValue>(2048);
		BootstrapBackendTest.AssertStored<BootstrapBackend<Space11>, StoreObject>(2048);
		BootstrapBackendTest.AssertAdded<BootstrapBackend31, AddValue>(32);
		BootstrapBackendTest.AssertAdded<BootstrapBackend127, AddValue>(128);
		BootstrapBackendTest.AssertAdded<BootstrapBackend<Space11>, AddValue>(2048);
		BootstrapBackendTest.AssertRegistered();
		BootstrapBackendTest.AssertPrepared<BootstrapBackend31, PrepValue>(32);
		BootstrapBackendTest.AssertPrepared<BootstrapBackend127, PrepValue>(128);
		BootstrapBackendTest.AssertPrepared<BootstrapBackend<Space11>, PrepValue>(2048);
		BootstrapBackendTest.AssertReplaced<BootstrapBackend31, ReplaceValue>(32);
		BootstrapBackendTest.AssertReplaced<BootstrapBackend<Space11>, ReplaceValue>(2048);
		BootstrapBackendTest.AssertPreparedOverNonBinary<BootstrapBackend31, PrepReplaceValue>(32);
	}
	private static void AssertPages<TBackend, T>(UInt16 inlineLength, Int32 slotCount)
		where TBackend : struct, IMetadataStorageBackend
	{
		TBackend backend = default;
		BootstrapBackendTest.AssertSamePage<TBackend, T>(ref backend, 1, inlineLength);
		if (!typeof(T).IsValueType)
		{
			ref BufferTypeMetadata<T>? inlineLimit = ref backend.GetBinaryReference<T>(inlineLength);
			Assert.NotNull(inlineLimit);
			Assert.Equal(inlineLength, inlineLimit.Size);
		}

		UInt32 start = (UInt32)inlineLength + 1;
		for (Int32 slot = 0; slot < slotCount; slot++)
		{
			UInt32 end = start * 2 - 1;
			BootstrapBackendTest.AssertDifferentPage<TBackend, T>(ref backend, (UInt16)(start - 1), (UInt16)start);
			BootstrapBackendTest.AssertSamePage<TBackend, T>(ref backend, (UInt16)start, (UInt16)end);
			start = end + 1;
		}
		if (start <= UInt16.MaxValue)
			BootstrapBackendTest.AssertOutOfRange(() => backend.GetBinaryReference<T>((UInt16)start));
	}

	private static void AssertZero<TBackend, T>() where TBackend : struct, IMetadataStorageBackend
	{
		MetadataStorage storage = new MetadataStorage<TBackend>();
		BufferTypeMetadata<T>? metadata = storage.GetMetadata<T>(0);
		Assert.NotNull(metadata);
		Assert.Equal((UInt16)1, metadata.Size);
		storage.PrepareBinaryMetadata<T>(0);
	}
	private static void AssertSamePage<TBackend, T>(ref TBackend backend, UInt16 first, UInt16 last)
		where TBackend : struct, IMetadataStorageBackend
	{
		ref BufferTypeMetadata<T>? origin = ref backend.GetBinaryReference<T>(first);
		ref BufferTypeMetadata<T>? target = ref backend.GetBinaryReference<T>(last);
		Assert.Equal(BootstrapBackendTest.ContiguousOffset(first, last),
		             BootstrapBackendTest.Distance(ref origin, ref target));
	}
	private static void AssertDifferentPage<TBackend, T>(ref TBackend backend, UInt16 first, UInt16 last)
		where TBackend : struct, IMetadataStorageBackend
	{
		ref BufferTypeMetadata<T>? origin = ref backend.GetBinaryReference<T>(first);
		ref BufferTypeMetadata<T>? target = ref backend.GetBinaryReference<T>(last);
		Assert.NotEqual(BootstrapBackendTest.ContiguousOffset(first, last),
		                BootstrapBackendTest.Distance(ref origin, ref target));
	}
	private static void AssertOutOfRange(Action action)
	{
		Exception? error = Record.Exception(action);
		Assert.NotNull(error);
		Assert.True(error is IndexOutOfRangeException || error.GetType().Name.Contains("Assert"));
	}
	private static void AssertStored<TBackend, T>(UInt16 size) where TBackend : struct, IMetadataStorageBackend
	{
		MetadataStorage storage = new MetadataStorage<TBackend>();
		BufferTypeMetadata<T> metadata =
			new BufferTypeMetadata<Atomic<T>, T>(size, Array.Empty<BufferTypeMetadata<T>>(), true);
		Assert.Null(storage.GetMetadata<T>(size));
		Assert.Null(NonBinary<T>(size));
		Assert.True(storage.TryAdd(metadata));
		BootstrapBackendTest.AssertNonBinary(storage, size, metadata);
		Assert.False(storage.TryAdd(metadata));
		BootstrapBackendTest.AssertNonBinary(storage, size, metadata);
	}
	private static void AssertAdded<TBackend, T>(UInt16 size) where TBackend : struct, IMetadataStorageBackend
	{
		MetadataStorage storage = new MetadataStorage<TBackend>();
		BufferTypeMetadata<T> metadata =
			new BufferTypeMetadata<Atomic<T>, T>(size, Array.Empty<BufferTypeMetadata<T>>(), true);
		BufferTypeMetadata<T> repeated =
			new BufferTypeMetadata<Atomic<T>, T>(size, Array.Empty<BufferTypeMetadata<T>>(), true);
		Assert.Null(NonBinary<T>(size));
		Assert.Same(metadata, storage.AddBinaryMetadata(metadata));
		BootstrapBackendTest.AssertNonBinary(storage, size, metadata);
		Assert.Same(repeated, storage.AddBinaryMetadata(repeated));
		BootstrapBackendTest.AssertNonBinary(storage, size, metadata);
	}
	private static void AssertRegistered()
	{
		MetadataStorage storage = new MetadataStorage<BootstrapBackend31>();
		Assert.Null(NonBinary<RegValue>(32));
		storage.RegisterBuffer<RegValue, R32>();
		BufferTypeMetadata<RegValue>? stored = NonBinary<RegValue>(32);
		Assert.NotNull(stored);
		Assert.True(stored.IsBinary);
		Assert.Equal((UInt16)32, stored.Size);
		Assert.Same(IManagedBuffer<RegValue>.GetMetadata<R32>(), stored);
		BootstrapBackendTest.AssertNonBinary(storage, 32, stored);
		storage.RegisterBuffer<RegValue, R32>();
		BootstrapBackendTest.AssertNonBinary(storage, 32, stored);
	}
	private static void AssertPrepared<TBackend, T>(UInt16 size) where TBackend : struct, IMetadataStorageBackend
	{
		MetadataStorage storage = new MetadataStorage<TBackend>();
		Assert.Null(NonBinary<T>(size));
		storage.PrepareBinaryMetadata<T>(size);
		BufferTypeMetadata<T>? stored = NonBinary<T>(size);
		Assert.NotNull(stored);
		Assert.True(stored.IsBinary);
		Assert.Equal(size, stored.Size);
		BootstrapBackendTest.AssertNonBinary(storage, size, stored);
		storage.PrepareBinaryMetadata<T>(size);
		BootstrapBackendTest.AssertNonBinary(storage, size, stored);
	}
	private static void AssertReplaced<TBackend, T>(UInt16 size) where TBackend : struct, IMetadataStorageBackend
	{
		MetadataStorage storage = new MetadataStorage<TBackend>();
		BufferTypeMetadata<T> nonBinary =
			new BufferTypeMetadata<Atomic<T>, T>(size, Array.Empty<BufferTypeMetadata<T>>(), false);
		BufferTypeMetadata<T> binary =
			new BufferTypeMetadata<Atomic<T>, T>(size, Array.Empty<BufferTypeMetadata<T>>(), true);
		BufferTypeMetadata<T> repeated =
			new BufferTypeMetadata<Atomic<T>, T>(size, Array.Empty<BufferTypeMetadata<T>>(), true);
		MetadataStorage.NonBinaryStore<T>.AddNonBinary(nonBinary);
		BootstrapBackendTest.AssertNonBinary(storage, size, nonBinary);
		Assert.Same(binary, storage.AddBinaryMetadata(binary));
		BootstrapBackendTest.AssertNonBinary(storage, size, binary);
		Assert.False(storage.TryAdd(repeated));
		MetadataStorage.NonBinaryStore<T>.AddNonBinary(nonBinary);
		BootstrapBackendTest.AssertNonBinary(storage, size, binary);
	}
	private static void AssertPreparedOverNonBinary<TBackend, T>(UInt16 size)
		where TBackend : struct, IMetadataStorageBackend
	{
		MetadataStorage storage = new MetadataStorage<TBackend>();
		BufferTypeMetadata<T> nonBinary =
			new BufferTypeMetadata<Atomic<T>, T>(size, Array.Empty<BufferTypeMetadata<T>>(), false);
		MetadataStorage.NonBinaryStore<T>.AddNonBinary(nonBinary);
		storage.PrepareBinaryMetadata<T>(size);
		BufferTypeMetadata<T>? stored = NonBinary<T>(size);
		Assert.NotNull(stored);
		Assert.True(stored.IsBinary);
		Assert.NotSame(nonBinary, stored);
		Assert.Equal(size, stored.Size);
		BootstrapBackendTest.AssertNonBinary(storage, size, stored);
	}
	private static void AssertNonBinary<T>(MetadataStorage storage, UInt16 size, BufferTypeMetadata<T> metadata)
	{
		Assert.Same(metadata, NonBinary<T>(size));
		Assert.Same(metadata, storage.GetMetadata<T>(size));
	}
	private static Int64 ContiguousOffset(Int32 fromSize, Int32 toSize) => (toSize - fromSize) * (Int64)IntPtr.Size;
	private static Int64 Distance<T>(ref BufferTypeMetadata<T>? origin, ref BufferTypeMetadata<T>? target)
		=> Unsafe.ByteOffset(ref origin, ref target).ToInt64();

	private struct LimitValue
	{
		public String? Text { get; set; }
	}

	private struct StoreValue
	{
		public String? Text { get; set; }
	}

	private sealed class StoreObject;

	private struct PageValue
	{
		public String? Text { get; set; }
	}

	private sealed class PageObject;

	internal struct RegValue
	{
		public String? Text { get; set; }
	}

	private struct AddValue
	{
		public String? Text { get; set; }
	}

	private struct PrepValue
	{
		public String? Text { get; set; }
	}

	private struct ReplaceValue
	{
		public String? Text { get; set; }
	}

	private struct PrepReplaceValue
	{
		public String? Text { get; set; }
	}

	private static BufferTypeMetadata<T>? NonBinary<T>(UInt16 size)
		=> MetadataStorage.NonBinaryStore<T>.GetNonBinary(size, out _);
}
#endif