namespace Rxmxnx.PInvoke.Tests;

[ExcludeFromCodeCoverage]
[SuppressMessage("csharpsquid", "S2699")]
#pragma warning disable CS8500
public sealed class ValPtrTests
{
	private static readonly CultureInfo[] allCultures = CultureInfo.GetCultures(CultureTypes.AllCultures);
	private static readonly String[] formats = ["", "b", "B", "D", "d", "E", "e", "G", "g", "X", "x",];
	private static readonly IFixture fixture = ManagedStruct.Register(new Fixture());
	[Fact]
	internal void BooleanTest() => ValPtrTests.Test<Boolean>();
	[Fact]
	internal void ByteTest() => ValPtrTests.Test<Byte>();
	[Fact]
	internal void Int16Test() => ValPtrTests.Test<Int16>();
	[Fact]
	internal void CharTest() => ValPtrTests.Test<Char>();
	[Fact]
	internal void Int32Test() => ValPtrTests.Test<Int32>();
	[Fact]
	internal void Int64Test() => ValPtrTests.Test<Int64>();
	[Fact]
	internal void Int128Test() => ValPtrTests.Test<Int128>();
	[Fact]
	internal void GuidTest() => ValPtrTests.Test<Guid>();
	[Fact]
	internal void SingleTest() => ValPtrTests.Test<Single>();
	[Fact]
	internal void HalfTest() => ValPtrTests.Test<Half>();
	[Fact]
	internal void DoubleTest() => ValPtrTests.Test<Double>();
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
	[Fact]
	internal void StringTest() => ValPtrTests.Test<String>();
	private static unsafe void Test<T>()
	{
		T[] arr = ValPtrTests.fixture.CreateMany<T>(10).ToArray();
		try
		{
			GCHandle.Alloc(arr, GCHandleType.Pinned).Free();
			Assert.True(ValPtr<T>.IsUnmanaged);
		}
		catch (ArgumentException)
		{
			Assert.False(ValPtr<T>.IsUnmanaged);
		}
		Span<T> span = arr;
		fixed (void* ptr = &MemoryMarshal.GetReference(span))
			ValPtrTests.Test((ValPtr<T>)new IntPtr(ptr), span);
	}
	private static unsafe void Test<T>(ValPtr<T> valPtr, Span<T> span)
	{
		ValPtr<T> empty = (ValPtr<T>)IntPtr.Zero;
		ValPtr<T> emptyPtr = (ValPtr<T>)IntPtr.Zero.ToPointer();
		ValPtr<T> emptyPtr2 = (T*)IntPtr.Zero.ToPointer();
		Assert.Equal(empty, ValPtr<T>.Zero);
		Assert.Equal(empty.Pointer, ValPtr<T>.Zero.Pointer);
		Assert.Throws<NullReferenceException>(() => empty.Reference);
		Assert.Throws<NullReferenceException>(() => ValPtr<T>.Zero.Reference);
		Assert.True(empty.IsZero);
		Assert.True(ValPtr<T>.Zero.IsZero);
		Assert.True(emptyPtr == IntPtr.Zero.ToPointer());
		Assert.True(emptyPtr2 == IntPtr.Zero.ToPointer());

		Assert.True(ValPtr<T>.Zero.Equals(empty));
		Assert.True(ValPtr<T>.Zero.Equals((Object)ValPtr<T>.Zero));
		Assert.False(ValPtr<T>.Zero.Equals((Object?)null));
		Assert.False(ValPtr<T>.Zero.Equals(empty.Pointer));

		Assert.Equal(1, valPtr.CompareTo(null));
		Assert.Throws<ArgumentException>(() => valPtr.CompareTo(valPtr.Pointer));

		ValPtrTests.FormatTest(ValPtr<T>.Zero);
		ValPtrTests.MarshallerTest(ValPtr<T>.Zero);

		ValPtr<T> ptrI;
		for (Int32 i = 0; i < span.Length; i++)
		{
			Int32 binaryOffset = sizeof(T) * i;
			ValPtr<T> ptrIAdd = ValPtr<T>.Add(valPtr, i);
			ptrI = valPtr + i;
			Assert.Equal(ptrI, ptrIAdd);
			Assert.Equal(valPtr, ValPtr<T>.Subtract(ptrI, i));
			Assert.True(ptrI == valPtr.Pointer + binaryOffset);
			Assert.True(Unsafe.AreSame(ref ptrI.Reference, ref span[i]));
			Assert.Equal(valPtr.Pointer, ptrI.Pointer - binaryOffset);
			Assert.False(ptrI.IsZero);
			Assert.Equal(ptrI.Pointer, (ptrI as IWrapper<IntPtr>).Value);

			ValPtrTests.ReferenceTest(ptrI, ref span[i]);

			Assert.True(ptrI >= valPtr);
			Assert.True(valPtr <= ptrI);

			if (i <= 0) continue;
			Assert.True(ptrI > valPtr);
			Assert.True(valPtr < ptrI);

			ReadOnlyValPtr<T> ptrIAdd2 = ptrIAdd;

			Assert.Equal(1, ptrI.CompareTo(valPtr));
			Assert.Equal(0, ptrI.CompareTo(ptrIAdd));
			Assert.Equal(0, valPtr.CompareTo(ptrI - i));

			Assert.Equal(1, ptrI.CompareTo((Object)valPtr));
			Assert.Equal(0, ptrI.CompareTo(ptrIAdd2));
			Assert.Equal(1, ptrI.CompareTo(null));
			Assert.Throws<ArgumentException>(() => ptrI.CompareTo(ptrI.Pointer));

			Assert.False(ptrI.Equals((Object)valPtr));
			Assert.True(ptrI.Equals(ptrIAdd2));
			Assert.False(ptrI.Equals((Object?)null));
			Assert.False(ptrI.Equals(ptrI.Pointer));

			ValPtrTests.FormatTest(ptrI);

			void* ptrI2 = ptrI;
			T* ptrI3 = ptrI;

			Assert.True(ptrI2 == ptrI3);
		}

		if (span.Length <= 0) return;
		ValPtr<T> incValue = valPtr;
		Assert.True(valPtr == incValue);
		incValue++;
		Assert.True(valPtr.Pointer + sizeof(T) == incValue);
		Assert.True(valPtr != incValue);
		incValue--;
		Assert.Equal(valPtr.Pointer, incValue);
		Assert.False(valPtr != incValue);

		ValPtrTests.ContextTest(valPtr, span);
		ValPtrTests.MarshallerTest(valPtr);
	}
	private static unsafe void ContextTest<T>(ValPtr<T> valPtr, Span<T> span)
	{
		using IFixedContext<T>.IDisposable ctx = valPtr.GetUnsafeFixedContext(span.Length);
		Assert.Equal(ctx.Values.Length, span.Length);
		Assert.Equal(valPtr.Pointer, ctx.Pointer);
		if (!ValPtr<T>.IsUnmanaged)
		{
			Assert.Throws<InvalidOperationException>(ctx.AsBinaryContext);
			Assert.Throws<InvalidOperationException>(() => ctx.Transformation<Byte>(out IFixedMemory _));

			if (typeof(T).IsValueType)
			{
				Assert.Throws<InvalidOperationException>(ctx.AsObjectContext);
				Assert.True(ctx.Objects.IsEmpty);
			}
			else
			{
				Span<T>.Enumerator enumerator = span.GetEnumerator();
				foreach (ref Object refObj in ctx.AsObjectContext().Values)
				{
					if (!enumerator.MoveNext()) break;
					Assert.True(Unsafe.AreSame(ref enumerator.Current, ref Unsafe.As<Object, T>(ref refObj)));
				}
				Assert.Equal(typeof(T).IsValueType || ctx.IsNullOrEmpty, ctx.Objects.IsEmpty);
			}
			return;
		}

		Assert.True(
			ctx.AsBinaryContext().Values.SequenceEqual(new(valPtr.Pointer.ToPointer(), span.Length * sizeof(T))));
		Assert.Throws<InvalidOperationException>(ctx.AsObjectContext);

		Span<T> span2 = ctx.Values;
		for (Int32 i = 0; i < span.Length; i++)
			Assert.True(Unsafe.AreSame(ref span[i], ref span2[i]));

		ValPtrTests.ContextTransformTest<T, Byte>(ctx);
		ValPtrTests.ContextTransformTest<T, Int16>(ctx);
		ValPtrTests.ContextTransformTest<T, Int32>(ctx);
		ValPtrTests.ContextTransformTest<T, Int64>(ctx);
	}
	private static unsafe void ReferenceTest<T>(ValPtr<T> ptrI, ref T reference)
	{
		using IFixedReference<T>.IDisposable fixedReference = ptrI.GetUnsafeFixedReference();
		Assert.True(Unsafe.AreSame(ref fixedReference.Reference, ref reference));
		Assert.Equal(ptrI.Pointer, fixedReference.Pointer);
		Assert.Equal(ptrI.IsZero, fixedReference.IsNullOrEmpty);
		if (!ValPtr<T>.IsUnmanaged)
		{
			Assert.True(fixedReference.Bytes.IsEmpty);
			Assert.Equal(ptrI.IsZero || typeof(T).IsValueType, fixedReference.Objects.IsEmpty);
			Assert.Throws<InvalidOperationException>(fixedReference.AsBinaryContext);
			if (typeof(T).IsValueType)
			{
				Assert.Throws<InvalidOperationException>(fixedReference.AsObjectContext);
				Assert.True(fixedReference.Objects.IsEmpty);
			}
			else
			{
				Assert.True(Unsafe.AreSame(in fixedReference.Reference,
				                           ref Unsafe.As<Object, T>(
					                           ref Unsafe.AsRef(in fixedReference.AsObjectContext().Values[0]))));
				Assert.Equal(typeof(T).IsValueType || fixedReference.IsNullOrEmpty, fixedReference.Objects.IsEmpty);
			}
			Assert.Throws<InvalidOperationException>(() => fixedReference.Transformation<Byte>(out IFixedMemory _));
			return;
		}
		Assert.True(fixedReference.Objects.IsEmpty);
		Assert.Throws<InvalidOperationException>(fixedReference.AsObjectContext);
		Assert.True(fixedReference.Bytes.SequenceEqual(new(ptrI.Pointer.ToPointer(), sizeof(T))));
		Assert.True(fixedReference.AsBinaryContext().Values.SequenceEqual(new(ptrI.Pointer.ToPointer(), sizeof(T))));

		ValPtrTests.ReferenceTransformTest<T, Byte>(ptrI, fixedReference);
		ValPtrTests.ReferenceTransformTest<T, Int16>(ptrI, fixedReference);
		ValPtrTests.ReferenceTransformTest<T, Int32>(ptrI, fixedReference);
		ValPtrTests.ReferenceTransformTest<T, Int64>(ptrI, fixedReference);
	}
	private static void FormatTest<T>(ValPtr<T> valPtr)
	{
		CultureInfo culture = ValPtrTests.allCultures[Random.Shared.Next(0, ValPtrTests.allCultures.Length)];
		Assert.Equal(valPtr.Pointer.GetHashCode(), valPtr.GetHashCode());
		Assert.Equal(valPtr.Pointer.ToString(), valPtr.ToString());

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
			culture = ValPtrTests.allCultures[Random.Shared.Next(0, ValPtrTests.allCultures.Length)];
			Assert.Equal(valPtr.Pointer.ToString(format), valPtr.ToString(format));
			Assert.Equal(valPtr.Pointer.ToString(format, culture), spanFormattable.ToString(format, culture));
		}
	}
	private static void MarshallerTest<T>(ValPtr<T> valPtr)
	{
		IntPtr value = ValPtr<T>.Marshaller.ConvertToUnmanaged(valPtr);
		ValPtr<T> ptr = ValPtr<T>.Marshaller.ConvertToManaged(value);

		Assert.Equal(value, valPtr.Pointer);
		Assert.Equal(valPtr, ptr);
	}
	private static unsafe void ReferenceTransformTest<T, TDestination>(ValPtr<T> ptrI,
		IFixedReference<T>.IDisposable fRef)
	{
		if (sizeof(TDestination) > sizeof(T)) return;
		IReferenceable<TDestination> fRef2 = fRef.Transformation<TDestination>(out IFixedMemory offset);
		IReadOnlyReferenceable<TDestination> fRef3 =
			(fRef as IReadOnlyFixedReference<T>).Transformation<TDestination>(out IReadOnlyFixedMemory _);
		Assert.True(Unsafe.AreSame(ref fRef2.Reference, ref Unsafe.AsRef<TDestination>(ptrI.Pointer.ToPointer())));
		Assert.Equal(sizeof(T) - sizeof(TDestination), offset.Bytes.Length);
		Assert.True(Unsafe.AreSame(ref fRef2.Reference, in fRef3.Reference));
	}
	private static unsafe void ContextTransformTest<T, TDestination>(IFixedContext<T>.IDisposable ctx)
	{
		IFixedContext<TDestination> ctx2 = ctx.Transformation<TDestination>(out IFixedMemory offset);
		Assert.Equal(ctx2.Values.Length, ctx.Bytes.Length / sizeof(TDestination));
		Assert.Equal(offset.Bytes.Length, ctx.Bytes.Length - ctx2.Values.Length * sizeof(TDestination));
	}
}
#pragma warning restore CS8500