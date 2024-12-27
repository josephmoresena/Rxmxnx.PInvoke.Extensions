namespace Rxmxnx.PInvoke.Tests.PointerExtensionsTests;

[ExcludeFromCodeCoverage]
public sealed class GetUnsafeStringTest
{
	private static readonly IFixture fixture = new Fixture();

	[Theory]
	[InlineData(-1)]
	[InlineData(0)]
	[InlineData(1)]
	internal void ZeroTest(Int32 length)
	{
		MemoryHandle handle = default;
		if (length >= 0)
		{
			Assert.Null(IntPtr.Zero.GetUnsafeString(length));
			Assert.Null(UIntPtr.Zero.GetUnsafeString(length));
			Assert.Null(handle.GetUnsafeString(length));
		}
		else
		{
			Assert.Throws<ArgumentException>(() => IntPtr.Zero.GetUnsafeString(length));
			Assert.Throws<ArgumentException>(() => UIntPtr.Zero.GetUnsafeString(length));
			Assert.Throws<ArgumentException>(() => handle.GetUnsafeString(length));
		}
	}

	[Theory]
	[InlineData(false)]
	[InlineData(true)]
	internal unsafe void Test(Boolean fixedLength)
	{
		String input = GetUnsafeStringTest.fixture.Create<String>();
		using MemoryHandle handle = input.AsMemory().Pin();
		fixed (void* p = input)
		{
			IntPtr intPtr = (IntPtr)p;
			UIntPtr uintPtr = (UIntPtr)p;

			if (fixedLength)
			{
				Assert.Equal(input, intPtr.GetUnsafeString(input.Length));
				Assert.Equal(input, uintPtr.GetUnsafeString(input.Length));
				Assert.Equal(input, handle.GetUnsafeString(input.Length));
			}
			else
			{
				Assert.Equal(input, intPtr.GetUnsafeString());
				Assert.Equal(input, uintPtr.GetUnsafeString());
				Assert.Equal(input, handle.GetUnsafeString());
			}
		}
	}
}