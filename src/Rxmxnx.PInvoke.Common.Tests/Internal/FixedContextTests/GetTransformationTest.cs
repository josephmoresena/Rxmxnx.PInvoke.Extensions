namespace Rxmxnx.PInvoke.Tests.Internal.FixedContextTests;

public sealed class GetTransformationTest : FixedContextTestsBase
{
    [Fact]
    internal void BooleanTest() => this.Test<Boolean>();
    [Fact]
    internal void ByteTest() => this.Test<Byte>();
    [Fact]
    internal void Int16Test() => this.Test<Int16>();
    [Fact]
    internal void CharTest() => this.Test<Char>();
    [Fact]
    internal void Int32Test() => this.Test<Int32>();
    [Fact]
    internal void Int64Test() => this.Test<Int64>();
    [Fact]
    internal void Int128Test() => this.Test<Int128>();
    [Fact]
    internal void GuidTest() => this.Test<Guid>();
    [Fact]
    internal void SingleTest() => this.Test<Single>();
    [Fact]
    internal void HalfTest() => this.Test<Half>();
    [Fact]
    internal void DoubleTest() => this.Test<Double>();
    [Fact]
    internal void DecimalTest() => this.Test<Decimal>();
    [Fact]
    internal void DateTimeTest() => this.Test<DateTime>();
    [Fact]
    internal void TimeOnlyTest() => this.Test<TimeOnly>();
    [Fact]
    internal void TimeSpanTest() => this.Test<TimeSpan>();

    private void Test<T>() where T : unmanaged
    {
        T[] values = fixture.CreateMany<T>().ToArray();
        base.WithFixed(values, false, Test);
        base.WithFixed(values, true, Test);
    }

    private static void Test<T>(FixedContext<T> ctx, T[] values) where T : unmanaged
    {
        MethodInfo transformMethod = ctx.GetType()
            .GetMethod("GetTransformation", BindingFlags.NonPublic | BindingFlags.Instance)!;
        Boolean isReadOnly = IsReadOnly(ctx);

        Test<T, Boolean>(ctx, isReadOnly, transformMethod);
        Test<T, Byte>(ctx, isReadOnly, transformMethod);
        Test<T, Int16>(ctx, isReadOnly, transformMethod);
        Test<T, Char>(ctx, isReadOnly, transformMethod);
        Test<T, Int32>(ctx, isReadOnly, transformMethod);
        Test<T, Int64>(ctx, isReadOnly, transformMethod);
        Test<T, Int128>(ctx, isReadOnly, transformMethod);
        Test<T, Single>(ctx, isReadOnly, transformMethod);
        Test<T, Half>(ctx, isReadOnly, transformMethod);
        Test<T, Double>(ctx, isReadOnly, transformMethod);
        Test<T, Decimal>(ctx, isReadOnly, transformMethod);
        Test<T, DateTime>(ctx, isReadOnly, transformMethod);
        Test<T, TimeOnly>(ctx, isReadOnly, transformMethod);
        Test<T, TimeSpan>(ctx, isReadOnly, transformMethod);

        ctx.Unload();
        Assert.Equal(InvalidError, Assert.Throws<InvalidOperationException>(() => Test<T, Boolean>(ctx, isReadOnly, transformMethod)).Message);
        Assert.Equal(InvalidError, Assert.Throws<InvalidOperationException>(() => Test<T, Byte>(ctx, isReadOnly, transformMethod)).Message);
        Assert.Equal(InvalidError, Assert.Throws<InvalidOperationException>(() => Test<T, Int16>(ctx, isReadOnly, transformMethod)).Message);
        Assert.Equal(InvalidError, Assert.Throws<InvalidOperationException>(() => Test<T, Char>(ctx, isReadOnly, transformMethod)).Message);
        Assert.Equal(InvalidError, Assert.Throws<InvalidOperationException>(() => Test<T, Int32>(ctx, isReadOnly, transformMethod)).Message);
        Assert.Equal(InvalidError, Assert.Throws<InvalidOperationException>(() => Test<T, Int64>(ctx, isReadOnly, transformMethod)).Message);
        Assert.Equal(InvalidError, Assert.Throws<InvalidOperationException>(() => Test<T, Int128>(ctx, isReadOnly, transformMethod)).Message);
        Assert.Equal(InvalidError, Assert.Throws<InvalidOperationException>(() => Test<T, Single>(ctx, isReadOnly, transformMethod)).Message);
        Assert.Equal(InvalidError, Assert.Throws<InvalidOperationException>(() => Test<T, Half>(ctx, isReadOnly, transformMethod)).Message);
        Assert.Equal(InvalidError, Assert.Throws<InvalidOperationException>(() => Test<T, Double>(ctx, isReadOnly, transformMethod)).Message);
        Assert.Equal(InvalidError, Assert.Throws<InvalidOperationException>(() => Test<T, Decimal>(ctx, isReadOnly, transformMethod)).Message);
        Assert.Equal(InvalidError, Assert.Throws<InvalidOperationException>(() => Test<T, DateTime>(ctx, isReadOnly, transformMethod)).Message);
        Assert.Equal(InvalidError, Assert.Throws<InvalidOperationException>(() => Test<T, TimeOnly>(ctx, isReadOnly, transformMethod)).Message);
        Assert.Equal(InvalidError, Assert.Throws<InvalidOperationException>(() => Test<T, TimeSpan>(ctx, isReadOnly, transformMethod)).Message);
    }
    private static void Test<T, T2>(FixedContext<T> ctx, Boolean isReadOnly, MethodInfo transformMethod)
        where T : unmanaged
        where T2 : unmanaged
    {
        MethodInfo invokableTransform = transformMethod.MakeGenericMethod(typeof(T2));
        Object? result = InvokeTransformationMethod(ctx, invokableTransform, true);

        Assert.IsType<TransformationContext<T, T2>>(result);
        if (!isReadOnly)
        {
            Object? result2 = invokableTransform.Invoke(ctx, new Object?[] { false });
            Assert.IsType<TransformationContext<T, T2>>(result2);
            Assert.Equal(result, result2);
        }
        else
        {
            Exception readOnly = Assert.Throws<InvalidOperationException>(() => InvokeTransformationMethod(ctx, invokableTransform, false));
            Assert.Equal(ReadOnlyError, readOnly.Message);
        }
    }
    private static Object? InvokeTransformationMethod<T>(FixedContext<T> ctx, MethodInfo method, Boolean readOnly)
        where T : unmanaged
    {
        try
        {
            return method.Invoke(ctx, new Object?[] { readOnly });
        }
        catch (TargetInvocationException ex)
        {
            throw ex.InnerException!;
        }
    }

}

