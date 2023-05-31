namespace Rxmxnx.PInvoke.Tests.CStringTests;

[ExcludeFromCodeCoverage]
public sealed class WithSafeFixedTest
{
    [Fact]
    internal void Test()
    {
        List<GCHandle> handles = new();
        IReadOnlyList<Int32> indices = TestSet.GetIndices();

        try
        {
            for (Int32 i = 0; i < indices.Count; i++)
                if (TestSet.GetCString(indices[i], handles) is CString value)
                {
                    value.WithSafeFixed(ActionMethod);
                    value.WithSafeFixed(value, ActionMethod);
                    Assert.Equal(value, value.WithSafeFixed(FunctionMethod));
                    Assert.Equal(value, value.WithSafeFixed(value, FunctionMethod));
                }
        }
        finally
        {
            foreach (GCHandle handle in handles)
                handle.Free();
        }
    }

    private static unsafe void ActionMethod(in IReadOnlyFixedMemory fmem)
    {
        ReadOnlySpan<Byte> span = fmem.Bytes;
        if (fmem.Pointer == IntPtr.Zero)
            Assert.True(span.IsEmpty);
        else
        {
            GCHandle handle = GCHandle.FromIntPtr(fmem.Pointer);
            fixed (void* ptr = span)
                if (span.Length != 0)
                    Assert.Equal(fmem.Pointer, new(ptr));
                else if (fmem.Pointer != IntPtr.Zero)
                    fixed (void* ptrEmpty = CString.GetBytes(CString.Empty))
                        Assert.Equal(fmem.Pointer, new(ptrEmpty));
            Assert.True(handle.IsAllocated);
        }

    }
    private static unsafe void ActionMethod(in IReadOnlyFixedMemory fmem, CString cstr)
    {
        IReadOnlyFixedMemory fmem2 = fmem;

        ActionMethod(fmem);
        Assert.Equal(cstr, new(() => fmem2.Bytes));
        BinaryPointerTest(fmem, cstr);
    }

    private static unsafe CString FunctionMethod(in IReadOnlyFixedMemory fmem)
    {
        ActionMethod(fmem);
        return CString.Create(fmem.Bytes);
    }
    private static unsafe CString FunctionMethod(in IReadOnlyFixedMemory fmem, CString cstr)
    {
        ActionMethod(fmem, cstr);
        return CString.Create(fmem.Bytes);
    }

    private static unsafe void BinaryPointerTest(IReadOnlyFixedMemory fmem, CString cstr)
    {
        IntPtr? ptr = GetPointerFromBytes(cstr);
        if (ptr.HasValue)
            Assert.Equal(fmem.Pointer, ptr);
    }
    private static unsafe IntPtr? GetPointerFromBytes(CString cstr)
    {
        try
        {
            fixed (void* ptr = CString.GetBytes(cstr))
                return new(ptr);
        }
        catch (Exception)
        {
            return default;
        }
    }
}
