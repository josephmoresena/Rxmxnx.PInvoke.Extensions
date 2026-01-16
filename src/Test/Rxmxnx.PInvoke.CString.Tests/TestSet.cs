#if NETCOREAPP && !NET8_0_OR_GREATER
[assembly: CollectionBehavior(DisableTestParallelization = true)]
#endif

namespace Rxmxnx.PInvoke.Tests;

[ExcludeFromCodeCoverage]
internal static partial class TestSet
{
	private static readonly String[] utf16Text = default!;
	private static readonly ReadOnlySpanFunc<Byte>[] utf8Text = default!;
	private static readonly Byte[][] utf8Bytes = default!;
	private static readonly Byte[][] utf8NullTerminatedBytes = default!;

	private static readonly String[] utf16TextLower = default!;
	private static readonly ReadOnlySpanFunc<Byte>[] utf8TextLower = default!;

	private static readonly String[] utf16TextUpper = default!;
	private static readonly ReadOnlySpanFunc<Byte>[] utf8TextUpper = default!;

	public static IReadOnlyList<String> Utf16Text => TestSet.utf16Text;
	public static IReadOnlyList<ReadOnlySpanFunc<Byte>> Utf8Text => TestSet.utf8Text;
	public static IReadOnlyList<Byte[]> Utf8Bytes => TestSet.utf8Bytes;
	public static IReadOnlyList<Byte[]> Utf8NullTerminatedBytes => TestSet.utf8NullTerminatedBytes;

	public static IReadOnlyList<String> Utf16TextLower => TestSet.utf16TextLower;
	public static IReadOnlyList<ReadOnlySpanFunc<Byte>> Utf8TextLower => TestSet.utf8TextLower;

	public static IReadOnlyList<String> Utf16TextUpper => TestSet.utf16TextUpper;
	public static IReadOnlyList<ReadOnlySpanFunc<Byte>> Utf8TextUpper => TestSet.utf8TextUpper;

	public static List<Int32> GetIndices(Int32? length = default)
	{
		length ??= 2 * TestSet.Utf16Text.Count + PInvokeRandom.Shared.Next(3, 10);
		List<Int32> result = new(length.Value);
		while (length > 0)
		{
			result.Add(PInvokeRandom.Shared.Next(-3, TestSet.Utf16Text.Count));
			length--;
		}
		return result;
	}
	public static unsafe String? GetString(Int32 index, Boolean nullWhenNullPointer = false)
		=> index switch
		{
			-3 => !nullWhenNullPointer ? new((Char*)IntPtr.Zero.ToPointer()) : default,
			-2 => default,
			-1 => String.Empty,
			_ => TestSet.Utf16Text[index],
		};
	public static unsafe ReadOnlySpan<Byte> GetSpan(Int32 index)
		=> index switch
		{
			-3 => new(IntPtr.Zero.ToPointer(), 0),
			-2 => default,
			-1 => CString.Empty,
			_ => TestSet.Utf8Text[index](),
		};
	public static CString?[] GetValues(IReadOnlyList<Int32> indices, TestMemoryHandle handle)
	{
		CString?[] values = new CString[indices.Count];
		for (Int32 i = 0; i < values.Length; i++)
			values[i] = TestSet.GetCString(indices[i], handle);
		return values;
	}
	public static CString? GetCString(Int32 index, TestMemoryHandle handle)
	{
		switch (index)
		{
			case -3:
				return new(IntPtr.Zero, default);
			case -2:
				return default;
			case -1:
				MemoryHandle utf8Handle = CString.Empty.TryPin(out Boolean pinned);
				if (pinned) handle.Add(utf8Handle);
				return CString.Empty;
			default:
				return TestSet.GetCStringWithHandle(index, handle);
		}
	}
	private static unsafe CString GetCStringWithHandle(Int32 index, TestMemoryHandle handle)
	{
		switch (PInvokeRandom.Shared.Next(default, 9))
		{
			case 0:
			case 1:
			case 2:
				return new(TestSet.Utf8Text[index]);
			case 4:
				return TestSet.Utf8Bytes[index];
			case 5:
				return TestSet.Utf8NullTerminatedBytes[index];
			case 6:
				MemoryHandle utf8Handle = TestSet.Utf8Bytes[index].AsMemory().Pin();
				handle.Add(utf8Handle);
				return new(new IntPtr(utf8Handle.Pointer), TestSet.Utf8Bytes[index].Length);
			case 7:
				MemoryHandle nullTerminatedUtf8Handle = TestSet.Utf8NullTerminatedBytes[index].AsMemory().Pin();
				handle.Add(nullTerminatedUtf8Handle);
				return new(new IntPtr(nullTerminatedUtf8Handle.Pointer), TestSet.Utf8NullTerminatedBytes[index].Length);
			default:
				return (CString)TestSet.Utf16Text[index];
		}
	}
}