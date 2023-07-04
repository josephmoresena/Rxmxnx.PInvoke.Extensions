namespace Rxmxnx.PInvoke.Tests.DelegateExtensionsTests;

[ExcludeFromCodeCoverage]
public sealed class GetUnsafePointerTest
{
	private static readonly IFixture fixture = new Fixture();

	[Fact]
	internal void EmptyTest()
	{
		GetValue<Boolean>? getValueGeneric = default;
		GetByteValue? getByteValue = default;

		Assert.Equal(IntPtr.Zero, getValueGeneric.GetUnsafeIntPtr());
		Assert.Equal(UIntPtr.Zero, getValueGeneric.GetUnsafeUIntPtr());
		Assert.Equal(IntPtr.Zero, getByteValue.GetUnsafeIntPtr());
		Assert.Equal(UIntPtr.Zero, getByteValue.GetUnsafeUIntPtr());
	}

	[Fact]
	internal unsafe void NormalTest()
	{
		GetByteValue getByteValue = GetUnsafePointerTest.GetByte;
		GetValue<Byte> getValue = GetUnsafePointerTest.GetByte;
		IntPtr intPtr = Marshal.GetFunctionPointerForDelegate(getByteValue);
		UIntPtr uintPtr = (UIntPtr)intPtr.ToPointer();
		Byte input = GetUnsafePointerTest.fixture.Create<Byte>();

		IntPtr result = getByteValue.GetUnsafeIntPtr();
		UIntPtr result2 = getByteValue.GetUnsafeUIntPtr();

		Assert.Equal(intPtr, result);
		Assert.Equal(uintPtr, result2);
		Assert.Equal(getValue(input), Marshal.GetDelegateForFunctionPointer<GetByteValue>(result)(input));
		Assert.Equal(getValue(input), Marshal.GetDelegateForFunctionPointer<GetByteValue>(result)(input));

		Assert.Throws<ArgumentException>(() => getValue.GetUnsafeIntPtr());
		Assert.Throws<ArgumentException>(() => getValue.GetUnsafeUIntPtr());
	}

	private static Byte GetByte(Byte value) => value;
	private delegate T GetValue<T>(T value);
	private delegate Byte GetByteValue(Byte value);
}