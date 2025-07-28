#if !NETCOREAPP
using Fact = NUnit.Framework.TestAttribute;
#endif

namespace Rxmxnx.PInvoke.Tests.PointerExtensionsTests;

[TestFixture]
[ExcludeFromCodeCoverage]
[SuppressMessage("csharpsquid", "S2699")]
public sealed class GetUnsafeSpanTest
{
	private static readonly IFixture fixture = new Fixture();

	[Fact]
	public void ByteTest() => GetUnsafeSpanTest.Test<Byte>();
	[Fact]
	public void CharTest() => GetUnsafeSpanTest.Test<Char>();
#if NET7_0_OR_GREATER
	[Fact]
	public void DateTimeTest() => GetUnsafeSpanTest.Test<DateTime>();
#endif
	[Fact]
	public void DecimalTest() => GetUnsafeSpanTest.Test<Decimal>();
	[Fact]
	public void DoubleTest() => GetUnsafeSpanTest.Test<Double>();
	[Fact]
	public void GuidTest() => GetUnsafeSpanTest.Test<Guid>();
#if NET5_0_OR_GREATER
	[Fact]
	internal void HalfTest() => GetUnsafeSpanTest.Test<Half>();
#endif
	[Fact]
	public void Int16Test() => GetUnsafeSpanTest.Test<Int16>();
	[Fact]
	public void Int32Test() => GetUnsafeSpanTest.Test<Int32>();
	[Fact]
	public void Int64Test() => GetUnsafeSpanTest.Test<Int64>();
	[Fact]
	public void SByteTest() => GetUnsafeSpanTest.Test<SByte>();
	[Fact]
	public void SingleTest() => GetUnsafeSpanTest.Test<Single>();
	[Fact]
	public void UInt16Test() => GetUnsafeSpanTest.Test<UInt16>();
	[Fact]
	public void UInt32Test() => GetUnsafeSpanTest.Test<UInt32>();
	[Fact]
	public void UInt64Test() => GetUnsafeSpanTest.Test<UInt64>();

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

			PInvokeAssert.Equal(input, intPtr.GetUnsafeSpan<T>(input.Length).ToArray());
			PInvokeAssert.Equal(input, intPtr.GetUnsafeReadOnlySpan<T>(input.Length).ToArray());
			PInvokeAssert.Equal(input, uintPtr.GetUnsafeSpan<T>(input.Length).ToArray());
			PInvokeAssert.Equal(input, uintPtr.GetUnsafeReadOnlySpan<T>(input.Length).ToArray());
			PInvokeAssert.Equal(input, handle.GetUnsafeSpan<T>(input.Length).ToArray());
			PInvokeAssert.Equal(input, handle.GetUnsafeReadOnlySpan<T>(input.Length).ToArray());

			PInvokeAssert.Equal(input, intPtr.GetUnsafeArray<T>(input.Length));
			PInvokeAssert.Equal(input, uintPtr.GetUnsafeArray<T>(input.Length));
			PInvokeAssert.Equal(input, handle.GetUnsafeArray<T>(input.Length));
			PInvokeAssert.True(p == handle.Pointer);
			PInvokeAssert.Equal(intPtr, handle.ToIntPtr());
			PInvokeAssert.Equal(uintPtr, handle.ToUIntPtr());
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
			PInvokeAssert.True(result.IsEmpty);
			PInvokeAssert.True(result2.IsEmpty);
			PInvokeAssert.True(result3.IsEmpty);
			PInvokeAssert.True(result4.IsEmpty);

			PInvokeAssert.Null(IntPtr.Zero.GetUnsafeArray<T>(length));
			PInvokeAssert.Null(UIntPtr.Zero.GetUnsafeArray<T>(length));
		}
		else
		{
			PInvokeAssert.Throws<ArgumentException>(() => IntPtr.Zero.GetUnsafeSpan<T>(length));
			PInvokeAssert.Throws<ArgumentException>(() => IntPtr.Zero.GetUnsafeReadOnlySpan<T>(length));
			PInvokeAssert.Throws<ArgumentException>(() => UIntPtr.Zero.GetUnsafeSpan<T>(length));
			PInvokeAssert.Throws<ArgumentException>(() => UIntPtr.Zero.GetUnsafeReadOnlySpan<T>(length));
		}
	}
	private static void MemoryTest<T>(T[] arr) where T : unmanaged
	{
		Memory<T> objMem = arr.AsMemory();
		using MemoryHandle handle = objMem.Pin();
		Span<T> span = handle.GetUnsafeSpan<T>(objMem.Length);
		ReadOnlySpan<T> readOnlySpan = handle.GetUnsafeReadOnlySpan<T>(objMem.Length);

#if NET6_0_OR_GREATER
		Assert.True(readOnlySpan.SequenceEqual(span));
#else
		PInvokeAssert.Equal(readOnlySpan.ToArray(), span.ToArray());
#endif
		PInvokeAssert.True(Unsafe.AreSame(ref MemoryMarshal.GetReference(span),
		                                  ref MemoryMarshal.GetReference(arr.AsSpan())));

		for (Int32 i = 0; i < arr.Length; i++)
		{
			PInvokeAssert.Equal(readOnlySpan[i], arr[i]);
			PInvokeAssert.True(Unsafe.AreSame(ref span[i], ref arr[i]));
		}

		MemoryHandle defaultHandle = default;
		Span<T> defaultSpan = defaultHandle.GetUnsafeSpan<T>(objMem.Length);
		ReadOnlySpan<T> defaultReadOnlySpan = defaultHandle.GetUnsafeReadOnlySpan<T>(objMem.Length);

		PInvokeAssert.True(defaultSpan.IsEmpty);
		PInvokeAssert.True(defaultReadOnlySpan.IsEmpty);
	}
}