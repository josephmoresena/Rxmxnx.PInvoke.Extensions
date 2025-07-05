namespace Rxmxnx.PInvoke.Tests;

[ExcludeFromCodeCoverage]
[SuppressMessage("csharpsquid", "S2699")]
#pragma warning disable CS8500
public sealed class ReadOnlyValPtrTests
{
	private static readonly CultureInfo[] allCultures = CultureInfo.GetCultures(CultureTypes.AllCultures);
	private static readonly String[] formats = ["", "b", "B", "D", "d", "E", "e", "G", "g", "X", "x",];
	private static readonly IFixture fixture = ManagedStruct.Register(new Fixture());
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
	[Fact]
	internal void ManagedStructTest() => ReadOnlyValPtrTests.Test<ManagedStruct>();
	[Fact]
	internal void StringTest() => ReadOnlyValPtrTests.Test<String>();
	private static unsafe void Test<T>()
	{
		T[] arr = ReadOnlyValPtrTests.fixture.CreateMany<T>(10).ToArray();
		try
		{
			GCHandle.Alloc(arr, GCHandleType.Pinned).Free();
			Assert.True(ReadOnlyValPtr<T>.IsUnmanaged);
		}
		catch (ArgumentException)
		{
			Assert.False(ReadOnlyValPtr<T>.IsUnmanaged);
		}
		Span<T> span = arr;
		fixed (void* ptr = &MemoryMarshal.GetReference(span))
			ReadOnlyValPtrTests.Test((ReadOnlyValPtr<T>)new IntPtr(ptr), span);
	}
	private static unsafe void Test<T>(ReadOnlyValPtr<T> valPtr, ReadOnlySpan<T> span)
	{
		ReadOnlyValPtr<T> empty = (ReadOnlyValPtr<T>)IntPtr.Zero;
		ReadOnlyValPtr<T> emptyPtr = (ReadOnlyValPtr<T>)IntPtr.Zero.ToPointer();
		ReadOnlyValPtr<T> emptyPtr2 = (T*)IntPtr.Zero.ToPointer();
		Assert.Equal(empty, ReadOnlyValPtr<T>.Zero);
		Assert.Equal(empty.Pointer, ReadOnlyValPtr<T>.Zero.Pointer);
		Assert.Throws<NullReferenceException>(() => empty.Reference);
		Assert.Throws<NullReferenceException>(() => ReadOnlyValPtr<T>.Zero.Reference);
		Assert.True(empty.IsZero);
		Assert.True(ReadOnlyValPtr<T>.Zero.IsZero);
		Assert.True(emptyPtr == IntPtr.Zero.ToPointer());
		Assert.True(emptyPtr2 == IntPtr.Zero.ToPointer());

		Assert.True(ReadOnlyValPtr<T>.Zero.Equals(empty));
		Assert.True(ReadOnlyValPtr<T>.Zero.Equals((Object)ValPtr<T>.Zero));
		Assert.False(ReadOnlyValPtr<T>.Zero.Equals((Object?)null));
		Assert.False(ReadOnlyValPtr<T>.Zero.Equals(empty.Pointer));

		Assert.Equal(1, valPtr.CompareTo(null));
		Assert.Throws<ArgumentException>(() => valPtr.CompareTo(valPtr.Pointer));

		ReadOnlyValPtrTests.FormatTest(ReadOnlyValPtr<T>.Zero);
		ReadOnlyValPtrTests.MarshallerTest(ReadOnlyValPtr<T>.Zero);

		ReadOnlyValPtr<T> ptrI;
		for (Int32 i = 0; i < span.Length; i++)
		{
			Int32 binaryOffset = sizeof(T) * i;
			ReadOnlyValPtr<T> ptrIAdd = ReadOnlyValPtr<T>.Add(valPtr, i);
			ptrI = valPtr + i;
			Assert.Equal(ptrI, ptrIAdd);
			Assert.Equal(valPtr, ReadOnlyValPtr<T>.Subtract(ptrI, i));
			Assert.True(ptrI == valPtr.Pointer + binaryOffset);
			Assert.True(Unsafe.AreSame(in ptrI.Reference, ref Unsafe.AsRef(in span[i])));
			Assert.Equal(valPtr.Pointer, ptrI.Pointer - binaryOffset);
			Assert.False(ptrI.IsZero);
			Assert.Equal(ptrI.Pointer, (ptrI as IWrapper<IntPtr>).Value);

			ReadOnlyValPtrTests.ReferenceTest(ptrI, ref Unsafe.AsRef(in span[i]));

			Assert.True(ptrI >= valPtr);
			Assert.True(valPtr <= ptrI);

			if (i <= 0) continue;
			Assert.True(ptrI > valPtr);
			Assert.True(valPtr < ptrI);

			ValPtr<T> ptrIAdd2 = Unsafe.As<ReadOnlyValPtr<T>, ValPtr<T>>(ref Unsafe.AsRef(in ptrIAdd));

			Assert.Equal(1, ptrI.CompareTo(valPtr));
			Assert.Equal(0, ptrI.CompareTo(ptrIAdd));
			Assert.Equal(0, valPtr.CompareTo(ptrI - i));

			Assert.Equal(1, ptrI.CompareTo((Object)valPtr));
			Assert.Equal(0, ptrI.CompareTo((Object)ptrIAdd2));
			Assert.Equal(1, ptrI.CompareTo(null));
			Assert.Throws<ArgumentException>(() => ptrI.CompareTo(ptrI.Pointer));

			Assert.False(ptrI.Equals((Object)valPtr));
			Assert.True(ptrI.Equals((Object)ptrIAdd2));
			Assert.False(ptrI.Equals((Object?)null));
			Assert.False(ptrI.Equals(ptrI.Pointer));

			ReadOnlyValPtrTests.FormatTest(ptrI);

			void* ptrI2 = ptrI;
			T* ptrI3 = ptrI;

			Assert.True(ptrI2 == ptrI3);
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
		ReadOnlyValPtrTests.MarshallerTest(valPtr);
	}
	private static unsafe void ContextTest<T>(ReadOnlyValPtr<T> valPtr, ReadOnlySpan<T> span)
	{
		using IReadOnlyFixedContext<T>.IDisposable ctx = valPtr.GetUnsafeFixedContext(span.Length);
		Assert.Equal(ctx.Values.Length, span.Length);
		Assert.Equal(valPtr.Pointer, ctx.Pointer);
		if (!ReadOnlyValPtr<T>.IsUnmanaged)
		{
			Assert.Throws<InvalidOperationException>(ctx.AsBinaryContext);
			Assert.Throws<InvalidOperationException>(() => ctx.Transformation<Byte>(out _));

			if (typeof(T).IsValueType)
			{
				Assert.Throws<InvalidOperationException>(ctx.AsObjectContext);
				Assert.True(ctx.Objects.IsEmpty);
			}
			else
			{
				ReadOnlySpan<T>.Enumerator enumerator = span.GetEnumerator();
				foreach (ref readonly Object refObj in ctx.AsObjectContext().Values)
				{
					if (!enumerator.MoveNext()) break;
					Assert.True(Unsafe.AreSame(in enumerator.Current,
					                           ref Unsafe.As<Object, T>(ref Unsafe.AsRef(in refObj))));
				}
				Assert.Equal(typeof(T).IsValueType || ctx.IsNullOrEmpty, ctx.Objects.IsEmpty);
			}

			return;
		}
		Assert.True(
			ctx.AsBinaryContext().Values.SequenceEqual(new(valPtr.Pointer.ToPointer(), span.Length * sizeof(T))));
		Assert.Throws<InvalidOperationException>(ctx.AsObjectContext);

		ReadOnlySpan<T> span2 = ctx.Values;
		for (Int32 i = 0; i < span.Length; i++)
			Assert.True(Unsafe.AreSame(ref Unsafe.AsRef(in span[i]), ref Unsafe.AsRef(in span2[i])));

		ReadOnlyValPtrTests.ContextTransformTest<T, Byte>(ctx);
		ReadOnlyValPtrTests.ContextTransformTest<T, Int16>(ctx);
		ReadOnlyValPtrTests.ContextTransformTest<T, Int32>(ctx);
		ReadOnlyValPtrTests.ContextTransformTest<T, Int64>(ctx);
	}
	private static unsafe void ReferenceTest<T>(ReadOnlyValPtr<T> ptrI, ref T reference)
	{
		using IReadOnlyFixedReference<T>.IDisposable fixedReference = ptrI.GetUnsafeFixedReference();
		Assert.True(Unsafe.AreSame(ref Unsafe.AsRef(in fixedReference.Reference), ref reference));
		Assert.Equal(ptrI.Pointer, fixedReference.Pointer);
		Assert.Equal(ptrI.IsZero, fixedReference.IsNullOrEmpty);
		if (!ReadOnlyValPtr<T>.IsUnmanaged)
		{
			Assert.True(fixedReference.Bytes.IsEmpty);
			Assert.Equal(ptrI.IsZero || typeof(T).IsValueType, fixedReference.Objects.IsEmpty);
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
			Assert.Throws<InvalidOperationException>(fixedReference.AsBinaryContext);
			Assert.Throws<InvalidOperationException>(() => fixedReference.Transformation<Byte>(out _));
			return;
		}
		Assert.True(fixedReference.Objects.IsEmpty);
		Assert.Throws<InvalidOperationException>(fixedReference.AsObjectContext);
		Assert.True(fixedReference.Bytes.SequenceEqual(new(ptrI.Pointer.ToPointer(), sizeof(T))));
		Assert.True(fixedReference.AsBinaryContext().Values.SequenceEqual(new(ptrI.Pointer.ToPointer(), sizeof(T))));

		ReadOnlyValPtrTests.ReferenceTransformTest<T, Byte>(ptrI, fixedReference);
		ReadOnlyValPtrTests.ReferenceTransformTest<T, Int16>(ptrI, fixedReference);
		ReadOnlyValPtrTests.ReferenceTransformTest<T, Int32>(ptrI, fixedReference);
		ReadOnlyValPtrTests.ReferenceTransformTest<T, Int64>(ptrI, fixedReference);
	}
	private static void FormatTest<T>(ReadOnlyValPtr<T> valPtr)
	{
		CultureInfo culture =
			ReadOnlyValPtrTests.allCultures[Random.Shared.Next(0, ReadOnlyValPtrTests.allCultures.Length)];
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

		foreach (String format in ReadOnlyValPtrTests.formats)
		{
			culture = ReadOnlyValPtrTests.allCultures[Random.Shared.Next(0, ReadOnlyValPtrTests.allCultures.Length)];
			Assert.Equal(valPtr.Pointer.ToString(format), valPtr.ToString(format));
			Assert.Equal(valPtr.Pointer.ToString(format, culture), spanFormattable.ToString(format, culture));
		}
	}
	private static void MarshallerTest<T>(ReadOnlyValPtr<T> valPtr)
	{
		IntPtr value = ReadOnlyValPtr<T>.Marshaller.ConvertToUnmanaged(valPtr);
		ReadOnlyValPtr<T> ptr = ReadOnlyValPtr<T>.Marshaller.ConvertToManaged(value);

		Assert.Equal(value, valPtr.Pointer);
		Assert.Equal(valPtr, ptr);
	}
	private static unsafe void ReferenceTransformTest<T, TDestination>(ReadOnlyValPtr<T> ptrI,
		IReadOnlyFixedReference<T>.IDisposable fRef) where TDestination : unmanaged
	{
		if (sizeof(TDestination) > sizeof(T)) return;
		IReadOnlyReferenceable<TDestination> fRef2 = fRef.Transformation<TDestination>(out IReadOnlyFixedMemory offset);
		Assert.True(Unsafe.AreSame(ref Unsafe.AsRef(in fRef2.Reference),
		                           ref Unsafe.AsRef<TDestination>(ptrI.Pointer.ToPointer())));
		Assert.Equal(sizeof(T) - sizeof(TDestination), offset.Bytes.Length);
	}
	private static unsafe void ContextTransformTest<T, TDestination>(IReadOnlyFixedContext<T>.IDisposable ctx)
	{
		IReadOnlyFixedContext<TDestination> ctx2 = ctx.Transformation<TDestination>(out IReadOnlyFixedMemory offset);
		Assert.Equal(ctx2.Values.Length, ctx.Bytes.Length / sizeof(TDestination));
		Assert.Equal(offset.Bytes.Length, ctx.Bytes.Length - ctx2.Values.Length * sizeof(TDestination));
	}
}
#pragma warning restore CS8500