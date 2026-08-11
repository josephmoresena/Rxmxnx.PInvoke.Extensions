namespace Rxmxnx.PInvoke.Tests.FixedPointerListValueExtensionsTests.WithFixedSafeTests;

[TestFixture]
[ExcludeFromCodeCoverage]
public sealed class WithSafeFixedListTests
{
	private static readonly IFixture fixture = new Fixture();

	[Fact]
	public void EmptyTest()
	{
		FixedPointerValueList fpvl = default;

		PInvokeAssert.Equal(0, fpvl.Count);
		PInvokeAssert.True(fpvl.IsEmpty);
		PInvokeAssert.False(fpvl.IsReadOnly);
		PInvokeAssert.Null(fpvl.Handle);
	}

	[Fact]
	public void ReadOnlyTest()
	{
		Array[] array = WithSafeFixedListTests.GetArray(10);
		ReadOnlySpan<Byte> ros0 = (ReadOnlySpan<Byte>)array[0]!;
		ReadOnlySpan<Int16> ros1 = (ReadOnlySpan<Int16>)array[1]!;
		ReadOnlySpan<Int32> ros2 = (ReadOnlySpan<Int32>)array[2]!;
		ReadOnlySpan<Int64> ros3 = (ReadOnlySpan<Int64>)array[3]!;
		ReadOnlySpan<SByte> ros4 = (ReadOnlySpan<SByte>)array[4]!;
		ReadOnlySpan<UInt16> ros5 = (ReadOnlySpan<UInt16>)array[5]!;
		ReadOnlySpan<UInt32> ros6 = (ReadOnlySpan<UInt32>)array[6]!;
		ReadOnlySpan<String> ros7 = (ReadOnlySpan<String>)array[7]!;

		FixedListAction action = new();
		FixedListFunction func = new();
		action.WithSafeFixed(ros0, ros1);
		func.WithSafeFixed(ros0, ros1, out Array[] result);
		WithSafeFixedListTests.AssertArray(array, result);
		action.WithSafeFixed(ros0, ros1, ros2);
		func.WithSafeFixed(ros0, ros1, ros2, out result);
		WithSafeFixedListTests.AssertArray(array, result);
		action.WithSafeFixed(ros0, ros1, ros2, ros3);
		func.WithSafeFixed(ros0, ros1, ros2, ros3, out result);
		WithSafeFixedListTests.AssertArray(array, result);
		action.WithSafeFixed(ros0, ros1, ros2, ros3, ros4);
		func.WithSafeFixed(ros0, ros1, ros2, ros3, ros4, out result);
		WithSafeFixedListTests.AssertArray(array, result);
		action.WithSafeFixed(ros0, ros1, ros2, ros3, ros4, ros5);
		func.WithSafeFixed(ros0, ros1, ros2, ros3, ros4, ros5, out result);
		WithSafeFixedListTests.AssertArray(array, result);
		action.WithSafeFixed(ros0, ros1, ros2, ros3, ros4, ros5, ros6);
		func.WithSafeFixed(ros0, ros1, ros2, ros3, ros4, ros5, ros6, out result);
		WithSafeFixedListTests.AssertArray(array, result);
		action.WithSafeFixed(ros0, ros1, ros2, ros3, ros4, ros5, ros6, ros7);
		func.WithSafeFixed(ros0, ros1, ros2, ros3, ros4, ros5, ros6, ros7, out result);
		WithSafeFixedListTests.AssertArray(array, result);
	}
	[Fact]
	public void Test()
	{
		Array[] array = WithSafeFixedListTests.GetArray(10);
		Span<Byte> s0 = (Span<Byte>)array[0]!;
		Span<Int16> s1 = (Span<Int16>)array[1]!;
		Span<Int32> s2 = (Span<Int32>)array[2]!;
		Span<Int64> s3 = (Span<Int64>)array[3]!;
		Span<SByte> s4 = (Span<SByte>)array[4]!;
		Span<UInt16> s5 = (Span<UInt16>)array[5]!;
		Span<UInt32> s6 = (Span<UInt32>)array[6]!;
		Span<String> s7 = (Span<String>)array[7]!;

		FixedListAction action = new();
		FixedListFunction func = new();
		action.WithSafeFixed(s0, s1);
		func.WithSafeFixed(s0, s1, out Array[] result);
		WithSafeFixedListTests.AssertArray(array, result);
		action.WithSafeFixed(s0, s1, s2);
		func.WithSafeFixed(s0, s1, s2, out result);
		WithSafeFixedListTests.AssertArray(array, result);
		action.WithSafeFixed(s0, s1, s2, s3);
		func.WithSafeFixed(s0, s1, s2, s3, out result);
		WithSafeFixedListTests.AssertArray(array, result);
		action.WithSafeFixed(s0, s1, s2, s3, s4);
		func.WithSafeFixed(s0, s1, s2, s3, s4, out result);
		WithSafeFixedListTests.AssertArray(array, result);
		action.WithSafeFixed(s0, s1, s2, s3, s4, s5);
		func.WithSafeFixed(s0, s1, s2, s3, s4, s5, out result);
		WithSafeFixedListTests.AssertArray(array, result);
		action.WithSafeFixed(s0, s1, s2, s3, s4, s5, s6);
		func.WithSafeFixed(s0, s1, s2, s3, s4, s5, s6, out result);
		WithSafeFixedListTests.AssertArray(array, result);
		action.WithSafeFixed(s0, s1, s2, s3, s4, s5, s6, s7);
		func.WithSafeFixed(s0, s1, s2, s3, s4, s5, s6, s7, out result);
		WithSafeFixedListTests.AssertArray(array, result);
	}

	private static Array[] GetArray(Int32 length)
		=>
		[
			WithSafeFixedListTests.fixture.CreateMany<Byte>(length).ToArray(),
			WithSafeFixedListTests.fixture.CreateMany<Int16>(length).ToArray(),
			WithSafeFixedListTests.fixture.CreateMany<Int32>(length).ToArray(),
			WithSafeFixedListTests.fixture.CreateMany<Int64>(length).ToArray(),
			WithSafeFixedListTests.fixture.CreateMany<SByte>(length).ToArray(),
			WithSafeFixedListTests.fixture.CreateMany<UInt16>(length).ToArray(),
			WithSafeFixedListTests.fixture.CreateMany<UInt32>(length).ToArray(),
			WithSafeFixedListTests.fixture.CreateMany<String>(length).ToArray(),
		];
	private static void AssertArray(ReadOnlySpan<Array> arr0, ReadOnlySpan<Array> arr1)
	{
		for (Int32 i = 0; i < arr0.Length && i < arr1.Length; i++)
		{
			PInvokeAssert.Equal(arr0[i].Length, arr1[i].Length);
			for (Int32 j = 0; j < arr0[i].Length; j++)
				PInvokeAssert.Equal(arr0[i].GetValue(j), arr1[i].GetValue(j));
		}
	}

	private readonly struct FixedListAction : IFixedPointerListAction
	{
		public void Accept(scoped FixedPointerValueList fixedPointerValueList)
		{
			// ReSharper disable once ForCanBeConvertedToForeach
			for (Int32 index = 0; index < fixedPointerValueList.Count; index++)
			{
				FixedPointerValue fpv = fixedPointerValueList[index].Value;
				PInvokeAssert.Equal(fpv.Size, fixedPointerValueList[index].Count * fixedPointerValueList[index].SizeOf);
				PInvokeAssert.Equal(fpv.Pointer, fixedPointerValueList[index].Pointer);
				PInvokeAssert.Equal(fpv.Type ?? typeof(Byte), fixedPointerValueList[index].Type);
				PInvokeAssert.Equal(fpv.IsUnmanaged, fixedPointerValueList[index].IsUnmanaged);
				PInvokeAssert.Equal(fpv.IsReadOnly, fpv.IsReadOnly);
				PInvokeAssert.Equal(fpv.IsNullOrEmpty,
				                    fixedPointerValueList[index].Count == 0 || fpv.Pointer == default);
				FixedListAction.ItemTest(fixedPointerValueList[index]);
			}
		}
		private static unsafe void ItemTest(FixedPointerValueList.ItemValue item)
		{
			FixedPointerValue fpv = item.Value;
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
	}

	private readonly struct FixedListFunction : IFixedPointerListFunction<Array[]>
	{
		private delegate void CopyItemsDelegate(Array array, FixedPointerValue fpv);
		private static readonly MethodInfo? info =
			typeof(FixedListFunction).GetMethod(nameof(FixedListFunction.CopyItems),
			                                    BindingFlags.NonPublic | BindingFlags.Static);
		public Array[] Apply(scoped FixedPointerValueList fixedPointerValueList)
		{
			Array[] arr = new Array[fixedPointerValueList.Count];
			new FixedListAction().Accept(fixedPointerValueList);
			if (FixedListFunction.info is null) return [];
			Int32 index = 0;
			foreach (FixedPointerValueList.ItemValue item in fixedPointerValueList)
			{
				arr[index] = Array.CreateInstance(item.Type, item.Count);
				CopyItemsDelegate del =
#if NET5_0_OR_GREATER
					FixedListFunction.info.MakeGenericMethod(item.Type).CreateDelegate<CopyItemsDelegate>();
#else
					(CopyItemsDelegate)FixedListFunction.info.MakeGenericMethod(item.Type)
					                                    .CreateDelegate(typeof(CopyItemsDelegate));
#endif
				del(arr[index], item.Value);
				index++;
			}
			return arr;
		}

		private static void CopyItems<T>(Array arr, FixedPointerValue fpv)
		{
			if (arr is not T[] array) return;
			ReadOnlyFixedContextValue<T> ctx = (ReadOnlyFixedContextValue<T>)fpv;
			ctx.Values.CopyTo(array.AsSpan());
		}
	}
}