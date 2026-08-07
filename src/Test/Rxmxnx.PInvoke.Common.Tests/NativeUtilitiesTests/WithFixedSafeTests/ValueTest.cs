namespace Rxmxnx.PInvoke.Tests.NativeUtilitiesTests.WithFixedSafeTests;

[TestFixture]
[ExcludeFromCodeCoverage]
[SuppressMessage("csharpsquid", "S2699")]
public sealed class ValueTest
{
	private static readonly IFixture fixture = new Fixture();

#if NETSTANDARD2_1 && !LEGACY || NETCOREAPP3_0_OR_GREATER
	private IReferenceableWrapper? _wraper;
#else
	private Object? _wraper;
#endif

	[Fact]
	public void ByteTest() => this.Test<Byte>();
	[Fact]
	public void CharTest() => this.Test<Char>();
	[Fact]
	public void DateTimeTest() => this.Test<DateTime>();
	[Fact]
	public void DecimalTest() => this.Test<Decimal>();
	[Fact]
	public void DoubleTest() => this.Test<Double>();
	[Fact]
	public void GuidTest() => this.Test<Guid>();
#if NET5_0_OR_GREATER
	[Fact]
	internal void HalfTest() => this.Test<Half>();
#endif
	[Fact]
	public void Int16Test() => this.Test<Int16>();
	[Fact]
	public void Int32Test() => this.Test<Int32>();
	[Fact]
	public void Int64Test() => this.Test<Int64>();
	[Fact]
	public void SByteTest() => this.Test<SByte>();
	[Fact]
	public void SingleTest() => this.Test<Single>();
	[Fact]
	public void UInt16Test() => this.Test<UInt16>();
	[Fact]
	public void UInt32Test() => this.Test<UInt32>();
	[Fact]
	public void UInt64Test() => this.Test<UInt64>();

#pragma warning disable CS0612
	private void Test<T>() where T : unmanaged
	{
#if NETSTANDARD2_1 && !LEGACY || NETCOREAPP3_0_OR_GREATER
		IReferenceableWrapper<T> value = IReferenceableWrapper.Create(ValueTest.fixture.Create<T>());
#else
		IReferenceableWrapper<T> value = WrapperFactory.CreateReadOnlyReferenceable(ValueTest.fixture.Create<T>());
#endif
#if NETSTANDARD2_1 || NETCOREAPP2_1_OR_GREATER
		Byte[] bytes = MemoryMarshal.AsBytes(MemoryMarshal.CreateSpan(ref Unsafe.AsRef(in value.Reference), 1))
		                            .ToArray();
#else

		Byte[] bytes = NativeUtilities.ToBytes(in value.Reference);
#endif

		this._wraper = value;
		NativeUtilities.WithSafeFixed(value.Reference, this.TestActionMethod);
		NativeUtilities.WithSafeFixed(value.Reference, this, ValueTest.TestActionMethod);
		PInvokeAssert.Equal(bytes, NativeUtilities.WithSafeFixed(value.Reference, this.TestFuncMethod));
		PInvokeAssert.Equal(bytes, NativeUtilities.WithSafeFixed(value.Reference, this, ValueTest.TestFuncMethod));
	}
#pragma warning restore CS0612

	[Obsolete]
	private unsafe void TestActionMethod<T>(in IReadOnlyFixedReference<T> fRef) where T : unmanaged
	{
		IReferenceableWrapper<T> wrapper = (IReferenceableWrapper<T>)this._wraper!;
		Byte[] bytes = new ReadOnlySpan<Byte>(fRef.Pointer.ToPointer(), sizeof(T)).ToArray();
		PInvokeAssert.Equal(wrapper.Value, fRef.Reference);
		PInvokeAssert.Equal(wrapper.Value, Unsafe.Read<T>(fRef.Pointer.ToPointer()));
#if NETSTANDARD2_1 && !LEGACY || NETCOREAPP3_0_OR_GREATER
		PInvokeAssert.Equal(sizeof(T), fRef.Bytes.Length);
		PInvokeAssert.Equal(bytes, fRef.Bytes.ToArray());

		IReadOnlyFixedContext<Byte> ctx = fRef.AsBinaryContext();

		PInvokeAssert.Equal(bytes, ctx.Values.ToArray());
		PInvokeAssert.Equal(bytes, ctx.Bytes.ToArray());
		PInvokeAssert.Equal(fRef.Pointer, ctx.Pointer);
#endif

		ValueTest.Test<T, Boolean>(fRef, bytes);
		ValueTest.Test<T, Byte>(fRef, bytes);
		ValueTest.Test<T, Char>(fRef, bytes);
		ValueTest.Test<T, DateTime>(fRef, bytes);
		ValueTest.Test<T, Decimal>(fRef, bytes);
		ValueTest.Test<T, Double>(fRef, bytes);
		ValueTest.Test<T, Guid>(fRef, bytes);
#if NET5_0_OR_GREATER
		ValueTest.Test<T, Half>(fRef, bytes);
#endif
		ValueTest.Test<T, Int16>(fRef, bytes);
		ValueTest.Test<T, Int32>(fRef, bytes);
		ValueTest.Test<T, Int64>(fRef, bytes);
		ValueTest.Test<T, SByte>(fRef, bytes);
		ValueTest.Test<T, Single>(fRef, bytes);
		ValueTest.Test<T, UInt16>(fRef, bytes);
		ValueTest.Test<T, UInt32>(fRef, bytes);
		ValueTest.Test<T, UInt64>(fRef, bytes);
	}
#pragma warning disable CS0612
	private unsafe Byte[] TestFuncMethod<T>(in IReadOnlyFixedReference<T> fRef) where T : unmanaged
	{
		this.TestActionMethod(fRef);
		return new ReadOnlySpan<Byte>(fRef.Pointer.ToPointer(), sizeof(T)).ToArray();
	}

	private static void TestActionMethod<T>(in IReadOnlyFixedReference<T> fRef, ValueTest test) where T : unmanaged
		=> test.TestActionMethod(fRef);
#pragma warning restore CS0612
	private static Byte[] TestFuncMethod<T>(in IReadOnlyFixedReference<T> fRef, ValueTest test) where T : unmanaged
		=> test.TestFuncMethod(fRef);
	[Obsolete]
	private static unsafe void Test<T, T2>(IReadOnlyFixedReference<T> fRef, Byte[] bytes)
		where T : unmanaged where T2 : unmanaged
	{
		if (sizeof(T) >= sizeof(T2))
		{
#if NETSTANDARD2_1 && !LEGACY || NETCOREAPP3_0_OR_GREATER
			IReadOnlyFixedReference<T2> fRef2 = fRef.Transformation<T2>(out IReadOnlyFixedMemory residual);
			IReadOnlyFixedContext<Byte> ctx = fRef2.AsBinaryContext();
			IReadOnlyFixedContext<Byte> ctxR = residual.AsBinaryContext();
#else
			IReadOnlyFixedReference<T2> fRef2 = fRef.Transformation<T2>();
#endif

			if (typeof(T) == typeof(T2))
				PInvokeAssert.Equal((Object)fRef.Reference, fRef2.Reference);
			else if (sizeof(T) % sizeof(T2) == 0)
#if (!NETSTANDARD2_1 || LEGACY) && !NETCOREAPP3_0_OR_GREATER
				PInvokeAssert.Equal(bytes, NativeUtilities.ToBytes(in fRef2.Reference));
#else
				PInvokeAssert.Equal(bytes, fRef2.Bytes.ToArray());
			else
				PInvokeAssert.Equal(bytes, fRef2.Bytes.ToArray().Concat(residual.Bytes.ToArray()));

			PInvokeAssert.Equal(sizeof(T) == sizeof(T2), residual.Bytes.IsEmpty);
			PInvokeAssert.Equal(sizeof(T) == sizeof(T2), ctxR.Bytes.IsEmpty);
			PInvokeAssert.Equal(fRef.Pointer + sizeof(T2), residual.Pointer);
			PInvokeAssert.Equal(fRef.Pointer, fRef2.Pointer);

			PInvokeAssert.Equal(bytes, ctx.Values.ToArray());
			PInvokeAssert.Equal(bytes, ctx.Bytes.ToArray());
			PInvokeAssert.Equal(fRef2.Pointer, ctx.Pointer);
			PInvokeAssert.Equal(bytes[sizeof(T2)..], ctxR.Bytes.ToArray());
			PInvokeAssert.Equal(residual.Pointer, ctxR.Pointer);
#endif
		}
		else
		{
#if NETSTANDARD2_1 && !LEGACY || NETCOREAPP3_0_OR_GREATER
			PInvokeAssert.Throws<InsufficientMemoryException>(() => fRef.Transformation<T2>(out _));
#else
			PInvokeAssert.Throws<InsufficientMemoryException>(fRef.Transformation<T2>);
#endif
		}
	}
}