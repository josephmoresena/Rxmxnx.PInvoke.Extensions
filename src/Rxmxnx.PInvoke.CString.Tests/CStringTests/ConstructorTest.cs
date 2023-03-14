namespace Rxmxnx.PInvoke.Tests.CStringTests;

public sealed class ConstructorTest
{
    [Fact]
    internal async Task TestAsync()
    {
        Int32 lenght = TestSet.Utf16Text.Count;
        CString[,] cstr = new CString[4, lenght];
        Task task1 = Task.Run(() => CreateCStringFromString(cstr));
        Task task2 = Task.Run(() => CreateCStringFromFunction(cstr));
        Task task3 = Task.Run(() => CreateCStringFromBytes(cstr));
        Task task4 = Task.Run(() => CreateCStringFromNullTerminatedBytes(cstr));

        await Task.WhenAll(task1, task2, task3, task4);
        for (Int32 i = 0; i < lenght; i++)
            for (Int32 j = 1; i < 4; j++)
                Assert.Equal(cstr[0, i], cstr[j, i]);
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
}

