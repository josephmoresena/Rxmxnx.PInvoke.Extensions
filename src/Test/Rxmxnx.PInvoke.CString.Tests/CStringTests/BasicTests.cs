namespace Rxmxnx.PInvoke.Tests.CStringTests;

[ExcludeFromCodeCoverage]
public sealed class BasicTests
{
    private static readonly IFixture fixture = new Fixture();

    [Fact]
    internal void EmptyTest()
    {
        ReadOnlySpan<Byte> emptySpan = default(CString?);
        Byte[] emptyBytes = CString.GetBytes(CString.Empty);
        CString? nullCStr = default(Byte[]);

        CString noEmpty1 = CString.Create(emptyBytes);
        CString noEmpty2 = CString.Create(new Byte[] { default, default });
        CString noEmpty3 = noEmpty2[..1];
        CString noEmpty4 = noEmpty2[1..];
        CString noEmpty5 = new(() => emptyBytes);

        CString empty = new(IntPtr.Zero, 0);
        CString empty2 = (CString)emptyBytes;
        CString empty3 = noEmpty1[..0];
        CString empty4 = CString.Create(() => emptyBytes);
        CString empty5 = new Byte[] { default, default };

        Assert.Equal(CString.Zero, empty);
        Assert.Equal(CString.Empty, empty);
        Assert.Equal(String.Empty, CString.Empty.ToString());
        Assert.Equal(CString.Empty, (CString)String.Empty);
        Assert.Equal(String.Empty, empty.ToString());
        Assert.Equal(empty, (CString)String.Empty);
        Assert.True(CString.IsNullOrEmpty(empty));
        Assert.True(CString.IsNullOrEmpty(CString.Empty));
        Assert.True(CString.IsNullOrEmpty(default));

        Assert.Equal(0, empty.CompareTo(CString.Empty));
        Assert.Equal(0, empty.CompareTo(String.Empty));
        Assert.Equal(0, empty.CompareTo((Object)CString.Empty));
        Assert.Equal(0, empty.CompareTo((Object)String.Empty));

        Assert.True(default(CString) == default(CString));
        Assert.True(default(String) == default(CString));
        Assert.True(default(CString) == default(String));
        Assert.True(String.Empty == CString.Empty);
        Assert.True(CString.Empty == String.Empty);

        Assert.False(default(CString) != default(CString));
        Assert.False(default(String) != default(CString));
        Assert.False(default(CString) != default(String));
        Assert.False(String.Empty != CString.Empty);
        Assert.False(CString.Empty != String.Empty);

        Assert.True(default(CString) != CString.Empty);
        Assert.True(default(String) != CString.Empty);
        Assert.True(default(CString) != String.Empty);
        Assert.True(CString.Empty != default(CString));
        Assert.True(String.Empty != default(CString));
        Assert.True(CString.Empty != default(String));

        Assert.Null(CString.Create(default(ReadOnlySpanFunc<Byte>)));
        Assert.Null(CString.Create(default(Byte[])));
        Assert.Null(nullCStr);

        Assert.Throws<ArgumentNullException>(() => CString.GetBytes(default!));
        Assert.Throws<ArgumentException>(() => empty.CompareTo(new Object()));

        Assert.True(emptySpan.IsEmpty);

        Assert.NotEqual(CString.Empty, noEmpty1);
        Assert.False(noEmpty1.IsReference);
        Assert.False(noEmpty1.IsNullTerminated);
        Assert.False(noEmpty1.IsFunction);
        Assert.False(noEmpty1.IsSegmented);
        Assert.Equal(1, noEmpty1.Length);
        Assert.NotEqual(CString.Empty, noEmpty2);
        Assert.False(noEmpty2.IsReference);
        Assert.False(noEmpty2.IsNullTerminated);
        Assert.False(noEmpty2.IsFunction);
        Assert.False(noEmpty2.IsSegmented);
        Assert.Equal(2, noEmpty2.Length);
        Assert.NotEqual(CString.Empty, noEmpty3);
        Assert.False(noEmpty3.IsReference);
        Assert.True(noEmpty3.IsNullTerminated);
        Assert.False(noEmpty3.IsFunction);
        Assert.False(noEmpty3.IsSegmented);
        Assert.Equal(1, noEmpty3.Length);
        Assert.NotEqual(CString.Empty, noEmpty4);
        Assert.False(noEmpty4.IsReference);
        Assert.True(noEmpty4.IsNullTerminated);
        Assert.False(noEmpty4.IsFunction);
        Assert.True(noEmpty4.IsSegmented);
        Assert.Equal(1, noEmpty4.Length);
        Assert.NotEqual(CString.Empty, noEmpty5);
        Assert.False(noEmpty5.IsReference);
        Assert.True(noEmpty5.IsNullTerminated);
        Assert.True(noEmpty5.IsFunction);
        Assert.False(noEmpty5.IsSegmented);
        Assert.Equal(1, noEmpty5.Length);

        Assert.Equal(CString.Empty, empty2);
        Assert.False(empty2.IsReference);
        Assert.True(empty2.IsNullTerminated);
        Assert.False(empty2.IsFunction);
        Assert.Equal(CString.Empty, empty3);
        Assert.False(empty3.IsReference);
        Assert.True(empty3.IsNullTerminated);
        Assert.False(empty3.IsFunction);
        Assert.Equal(CString.Empty, empty4);
        Assert.False(empty4.IsReference);
        Assert.True(empty4.IsNullTerminated);
        Assert.True(empty4.IsFunction);
        Assert.Equal(CString.Empty, empty5);
        Assert.False(empty4.IsReference);
        Assert.True(empty5.IsNullTerminated);
        Assert.False(empty5.IsFunction);
    }

    [Fact]
    internal void NormalTest()
    {
        Int32 lenght = TestSet.Utf16Text.Count;
        CString[,] cstr = new CString[5, lenght];
        CreateCStringFromString(cstr);
        CreateCStringFromFunction(cstr);
        CreateCStringFromBytes(cstr);
        CreateCStringFromNullTerminatedBytes(cstr);
        CreateCStringFromFunctionNonLiteral(cstr);

        for (Int32 i = 0; i < lenght; i++)
        {
            CString cstr1 = cstr[0, i];
            for (Int32 j = 1; j < 4; j++)
            {
                CString cstr2 = cstr[j, i];
                Assert.Equal(cstr1, cstr2);
                switch (j)
                {
                    case 1:
                        AssertFromFunction(cstr2);
                        break;
                    case 2:
                        AssertFromBytes(cstr2);
                        break;
                    case 3:
                        AssertFromNullTerminatedBytes(cstr2);
                        break;
                    case 4:
                        AssertFromFunctionNonLiteral(cstr2);
                        break;
                }
                RefEnumerationTest(cstr1, cstr2);
                EnumerationTest(cstr1, cstr2);
                Assert.True(cstr2.Equals(TestSet.Utf16Text[i]));
                Assert.True(cstr2.Equals((Object)TestSet.Utf16Text[i]));
                Assert.True(cstr2.Equals((Object)cstr1));
                Assert.Equal(cstr1.GetHashCode(), cstr2.GetHashCode());
                Assert.Equal(cstr1.ToHexString(), cstr2.ToHexString());
                Assert.Equal(cstr1.ToArray(), cstr2.ToArray());
                Assert.Equal(0, cstr1.CompareTo(cstr2));
            }
            AssertFromString(cstr1);
            Assert.True(cstr1.Equals(TestSet.Utf16Text[i]));
            Assert.Equal(0, cstr1.CompareTo(TestSet.Utf16Text[i]));
            Assert.Equal(TestSet.Utf16Text[i].GetHashCode(), cstr1.GetHashCode());
        }
    }

    [Fact]
    internal unsafe void PointerTest()
    {
        Int32 lenght = TestSet.Utf8Bytes.Count;
        for (Int32 i = 0; i < lenght; i++)
        {
            CString cstr1 = new(TestSet.Utf8Text[i]);
            TestBytesPointer(TestSet.Utf8Bytes[i], TestSet.Utf16Text[i], cstr1);
            TestNullTerminatedBytesPointer(TestSet.Utf8NullTerminatedBytes[i], TestSet.Utf16Text[i], cstr1);
        }
    }

    private static void CreateCStringFromString(CString[,] cstr)
    {
        for (Int32 i = 0; i < TestSet.Utf16Text.Count; i++)
            cstr[0, i] = (CString)TestSet.Utf16Text[i];
    }
    private static void CreateCStringFromFunction(CString[,] cstr)
    {
        for (Int32 i = 0; i < TestSet.Utf8Text.Count; i++)
            cstr[1, i] = new(TestSet.Utf8Text[i]);
    }
    private static void CreateCStringFromBytes(CString[,] cstr)
    {
        for (Int32 i = 0; i < TestSet.Utf8Bytes.Count; i++)
            cstr[2, i] = TestSet.Utf8Bytes[i];
    }
    private static void CreateCStringFromNullTerminatedBytes(CString[,] cstr)
    {
        for (Int32 i = 0; i < TestSet.Utf8NullTerminatedBytes.Count; i++)
            cstr[3, i] = TestSet.Utf8NullTerminatedBytes[i];
    }
    private static void CreateCStringFromFunctionNonLiteral(CString[,] cstr)
    {
        for (Int32 i = 0; i < TestSet.Utf8Text.Count; i++)
            cstr[4, i] = CString.Create(TestSet.Utf8Text[i]);
    }
    private static void AssertFromString(CString cstr)
    {
        Assert.True(cstr.IsFunction);
        Assert.True(cstr.IsNullTerminated);
        Assert.False(cstr.IsReference);
        Assert.False(cstr.IsSegmented);
        Assert.False(CString.IsNullOrEmpty(cstr));
        AssertFromNullTerminatedBytes((CString)cstr.Clone());

        Exception ex = AssertGetBytesException(cstr);
        Assert.Contains("does not contains the UTF-8 text.", ex.Message);
    }
    private static void AssertFromNullTerminatedBytes(CString cstr)
    {
        Assert.False(cstr.IsFunction);
        Assert.True(cstr.IsNullTerminated);
        Assert.False(cstr.IsReference);
        Assert.False(cstr.IsSegmented);
        Assert.False(CString.IsNullOrEmpty(cstr));

        Assert.Equal(cstr.Length + 1, CString.GetBytes(cstr).Length);

        CString rawClone = CString.Create(CString.GetBytes(cstr));
        Assert.False(rawClone.IsFunction);
        Assert.False(rawClone.IsNullTerminated);
        Assert.False(rawClone.IsReference);
        Assert.False(rawClone.IsSegmented);
        Assert.False(CString.IsNullOrEmpty(rawClone));
        Assert.NotEqual(cstr, rawClone);
        Assert.Equal(cstr.Length + 1, rawClone.Length);

        CString rawSpanClone = CString.Create(CString.GetBytes(cstr).AsSpan());
        Assert.False(rawSpanClone.IsFunction);
        Assert.False(rawSpanClone.IsNullTerminated);
        Assert.False(rawSpanClone.IsReference);
        Assert.False(rawSpanClone.IsSegmented);
        Assert.False(CString.IsNullOrEmpty(rawSpanClone));
        Assert.NotEqual(cstr, rawSpanClone);
        Assert.Equal(cstr.Length + 1, rawSpanClone.Length);

    }
    private static void AssertFromBytes(CString cstr)
    {
        Assert.False(cstr.IsFunction);
        Assert.False(cstr.IsNullTerminated);
        Assert.False(cstr.IsReference);
        Assert.False(cstr.IsSegmented);
        Assert.False(CString.IsNullOrEmpty(cstr));
        AssertFromNullTerminatedBytes((CString)cstr.Clone());

        Assert.Equal(cstr.Length, CString.GetBytes(cstr).Length);

        CString rawClone = CString.Create(CString.GetBytes(cstr));
        Assert.False(rawClone.IsFunction);
        Assert.False(rawClone.IsNullTerminated);
        Assert.False(rawClone.IsReference);
        Assert.False(rawClone.IsSegmented);
        Assert.False(CString.IsNullOrEmpty(rawClone));
        Assert.Equal(cstr, rawClone);
        Assert.Equal(cstr.Length, rawClone.Length);

        CString rawSpanClone = CString.Create(CString.GetBytes(cstr).AsSpan());
        Assert.False(rawSpanClone.IsFunction);
        Assert.False(rawSpanClone.IsNullTerminated);
        Assert.False(rawSpanClone.IsReference);
        Assert.False(rawSpanClone.IsSegmented);
        Assert.False(CString.IsNullOrEmpty(rawSpanClone));
        Assert.Equal(cstr, rawSpanClone);
        Assert.Equal(cstr.Length, rawSpanClone.Length);
    }
    private static void AssertFromFunction(CString cstr)
    {
        Assert.True(cstr.IsFunction);
        Assert.True(cstr.IsNullTerminated);
        Assert.False(cstr.IsReference);
        Assert.False(cstr.IsSegmented);
        Assert.False(CString.IsNullOrEmpty(cstr));
        AssertFromNullTerminatedBytes((CString)cstr.Clone());

        Exception ex = AssertGetBytesException(cstr);
        Assert.Contains("does not contains the UTF-8 text.", ex.Message);

        CString rawSpanClone = CString.Create(cstr);
        Assert.False(rawSpanClone.IsFunction);
        Assert.False(rawSpanClone.IsNullTerminated);
        Assert.False(rawSpanClone.IsReference);
        Assert.False(rawSpanClone.IsSegmented);
        Assert.False(CString.IsNullOrEmpty(rawSpanClone));
        Assert.Equal(cstr, rawSpanClone);
        Assert.Equal(cstr.Length, rawSpanClone.Length);
    }
    private static void AssertFromFunctionNonLiteral(CString cstr)
    {
        Assert.True(cstr.IsFunction);
        Assert.False(cstr.IsNullTerminated);
        Assert.False(cstr.IsReference);
        Assert.False(cstr.IsSegmented);
        Assert.False(CString.IsNullOrEmpty(cstr));
        AssertFromNullTerminatedBytes((CString)cstr.Clone());

        Exception ex = AssertGetBytesException(cstr);
        Assert.Contains("does not contains the UTF-8 text.", ex.Message);

        CString rawSpanClone = CString.Create(cstr);
        Assert.False(rawSpanClone.IsFunction);
        Assert.False(rawSpanClone.IsNullTerminated);
        Assert.False(rawSpanClone.IsReference);
        Assert.False(rawSpanClone.IsSegmented);
        Assert.False(CString.IsNullOrEmpty(rawSpanClone));
        Assert.Equal(cstr, rawSpanClone);
        Assert.Equal(cstr.Length, rawSpanClone.Length);

    }
    private static unsafe void AssertFromBytesPointer(CString cstr)
    {
        Assert.False(cstr.IsFunction);
        Assert.False(cstr.IsNullTerminated);
        Assert.True(cstr.IsReference);
        Assert.False(cstr.IsSegmented);
        Assert.False(CString.IsNullOrEmpty(cstr));
        AssertFromNullTerminatedBytes((CString)cstr.Clone());
        Exception ex = AssertGetBytesException(cstr);
        Assert.Contains("does not contains the UTF-8 text.", ex.Message);

        fixed (void* ptr = cstr.AsSpan())
        {
            CString rawPointerSpan = CString.Create(new IntPtr(ptr), cstr.Length);
            Assert.False(rawPointerSpan.IsFunction);
            Assert.False(rawPointerSpan.IsNullTerminated);
            Assert.True(rawPointerSpan.IsReference);
            Assert.False(rawPointerSpan.IsSegmented);
            Assert.False(CString.IsNullOrEmpty(rawPointerSpan));
            Assert.Equal(cstr, rawPointerSpan);
            Assert.Equal(cstr.Length, rawPointerSpan.Length);
        }
    }

    private static unsafe void AssertFromNullTerminatedBytesPointer(CString cstr)
    {
        Assert.False(cstr.IsFunction);
        Assert.True(cstr.IsNullTerminated);
        Assert.True(cstr.IsReference);
        Assert.False(cstr.IsSegmented);
        Assert.False(CString.IsNullOrEmpty(cstr));
        AssertFromNullTerminatedBytes((CString)cstr.Clone());

        fixed (void* ptr = cstr.AsSpan())
        {
            CString rawPointerSpan = CString.Create(new IntPtr(ptr), cstr.Length + 1);
            Assert.False(rawPointerSpan.IsFunction);
            Assert.False(rawPointerSpan.IsNullTerminated);
            Assert.True(rawPointerSpan.IsReference);
            Assert.False(rawPointerSpan.IsSegmented);
            Assert.False(CString.IsNullOrEmpty(rawPointerSpan));
            Assert.NotEqual(cstr, rawPointerSpan);
            Assert.Equal(cstr.Length + 1, rawPointerSpan.Length);
        }
    }
    private static void RefEnumerationTest(CString cstr1, CString cstr2)
    {
        Int32 i = 0;
        foreach (ref readonly Byte utf8Char in cstr2)
        {
            Assert.Equal(cstr1[i], utf8Char);
            Assert.True(Unsafe.AreSame(
                ref Unsafe.AsRef(cstr2.AsSpan()[i]), ref Unsafe.AsRef(utf8Char)));
            i++;
        }
    }
    private static void EnumerationTest(IEnumerable<Byte> cstr1, IEnumerable<Byte> cstr2)
    {
        Int32 i = 0;
        IEnumerator<Byte> enumerator1 = cstr1.GetEnumerator();
        foreach (Byte utf8Char in cstr2)
        {
            enumerator1.MoveNext();
            Assert.Equal(enumerator1.Current, utf8Char);
            i++;
        }
    }
    private static unsafe void TestBytesPointer(Byte[] bytes, String text, CString cstr1)
    {
        fixed (Byte* bptr2 = bytes)
        {
            CString cstr2 = new(new IntPtr(bptr2), bytes.Length);
            Assert.Equal(cstr1, cstr2);
            AssertFromBytesPointer(cstr2);
            Assert.True(cstr2.Equals(text));
            Assert.Equal(cstr1.GetHashCode(), cstr2.GetHashCode());
            Assert.Equal(cstr1.ToHexString(), cstr2.ToHexString());
            Assert.Equal(cstr1.ToArray(), cstr2.ToArray());
            RefEnumerationTest(cstr1, cstr2);
            EnumerationTest(cstr1, cstr2);
        }
    }
    private static unsafe void TestNullTerminatedBytesPointer(Byte[] bytes, String text, CString cstr1)
    {
        fixed (Byte* bptr2 = bytes)
        {
            CString cstr2 = new(new IntPtr(bptr2), bytes.Length);
            Assert.Equal(cstr1, cstr2);
            Assert.True(cstr2.Equals(text));
            AssertFromNullTerminatedBytesPointer(cstr2);
            Assert.Equal(cstr1.GetHashCode(), cstr2.GetHashCode());
            Assert.Equal(cstr1.ToHexString(), cstr2.ToHexString());
            Assert.Equal(cstr1.ToArray(), cstr2.ToArray());
            RefEnumerationTest(cstr1, cstr2);
            EnumerationTest(cstr1, cstr2);
        }
    }

    private static Exception AssertGetBytesException(CString cstr)
    {
        Exception ex;
        try
        {
            ex = Assert.Throws<InvalidOperationException>(() => CString.GetBytes(cstr));
        }
        catch (Exception)
        {
            // For some reason sometimes the test fails even though it shouldn't.
            // The test must be run again so that it does not fail.
            Assert.NotEmpty(cstr.ToArray());
            ex = Assert.Throws<InvalidOperationException>(() => CString.GetBytes(cstr));
        }
        return ex;
    }
}
