namespace Rxmxnx.PInvoke.Tests.Internal;

[ExcludeFromCodeCoverage]
public sealed class FixedDelegateTest : FixedMemoryTestsBase
{
	[Fact]
	internal void Test()
	{
		String str1 = FixedMemoryTestsBase.Fixture.Create<String>();
		String str2 = FixedMemoryTestsBase.Fixture.Create<String>();
		Byte[] bytes = FixedMemoryTestsBase.Fixture.Create<Byte[]>();
		Guid[] guids = FixedMemoryTestsBase.Fixture.CreateMany<Guid>(10).ToArray();
		Object obj = new();
		Boolean called = false;

#pragma warning disable IDE0039
		GetStringDelegate d1 = () => str1;
		GetStringDelegate d2 = () => str2 + str1;
		GetByteSpanDelegate d3 = bArr => bArr;
		VoidDelegate d4 = () => called = !called;
		VoidObjectDelegate d5 = o => Assert.Same(obj, o);
		GetGuidSpanDelegate d6 = () => guids;
#pragma warning restore IDE0039

		FixedDelegateTestStatus delegateStatus = new()
		{
			Status1 = new(false, new(d1)) { Delegate = d1, },
			Status2 = new(false, new(d2)) { Delegate = d2, },
			Status3 = new(false, new(d3)) { Delegate = d3, },
			Status4 = new(false, new(d4)) { Delegate = d4, },
			Status5 = new(false, new(d5)) { Delegate = d5, },
			Status6 = new(false, new(d6)) { Delegate = d6, },
		};

		FixedDelegateTestStatus functionStatus = new()
		{
			Status1 = new(true, new(F1)) { Delegate = F1, },
			Status2 = new(true, new(F2)) { Delegate = F2, },
			Status3 = new(true, new(F3)) { Delegate = F3, },
			Status4 = new(true, new(F4)) { Delegate = F4, },
			Status5 = new(true, new(F5)) { Delegate = F5, },
			Status6 = new(true, new(F6)) { Delegate = F6, },
		};

		FixedDelegateTest.AssertTest(bytes, guids, ref obj, ref called, delegateStatus);
		FixedDelegateTest.AssertTest(bytes, guids, ref obj, ref called, functionStatus);
		return;

		String F1() => str1;
		String F2() => str2 + str1;
		Span<Byte> F3(Byte[] bArr) => bArr;
		void F4() => called = !called;
		void F5(Object o) => Assert.Same(obj, o);
		Span<Guid> F6() => guids;
	}

	private static void AssertTest(Byte[] bytes, Guid[] guids, ref Object obj, ref Boolean toggle,
		FixedDelegateTestStatus status)
	{
		Object initObj = obj;

		toggle = false;

		FixedDelegateTest.AssertProperties(status.Status1);
		FixedDelegateTest.AssertProperties(status.Status2);
		FixedDelegateTest.AssertProperties(status.Status3);
		FixedDelegateTest.AssertProperties(status.Status4);
		FixedDelegateTest.AssertProperties(status.Status5);
		FixedDelegateTest.AssertProperties(status.Status6);

		Assert.Equal(status.Status1.Delegate(), status.Status1.Fixed.CreateDelegate<GetStringDelegate>()());
		Assert.Equal(status.Status2.Delegate(), status.Status2.Fixed.CreateDelegate<GetStringDelegate>()());
		Assert.True(Unsafe.AreSame(ref bytes[0],
		                           ref status.Status3.Fixed.CreateDelegate<GetByteSpanDelegate>()(bytes)[0]));
		Assert.False(toggle);
		status.Status4.Fixed.CreateDelegate<VoidDelegate>()();
		Assert.True(toggle);
		status.Status4.Delegate();
		Assert.False(toggle);
		status.Status5.Fixed.CreateDelegate<VoidObjectDelegate>()(obj);
		obj = FixedMemoryTestsBase.Fixture.Create<String>();
		Assert.ThrowsAny<Exception>(() => status.Status5.Fixed.CreateDelegate<VoidObjectDelegate>()(initObj));
		status.Status5.Fixed.CreateDelegate<VoidObjectDelegate>()(obj);
		Assert.True(Unsafe.AreSame(ref guids[0], ref status.Status6.Fixed.CreateDelegate<GetGuidSpanDelegate>()()[0]));

		FixedDelegateTest.AssertUnload(status.Status1.Fixed);
		FixedDelegateTest.AssertUnload(status.Status2.Fixed);
		FixedDelegateTest.AssertUnload(status.Status3.Fixed);
		FixedDelegateTest.AssertUnload(status.Status4.Fixed);
		FixedDelegateTest.AssertUnload(status.Status5.Fixed);
		FixedDelegateTest.AssertUnload(status.Status6.Fixed);
	}

	private static unsafe void AssertProperties<TDelegate>(FixedDelegateStatus<TDelegate> status)
		where TDelegate : Delegate
	{
		Assert.Equal(sizeof(IntPtr), status.Fixed.BinaryLength);
		Assert.Equal(0, status.Fixed.BinaryOffset);
		Assert.True(status.Fixed.IsReadOnly);
		Assert.Equal(typeof(TDelegate), status.Fixed.Type);
		Assert.True(status.Fixed.IsFunction);

		if (!status.IsFunction)
			Assert.Same(status.Delegate, status.Fixed.CreateDelegate<TDelegate>());
		else
			Assert.NotSame(status.Delegate, status.Fixed.CreateDelegate<TDelegate>());

		FixedDelegateTest.AssertErrorType<TDelegate, GetStringDelegate>(status.Fixed);
		FixedDelegateTest.AssertErrorType<TDelegate, GetStringDelegate>(status.Fixed);
		FixedDelegateTest.AssertErrorType<TDelegate, GetByteSpanDelegate>(status.Fixed);
		FixedDelegateTest.AssertErrorType<TDelegate, VoidDelegate>(status.Fixed);
		FixedDelegateTest.AssertErrorType<TDelegate, VoidObjectDelegate>(status.Fixed);

		FixedDelegateTest.AssertFunction(status.Fixed);
	}

	private static void AssertUnload<TDelegate>(FixedDelegate<TDelegate> fdel) where TDelegate : Delegate
	{
		fdel.Unload();
		Exception invalid = Assert.Throws<InvalidOperationException>(fdel.CreateDelegate<TDelegate>);
		Assert.Equal(FixedMemoryTestsBase.InvalidError, invalid.Message);
		FixedDelegateTest.AssertFunction(fdel);
	}

	private static void AssertErrorType<TDelegate, TAlien>(FixedDelegate<TDelegate> fdel)
		where TDelegate : Delegate where TAlien : Delegate
	{
		if (typeof(TDelegate) != typeof(TAlien))
			Assert.Throws<InvalidCastException>(fdel.CreateDelegate<TAlien>);
	}

	private static void AssertFunction<TDelegate>(FixedDelegate<TDelegate> fdel) where TDelegate : Delegate
	{
		Exception functionException1 =
			Assert.Throws<InvalidOperationException>(() => fdel.CreateReadOnlyReference<Int32>());
		Assert.Equal(FixedMemoryTestsBase.IsFunction, functionException1.Message);
		Exception functionException2 = Assert.Throws<InvalidOperationException>(() => fdel.CreateReadOnlyBinarySpan());
		Assert.Equal(FixedMemoryTestsBase.IsFunction, functionException2.Message);
		Exception functionException3 =
			Assert.Throws<InvalidOperationException>(() => fdel.CreateReadOnlySpan<Int32>(0));
		Assert.Equal(FixedMemoryTestsBase.IsFunction, functionException3.Message);

		Exception functionException4 = Assert.Throws<InvalidOperationException>(() => fdel.CreateReference<Int32>());
		Assert.Equal(FixedMemoryTestsBase.IsFunction, functionException4.Message);
		Exception functionException5 = Assert.Throws<InvalidOperationException>(() => fdel.CreateBinarySpan());
		Assert.Equal(FixedMemoryTestsBase.IsFunction, functionException5.Message);
		Exception functionException6 = Assert.Throws<InvalidOperationException>(() => fdel.CreateSpan<Int32>(0));
		Assert.Equal(FixedMemoryTestsBase.IsFunction, functionException6.Message);
	}
}