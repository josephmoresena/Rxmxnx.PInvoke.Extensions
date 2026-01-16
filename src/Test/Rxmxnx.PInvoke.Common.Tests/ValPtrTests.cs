namespace Rxmxnx.PInvoke.Tests;

[TestFixture]
[ExcludeFromCodeCoverage]
[SuppressMessage("csharpsquid", "S2699")]
#pragma warning disable CS8500
public sealed class ValPtrTests
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
	public void BooleanTest() => ValPtrTests.Test<Boolean>();
	[Fact]
	public void ByteTest() => ValPtrTests.Test<Byte>();
	[Fact]
	public void Int16Test() => ValPtrTests.Test<Int16>();
	[Fact]
	public void CharTest() => ValPtrTests.Test<Char>();
	[Fact]
	public void Int32Test() => ValPtrTests.Test<Int32>();
	[Fact]
	public void Int64Test() => ValPtrTests.Test<Int64>();
	[Fact]
	public void SingleTest() => ValPtrTests.Test<Single>();
	[Fact]
	public void DoubleTest() => ValPtrTests.Test<Double>();
#if NET7_0_OR_GREATER
	[Fact]
	internal void Int128Test() => ValPtrTests.Test<Int128>();
	[Fact]
	internal void GuidTest() => ValPtrTests.Test<Guid>();
	[Fact]
	internal void HalfTest() => ValPtrTests.Test<Half>();
	[Fact]
	internal void DecimalTest() => ValPtrTests.Test<Decimal>();
	[Fact]
	internal void DateTimeTest() => ValPtrTests.Test<DateTime>();
	[Fact]
	internal void TimeOnlyTest() => ValPtrTests.Test<TimeOnly>();
	[Fact]
	internal void TimeSpanTest() => ValPtrTests.Test<TimeSpan>();
	[Fact]
	internal void ManagedStructTest() => ValPtrTests.Test<ManagedStruct>();
#endif
	[Fact]
	public void StringTest() => ValPtrTests.Test<String>();

	private static unsafe void Test<T>()
	{
		T[] arr = ValPtrTests.fixture.CreateMany<T>(10).ToArray();
#if NETCOREAPP
		try
		{
			GCHandle.Alloc(arr, GCHandleType.Pinned).Free();
			Assert.True(ValPtr<T>.IsUnmanaged);
		}
		catch (ArgumentException)
		{
			Assert.False(ValPtr<T>.IsUnmanaged);
		}
#endif
		Span<T> span = arr;
		fixed (void* ptr = &MemoryMarshal.GetReference(span))
			ValPtrTests.Test((ValPtr<T>)new IntPtr(ptr), span);
	}
	private static unsafe void Test<T>(ValPtr<T> valPtr, Span<T> span)
	{
		ValPtr<T> empty = (ValPtr<T>)IntPtr.Zero;
		ValPtr<T> emptyPtr = (ValPtr<T>)IntPtr.Zero.ToPointer();
		ValPtr<T> emptyPtr2 = (T*)IntPtr.Zero.ToPointer();
		PInvokeAssert.Equal(empty, ValPtr<T>.Zero);
		PInvokeAssert.Equal(empty.Pointer, ValPtr<T>.Zero.Pointer);
		PInvokeAssert.Throws<NullReferenceException>(() => empty.Reference);
		PInvokeAssert.Throws<NullReferenceException>(() => ValPtr<T>.Zero.Reference);
		PInvokeAssert.True(empty.IsZero);
		PInvokeAssert.True(ValPtr<T>.Zero.IsZero);
		PInvokeAssert.True(emptyPtr == IntPtr.Zero.ToPointer());
		PInvokeAssert.True(emptyPtr2 == IntPtr.Zero.ToPointer());

		PInvokeAssert.True(ValPtr<T>.Zero.Equals(empty));
		PInvokeAssert.True(ValPtr<T>.Zero.Equals((Object)ValPtr<T>.Zero));
		PInvokeAssert.False(ValPtr<T>.Zero.Equals((Object?)null));
		PInvokeAssert.False(ValPtr<T>.Zero.Equals(empty.Pointer));

		PInvokeAssert.Equal(1, valPtr.CompareTo(null));
		PInvokeAssert.Throws<ArgumentException>(() => valPtr.CompareTo(valPtr.Pointer));

		ValPtrTests.FormatTest(ValPtr<T>.Zero);
		ValPtrTests.MarshallerTest(ValPtr<T>.Zero);

		ValPtr<T> ptrI;
		for (Int32 i = 0; i < span.Length; i++)
		{
			Int32 binaryOffset = sizeof(T) * i;
			ValPtr<T> ptrIAdd = ValPtr<T>.Add(valPtr, i);
			ptrI = valPtr + i;
			PInvokeAssert.Equal(ptrI, ptrIAdd);
			PInvokeAssert.Equal(valPtr, ValPtr<T>.Subtract(ptrI, i));
			PInvokeAssert.True(ptrI == valPtr.Pointer + binaryOffset);
			PInvokeAssert.True(Unsafe.AreSame(ref ptrI.Reference, ref span[i]));
			PInvokeAssert.Equal(valPtr.Pointer, ptrI.Pointer - binaryOffset);
			PInvokeAssert.False(ptrI.IsZero);
			PInvokeAssert.Equal(ptrI.Pointer, (ptrI as IWrapper<IntPtr>).Value);

			ValPtrTests.ReferenceTest(ptrI, ref span[i]);

			PInvokeAssert.True(ptrI >= valPtr);
			PInvokeAssert.True(valPtr <= ptrI);

			if (i <= 0) continue;
			PInvokeAssert.True(ptrI > valPtr);
			PInvokeAssert.True(valPtr < ptrI);

			ReadOnlyValPtr<T> ptrIAdd2 = ptrIAdd;

			PInvokeAssert.Equal(1, ptrI.CompareTo(valPtr));
			PInvokeAssert.Equal(0, ptrI.CompareTo(ptrIAdd));
			PInvokeAssert.Equal(0, valPtr.CompareTo(ptrI - i));

			PInvokeAssert.Equal(1, ptrI.CompareTo((Object)valPtr));
			PInvokeAssert.Equal(0, ptrI.CompareTo(ptrIAdd2));
			PInvokeAssert.Equal(1, ptrI.CompareTo(null));
			PInvokeAssert.Throws<ArgumentException>(() => ptrI.CompareTo(ptrI.Pointer));

			PInvokeAssert.False(ptrI.Equals((Object)valPtr));
			PInvokeAssert.True(ptrI.Equals(ptrIAdd2));
			PInvokeAssert.False(ptrI.Equals((Object?)null));
			PInvokeAssert.False(ptrI.Equals(ptrI.Pointer));

			ValPtrTests.FormatTest(ptrI);

			void* ptrI2 = ptrI;
			T* ptrI3 = ptrI;

			PInvokeAssert.True(ptrI2 == ptrI3);
		}

		if (span.Length <= 0) return;
		ValPtr<T> incValue = valPtr;
		PInvokeAssert.True(valPtr == incValue);
		incValue++;
		PInvokeAssert.True(valPtr.Pointer + sizeof(T) == incValue);
		PInvokeAssert.True(valPtr != incValue);
		incValue--;
		PInvokeAssert.Equal(valPtr.Pointer, incValue);
		PInvokeAssert.False(valPtr != incValue);

		ValPtrTests.ContextTest(valPtr, span);
		ValPtrTests.MarshallerTest(valPtr);
	}
	private static unsafe void ContextTest<T>(ValPtr<T> valPtr, Span<T> span)
	{
		using IFixedContext<T>.IDisposable ctx = valPtr.GetUnsafeFixedContext(span.Length);
		PInvokeAssert.Equal(ctx.Values.Length, span.Length);
		PInvokeAssert.Equal(valPtr.Pointer, ctx.Pointer);
		if (!ValPtr<T>.IsUnmanaged)
		{
			PInvokeAssert.Throws<InvalidOperationException>(ctx.AsBinaryContext);
			PInvokeAssert.Throws<InvalidOperationException>(() => ctx.Transformation<Byte>(out IFixedMemory _));

			if (typeof(T).IsValueType)
			{
				PInvokeAssert.Throws<InvalidOperationException>(ctx.AsObjectContext);
				PInvokeAssert.True(ctx.Objects.IsEmpty);
			}
			else
			{
				Span<T>.Enumerator enumerator = span.GetEnumerator();
				foreach (ref Object refObj in ctx.AsObjectContext().Values)
				{
					if (!enumerator.MoveNext()) break;
					PInvokeAssert.True(Unsafe.AreSame(ref enumerator.Current, ref Unsafe.As<Object, T>(ref refObj)));
				}
				PInvokeAssert.Equal(typeof(T).IsValueType || ctx.IsNullOrEmpty, ctx.Objects.IsEmpty);
			}
			return;
		}

		PInvokeAssert.True(ctx.AsBinaryContext().Values
		                      .SequenceEqual(new(valPtr.Pointer.ToPointer(), span.Length * sizeof(T))));
		PInvokeAssert.Throws<InvalidOperationException>(ctx.AsObjectContext);

		Span<T> span2 = ctx.Values;
		for (Int32 i = 0; i < span.Length; i++)
			PInvokeAssert.True(Unsafe.AreSame(ref span[i], ref span2[i]));

		ValPtrTests.ContextTransformTest<T, Byte>(ctx);
		ValPtrTests.ContextTransformTest<T, Int16>(ctx);
		ValPtrTests.ContextTransformTest<T, Int32>(ctx);
		ValPtrTests.ContextTransformTest<T, Int64>(ctx);
	}
	private static unsafe void ReferenceTest<T>(ValPtr<T> ptrI, ref T reference)
	{
		using IFixedReference<T>.IDisposable fixedReference = ptrI.GetUnsafeFixedReference();
		PInvokeAssert.True(Unsafe.AreSame(ref fixedReference.Reference, ref reference));
		PInvokeAssert.Equal(ptrI.Pointer, fixedReference.Pointer);
		PInvokeAssert.Equal(ptrI.IsZero, fixedReference.IsNullOrEmpty);
		if (!ValPtr<T>.IsUnmanaged)
		{
			PInvokeAssert.True(fixedReference.Bytes.IsEmpty);
			PInvokeAssert.Equal(ptrI.IsZero || typeof(T).IsValueType, fixedReference.Objects.IsEmpty);
			PInvokeAssert.Throws<InvalidOperationException>(fixedReference.AsBinaryContext);
			if (typeof(T).IsValueType)
			{
				PInvokeAssert.Throws<InvalidOperationException>(fixedReference.AsObjectContext);
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
					                                  ref Unsafe.AsRef(
						                                  in fixedReference.AsObjectContext().Values[0]))));
				PInvokeAssert.Equal(typeof(T).IsValueType || fixedReference.IsNullOrEmpty,
				                    fixedReference.Objects.IsEmpty);
			}
			PInvokeAssert.Throws<InvalidOperationException>(() => fixedReference.Transformation<Byte>(
				                                                out IFixedMemory _));
			return;
		}
		PInvokeAssert.True(fixedReference.Objects.IsEmpty);
		PInvokeAssert.Throws<InvalidOperationException>(fixedReference.AsObjectContext);
		PInvokeAssert.True(fixedReference.Bytes.SequenceEqual(new(ptrI.Pointer.ToPointer(), sizeof(T))));
		PInvokeAssert.True(fixedReference.AsBinaryContext().Values
		                                 .SequenceEqual(new(ptrI.Pointer.ToPointer(), sizeof(T))));

		ValPtrTests.ReferenceTransformTest<T, Byte>(ptrI, fixedReference);
		ValPtrTests.ReferenceTransformTest<T, Int16>(ptrI, fixedReference);
		ValPtrTests.ReferenceTransformTest<T, Int32>(ptrI, fixedReference);
		ValPtrTests.ReferenceTransformTest<T, Int64>(ptrI, fixedReference);
	}
	private static void FormatTest<T>(ValPtr<T> valPtr)
	{
		CultureInfo culture = ValPtrTests.allCultures[PInvokeRandom.Shared.Next(0, ValPtrTests.allCultures.Length)];
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

		foreach (String format in ValPtrTests.formats)
		{
			culture = ValPtrTests.allCultures[PInvokeRandom.Shared.Next(0, ValPtrTests.allCultures.Length)];
			Assert.Equal(valPtr.Pointer.ToString(format), valPtr.ToString(format));
			Assert.Equal(valPtr.Pointer.ToString(format, culture), spanFormattable.ToString(format, culture));
		}
#endif
	}
	private static void MarshallerTest<T>(ValPtr<T> valPtr)
	{
		IntPtr value = ValPtr<T>.Marshaller.ConvertToUnmanaged(valPtr);
		ValPtr<T> ptr = ValPtr<T>.Marshaller.ConvertToManaged(value);

		PInvokeAssert.Equal(value, valPtr.Pointer);
		PInvokeAssert.Equal(valPtr, ptr);
	}
	private static unsafe void ReferenceTransformTest<T, TDestination>(ValPtr<T> ptrI,
		IFixedReference<T>.IDisposable fRef)
	{
		if (sizeof(TDestination) > sizeof(T)) return;
		IReferenceable<TDestination> fRef2 = fRef.Transformation<TDestination>(out IFixedMemory offset);
		IReadOnlyReferenceable<TDestination> fRef3 =
			(fRef as IReadOnlyFixedReference<T>).Transformation<TDestination>(out IReadOnlyFixedMemory _);
		PInvokeAssert.True(
			Unsafe.AreSame(ref fRef2.Reference, ref Unsafe.AsRef<TDestination>(ptrI.Pointer.ToPointer())));
		PInvokeAssert.Equal(sizeof(T) - sizeof(TDestination), offset.Bytes.Length);
#if NET8_0_OR_GREATER
		Assert.True(Unsafe.AreSame(ref fRef2.Reference, in fRef3.Reference));
#else
		PInvokeAssert.True(Unsafe.AreSame(ref fRef2.Reference, ref Unsafe.AsRef(in fRef3.Reference)));
#endif
	}
	private static unsafe void ContextTransformTest<T, TDestination>(IFixedContext<T>.IDisposable ctx)
	{
		IFixedContext<TDestination> ctx2 = ctx.Transformation<TDestination>(out IFixedMemory offset);
		PInvokeAssert.Equal(ctx2.Values.Length, ctx.Bytes.Length / sizeof(TDestination));
		PInvokeAssert.Equal(offset.Bytes.Length, ctx.Bytes.Length - ctx2.Values.Length * sizeof(TDestination));
	}
}
#pragma warning restore CS8500