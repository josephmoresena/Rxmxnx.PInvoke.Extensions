#if !NETCOREAPP
using Fact = NUnit.Framework.TestAttribute;
#endif

namespace Rxmxnx.PInvoke.Tests.PointerExtensionsTests;

[TestFixture]
[ExcludeFromCodeCoverage]
[SuppressMessage("csharpsquid", "S2699")]
public sealed class GetUnsafeReferenceTest
{
	private static readonly IFixture fixture = new Fixture();

	[Fact]
	public void ByteTest() => GetUnsafeReferenceTest.Test<Byte>();
	[Fact]
	public void CharTest() => GetUnsafeReferenceTest.Test<Char>();
	[Fact]
	public void DateTimeTest() => GetUnsafeReferenceTest.Test<DateTime>();
	[Fact]
	public void DecimalTest() => GetUnsafeReferenceTest.Test<Decimal>();
	[Fact]
	public void DoubleTest() => GetUnsafeReferenceTest.Test<Double>();
	[Fact]
	public void GuidTest() => GetUnsafeReferenceTest.Test<Guid>();
#if NET5_0_OR_GREATER
	[Fact]
	internal void HalfTest() => GetUnsafeReferenceTest.Test<Half>();
#endif
	[Fact]
	public void Int16Test() => GetUnsafeReferenceTest.Test<Int16>();
	[Fact]
	public void Int32Test() => GetUnsafeReferenceTest.Test<Int32>();
	[Fact]
	public void Int64Test() => GetUnsafeReferenceTest.Test<Int64>();
	[Fact]
	public void SByteTest() => GetUnsafeReferenceTest.Test<SByte>();
	[Fact]
	public void SingleTest() => GetUnsafeReferenceTest.Test<Single>();
	[Fact]
	public void UInt16Test() => GetUnsafeReferenceTest.Test<UInt16>();
	[Fact]
	public void UInt32Test() => GetUnsafeReferenceTest.Test<UInt32>();
	[Fact]
	public void UInt64Test() => GetUnsafeReferenceTest.Test<UInt64>();

	private static unsafe void Test<T>() where T : unmanaged
	{
		T value = GetUnsafeReferenceTest.fixture.Create<T>();
		ref T refValue = ref value;
		fixed (void* p = &refValue)
		{
			IntPtr intPtr = (IntPtr)p;
			UIntPtr uintPtr = (UIntPtr)p;

			ref T refValue1 = ref intPtr.GetUnsafeReference<T>();
			ref T refValue2 = ref uintPtr.GetUnsafeReference<T>();
			ref readonly T refReadOnlyValue1 = ref intPtr.GetUnsafeReadOnlyReference<T>();
			ref readonly T refReadOnlyValue2 = ref uintPtr.GetUnsafeReadOnlyReference<T>();

			PInvokeAssert.Equal(value, refValue1);
			PInvokeAssert.Equal(value, refValue2);
			PInvokeAssert.Equal(value, refReadOnlyValue1);
			PInvokeAssert.Equal(value, refReadOnlyValue2);
			PInvokeAssert.Equal(value, intPtr.GetUnsafeValue<T>());
			PInvokeAssert.Equal(value, uintPtr.GetUnsafeValue<T>());

			PInvokeAssert.True(Unsafe.AreSame(ref refValue, ref refValue1));
			PInvokeAssert.True(Unsafe.AreSame(ref refValue, ref refValue2));
#if NET8_0_OR_GREATER
			Assert.True(Unsafe.AreSame(ref refValue, in refReadOnlyValue1));
			Assert.True(Unsafe.AreSame(ref refValue, in refReadOnlyValue2));
#else
			PInvokeAssert.True(Unsafe.AreSame(ref refValue, ref Unsafe.AsRef(in refReadOnlyValue1)));
			PInvokeAssert.True(Unsafe.AreSame(ref refValue, ref Unsafe.AsRef(in refReadOnlyValue2)));
#endif

			PInvokeAssert.Null(IntPtr.Zero.GetUnsafeValue<T>());
			PInvokeAssert.Null(UIntPtr.Zero.GetUnsafeValue<T>());
		}
	}
}