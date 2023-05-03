namespace Rxmxnx.PInvoke.Tests.Internal;

[ExcludeFromCodeCoverage]
public sealed class FixedDelegateTest : FixedMemoryTestsBase
{
    [Fact]
    internal void Test()
    {
        String str1 = fixture.Create<String>();
        String str2 = fixture.Create<String>();
        Byte[] bytes = fixture.Create<Byte[]>();
        Guid[] guids = fixture.CreateMany<Guid>(10).ToArray();
        Object obj = new();
        Boolean called = false;

#pragma warning disable IDE0039
        GetStringDelegate d1 = () => str1;
        GetStringDelegate d2 = () => str2 + str1;
        GetByteSpanDelegate d3 = (bArr) => bArr;
        VoidDelegate d4 = () => called = !called;
        VoidObjectDelegate d5 = (o) => Assert.Same(obj, o);
        GetGuidSpanDelegate d6 = () => guids;
#pragma warning restore IDE0039

        String f1() => str1;
        String f2() => str2 + str1;
        Span<Byte> f3(Byte[] bArr) => bArr;
        void f4() => called = !called;
        void f5(Object o) => Assert.Same(obj, o);
        Span<Guid> f6() => guids;

        FixedDelegateTestStatus delegateStatus = new()
        {
            Status1 = new(false, new FixedDelegate<GetStringDelegate>(d1)) { Delegate = d1 },
            Status2 = new(false, new FixedDelegate<GetStringDelegate>(d2)) { Delegate = d2 },
            Status3 = new(false, new FixedDelegate<GetByteSpanDelegate>(d3)) { Delegate = d3 },
            Status4 = new(false, new FixedDelegate<VoidDelegate>(d4)) { Delegate = d4 },
            Status5 = new(false, new FixedDelegate<VoidObjectDelegate>(d5)) { Delegate = d5 },
            Status6 = new(false, new FixedDelegate<GetGuidSpanDelegate>(d6)) { Delegate = d6 },
        };

        FixedDelegateTestStatus functionStatus = new()
        {
            Status1 = new(true, new FixedDelegate<GetStringDelegate>(f1)) { Delegate = f1 },
            Status2 = new(true, new FixedDelegate<GetStringDelegate>(f2)) { Delegate = f2 },
            Status3 = new(true, new FixedDelegate<GetByteSpanDelegate>(f3)) { Delegate = f3 },
            Status4 = new(true, new FixedDelegate<VoidDelegate>(f4)) { Delegate = f4 },
            Status5 = new(true, new FixedDelegate<VoidObjectDelegate>(f5)) { Delegate = f5 },
            Status6 = new(true, new FixedDelegate<GetGuidSpanDelegate>(f6)) { Delegate = f6 },
        };

        AssertTest(bytes, guids, ref obj, ref called, delegateStatus);
        AssertTest(bytes, guids, ref obj, ref called, functionStatus);
    }

    private static void AssertTest(Byte[] bytes, Guid[] guids, ref Object obj, ref Boolean togle, FixedDelegateTestStatus status)
    {
        Object initObj = obj;

        togle = false;

        AssertProperties(status.Status1);
        AssertProperties(status.Status2);
        AssertProperties(status.Status3);
        AssertProperties(status.Status4);
        AssertProperties(status.Status5);
        AssertProperties(status.Status6);

        Assert.Equal(status.Status1.Delegate(), status.Status1.Fixed.CreateDelegate<GetStringDelegate>()());
        Assert.Equal(status.Status2.Delegate(), status.Status2.Fixed.CreateDelegate<GetStringDelegate>()());
        Assert.True(Unsafe.AreSame(ref bytes[0], ref status.Status3.Fixed.CreateDelegate<GetByteSpanDelegate>()(bytes)[0]));
        Assert.False(togle);
        status.Status4.Fixed.CreateDelegate<VoidDelegate>()();
        Assert.True(togle);
        status.Status4.Delegate();
        Assert.False(togle);
        status.Status5.Fixed.CreateDelegate<VoidObjectDelegate>()(obj);
        obj = fixture.Create<String>();
        Assert.ThrowsAny<Exception>(() => status.Status5.Fixed.CreateDelegate<VoidObjectDelegate>()(initObj));
        status.Status5.Fixed.CreateDelegate<VoidObjectDelegate>()(obj);
        Assert.True(Unsafe.AreSame(ref guids[0], ref status.Status6.Fixed.CreateDelegate<GetGuidSpanDelegate>()()[0]));

        AssertUnload(status.Status1.Fixed);
        AssertUnload(status.Status2.Fixed);
        AssertUnload(status.Status3.Fixed);
        AssertUnload(status.Status4.Fixed);
        AssertUnload(status.Status5.Fixed);
        AssertUnload(status.Status6.Fixed);
    }

    private static unsafe void AssertProperties<TDelegate>(FixedDelegateStatus<TDelegate> status)
        where TDelegate : Delegate
    {
        Assert.Equal(sizeof(IntPtr), status.Fixed.BinaryLength);
        Assert.Equal(0, status.Fixed.BinaryOffset);
        Assert.True(status.Fixed.IsReadOnly);
        Assert.Equal(typeof(TDelegate), status.Fixed.Type);

        if (!status.IsFunction)
            Assert.Same(status.Delegate, status.Fixed.CreateDelegate<TDelegate>());
        else
            Assert.NotSame(status.Delegate, status.Fixed.CreateDelegate<TDelegate>());

        AssertErrorType<TDelegate, GetStringDelegate>(status.Fixed);
        AssertErrorType<TDelegate, GetStringDelegate>(status.Fixed);
        AssertErrorType<TDelegate, GetByteSpanDelegate>(status.Fixed);
        AssertErrorType<TDelegate, VoidDelegate>(status.Fixed);
        AssertErrorType<TDelegate, VoidObjectDelegate>(status.Fixed);
    }

    private static unsafe void AssertUnload<TDelegate>(FixedDelegate<TDelegate> fdel)
        where TDelegate : Delegate
    {
        fdel.Unload();
        Exception invalid = Assert.Throws<InvalidOperationException>(() => fdel.CreateDelegate<TDelegate>());
        Assert.Equal(InvalidError, invalid.Message);
    }

    private static void AssertErrorType<TDelegate, TAlien>(FixedDelegate<TDelegate> fdel)
        where TDelegate : Delegate
        where TAlien : Delegate
    {
        if (typeof(TDelegate) != typeof(TAlien))
            Assert.Throws<InvalidCastException>(() => fdel.CreateDelegate<TAlien>());
    }
}
