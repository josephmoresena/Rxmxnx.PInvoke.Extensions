namespace Rxmxnx.PInvoke.Tests.FixedPointerListValueExtensionsTests;

[TestFixture]
[ExcludeFromCodeCoverage]
[SuppressMessage("csharpsquid", "S2699")]
public sealed class FixedReferenceListTest
{
	[Fact]
	public void Test()
	{
		Byte v0 = 1;
		Int16 v1 = 2;
		Int32 v2 = 3;
		Int64 v3 = 4;
		SByte v4 = 5;
		UInt16 v5 = 6;
		UInt32 v6 = 7;
		Char v7 = 'A';

		FixedReferenceListAction action = new();
		FixedReferenceListFunction func = new();

		action.WithSafeFixed(ref v0, ref v1);
		func.WithSafeFixed(ref v0, ref v1, out Int32[] result);
		FixedReferenceListTest.AssertValues([1, 2,], result);

		action.WithSafeFixed(ref v0, ref v1, ref v2);
		func.WithSafeFixed(ref v0, ref v1, ref v2, out result);
		FixedReferenceListTest.AssertValues([1, 2, 3,], result);

		action.WithSafeFixed(ref v0, ref v1, ref v2, ref v3);
		func.WithSafeFixed(ref v0, ref v1, ref v2, ref v3, out result);
		FixedReferenceListTest.AssertValues([1, 2, 3, 4,], result);

		action.WithSafeFixed(ref v0, ref v1, ref v2, ref v3, ref v4);
		func.WithSafeFixed(ref v0, ref v1, ref v2, ref v3, ref v4, out result);
		FixedReferenceListTest.AssertValues([1, 2, 3, 4, 5,], result);

		action.WithSafeFixed(ref v0, ref v1, ref v2, ref v3, ref v4, ref v5);
		func.WithSafeFixed(ref v0, ref v1, ref v2, ref v3, ref v4, ref v5, out result);
		FixedReferenceListTest.AssertValues([1, 2, 3, 4, 5, 6,], result);

		action.WithSafeFixed(ref v0, ref v1, ref v2, ref v3, ref v4, ref v5, ref v6);
		func.WithSafeFixed(ref v0, ref v1, ref v2, ref v3, ref v4, ref v5, ref v6, out result);
		FixedReferenceListTest.AssertValues([1, 2, 3, 4, 5, 6, 7,], result);

		action.WithSafeFixed(ref v0, ref v1, ref v2, ref v3, ref v4, ref v5, ref v6, ref v7);
		func.WithSafeFixed(ref v0, ref v1, ref v2, ref v3, ref v4, ref v5, ref v6, ref v7, out result);
		FixedReferenceListTest.AssertValues([1, 2, 3, 4, 5, 6, 7, 'A',], result);
	}
	[Fact]
	public void ReadOnlyTest()
	{
		Byte v0 = 1;
		Int16 v1 = 2;
		Int32 v2 = 3;
		Int64 v3 = 4;
		SByte v4 = 5;
		UInt16 v5 = 6;
		UInt32 v6 = 7;
		Char v7 = 'A';

		FixedReferenceListReadOnlyAction action = new();
		FixedReferenceListReadOnlyFunction func = new();

		action.WithSafeReadOnlyFixed(in v0, in v1);
		func.WithSafeReadOnlyFixed(in v0, in v1, out Int32[] result);
		FixedReferenceListTest.AssertValues([1, 2,], result);

		action.WithSafeReadOnlyFixed(in v0, in v1, in v2);
		func.WithSafeReadOnlyFixed(in v0, in v1, in v2, out result);
		FixedReferenceListTest.AssertValues([1, 2, 3,], result);

		action.WithSafeReadOnlyFixed(in v0, in v1, in v2, in v3);
		func.WithSafeReadOnlyFixed(in v0, in v1, in v2, in v3, out result);
		FixedReferenceListTest.AssertValues([1, 2, 3, 4,], result);

		action.WithSafeReadOnlyFixed(in v0, in v1, in v2, in v3, in v4);
		func.WithSafeReadOnlyFixed(in v0, in v1, in v2, in v3, in v4, out result);
		FixedReferenceListTest.AssertValues([1, 2, 3, 4, 5,], result);

		action.WithSafeReadOnlyFixed(in v0, in v1, in v2, in v3, in v4, in v5);
		func.WithSafeReadOnlyFixed(in v0, in v1, in v2, in v3, in v4, in v5, out result);
		FixedReferenceListTest.AssertValues([1, 2, 3, 4, 5, 6,], result);

		action.WithSafeReadOnlyFixed(in v0, in v1, in v2, in v3, in v4, in v5, in v6);
		func.WithSafeReadOnlyFixed(in v0, in v1, in v2, in v3, in v4, in v5, in v6, out result);
		FixedReferenceListTest.AssertValues([1, 2, 3, 4, 5, 6, 7,], result);

		action.WithSafeReadOnlyFixed(in v0, in v1, in v2, in v3, in v4, in v5, in v6, in v7);
		func.WithSafeReadOnlyFixed(in v0, in v1, in v2, in v3, in v4, in v5, in v6, in v7, out result);
		FixedReferenceListTest.AssertValues([1, 2, 3, 4, 5, 6, 7, 'A',], result);
	}
	[Fact]
	public void WriteTest()
	{
		Int32 left = 1;
		Int32 right = 2;
		MutatingPairAction action = new();
		action.WithSafeFixed(ref left, ref right);
		PInvokeAssert.Equal(3, action.Sum);
		PInvokeAssert.Equal(10, left);
		PInvokeAssert.Equal(20, right);
	}
	[Fact]
	public void ReadOnlyPairTest()
	{
		Int32 left = 4;
		Int32 right = 6;
		ReadOnlyPairAction action = new();
		action.WithSafeReadOnlyFixed(in left, in right);
		PInvokeAssert.Equal(10, action.Sum);
		PInvokeAssert.Equal(4, left);
		PInvokeAssert.Equal(6, right);
	}
	[Fact]
	public void NullTest()
	{
		Byte v0 = default;
		Int16 v1 = default;
		Int32 v2 = default;
		Int64 v3 = default;
		SByte v4 = default;
		UInt16 v5 = default;
		UInt32 v6 = default;
		Char v7 = default;

		default(IFixedPointerListAction).WithSafeFixed(ref v0, ref v1);
		default(IFixedPointerListFunction<Int32[]>).WithSafeFixed(ref v0, ref v1, out Int32[] result);
		PInvokeAssert.Null(result);
		default(IFixedPointerListAction).WithSafeFixed(ref v0, ref v1, ref v2);
		default(IFixedPointerListFunction<Int32[]>).WithSafeFixed(ref v0, ref v1, ref v2, out result);
		PInvokeAssert.Null(result);
		default(IFixedPointerListAction).WithSafeFixed(ref v0, ref v1, ref v2, ref v3);
		default(IFixedPointerListFunction<Int32[]>).WithSafeFixed(ref v0, ref v1, ref v2, ref v3, out result);
		PInvokeAssert.Null(result);
		default(IFixedPointerListAction).WithSafeFixed(ref v0, ref v1, ref v2, ref v3, ref v4);
		default(IFixedPointerListFunction<Int32[]>).WithSafeFixed(ref v0, ref v1, ref v2, ref v3, ref v4, out result);
		PInvokeAssert.Null(result);
		default(IFixedPointerListAction).WithSafeFixed(ref v0, ref v1, ref v2, ref v3, ref v4, ref v5);
		default(IFixedPointerListFunction<Int32[]>).WithSafeFixed(ref v0, ref v1, ref v2, ref v3, ref v4, ref v5,
		                                                          out result);
		PInvokeAssert.Null(result);
		default(IFixedPointerListAction).WithSafeFixed(ref v0, ref v1, ref v2, ref v3, ref v4, ref v5, ref v6);
		default(IFixedPointerListFunction<Int32[]>).WithSafeFixed(ref v0, ref v1, ref v2, ref v3, ref v4, ref v5,
		                                                          ref v6, out result);
		PInvokeAssert.Null(result);
		default(IFixedPointerListAction).WithSafeFixed(ref v0, ref v1, ref v2, ref v3, ref v4, ref v5, ref v6, ref v7);
		default(IFixedPointerListFunction<Int32[]>).WithSafeFixed(ref v0, ref v1, ref v2, ref v3, ref v4, ref v5,
		                                                          ref v6, ref v7, out result);
		PInvokeAssert.Null(result);

		default(IFixedPointerListAction).WithSafeReadOnlyFixed(in v0, in v1);
		default(IFixedPointerListFunction<Int32[]>).WithSafeReadOnlyFixed(in v0, in v1, out result);
		PInvokeAssert.Null(result);
		default(IFixedPointerListAction).WithSafeReadOnlyFixed(in v0, in v1, in v2);
		default(IFixedPointerListFunction<Int32[]>).WithSafeReadOnlyFixed(in v0, in v1, in v2, out result);
		PInvokeAssert.Null(result);
		default(IFixedPointerListAction).WithSafeReadOnlyFixed(in v0, in v1, in v2, in v3);
		default(IFixedPointerListFunction<Int32[]>).WithSafeReadOnlyFixed(in v0, in v1, in v2, in v3, out result);
		PInvokeAssert.Null(result);
		default(IFixedPointerListAction).WithSafeReadOnlyFixed(in v0, in v1, in v2, in v3, in v4);
		default(IFixedPointerListFunction<Int32[]>).WithSafeReadOnlyFixed(in v0, in v1, in v2, in v3, in v4,
		                                                                  out result);
		PInvokeAssert.Null(result);
		default(IFixedPointerListAction).WithSafeReadOnlyFixed(in v0, in v1, in v2, in v3, in v4, in v5);
		default(IFixedPointerListFunction<Int32[]>).WithSafeReadOnlyFixed(in v0, in v1, in v2, in v3, in v4, in v5,
		                                                                  out result);
		PInvokeAssert.Null(result);
		default(IFixedPointerListAction).WithSafeReadOnlyFixed(in v0, in v1, in v2, in v3, in v4, in v5, in v6);
		default(IFixedPointerListFunction<Int32[]>).WithSafeReadOnlyFixed(in v0, in v1, in v2, in v3, in v4, in v5,
		                                                                  in v6, out result);
		PInvokeAssert.Null(result);
		default(IFixedPointerListAction).WithSafeReadOnlyFixed(in v0, in v1, in v2, in v3, in v4, in v5, in v6, in v7);
		default(IFixedPointerListFunction<Int32[]>).WithSafeReadOnlyFixed(in v0, in v1, in v2, in v3, in v4, in v5,
		                                                                  in v6, in v7, out result);
		PInvokeAssert.Null(result);
	}
	[Fact]
	public void ClassCallbackTest()
	{
		Byte v0 = 1;
		Int16 v1 = 2;
		Int32 v2 = 3;
		Int64 v3 = 4;
		SByte v4 = 5;
		UInt16 v5 = 6;
		UInt32 v6 = 7;
		Char v7 = 'A';

		// Reference-type callbacks force NonGenericAction / NonGenericFunction wrappers.
		ClassReferenceListAction action = new();
		ClassReferenceListFunction func = new();
		ClassReferenceListReadOnlyAction readOnlyAction = new();
		ClassReferenceListReadOnlyFunction readOnlyFunc = new();

		action.WithSafeFixed(ref v0, ref v1);
		func.WithSafeFixed(ref v0, ref v1, out Int32[] result);
		FixedReferenceListTest.AssertValues([1, 2,], result);
		PInvokeAssert.Equal(2, action.Count);
		readOnlyAction.WithSafeReadOnlyFixed(in v0, in v1);
		readOnlyFunc.WithSafeReadOnlyFixed(in v0, in v1, out result);
		FixedReferenceListTest.AssertValues([1, 2,], result);
		PInvokeAssert.Equal(2, readOnlyAction.Count);

		action.WithSafeFixed(ref v0, ref v1, ref v2);
		func.WithSafeFixed(ref v0, ref v1, ref v2, out result);
		FixedReferenceListTest.AssertValues([1, 2, 3,], result);
		PInvokeAssert.Equal(3, action.Count);
		readOnlyAction.WithSafeReadOnlyFixed(in v0, in v1, in v2);
		readOnlyFunc.WithSafeReadOnlyFixed(in v0, in v1, in v2, out result);
		FixedReferenceListTest.AssertValues([1, 2, 3,], result);
		PInvokeAssert.Equal(3, readOnlyAction.Count);

		action.WithSafeFixed(ref v0, ref v1, ref v2, ref v3);
		func.WithSafeFixed(ref v0, ref v1, ref v2, ref v3, out result);
		FixedReferenceListTest.AssertValues([1, 2, 3, 4,], result);
		PInvokeAssert.Equal(4, action.Count);
		readOnlyAction.WithSafeReadOnlyFixed(in v0, in v1, in v2, in v3);
		readOnlyFunc.WithSafeReadOnlyFixed(in v0, in v1, in v2, in v3, out result);
		FixedReferenceListTest.AssertValues([1, 2, 3, 4,], result);
		PInvokeAssert.Equal(4, readOnlyAction.Count);

		action.WithSafeFixed(ref v0, ref v1, ref v2, ref v3, ref v4);
		func.WithSafeFixed(ref v0, ref v1, ref v2, ref v3, ref v4, out result);
		FixedReferenceListTest.AssertValues([1, 2, 3, 4, 5,], result);
		PInvokeAssert.Equal(5, action.Count);
		readOnlyAction.WithSafeReadOnlyFixed(in v0, in v1, in v2, in v3, in v4);
		readOnlyFunc.WithSafeReadOnlyFixed(in v0, in v1, in v2, in v3, in v4, out result);
		FixedReferenceListTest.AssertValues([1, 2, 3, 4, 5,], result);
		PInvokeAssert.Equal(5, readOnlyAction.Count);

		action.WithSafeFixed(ref v0, ref v1, ref v2, ref v3, ref v4, ref v5);
		func.WithSafeFixed(ref v0, ref v1, ref v2, ref v3, ref v4, ref v5, out result);
		FixedReferenceListTest.AssertValues([1, 2, 3, 4, 5, 6,], result);
		PInvokeAssert.Equal(6, action.Count);
		readOnlyAction.WithSafeReadOnlyFixed(in v0, in v1, in v2, in v3, in v4, in v5);
		readOnlyFunc.WithSafeReadOnlyFixed(in v0, in v1, in v2, in v3, in v4, in v5, out result);
		FixedReferenceListTest.AssertValues([1, 2, 3, 4, 5, 6,], result);
		PInvokeAssert.Equal(6, readOnlyAction.Count);

		action.WithSafeFixed(ref v0, ref v1, ref v2, ref v3, ref v4, ref v5, ref v6);
		func.WithSafeFixed(ref v0, ref v1, ref v2, ref v3, ref v4, ref v5, ref v6, out result);
		FixedReferenceListTest.AssertValues([1, 2, 3, 4, 5, 6, 7,], result);
		PInvokeAssert.Equal(7, action.Count);
		readOnlyAction.WithSafeReadOnlyFixed(in v0, in v1, in v2, in v3, in v4, in v5, in v6);
		readOnlyFunc.WithSafeReadOnlyFixed(in v0, in v1, in v2, in v3, in v4, in v5, in v6, out result);
		FixedReferenceListTest.AssertValues([1, 2, 3, 4, 5, 6, 7,], result);
		PInvokeAssert.Equal(7, readOnlyAction.Count);

		action.WithSafeFixed(ref v0, ref v1, ref v2, ref v3, ref v4, ref v5, ref v6, ref v7);
		func.WithSafeFixed(ref v0, ref v1, ref v2, ref v3, ref v4, ref v5, ref v6, ref v7, out result);
		FixedReferenceListTest.AssertValues([1, 2, 3, 4, 5, 6, 7, 'A',], result);
		PInvokeAssert.Equal(8, action.Count);
		readOnlyAction.WithSafeReadOnlyFixed(in v0, in v1, in v2, in v3, in v4, in v5, in v6, in v7);
		readOnlyFunc.WithSafeReadOnlyFixed(in v0, in v1, in v2, in v3, in v4, in v5, in v6, in v7, out result);
		FixedReferenceListTest.AssertValues([1, 2, 3, 4, 5, 6, 7, 'A',], result);
		PInvokeAssert.Equal(8, readOnlyAction.Count);
	}
	[Fact]
	public void ReferenceReceiverTest()
	{
		Byte v0 = 1;
		Int16 v1 = 2;
		Int32 v2 = 3;
		Int64 v3 = 4;
		SByte v4 = 5;
		UInt16 v5 = 6;
		UInt32 v6 = 7;
		Char v7 = 'A';

		FixedReferenceListAction action = new();
		FixedReferenceListFunction func = new();
		FixedReferenceListReadOnlyAction readOnlyAction = new();
		FixedReferenceListReadOnlyFunction readOnlyFunc = new();

		action.WithReferenceSafeFixed(ref v0, ref v1);
		func.WithReferenceSafeFixed(ref v0, ref v1, out Int32[] result);
		FixedReferenceListTest.AssertValues([1, 2,], result);
		readOnlyAction.WithReferenceSafeReadOnlyFixed(in v0, in v1);
		readOnlyFunc.WithReferenceSafeReadOnlyFixed(in v0, in v1, out result);
		FixedReferenceListTest.AssertValues([1, 2,], result);

		action.WithReferenceSafeFixed(ref v0, ref v1, ref v2);
		func.WithReferenceSafeFixed(ref v0, ref v1, ref v2, out result);
		FixedReferenceListTest.AssertValues([1, 2, 3,], result);
		readOnlyAction.WithReferenceSafeReadOnlyFixed(in v0, in v1, in v2);
		readOnlyFunc.WithReferenceSafeReadOnlyFixed(in v0, in v1, in v2, out result);
		FixedReferenceListTest.AssertValues([1, 2, 3,], result);

		action.WithReferenceSafeFixed(ref v0, ref v1, ref v2, ref v3);
		func.WithReferenceSafeFixed(ref v0, ref v1, ref v2, ref v3, out result);
		FixedReferenceListTest.AssertValues([1, 2, 3, 4,], result);
		readOnlyAction.WithReferenceSafeReadOnlyFixed(in v0, in v1, in v2, in v3);
		readOnlyFunc.WithReferenceSafeReadOnlyFixed(in v0, in v1, in v2, in v3, out result);
		FixedReferenceListTest.AssertValues([1, 2, 3, 4,], result);

		action.WithReferenceSafeFixed(ref v0, ref v1, ref v2, ref v3, ref v4);
		func.WithReferenceSafeFixed(ref v0, ref v1, ref v2, ref v3, ref v4, out result);
		FixedReferenceListTest.AssertValues([1, 2, 3, 4, 5,], result);
		readOnlyAction.WithReferenceSafeReadOnlyFixed(in v0, in v1, in v2, in v3, in v4);
		readOnlyFunc.WithReferenceSafeReadOnlyFixed(in v0, in v1, in v2, in v3, in v4, out result);
		FixedReferenceListTest.AssertValues([1, 2, 3, 4, 5,], result);

		action.WithReferenceSafeFixed(ref v0, ref v1, ref v2, ref v3, ref v4, ref v5);
		func.WithReferenceSafeFixed(ref v0, ref v1, ref v2, ref v3, ref v4, ref v5, out result);
		FixedReferenceListTest.AssertValues([1, 2, 3, 4, 5, 6,], result);
		readOnlyAction.WithReferenceSafeReadOnlyFixed(in v0, in v1, in v2, in v3, in v4, in v5);
		readOnlyFunc.WithReferenceSafeReadOnlyFixed(in v0, in v1, in v2, in v3, in v4, in v5, out result);
		FixedReferenceListTest.AssertValues([1, 2, 3, 4, 5, 6,], result);

		action.WithReferenceSafeFixed(ref v0, ref v1, ref v2, ref v3, ref v4, ref v5, ref v6);
		func.WithReferenceSafeFixed(ref v0, ref v1, ref v2, ref v3, ref v4, ref v5, ref v6, out result);
		FixedReferenceListTest.AssertValues([1, 2, 3, 4, 5, 6, 7,], result);
		readOnlyAction.WithReferenceSafeReadOnlyFixed(in v0, in v1, in v2, in v3, in v4, in v5, in v6);
		readOnlyFunc.WithReferenceSafeReadOnlyFixed(in v0, in v1, in v2, in v3, in v4, in v5, in v6, out result);
		FixedReferenceListTest.AssertValues([1, 2, 3, 4, 5, 6, 7,], result);

		action.WithReferenceSafeFixed(ref v0, ref v1, ref v2, ref v3, ref v4, ref v5, ref v6, ref v7);
		func.WithReferenceSafeFixed(ref v0, ref v1, ref v2, ref v3, ref v4, ref v5, ref v6, ref v7, out result);
		FixedReferenceListTest.AssertValues([1, 2, 3, 4, 5, 6, 7, 'A',], result);
		readOnlyAction.WithReferenceSafeReadOnlyFixed(in v0, in v1, in v2, in v3, in v4, in v5, in v6, in v7);
		readOnlyFunc.WithReferenceSafeReadOnlyFixed(in v0, in v1, in v2, in v3, in v4, in v5, in v6, in v7, out result);
		FixedReferenceListTest.AssertValues([1, 2, 3, 4, 5, 6, 7, 'A',], result);
	}

	private static void AssertValues(ReadOnlySpan<Int32> expected, ReadOnlySpan<Int32> actual)
	{
		PInvokeAssert.Equal(expected.Length, actual.Length);
		for (Int32 i = 0; i < expected.Length; i++)
			PInvokeAssert.Equal(expected[i], actual[i]);
	}
	private static unsafe void ItemTest(FixedPointerValueList.ItemValue item)
	{
		FixedPointerValue fpv = item.Value;
		PInvokeAssert.Equal(1, item.Count);
		PInvokeAssert.Equal(fpv.Size, item.Count * item.SizeOf);
		PInvokeAssert.Equal(fpv.Pointer, item.Pointer);
		PInvokeAssert.Equal(fpv.Type ?? typeof(Byte), item.Type);
		PInvokeAssert.Equal(fpv.IsUnmanaged, item.IsUnmanaged);
		PInvokeAssert.Equal(fpv.IsReadOnly, item.IsReadOnly);
		PInvokeAssert.Equal(fpv.IsNullOrEmpty, item.Count == 0 || fpv.Pointer == default);

		Boolean isObject = !item.IsUnmanaged && item.Type is { IsValueType: false, };
		PInvokeAssert.Equal(isObject, fpv.TryGetReadOnlyObjectContext(out ReadOnlyFixedContextValue<Object> octx));
		PInvokeAssert.Equal(item.IsUnmanaged,
		                    fpv.TryGetReadOnlyBinaryContext(out ReadOnlyFixedContextValue<Byte> bctx));
		if (isObject)
		{
			PInvokeAssert.Equal(item.Count, octx.Values.Length);
			PInvokeAssert.True(Unsafe.AreSame(ref MemoryMarshal.GetReference(octx.Values),
			                                  ref Unsafe.AsRef<Object>(fpv.Pointer.ToPointer())));
			PInvokeAssert.True(bctx.IsNullOrEmpty);
			PInvokeAssert.Equal(item.IsReadOnly, !fpv.TryGetObjectContext(out FixedContextValue<Object> _));
		}
		else if (item.IsUnmanaged && item.SizeOf != 0)
		{
			PInvokeAssert.Equal(item.Count, bctx.Values.Length / item.SizeOf);
			PInvokeAssert.True(Unsafe.AreSame(ref MemoryMarshal.GetReference(bctx.Values),
			                                  ref Unsafe.AsRef<Byte>(fpv.Pointer.ToPointer())));
			PInvokeAssert.True(octx.IsNullOrEmpty);
			PInvokeAssert.Equal(item.IsReadOnly, !fpv.TryGetBinaryContext(out FixedContextValue<Byte> _));
		}
	}
	private static Int32 Read(FixedPointerValueList.ItemValue item)
	{
		FixedPointerValue fpv = item.Value;
		if (item.IsReadOnly)
		{
			if (item.Type == typeof(Byte))
				return ((ReadOnlyFixedReferenceValue<Byte>)fpv).Reference;
			if (item.Type == typeof(Int16))
				return ((ReadOnlyFixedReferenceValue<Int16>)fpv).Reference;
			if (item.Type == typeof(Int32))
				return ((ReadOnlyFixedReferenceValue<Int32>)fpv).Reference;
			if (item.Type == typeof(Int64))
				return (Int32)((ReadOnlyFixedReferenceValue<Int64>)fpv).Reference;
			if (item.Type == typeof(SByte))
				return ((ReadOnlyFixedReferenceValue<SByte>)fpv).Reference;
			if (item.Type == typeof(UInt16))
				return ((ReadOnlyFixedReferenceValue<UInt16>)fpv).Reference;
			if (item.Type == typeof(UInt32))
				return (Int32)((ReadOnlyFixedReferenceValue<UInt32>)fpv).Reference;
			if (item.Type == typeof(Char))
				return ((ReadOnlyFixedReferenceValue<Char>)fpv).Reference;
		}
		else
		{
			if (item.Type == typeof(Byte))
				return ((FixedReferenceValue<Byte>)fpv).Reference;
			if (item.Type == typeof(Int16))
				return ((FixedReferenceValue<Int16>)fpv).Reference;
			if (item.Type == typeof(Int32))
				return ((FixedReferenceValue<Int32>)fpv).Reference;
			if (item.Type == typeof(Int64))
				return (Int32)((FixedReferenceValue<Int64>)fpv).Reference;
			if (item.Type == typeof(SByte))
				return ((FixedReferenceValue<SByte>)fpv).Reference;
			if (item.Type == typeof(UInt16))
				return ((FixedReferenceValue<UInt16>)fpv).Reference;
			if (item.Type == typeof(UInt32))
				return (Int32)((FixedReferenceValue<UInt32>)fpv).Reference;
			if (item.Type == typeof(Char))
				return ((FixedReferenceValue<Char>)fpv).Reference;
		}
		PInvokeAssert.True(false);
		return default;
	}

	private readonly struct FixedReferenceListAction : IFixedPointerListAction
	{
		public void Accept(scoped FixedPointerValueList list)
		{
			for (Int32 index = 0; index < list.Count; index++)
			{
				PInvokeAssert.False(list[index].IsReadOnly);
				FixedReferenceListTest.ItemTest(list[index]);
			}
		}
	}

	private readonly struct FixedReferenceListFunction : IFixedPointerListFunction<Int32[]>
	{
		public Int32[] Apply(scoped FixedPointerValueList list)
		{
			new FixedReferenceListAction().Accept(list);
			Int32[] values = new Int32[list.Count];
			for (Int32 index = 0; index < list.Count; index++)
				values[index] = FixedReferenceListTest.Read(list[index]);
			return values;
		}
	}

	private readonly struct FixedReferenceListReadOnlyAction : IFixedPointerListAction
	{
		public void Accept(scoped FixedPointerValueList list)
		{
			for (Int32 index = 0; index < list.Count; index++)
			{
				PInvokeAssert.True(list[index].IsReadOnly);
				FixedReferenceListTest.ItemTest(list[index]);
			}
		}
	}

	private readonly struct FixedReferenceListReadOnlyFunction : IFixedPointerListFunction<Int32[]>
	{
		public Int32[] Apply(scoped FixedPointerValueList list)
		{
			new FixedReferenceListReadOnlyAction().Accept(list);
			Int32[] values = new Int32[list.Count];
			for (Int32 index = 0; index < list.Count; index++)
				values[index] = FixedReferenceListTest.Read(list[index]);
			return values;
		}
	}

	private sealed class ClassReferenceListAction : IFixedPointerListAction
	{
		public Int32 Count { get; private set; }
		public void Accept(scoped FixedPointerValueList list)
		{
			this.Count = list.Count;
			for (Int32 index = 0; index < list.Count; index++)
			{
				PInvokeAssert.False(list[index].IsReadOnly);
				FixedReferenceListTest.ItemTest(list[index]);
			}
		}
	}

	private sealed class ClassReferenceListFunction : IFixedPointerListFunction<Int32[]>
	{
		public Int32[] Apply(scoped FixedPointerValueList list)
		{
			new ClassReferenceListAction().Accept(list);
			Int32[] values = new Int32[list.Count];
			for (Int32 index = 0; index < list.Count; index++)
				values[index] = FixedReferenceListTest.Read(list[index]);
			return values;
		}
	}

	private sealed class ClassReferenceListReadOnlyAction : IFixedPointerListAction
	{
		public Int32 Count { get; private set; }
		public void Accept(scoped FixedPointerValueList list)
		{
			this.Count = list.Count;
			for (Int32 index = 0; index < list.Count; index++)
			{
				PInvokeAssert.True(list[index].IsReadOnly);
				FixedReferenceListTest.ItemTest(list[index]);
			}
		}
	}

	private sealed class ClassReferenceListReadOnlyFunction : IFixedPointerListFunction<Int32[]>
	{
		public Int32[] Apply(scoped FixedPointerValueList list)
		{
			new ClassReferenceListReadOnlyAction().Accept(list);
			Int32[] values = new Int32[list.Count];
			for (Int32 index = 0; index < list.Count; index++)
				values[index] = FixedReferenceListTest.Read(list[index]);
			return values;
		}
	}

	private sealed class MutatingPairAction : IFixedPointerListAction
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

#if NET9_0_OR_GREATER
	private ref struct SampleRefStruct
	{
		public Int32 Value;
		public Boolean Flag;
	}

	private sealed class SpanPairAction : IFixedPointerListAction
	{
		public Boolean BytesEmpty;
		public Byte LeftFirst;
		public Int32 LeftLength;
		public Boolean LeftManagedLike;
		public Boolean ObjectsEmpty;
		public Byte RightFirst;
		public Int32 RightLength;
		public Boolean RightManagedLike;
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
		public Int32 LeftFirst;
		public Int32 LeftLength;
		public Boolean LeftManagedLike;
		public Int32 RightFirst;
		public Int32 RightLength;
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
		public Boolean LeftFlag;
		public Boolean LeftManagedLike;
		public Int32 LeftSeen;
		public Boolean RightFlag;
		public Boolean RightManagedLike;
		public Int32 RightSeen;
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