namespace Rxmxnx.PInvoke.Tests.CStringTests;

[TestFixture]
[ExcludeFromCodeCoverage]
public sealed class BackingTest
{
	[Theory]
	[InlineData(8)]
	[InlineData(32)]
	[InlineData(128)]
	public void Test(Int32? length)
	{
		using TestMemoryHandle handle = new();
		List<Int32> indices = TestSet.GetIndices(length);
		CString?[] values = indices.Select(i => TestSet.GetCString(i, handle)).ToArray();
		Byte[,] array2D = BackingTest.CreateFor(values);
		CString[] backedValues = Backing2D.GetValues(array2D).ToArray();

		for (Int32 i = 0; i < values.Length; i++)
		{
			BackingTest.Assert2D(values[i], backedValues[i]);
			BackingTest.AssertNormalize2D(values[i]);
			BackingTest.AssertSegment(backedValues[i]);
		}
		PInvokeAssert.True(values.All(c => Backing2D.GetBacking(c) is null));
		PInvokeAssert.True(values.All(c => (CString.Backing?)c is null));
	}
	private static void AssertSegment(CString backedValue)
	{
		for (Int32 i = 0; i < backedValue.Length; i++)
		{
			Int32 start = Random.Shared.Next(i, backedValue.Length);
			Int32 end = Random.Shared.Next(start, backedValue.Length + 1);

			CString segment = backedValue[start..end];
			if (segment.Length == 0) continue;

			PInvokeAssert.True(segment.IsSegmented);
			PInvokeAssert.False(segment.IsFunction);
			PInvokeAssert.False(segment.IsZero);
			PInvokeAssert.Equal(end == backedValue.Length, segment.IsNullTerminated);

			ReadOnlyMemory<Byte> backedMemory = UnsafeMemoryBacking.GetMemory(backedValue);
			ReadOnlyMemory<Byte> segmentMemory = UnsafeMemoryBacking.GetMemory(segment);

			PInvokeAssert.Equal(start == 0 && end == backedValue.Length,
			                    backedMemory.Span.SequenceEqual(segmentMemory.Span));

			using MemoryHandle handle0 = segment.TryPin(out Boolean pinned);
			using MemoryHandle handle1 = segmentMemory.Pin();

			PInvokeAssert.True(pinned);
			PInvokeAssert.Equal(handle0, handle1);
		}
	}
	private static void AssertNormalize2D(CString? value)
	{
		CString? normalized = Backing2D.Normalize(value);
		Backing2D? back = Backing2D.GetBacking(normalized);

		if (normalized is null)
		{
			PInvokeAssert.Null(value);
			return;
		}

		PInvokeAssert.NotNull(back);
		PInvokeAssert.NotNull(value);
		try
		{
			if (back.Array is not null)
			{
				PInvokeAssert.True(value.IsFunction || value.IsReference);
				return;
			}

			PInvokeAssert.True(MemoryMarshal.TryGetArray(back.Value, out ArraySegment<Byte> segment));
			PInvokeAssert.Equal(segment.Offset != 0 || segment.Array?.Length != segment.Count, normalized.IsSegmented);
		}
		finally
		{
			using MemoryHandle handle0 = normalized.TryPin(out Boolean pinned);
			PInvokeAssert.True(pinned);
		}
	}
	private static void Assert2D(CString? value, CString backedValue)
	{
		PInvokeAssert.IsType<Backing2D>((CString.Backing?)backedValue);
		PInvokeAssert.True(backedValue.AsSpan().SequenceEqual(value));
		PInvokeAssert.Null(Backing2D.GetBacking(value));
		PInvokeAssert.NotNull(Backing2D.GetBacking(backedValue));
		PInvokeAssert.StrictEqual(backedValue, Backing2D.Normalize(backedValue));
		PInvokeAssert.False(backedValue.IsFunction);
		PInvokeAssert.False(backedValue.IsReference);
		PInvokeAssert.False(backedValue.IsZero);
		PInvokeAssert.True(backedValue.IsNullTerminated);
	}

	private static Byte[,] CreateFor(CString?[] values)
	{
		Int32 maxLength = values.Select(c => c?.Length ?? 0).Max();
		Byte[,] result = new Byte[values.Length, maxLength + 1];
		for (Int32 i = 0; i < values.Length; i++)
		{
			ReadOnlySpan<Byte> value = values[i];
			BackingTest.CopyValue(result, i, value);
		}
		return result;
	}
	private static void CopyValue(Byte[,] array, Int32 index, ReadOnlySpan<Byte> value)
	{
		for (Int32 i = 0; i < value.Length; i++)
			array[index, i] = value[i];
	}

	private sealed class UnsafeMemoryBacking(ReadOnlyMemory<Byte> memory) : CString.Backing(memory, true)
	{
		protected override ValueRegion<Byte> Slice(ReadOnlyMemory<Byte> memory) => new UnsafeMemoryBacking(memory);

		public static ReadOnlyMemory<Byte> GetMemory(CString? value)
		{
			if (value is null) return default;
			return CString.Backing.TryGetMemory(value, !value.IsNullTerminated, out ReadOnlyMemory<Byte> mem) ?
				mem :
				value.ToArray();
		}
	}

	private sealed class Backing2D : CString.Backing
	{
		public new ReadOnlyMemory<Byte> Value => base.Value;
		public Byte[,]? Array { get; }

		private Backing2D(Byte[,]? array) : this(array, ArrayMemoryManager<Byte>.GetMemory(array)) { }
		private Backing2D(ReadOnlyMemory<Byte> memory) : this(default, memory) { }
		private Backing2D(Byte[,]? array, ReadOnlyMemory<Byte> memory) : base(memory, false) => this.Array = array;
		protected override ValueRegion<Byte> Slice(ReadOnlyMemory<Byte> memory) => new UnsafeMemoryBacking(memory);

		[return: NotNullIfNotNull(nameof(value))]
		public static CString? Normalize(CString? value)
		{
			if (value is null) return default;
			if (CString.Backing.TryGetBacking<Backing2D>(value, out _)) return value;
			if (CString.Backing.TryGetMemory(value, true, out ReadOnlyMemory<Byte> mem))
				return new Backing2D(mem);
			ReadOnlySpan<Byte> span = value.AsSpan();
			Byte[,] array = new Byte[1, span.Length + 1];
			BackingTest.CopyValue(array, 0, span);
			return new Backing2D(array);
		}

		public static IEnumerable<CString> GetValues(Byte[,] array)
		{
			ReadOnlyMemory<Byte> mem = ArrayMemoryManager<Byte>.GetMemory(array);
			for (Int32 i = 0; i < array.GetLength(0); i++)
				yield return new Backing2D(array, mem.Slice(i * array.GetLength(1), array.GetLength(1)));
		}
		public static Backing2D? GetBacking(CString? value)
			=> CString.Backing.TryGetBacking(value, out Backing2D result) ? result : default;
	}
#if !NET6_0_OR_GREATER
	private static class Random
	{
		public static readonly System.Random Shared = new();
	}
#endif
}