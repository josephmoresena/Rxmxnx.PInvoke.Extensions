#if !NETCOREAPP
using InlineData = NUnit.Framework.TestCaseAttribute;
#endif

namespace Rxmxnx.PInvoke.Tests.PointerExtensionsTests;

[TestFixture]
[ExcludeFromCodeCoverage]
public sealed class GetUnsafeDelegateTest
{
	private static readonly IFixture fixture = new Fixture();

	[Theory]
	[InlineData(false)]
	[InlineData(true)]
	public void EmptyTest(Boolean useGeneric)
	{
		if (useGeneric)
		{
			PInvokeAssert.Null(IntPtr.Zero.GetUnsafeDelegate<GetValue<Byte>>());
			PInvokeAssert.Null(UIntPtr.Zero.GetUnsafeDelegate<GetValue<Byte>>());
		}
		else
		{
			PInvokeAssert.Null(IntPtr.Zero.GetUnsafeDelegate<GetByteValue>());
			PInvokeAssert.Null(UIntPtr.Zero.GetUnsafeDelegate<GetByteValue>());
		}
	}

	[Theory]
	[InlineData(false)]
	[InlineData(true)]
	public unsafe void NormalTest(Boolean useGeneric)
	{
		IntPtr intPtr = Marshal.GetFunctionPointerForDelegate<GetByteValue>(GetUnsafeDelegateTest.GetByte);
		UIntPtr uIntPtr = (UIntPtr)intPtr.ToPointer();
		Byte input = GetUnsafeDelegateTest.fixture.Create<Byte>();
		if (!useGeneric)
		{
			PInvokeAssert.Equal(GetUnsafeDelegateTest.GetByte(input), intPtr.GetUnsafeDelegate<GetByteValue>()!(input));
			PInvokeAssert.Equal(GetUnsafeDelegateTest.GetByte(input),
			                    uIntPtr.GetUnsafeDelegate<GetByteValue>()!(input));
		}
		else
		{
			PInvokeAssert.Throws<ArgumentException>(() => intPtr.GetUnsafeDelegate<GetValue<Byte>>()!(input));
			PInvokeAssert.Throws<ArgumentException>(() => uIntPtr.GetUnsafeDelegate<GetValue<Byte>>()!(input));
		}
	}

	private static Byte GetByte(Byte value) => value;
	private delegate T GetValue<T>(T value);
	private delegate Byte GetByteValue(Byte value);
}