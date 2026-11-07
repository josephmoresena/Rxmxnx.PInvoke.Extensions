namespace Rxmxnx.PInvoke.Tests;

[TestFixture]
[ExcludeFromCodeCoverage]
[SuppressMessage("csharpsquid", "S2699")]
public sealed unsafe class TypedPointerAbiTest
{
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	private delegate Int32 Compare(ReadOnlyValPtr<Int32> left, ReadOnlyValPtr<Int32> right);

	private static readonly Boolean CanMarshalTypedPointers =
#if NET5_0_OR_GREATER
		true;
#else
		SystemInfo.IsMonoRuntime;
#endif

	[Fact]
	public void MemcmpTest()
	{
		if (!TypedPointerAbiTest.CanMarshalTypedPointers) return;
		Byte[] left = [1, 2, 3, 4,];
		Byte[] same = [1, 2, 3, 4,];
		Byte[] greater = [1, 2, 3, 5,];
		fixed (Byte* leftPtr = left)
		fixed (Byte* samePtr = same)
		fixed (Byte* greaterPtr = greater)
		{
			UIntPtr length = (UIntPtr)left.Length;
			ReadOnlyValPtr<Byte> readOnly = leftPtr;
			ValPtr<Byte> mutable = leftPtr;
			Int32 readOnlyResult = TypedPointerAbiTest.Memcmp(readOnly, samePtr, length);
			Int32 mutableResult = TypedPointerAbiTest.Memcmp(mutable, samePtr, length);
			Int32 rawResult = TypedPointerAbiTest.Memcmp((IntPtr)leftPtr, (IntPtr)samePtr, length);
			PInvokeAssert.Equal(0, rawResult);
			PInvokeAssert.Equal(rawResult, readOnlyResult);
			PInvokeAssert.Equal(rawResult, mutableResult);

			Int32 typedDiff = TypedPointerAbiTest.Memcmp(readOnly, greaterPtr, length);
			Int32 rawDiff = TypedPointerAbiTest.Memcmp((IntPtr)leftPtr, (IntPtr)greaterPtr, length);
			PInvokeAssert.NotEqual(0, rawDiff);
			PInvokeAssert.Equal(rawDiff, typedDiff);
		}
	}
	[Fact]
	public void MemchrTest()
	{
		if (!TypedPointerAbiTest.CanMarshalTypedPointers) return;
		Byte[] bytes = [1, 2, 3, 4,];
		fixed (Byte* ptr = bytes)
		{
			ReadOnlyValPtr<Byte> start = ptr;
			UIntPtr length = (UIntPtr)bytes.Length;
			ReadOnlyValPtr<Byte> found = TypedPointerAbiTest.Memchr(start, 3, length);
			IntPtr foundRaw = TypedPointerAbiTest.Memchr((IntPtr)ptr, 3, length);
			PInvokeAssert.Equal(foundRaw, found);
			PInvokeAssert.Equal(((IntPtr)ptr).ToInt64() + 2, foundRaw.ToInt64());

			ReadOnlyValPtr<Byte> missing = TypedPointerAbiTest.Memchr(start, 9, length);
			IntPtr missingRaw = TypedPointerAbiTest.Memchr((IntPtr)ptr, 9, length);
			PInvokeAssert.Equal(IntPtr.Zero, missingRaw);
			PInvokeAssert.Equal(missingRaw, missing);
		}
	}
	[Fact]
	public void QsortTest()
	{
		if (!TypedPointerAbiTest.CanMarshalTypedPointers) return;
		Int32[] expected = [3, 1, 4, 1, 5,];
		Int32[] typed = (Int32[])expected.Clone();
		Int32[] raw = (Int32[])expected.Clone();
		Array.Sort(expected);
		Compare compare = TypedPointerAbiTest.CompareValues;
		IntPtr function = Marshal.GetFunctionPointerForDelegate(compare);
		UIntPtr count = (UIntPtr)typed.Length;
		UIntPtr size = (UIntPtr)Unsafe.SizeOf<Int32>();

		fixed (Int32* typedPtr = typed)
			TypedPointerAbiTest.Qsort(typedPtr, count, size, (FuncPtr<Compare>)function);
		fixed (Int32* rawPtr = raw)
			TypedPointerAbiTest.Qsort((IntPtr)rawPtr, count, size, function);

		PInvokeAssert.Equal(expected, typed);
		PInvokeAssert.Equal(expected, raw);
		GC.KeepAlive(compare);
	}

	private static Int32 CompareValues(ReadOnlyValPtr<Int32> left, ReadOnlyValPtr<Int32> right)
		=> left.Reference.CompareTo(right.Reference);
	private static Boolean IsWindows => RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

	private static Int32 Memcmp(ReadOnlyValPtr<Byte> left, ReadOnlyValPtr<Byte> right, UIntPtr length)
		=> TypedPointerAbiTest.IsWindows ?
			TypedPointerAbiTest.MemcmpWindows(left, right, length) :
			TypedPointerAbiTest.MemcmpUnix(left, right, length);
	private static Int32 Memcmp(ValPtr<Byte> left, ReadOnlyValPtr<Byte> right, UIntPtr length)
		=> TypedPointerAbiTest.IsWindows ?
			TypedPointerAbiTest.MemcmpWindows(left, right, length) :
			TypedPointerAbiTest.MemcmpUnix(left, right, length);
	private static Int32 Memcmp(IntPtr left, IntPtr right, UIntPtr length)
		=> TypedPointerAbiTest.IsWindows ?
			TypedPointerAbiTest.MemcmpWindows(left, right, length) :
			TypedPointerAbiTest.MemcmpUnix(left, right, length);
	private static ReadOnlyValPtr<Byte> Memchr(ReadOnlyValPtr<Byte> value, Int32 item, UIntPtr length)
		=> TypedPointerAbiTest.IsWindows ?
			TypedPointerAbiTest.MemchrWindows(value, item, length) :
			TypedPointerAbiTest.MemchrUnix(value, item, length);
	private static IntPtr Memchr(IntPtr value, Int32 item, UIntPtr length)
		=> TypedPointerAbiTest.IsWindows ?
			TypedPointerAbiTest.MemchrWindows(value, item, length) :
			TypedPointerAbiTest.MemchrUnix(value, item, length);
	private static void Qsort(ValPtr<Int32> values, UIntPtr count, UIntPtr size, FuncPtr<Compare> compare)
	{
		if (TypedPointerAbiTest.IsWindows)
			TypedPointerAbiTest.QsortWindows(values, count, size, compare);
		else
			TypedPointerAbiTest.QsortUnix(values, count, size, compare);
	}
	private static void Qsort(IntPtr values, UIntPtr count, UIntPtr size, IntPtr compare)
	{
		if (TypedPointerAbiTest.IsWindows)
			TypedPointerAbiTest.QsortWindows(values, count, size, compare);
		else
			TypedPointerAbiTest.QsortUnix(values, count, size, compare);
	}

#pragma warning disable SYSLIB1054
	[DllImport("msvcrt", EntryPoint = "memcmp", CallingConvention = CallingConvention.Cdecl)]
	private static extern Int32 MemcmpWindows(ReadOnlyValPtr<Byte> left, ReadOnlyValPtr<Byte> right, UIntPtr length);
	[DllImport("libc", EntryPoint = "memcmp", CallingConvention = CallingConvention.Cdecl)]
	private static extern Int32 MemcmpUnix(ReadOnlyValPtr<Byte> left, ReadOnlyValPtr<Byte> right, UIntPtr length);
	[DllImport("msvcrt", EntryPoint = "memcmp", CallingConvention = CallingConvention.Cdecl)]
	private static extern Int32 MemcmpWindows(ValPtr<Byte> left, ReadOnlyValPtr<Byte> right, UIntPtr length);
	[DllImport("libc", EntryPoint = "memcmp", CallingConvention = CallingConvention.Cdecl)]
	private static extern Int32 MemcmpUnix(ValPtr<Byte> left, ReadOnlyValPtr<Byte> right, UIntPtr length);
	[DllImport("msvcrt", EntryPoint = "memcmp", CallingConvention = CallingConvention.Cdecl)]
	private static extern Int32 MemcmpWindows(IntPtr left, IntPtr right, UIntPtr length);
	[DllImport("libc", EntryPoint = "memcmp", CallingConvention = CallingConvention.Cdecl)]
	private static extern Int32 MemcmpUnix(IntPtr left, IntPtr right, UIntPtr length);

	[DllImport("msvcrt", EntryPoint = "memchr", CallingConvention = CallingConvention.Cdecl)]
	private static extern ReadOnlyValPtr<Byte> MemchrWindows(ReadOnlyValPtr<Byte> value, Int32 item, UIntPtr length);
	[DllImport("libc", EntryPoint = "memchr", CallingConvention = CallingConvention.Cdecl)]
	private static extern ReadOnlyValPtr<Byte> MemchrUnix(ReadOnlyValPtr<Byte> value, Int32 item, UIntPtr length);
	[DllImport("msvcrt", EntryPoint = "memchr", CallingConvention = CallingConvention.Cdecl)]
	private static extern IntPtr MemchrWindows(IntPtr value, Int32 item, UIntPtr length);
	[DllImport("libc", EntryPoint = "memchr", CallingConvention = CallingConvention.Cdecl)]
	private static extern IntPtr MemchrUnix(IntPtr value, Int32 item, UIntPtr length);

	[DllImport("msvcrt", EntryPoint = "qsort", CallingConvention = CallingConvention.Cdecl)]
	private static extern void QsortWindows(ValPtr<Int32> values, UIntPtr count, UIntPtr size,
		FuncPtr<Compare> compare);
	[DllImport("libc", EntryPoint = "qsort", CallingConvention = CallingConvention.Cdecl)]
	private static extern void QsortUnix(ValPtr<Int32> values, UIntPtr count, UIntPtr size, FuncPtr<Compare> compare);
	[DllImport("msvcrt", EntryPoint = "qsort", CallingConvention = CallingConvention.Cdecl)]
	private static extern void QsortWindows(IntPtr values, UIntPtr count, UIntPtr size, IntPtr compare);
	[DllImport("libc", EntryPoint = "qsort", CallingConvention = CallingConvention.Cdecl)]
	private static extern void QsortUnix(IntPtr values, UIntPtr count, UIntPtr size, IntPtr compare);
#pragma warning restore SYSLIB1054
}