namespace Rxmxnx.PInvoke.Tests;

[ExcludeFromCodeCoverage]
[SuppressMessage("csharpsquid", "S2699")]
public sealed class ReadOnlyValPtrTests
{
	private static readonly CultureInfo[] allCultures = CultureInfo.GetCultures(CultureTypes.AllCultures);
	private static readonly String[] formats = {"", "b", "B", "D", "d", "E", "e", "G", "g", "X", "x", };
	private static readonly IFixture fixture = new Fixture();
	
	[Fact]
	internal void BooleanTest() => ReadOnlyValPtrTests.Test<Boolean>();
	[Fact]
	internal void ByteTest() => ReadOnlyValPtrTests.Test<Byte>();
	[Fact]
	internal void Int16Test() => ReadOnlyValPtrTests.Test<Int16>();
	[Fact]
	internal void CharTest() => ReadOnlyValPtrTests.Test<Char>();
	[Fact]
	internal void Int32Test() => ReadOnlyValPtrTests.Test<Int32>();
	[Fact]
	internal void Int64Test() => ReadOnlyValPtrTests.Test<Int64>();
	[Fact]
	internal void Int128Test() => ReadOnlyValPtrTests.Test<Int128>();
	[Fact]
	internal void GuidTest() => ReadOnlyValPtrTests.Test<Guid>();
	[Fact]
	internal void SingleTest() => ReadOnlyValPtrTests.Test<Single>();
	[Fact]
	internal void HalfTest() => ReadOnlyValPtrTests.Test<Half>();
	[Fact]
	internal void DoubleTest() => ReadOnlyValPtrTests.Test<Double>();
	[Fact]
	internal void DecimalTest() => ReadOnlyValPtrTests.Test<Decimal>();
	[Fact]
	internal void DateTimeTest() => ReadOnlyValPtrTests.Test<DateTime>();
	[Fact]
	internal void TimeOnlyTest() => ReadOnlyValPtrTests.Test<TimeOnly>();
	[Fact]
	internal void TimeSpanTest() => ReadOnlyValPtrTests.Test<TimeSpan>();

	private static unsafe void Test<T>() where T : unmanaged
	{
		T[] arr = ReadOnlyValPtrTests.fixture.CreateMany<T>(10).ToArray();
		Span<T> span = arr;
		fixed (void* ptr = &MemoryMarshal.GetReference(span))
			ReadOnlyValPtrTests.Test((ReadOnlyValPtr<T>)new IntPtr(ptr), span);
	}
	private static unsafe void Test<T>(ReadOnlyValPtr<T> valPtr, ReadOnlySpan<T> span) where T : unmanaged
	{
		ReadOnlyValPtr<T> empty = (ReadOnlyValPtr<T>)IntPtr.Zero;
		Assert.Equal(empty, ReadOnlyValPtr<T>.Zero);
		Assert.Equal(empty.Pointer, ReadOnlyValPtr<T>.Zero.Pointer);
		Assert.Throws<NullReferenceException>(() => empty.Reference);
		Assert.Throws<NullReferenceException>(() => ReadOnlyValPtr<T>.Zero.Reference);
		Assert.True(empty.IsZero);
		Assert.True(ReadOnlyValPtr<T>.Zero.IsZero);
		
		Assert.True(ReadOnlyValPtr<T>.Zero.Equals(empty));
		Assert.True(ReadOnlyValPtr<T>.Zero.Equals((Object)ValPtr<T>.Zero));
		Assert.False(ReadOnlyValPtr<T>.Zero.Equals(null));
		Assert.False(ReadOnlyValPtr<T>.Zero.Equals(empty.Pointer));
		
		Assert.Equal(1, valPtr.CompareTo(null));
		Assert.Throws<ArgumentException>(() => valPtr.CompareTo(valPtr.Pointer));
		
		ReadOnlyValPtrTests.FormatTest(ReadOnlyValPtr<T>.Zero);

		ReadOnlyValPtr<T> ptrI;
		for (Int32 i = 0; i < span.Length; i++)
		{ 
			Int32 binaryOffset = sizeof(T) * i;
			ReadOnlyValPtr<T> ptrIAdd = ReadOnlyValPtr<T>.Add(valPtr, i);
			ptrI = valPtr + i;
			Assert.Equal(ptrI, ptrIAdd);
			Assert.Equal(valPtr, ReadOnlyValPtr<T>.Subtract(ptrI, i));
			Assert.True(ptrI == valPtr.Pointer + binaryOffset);
			Assert.True(Unsafe.AreSame(ref UnsafeLegacy.AsRef(in ptrI.Reference), ref UnsafeLegacy.AsRef(in span[i])));
			Assert.Equal(valPtr.Pointer, ptrI.Pointer - binaryOffset);
			Assert.False(ptrI.IsZero);
			
			ReadOnlyValPtrTests.ReferenceTest(ptrI, ref UnsafeLegacy.AsRef(in span[i]));

			if (i <= 0) continue;

			ValPtr<T> ptrIAdd2 = Unsafe.As<ReadOnlyValPtr<T>, ValPtr<T>>(ref UnsafeLegacy.AsRef(in ptrIAdd));
			
			Assert.Equal(1, ptrI.CompareTo(valPtr));
			Assert.Equal(0, ptrI.CompareTo(ptrIAdd));
			Assert.Equal(0, valPtr.CompareTo(ptrI - i));
				
			Assert.Equal(1, ptrI.CompareTo((Object)valPtr));
			Assert.Equal(0, ptrI.CompareTo((Object)ptrIAdd2));
			Assert.Equal(1, ptrI.CompareTo(null));
			Assert.Throws<ArgumentException>(() => ptrI.CompareTo(ptrI.Pointer));
			
			Assert.False(ptrI.Equals((Object)valPtr));
			Assert.True(ptrI.Equals((Object)ptrIAdd2));
			Assert.False(ptrI.Equals(null));
			Assert.False(ptrI.Equals(ptrI.Pointer));
			
			ReadOnlyValPtrTests.FormatTest(ptrI);
		}

		if (span.Length <= 0) return;
		ReadOnlyValPtr<T> incValue = valPtr;
		Assert.True(valPtr == incValue);
		incValue++;
		Assert.True(valPtr.Pointer + sizeof(T) == incValue);
		Assert.True(valPtr != incValue);
		incValue--;
		Assert.Equal(valPtr.Pointer, incValue);
		Assert.False(valPtr != incValue);

		ReadOnlyValPtrTests.ContextTest(valPtr, span);
	}
	private static unsafe void ContextTest<T>(ReadOnlyValPtr<T> valPtr, ReadOnlySpan<T> span) where T : unmanaged
	{
		using IReadOnlyFixedContext<T>.IDisposable ctx = valPtr.GetUnsafeFixedContext(span.Length);
		Assert.Equal(ctx.Values.Length, span.Length);
		Assert.Equal(valPtr.Pointer, ctx.Pointer);
		Assert.True(ctx.AsBinaryContext().Values.SequenceEqual(new(valPtr.Pointer.ToPointer(), span.Length * sizeof(T))));

		ReadOnlySpan<T> span2 = ctx.Values;
		for (Int32 i = 0; i < span.Length; i++)
			Assert.True(Unsafe.AreSame(ref UnsafeLegacy.AsRef(in span[i]), ref UnsafeLegacy.AsRef(in span2[i])));
		
		ReadOnlyValPtrTests.ContextTransformTest<T, Byte>(ctx);
		ReadOnlyValPtrTests.ContextTransformTest<T, Int16>(ctx);
		ReadOnlyValPtrTests.ContextTransformTest<T, Int32>(ctx);
		ReadOnlyValPtrTests.ContextTransformTest<T, Int64>(ctx);
	}
	private static unsafe void ReferenceTest<T>(ReadOnlyValPtr<T> ptrI, ref T reference) where T : unmanaged
	{
		using IReadOnlyFixedReference<T>.IDisposable fixedReference = ptrI.GetUnsafeFixedReference();
		Assert.True(Unsafe.AreSame(ref UnsafeLegacy.AsRef(in fixedReference.Reference), ref reference));
		Assert.Equal(ptrI.Pointer, fixedReference.Pointer);
		Assert.True(fixedReference.Bytes.SequenceEqual(new(ptrI.Pointer.ToPointer(), sizeof(T))));
		Assert.True(fixedReference.AsBinaryContext().Values.SequenceEqual(new(ptrI.Pointer.ToPointer(), sizeof(T))));

		ReadOnlyValPtrTests.ReferenceTransformTest<T, Byte>(ptrI, fixedReference);
		ReadOnlyValPtrTests.ReferenceTransformTest<T, Int16>(ptrI, fixedReference);
		ReadOnlyValPtrTests.ReferenceTransformTest<T, Int32>(ptrI, fixedReference);
		ReadOnlyValPtrTests.ReferenceTransformTest<T, Int64>(ptrI, fixedReference);
	}
	private static void FormatTest<T>(ReadOnlyValPtr<T> valPtr) where T : unmanaged
	{
		CultureInfo culture = ReadOnlyValPtrTests.allCultures[Random.Shared.Next(0, ReadOnlyValPtrTests.allCultures.Length)];
		Assert.Equal(valPtr.Pointer.GetHashCode(), valPtr.GetHashCode());
		Assert.Equal(valPtr.Pointer.ToString(), valPtr.ToString());
		Assert.Equal(valPtr.Pointer.ToString(culture), valPtr.ToString(culture));

		Span<Char> span1 = stackalloc Char[20];
		Span<Char> span2 = stackalloc Char[20];
		Boolean res1 = valPtr.Pointer.TryFormat(span1, out Int32 pC, "X", culture);
		Boolean res2 = valPtr.TryFormat(span2, out Int32 vC, "X", culture);
		
		Assert.Equal(res1, res2);
		Assert.Equal(pC, vC);
		Assert.True(span1.SequenceEqual(span2));
		
		foreach (String format in ReadOnlyValPtrTests.formats)
		{
			culture = ReadOnlyValPtrTests.allCultures[Random.Shared.Next(0, ReadOnlyValPtrTests.allCultures.Length)];
			Assert.Equal(valPtr.Pointer.ToString(format), valPtr.ToString(format));
			Assert.Equal(valPtr.Pointer.ToString(format, culture), valPtr.ToString(format, culture));
		}
	}
	private static unsafe void ReferenceTransformTest<T, TDestination>(ReadOnlyValPtr<T> ptrI, IReadOnlyFixedReference<T>.IDisposable fRef) 
		where T : unmanaged
		where TDestination : unmanaged
	{
		if (sizeof(TDestination) > sizeof(T)) return;
		IReadOnlyReferenceable<TDestination> fRef2 = fRef.Transformation<TDestination>(out IReadOnlyFixedMemory offset);
		Assert.True(Unsafe.AreSame(ref UnsafeLegacy.AsRef(in fRef2.Reference),
		                           ref Unsafe.AsRef<TDestination>(ptrI.Pointer.ToPointer())));
		Assert.Equal(sizeof(T) - sizeof(TDestination), offset.Bytes.Length);
	}
	private static unsafe void ContextTransformTest<T, TDestination>(IReadOnlyFixedContext<T>.IDisposable ctx) where T: unmanaged where TDestination : unmanaged
	{
		IReadOnlyFixedContext<TDestination> ctx2 = ctx.Transformation<TDestination>(out IReadOnlyFixedMemory offset);
		Assert.Equal(ctx2.Values.Length, ctx.Bytes.Length / sizeof(TDestination));
		Assert.Equal(offset.Bytes.Length, ctx.Bytes.Length - ctx2.Values.Length * sizeof(TDestination));
	}
}