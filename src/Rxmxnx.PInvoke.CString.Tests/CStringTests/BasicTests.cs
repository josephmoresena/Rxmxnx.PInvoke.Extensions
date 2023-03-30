namespace Rxmxnx.PInvoke.Tests.CStringTests;

public sealed class BasicTests
{
    private static readonly IFixture fixture = new Fixture();

    [Fact]
    internal void EmptyTest()
    {
        CString empty = new(IntPtr.Zero, 0);
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

        Assert.Throws<ArgumentNullException>(() => CString.GetBytes(default!));
        Assert.Throws<ArgumentException>(() => empty.CompareTo(new Object()));
    }

    [Fact]
    internal async Task TestAsync()
    {
        Int32 lenght = TestSet.Utf16Text.Count;
        CString[,] cstr = new CString[5, lenght];
        Task task1 = Task.Run(() => CreateCStringFromString(cstr));
        Task task2 = Task.Run(() => CreateCStringFromFunction(cstr));
        Task task3 = Task.Run(() => CreateCStringFromBytes(cstr));
        Task task4 = Task.Run(() => CreateCStringFromNullTerminatedBytes(cstr));
        Task task5 = Task.Run(() => CreateCStringFromFunctionNonLiteral(cstr));

        await Task.WhenAll(task1, task2, task3, task4);
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
    internal void Test()
    {
        Int32 lenght = TestSet.Utf8Bytes.Count;
        for (Int32 i = 0; i < lenght; i++)
        {
            CString cstr1 = new(TestSet.Utf8Text[i]);
            unsafe
            {
                TestBytesPointer(TestSet.Utf8Bytes[i], TestSet.Utf16Text[i], cstr1);
                TestNullTerminatedBytesPointer(TestSet.Utf8NullTerminatedBytes[i], TestSet.Utf16Text[i], cstr1);
            }
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
        Byte rChar = fixture.Create<Byte>();
        Assert.True(cstr.IsFunction);
        Assert.True(cstr.IsNullTerminated);
        Assert.False(cstr.IsReference);
        Assert.False(cstr.IsSegmented);
        Assert.False(CString.IsNullOrEmpty(cstr));
        Assert.NotEqual(cstr, new CString(rChar, cstr.Length));
        AssertFromNullTerminatedBytes((CString)cstr.Clone());

        Exception ex = Assert.Throws<InvalidOperationException>(() => CString.GetBytes(cstr));
        Assert.Contains("does not contains the UTF-8 text.", ex.Message);
    }
    private static void AssertFromNullTerminatedBytes(CString cstr)
    {
        Byte rChar = fixture.Create<Byte>();
        Assert.False(cstr.IsFunction);
        Assert.True(cstr.IsNullTerminated);
        Assert.False(cstr.IsReference);
        Assert.False(cstr.IsSegmented);
        Assert.False(CString.IsNullOrEmpty(cstr));
        Assert.NotEqual(cstr, new CString(rChar, cstr.Length));

        Assert.Equal(cstr.Length + 1, CString.GetBytes(cstr).Length);
    }
    private static void AssertFromBytes(CString cstr)
    {
        Byte rChar = fixture.Create<Byte>();
        Assert.False(cstr.IsFunction);
        Assert.False(cstr.IsNullTerminated);
        Assert.False(cstr.IsReference);
        Assert.False(cstr.IsSegmented);
        Assert.False(CString.IsNullOrEmpty(cstr));
        Assert.NotEqual(cstr, new CString(rChar, cstr.Length));
        AssertFromNullTerminatedBytes((CString)cstr.Clone());

        Assert.Equal(cstr.Length, CString.GetBytes(cstr).Length);
    }
    private static void AssertFromFunction(CString cstr)
    {
        Byte rChar = fixture.Create<Byte>();
        Assert.True(cstr.IsFunction);
        Assert.True(cstr.IsNullTerminated);
        Assert.False(cstr.IsReference);
        Assert.False(cstr.IsSegmented);
        Assert.False(CString.IsNullOrEmpty(cstr));
        Assert.NotEqual(cstr, new CString(rChar, cstr.Length));
        AssertFromNullTerminatedBytes((CString)cstr.Clone());

        Exception ex = Assert.Throws<InvalidOperationException>(() => CString.GetBytes(cstr));
        Assert.Contains("does not contains the UTF-8 text.", ex.Message);
    }
    private static void AssertFromFunctionNonLiteral(CString cstr)
    {
        Byte rChar = fixture.Create<Byte>();
        Assert.True(cstr.IsFunction);
        Assert.False(cstr.IsNullTerminated);
        Assert.False(cstr.IsReference);
        Assert.False(cstr.IsSegmented);
        Assert.False(CString.IsNullOrEmpty(cstr));
        Assert.NotEqual(cstr, new CString(rChar, cstr.Length));
        AssertFromNullTerminatedBytes((CString)cstr.Clone());

        Exception ex = Assert.Throws<InvalidOperationException>(() => CString.GetBytes(cstr));
        Assert.Contains("does not contains the UTF-8 text.", ex.Message);
    }
    private static void AssertFromBytesPointer(CString cstr)
    {
        Byte rChar = fixture.Create<Byte>();
        Assert.False(cstr.IsFunction);
        Assert.False(cstr.IsNullTerminated);
        Assert.True(cstr.IsReference);
        Assert.False(cstr.IsSegmented);
        Assert.False(CString.IsNullOrEmpty(cstr));
        Assert.NotEqual(cstr, new CString(rChar, cstr.Length));
        AssertFromNullTerminatedBytes((CString)cstr.Clone());

        Exception ex = Assert.Throws<InvalidOperationException>(() => CString.GetBytes(cstr));
        Assert.Contains("does not contains the UTF-8 text.", ex.Message);
    }
    private static void AssertFromNullTerminatedBytesPointer(CString cstr)
    {
        Byte rChar = fixture.Create<Byte>();
        Assert.False(cstr.IsFunction);
        Assert.True(cstr.IsNullTerminated);
        Assert.True(cstr.IsReference);
        Assert.False(cstr.IsSegmented);
        Assert.False(CString.IsNullOrEmpty(cstr));
        Assert.NotEqual(cstr, new CString(rChar, cstr.Length));
        AssertFromNullTerminatedBytes((CString)cstr.Clone());
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
}
