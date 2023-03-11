using System.Reflection;
using System.Runtime.CompilerServices;
using Rxmxnx.PInvoke.Internal;

namespace Rxmxnx.PInvoke.Tests.Internal.FixedContextTests;

public class GetTransformationTest : FixedContextTestsBase
{
    [Fact]
    internal void BooleanTest() => Test<Boolean>();
    [Fact]
    internal void ByteTest() => Test<Byte>();
    [Fact]
    internal void Int16Test() => Test<Int16>();
    [Fact]
    internal void CharTest() => Test<Char>();
    [Fact]
    internal void Int32Test() => Test<Int32>();
    [Fact]
    internal void Int64Test() => Test<Int64>();
    [Fact]
    internal void Int128Test() => Test<Int128>();
    [Fact]
    internal void GuidTest() => Test<Guid>();
    [Fact]
    internal void SingleTest() => Test<Single>();
    [Fact]
    internal void HalfTest() => Test<Half>();
    [Fact]
    internal void DoubleTest() => Test<Double>();
    [Fact]
    internal void DecimalTest() => Test<Decimal>();
    [Fact]
    internal void DateTimeTest() => Test<DateTime>();
    [Fact]
    internal void TimeOnlyTest() => Test<TimeOnly>();
    [Fact]
    internal void TimeSpanTest() => Test<TimeSpan>();

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

