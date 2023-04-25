namespace Rxmxnx.PInvoke.Tests;

[ExcludeFromCodeCoverage]
public sealed class IReferenceableWrapperTests
{
    private static readonly IFixture fixture = new Fixture();

    [Fact]
    internal Task BooleanTestAsync() => TestAsync<Boolean>();
    [Fact]
    internal Task ByteTestAsync() => TestAsync<Byte>();
    [Fact]
    internal Task Int16TestAsync() => TestAsync<Int16>();
    [Fact]
    internal Task CharTestAsync() => TestAsync<Char>();
    [Fact]
    internal Task Int32TestAsync() => TestAsync<Int32>();
    [Fact]
    internal Task Int64TestAsync() => TestAsync<Int64>();
    [Fact]
    internal Task Int128TestAsync() => TestAsync<Int128>();
    [Fact]
    internal Task GuidTestAsync() => TestAsync<Guid>();
    [Fact]
    internal Task SingleTestAsync() => TestAsync<Single>();
    [Fact]
    internal Task HalfTestAsync() => TestAsync<Half>();
    [Fact]
    internal Task DoubleTestAsync() => TestAsync<Double>();
    [Fact]
    internal Task DecimalTestAsync() => TestAsync<Decimal>();
    [Fact]
    internal Task DateTimeTestAsync() => TestAsync<DateTime>();
    [Fact]
    internal Task TimeOnlyTestAsync() => TestAsync<TimeOnly>();
    [Fact]
    internal Task TimeSpanTestAsync() => TestAsync<TimeSpan>();

    private static async Task TestAsync<T>() where T : unmanaged
    {
        Task inputTest = Task.Run(Value<T>);
        Task nullTest1 = Task.Run(() => Nullable<T>(false));
        Task nullTest2 = Task.Run(() => Nullable<T>(true));

        await Task.WhenAll(inputTest, nullTest1, nullTest2);
    }
    private static void Value<T>() where T : unmanaged
    {
        T value = fixture.Create<T>();
        T value2 = fixture.Create<T>();
        var result = IReferenceableWrapper.Create(value);
        var result2 = IReferenceableWrapper.Create(value);
        var result3 = new ReferenceableWrapper<T>(result);
        ref readonly T refValue = ref result.Reference;
        ref T mutableValueRef = ref Unsafe.AsRef(result.Reference);
        Assert.NotNull(result);
        Assert.Equal(value, result.Value);
        Assert.Equal(value, refValue);
        Assert.True(result.Equals(value));
        Assert.Equal(Equals(value, value2), result.Equals(value2));
        Assert.True(Unsafe.AreSame(ref Unsafe.AsRef(result.Reference), ref mutableValueRef));
        Assert.False(Unsafe.AreSame(ref Unsafe.AsRef(result.Reference), ref value));
        Assert.False(result.Equals(result2));
        Assert.True(result.Equals(result3));
        Assert.False(result.Equals(default(IReferenceable<T>)));
    }
    private static void Nullable<T>(Boolean nullInput) where T : unmanaged
    {
        T? value = !nullInput ? fixture.Create<T>() : null;
        T? value2 = fixture.Create<Boolean>() ? fixture.Create<T>() : null;
        var result = IReferenceableWrapper.CreateNullable(value);
        var result2 = IReferenceableWrapper.CreateNullable(value);
        var result3 = new ReferenceableWrapper<T?>(result);
        ref readonly T? refValue = ref result.Reference;
        ref T? mutableValueRef = ref Unsafe.AsRef(result.Reference);
        Assert.NotNull(result);
        Assert.Equal(value, refValue);
        Assert.Equal(value, result.Value);
        Assert.Equal(Equals(value, value2), result.Equals(value2));
        Assert.True(Unsafe.AreSame(ref Unsafe.AsRef(result.Reference), ref mutableValueRef));
        Assert.False(Unsafe.AreSame(ref Unsafe.AsRef(result.Reference), ref value));
        Assert.False(result.Equals(result2));
        Assert.True(result.Equals(result3));
        Assert.False(result.Equals(default(IReferenceable<T?>)));
    }
}

