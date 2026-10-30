namespace Rxmxnx.PInvoke.Tests.FixedPointerListValueExtensionsTests;

[TestFixture]
[ExcludeFromCodeCoverage]
[SuppressMessage("csharpsquid", "S2699")]
public sealed class FixedReferenceListTest
{
	[Fact]
	public void TwoReferencesTest()
	{
		Int32 left = 1;
		Int32 right = 2;
		PairAction action = new();
		action.WithSafeFixed(ref left, ref right);
		PInvokeAssert.Equal(3, action.Sum);
		PInvokeAssert.Equal(10, left);
		PInvokeAssert.Equal(20, right);
	}
	[Fact]
	public void TwoReadOnlyReferencesTest()
	{
		Int32 left = 4;
		Int32 right = 6;
		ReadOnlyPairAction action = new();
		action.WithSafeReadOnlyFixed(in left, in right);
		PInvokeAssert.Equal(10, action.Sum);
		PInvokeAssert.Equal(4, left);
		PInvokeAssert.Equal(6, right);
	}

#if NET9_0_OR_GREATER
	[Fact]
	public void TwoSpanReferencesTest()
	{
		PInvokeAssert.True(RuntimeHelpers.IsReferenceOrContainsReferences<Span<Byte>>());

		Span<Byte> left = stackalloc Byte[] { 1, 2, };
		Span<Byte> right = stackalloc Byte[] { 3, 4, };
		SpanPairAction action = new();
		action.WithSafeFixed(ref left, ref right);

		PInvokeAssert.Equal(2, action.LeftLength);
		PInvokeAssert.Equal(2, action.RightLength);
		PInvokeAssert.Equal(1, action.LeftFirst);
		PInvokeAssert.Equal(3, action.RightFirst);
		PInvokeAssert.Equal(99, left[0]);
		PInvokeAssert.Equal(88, right[0]);
		PInvokeAssert.True(action.LeftManagedLike);
		PInvokeAssert.True(action.RightManagedLike);
		PInvokeAssert.True(action.BytesEmpty);
		PInvokeAssert.True(action.ObjectsEmpty);
	}
	[Fact]
	public void TwoReadOnlySpanReferencesTest()
	{
		PInvokeAssert.True(RuntimeHelpers.IsReferenceOrContainsReferences<ReadOnlySpan<Int32>>());

		ReadOnlySpan<Int32> left = stackalloc Int32[] { 7, 8, };
		ReadOnlySpan<Int32> right = stackalloc Int32[] { 9, 10, };
		ReadOnlySpanPairAction action = new();
		action.WithSafeReadOnlyFixed(in left, in right);

		PInvokeAssert.Equal(2, action.LeftLength);
		PInvokeAssert.Equal(2, action.RightLength);
		PInvokeAssert.Equal(7, action.LeftFirst);
		PInvokeAssert.Equal(9, action.RightFirst);
		PInvokeAssert.True(action.LeftManagedLike);
		PInvokeAssert.True(action.RightManagedLike);
	}
	[Fact]
	public void TwoUnmanagedRefStructReferencesTest()
	{
		SampleRefStruct left = new() { Value = 11, Flag = true, };
		SampleRefStruct right = new() { Value = 22, Flag = false, };
		PInvokeAssert.False(RuntimeHelpers.IsReferenceOrContainsReferences<SampleRefStruct>());

		SampleRefStructPairAction action = new();
		action.WithSafeFixed(ref left, ref right);

		PInvokeAssert.Equal(11, action.LeftSeen);
		PInvokeAssert.Equal(22, action.RightSeen);
		PInvokeAssert.True(action.LeftFlag);
		PInvokeAssert.False(action.RightFlag);
		PInvokeAssert.Equal(33, left.Value);
		PInvokeAssert.Equal(44, right.Value);
		PInvokeAssert.False(action.LeftManagedLike);
		PInvokeAssert.False(action.RightManagedLike);
	}
#endif

	private sealed class PairAction : IFixedPointerListAction
	{
		public Int32 Sum;
		public void Accept(scoped FixedPointerValueList list)
		{
			FixedReferenceValue<Int32> left = (FixedReferenceValue<Int32>)list[0].Value;
			FixedReferenceValue<Int32> right = (FixedReferenceValue<Int32>)list[1].Value;
			this.Sum = left.Reference + right.Reference;
			left.Reference = 10;
			right.Reference = 20;
		}
	}

	private sealed class ReadOnlyPairAction : IFixedPointerListAction
	{
		public Int32 Sum;
		public void Accept(scoped FixedPointerValueList list)
		{
			ReadOnlyFixedReferenceValue<Int32> left = (ReadOnlyFixedReferenceValue<Int32>)list[0].Value;
			ReadOnlyFixedReferenceValue<Int32> right = (ReadOnlyFixedReferenceValue<Int32>)list[1].Value;
			this.Sum = left.Reference + right.Reference;
		}
	}

#if NET9_0_OR_GREATER
	private ref struct SampleRefStruct
	{
		public Int32 Value;
		public Boolean Flag;
	}

	private sealed class SpanPairAction : IFixedPointerListAction
	{
		public Int32 LeftLength;
		public Int32 RightLength;
		public Byte LeftFirst;
		public Byte RightFirst;
		public Boolean LeftManagedLike;
		public Boolean RightManagedLike;
		public Boolean BytesEmpty;
		public Boolean ObjectsEmpty;
		public void Accept(scoped FixedPointerValueList list)
		{
			FixedReferenceValue<Span<Byte>> left = (FixedReferenceValue<Span<Byte>>)list[0].Value;
			FixedReferenceValue<Span<Byte>> right = (FixedReferenceValue<Span<Byte>>)list[1].Value;
			FixedPointerValue leftPointer = left;
			FixedPointerValue rightPointer = right;
			this.LeftManagedLike = !leftPointer.IsUnmanaged;
			this.RightManagedLike = !rightPointer.IsUnmanaged;
			this.BytesEmpty = left.Bytes.IsEmpty && right.Bytes.IsEmpty;
			this.ObjectsEmpty = left.Objects.IsEmpty && right.Objects.IsEmpty;
			this.LeftLength = left.Reference.Length;
			this.RightLength = right.Reference.Length;
			this.LeftFirst = left.Reference[0];
			this.RightFirst = right.Reference[0];
			left.Reference[0] = 99;
			right.Reference[0] = 88;

			try
			{
				_ = left.Transformation<Int32>(out _);
				PInvokeAssert.True(false);
			}
			catch (InvalidOperationException) { }
			try
			{
				_ = right.Transformation<Byte>(out _);
				PInvokeAssert.True(false);
			}
			catch (InvalidOperationException) { }
		}
	}
	private sealed class ReadOnlySpanPairAction : IFixedPointerListAction
	{
		public Int32 LeftLength;
		public Int32 RightLength;
		public Int32 LeftFirst;
		public Int32 RightFirst;
		public Boolean LeftManagedLike;
		public Boolean RightManagedLike;
		public void Accept(scoped FixedPointerValueList list)
		{
			ReadOnlyFixedReferenceValue<ReadOnlySpan<Int32>> left =
				(ReadOnlyFixedReferenceValue<ReadOnlySpan<Int32>>)list[0].Value;
			ReadOnlyFixedReferenceValue<ReadOnlySpan<Int32>> right =
				(ReadOnlyFixedReferenceValue<ReadOnlySpan<Int32>>)list[1].Value;
			FixedPointerValue leftPointer = left;
			FixedPointerValue rightPointer = right;
			this.LeftManagedLike = !leftPointer.IsUnmanaged;
			this.RightManagedLike = !rightPointer.IsUnmanaged;
			this.LeftLength = left.Reference.Length;
			this.RightLength = right.Reference.Length;
			this.LeftFirst = left.Reference[0];
			this.RightFirst = right.Reference[0];

			try
			{
				_ = left.Transformation<Int32>(out _);
				PInvokeAssert.True(false);
			}
			catch (InvalidOperationException) { }
		}
	}
	private sealed class SampleRefStructPairAction : IFixedPointerListAction
	{
		public Int32 LeftSeen;
		public Int32 RightSeen;
		public Boolean LeftFlag;
		public Boolean RightFlag;
		public Boolean LeftManagedLike;
		public Boolean RightManagedLike;
		public void Accept(scoped FixedPointerValueList list)
		{
			FixedReferenceValue<SampleRefStruct> left = (FixedReferenceValue<SampleRefStruct>)list[0].Value;
			FixedReferenceValue<SampleRefStruct> right = (FixedReferenceValue<SampleRefStruct>)list[1].Value;
			FixedPointerValue leftPointer = left;
			FixedPointerValue rightPointer = right;
			this.LeftManagedLike = !leftPointer.IsUnmanaged;
			this.RightManagedLike = !rightPointer.IsUnmanaged;
			this.LeftSeen = left.Reference.Value;
			this.RightSeen = right.Reference.Value;
			this.LeftFlag = left.Reference.Flag;
			this.RightFlag = right.Reference.Flag;
			left.Reference.Value = 33;
			right.Reference.Value = 44;
		}
	}
#endif
}