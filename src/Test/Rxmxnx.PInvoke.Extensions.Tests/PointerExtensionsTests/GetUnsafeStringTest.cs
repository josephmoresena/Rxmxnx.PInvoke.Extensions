#if !NETCOREAPP
using InlineData = NUnit.Framework.TestCaseAttribute;
#endif

namespace Rxmxnx.PInvoke.Tests.PointerExtensionsTests;

[TestFixture]
[ExcludeFromCodeCoverage]
public sealed class GetUnsafeStringTest
{
	private static readonly IFixture fixture = new Fixture();

	[Theory]
	[InlineData(-1)]
	[InlineData(0)]
	[InlineData(1)]
	public void ZeroTest(Int32 length)
	{
		MemoryHandle handle = default;
		if (length >= 0)
		{
			PInvokeAssert.Null(IntPtr.Zero.GetUnsafeString(length));
			PInvokeAssert.Null(UIntPtr.Zero.GetUnsafeString(length));
			PInvokeAssert.Null(handle.GetUnsafeString(length));
		}
		else
		{
			PInvokeAssert.Throws<ArgumentException>(() => IntPtr.Zero.GetUnsafeString(length));
			PInvokeAssert.Throws<ArgumentException>(() => UIntPtr.Zero.GetUnsafeString(length));
			PInvokeAssert.Throws<ArgumentException>(() => handle.GetUnsafeString(length));
		}
	}

	[Theory]
	[InlineData(false)]
	[InlineData(true)]
	public unsafe void Test(Boolean fixedLength)
	{
		String input = GetUnsafeStringTest.fixture.Create<String>();
		using MemoryHandle handle = input.AsMemory().Pin();
		fixed (void* p = input)
		{
			IntPtr intPtr = (IntPtr)p;
			UIntPtr uintPtr = (UIntPtr)p;

			if (fixedLength)
			{
				PInvokeAssert.Equal(input, intPtr.GetUnsafeString(input.Length));
				PInvokeAssert.Equal(input, uintPtr.GetUnsafeString(input.Length));
				PInvokeAssert.Equal(input, handle.GetUnsafeString(input.Length));
			}
			else
			{
				PInvokeAssert.Equal(input, intPtr.GetUnsafeString());
				PInvokeAssert.Equal(input, uintPtr.GetUnsafeString());
				PInvokeAssert.Equal(input, handle.GetUnsafeString());
			}
		}
	}
}