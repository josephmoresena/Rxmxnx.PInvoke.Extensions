#if !NETCOREAPP
using Fact = NUnit.Framework.TestAttribute;
#endif

namespace Rxmxnx.PInvoke.Tests;

[TestFixture]
[ExcludeFromCodeCoverage]
[SuppressMessage("csharpsquid", "S2699")]
#pragma warning disable CS8500
public sealed class ReadOnlyValPtrTests
{
	private static readonly CultureInfo[] allCultures = CultureInfo.GetCultures(CultureTypes.AllCultures);
	private static readonly String[] formats =
	[
		"",
#if NET8_0_OR_GREATER
		"b", "B",
#endif
		"D", "d", "E", "e", "G", "g", "X", "x",
	];
	private static readonly IFixture fixture = ManagedStruct.Register(new Fixture());

	[Fact]
	public void BooleanTest() => ReadOnlyValPtrTests.Test<Boolean>();
	[Fact]
	public void ByteTest() => ReadOnlyValPtrTests.Test<Byte>();
	[Fact]
	public void Int16Test() => ReadOnlyValPtrTests.Test<Int16>();
	[Fact]
	public void CharTest() => ReadOnlyValPtrTests.Test<Char>();
	[Fact]
	public void Int32Test() => ReadOnlyValPtrTests.Test<Int32>();
	[Fact]
	public void Int64Test() => ReadOnlyValPtrTests.Test<Int64>();
	[Fact]
	public void SingleTest() => ReadOnlyValPtrTests.Test<Single>();
	[Fact]
	public void DoubleTest() => ReadOnlyValPtrTests.Test<Double>();
#if NET7_0_OR_GREATER
	[Fact]
	internal void Int128Test() => ReadOnlyValPtrTests.Test<Int128>();
	[Fact]
	internal void GuidTest() => ReadOnlyValPtrTests.Test<Guid>();
	[Fact]
	internal void HalfTest() => ReadOnlyValPtrTests.Test<Half>();
	[Fact]
	internal void DecimalTest() => ReadOnlyValPtrTests.Test<Decimal>();
	[Fact]
	internal void DateTimeTest() => ReadOnlyValPtrTests.Test<DateTime>();
	[Fact]
	internal void TimeOnlyTest() => ReadOnlyValPtrTests.Test<TimeOnly>();
	[Fact]
	internal void TimeSpanTest() => ReadOnlyValPtrTests.Test<TimeSpan>();
	[Fact]
	internal void ManagedStructTest() => ReadOnlyValPtrTests.Test<ManagedStruct>();
#endif
	[Fact]
	public void StringTest() => ReadOnlyValPtrTests.Test<String>();

	private static unsafe void Test<T>()
	{
		T[] arr = ReadOnlyValPtrTests.fixture.CreateMany<T>(10).ToArray();
#if NETCOREAPP
		try
		{
			GCHandle.Alloc(arr, GCHandleType.Pinned).Free();
			Assert.True(ReadOnlyValPtr<T>.IsUnmanaged);
		}
		catch (ArgumentException)
		{
			Assert.False(ReadOnlyValPtr<T>.IsUnmanaged);
		}
#endif
		Span<T> span = arr;
		fixed (void* ptr = &MemoryMarshal.GetReference(span))
			ReadOnlyValPtrTests.Test((ReadOnlyValPtr<T>)new IntPtr(ptr), span);
	}
	private static unsafe void Test<T>(ReadOnlyValPtr<T> valPtr, ReadOnlySpan<T> span)
	{
		ReadOnlyValPtr<T> empty = (ReadOnlyValPtr<T>)IntPtr.Zero;
		ReadOnlyValPtr<T> emptyPtr = (ReadOnlyValPtr<T>)IntPtr.Zero.ToPointer();
		ReadOnlyValPtr<T> emptyPtr2 = (T*)IntPtr.Zero.ToPointer();
		PInvokeAssert.Equal(empty, ReadOnlyValPtr<T>.Zero);
		PInvokeAssert.Equal(empty.Pointer, ReadOnlyValPtr<T>.Zero.Pointer);
		PInvokeAssert.Throws<NullReferenceException>(() => empty.Reference);
		PInvokeAssert.Throws<NullReferenceException>(() => ReadOnlyValPtr<T>.Zero.Reference);
		PInvokeAssert.True(empty.IsZero);
		PInvokeAssert.True(ReadOnlyValPtr<T>.Zero.IsZero);
		PInvokeAssert.True(emptyPtr == IntPtr.Zero.ToPointer());
		PInvokeAssert.True(emptyPtr2 == IntPtr.Zero.ToPointer());

		PInvokeAssert.True(ReadOnlyValPtr<T>.Zero.Equals(empty));
		PInvokeAssert.True(ReadOnlyValPtr<T>.Zero.Equals((Object)ValPtr<T>.Zero));
		PInvokeAssert.False(ReadOnlyValPtr<T>.Zero.Equals((Object?)null));
		PInvokeAssert.False(ReadOnlyValPtr<T>.Zero.Equals(empty.Pointer));

		PInvokeAssert.Equal(1, valPtr.CompareTo(null));
		PInvokeAssert.Throws<ArgumentException>(() => valPtr.CompareTo(valPtr.Pointer));

		ReadOnlyValPtrTests.FormatTest(ReadOnlyValPtr<T>.Zero);
		ReadOnlyValPtrTests.MarshallerTest(ReadOnlyValPtr<T>.Zero);

		ReadOnlyValPtr<T> ptrI;
		for (Int32 i = 0; i < span.Length; i++)
		{
			Int32 binaryOffset = sizeof(T) * i;
			ReadOnlyValPtr<T> ptrIAdd = ReadOnlyValPtr<T>.Add(valPtr, i);
			ptrI = valPtr + i;
			PInvokeAssert.Equal(ptrI, ptrIAdd);
			PInvokeAssert.Equal(valPtr, ReadOnlyValPtr<T>.Subtract(ptrI, i));
			PInvokeAssert.True(ptrI == valPtr.Pointer + binaryOffset);
#if NET8_0_OR_GREATER
			Assert.True(Unsafe.AreSame(in ptrI.Reference, in span[i]));
#else
			PInvokeAssert.True(Unsafe.AreSame(ref Unsafe.AsRef(in ptrI.Reference), ref Unsafe.AsRef(in span[i])));
#endif
			PInvokeAssert.Equal(valPtr.Pointer, ptrI.Pointer - binaryOffset);
			PInvokeAssert.False(ptrI.IsZero);
			PInvokeAssert.Equal(ptrI.Pointer, (ptrI as IWrapper<IntPtr>).Value);

			ReadOnlyValPtrTests.ReferenceTest(ptrI, ref Unsafe.AsRef(in span[i]));

			PInvokeAssert.True(ptrI >= valPtr);
			PInvokeAssert.True(valPtr <= ptrI);

			if (i <= 0) continue;
			PInvokeAssert.True(ptrI > valPtr);
			PInvokeAssert.True(valPtr < ptrI);

			ValPtr<T> ptrIAdd2 = Unsafe.As<ReadOnlyValPtr<T>, ValPtr<T>>(ref Unsafe.AsRef(in ptrIAdd));

			PInvokeAssert.Equal(1, ptrI.CompareTo(valPtr));
			PInvokeAssert.Equal(0, ptrI.CompareTo(ptrIAdd));
			PInvokeAssert.Equal(0, valPtr.CompareTo(ptrI - i));

			PInvokeAssert.Equal(1, ptrI.CompareTo((Object)valPtr));
			PInvokeAssert.Equal(0, ptrI.CompareTo((Object)ptrIAdd2));
			PInvokeAssert.Equal(1, ptrI.CompareTo(null));
			PInvokeAssert.Throws<ArgumentException>(() => ptrI.CompareTo(ptrI.Pointer));

			PInvokeAssert.False(ptrI.Equals((Object)valPtr));
			PInvokeAssert.True(ptrI.Equals((Object)ptrIAdd2));
			PInvokeAssert.False(ptrI.Equals((Object?)null));
			PInvokeAssert.False(ptrI.Equals(ptrI.Pointer));

			ReadOnlyValPtrTests.FormatTest(ptrI);

			void* ptrI2 = ptrI;
			T* ptrI3 = ptrI;

			PInvokeAssert.True(ptrI2 == ptrI3);
		}

		if (span.Length <= 0) return;
		ReadOnlyValPtr<T> incValue = valPtr;
		PInvokeAssert.True(valPtr == incValue);
		incValue++;
		PInvokeAssert.True(valPtr.Pointer + sizeof(T) == incValue);
		PInvokeAssert.True(valPtr != incValue);
		incValue--;
		PInvokeAssert.Equal(valPtr.Pointer, incValue);
		PInvokeAssert.False(valPtr != incValue);

		ReadOnlyValPtrTests.ContextTest(valPtr, span);
		ReadOnlyValPtrTests.MarshallerTest(valPtr);
	}
	private static unsafe void ContextTest<T>(ReadOnlyValPtr<T> valPtr, ReadOnlySpan<T> span)
	{
		using IReadOnlyFixedContext<T>.IDisposable ctx = valPtr.GetUnsafeFixedContext(span.Length);
		PInvokeAssert.Equal(ctx.Values.Length, span.Length);
		PInvokeAssert.Equal(valPtr.Pointer, ctx.Pointer);
		if (!ReadOnlyValPtr<T>.IsUnmanaged)
		{
			PInvokeAssert.Throws<InvalidOperationException>(ctx.AsBinaryContext);
			PInvokeAssert.Throws<InvalidOperationException>(() => ctx.Transformation<Byte>(out _));

			if (typeof(T).IsValueType)
			{
				PInvokeAssert.Throws<InvalidOperationException>(ctx.AsObjectContext);
				PInvokeAssert.True(ctx.Objects.IsEmpty);
			}
			else
			{
				ReadOnlySpan<T>.Enumerator enumerator = span.GetEnumerator();
				foreach (ref readonly Object refObj in ctx.AsObjectContext().Values)
				{
					if (!enumerator.MoveNext()) break;
#if NET8_0_OR_GREATER
					Assert.True(Unsafe.AreSame(in enumerator.Current,
#else
					PInvokeAssert.True(Unsafe.AreSame(ref Unsafe.AsRef(in enumerator.Current),
#endif
					                           ref Unsafe.As<Object, T>(ref Unsafe.AsRef(in refObj))));
				}
				PInvokeAssert.Equal(typeof(T).IsValueType || ctx.IsNullOrEmpty, ctx.Objects.IsEmpty);
			}

			return;
		}
		PInvokeAssert.True(ctx.AsBinaryContext().Values
		                      .SequenceEqual(new(valPtr.Pointer.ToPointer(), span.Length * sizeof(T))));
		PInvokeAssert.Throws<InvalidOperationException>(ctx.AsObjectContext);

		ReadOnlySpan<T> span2 = ctx.Values;
		for (Int32 i = 0; i < span.Length; i++)
			PInvokeAssert.True(Unsafe.AreSame(ref Unsafe.AsRef(in span[i]), ref Unsafe.AsRef(in span2[i])));

		ReadOnlyValPtrTests.ContextTransformTest<T, Byte>(ctx);
		ReadOnlyValPtrTests.ContextTransformTest<T, Int16>(ctx);
		ReadOnlyValPtrTests.ContextTransformTest<T, Int32>(ctx);
		ReadOnlyValPtrTests.ContextTransformTest<T, Int64>(ctx);
	}
	private static unsafe void ReferenceTest<T>(ReadOnlyValPtr<T> ptrI, ref T reference)
	{
		using IReadOnlyFixedReference<T>.IDisposable fixedReference = ptrI.GetUnsafeFixedReference();
		PInvokeAssert.True(Unsafe.AreSame(ref Unsafe.AsRef(in fixedReference.Reference), ref reference));
		PInvokeAssert.Equal(ptrI.Pointer, fixedReference.Pointer);
		PInvokeAssert.Equal(ptrI.IsZero, fixedReference.IsNullOrEmpty);
		if (!ReadOnlyValPtr<T>.IsUnmanaged)
		{
			PInvokeAssert.True(fixedReference.Bytes.IsEmpty);
			PInvokeAssert.Equal(ptrI.IsZero || typeof(T).IsValueType, fixedReference.Objects.IsEmpty);
			if (typeof(T).IsValueType)
			{
				PInvokeAssert.Throws<InvalidOperationException>(() => fixedReference.AsObjectContext());
				PInvokeAssert.True(fixedReference.Objects.IsEmpty);
			}
			else
			{
#if NET8_0_OR_GREATER
				Assert.True(Unsafe.AreSame(in fixedReference.Reference,
#else
				PInvokeAssert.True(Unsafe.AreSame(ref Unsafe.AsRef(in fixedReference.Reference),
#endif
				                           ref Unsafe.As<Object, T>(
					                           ref Unsafe.AsRef(in fixedReference.AsObjectContext().Values[0]))));
				PInvokeAssert.Equal(typeof(T).IsValueType || fixedReference.IsNullOrEmpty,
				                    fixedReference.Objects.IsEmpty);
			}
			PInvokeAssert.Throws<InvalidOperationException>(fixedReference.AsBinaryContext);
			PInvokeAssert.Throws<InvalidOperationException>(() => fixedReference.Transformation<Byte>(out _));
			return;
		}
		PInvokeAssert.True(fixedReference.Objects.IsEmpty);
		PInvokeAssert.Throws<InvalidOperationException>(fixedReference.AsObjectContext);
		PInvokeAssert.True(fixedReference.Bytes.SequenceEqual(new(ptrI.Pointer.ToPointer(), sizeof(T))));
		PInvokeAssert.True(fixedReference.AsBinaryContext().Values
		                                 .SequenceEqual(new(ptrI.Pointer.ToPointer(), sizeof(T))));

		ReadOnlyValPtrTests.ReferenceTransformTest<T, Byte>(ptrI, fixedReference);
		ReadOnlyValPtrTests.ReferenceTransformTest<T, Int16>(ptrI, fixedReference);
		ReadOnlyValPtrTests.ReferenceTransformTest<T, Int32>(ptrI, fixedReference);
		ReadOnlyValPtrTests.ReferenceTransformTest<T, Int64>(ptrI, fixedReference);
	}
	private static void FormatTest<T>(ReadOnlyValPtr<T> valPtr)
	{
		CultureInfo culture =
			ReadOnlyValPtrTests.allCultures[Random.Shared.Next(0, ReadOnlyValPtrTests.allCultures.Length)];
		PInvokeAssert.Equal(valPtr.Pointer.GetHashCode(), valPtr.GetHashCode());
		PInvokeAssert.Equal(valPtr.Pointer.ToString(), valPtr.ToString());

#if NET6_0_OR_GREATER
		if ((Object)valPtr is not ISpanFormattable spanFormattable) return;
		MethodInfo? toStringMethodInfo = spanFormattable.GetType()
		                                                .GetMethod(nameof(IntPtr.ToString),
		                                                           BindingFlags.Public | BindingFlags.Instance, null,
		                                                           [typeof(IFormatProvider),], null);
		if (toStringMethodInfo is not null)
			Assert.Equal(valPtr.Pointer.ToString(culture), toStringMethodInfo.Invoke(valPtr, [culture,]));

		Span<Char> span1 = stackalloc Char[20];
		Span<Char> span2 = stackalloc Char[20];
		Boolean res1 = valPtr.Pointer.TryFormat(span1, out Int32 pC, "X", culture);
		Boolean res2 = spanFormattable.TryFormat(span2, out Int32 vC, "X", culture);

		Assert.Equal(res1, res2);
		Assert.Equal(pC, vC);
		Assert.True(span1.SequenceEqual(span2));

		foreach (String format in ReadOnlyValPtrTests.formats)
		{
			culture = ReadOnlyValPtrTests.allCultures[Random.Shared.Next(0, ReadOnlyValPtrTests.allCultures.Length)];
			Assert.Equal(valPtr.Pointer.ToString(format), valPtr.ToString(format));
			Assert.Equal(valPtr.Pointer.ToString(format, culture), spanFormattable.ToString(format, culture));
		}
#endif
	}
	private static void MarshallerTest<T>(ReadOnlyValPtr<T> valPtr)
	{
		IntPtr value = ReadOnlyValPtr<T>.Marshaller.ConvertToUnmanaged(valPtr);
		ReadOnlyValPtr<T> ptr = ReadOnlyValPtr<T>.Marshaller.ConvertToManaged(value);

		PInvokeAssert.Equal(value, valPtr.Pointer);
		PInvokeAssert.Equal(valPtr, ptr);
	}
	private static unsafe void ReferenceTransformTest<T, TDestination>(ReadOnlyValPtr<T> ptrI,
		IReadOnlyFixedReference<T>.IDisposable fRef) where TDestination : unmanaged
	{
		if (sizeof(TDestination) > sizeof(T)) return;
		IReadOnlyReferenceable<TDestination> fRef2 = fRef.Transformation<TDestination>(out IReadOnlyFixedMemory offset);
		PInvokeAssert.True(Unsafe.AreSame(ref Unsafe.AsRef(in fRef2.Reference),
		                                  ref Unsafe.AsRef<TDestination>(ptrI.Pointer.ToPointer())));
		PInvokeAssert.Equal(sizeof(T) - sizeof(TDestination), offset.Bytes.Length);
	}
	private static unsafe void ContextTransformTest<T, TDestination>(IReadOnlyFixedContext<T>.IDisposable ctx)
	{
		IReadOnlyFixedContext<TDestination> ctx2 = ctx.Transformation<TDestination>(out IReadOnlyFixedMemory offset);
		PInvokeAssert.Equal(ctx2.Values.Length, ctx.Bytes.Length / sizeof(TDestination));
		PInvokeAssert.Equal(offset.Bytes.Length, ctx.Bytes.Length - ctx2.Values.Length * sizeof(TDestination));
	}
#if !NET6_0_OR_GREATER
	private static class Random
	{
		public static readonly System.Random Shared = new();
	}
#endif
}
#pragma warning restore CS8500