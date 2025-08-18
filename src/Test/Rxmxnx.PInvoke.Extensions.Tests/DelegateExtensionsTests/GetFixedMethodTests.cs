#if !NETCOREAPP
using Fact = NUnit.Framework.TestAttribute;
#endif

namespace Rxmxnx.PInvoke.Tests.DelegateExtensionsTests;

[TestFixture]
[ExcludeFromCodeCoverage]
public sealed class GetFixedMethodTests : DelegatesTests
{
	[Fact]
	public void EmptyTest()
	{
		GetValue<Boolean>? getValueGeneric = default;
		GetByteValue? getByteValue = default;

		using IFixedMethod<GetValue<Boolean>>.IDisposable empty0 = getValueGeneric.GetFixedMethod();
		using IFixedMethod<GetByteValue>.IDisposable empty1 = getByteValue.GetFixedMethod();
		IFixedMethod<GetValue<Boolean>>.IDisposable empty2 = getValueGeneric.GetFixedMethod();
		IFixedMethod<GetByteValue>.IDisposable empty3 = getByteValue.GetFixedMethod();

		PInvokeAssert.Equal(IntPtr.Zero, empty0.Pointer);
		PInvokeAssert.Equal(IntPtr.Zero, empty1.Pointer);
		PInvokeAssert.Null(empty0.Method);
		PInvokeAssert.Null(empty1.Method);

		empty2.Dispose();
		empty3.Dispose();

		PInvokeAssert.Equal(IntPtr.Zero, empty2.Pointer);
		PInvokeAssert.Equal(IntPtr.Zero, empty3.Pointer);
		PInvokeAssert.Null(empty2.Method);
		PInvokeAssert.Null(empty3.Method);
	}

	[Fact]
	public void NormalTest()
	{
		GetByteValue getByteValue = DelegatesTests.GetByte;
		GetValue<Byte> getValue = DelegatesTests.GetByte;

		Byte input = DelegatesTests.Fixture.Create<Byte>();
		IntPtr intPtr = Marshal.GetFunctionPointerForDelegate(getByteValue);

		using IFixedMethod<GetByteValue>.IDisposable getByteValueDisposable = getByteValue.GetFixedMethod();

		PInvokeAssert.True(Object.ReferenceEquals(getByteValue, getByteValueDisposable.Method));
		PInvokeAssert.Equal(intPtr, getByteValueDisposable.Pointer);
		PInvokeAssert.Equal(getValue(input), getByteValueDisposable.Method(input));
		PInvokeAssert.Equal(getValue(input),
		                    Marshal.GetDelegateForFunctionPointer<GetByteValue>(getByteValueDisposable.Pointer)(input));

#if NETCOREAPP
		Assert.Throws<ArgumentException>(() => getValue.GetFixedMethod());
#endif
	}
}