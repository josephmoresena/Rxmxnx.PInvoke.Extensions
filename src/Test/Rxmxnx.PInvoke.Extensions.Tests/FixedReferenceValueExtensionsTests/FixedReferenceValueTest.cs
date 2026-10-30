namespace Rxmxnx.PInvoke.Tests.FixedReferenceValueExtensionsTests;

[TestFixture]
[ExcludeFromCodeCoverage]
[SuppressMessage("csharpsquid", "S2699")]
public sealed class FixedReferenceValueTest
{
	[Fact]
	public void ReferenceTest()
	{
		Int32 value = 7;
		ReferenceAction action = new();
		action.WithSafeFixed(ref value);
		PInvokeAssert.Equal(7, action.Seen);
		PInvokeAssert.Equal(11, value);
	}
	[Fact]
	public void ReadOnlyReferenceTest()
	{
		Int32 value = 4;
		ReadOnlyReferenceAction action = new();
		action.WithSafeReadOnlyFixed(in value);
		PInvokeAssert.Equal(4, action.Seen);
		PInvokeAssert.Equal(4, value);
	}
	[Fact]
	public void TransformationResidualTest()
	{
		Int64 value = 0x0102030405060708L;
		TransformationResidualAction action = new();
		action.WithSafeFixed(ref value);
		PInvokeAssert.True(action.ResidualAtDestinationSize);
		PInvokeAssert.Equal(sizeof(Int64) - sizeof(Int32), action.ResidualBytesLength);
	}
	[Fact]
	public void ReadOnlyTransformationResidualTest()
	{
		Int64 value = 0x0102030405060708L;
		ReadOnlyTransformationResidualAction action = new();
		action.WithSafeReadOnlyFixed(in value);
		PInvokeAssert.True(action.ResidualAtDestinationSize);
		PInvokeAssert.Equal(sizeof(Int64) - sizeof(Int32), action.ResidualBytesLength);
	}
	[Fact]
	public void ExplicitCastRejectsInsufficientSizeTest()
	{
		Int16 value = 7;
		InsufficientSizeCastAction action = new();
		action.WithSafeFixed(ref value);
		PInvokeAssert.True(action.Rejected);
	}

#if NET9_0_OR_GREATER
	[Fact]
	public void SpanReferenceTest()
	{
		PInvokeAssert.True(RuntimeHelpers.IsReferenceOrContainsReferences<Span<Byte>>());

		Span<Byte> span = stackalloc Byte[] { 1, 2, 3, 4, };
		SpanReferenceAction action = new();
		action.WithSafeFixed(ref span);

		PInvokeAssert.Equal(4, action.Length);
		PInvokeAssert.Equal(1, action.First);
		PInvokeAssert.Equal(99, span[0]);
		PInvokeAssert.True(action.IsManagedLike);
		PInvokeAssert.True(action.BytesEmpty);
		PInvokeAssert.True(action.ObjectsEmpty);
	}
	[Fact]
	public void ReadOnlySpanReferenceTest()
	{
		PInvokeAssert.True(RuntimeHelpers.IsReferenceOrContainsReferences<ReadOnlySpan<Int32>>());

		ReadOnlySpan<Int32> span = stackalloc Int32[] { 7, 8, 9, };
		ReadOnlySpanReferenceAction action = new();
		action.WithSafeReadOnlyFixed(in span);

		PInvokeAssert.Equal(3, action.Length);
		PInvokeAssert.Equal(7, action.First);
		PInvokeAssert.True(action.IsManagedLike);
		PInvokeAssert.True(action.BytesEmpty);
		PInvokeAssert.True(action.ObjectsEmpty);
	}
	[Fact]
	public void CustomRefStructReferenceTest()
	{
		SampleRefStruct sample = new() { Value = 21, Flag = true, };
		PInvokeAssert.False(RuntimeHelpers.IsReferenceOrContainsReferences<SampleRefStruct>());

		SampleRefStructAction action = new();
		action.WithSafeFixed(ref sample);

		PInvokeAssert.Equal(21, action.Seen);
		PInvokeAssert.True(action.Flag);
		PInvokeAssert.Equal(42, sample.Value);
		PInvokeAssert.False(action.IsManagedLike);
	}
	[Fact]
	public void CustomRefStructReadOnlyReferenceTest()
	{
		SampleRefStruct sample = new() { Value = 5, Flag = false, };
		SampleRefStructReadOnlyAction action = new();
		action.WithSafeReadOnlyFixed(in sample);

		PInvokeAssert.Equal(5, action.Seen);
		PInvokeAssert.False(action.Flag);
		PInvokeAssert.Equal(5, sample.Value);
	}
#endif

	private sealed class ReferenceAction : IFixedReferenceAction<Int32>
	{
		public Int32 Seen;
		public void Accept(scoped FixedReferenceValue<Int32> fixedReference)
		{
			this.Seen = fixedReference.Reference;
			fixedReference.Reference = 11;
		}
	}

	private sealed class ReadOnlyReferenceAction : IReadOnlyFixedReferenceAction<Int32>
	{
		public Int32 Seen;
		public void Accept(scoped ReadOnlyFixedReferenceValue<Int32> fixedReference)
			=> this.Seen = fixedReference.Reference;
	}
	private sealed class TransformationResidualAction : IFixedReferenceAction<Int64>
	{
		public Boolean ResidualAtDestinationSize;
		public Int32 ResidualBytesLength;
		public void Accept(scoped FixedReferenceValue<Int64> fixedReference)
		{
			FixedReferenceValue<Int32> transformed = fixedReference.Transformation<Int32>(out FixedPointerValue residual);
			this.ResidualAtDestinationSize = residual.Pointer == fixedReference.Pointer + sizeof(Int32);
			FixedReferenceValue<Int32> leftover = (FixedReferenceValue<Int32>)residual;
			this.ResidualBytesLength = leftover.Bytes.Length;
			PInvokeAssert.Equal(fixedReference.Bytes.Length, transformed.Bytes.Length);
		}
	}
	private sealed class ReadOnlyTransformationResidualAction : IReadOnlyFixedReferenceAction<Int64>
	{
		public Boolean ResidualAtDestinationSize;
		public Int32 ResidualBytesLength;
		public void Accept(scoped ReadOnlyFixedReferenceValue<Int64> fixedReference)
		{
			ReadOnlyFixedReferenceValue<Int32> transformed =
				fixedReference.Transformation<Int32>(out FixedPointerValue residual);
			this.ResidualAtDestinationSize = residual.Pointer == fixedReference.Pointer + sizeof(Int32);
			ReadOnlyFixedReferenceValue<Int32> leftover = (ReadOnlyFixedReferenceValue<Int32>)residual;
			this.ResidualBytesLength = leftover.Bytes.Length;
			PInvokeAssert.Equal(fixedReference.Bytes.Length, transformed.Bytes.Length);
		}
	}
	private sealed class InsufficientSizeCastAction : IFixedReferenceAction<Int16>
	{
		public Boolean Rejected;
		public void Accept(scoped FixedReferenceValue<Int16> fixedReference)
		{
			FixedPointerValue pointer = fixedReference;
			try
			{
				_ = (FixedReferenceValue<Int32>)pointer;
				this.Rejected = false;
			}
			catch (InsufficientMemoryException)
			{
				this.Rejected = true;
			}
		}
	}

#if NET9_0_OR_GREATER
	private ref struct SampleRefStruct
	{
		public Int32 Value;
		public Boolean Flag;
	}

	private sealed class SpanReferenceAction : IFixedReferenceAction<Span<Byte>>
	{
		public Int32 Length;
		public Byte First;
		public Boolean IsManagedLike;
		public Boolean BytesEmpty;
		public Boolean ObjectsEmpty;
		public void Accept(scoped FixedReferenceValue<Span<Byte>> fixedReference)
		{
			FixedPointerValue pointer = fixedReference;
			this.IsManagedLike = !pointer.IsUnmanaged;
			this.BytesEmpty = fixedReference.Bytes.IsEmpty;
			this.ObjectsEmpty = fixedReference.Objects.IsEmpty;
			this.Length = fixedReference.Reference.Length;
			this.First = fixedReference.Reference[0];
			fixedReference.Reference[0] = 99;
			try
			{
				_ = fixedReference.Transformation<Int32>(out _);
				PInvokeAssert.True(false);
			}
			catch (InvalidOperationException) { }
		}
	}
	private sealed class ReadOnlySpanReferenceAction : IReadOnlyFixedReferenceAction<ReadOnlySpan<Int32>>
	{
		public Int32 Length;
		public Int32 First;
		public Boolean IsManagedLike;
		public Boolean BytesEmpty;
		public Boolean ObjectsEmpty;
		public void Accept(scoped ReadOnlyFixedReferenceValue<ReadOnlySpan<Int32>> fixedReference)
		{
			FixedPointerValue pointer = fixedReference;
			this.IsManagedLike = !pointer.IsUnmanaged;
			this.BytesEmpty = fixedReference.Bytes.IsEmpty;
			this.ObjectsEmpty = fixedReference.Objects.IsEmpty;
			this.Length = fixedReference.Reference.Length;
			this.First = fixedReference.Reference[0];
		}
	}
	private sealed class SampleRefStructAction : IFixedReferenceAction<SampleRefStruct>
	{
		public Int32 Seen;
		public Boolean Flag;
		public Boolean IsManagedLike;
		public void Accept(scoped FixedReferenceValue<SampleRefStruct> fixedReference)
		{
			FixedPointerValue pointer = fixedReference;
			this.IsManagedLike = !pointer.IsUnmanaged;
			this.Seen = fixedReference.Reference.Value;
			this.Flag = fixedReference.Reference.Flag;
			fixedReference.Reference.Value = 42;
		}
	}
	private sealed class SampleRefStructReadOnlyAction : IReadOnlyFixedReferenceAction<SampleRefStruct>
	{
		public Int32 Seen;
		public Boolean Flag;
		public void Accept(scoped ReadOnlyFixedReferenceValue<SampleRefStruct> fixedReference)
		{
			this.Seen = fixedReference.Reference.Value;
			this.Flag = fixedReference.Reference.Flag;
		}
	}
#endif
}