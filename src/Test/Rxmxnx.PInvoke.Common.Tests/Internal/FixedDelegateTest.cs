#if !NETCOREAPP
using Fact = NUnit.Framework.TestAttribute;
#endif

namespace Rxmxnx.PInvoke.Tests.Internal;

[TestFixture]
[ExcludeFromCodeCoverage]
public sealed class FixedDelegateTest : FixedMemoryTestsBase
{
	[Fact]
	public void Test()
	{
		String str1 = FixedMemoryTestsBase.Fixture.Create<String>();
		String str2 = FixedMemoryTestsBase.Fixture.Create<String>();
#if NETCOREAPP
		Byte[] bytes = FixedMemoryTestsBase.Fixture.Create<Byte[]>();
		Guid[] guids = FixedMemoryTestsBase.Fixture.CreateMany<Guid>(10).ToArray();
#endif
		Object obj = new();
		Boolean called = false;

#pragma warning disable IDE0039
		GetStringDelegate d1 = () => str1;
		GetStringDelegate d2 = () => str2 + str1;
#if NETCOREAPP
		GetByteSpanDelegate d3 = bArr => bArr;
#endif
		VoidDelegate d4 = () => called = !called;
		VoidObjectDelegate d5 = o => PInvokeAssert.Same(obj, o);
#if NETCOREAPP
		GetGuidSpanDelegate d6 = () => guids;
#endif
#pragma warning restore IDE0039

		FixedDelegateTestStatus delegateStatus = new()
		{
			Status1 = new(false, new(d1)) { Delegate = d1, },
			Status2 = new(false, new(d2)) { Delegate = d2, },
#if NETCOREAPP
			Status3 = new(false, new(d3)) { Delegate = d3, },
#endif
			Status4 = new(false, new(d4)) { Delegate = d4, },
			Status5 = new(false, new(d5)) { Delegate = d5, },
#if NETCOREAPP
			Status6 = new(false, new(d6)) { Delegate = d6, },
#endif
		};

		FixedDelegateTestStatus functionStatus = new()
		{
			Status1 = new(true, new(F1)) { Delegate = F1, },
			Status2 = new(true, new(F2)) { Delegate = F2, },
#if NETCOREAPP
			Status3 = new(true, new(F3)) { Delegate = F3, },
#endif
			Status4 = new(true, new(F4)) { Delegate = F4, },
			Status5 = new(true, new(F5)) { Delegate = F5, },
#if NETCOREAPP
			Status6 = new(true, new(F6)) { Delegate = F6, },
#endif
		};

#if NETCOREAPP
		FixedDelegateTest.AssertTest(bytes, guids, ref obj, ref called, delegateStatus);
		FixedDelegateTest.AssertTest(bytes, guids, ref obj, ref called, functionStatus);
#else
		FixedDelegateTest.AssertTest(ref obj, ref called, delegateStatus);
		FixedDelegateTest.AssertTest(ref obj, ref called, functionStatus);
#endif
		return;

		String F1() => str1;
		String F2() => str2 + str1;
#if NETCOREAPP
		Span<Byte> F3(Byte[] bArr) => bArr;
#endif
		void F4() => called = !called;
		void F5(Object o) => PInvokeAssert.Same(obj, o);
#if NETCOREAPP
		Span<Guid> F6() => guids;
#endif
	}

#if NETCOREAPP
	private static void AssertTest(Byte[] bytes, Guid[] guids, ref Object obj, ref Boolean toggle,
		FixedDelegateTestStatus status)
#else
	private static void AssertTest(ref Object obj, ref Boolean toggle, FixedDelegateTestStatus status)
#endif
	{
		Object initObj = obj;

		toggle = false;

		FixedDelegateTest.AssertProperties(status.Status1);
		FixedDelegateTest.AssertProperties(status.Status2);
#if NETCOREAPP
		FixedDelegateTest.AssertProperties(status.Status3);
#endif
		FixedDelegateTest.AssertProperties(status.Status4);
		FixedDelegateTest.AssertProperties(status.Status5);
#if NETCOREAPP
		FixedDelegateTest.AssertProperties(status.Status6);
#endif

		PInvokeAssert.Equal(status.Status1.Delegate(), status.Status1.Fixed.CreateDelegate<GetStringDelegate>()());
		PInvokeAssert.Equal(status.Status2.Delegate(), status.Status2.Fixed.CreateDelegate<GetStringDelegate>()());
#if NETCOREAPP
		Assert.True(Unsafe.AreSame(ref bytes[0],
		                           ref status.Status3.Fixed.CreateDelegate<GetByteSpanDelegate>()(bytes)[0]));
#endif
		PInvokeAssert.False(toggle);
		status.Status4.Fixed.CreateDelegate<VoidDelegate>()();
		PInvokeAssert.True(toggle);
		status.Status4.Delegate();
		PInvokeAssert.False(toggle);
		status.Status5.Fixed.CreateDelegate<VoidObjectDelegate>()(obj);
		obj = FixedMemoryTestsBase.Fixture.Create<String>();
		PInvokeAssert.ThrowsAny<Exception>(() => status.Status5.Fixed.CreateDelegate<VoidObjectDelegate>()(initObj));
		status.Status5.Fixed.CreateDelegate<VoidObjectDelegate>()(obj);
#if NETCOREAPP
		Assert.True(Unsafe.AreSame(ref guids[0], ref status.Status6.Fixed.CreateDelegate<GetGuidSpanDelegate>()()[0]));
#endif
		FixedDelegateTest.AssertUnload(status.Status1.Fixed);
		FixedDelegateTest.AssertUnload(status.Status2.Fixed);
#if NETCOREAPP
		FixedDelegateTest.AssertUnload(status.Status3.Fixed);
#endif
		FixedDelegateTest.AssertUnload(status.Status4.Fixed);
		FixedDelegateTest.AssertUnload(status.Status5.Fixed);
#if NETCOREAPP
		FixedDelegateTest.AssertUnload(status.Status6.Fixed);
#endif
	}

	private static unsafe void AssertProperties<TDelegate>(FixedDelegateStatus<TDelegate> status)
		where TDelegate : Delegate
	{
		PInvokeAssert.Equal(sizeof(IntPtr), status.Fixed.BinaryLength);
		PInvokeAssert.Equal(0, status.Fixed.BinaryOffset);
		PInvokeAssert.True(status.Fixed.IsReadOnly);
		PInvokeAssert.Equal(typeof(TDelegate), status.Fixed.Type);
		PInvokeAssert.True(status.Fixed.IsFunction);

		if (!status.IsFunction)
			PInvokeAssert.Same(status.Delegate, status.Fixed.CreateDelegate<TDelegate>());
		else
			PInvokeAssert.NotSame(status.Delegate, status.Fixed.CreateDelegate<TDelegate>());

		FixedDelegateTest.AssertErrorType<TDelegate, GetStringDelegate>(status.Fixed);
		FixedDelegateTest.AssertErrorType<TDelegate, GetStringDelegate>(status.Fixed);
#if NETCOREAPP
		FixedDelegateTest.AssertErrorType<TDelegate, GetByteSpanDelegate>(status.Fixed);
#endif
		FixedDelegateTest.AssertErrorType<TDelegate, VoidDelegate>(status.Fixed);
		FixedDelegateTest.AssertErrorType<TDelegate, VoidObjectDelegate>(status.Fixed);

		FixedDelegateTest.AssertFunction(status.Fixed);
	}

	private static void AssertUnload<TDelegate>(FixedDelegate<TDelegate> fdel) where TDelegate : Delegate
	{
		fdel.Unload();
		Exception invalid = PInvokeAssert.Throws<InvalidOperationException>(fdel.CreateDelegate<TDelegate>);
		PInvokeAssert.Equal(FixedMemoryTestsBase.InvalidError, invalid.Message);
		FixedDelegateTest.AssertFunction(fdel);
	}

	private static void AssertErrorType<TDelegate, TAlien>(FixedDelegate<TDelegate> fdel)
		where TDelegate : Delegate where TAlien : Delegate
	{
		if (typeof(TDelegate) != typeof(TAlien))
			PInvokeAssert.Throws<InvalidCastException>(fdel.CreateDelegate<TAlien>);
	}

	private static void AssertFunction<TDelegate>(FixedDelegate<TDelegate> fdel) where TDelegate : Delegate
	{
		Exception functionException1 =
			PInvokeAssert.Throws<InvalidOperationException>(() => fdel.CreateReadOnlyReference<Int32>());
		PInvokeAssert.Equal(FixedMemoryTestsBase.IsFunction, functionException1.Message);
		Exception functionException2 =
			PInvokeAssert.Throws<InvalidOperationException>(() => fdel.CreateReadOnlyBinarySpan());
		PInvokeAssert.Equal(FixedMemoryTestsBase.IsFunction, functionException2.Message);
		Exception functionException3 =
			PInvokeAssert.Throws<InvalidOperationException>(() => fdel.CreateReadOnlySpan<Int32>(0));
		PInvokeAssert.Equal(FixedMemoryTestsBase.IsFunction, functionException3.Message);

		Exception functionException4 =
			PInvokeAssert.Throws<InvalidOperationException>(() => fdel.CreateReference<Int32>());
		PInvokeAssert.Equal(FixedMemoryTestsBase.IsFunction, functionException4.Message);
		Exception functionException5 = PInvokeAssert.Throws<InvalidOperationException>(() => fdel.CreateBinarySpan());
		PInvokeAssert.Equal(FixedMemoryTestsBase.IsFunction, functionException5.Message);
		Exception functionException6 = PInvokeAssert.Throws<InvalidOperationException>(() => fdel.CreateSpan<Int32>(0));
		PInvokeAssert.Equal(FixedMemoryTestsBase.IsFunction, functionException6.Message);
		Exception functionException7 = PInvokeAssert.Throws<InvalidOperationException>(() => fdel.CreateObjectSpan());
		PInvokeAssert.Equal(FixedMemoryTestsBase.IsFunction, functionException7.Message);
	}
}