namespace Rxmxnx.PInvoke.Tests.PointerCCStringExtensions;

[TestFixture]
[ExcludeFromCodeCoverage]
public sealed class GetUnsafeCStringTest
{
	private static readonly IFixture fixture = new Fixture();

	[Fact]
	public unsafe void NullFixedTest()
	{
		CString? input = null;
		fixed (void* ptr = input)
			PInvokeAssert.Equal(IntPtr.Zero, (IntPtr)ptr);
	}

	[Fact]
	public unsafe void EmptyFixedTest()
	{
		CString input = CString.Empty;
		fixed (void* ptr = input)
			PInvokeAssert.NotEqual(IntPtr.Zero, (IntPtr)ptr);
	}

	[Fact]
	public unsafe void ZeroFixedTest()
	{
		CString input = CString.Zero;
		fixed (void* ptr = input)
			PInvokeAssert.Equal(IntPtr.Zero, (IntPtr)ptr);
	}

	[Theory]
	[InlineData(-1)]
	[InlineData(0)]
	[InlineData(1)]
	public void ZeroTest(Int32 length)
	{
		MemoryHandle handle = default;
		if (length >= 0)
		{
			PInvokeAssert.Equal(CString.Empty, IntPtr.Zero.GetUnsafeCString(length));
			PInvokeAssert.Equal(CString.Empty, UIntPtr.Zero.GetUnsafeCString(length));
			PInvokeAssert.Equal(CString.Empty, handle.GetUnsafeCString(length));
			PInvokeAssert.Same(CString.Empty, IntPtr.Zero.GetUnsafeCString(length));
			PInvokeAssert.Same(CString.Empty, UIntPtr.Zero.GetUnsafeCString(length));
			PInvokeAssert.Same(CString.Empty, handle.GetUnsafeCString(length));
		}
		else
		{
			PInvokeAssert.Throws<ArgumentException>(() => IntPtr.Zero.GetUnsafeCString(length));
			PInvokeAssert.Throws<ArgumentException>(() => UIntPtr.Zero.GetUnsafeCString(length));
			PInvokeAssert.Throws<ArgumentException>(() => handle.GetUnsafeCString(length));
		}
	}

	[Fact]
	public unsafe void Test()
	{
		String strInput = GetUnsafeCStringTest.fixture.Create<String>();
		CString input = (CString)((CString)strInput).Clone();
		using MemoryHandle handle = CString.GetBytes(input).AsMemory().Pin();
		fixed (void* p = input)
		{
			IntPtr intPtr = (IntPtr)p;
			UIntPtr uintPtr = (UIntPtr)p;

			CString cstr1 = intPtr.GetUnsafeCString(input.Length);
			CString cstr2 = uintPtr.GetUnsafeCString(input.Length);
			CString cstr3 = handle.GetUnsafeCString(input.Length);

			PInvokeAssert.Equal(input, cstr1);
			PInvokeAssert.Equal(input, cstr2);
			PInvokeAssert.Equal(input, cstr3);

			PInvokeAssert.False(cstr1.IsFunction);
			PInvokeAssert.False(cstr1.IsReference);
			PInvokeAssert.False(cstr1.IsSegmented);
			PInvokeAssert.True(cstr1.IsNullTerminated);

			PInvokeAssert.False(cstr2.IsFunction);
			PInvokeAssert.False(cstr2.IsReference);
			PInvokeAssert.False(cstr2.IsSegmented);
			PInvokeAssert.True(cstr2.IsNullTerminated);

			PInvokeAssert.False(cstr3.IsFunction);
			PInvokeAssert.False(cstr3.IsReference);
			PInvokeAssert.False(cstr3.IsSegmented);
			PInvokeAssert.True(cstr3.IsNullTerminated);
		}
	}
}