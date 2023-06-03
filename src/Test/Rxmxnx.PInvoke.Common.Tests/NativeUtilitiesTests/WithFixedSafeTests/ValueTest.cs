namespace Rxmxnx.PInvoke.Tests.NativeUtilitiesTests.WithFixedSafeTests;

[ExcludeFromCodeCoverage]
public sealed class ValueTest
{
    private static readonly IFixture fixture = new Fixture();

    private IReferenceableWrapper? _wraper = default;

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

    private void Test<T>() where T : unmanaged
    {
        IReferenceableWrapper<T> value = IReferenceableWrapper.Create(fixture.Create<T>());
        Byte[] bytes = MemoryMarshal.AsBytes(MemoryMarshal.CreateSpan(ref Unsafe.AsRef(value.Reference), 1)).ToArray();

        this._wraper = value;
        NativeUtilities.WithSafeFixed(value.Reference, this.TestActionMethod);
        NativeUtilities.WithSafeFixed(value.Reference, this, TestActionMethod);
        Assert.Equal(bytes, NativeUtilities.WithSafeFixed(value.Reference, this.TestFuncMethod));
        Assert.Equal(bytes, NativeUtilities.WithSafeFixed(value.Reference, this, TestFuncMethod));
    }

    private unsafe void TestActionMethod<T>(in IReadOnlyFixedReference<T> fRef) where T : unmanaged
    {
        IReferenceableWrapper<T> wrapper = (IReferenceableWrapper<T>)this._wraper!;
        Byte[] bytes = new ReadOnlySpan<Byte>(fRef.Pointer.ToPointer(), sizeof(T)).ToArray();
        Assert.Equal(wrapper.Value, fRef.Reference);
        Assert.Equal(wrapper.Value, Unsafe.Read<T>(fRef.Pointer.ToPointer()));
        Assert.Equal(sizeof(T), fRef.Bytes.Length);
        Assert.Equal(bytes, fRef.Bytes.ToArray());

        //TODO: ContextTest
        Test<T, Boolean>(fRef, bytes);
        Test<T, Byte>(fRef, bytes);
        Test<T, Char>(fRef, bytes);
        Test<T, DateTime>(fRef, bytes);
        Test<T, Decimal>(fRef, bytes);
        Test<T, Double>(fRef, bytes);
        Test<T, Guid>(fRef, bytes);
        Test<T, Half>(fRef, bytes);
        Test<T, Int16>(fRef, bytes);
        Test<T, Int32>(fRef, bytes);
        Test<T, Int64>(fRef, bytes);
        Test<T, SByte>(fRef, bytes);
        Test<T, Single>(fRef, bytes);
        Test<T, UInt16>(fRef, bytes);
        Test<T, UInt32>(fRef, bytes);
        Test<T, UInt64>(fRef, bytes);
    }
    private unsafe Byte[] TestFuncMethod<T>(in IReadOnlyFixedReference<T> fRef) where T : unmanaged
    {
        this.TestActionMethod(fRef);
        return new ReadOnlySpan<Byte>(fRef.Pointer.ToPointer(), sizeof(T)).ToArray();
    }

    private static void TestActionMethod<T>(in IReadOnlyFixedReference<T> fRef, ValueTest test) where T : unmanaged
        => test.TestActionMethod(fRef);
    private static Byte[] TestFuncMethod<T>(in IReadOnlyFixedReference<T> fRef, ValueTest test) where T : unmanaged
        => test.TestFuncMethod(fRef);
    private static unsafe void Test<T, T2>(IReadOnlyFixedReference<T> fRef, Byte[] bytes)
        where T : unmanaged where T2 : unmanaged
    {
        if (sizeof(T) >= sizeof(T2))
        {
            IReadOnlyFixedReference<T2> fRef2 = fRef.Transformation<T2>(out IReadOnlyFixedMemory residual);
            if (typeof(T) == typeof(T2))
                Assert.Equal((Object)fRef.Reference, fRef2.Reference);
            else if (sizeof(T2) == sizeof(T2))
                Assert.Equal(bytes, fRef2.Bytes.ToArray());
            else
                Assert.Equal(bytes, fRef2.Bytes.ToArray().Concat(residual.Bytes.ToArray()));
            Assert.Equal(sizeof(T) == sizeof(T2), residual.Bytes.IsEmpty);
            Assert.Equal(fRef.Pointer + sizeof(T2), residual.Pointer);
            Assert.Equal(fRef.Pointer, fRef2.Pointer);
            //TODO: ContextTest
        }
        else
            Assert.Throws<InsufficientMemoryException>(() => fRef.Transformation<T2>(out _));
    }
}
