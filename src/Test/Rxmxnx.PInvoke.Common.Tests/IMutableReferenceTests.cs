namespace Rxmxnx.PInvoke.Tests;

[ExcludeFromCodeCoverage]
[SuppressMessage("csharpsquid", "S2699")]
public sealed class IMutableReferenceTests
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
        Task objectTest = Task.Run(Object<T>);

        await Task.WhenAll(inputTest, nullTest1, nullTest2, objectTest);
    }
    private static void Value<T>() where T : unmanaged
    {
        T value = fixture.Create<T>();
        T value2 = fixture.Create<T>();
        T value3 = fixture.Create<T>();
        var result = IMutableReference.Create(value);
        var result2 = IReferenceableWrapper.Create(value);
        var result3 = new ReferenceableWrapper<T>(result);
        ref readonly T refValue = ref result.Reference;
        ref T mutableValueRef = ref result.Reference;
        Assert.NotNull(result);
        Assert.Equal(value, result.Value);
        Assert.Equal(value, refValue);
        Assert.True(result.Equals(value));
        Assert.Equal(Equals(value, value2), result.Equals(value2));
        Assert.True(Unsafe.AreSame(ref Unsafe.AsRef(result.Reference), ref mutableValueRef));
        Assert.False(Unsafe.AreSame(ref Unsafe.AsRef(result.Reference), ref value));
        Assert.False(result.Equals(result2));
        Assert.True(result.Equals(result3));
        Assert.True((result as IReadOnlyReferenceable<T>).Equals(result3));

        result.Value = value2;
        Assert.Equal(value2, result.Value);
        Assert.Equal(value2, refValue);
        Assert.True(result.Equals(value2));
        Assert.Equal(Equals(value2, value), result.Equals(value));
        Assert.True(Unsafe.AreSame(ref Unsafe.AsRef(result.Reference), ref mutableValueRef));
        Assert.False(Unsafe.AreSame(ref Unsafe.AsRef(result.Reference), ref value2));
        Assert.False(result.Equals(result2));
        Assert.True(result.Equals(result3));
        Assert.True((result as IReadOnlyReferenceable<T>).Equals(result3));

        result.Reference = value3;
        Assert.Equal(value3, result.Value);
        Assert.Equal(value3, refValue);
    }
    private static void Nullable<T>(Boolean nullInput) where T : unmanaged
    {
        T? value = !nullInput ? fixture.Create<T>() : null;
        T? value2 = fixture.Create<Boolean>() ? fixture.Create<T>() : null;
        T? value3 = fixture.Create<Boolean>() ? fixture.Create<T>() : null;
        var result = IMutableReference.CreateNullable(value);
        var result2 = IReferenceableWrapper.CreateNullable(value);
        var result3 = new ReferenceableWrapper<T?>(result);
        ref readonly T? refValue = ref result.Reference;
        ref T? mutableValueRef = ref result.Reference;
        Assert.NotNull(result);
        Assert.Equal(value, refValue);
        Assert.Equal(value, result.Value);
        Assert.Equal(Equals(value, value2), result.Equals(value2));
        Assert.True(Unsafe.AreSame(ref Unsafe.AsRef(result.Reference), ref mutableValueRef));
        Assert.False(Unsafe.AreSame(ref Unsafe.AsRef(result.Reference), ref value));
        Assert.False(result.Equals(result2));
        Assert.True(result.Equals(result3));

        result.Value = value2;
        Assert.Equal(value2, result.Value);
        Assert.Equal(value2, refValue);
        Assert.True(result.Equals(value2));
        Assert.Equal(Equals(value2, value), result.Equals(value));
        Assert.True(Unsafe.AreSame(ref Unsafe.AsRef(result.Reference), ref mutableValueRef));
        Assert.False(Unsafe.AreSame(ref Unsafe.AsRef(result.Reference), ref value2));
        Assert.False(result.Equals(result2));
        Assert.True(result.Equals(result3));

        result.Reference = value3;
        Assert.Equal(value3, result.Value);
        Assert.Equal(value3, refValue);
    }
    private static void Object<T>() where T : unmanaged
    {
        T[] array = fixture.CreateMany<T>().ToArray();
        T[] array2 = fixture.CreateMany<T>().ToArray();
        T[] array3 = fixture.CreateMany<T>().ToArray();
        var result = IMutableReference.CreateObject(array);
        var result2 = IReferenceableWrapper.CreateObject(array);
        var result3 = new ReferenceableWrapper<T[]>(result);
        ref readonly T[] refValue = ref result.Reference;
        ref T[] mutableValueRef = ref result.Reference;
        Assert.NotNull(result);
        Assert.Equal(array, result.Value);
        Assert.Equal(array, refValue);
        Assert.True(result.Equals(array));
        Assert.Equal(Equals(array, array2), result.Equals(array2));
        Assert.True(Unsafe.AreSame(ref Unsafe.AsRef(result.Reference), ref mutableValueRef));
        Assert.False(Unsafe.AreSame(ref Unsafe.AsRef(result.Reference), ref array));
        Assert.False(result.Equals(result2));
        Assert.True(result.Equals(result3));
        Assert.True((result as IReadOnlyReferenceable<T[]>).Equals(result3));

        result.Value = array2;
        Assert.Equal(array2, result.Value);
        Assert.Equal(array2, refValue);
        Assert.True(result.Equals(array2));
        Assert.Equal(Equals(array2, array), result.Equals(array));
        Assert.True(Unsafe.AreSame(ref Unsafe.AsRef(result.Reference), ref mutableValueRef));
        Assert.False(Unsafe.AreSame(ref Unsafe.AsRef(result.Reference), ref array2));
        Assert.False(result.Equals(result2));
        Assert.True(result.Equals(result3));
        Assert.True((result as IReadOnlyReferenceable<T[]>).Equals(result3));

        result.Reference = array3;
        Assert.Equal(array3, result.Value);
        Assert.Equal(array3, refValue);
    }
}

