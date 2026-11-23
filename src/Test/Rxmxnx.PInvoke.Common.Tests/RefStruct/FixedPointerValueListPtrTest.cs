using Rxmxnx.PInvoke.RefStruct;
using Rxmxnx.PInvoke.VisualBasic;

namespace Rxmxnx.PInvoke.Tests.RefStruct;

[TestFixture]
[ExcludeFromCodeCoverage]
public sealed class FixedPointerValueListPtrTest
{
#if !NET5_0_OR_GREATER
	private static Type int32Type = typeof(Int32);
#endif

	[Fact]
	public void MutableTest() => FixedPointerValueListPtrTest.Exercise(false);
	[Fact]
	public void ReadOnlyTest() => FixedPointerValueListPtrTest.Exercise(true);
	[Fact]
	public void EmptyTest()
	{
		FixedPointerValueListPtr pointer = default;
		PInvokeAssert.Equal(0, pointer.Count);
		PInvokeAssert.True(pointer.IsEmpty);
		PInvokeAssert.False(pointer.IsReadOnly);
		PInvokeAssert.Equal(0, pointer.GetReadOnlySpan<Int32>(0).Length);
		PInvokeAssert.Equal(0, pointer.GetSpan<Int32>(0).Length);
		PInvokeAssert.Equal(default, pointer.GetReadOnlyValuePointer<Int32>(0));
		PInvokeAssert.Equal(default, pointer.GetValuePointer<Int32>(0));
	}
	[Fact]
	public void InvalidTransformationTest() => FixedPointerValueListPtrTest.RejectManaged();

	private static unsafe void Exercise(Boolean readOnly)
	{
		Int32[] values = [10, 20, 30,];
		fixed (Int32* pinned = values)
		{
			FixedPointerInfo info = new()
			{
				Pointer = pinned,
				Count = values.Length,
				SizeOf = sizeof(Int32),
				IsUnmanaged = true,
				TypeOrFunctionPointer = FixedPointerValueListPtrTest.Int32TypePointer(),
			};
			FixedPointerInfo[] infos = [info,];
			FixedPointerValueList list = new() { Information = infos, IsReadOnly = readOnly, };
			FixedPointerValueListPtr pointer = FixedPointerValueListPtr.CreateUnsafePtr(ref list);

			PInvokeAssert.Equal(1, pointer.Count);
			PInvokeAssert.False(pointer.IsEmpty);
			PInvokeAssert.Equal(readOnly, pointer.IsReadOnly);
			FixedPointerValueList.ItemValue block = pointer[0];
			PInvokeAssert.Equal(values.Length, block.Count);
			PInvokeAssert.Equal(sizeof(Int32), block.SizeOf);
			PInvokeAssert.True(block.IsUnmanaged);
			PInvokeAssert.Equal(values, block.GetReadOnlySpan<Int32>().ToArray());
			PInvokeAssert.Equal(10, block.GetUnsafe<Int32>());
			PInvokeAssert.Equal(values, pointer.GetReadOnlySpan<Int32>(0).ToArray());
			PInvokeAssert.Equal(10, pointer.GetReadOnlyValuePointer<Int32>(0).Reference);
			if (readOnly)
			{
				PInvokeAssert.Equal(0, pointer.GetSpan<Int32>(0).Length);
				PInvokeAssert.Throws<NullReferenceException>(() => FixedPointerValueListPtrTest.RejectWrite(pointer));
				PInvokeAssert.Equal(default, pointer.GetValuePointer<Int32>(0));
				return;
			}

			pointer.GetSpan<Int32>(0)[1] = 21;
			PInvokeAssert.Equal(21, values[1]);
			ValPtr<Int32> valPtr = pointer.GetValuePointer<Int32>(0);
			PInvokeAssert.Equal(pointer[0].Pointer, valPtr.Pointer);
			PInvokeAssert.Equal(values[0], valPtr.Reference);
			valPtr.Reference = 123;
			PInvokeAssert.Equal(123, values[0]);
		}
	}
	private static void RejectWrite(FixedPointerValueListPtr pointer) => pointer[0].SetUnsafe(1);
	private static unsafe void RejectManaged()
	{
		Int32[] values = [1,];
		fixed (Int32* item = values)
		{
			FixedPointerInfo info = new()
			{
				Pointer = item,
				Count = values.Length,
				SizeOf = sizeof(Int32),
				IsUnmanaged = true,
				TypeOrFunctionPointer = FixedPointerValueListPtrTest.Int32TypePointer(),
			};
			FixedPointerInfo[] infos = [info,];
			FixedPointerValueList list = new() { Information = infos, };
			FixedPointerValueListPtr pointer = FixedPointerValueListPtr.CreateUnsafePtr(ref list);
			PInvokeAssert.Equal(0, pointer.GetReadOnlySpan<KeyValuePair<Int32, String>>(0).Length);
		}
	}
	private static unsafe void* Int32TypePointer()
	{
#if NET5_0_OR_GREATER
		return FixedPointerInfo.ToUnmanaged(&FixedPointerValueListPtrTest.Int32Type);
#else
		return Unsafe.AsPointer(ref FixedPointerValueListPtrTest.int32Type);
#endif
	}
#if NET5_0_OR_GREATER
	private static Type Int32Type() => typeof(Int32);
#endif
}