namespace Rxmxnx.PInvoke.Tests.CStringTests;

[ExcludeFromCodeCoverage]
public sealed class Utf8Constructor
{
    [Theory]
    [InlineData(0x41)]
    [InlineData(0x21)]
    [InlineData(0x35)]
    [InlineData(0x2B)]
    [InlineData(0x25)]
    [InlineData(0x24)]
    [InlineData(0x26)]
    [InlineData(0x5E)]
    [InlineData(0x5B)]
    [InlineData(0x7D)]
    public void SingleTest(Byte character)
    {
        Int32 count = Random.Shared.Next(0, 10);
        CString cstr = new(character, count);
        Test(stackalloc Byte[] { character }, count, cstr);
    }

    [Theory]
    [InlineData(0xC2, 0xA9)]
    [InlineData(0xC3, 0xB6)]
    [InlineData(0xC3, 0xB1)]
    [InlineData(0xC2, 0xAE)]
    [InlineData(0xC5, 0xBE)]
    [InlineData(0xC3, 0x9F)]
    [InlineData(0xC3, 0xAA)]
    [InlineData(0xC5, 0x82)]
    [InlineData(0xC3, 0xA7)]
    [InlineData(0xC4, 0xB1)]
    public void DoupleTest(Byte u0, Byte u1)
    {
        Int32 count = Random.Shared.Next(0, 10);
        CString cstr = new(u0, u1, count);
        Test(stackalloc Byte[] { u0, u1 }, count, cstr);
    }

    [Theory]
    [InlineData(0xE2, 0x84, 0xA2)]
    [InlineData(0xE2, 0x82, 0xAC)]
    [InlineData(0xE3, 0x81, 0x82)]
    [InlineData(0xE3, 0x81, 0x84)]
    [InlineData(0xE3, 0x81, 0x86)]
    [InlineData(0xE3, 0x81, 0x88)]
    [InlineData(0xE3, 0x81, 0x8A)]
    [InlineData(0xE3, 0x81, 0x8B)]
    [InlineData(0xE3, 0x81, 0x8D)]
    [InlineData(0xE3, 0x81, 0x8F)]
    public void TripleTest(Byte u0, Byte u1, Byte u2)
    {
        Int32 count = Random.Shared.Next(0, 10);
        CString cstr = new(u0, u1, u2, count);
        Test(stackalloc Byte[] { u0, u1, u2 }, count, cstr);
    }

    [Theory]
    [InlineData(0xF0, 0x9F, 0x8C, 0x88)]
    [InlineData(0xF0, 0x9F, 0x90, 0xBC)]
    [InlineData(0xF0, 0x9F, 0x8E, 0x81)]
    [InlineData(0xF0, 0x9F, 0x8D, 0x94)]
    [InlineData(0xF0, 0x9F, 0x8C, 0x9E)]
    [InlineData(0xF0, 0x9F, 0x8E, 0x89)]
    [InlineData(0xF0, 0x9F, 0x9A, 0x80)]
    [InlineData(0xF0, 0x9F, 0x92, 0xBB)]
    [InlineData(0xF0, 0x9F, 0x91, 0x8B)]
    [InlineData(0xF0, 0x9F, 0x8D, 0xBA)]
    public void QuadrupleTest(Byte u0, Byte u1, Byte u2, Byte u3)
    {
        Int32 count = Random.Shared.Next(0, 10);
        CString cstr = new(u0, u1, u2, u3, count);
        Test(stackalloc Byte[] { u0, u1, u2, u3 }, count, cstr);
    }

    private static void Test(ReadOnlySpan<Byte> seq, Int32 count, CString cstr)
    {
        String strSeq = Encoding.UTF8.GetString(seq);
        Assert.Equal(seq.Length * count, cstr.Length);
        Assert.Equal(seq.Length * count + 1, CString.GetBytes(cstr).Length);

        for (Int32 i = 0; i < count; i++)
            for (Int32 j = 0; j < seq.Length; j++)
                Assert.Equal(seq[j], cstr[(seq.Length * i) + j]);

        Assert.Equal(String.Concat(Enumerable.Repeat(strSeq, count)), cstr.ToString());
    }
}

