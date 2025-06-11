namespace Rxmxnx.PInvoke.Tests.CStringSequenceTests;

[ExcludeFromCodeCoverage]
public sealed unsafe class MarshallerTests
{
	[Fact]
	internal void NullTest()
	{
		CStringSequence? seqNull = default;
		CStringSequence.InputMarshaller marshaller = new();

		marshaller.FromManaged(seqNull);
		Assert.Equal(IntPtr.Zero, marshaller.ToUnmanaged());
		marshaller.FromManaged(MarshallerTests.GetInterop(seqNull));
		Assert.Equal(IntPtr.Zero, marshaller.ToUnmanaged());
	}
	[Theory]
	[InlineData(null)]
	[InlineData(8)]
	[InlineData(32)]
	[InlineData(256)]
	[InlineData(300)]
	[InlineData(1000)]
	internal void FromManagedTest(Int32? length)
	{
		using TestMemoryHandle handle = new();
		IReadOnlyList<Int32> indices = TestSet.GetIndices(length);
		CStringSequence seq = new(TestSet.GetValues(indices, handle));
		String buffer = seq.ToString();

		CStringSequence.InputMarshaller marshaller = new();
		marshaller.FromManaged(seq);

		IntPtr ptr = marshaller.ToUnmanaged();
		try
		{
			ReadOnlySpan<ReadOnlyValPtr<Byte>> values = new(ptr.ToPointer(), seq.NonEmptyCount);
			Span<Int32> offset = stackalloc Int32[values.Length];
			CStringSequence result = CStringSequence.CreateUnsafe(values);

			Assert.Equal(buffer, result.ToString());
			Assert.Equal(seq.NonEmptyCount, result.Count);
			Assert.Equal(seq.NonEmptyCount, result.NonEmptyCount);

			seq.GetOffsets(offset);
			fixed (void* fPtr = &MemoryMarshal.GetReference<Char>(buffer))
			{
				ReadOnlyValPtr<Byte> seqPtr = (ReadOnlyValPtr<Byte>)fPtr;
				for (Int32 i = 0; i < offset.Length; i++)
					Assert.Equal(seqPtr + offset[i], values[i]);
			}
			Assert.Equal(ptr, marshaller.ToUnmanaged());
		}
		finally
		{
			marshaller.Free();
		}
	}
	[Theory]
	[InlineData(null)]
	[InlineData(8)]
	[InlineData(32)]
	[InlineData(256)]
	[InlineData(300)]
	[InlineData(1000)]
	internal void FromInteropTest(Int32? length)
	{
		using TestMemoryHandle handle = new();
		IReadOnlyList<Int32> indices = TestSet.GetIndices(length);
		CStringSequence seq = new(TestSet.GetValues(indices, handle));
		String buffer = seq.ToString();
		CStringSequence.Interop interop = MarshallerTests.GetInterop(seq);

		CStringSequence.InputMarshaller marshaller = new();
		marshaller.FromManaged(interop);

		IntPtr ptr = marshaller.ToUnmanaged();
		try
		{
			ReadOnlySpan<ReadOnlyValPtr<Byte>> values = new(ptr.ToPointer(), seq.Count);
			CStringSequence result = CStringSequence.CreateUnsafe(values);

			Assert.Equal(buffer, result.ToString());
			Assert.Equal(seq.Count, result.Count);
			Assert.Equal(seq.NonEmptyCount, result.NonEmptyCount);

			Assert.Equal(seq, result);
			Assert.True(seq == interop);
			Assert.False(result != interop);
			Assert.True(interop.Equals((Object)seq));
			Assert.True(interop.Equals((Object)interop));
		}
		finally
		{
			marshaller.Free();
		}
	}

	private static CStringSequence.Interop GetInterop(CStringSequence? seq)
	{
		CStringSequence.Interop interop = seq;

		Assert.Equal(seq?.Count, interop.Count);
		Assert.Equal(seq, interop.Value);
		Assert.Equal(seq?.ToString(), interop.ToString());
		Assert.Equal(seq?.GetHashCode() ?? 0, interop.GetHashCode());

		return interop;
	}
}