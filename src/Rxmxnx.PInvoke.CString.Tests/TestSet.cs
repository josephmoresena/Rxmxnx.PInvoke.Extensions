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
}