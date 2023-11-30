namespace Rxmxnx.PInvoke.Tests;

[ExcludeFromCodeCoverage]
[SuppressMessage("csharpsquid", "S2699")]
public sealed class FuncPtrTests
{
	private static readonly CultureInfo[] allCultures = CultureInfo.GetCultures(CultureTypes.AllCultures);
	private static readonly String[] formats = { "", "b", "B", "D", "d", "E", "e", "G", "g", "X", "x", };
	private static readonly IFixture fixture = new Fixture();

	[Fact]
	internal void StringTest() => FuncPtrTests.TestDelegate<GetStringDelegate>(() => String.Empty);
	[Fact]
	internal void SpanTest() => FuncPtrTests.TestDelegate<GetByteSpanDelegate>(a => a.AsSpan());
	[Fact]
	internal void GuidTest()
		=> FuncPtrTests.TestDelegate<GetGuidSpanDelegate>(() => FuncPtrTests.fixture.CreateMany<Guid>().ToArray());
	[Fact]
	internal void VoidTest() => FuncPtrTests.TestDelegate<VoidDelegate>(() => Console.WriteLine(String.Empty));
	[Fact]
	internal void VoidObjectTest() => FuncPtrTests.TestDelegate<VoidObjectDelegate>(o => Console.WriteLine(o));

	private static void TestDelegate<TDelegate>(TDelegate del) where TDelegate : Delegate
	{
		FuncPtr<TDelegate> empty = (FuncPtr<TDelegate>)IntPtr.Zero;
		Assert.Equal(empty, FuncPtr<TDelegate>.Zero);
		Assert.Equal(empty.Pointer, FuncPtr<TDelegate>.Zero.Pointer);
		Assert.Throws<NullReferenceException>(() => empty.Invoke);
		Assert.Throws<NullReferenceException>(() => FuncPtr<TDelegate>.Zero.Invoke);
		Assert.True(empty.IsZero);
		Assert.True(FuncPtr<TDelegate>.Zero.IsZero);

		Assert.True(FuncPtr<TDelegate>.Zero.Equals(empty));
		Assert.True(FuncPtr<TDelegate>.Zero.Equals((Object)FuncPtr<TDelegate>.Zero));
		Assert.False(FuncPtr<TDelegate>.Zero.Equals(null));
		Assert.False(FuncPtr<TDelegate>.Zero.Equals(empty.Pointer));

		FuncPtrTests.FormatTest(FuncPtr<TDelegate>.Zero);

		IntPtr ptr = Marshal.GetFunctionPointerForDelegate(del);
		FuncPtr<TDelegate> funcPtr = (FuncPtr<TDelegate>)ptr;
		Assert.NotNull(funcPtr.Invoke);
		IntPtr ptr2 = funcPtr;

		Assert.Equal(ptr, ptr2);
		Assert.True(empty == FuncPtr<TDelegate>.Zero);
		Assert.False(funcPtr == FuncPtr<TDelegate>.Zero);
		Assert.True(empty != funcPtr);
		Assert.False(funcPtr != (FuncPtr<TDelegate>)funcPtr.Pointer);

		FuncPtrTests.FormatTest(funcPtr);
	}

	private static void FormatTest<TDelegate>(FuncPtr<TDelegate> funcPtr) where TDelegate : Delegate
	{
		CultureInfo culture = FuncPtrTests.allCultures[Random.Shared.Next(0, FuncPtrTests.allCultures.Length)];
		Assert.Equal(funcPtr.Pointer.GetHashCode(), funcPtr.GetHashCode());
		Assert.Equal(funcPtr.Pointer.ToString(), funcPtr.ToString());
		Assert.Equal(funcPtr.Pointer.ToString(culture), funcPtr.ToString(culture));

		Span<Char> span1 = stackalloc Char[20];
		Span<Char> span2 = stackalloc Char[20];
		Boolean res1 = funcPtr.Pointer.TryFormat(span1, out Int32 pC, "X", culture);
		Boolean res2 = funcPtr.TryFormat(span2, out Int32 vC, "X", culture);

		Assert.Equal(res1, res2);
		Assert.Equal(pC, vC);
		Assert.True(span1.SequenceEqual(span2));

		foreach (String format in FuncPtrTests.formats)
		{
			culture = FuncPtrTests.allCultures[Random.Shared.Next(0, FuncPtrTests.allCultures.Length)];
			Assert.Equal(funcPtr.Pointer.ToString(format), funcPtr.ToString(format));
			Assert.Equal(funcPtr.Pointer.ToString(format, culture), funcPtr.ToString(format, culture));
		}
	}
}