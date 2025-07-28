#if !NETCOREAPP
using Fact = NUnit.Framework.TestAttribute;
#endif

namespace Rxmxnx.PInvoke.Tests.CStringTests;

[TestFixture]
[ExcludeFromCodeCoverage]
public sealed class WithSafeFixedTest
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

	private static unsafe void ActionMethod(in IReadOnlyFixedMemory fmem)
	{
		ReadOnlySpan<Byte> span = fmem.Bytes;
		if (fmem.Pointer == IntPtr.Zero)
		{
			PInvokeAssert.True(span.IsEmpty);
		}
		else
		{
			GCHandle handle = GCHandle.FromIntPtr(fmem.Pointer);
			fixed (void* ptr = span)
			{
				if (span.Length != 0)
					PInvokeAssert.Equal(fmem.Pointer, new(ptr));
				else if (fmem.Pointer != IntPtr.Zero)
					fixed (void* ptrEmpty = CString.Empty)
						PInvokeAssert.Equal(fmem.Pointer, new(ptrEmpty));
			}
			PInvokeAssert.True(handle.IsAllocated);
		}
	}
	private static void ActionMethod(in IReadOnlyFixedMemory fmem, CString cstr)
	{
		IReadOnlyFixedMemory fmem2 = fmem;

		WithSafeFixedTest.ActionMethod(fmem);
		PInvokeAssert.Equal(cstr, new(() => fmem2.Bytes));
		WithSafeFixedTest.BinaryPointerTest(fmem, cstr);
	}

	private static CString FunctionMethod(in IReadOnlyFixedMemory fmem)
	{
		WithSafeFixedTest.ActionMethod(fmem);
		return CString.Create(fmem.Bytes);
	}
	private static CString FunctionMethod(in IReadOnlyFixedMemory fmem, CString cstr)
	{
		WithSafeFixedTest.ActionMethod(fmem, cstr);
		return CString.Create(fmem.Bytes);
	}

	private static void BinaryPointerTest(IReadOnlyFixedMemory fmem, CString cstr)
	{
		IntPtr? ptr = WithSafeFixedTest.GetPointerFromBytes(cstr);
		if (ptr.HasValue)
			PInvokeAssert.Equal(fmem.Pointer, ptr);
	}
	private static unsafe IntPtr? GetPointerFromBytes(CString cstr)
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