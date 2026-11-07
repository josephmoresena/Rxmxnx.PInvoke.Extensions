namespace Rxmxnx.PInvoke.Tests.FixedReferenceValueTests;

[TestFixture]
[ExcludeFromCodeCoverage]
[SuppressMessage("csharpsquid", "S2699")]
public sealed class CreateValueTest : FixedReferenceTestsBase
{
	[Fact]
	public void ByteTest() => CreateValueTest.Test<Byte>();
	[Fact]
	public void Int16Test() => CreateValueTest.Test<Int16>();
	[Fact]
	public void Int32Test() => CreateValueTest.Test<Int32>();
	[Fact]
	public void Int64Test() => CreateValueTest.Test<Int64>();
	[Fact]
	public void GuidTest() => CreateValueTest.Test<Guid>();

	private static void Test<T>() where T : unmanaged
	{
		T value = FixedMemoryTestsBase.Fixture.Create<T>();
		FixedReferenceTestsBase.WithFixed(ref value, CreateValueTest.AssertFromFixedReference);
		FixedReferenceTestsBase.WithFixed(ref value, CreateValueTest.AssertFromReadOnlyFixedReference);
		CreateValueTest.AssertEmptyReference();
	}

	private static unsafe void AssertFromFixedReference<T>(FixedReference<T> fref, IntPtr ptr) where T : unmanaged
	{
		FixedReferenceValue<T> value = FixedReferenceValue<T>.CreateValue(fref);
		PInvokeAssert.Equal(ptr, value.Pointer);
		PInvokeAssert.True(Unsafe.AreSame(ref fref.CreateReference<T>(), ref value.Reference));
		PInvokeAssert.Equal(Unsafe.ReadUnaligned<T>(ptr.ToPointer()), value.Reference);

#if NETSTANDARD2_1 || NETCOREAPP3_0_OR_GREATER
		using IFixedReference<T>.IDisposable disposable = fref.ToDisposable(null);
		FixedReferenceValue<T> fromDisposable = FixedReferenceValue<T>.CreateValue(disposable);
		PInvokeAssert.Equal(ptr, fromDisposable.Pointer);
		PInvokeAssert.True(Unsafe.AreSame(ref fref.CreateReference<T>(), ref fromDisposable.Reference));
#endif
	}
	private static unsafe void AssertFromReadOnlyFixedReference<T>(ReadOnlyFixedReference<T> fref, IntPtr ptr)
		where T : unmanaged
	{
		ReadOnlyFixedReferenceValue<T> value = ReadOnlyFixedReferenceValue<T>.CreateValue(fref);
		PInvokeAssert.Equal(ptr, value.Pointer);
		PInvokeAssert.Equal(Unsafe.ReadUnaligned<T>(ptr.ToPointer()), value.Reference);
#if NET8_0_OR_GREATER
		Assert.True(Unsafe.AreSame(in fref.CreateReadOnlyReference<T>(), in value.Reference));
#else
		PInvokeAssert.True(Unsafe.AreSame(ref Unsafe.AsRef(in fref.CreateReadOnlyReference<T>()),
		                                  ref Unsafe.AsRef(in value.Reference)));
#endif

#if NETSTANDARD2_1 || NETCOREAPP3_0_OR_GREATER
		using IReadOnlyFixedReference<T>.IDisposable disposable = fref.ToDisposable(null);
		ReadOnlyFixedReferenceValue<T> fromDisposable = ReadOnlyFixedReferenceValue<T>.CreateValue(disposable);
		PInvokeAssert.Equal(ptr, fromDisposable.Pointer);
		PInvokeAssert.Equal(Unsafe.ReadUnaligned<T>(ptr.ToPointer()), fromDisposable.Reference);
#endif
	}
	private static unsafe void AssertEmptyReference()
	{
		FixedReference<Int32> empty = new(null);
		FixedReferenceValue<Int32> value = FixedReferenceValue<Int32>.CreateValue(empty);
		PInvokeAssert.True(value.IsNullOrEmpty);
		PInvokeAssert.Equal(IntPtr.Zero, value.Pointer);

		ReadOnlyFixedReference<Int32> readOnlyEmpty = new(null);
		ReadOnlyFixedReferenceValue<Int32>
			readOnlyValue = ReadOnlyFixedReferenceValue<Int32>.CreateValue(readOnlyEmpty);
		PInvokeAssert.True(readOnlyValue.IsNullOrEmpty);
		PInvokeAssert.Equal(IntPtr.Zero, readOnlyValue.Pointer);

#if NETSTANDARD2_1 || NETCOREAPP3_0_OR_GREATER
		using IFixedReference<Int32>.IDisposable disposable = empty.ToDisposable(null);
		FixedReferenceValue<Int32> fromDisposable = FixedReferenceValue<Int32>.CreateValue(disposable);
		using IReadOnlyFixedReference<Int32>.IDisposable readOnlyDisposable = readOnlyEmpty.ToDisposable(null);
		ReadOnlyFixedReferenceValue<Int32> fromReadOnlyDisposable =
			ReadOnlyFixedReferenceValue<Int32>.CreateValue(readOnlyDisposable);
		PInvokeAssert.True(fromDisposable.IsNullOrEmpty);
		PInvokeAssert.True(fromReadOnlyDisposable.IsNullOrEmpty);
#endif
	}
}