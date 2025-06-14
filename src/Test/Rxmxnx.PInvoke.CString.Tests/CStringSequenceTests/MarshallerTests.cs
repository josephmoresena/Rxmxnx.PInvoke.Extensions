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
		marshaller.FromManaged(MarshallerTests.GetValue(seqNull, true));
		Assert.Equal(IntPtr.Zero, marshaller.ToUnmanaged());
	}
	[Fact]
	internal void EmptyTest()
	{
		CStringSequence empty = CStringSequence.Empty;
		CStringSequence.InputMarshaller marshaller = new();

		marshaller.FromManaged(empty);
		Assert.NotEqual(IntPtr.Zero, marshaller.ToUnmanaged());
		marshaller.Free();
		marshaller.FromManaged(MarshallerTests.GetValue(empty, false));
		Assert.NotEqual(IntPtr.Zero, marshaller.ToUnmanaged());
		marshaller.Free();
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
			CStringSequence result = CStringSequence.GetUnsafe(values);

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
	internal void FromValueTest(Int32? length)
	{
		using TestMemoryHandle handle = new();
		IReadOnlyList<Int32> indices = TestSet.GetIndices(length);
		CStringSequence seq = new(TestSet.GetValues(indices, handle));
		String buffer = seq.ToString();
		CStringSequence.Utf8View value = MarshallerTests.GetValue(seq, true);

		CStringSequence.InputMarshaller marshaller = new();
		marshaller.FromManaged(value);

		IntPtr ptr = marshaller.ToUnmanaged();
		try
		{
			ReadOnlySpan<ReadOnlyValPtr<Byte>> values = new(ptr.ToPointer(), seq.Count);
			CStringSequence result = CStringSequence.GetUnsafe(values);

			Assert.Equal(buffer, result.ToString());
			Assert.Equal(seq.Count, result.Count);
			Assert.Equal(seq.NonEmptyCount, result.NonEmptyCount);

			Assert.Equal(seq, result);
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
	internal void FromValueNonEmptyOnlyTest(Int32? length)
	{
		using TestMemoryHandle handle = new();
		IReadOnlyList<Int32> indices = TestSet.GetIndices(length);
		CStringSequence seq = new(TestSet.GetValues(indices, handle));
		String buffer = seq.ToString();
		CStringSequence.Utf8View value = MarshallerTests.GetValue(seq, false);

		CStringSequence.InputMarshaller marshaller = new();
		marshaller.FromManaged(value);

		IntPtr ptr = marshaller.ToUnmanaged();
		try
		{
			ReadOnlySpan<ReadOnlyValPtr<Byte>> values = new(ptr.ToPointer(), seq.NonEmptyCount);
			Span<Int32> offset = stackalloc Int32[values.Length];
			CStringSequence result = CStringSequence.GetUnsafe(values);

			Assert.Equal(buffer, result.ToString());
			Assert.Equal(seq.NonEmptyCount, result.Count);
			Assert.Equal(seq.NonEmptyCount, result.NonEmptyCount);
			Assert.Equal(seq.Count == seq.NonEmptyCount, value.Equals(result));

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

	private static CStringSequence.Utf8View GetValue(CStringSequence? seq, Boolean includeEmptyItems)
	{
		CStringSequence.Utf8View value = seq.CreateView(includeEmptyItems);
		Assert.Equal((includeEmptyItems ? seq?.Count : seq?.NonEmptyCount).GetValueOrDefault(), value.Count);
		Assert.Equal(seq, value.Source);
		Assert.Equal(seq?.ToString(), value.ToString());
		Assert.Equal(seq?.GetHashCode() ?? 0, value.GetHashCode());
		Assert.True(value.Equals(seq));
		Assert.True(value == seq.CreateView(includeEmptyItems));
		Assert.True(value != seq.CreateView(!includeEmptyItems));

		return value;
	}
}