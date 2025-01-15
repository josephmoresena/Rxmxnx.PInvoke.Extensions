namespace Rxmxnx.PInvoke.Tests.PointerExtensionsTests;

[ExcludeFromCodeCoverage]
[SuppressMessage("csharpsquid", "S2699")]
public sealed class GetUnsafeSpanTest
{
	private static readonly IFixture fixture = new Fixture();

	[Fact]
	internal void ByteTest() => GetUnsafeSpanTest.Test<Byte>();
	[Fact]
	internal void CharTest() => GetUnsafeSpanTest.Test<Char>();
	[Fact]
	internal void DateTimeTest() => GetUnsafeSpanTest.Test<DateTime>();
	[Fact]
	internal void DecimalTest() => GetUnsafeSpanTest.Test<Decimal>();
	[Fact]
	internal void DoubleTest() => GetUnsafeSpanTest.Test<Double>();
	[Fact]
	internal void GuidTest() => GetUnsafeSpanTest.Test<Guid>();
	[Fact]
	internal void HalfTest() => GetUnsafeSpanTest.Test<Half>();
	[Fact]
	internal void Int16Test() => GetUnsafeSpanTest.Test<Int16>();
	[Fact]
	internal void Int32Test() => GetUnsafeSpanTest.Test<Int32>();
	[Fact]
	internal void Int64Test() => GetUnsafeSpanTest.Test<Int64>();
	[Fact]
	internal void SByteTest() => GetUnsafeSpanTest.Test<SByte>();
	[Fact]
	internal void SingleTest() => GetUnsafeSpanTest.Test<Single>();
	[Fact]
	internal void UInt16Test() => GetUnsafeSpanTest.Test<UInt16>();
	[Fact]
	internal void UInt32Test() => GetUnsafeSpanTest.Test<UInt32>();
	[Fact]
	internal void UInt64Test() => GetUnsafeSpanTest.Test<UInt64>();

	private static unsafe void Test<T>() where T : unmanaged
	{
		GetUnsafeSpanTest.EmptyTest<T>(-1);
		GetUnsafeSpanTest.EmptyTest<T>(0);
		GetUnsafeSpanTest.EmptyTest<T>(1);

		T[] input = GetUnsafeSpanTest.fixture.CreateMany<T>(10).ToArray();

		MemoryHandle handle = input.AsMemory().Pin();
		fixed (void* p = &MemoryMarshal.GetReference(input.AsSpan()))
		{
			IntPtr intPtr = (IntPtr)p;
			UIntPtr uintPtr = (UIntPtr)p;

			Assert.Equal(input, intPtr.GetUnsafeSpan<T>(input.Length).ToArray());
			Assert.Equal(input, intPtr.GetUnsafeReadOnlySpan<T>(input.Length).ToArray());
			Assert.Equal(input, uintPtr.GetUnsafeSpan<T>(input.Length).ToArray());
			Assert.Equal(input, uintPtr.GetUnsafeReadOnlySpan<T>(input.Length).ToArray());
			Assert.Equal(input, handle.GetUnsafeSpan<T>(input.Length).ToArray());
			Assert.Equal(input, handle.GetUnsafeReadOnlySpan<T>(input.Length).ToArray());

			Assert.Equal(input, intPtr.GetUnsafeArray<T>(input.Length));
			Assert.Equal(input, uintPtr.GetUnsafeArray<T>(input.Length));
			Assert.Equal(input, handle.GetUnsafeArray<T>(input.Length));
			Assert.True(p == handle.Pointer);
			Assert.Equal(intPtr, handle.ToIntPtr());
			Assert.Equal(uintPtr, handle.ToUIntPtr());
		}

		GetUnsafeSpanTest.MemoryTest(input);
	}
	private static void EmptyTest<T>(Int32 length) where T : unmanaged
	{
		if (length >= 0)
		{
			Span<T> result = IntPtr.Zero.GetUnsafeSpan<T>(length);
			ReadOnlySpan<T> result2 = IntPtr.Zero.GetUnsafeReadOnlySpan<T>(length);
			Span<T> result3 = UIntPtr.Zero.GetUnsafeSpan<T>(length);
			ReadOnlySpan<T> result4 = UIntPtr.Zero.GetUnsafeReadOnlySpan<T>(length);
			Assert.True(result.IsEmpty);
			Assert.True(result2.IsEmpty);
			Assert.True(result3.IsEmpty);
			Assert.True(result4.IsEmpty);

			Assert.Null(IntPtr.Zero.GetUnsafeArray<T>(length));
			Assert.Null(UIntPtr.Zero.GetUnsafeArray<T>(length));
		}
		else
		{
			Assert.Throws<ArgumentException>(() => IntPtr.Zero.GetUnsafeSpan<T>(length));
			Assert.Throws<ArgumentException>(() => IntPtr.Zero.GetUnsafeReadOnlySpan<T>(length));
			Assert.Throws<ArgumentException>(() => UIntPtr.Zero.GetUnsafeSpan<T>(length));
			Assert.Throws<ArgumentException>(() => UIntPtr.Zero.GetUnsafeReadOnlySpan<T>(length));
		}
	}
	private static void MemoryTest<T>(T[] arr) where T : unmanaged
	{
		Memory<T> objMem = arr.AsMemory();
		using MemoryHandle handle = objMem.Pin();
		Span<T> span = handle.GetUnsafeSpan<T>(objMem.Length);
		ReadOnlySpan<T> readOnlySpan = handle.GetUnsafeReadOnlySpan<T>(objMem.Length);

		Assert.True(readOnlySpan.SequenceEqual(span));
		Assert.True(Unsafe.AreSame(ref MemoryMarshal.GetReference(span), ref MemoryMarshal.GetReference(arr.AsSpan())));

		for (Int32 i = 0; i < arr.Length; i++)
		{
			Assert.Equal(readOnlySpan[i], arr[i]);
			Assert.True(Unsafe.AreSame(ref span[i], ref arr[i]));
		}

		MemoryHandle defaultHandle = default;
		Span<T> defaultSpan = defaultHandle.GetUnsafeSpan<T>(objMem.Length);
		ReadOnlySpan<T> defaultReadOnlySpan = defaultHandle.GetUnsafeReadOnlySpan<T>(objMem.Length);

		Assert.True(defaultSpan.IsEmpty);
		Assert.True(defaultReadOnlySpan.IsEmpty);
	}
}