#if !NETCOREAPP
using Fact = NUnit.Framework.TestAttribute;
#endif

namespace Rxmxnx.PInvoke.Tests.DelegateExtensionsTests;

[TestFixture]
[ExcludeFromCodeCoverage]
public sealed class GetUnsafePointerTest : DelegatesTests
{
	[Fact]
	public void EmptyTest()
	{
		GetValue<Boolean>? getValueGeneric = default;
		GetByteValue? getByteValue = default;

		PInvokeAssert.Equal(IntPtr.Zero, getValueGeneric.GetUnsafeIntPtr());
		PInvokeAssert.Equal(UIntPtr.Zero, getValueGeneric.GetUnsafeUIntPtr());
		PInvokeAssert.Equal(FuncPtr<GetValue<Boolean>>.Zero, getValueGeneric.GetUnsafeFuncPtr());
		PInvokeAssert.Equal(IntPtr.Zero, getByteValue.GetUnsafeIntPtr());
		PInvokeAssert.Equal(UIntPtr.Zero, getByteValue.GetUnsafeUIntPtr());
		PInvokeAssert.Equal(FuncPtr<GetByteValue>.Zero, getByteValue.GetUnsafeFuncPtr());
	}

	[Fact]
	public unsafe void NormalTest()
	{
		GetByteValue getByteValue = DelegatesTests.GetByte;
		GetValue<Byte> getValue = DelegatesTests.GetByte;
		IntPtr intPtr = Marshal.GetFunctionPointerForDelegate(getByteValue);
		UIntPtr uintPtr = (UIntPtr)intPtr.ToPointer();
		FuncPtr<GetByteValue> funcPtr = (FuncPtr<GetByteValue>)intPtr;
		Byte input = DelegatesTests.Fixture.Create<Byte>();

		IntPtr result = getByteValue.GetUnsafeIntPtr();
		UIntPtr result2 = getByteValue.GetUnsafeUIntPtr();
		FuncPtr<GetByteValue> result3 = getByteValue.GetUnsafeFuncPtr();

		PInvokeAssert.Equal(intPtr, result);
		PInvokeAssert.Equal(uintPtr, result2);
		PInvokeAssert.Equal(funcPtr, result3);
		PInvokeAssert.Equal(getValue(input), Marshal.GetDelegateForFunctionPointer<GetByteValue>(result)(input));
		PInvokeAssert.Equal(getValue(input), Marshal.GetDelegateForFunctionPointer<GetByteValue>(result)(input));
		PInvokeAssert.Equal(getValue(input), funcPtr.Invoke(input));

#if NETCOREAPP
		Assert.Throws<ArgumentException>(() => getValue.GetUnsafeIntPtr());
		Assert.Throws<ArgumentException>(() => getValue.GetUnsafeUIntPtr());
		Assert.Throws<ArgumentException>(() => getValue.GetUnsafeFuncPtr());
#endif
	}
}