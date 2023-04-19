namespace Rxmxnx.PInvoke.Tests.Internal.Utf8ComparatorTests;

[ExcludeFromCodeCoverage]
public sealed class InvalidTest
{
    private static readonly CStringUtf8Comparator cstrComparator = CStringUtf8Comparator.Create(StringComparison.InvariantCulture);
    private static readonly StringUtf8Comparator strComparator = StringUtf8Comparator.Create(StringComparison.InvariantCulture);

    [Theory]
    [InlineData("Hello")]
    [InlineData("Привет")]
    [InlineData("こんにちは")]
    internal void Test(String str)
    {
        CString cstr = (CString)str;
        CString invalidCStr = GetInvalidCstr(cstr);
        String invalidStr = invalidCStr.ToString();

        Int32 result = String.Compare(str, invalidStr, StringComparison.InvariantCulture);

        Assert.Equal(result, cstrComparator.Compare(cstr, invalidCStr));
        Assert.Equal(result, strComparator.Compare(cstr, invalidStr));

        Assert.Equal(-result, cstrComparator.Compare(invalidCStr, cstr));
        Assert.Equal(-result, strComparator.Compare(invalidCStr, str));

        Assert.Equal(0, cstrComparator.Compare(invalidCStr, invalidCStr));
        Assert.Equal(0, strComparator.Compare(invalidCStr, invalidStr));
    }

    private static CString GetInvalidCstr(CString cstr)
    {
        Byte[] utf8Byte = cstr.ToArray();
        for (Int32 i = 0; i < utf8Byte.Length; i++)
            utf8Byte[i] = Random.Shared.Next(0, 2) == 0 ? utf8Byte[i] : GetInvalidByte();
        return utf8Byte;
    }
    private static Byte GetInvalidByte()
    {
        Int32 result = Random.Shared.Next(0x80, 0xC0);
        return (Byte)result;
    }
}
