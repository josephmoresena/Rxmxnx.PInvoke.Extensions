#if !NETCOREAPP
using Fact = NUnit.Framework.TestAttribute;
#endif

namespace Rxmxnx.PInvoke.Tests.CStringTests;

[TestFixture]
[ExcludeFromCodeCoverage]
public sealed unsafe class WithSafeFixedTest
{
	[Fact]
	public void Test()
	{
		using TestMemoryHandle handle = new();
		List<Int32> indices = TestSet.GetIndices();
		indices.ForEach(i => WithSafeFixedTest.ExecuteTest(TestSet.GetCString(i, handle)));
	}

	private static void ExecuteTest(CString? value)
	{
		if (value is null) return;
		value.WithSafeFixed(WithSafeFixedTest.ActionMethod);
		value.WithSafeFixed(value, WithSafeFixedTest.ActionMethod);
		PInvokeAssert.Equal(value, value.WithSafeFixed(WithSafeFixedTest.FunctionMethod));
		PInvokeAssert.Equal(value, value.WithSafeFixed(value, WithSafeFixedTest.FunctionMethod));
	}

	private static void ActionMethod(in IReadOnlyFixedMemory fmem)
	{
		ReadOnlySpan<Byte> span = fmem.Bytes;
		if (fmem.Pointer == IntPtr.Zero)
		{
			PInvokeAssert.True(span.IsEmpty);
			return;
		}
		fixed (void* ptr = span)
		{
			if (span.Length != 0)
				PInvokeAssert.Equal(fmem.Pointer, new(ptr));
			else if (fmem.Pointer != IntPtr.Zero)
				fixed (void* ptrEmpty = CString.Empty)
					PInvokeAssert.Equal(fmem.Pointer, new(ptrEmpty));
		}
#if NETCOREAPP
		GCHandle handle = GCHandle.FromIntPtr(fmem.Pointer);
		Assert.True(handle.IsAllocated);
#endif
	}
	private static void ActionMethod(in IReadOnlyFixedMemory fmem, CString cstr)
	{
		IReadOnlyFixedMemory fmem2 = fmem;

		WithSafeFixedTest.ActionMethod(fmem);
		if (fmem2.Bytes.Length > 0)
			PInvokeAssert.Equal(cstr, new(() => fmem2.Bytes));
		WithSafeFixedTest.BinaryPointerTest(fmem, cstr);
	}

	private static CString FunctionMethod(in IReadOnlyFixedMemory fmem)
	{
		WithSafeFixedTest.ActionMethod(fmem);
		if (fmem.Bytes.Length > 0)
			return CString.Create(fmem.Bytes);
		return fmem.Pointer != IntPtr.Zero ? CString.Empty : CString.Zero;
	}
	private static CString FunctionMethod(in IReadOnlyFixedMemory fmem, CString cstr)
	{
		WithSafeFixedTest.ActionMethod(fmem, cstr);
		if (fmem.Bytes.Length > 0)
			return CString.Create(fmem.Bytes);
		return fmem.Pointer != IntPtr.Zero ? CString.Empty : CString.Zero;
	}

	private static void BinaryPointerTest(IReadOnlyFixedMemory fmem, CString cstr)
	{
		IntPtr? ptr = WithSafeFixedTest.GetPointerFromBytes(cstr);
		if (ptr.HasValue)
			PInvokeAssert.Equal(fmem.Pointer, ptr);
	}
	private static IntPtr? GetPointerFromBytes(CString cstr)
	{
		if (Object.ReferenceEquals(CString.Empty, cstr)) return default;
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