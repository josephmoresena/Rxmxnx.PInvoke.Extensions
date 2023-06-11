namespace Rxmxnx.PInvoke.Tests;

[ExcludeFromCodeCoverage]
[SuppressMessage("csharpsquid", "S2699")]
public sealed class IMutableWrapperTests
{
    private static readonly IFixture fixture = new Fixture();

    [Fact]
    internal void InterfaceTest()
    {
        IMutableWrapper<String> instance = new MutableInstance<String>();
        instance.Value = fixture.Create<String>();
        Assert.Equal(instance.Value, (instance as IWrapper<String>).Value);
    }

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
        var result = IMutableWrapper.Create(value);
        IWrapper<T> wrapper = result;

        Assert.NotNull(result);
        Assert.Equal(value, result.Value);
        Assert.True(result.Equals(value));
        Assert.Equal(Equals(value, value2), result.Equals(value2));
        Assert.NotNull(wrapper);
        Assert.Equal(value, wrapper.Value);
        Assert.True(wrapper.Equals(value));
        Assert.Equal(Equals(value, value2), wrapper.Equals(value2));

        result.Value = value2;
        Assert.Equal(value2, result.Value);
        Assert.True(result.Equals(value2));
        Assert.Equal(Equals(value2, value), result.Equals(value));
        Assert.Equal(value2, wrapper.Value);
        Assert.True(wrapper.Equals(value2));
        Assert.Equal(Equals(value2, value), wrapper.Equals(value));
    }

    private static void Nullable<T>(Boolean nullInput) where T : unmanaged
    {
        T? value = !nullInput ? fixture.Create<T>() : null;
        T? value2 = fixture.Create<Boolean>() ? fixture.Create<T>() : null;
        var result = IMutableWrapper<T>.CreateNullable(value);
        IWrapper<T?> wrapper = result;

        Assert.NotNull(result);
        Assert.Equal(value, result.Value);
        Assert.Equal(Equals(value, value2), result.Equals(value2));
        Assert.NotNull(wrapper);
        Assert.Equal(value, wrapper.Value);
        Assert.True(wrapper.Equals(value));
        Assert.Equal(Equals(value, value2), wrapper.Equals(value2));

        result.Value = value2;
        Assert.Equal(value2, result.Value);
        Assert.True(result.Equals(value2));
        Assert.Equal(Equals(value2, value), result.Equals(value));
        Assert.Equal(value2, wrapper.Value);
        Assert.True(wrapper.Equals(value2));
        Assert.Equal(Equals(value2, value), wrapper.Equals(value));
    }

    private static void Object<T>() where T : unmanaged
    {
        T[] array = fixture.CreateMany<T>().ToArray();
        T[] array2 = fixture.CreateMany<T>().ToArray();
        var result = IMutableWrapper.CreateObject(array);
        IWrapper<T[]> wrapper = result;

        Assert.NotNull(result);
        Assert.Equal(array, result.Value);
        Assert.True(result.Equals(array));
        Assert.Equal(Equals(array, array2), result.Equals(array2));
        Assert.NotNull(wrapper);
        Assert.Equal(array, wrapper.Value);
        Assert.True(wrapper.Equals(array));
        Assert.Equal(Equals(array, array2), wrapper.Equals(array2));

        result.Value = array2;
        Assert.Equal(array2, result.Value);
        Assert.True(result.Equals(array2));
        Assert.Equal(Equals(array2, array), result.Equals(array));
        Assert.Equal(array2, wrapper.Value);
        Assert.True(wrapper.Equals(array2));
        Assert.Equal(Equals(array2, array), wrapper.Equals(array));
    }
}

