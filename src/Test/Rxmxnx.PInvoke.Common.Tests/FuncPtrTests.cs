#if !NETCOREAPP
using Fact = NUnit.Framework.TestAttribute;
#endif

namespace Rxmxnx.PInvoke.Tests;

[TestFixture]
[ExcludeFromCodeCoverage]
[SuppressMessage("csharpsquid", "S2699")]
public sealed class FuncPtrTests
{
#if NETCOREAPP
	private static readonly CultureInfo[] allCultures = CultureInfo.GetCultures(CultureTypes.AllCultures);
	private static readonly String[] formats =
	[
		"",
#if NET8_0_OR_GREATER
		"b", "B",
#endif
		"D", "d", "E", "e", "G", "g", "X", "x",
	];
	private static readonly IFixture fixture = new Fixture();
#endif

	[Fact]
	public void StringTest() => FuncPtrTests.TestDelegate<GetStringDelegate>(() => String.Empty);
#if NETCOREAPP
	[Fact]
	public void SpanTest() => FuncPtrTests.TestDelegate<GetByteSpanDelegate>(a => a.AsSpan());
	[Fact]
	public void GuidTest()
		=> FuncPtrTests.TestDelegate<GetGuidSpanDelegate>(() => FuncPtrTests.fixture.CreateMany<Guid>().ToArray());
#endif
	[Fact]
	public void VoidTest() => FuncPtrTests.TestDelegate<VoidDelegate>(() => Console.WriteLine(String.Empty));
	[Fact]
	public void VoidObjectTest() => FuncPtrTests.TestDelegate<VoidObjectDelegate>(Console.WriteLine);

	private static unsafe void TestDelegate<TDelegate>(TDelegate del) where TDelegate : Delegate
	{
		FuncPtr<TDelegate> empty = (FuncPtr<TDelegate>)IntPtr.Zero;
		FuncPtr<TDelegate> emptyPtr = (FuncPtr<TDelegate>)IntPtr.Zero.ToPointer();
		PInvokeAssert.Equal(empty, FuncPtr<TDelegate>.Zero);
		PInvokeAssert.Equal(empty.Pointer, FuncPtr<TDelegate>.Zero.Pointer);
		PInvokeAssert.Null(empty.Invoke);
		PInvokeAssert.Null(FuncPtr<TDelegate>.Zero.Invoke);
		PInvokeAssert.True(empty.IsZero);
		PInvokeAssert.True(FuncPtr<TDelegate>.Zero.IsZero);
		PInvokeAssert.True(emptyPtr == IntPtr.Zero.ToPointer());

		PInvokeAssert.True(FuncPtr<TDelegate>.Zero.Equals(empty));
		PInvokeAssert.True(FuncPtr<TDelegate>.Zero.Equals((Object)FuncPtr<TDelegate>.Zero));
		PInvokeAssert.False(FuncPtr<TDelegate>.Zero.Equals(null));
		PInvokeAssert.False(FuncPtr<TDelegate>.Zero.Equals(empty.Pointer));

		FuncPtrTests.FormatTest(FuncPtr<TDelegate>.Zero);
		FuncPtrTests.MarshallerTest(FuncPtr<TDelegate>.Zero);

		IntPtr ptr = Marshal.GetFunctionPointerForDelegate(del);
		FuncPtr<TDelegate> funcPtr = (FuncPtr<TDelegate>)ptr;
		PInvokeAssert.NotNull(funcPtr.Invoke);
		IntPtr ptr2 = funcPtr;
		void* ptr3 = funcPtr;

		PInvokeAssert.Equal(ptr, ptr2);
		PInvokeAssert.True(ptr.ToPointer() == ptr3);
		PInvokeAssert.True(empty == FuncPtr<TDelegate>.Zero);
		PInvokeAssert.False(funcPtr == FuncPtr<TDelegate>.Zero);
		PInvokeAssert.True(empty != funcPtr);
		PInvokeAssert.False(funcPtr != (FuncPtr<TDelegate>)funcPtr.Pointer);
		PInvokeAssert.False(funcPtr != (FuncPtr<TDelegate>)funcPtr.Pointer.ToPointer());
		PInvokeAssert.Equal(funcPtr.Pointer, (funcPtr as IWrapper<IntPtr>).Value);

		FuncPtrTests.FormatTest(funcPtr);
		FuncPtrTests.MarshallerTest(funcPtr);
	}

	private static void FormatTest<TDelegate>(FuncPtr<TDelegate> funcPtr) where TDelegate : Delegate
	{
		PInvokeAssert.Equal(funcPtr.Pointer.GetHashCode(), funcPtr.GetHashCode());
		PInvokeAssert.Equal(funcPtr.Pointer.ToString(), funcPtr.ToString());
#if NET6_0_OR_GREATER
		if ((Object)funcPtr is not ISpanFormattable spanFormattable) return;

		CultureInfo culture = FuncPtrTests.allCultures[Random.Shared.Next(0, FuncPtrTests.allCultures.Length)];
		MethodInfo? toStringMethodInfo = spanFormattable.GetType()
		                                                .GetMethod(nameof(IntPtr.ToString),
		                                                           BindingFlags.Public | BindingFlags.Instance, null,
		                                                           [typeof(IFormatProvider),], null);
		if (toStringMethodInfo is not null)
			Assert.Equal(funcPtr.Pointer.ToString(culture), toStringMethodInfo.Invoke(funcPtr, [culture,]));

		Span<Char> span1 = stackalloc Char[20];
		Span<Char> span2 = stackalloc Char[20];
		Boolean res1 = funcPtr.Pointer.TryFormat(span1, out Int32 pC, "X", culture);
		Boolean res2 = spanFormattable.TryFormat(span2, out Int32 vC, "X", culture);

		Assert.Equal(res1, res2);
		Assert.Equal(pC, vC);
		Assert.True(span1.SequenceEqual(span2));

		foreach (String format in FuncPtrTests.formats)
		{
			culture = FuncPtrTests.allCultures[Random.Shared.Next(0, FuncPtrTests.allCultures.Length)];
			Assert.Equal(funcPtr.Pointer.ToString(format), funcPtr.ToString(format));
			Assert.Equal(funcPtr.Pointer.ToString(format, culture), spanFormattable.ToString(format, culture));
		}
#endif
	}
	private static void MarshallerTest<TDelegate>(FuncPtr<TDelegate> funcPtr) where TDelegate : Delegate
	{
		IntPtr value = FuncPtr<TDelegate>.Marshaller.ConvertToUnmanaged(funcPtr);
		FuncPtr<TDelegate> ptr = FuncPtr<TDelegate>.Marshaller.ConvertToManaged(value);

		PInvokeAssert.Equal(value, funcPtr.Pointer);
		PInvokeAssert.Equal(funcPtr, ptr);
	}
#if NETCOREAPP && !NET6_0_OR_GREATER
	private static class Random
	{
		public static readonly System.Random Shared = new();
	}
#endif
}