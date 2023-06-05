﻿namespace Rxmxnx.PInvoke.Tests.PointerExtensionsTests;

[ExcludeFromCodeCoverage]
[SuppressMessage("csharpsquid", "S2699")]
public sealed class GetUnsafeReferenceTest
{
    private static readonly IFixture fixture = new Fixture();

    [Fact]
    internal void ByteTest() => Test<Byte>();
    [Fact]
    internal void CharTest() => Test<Char>();
    [Fact]
    internal void DateTimeTest() => Test<DateTime>();
    [Fact]
    internal void DecimalTest() => Test<Decimal>();
    [Fact]
    internal void DoubleTest() => Test<Double>();
    [Fact]
    internal void GuidTest() => Test<Guid>();
    [Fact]
    internal void HalfTest() => Test<Half>();
    [Fact]
    internal void Int16Test() => Test<Int16>();
    [Fact]
    internal void Int32Test() => Test<Int32>();
    [Fact]
    internal void Int64Test() => Test<Int64>();
    [Fact]
    internal void SByteTest() => Test<SByte>();
    [Fact]
    internal void SingleTest() => Test<Single>();
    [Fact]
    internal void UInt16Test() => Test<UInt16>();
    [Fact]
    internal void UInt32Test() => Test<UInt32>();
    [Fact]
    internal void UInt64Test() => Test<UInt64>();

    private static unsafe void Test<T>() where T : unmanaged
    {
        T value = fixture.Create<T>();
        ref T refValue = ref value;
        fixed (void* p = &refValue)
        {
            IntPtr intPtr = (IntPtr)p;
            UIntPtr uintPtr = (UIntPtr)p;

            ref T refValue1 = ref intPtr.GetUnsafeReference<T>();
            ref T refValue2 = ref uintPtr.GetUnsafeReference<T>();
            ref readonly T refReadOnlyValue1 = ref intPtr.GetUnsafeReadOnlyReference<T>();
            ref readonly T refReadOnlyValue2 = ref uintPtr.GetUnsafeReadOnlyReference<T>();

            Assert.Equal(value, refValue1);
            Assert.Equal(value, refValue2);
            Assert.Equal(value, refReadOnlyValue1);
            Assert.Equal(value, refReadOnlyValue2);
            Assert.Equal(value, intPtr.GetUnsafeValue<T>());
            Assert.Equal(value, uintPtr.GetUnsafeValue<T>());

            Assert.True(Unsafe.AreSame(ref refValue, ref refValue1));
            Assert.True(Unsafe.AreSame(ref refValue, ref refValue2));
            Assert.True(Unsafe.AreSame(ref refValue, ref Unsafe.AsRef(refReadOnlyValue1)));
            Assert.True(Unsafe.AreSame(ref refValue, ref Unsafe.AsRef(refReadOnlyValue2)));

            Assert.Null(IntPtr.Zero.GetUnsafeValue<T>());
            Assert.Null(UIntPtr.Zero.GetUnsafeValue<T>());
        }
    }
}