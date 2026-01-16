namespace Rxmxnx.PInvoke.Tests.MemoryBlockExtensionsTest;

[TestFixture]
[ExcludeFromCodeCoverage]
[SuppressMessage("csharpsquid", "S2699")]
public sealed class CovariantSpanTest
{
	private static readonly IFixture fixture = new Fixture();

	[Fact]
	public void ByteTest() => CovariantSpanTest.UnmanagedTest<Byte>();
	[Fact]
	public void SByteTest() => CovariantSpanTest.UnmanagedTest<SByte>();
	[Fact]
	public void Int16Test() => CovariantSpanTest.UnmanagedTest<Int16>();
	[Fact]
	public void UInt16Test() => CovariantSpanTest.UnmanagedTest<UInt16>();
	[Fact]
	public void Int32Test() => CovariantSpanTest.UnmanagedTest<Int32>();
	[Fact]
	public void UInt32Test() => CovariantSpanTest.UnmanagedTest<UInt32>();
	[Fact]
	public void Int64Test() => CovariantSpanTest.UnmanagedTest<Int64>();
	[Fact]
	public void UInt64Test() => CovariantSpanTest.UnmanagedTest<UInt64>();
	[Fact]
	public void StringTest()
		=> CovariantSpanTest.Test<String, Object>(CovariantSpanTest.fixture.CreateMany<String>(10).ToArray());

	private static void UnmanagedTest<T>() where T : unmanaged
	{
		Type typeofT = typeof(T);
		T[] array = CovariantSpanTest.fixture.CreateMany<T>(10).ToArray();
		switch (Unsafe.SizeOf<T>())
		{
			case 1:
				if (typeofT != typeof(Byte)) CovariantSpanTest.Test<T, Byte>(array);
				if (typeofT != typeof(SByte)) CovariantSpanTest.Test<T, SByte>(array);
				break;
			case 2:
				if (typeofT != typeof(Int16)) CovariantSpanTest.Test<T, Int16>(array);
				if (typeofT != typeof(UInt16)) CovariantSpanTest.Test<T, UInt16>(array);
				break;
			case 4:
				if (typeofT != typeof(Int32)) CovariantSpanTest.Test<T, Int32>(array);
				if (typeofT != typeof(UInt32)) CovariantSpanTest.Test<T, UInt32>(array);
				break;
			case 8:
				if (typeofT != typeof(Int64)) CovariantSpanTest.Test<T, Int64>(array);
				if (typeofT != typeof(UInt64)) CovariantSpanTest.Test<T, UInt64>(array);
				break;
		}
	}
	private static void Test<T, T2>(T[] array)
	{
		Span<T> span = array.AsCovariantSpan();
		ReadOnlySpan<T> readOnlySpan = array.AsReadOnlySpan();
		Span<T2> span2 = array is T2[] array1 ? array1.AsCovariantSpan() : default;
		ReadOnlySpan<T2> readOnlySpan2 = array is T2[] array2 ? array2.AsCovariantSpan() : default;

		PInvokeAssert.Equal(readOnlySpan.ToArray(), span.ToArray());
		PInvokeAssert.Equal(readOnlySpan2.ToArray(), span2.ToArray());

		ref Byte ref0 = ref Unsafe.As<T, Byte>(ref MemoryMarshal.GetReference(span));
		ref Byte ref1 = ref Unsafe.As<T, Byte>(ref MemoryMarshal.GetReference(readOnlySpan));
		ref Byte ref2 = ref Unsafe.As<T2, Byte>(ref MemoryMarshal.GetReference(span2));
		ref Byte ref3 = ref Unsafe.As<T2, Byte>(ref MemoryMarshal.GetReference(readOnlySpan2));

		Assert.True(Unsafe.AreSame(ref ref0, ref ref1));
		Assert.True(Unsafe.AreSame(ref ref1, ref ref2));
		Assert.True(Unsafe.AreSame(ref ref2, ref ref3));
	}
}