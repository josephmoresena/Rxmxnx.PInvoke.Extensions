#if !NETCOREAPP
using Fact = NUnit.Framework.TestAttribute;
using InlineData = NUnit.Framework.TestCaseAttribute;
#endif

namespace Rxmxnx.PInvoke.Tests.CStringSequenceTests;

[TestFixture]
[ExcludeFromCodeCoverage]
public sealed unsafe class MarshallerTests
{
	[Fact]
	public void NullTest()
	{
		CStringSequence? seqNull = default;
		CStringSequence.InputMarshaller marshaller = new();

		marshaller.FromManaged(seqNull);
		PInvokeAssert.Equal(IntPtr.Zero, marshaller.ToUnmanaged());
		marshaller.FromManaged(MarshallerTests.GetValue(seqNull, true));
		PInvokeAssert.Equal(IntPtr.Zero, marshaller.ToUnmanaged());
	}
	[Fact]
	public void EmptyTest()
	{
		CStringSequence empty = CStringSequence.Empty;
		CStringSequence.InputMarshaller marshaller = new();

		marshaller.FromManaged(empty);
		PInvokeAssert.NotEqual(IntPtr.Zero, marshaller.ToUnmanaged());
		marshaller.Free();
		marshaller.FromManaged(MarshallerTests.GetValue(empty, false));
		PInvokeAssert.NotEqual(IntPtr.Zero, marshaller.ToUnmanaged());
		marshaller.Free();
	}
	[Theory]
	[InlineData(null)]
	[InlineData(8)]
	[InlineData(32)]
	[InlineData(256)]
	[InlineData(300)]
	[InlineData(1000)]
	public void FromManagedTest(Int32? length)
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

			PInvokeAssert.Equal(buffer, result.ToString());
			PInvokeAssert.Equal(seq.NonEmptyCount, result.Count);
			PInvokeAssert.Equal(seq.NonEmptyCount, result.NonEmptyCount);

			seq.GetOffsets(offset);
			fixed (void* fPtr = &MemoryMarshal.GetReference<Char>(buffer))
			{
				ReadOnlyValPtr<Byte> seqPtr = (ReadOnlyValPtr<Byte>)fPtr;
				for (Int32 i = 0; i < offset.Length; i++)
					PInvokeAssert.Equal(seqPtr + offset[i], values[i]);
			}
			PInvokeAssert.Equal(ptr, marshaller.ToUnmanaged());
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
	public void FromValueTest(Int32? length)
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

			PInvokeAssert.Equal(buffer, result.ToString());
			PInvokeAssert.Equal(seq.Count, result.Count);
			PInvokeAssert.Equal(seq.NonEmptyCount, result.NonEmptyCount);

			PInvokeAssert.Equal(seq, result);
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
	public void FromValueNonEmptyOnlyTest(Int32? length)
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

			PInvokeAssert.Equal(buffer, result.ToString());
			PInvokeAssert.Equal(seq.NonEmptyCount, result.Count);
			PInvokeAssert.Equal(seq.NonEmptyCount, result.NonEmptyCount);
			PInvokeAssert.Equal(seq.Count == seq.NonEmptyCount, value.Equals(result));

			seq.GetOffsets(offset);
			fixed (void* fPtr = &MemoryMarshal.GetReference<Char>(buffer))
			{
				ReadOnlyValPtr<Byte> seqPtr = (ReadOnlyValPtr<Byte>)fPtr;
				for (Int32 i = 0; i < offset.Length; i++)
					PInvokeAssert.Equal(seqPtr + offset[i], values[i]);
			}
			PInvokeAssert.Equal(ptr, marshaller.ToUnmanaged());
		}
		finally
		{
			marshaller.Free();
		}
	}

	private static CStringSequence.Utf8View GetValue(CStringSequence? seq, Boolean includeEmptyItems)
	{
		CStringSequence.Utf8View value = seq.CreateView(includeEmptyItems);
		PInvokeAssert.Equal((includeEmptyItems ? seq?.Count : seq?.NonEmptyCount).GetValueOrDefault(), value.Count);
		PInvokeAssert.Equal(seq, value.Source);
		PInvokeAssert.Equal(seq?.ToString(), value.ToString());
		PInvokeAssert.Equal(seq?.GetHashCode() ?? 0, value.GetHashCode());
		PInvokeAssert.True(value.Equals(seq));
		PInvokeAssert.True(value == seq.CreateView(includeEmptyItems));
		PInvokeAssert.True(value != seq.CreateView(!includeEmptyItems));

		return value;
	}
}