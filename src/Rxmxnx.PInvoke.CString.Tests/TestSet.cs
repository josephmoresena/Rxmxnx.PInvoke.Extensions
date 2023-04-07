namespace Rxmxnx.PInvoke.Tests;

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

    public static IReadOnlyList<String> Utf16Text => utf16Text;
    public static IReadOnlyList<ReadOnlySpanFunc<Byte>> Utf8Text => utf8Text;
    public static IReadOnlyList<Byte[]> Utf8Bytes => utf8Bytes;
    public static IReadOnlyList<Byte[]> Utf8NullTerminatedBytes => utf8NullTerminatedBytes;

    public static IReadOnlyList<String> Utf16TextLower => utf16TextLower;
    public static IReadOnlyList<ReadOnlySpanFunc<Byte>> Utf8TextLower => utf8TextLower;

    public static IReadOnlyList<String> Utf16TextUpper => utf16TextUpper;
    public static IReadOnlyList<ReadOnlySpanFunc<Byte>> Utf8TextUpper => utf8TextUpper;

    public static IReadOnlyList<Int32> GetIndices(Int32? length = default)
    {
        length ??= 2 * TestSet.Utf16Text.Count + Random.Shared.Next(3, 10);
        Int32[] result = new Int32[length.Value];
        while (length > 0)
        {
            result[length.Value - 1] = Random.Shared.Next(-3, TestSet.Utf16Text.Count);
            length--;
        }
        return result;
    }
    public static unsafe String? GetString(Int32 index)
        => index switch
        {
            -3 => new((Char*)IntPtr.Zero.ToPointer()),
            -2 => default,
            -1 => String.Empty,
            _ => TestSet.Utf16Text[index],
        };
    public static unsafe ReadOnlySpan<Byte> GetSpan(Int32 index)
        => index switch
        {
            -3 => new ReadOnlySpan<Byte>(IntPtr.Zero.ToPointer(), 0),
            -2 => default,
            -1 => CString.Empty,
            _ => TestSet.Utf8Text[index](),
        };
    public static unsafe CString? GetCString(Int32 index, ICollection<GCHandle> handles)
    {
        switch (index)
        {
            case -3:
                return new(IntPtr.Zero, default);
            case -2:
                return default;
            case -1:
                return CString.Empty;
            default:
                switch (Random.Shared.Next(default, 9))
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
                        handles.Add(GCHandle.Alloc(TestSet.Utf8Bytes[index], GCHandleType.Pinned));
                        fixed (void* ptr = TestSet.Utf8Bytes[index])
                            return new(new IntPtr(ptr), TestSet.Utf8Bytes[index].Length);
                    case 7:
                        handles.Add(GCHandle.Alloc(TestSet.Utf8NullTerminatedBytes[index], GCHandleType.Pinned));
                        fixed (void* ptr = TestSet.Utf8NullTerminatedBytes[index])
                            return new(new IntPtr(ptr), TestSet.Utf8NullTerminatedBytes[index].Length);
                    default:
                        return (CString)TestSet.Utf16Text[index];
                };
        };
    }
}