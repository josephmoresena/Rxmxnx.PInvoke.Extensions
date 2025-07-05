namespace Rxmxnx.PInvoke.Tests.ReferenceExtensionsTests;

[ExcludeFromCodeCoverage]
[SuppressMessage("csharpsquid", "S2699")]
public sealed class WithSafeFixedTest
{
	private static readonly IFixture fixture = new Fixture();

	private IReferenceableWrapper? _wraper;

	[Fact]
	internal void ByteTest() => this.Test<Byte>();
	[Fact]
	internal void CharTest() => this.Test<Char>();
	[Fact]
	internal void DateTimeTest() => this.Test<DateTime>();
	[Fact]
	internal void DecimalTest() => this.Test<Decimal>();
	[Fact]
	internal void DoubleTest() => this.Test<Double>();
	[Fact]
	internal void GuidTest() => this.Test<Guid>();
	[Fact]
	internal void HalfTest() => this.Test<Half>();
	[Fact]
	internal void Int16Test() => this.Test<Int16>();
	[Fact]
	internal void Int32Test() => this.Test<Int32>();
	[Fact]
	internal void Int64Test() => this.Test<Int64>();
	[Fact]
	internal void SByteTest() => this.Test<SByte>();
	[Fact]
	internal void SingleTest() => this.Test<Single>();
	[Fact]
	internal void UInt16Test() => this.Test<UInt16>();
	[Fact]
	internal void UInt32Test() => this.Test<UInt32>();
	[Fact]
	internal void UInt64Test() => this.Test<UInt64>();

	private void Test<T>() where T : unmanaged
	{
		IReferenceableWrapper<T> value = IReferenceableWrapper.Create(WithSafeFixedTest.fixture.Create<T>());
		Byte[] bytes = MemoryMarshal.AsBytes(MemoryMarshal.CreateSpan(ref Unsafe.AsRef(in value.Reference), 1))
		                            .ToArray();
		ref T refValue = ref Unsafe.AsRef(in value.Reference);

		this._wraper = value;

		refValue.WithSafeFixed(this.TestActionMethod);
		refValue.WithSafeFixed(this, WithSafeFixedTest.TestActionMethod);
		Assert.Equal(bytes, refValue.WithSafeFixed(this.TestFuncMethod));
		Assert.Equal(bytes, refValue.WithSafeFixed(this, WithSafeFixedTest.TestFuncMethod));

		refValue.WithSafeFixed(this.TestReadOnlyActionMethod);
		refValue.WithSafeFixed(this, WithSafeFixedTest.TestReadOnlyActionMethod);
		Assert.Equal(bytes, refValue.WithSafeFixed(this.TestReadOnlyFuncMethod));
		Assert.Equal(bytes, refValue.WithSafeFixed(this, WithSafeFixedTest.TestReadOnlyFuncMethod));
	}

	private unsafe void TestActionMethod<T>(in IFixedReference<T> fRef) where T : unmanaged
	{
		IReferenceableWrapper<T> wrapper = (IReferenceableWrapper<T>)this._wraper!;
		Byte[] bytes = new ReadOnlySpan<Byte>(fRef.Pointer.ToPointer(), sizeof(T)).ToArray();
		Assert.Equal(wrapper.Value, fRef.Reference);
		Assert.Equal(wrapper.Value, Unsafe.Read<T>(fRef.Pointer.ToPointer()));
		Assert.Equal(sizeof(T), fRef.Bytes.Length);
		Assert.Equal(bytes, fRef.Bytes.ToArray());

		IFixedContext<Byte> ctx = fRef.AsBinaryContext();

		Assert.Equal(bytes, ctx.Values.ToArray());
		Assert.Equal(bytes, ctx.Bytes.ToArray());
		Assert.Equal(fRef.Pointer, ctx.Pointer);

		WithSafeFixedTest.Test<T, Boolean>(fRef, bytes);
		WithSafeFixedTest.Test<T, Byte>(fRef, bytes);
		WithSafeFixedTest.Test<T, Char>(fRef, bytes);
		WithSafeFixedTest.Test<T, DateTime>(fRef, bytes);
		WithSafeFixedTest.Test<T, Decimal>(fRef, bytes);
		WithSafeFixedTest.Test<T, Double>(fRef, bytes);
		WithSafeFixedTest.Test<T, Guid>(fRef, bytes);
		WithSafeFixedTest.Test<T, Half>(fRef, bytes);
		WithSafeFixedTest.Test<T, Int16>(fRef, bytes);
		WithSafeFixedTest.Test<T, Int32>(fRef, bytes);
		WithSafeFixedTest.Test<T, Int64>(fRef, bytes);
		WithSafeFixedTest.Test<T, SByte>(fRef, bytes);
		WithSafeFixedTest.Test<T, Single>(fRef, bytes);
		WithSafeFixedTest.Test<T, UInt16>(fRef, bytes);
		WithSafeFixedTest.Test<T, UInt32>(fRef, bytes);
		WithSafeFixedTest.Test<T, UInt64>(fRef, bytes);
	}
	private unsafe void TestReadOnlyActionMethod<T>(in IReadOnlyFixedReference<T> fRef) where T : unmanaged
	{
		IReferenceableWrapper<T> wrapper = (IReferenceableWrapper<T>)this._wraper!;
		Byte[] bytes = new ReadOnlySpan<Byte>(fRef.Pointer.ToPointer(), sizeof(T)).ToArray();
		Assert.Equal(wrapper.Value, fRef.Reference);
		Assert.Equal(wrapper.Value, Unsafe.Read<T>(fRef.Pointer.ToPointer()));
		Assert.Equal(sizeof(T), fRef.Bytes.Length);
		Assert.Equal(bytes, fRef.Bytes.ToArray());

		IReadOnlyFixedContext<Byte> ctx = fRef.AsBinaryContext();

		Assert.Equal(bytes, ctx.Values.ToArray());
		Assert.Equal(bytes, ctx.Bytes.ToArray());
		Assert.Equal(fRef.Pointer, ctx.Pointer);

		WithSafeFixedTest.Test<T, Boolean>(fRef, bytes);
		WithSafeFixedTest.Test<T, Byte>(fRef, bytes);
		WithSafeFixedTest.Test<T, Char>(fRef, bytes);
		WithSafeFixedTest.Test<T, DateTime>(fRef, bytes);
		WithSafeFixedTest.Test<T, Decimal>(fRef, bytes);
		WithSafeFixedTest.Test<T, Double>(fRef, bytes);
		WithSafeFixedTest.Test<T, Guid>(fRef, bytes);
		WithSafeFixedTest.Test<T, Half>(fRef, bytes);
		WithSafeFixedTest.Test<T, Int16>(fRef, bytes);
		WithSafeFixedTest.Test<T, Int32>(fRef, bytes);
		WithSafeFixedTest.Test<T, Int64>(fRef, bytes);
		WithSafeFixedTest.Test<T, SByte>(fRef, bytes);
		WithSafeFixedTest.Test<T, Single>(fRef, bytes);
		WithSafeFixedTest.Test<T, UInt16>(fRef, bytes);
		WithSafeFixedTest.Test<T, UInt32>(fRef, bytes);
		WithSafeFixedTest.Test<T, UInt64>(fRef, bytes);
	}
	private unsafe Byte[] TestFuncMethod<T>(in IFixedReference<T> fRef) where T : unmanaged
	{
		this.TestActionMethod(fRef);
		return new ReadOnlySpan<Byte>(fRef.Pointer.ToPointer(), sizeof(T)).ToArray();
	}
	private unsafe Byte[] TestReadOnlyFuncMethod<T>(in IReadOnlyFixedReference<T> fRef) where T : unmanaged
	{
		this.TestReadOnlyActionMethod(fRef);
		return new ReadOnlySpan<Byte>(fRef.Pointer.ToPointer(), sizeof(T)).ToArray();
	}

	private static void TestActionMethod<T>(in IFixedReference<T> fRef, WithSafeFixedTest test) where T : unmanaged
		=> test.TestActionMethod(fRef);
	private static void TestReadOnlyActionMethod<T>(in IReadOnlyFixedReference<T> fRef, WithSafeFixedTest test)
		where T : unmanaged
		=> test.TestReadOnlyActionMethod(fRef);
	private static Byte[] TestFuncMethod<T>(in IFixedReference<T> fRef, WithSafeFixedTest test) where T : unmanaged
		=> test.TestFuncMethod(fRef);
	private static Byte[] TestReadOnlyFuncMethod<T>(in IReadOnlyFixedReference<T> fRef, WithSafeFixedTest test)
		where T : unmanaged
		=> test.TestReadOnlyFuncMethod(fRef);
	private static unsafe void Test<T, T2>(IFixedReference<T> fRef, Byte[] bytes)
		where T : unmanaged where T2 : unmanaged
	{
		if (sizeof(T) >= sizeof(T2))
		{
			IFixedReference<T2> fRef2 = fRef.Transformation<T2>(out IFixedMemory residual);
			IFixedContext<Byte> ctx = fRef2.AsBinaryContext();
			IFixedContext<Byte> ctxR = residual.AsBinaryContext();

			Assert.Equal(fRef2, fRef.Transformation<T2>(out IReadOnlyFixedMemory residual2));
			Assert.Equal(residual, residual2);
			if (typeof(T) == typeof(T2))
				Assert.Equal((Object)fRef.Reference, fRef2.Reference);
			else
				Assert.Equal(bytes, fRef2.Bytes.ToArray());

			Assert.Equal(sizeof(T) == sizeof(T2), residual.Bytes.IsEmpty);
			Assert.Equal(sizeof(T) == sizeof(T2), ctxR.Bytes.IsEmpty);
			Assert.Equal(fRef.Pointer + sizeof(T2), residual.Pointer);
			Assert.Equal(fRef.Pointer, fRef2.Pointer);

			Assert.Equal(bytes, ctx.Values.ToArray());
			Assert.Equal(bytes, ctx.Bytes.ToArray());
			Assert.Equal(fRef2.Pointer, ctx.Pointer);
			Assert.Equal(bytes[sizeof(T2)..], ctxR.Bytes.ToArray());
			Assert.Equal(residual.Pointer, ctxR.Pointer);
		}
		else
		{
			Assert.Throws<InsufficientMemoryException>(() => fRef.Transformation<T2>(out IFixedMemory _));
			Assert.Throws<InsufficientMemoryException>(() => fRef.Transformation<T2>(out IReadOnlyFixedMemory _));
		}
	}
	private static unsafe void Test<T, T2>(IReadOnlyFixedReference<T> fRef, Byte[] bytes)
		where T : unmanaged where T2 : unmanaged
	{
		if (sizeof(T) >= sizeof(T2))
		{
			IReadOnlyFixedReference<T2> fRef2 = fRef.Transformation<T2>(out IReadOnlyFixedMemory residual);
			IReadOnlyFixedContext<Byte> ctx = fRef2.AsBinaryContext();
			IReadOnlyFixedContext<Byte> ctxR = residual.AsBinaryContext();

			if (typeof(T) == typeof(T2))
				Assert.Equal((Object)fRef.Reference, fRef2.Reference);
			else
				Assert.Equal(bytes, fRef2.Bytes.ToArray());

			Assert.Equal(sizeof(T) == sizeof(T2), residual.Bytes.IsEmpty);
			Assert.Equal(sizeof(T) == sizeof(T2), ctxR.Bytes.IsEmpty);
			Assert.Equal(fRef.Pointer + sizeof(T2), residual.Pointer);
			Assert.Equal(fRef.Pointer, fRef2.Pointer);

			Assert.Equal(bytes, ctx.Values.ToArray());
			Assert.Equal(bytes, ctx.Bytes.ToArray());
			Assert.Equal(fRef2.Pointer, ctx.Pointer);
			Assert.Equal(bytes[sizeof(T2)..], ctxR.Bytes.ToArray());
			Assert.Equal(residual.Pointer, ctxR.Pointer);
		}
		else
		{
			Assert.Throws<InsufficientMemoryException>(() => fRef.Transformation<T2>(out _));
		}
	}
}