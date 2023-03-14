namespace Rxmxnx.PInvoke.Tests;

internal static partial class TestSet
{
    private static readonly String[] utf16Text = default!;
    private static readonly ReadOnlySpanFunc<Byte>[] utf8Text = default!;
    private static readonly Byte[][] utf8Bytes = default!;
    private static readonly Byte[][] utf8NullTerminatedBytes = default!;

    public static IReadOnlyList<String> Utf16Text => utf16Text;
    public static IReadOnlyList<ReadOnlySpanFunc<Byte>> Utf8Text => utf8Text;
    public static IReadOnlyList<Byte[]> Utf8Bytes => utf8Bytes;
    public static IReadOnlyList<Byte[]> Utf8NullTerminatedBytes => utf8NullTerminatedBytes;
}