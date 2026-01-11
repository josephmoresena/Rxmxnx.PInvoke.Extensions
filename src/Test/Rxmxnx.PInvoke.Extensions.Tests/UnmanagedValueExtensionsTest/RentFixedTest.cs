namespace Rxmxnx.PInvoke.Tests.UnmanagedValueExtensionsTest;

[TestFixture]
[ExcludeFromCodeCoverage]
[SuppressMessage("csharpsquid", "S2699")]
public sealed class RentFixedTest
{
	private static readonly IFixture fixture = new Fixture();

	[Theory]
	[InlineData(10)]
	[InlineData(100)]
	[InlineData(1000)]
	public void ByteTest(Int32 size) => TestClass<Byte>.Test(size);
	[Theory]
	[InlineData(10)]
	[InlineData(100)]
	[InlineData(1000)]
	public void CharTest(Int32 size) => TestClass<Char>.Test(size);
#if NET7_0_OR_GREATER
	[Theory]
	[InlineData(10)]
	[InlineData(100)]
	[InlineData(1000)]
	internal void DateTimeTest(Int32 size) => TestClass<DateTime>.Test(size);
#endif
	[Theory]
	[InlineData(10)]
	[InlineData(100)]
	[InlineData(1000)]
	public void DecimalTest(Int32 size) => TestClass<Decimal>.Test(size);
	[Theory]
	[InlineData(10)]
	[InlineData(100)]
	[InlineData(1000)]
	public void DoubleTest(Int32 size) => TestClass<Double>.Test(size);
	[Theory]
	[InlineData(10)]
	[InlineData(100)]
	[InlineData(1000)]
	public void GuidTest(Int32 size) => TestClass<Guid>.Test(size);
#if NET5_0_OR_GREATER
	[Theory]
	[InlineData(10)]
	[InlineData(100)]
	[InlineData(1000)]
	internal void HalfTest(Int32 size) => TestClass<Half>.Test(size);
#endif
	[Theory]
	[InlineData(10)]
	[InlineData(100)]
	[InlineData(1000)]
	public void Int16Test(Int32 size) => TestClass<Int16>.Test(size);
	[Theory]
	[InlineData(10)]
	[InlineData(100)]
	[InlineData(1000)]
	public void Int32Test(Int32 size) => TestClass<Int32>.Test(size);
	[Theory]
	[InlineData(10)]
	[InlineData(100)]
	[InlineData(1000)]
	public void Int64Test(Int32 size) => TestClass<Int64>.Test(size);
	[Theory]
	[InlineData(10)]
	[InlineData(100)]
	[InlineData(1000)]
	public void SByteTest(Int32 size) => TestClass<SByte>.Test(size);
	[Theory]
	[InlineData(10)]
	[InlineData(100)]
	[InlineData(1000)]
	public void UInt16Test(Int32 size) => TestClass<UInt16>.Test(size);
	[Theory]
	[InlineData(10)]
	[InlineData(100)]
	[InlineData(1000)]
	public void UInt32Test(Int32 size) => TestClass<UInt32>.Test(size);
	[Theory]
	[InlineData(10)]
	[InlineData(100)]
	[InlineData(1000)]
	public void UInt64Test(Int32 size) => TestClass<UInt64>.Test(size);

	private static class TestClass<T> where T : unmanaged
	{
		private static readonly ArrayPool<T> pool = ArrayPool<T>.Create(1024 * Unsafe.SizeOf<T>(), 50);

		public static void Test(Int32 size)
		{
			T[] arr = TestClass<T>.pool.Rent(size);
			RentFixedTest.fixture.CreateMany<T>(arr.Length).ToArray().CopyTo(arr.AsSpan());
			TestClass<T>.pool.Return(arr); // Ensure this array is used.

			using IFixedContext<T>.IDisposable ctx = TestClass<T>.pool.RentFixed(size, true, out Int32 arrayLength);
			PInvokeAssert.Equal(arr.Length, arrayLength);
			PInvokeAssert.Equal(size, ctx.Values.Length);
			PInvokeAssert.True(Unsafe.AreSame(ref ctx.ValuePointer.Reference,
			                                  ref MemoryMarshal.GetReference(arr.AsSpan())));
#if NET6_0_OR_GREATER
			Assert.True(ctx.Values.SequenceEqual(arr.AsSpan()[..size]));
#else
			PInvokeAssert.Equal(ctx.Values.ToArray(), arr.AsSpan()[..size].ToArray());
#endif
		}
	}
}