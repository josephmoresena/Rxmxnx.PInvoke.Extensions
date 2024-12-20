namespace Rxmxnx.PInvoke.Tests.PointerCCStringExtensions;

[ExcludeFromCodeCoverage]
public sealed class GetUnsafeCStringTest
{
	private static readonly IFixture fixture = new Fixture();

	[Fact]
	internal unsafe void NullFixedTest()
	{
		CString? input = null;
		fixed (void* ptr = input)
			Assert.Equal(IntPtr.Zero, (IntPtr)ptr);
	}

	[Fact]
	internal unsafe void EmptyFixedTest()
	{
		CString input = CString.Empty;
		fixed (void* ptr = input)
			Assert.NotEqual(IntPtr.Zero, (IntPtr)ptr);
	}

	[Fact]
	internal unsafe void ZeroFixedTest()
	{
		CString input = CString.Zero;
		fixed (void* ptr = input)
			Assert.Equal(IntPtr.Zero, (IntPtr)ptr);
	}

	[Theory]
	[InlineData(-1)]
	[InlineData(0)]
	[InlineData(1)]
	internal void ZeroTest(Int32 length)
	{
		MemoryHandle handle = default;
		if (length >= 0)
		{
			Assert.Equal(CString.Empty, IntPtr.Zero.GetUnsafeCString(length));
			Assert.Equal(CString.Empty, UIntPtr.Zero.GetUnsafeCString(length));
			Assert.Equal(CString.Empty, handle.GetUnsafeCString(length));
			Assert.Same(CString.Empty, IntPtr.Zero.GetUnsafeCString(length));
			Assert.Same(CString.Empty, UIntPtr.Zero.GetUnsafeCString(length));
			Assert.Same(CString.Empty, handle.GetUnsafeCString(length));
		}
		else
		{
			Assert.Throws<ArgumentException>(() => IntPtr.Zero.GetUnsafeCString(length));
			Assert.Throws<ArgumentException>(() => UIntPtr.Zero.GetUnsafeCString(length));
			Assert.Throws<ArgumentException>(() => handle.GetUnsafeCString(length));
		}
	}

	[Fact]
	internal unsafe void Test()
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

			Assert.Equal(input, cstr1);
			Assert.Equal(input, cstr2);
			Assert.Equal(input, cstr3);

			Assert.False(cstr1.IsFunction);
			Assert.False(cstr1.IsReference);
			Assert.False(cstr1.IsSegmented);
			Assert.True(cstr1.IsNullTerminated);

			Assert.False(cstr2.IsFunction);
			Assert.False(cstr2.IsReference);
			Assert.False(cstr2.IsSegmented);
			Assert.True(cstr2.IsNullTerminated);

			Assert.False(cstr3.IsFunction);
			Assert.False(cstr3.IsReference);
			Assert.False(cstr3.IsSegmented);
			Assert.True(cstr3.IsNullTerminated);
		}
	}
}