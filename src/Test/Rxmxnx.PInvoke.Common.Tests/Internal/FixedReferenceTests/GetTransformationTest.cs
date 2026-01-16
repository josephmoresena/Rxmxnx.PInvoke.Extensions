namespace Rxmxnx.PInvoke.Tests.Internal.FixedReferenceTests;

[TestFixture]
[ExcludeFromCodeCoverage]
[SuppressMessage("csharpsquid", "S2699")]
#pragma warning disable CS8500
public sealed class GetTransformationTest : FixedReferenceTestsBase
{
	[Fact]
	public void BooleanTest() => GetTransformationTest.Test<Boolean>();
	[Fact]
	public void ByteTest() => GetTransformationTest.Test<Byte>();
	[Fact]
	public void Int16Test() => GetTransformationTest.Test<Int16>();
	[Fact]
	public void CharTest() => GetTransformationTest.Test<Char>();
	[Fact]
	public void Int32Test() => GetTransformationTest.Test<Int32>();
	[Fact]
	public void Int64Test() => GetTransformationTest.Test<Int64>();
#if NET7_0_OR_GREATER
	[Fact]
	internal void Int128Test() => GetTransformationTest.Test<Int128>();
#endif
	[Fact]
	public void GuidTest() => GetTransformationTest.Test<Guid>();
	[Fact]
	public void SingleTest() => GetTransformationTest.Test<Single>();
#if NET5_0_OR_GREATER
	[Fact]
	internal void HalfTest() => GetTransformationTest.Test<Half>();
#endif
	[Fact]
	public void DoubleTest() => GetTransformationTest.Test<Double>();
	[Fact]
	public void DecimalTest() => GetTransformationTest.Test<Decimal>();
#if NET7_0_OR_GREATER
	[Fact]
	internal void DateTimeTest() => GetTransformationTest.Test<DateTime>();
#endif
#if NET6_0_OR_GREATER
	[Fact]
	internal void TimeOnlyTest() => GetTransformationTest.Test<TimeOnly>();
#endif
	[Fact]
	public void TimeSpanTest() => GetTransformationTest.Test<TimeSpan>();
	[Fact]
	public void ManagedStructTest() => GetTransformationTest.Test<ManagedStruct>();
	[Fact]
	public void StringTest() => GetTransformationTest.Test<String>();
	private static void Test<T>()
	{
		T value = FixedMemoryTestsBase.Fixture.Create<T>();
		FixedReferenceTestsBase.WithFixed(ref value, GetTransformationTest.Test);
		FixedReferenceTestsBase.WithFixed(ref value, GetTransformationTest.ReadOnlyTest);
	}
	private static unsafe void Test<T>(FixedReference<T> fref, IntPtr ptr)
	{
		try
		{
			GCHandle.Alloc(Array.Empty<T>(), GCHandleType.Pinned).Free();
#if !NETCOREAPP
			if (RuntimeHelpers.IsReferenceOrContainsReferences<T>())
				throw new ArgumentException(); // Required for Mono?
#endif
			GetTransformationTest.Test<T, Boolean>(fref);
			GetTransformationTest.Test<T, Byte>(fref);
			GetTransformationTest.Test<T, Int16>(fref);
			GetTransformationTest.Test<T, Char>(fref);
			GetTransformationTest.Test<T, Int32>(fref);
			GetTransformationTest.Test<T, Int64>(fref);
#if NET7_0_OR_GREATER
			GetTransformationTest.Test<T, Int128>(fref);
#endif
			GetTransformationTest.Test<T, Single>(fref);
#if NET5_0_OR_GREATER
			GetTransformationTest.Test<T, Half>(fref);
#endif
			GetTransformationTest.Test<T, Double>(fref);
			GetTransformationTest.Test<T, Decimal>(fref);
			GetTransformationTest.Test<T, DateTime>(fref);
#if NET6_0_OR_GREATER
			GetTransformationTest.Test<T, TimeOnly>(fref);
#endif
			GetTransformationTest.Test<T, TimeSpan>(fref);

			if (sizeof(T) < sizeof(ManagedStruct))
			{
				GetTransformationTest.Test<T, ManagedStruct>(fref);
				GetTransformationTest.Test<T, WrapperStruct<ManagedStruct>>(fref);
			}
			else
			{
				PInvokeAssert.Throws<InvalidOperationException>(() => GetTransformationTest
					                                                .Test<T, ManagedStruct>(fref));
				PInvokeAssert.Throws<InvalidOperationException>(() => GetTransformationTest
					                                                .Test<T, WrapperStruct<ManagedStruct>>(fref));
			}

			if (sizeof(T) < IntPtr.Size)
			{
				GetTransformationTest.Test<T, String>(fref);
				GetTransformationTest.Test<T, Object>(fref);
			}
			else
			{
				PInvokeAssert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, String>(fref));
				PInvokeAssert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, Object>(fref));
			}
		}
		catch (ArgumentException)
		{
			PInvokeAssert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, Boolean>(fref));
			PInvokeAssert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, Byte>(fref));
			PInvokeAssert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, Int16>(fref));
			PInvokeAssert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, Char>(fref));
			PInvokeAssert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, Int32>(fref));
			if (sizeof(T) < sizeof(Int64))
				GetTransformationTest.Test<T, Int64>(fref);
			else
				PInvokeAssert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, Int64>(fref));
#if NET7_0_OR_GREATER
			if (sizeof(T) < sizeof(Int128))
				GetTransformationTest.Test<T, Int128>(fref);
			else
				Assert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, Int128>(fref));
#endif
			PInvokeAssert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, Single>(fref));
#if NET5_0_OR_GREATER
			Assert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, Half>(fref));
#endif
			if (sizeof(T) < sizeof(Double))
				GetTransformationTest.Test<T, Double>(fref);
			else
				PInvokeAssert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, Double>(fref));
			if (sizeof(T) < sizeof(Decimal))
				GetTransformationTest.Test<T, Decimal>(fref);
			else
				PInvokeAssert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, Decimal>(fref));
			if (sizeof(T) < sizeof(DateTime))
				GetTransformationTest.Test<T, DateTime>(fref);
			else
				PInvokeAssert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, DateTime>(fref));
#if NET6_0_OR_GREATER
			if (sizeof(T) < sizeof(TimeOnly))
				GetTransformationTest.Test<T, TimeOnly>(fref);
			else
				Assert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, TimeOnly>(fref));
#endif
			if (sizeof(T) < sizeof(TimeSpan))
				GetTransformationTest.Test<T, TimeSpan>(fref);
			else
				PInvokeAssert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, TimeSpan>(fref));

			if (sizeof(T) < sizeof(ManagedStruct))
				GetTransformationTest.Test<T, WrapperStruct<ManagedStruct>>(fref);
			else
				PInvokeAssert.Throws<InvalidOperationException>(() => GetTransformationTest
					                                                .Test<T, WrapperStruct<ManagedStruct>>(fref));
			PInvokeAssert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, WrapperStruct<String>>(
				                                                fref));
			PInvokeAssert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, WrapperStruct<Object>>(
				                                                fref));

			if (typeof(T).IsValueType)
			{
				GetTransformationTest.Test<T, ManagedStruct>(fref);

				PInvokeAssert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, String>(fref));
				PInvokeAssert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, Object>(fref));
			}
			else
			{
				PInvokeAssert.Throws<InvalidOperationException>(() => GetTransformationTest
					                                                .Test<T, WrapperStruct<String>>(fref));
				PInvokeAssert.Throws<InvalidOperationException>(() => GetTransformationTest
					                                                .Test<T, WrapperStruct<Object>>(fref));
				GetTransformationTest.Test<T, String>(fref);
				GetTransformationTest.Test<T, Object>(fref);
			}
		}

		fref.Unload();
		PInvokeAssert.Equal(FixedMemoryTestsBase.InvalidError,
		                    PInvokeAssert
			                    .Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, Boolean>(
				                                                       fref, true)).Message);
		PInvokeAssert.Equal(FixedMemoryTestsBase.InvalidError,
		                    PInvokeAssert
			                    .Throws<InvalidOperationException>(() => GetTransformationTest
				                                                       .Test<T, Byte>(fref, true)).Message);
		PInvokeAssert.Equal(FixedMemoryTestsBase.InvalidError,
		                    PInvokeAssert
			                    .Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, Int16>(
				                                                       fref, true)).Message);
		PInvokeAssert.Equal(FixedMemoryTestsBase.InvalidError,
		                    PInvokeAssert
			                    .Throws<InvalidOperationException>(() => GetTransformationTest
				                                                       .Test<T, Char>(fref, true)).Message);
		PInvokeAssert.Equal(FixedMemoryTestsBase.InvalidError,
		                    PInvokeAssert
			                    .Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, Int32>(
				                                                       fref, true)).Message);
		PInvokeAssert.Equal(FixedMemoryTestsBase.InvalidError,
		                    PInvokeAssert
			                    .Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, Int64>(
				                                                       fref, true)).Message);
#if NET7_0_OR_GREATER
		Assert.Equal(FixedMemoryTestsBase.InvalidError,
		             Assert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, Int128>(fref, true))
		                   .Message);
#endif
		PInvokeAssert.Equal(FixedMemoryTestsBase.InvalidError,
		                    PInvokeAssert
			                    .Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, Single>(
				                                                       fref, true)).Message);
#if NET5_0_OR_GREATER
		Assert.Equal(FixedMemoryTestsBase.InvalidError,
		             Assert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, Half>(fref, true))
		                   .Message);
#endif
		PInvokeAssert.Equal(FixedMemoryTestsBase.InvalidError,
		                    PInvokeAssert
			                    .Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, Double>(
				                                                       fref, true)).Message);
		PInvokeAssert.Equal(FixedMemoryTestsBase.InvalidError,
		                    PInvokeAssert
			                    .Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, Decimal>(
				                                                       fref, true)).Message);
		PInvokeAssert.Equal(FixedMemoryTestsBase.InvalidError,
		                    PInvokeAssert
			                    .Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, DateTime>(
				                                                       fref, true)).Message);
#if NET6_0_OR_GREATER
		Assert.Equal(FixedMemoryTestsBase.InvalidError,
		             Assert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, TimeOnly>(fref, true))
		                   .Message);
#endif
		PInvokeAssert.Equal(FixedMemoryTestsBase.InvalidError,
		                    PInvokeAssert
			                    .Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, TimeSpan>(
				                                                       fref, true)).Message);
	}
	private static unsafe void ReadOnlyTest<T>(ReadOnlyFixedReference<T> fref, IntPtr ptr)
	{
		try
		{
			GCHandle.Alloc(Array.Empty<T>(), GCHandleType.Pinned).Free();
#if !NETCOREAPP
			if (RuntimeHelpers.IsReferenceOrContainsReferences<T>())
				throw new ArgumentException(); // Required for Mono?
#endif
			GetTransformationTest.Test<T, Boolean>(fref);
			GetTransformationTest.Test<T, Byte>(fref);
			GetTransformationTest.Test<T, Int16>(fref);
			GetTransformationTest.Test<T, Char>(fref);
			GetTransformationTest.Test<T, Int32>(fref);
			GetTransformationTest.Test<T, Int64>(fref);
#if NET7_0_OR_GREATER
			GetTransformationTest.Test<T, Int128>(fref);
#endif
			GetTransformationTest.Test<T, Single>(fref);
#if NET5_0_OR_GREATER
			GetTransformationTest.Test<T, Half>(fref);
#endif
			GetTransformationTest.Test<T, Double>(fref);
			GetTransformationTest.Test<T, Decimal>(fref);
			GetTransformationTest.Test<T, DateTime>(fref);
#if NET6_0_OR_GREATER
			GetTransformationTest.Test<T, TimeOnly>(fref);
#endif
			GetTransformationTest.Test<T, TimeSpan>(fref);

			if (sizeof(T) < sizeof(ManagedStruct))
			{
				GetTransformationTest.Test<T, ManagedStruct>(fref);
				GetTransformationTest.Test<T, WrapperStruct<ManagedStruct>>(fref);
			}
			else
			{
				PInvokeAssert.Throws<InvalidOperationException>(() => GetTransformationTest
					                                                .Test<T, ManagedStruct>(fref));
				PInvokeAssert.Throws<InvalidOperationException>(() => GetTransformationTest
					                                                .Test<T, WrapperStruct<ManagedStruct>>(fref));
			}

			if (sizeof(T) < IntPtr.Size)
			{
				GetTransformationTest.Test<T, String>(fref);
				GetTransformationTest.Test<T, Object>(fref);
			}
			else
			{
				PInvokeAssert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, String>(fref));
				PInvokeAssert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, Object>(fref));
			}
		}
		catch (ArgumentException)
		{
			PInvokeAssert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, Boolean>(fref));
			PInvokeAssert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, Byte>(fref));
			PInvokeAssert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, Int16>(fref));
			PInvokeAssert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, Char>(fref));
			PInvokeAssert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, Int32>(fref));
			if (sizeof(T) < sizeof(Int64))
				GetTransformationTest.Test<T, Int64>(fref);
			else
				PInvokeAssert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, Int64>(fref));
#if NET7_0_OR_GREATER
			if (sizeof(T) < sizeof(Int128))
				GetTransformationTest.Test<T, Int128>(fref);
			else
				Assert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, Int128>(fref));
#endif
			PInvokeAssert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, Single>(fref));
#if NET5_0_OR_GREATER
			Assert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, Half>(fref));
#endif
			if (sizeof(T) < sizeof(Double))
				GetTransformationTest.Test<T, Double>(fref);
			else
				PInvokeAssert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, Double>(fref));
			if (sizeof(T) < sizeof(Decimal))
				GetTransformationTest.Test<T, Decimal>(fref);
			else
				PInvokeAssert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, Decimal>(fref));
			if (sizeof(T) < sizeof(DateTime))
				GetTransformationTest.Test<T, DateTime>(fref);
			else
				PInvokeAssert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, DateTime>(fref));
#if NET6_0_OR_GREATER
			if (sizeof(T) < sizeof(TimeOnly))
				GetTransformationTest.Test<T, TimeOnly>(fref);
			else
				Assert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, TimeOnly>(fref));
#endif
			if (sizeof(T) < sizeof(TimeSpan))
				GetTransformationTest.Test<T, TimeSpan>(fref);
			else
				PInvokeAssert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, TimeSpan>(fref));

			if (sizeof(T) < sizeof(ManagedStruct))
				GetTransformationTest.Test<T, WrapperStruct<ManagedStruct>>(fref);
			else
				PInvokeAssert.Throws<InvalidOperationException>(() => GetTransformationTest
					                                                .Test<T, WrapperStruct<ManagedStruct>>(fref));
			PInvokeAssert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, WrapperStruct<String>>(
				                                                fref));
			PInvokeAssert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, WrapperStruct<Object>>(
				                                                fref));

			if (typeof(T).IsValueType)
			{
				GetTransformationTest.Test<T, ManagedStruct>(fref);

				PInvokeAssert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, String>(fref));
				PInvokeAssert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, Object>(fref));
			}
			else
			{
				PInvokeAssert.Throws<InvalidOperationException>(() => GetTransformationTest
					                                                .Test<T, WrapperStruct<String>>(fref));
				PInvokeAssert.Throws<InvalidOperationException>(() => GetTransformationTest
					                                                .Test<T, WrapperStruct<Object>>(fref));
				GetTransformationTest.Test<T, String>(fref);
				GetTransformationTest.Test<T, Object>(fref);
			}
		}

		fref.Unload();
		PInvokeAssert.Equal(FixedMemoryTestsBase.InvalidError,
		                    PInvokeAssert
			                    .Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, Boolean>(
				                                                       fref, true)).Message);
		PInvokeAssert.Equal(FixedMemoryTestsBase.InvalidError,
		                    PInvokeAssert
			                    .Throws<InvalidOperationException>(() => GetTransformationTest
				                                                       .Test<T, Byte>(fref, true)).Message);
		PInvokeAssert.Equal(FixedMemoryTestsBase.InvalidError,
		                    PInvokeAssert
			                    .Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, Int16>(
				                                                       fref, true)).Message);
		PInvokeAssert.Equal(FixedMemoryTestsBase.InvalidError,
		                    PInvokeAssert
			                    .Throws<InvalidOperationException>(() => GetTransformationTest
				                                                       .Test<T, Char>(fref, true)).Message);
		PInvokeAssert.Equal(FixedMemoryTestsBase.InvalidError,
		                    PInvokeAssert
			                    .Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, Int32>(
				                                                       fref, true)).Message);
		PInvokeAssert.Equal(FixedMemoryTestsBase.InvalidError,
		                    PInvokeAssert
			                    .Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, Int64>(
				                                                       fref, true)).Message);
#if NET7_0_OR_GREATER
		Assert.Equal(FixedMemoryTestsBase.InvalidError,
		             Assert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, Int128>(fref, true))
		                   .Message);
#endif
		PInvokeAssert.Equal(FixedMemoryTestsBase.InvalidError,
		                    PInvokeAssert
			                    .Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, Single>(
				                                                       fref, true)).Message);
#if NET5_0_OR_GREATER
		Assert.Equal(FixedMemoryTestsBase.InvalidError,
		             Assert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, Half>(fref, true))
		                   .Message);
#endif
		PInvokeAssert.Equal(FixedMemoryTestsBase.InvalidError,
		                    PInvokeAssert
			                    .Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, Double>(
				                                                       fref, true)).Message);
		PInvokeAssert.Equal(FixedMemoryTestsBase.InvalidError,
		                    PInvokeAssert
			                    .Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, Decimal>(
				                                                       fref, true)).Message);
		PInvokeAssert.Equal(FixedMemoryTestsBase.InvalidError,
		                    PInvokeAssert
			                    .Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, DateTime>(
				                                                       fref, true)).Message);
#if NET6_0_OR_GREATER
		Assert.Equal(FixedMemoryTestsBase.InvalidError,
		             Assert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, TimeOnly>(fref, true))
		                   .Message);
#endif
		PInvokeAssert.Equal(FixedMemoryTestsBase.InvalidError,
		                    PInvokeAssert
			                    .Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, TimeSpan>(
				                                                       fref, true)).Message);
	}
	private static unsafe void Test<T, T2>(FixedReference<T> fref, Boolean unloaded = false)
	{
		if (!unloaded && sizeof(T2) > fref.BinaryLength)
		{
			Exception invalidSize =
				PInvokeAssert.Throws<InsufficientMemoryException>(() => fref.CreateReadOnlyReference<T2>());
			PInvokeAssert.Equal(String.Format(FixedMemoryTestsBase.InvalidSizeFormat, typeof(T2)), invalidSize.Message);
			return;
		}

		FixedReference<T2> result = fref.GetTransformation<T2>(out FixedOffset offset, true);
		PInvokeAssert.NotNull(result);
		GetTransformationTest.ReferenceTest(fref, offset, result);
		FixedReference<T2> result2 = fref.GetTransformation<T2>(out FixedOffset offset2);
		PInvokeAssert.NotNull(result2);
		PInvokeAssert.Equal(offset, offset2);
		PInvokeAssert.Equal(result, result2);
	}
	private static unsafe void Test<T, T2>(ReadOnlyFixedReference<T> fref, Boolean unloaded = false)
	{
		if (!unloaded && sizeof(T2) > fref.BinaryLength)
		{
			Exception invalidSize =
				PInvokeAssert.Throws<InsufficientMemoryException>(() => fref.CreateReadOnlyReference<T2>());
			PInvokeAssert.Equal(String.Format(FixedMemoryTestsBase.InvalidSizeFormat, typeof(T2)), invalidSize.Message);
			return;
		}

		ReadOnlyFixedReference<T2> result = fref.GetTransformation<T2>(out ReadOnlyFixedOffset offset);
		PInvokeAssert.NotNull(result);
		GetTransformationTest.ReferenceTest(fref, offset, result);
	}
	private static unsafe void ReferenceTest<T, T2>(FixedReference<T> fref, FixedOffset offset,
		FixedReference<T2> result)
	{
		HashCode hashResidual = new();
		hashResidual.Add(new IntPtr(Unsafe.AsPointer(ref Unsafe.AsRef(in fref.CreateReadOnlyReference<T>()))));
		hashResidual.Add(sizeof(T2));
		hashResidual.Add(fref.BinaryLength);
		hashResidual.Add(false);

		PInvokeAssert.Equal(0, fref.BinaryOffset);
		PInvokeAssert.Equal(sizeof(T2), offset.BinaryOffset);
		PInvokeAssert.Equal(fref.BinaryLength, result.BinaryLength);
		PInvokeAssert.Equal(fref.BinaryLength, offset.BinaryLength + offset.BinaryOffset);
		PInvokeAssert.Equal(hashResidual.ToHashCode(), offset.GetHashCode());
		PInvokeAssert.False(offset.IsFunction);

		try
		{
			GCHandle.Alloc(Array.Empty<T>(), GCHandleType.Pinned).Free();
#if !NETCOREAPP
			if (RuntimeHelpers.IsReferenceOrContainsReferences<T>())
				throw new ArgumentException(); // Required for Mono?
#endif
		}
		catch (ArgumentException)
		{
			// Managed types
			PInvokeAssert.True(fref.CreateBinarySpan().IsEmpty);
			PInvokeAssert.True(fref.CreateReadOnlyBinarySpan().IsEmpty);
			PInvokeAssert.Equal(typeof(T).IsValueType || fref.IsNullOrEmpty, fref.CreateReadOnlyObjectSpan().IsEmpty);
			PInvokeAssert.Equal(typeof(T).IsValueType || fref.IsNullOrEmpty, fref.CreateObjectSpan().IsEmpty);
			return;
		}

		PInvokeAssert.Equal(fref.CreateReadOnlyBinarySpan()[offset.BinaryOffset..].ToArray(),
		                    offset.CreateReadOnlyBinarySpan().ToArray());
		PInvokeAssert.True(fref.CreateReadOnlyObjectSpan().IsEmpty);
		PInvokeAssert.True(offset.CreateReadOnlyObjectSpan().IsEmpty);
		PInvokeAssert.Equal(fref.CreateBinarySpan()[offset.BinaryOffset..].ToArray(),
		                    offset.CreateBinarySpan().ToArray());
		PInvokeAssert.True(fref.CreateObjectSpan().IsEmpty);
		PInvokeAssert.True(offset.CreateObjectSpan().IsEmpty);

		FixedOffset offset2;
		if (fref.BinaryLength >= sizeof(Boolean))
		{
			_ = fref.GetTransformation<Boolean>(out offset2, true);
			GetTransformationTest.OffsetTest<T2, Boolean>(offset, offset2);
		}
		if (fref.BinaryLength >= sizeof(Byte))
		{
			_ = fref.GetTransformation<Byte>(out offset2, true);
			GetTransformationTest.OffsetTest<T2, Byte>(offset, offset2);
		}
		if (fref.BinaryLength >= sizeof(Int16))
		{
			_ = fref.GetTransformation<Int16>(out offset2, true);
			GetTransformationTest.OffsetTest<T2, Int16>(offset, offset2);
		}
		if (fref.BinaryLength >= sizeof(Char))
		{
			_ = fref.GetTransformation<Char>(out offset2, true);
			GetTransformationTest.OffsetTest<T2, Char>(offset, offset2);
		}
		if (fref.BinaryLength >= sizeof(Int32))
		{
			_ = fref.GetTransformation<Int32>(out offset2, true);
			GetTransformationTest.OffsetTest<T2, Int32>(offset, offset2);
		}
		if (fref.BinaryLength >= sizeof(Int64))
		{
			_ = fref.GetTransformation<Int64>(out offset2, true);
			GetTransformationTest.OffsetTest<T2, Int64>(offset, offset2);
		}
#if NET7_0_OR_GREATER
		if (fref.BinaryLength >= sizeof(Int128))
		{
			_ = fref.GetTransformation<Int128>(out offset2, true);
			GetTransformationTest.OffsetTest<T2, Int128>(offset, offset2);
		}
#endif
		if (fref.BinaryLength >= sizeof(Single))
		{
			_ = fref.GetTransformation<Single>(out offset2, true);
			GetTransformationTest.OffsetTest<T2, Single>(offset, offset2);
		}
#if NET5_0_OR_GREATER
		if (fref.BinaryLength >= sizeof(Half))
		{
			_ = fref.GetTransformation<Half>(out offset2, true);
			GetTransformationTest.OffsetTest<T2, Half>(offset, offset2);
		}
#endif
		if (fref.BinaryLength >= sizeof(Double))
		{
			_ = fref.GetTransformation<Double>(out offset2, true);
			GetTransformationTest.OffsetTest<T2, Double>(offset, offset2);
		}
		if (fref.BinaryLength >= sizeof(Decimal))
		{
			_ = fref.GetTransformation<Decimal>(out offset2, true);
			GetTransformationTest.OffsetTest<T2, Decimal>(offset, offset2);
		}
		if (fref.BinaryLength >= sizeof(DateTime))
		{
			_ = fref.GetTransformation<DateTime>(out offset2, true);
			GetTransformationTest.OffsetTest<T2, DateTime>(offset, offset2);
		}
#if NET6_0_OR_GREATER
		if (fref.BinaryLength >= sizeof(TimeOnly))
		{
			_ = fref.GetTransformation<TimeOnly>(out offset2, true);
			GetTransformationTest.OffsetTest<T2, TimeOnly>(offset, offset2);
		}
#endif
		if (fref.BinaryLength >= sizeof(TimeSpan))
		{
			_ = fref.GetTransformation<TimeSpan>(out offset2, true);
			GetTransformationTest.OffsetTest<T2, TimeSpan>(offset, offset2);
		}

		Exception functionException = PInvokeAssert.Throws<InvalidOperationException>(offset.CreateDelegate<Action>);
		PInvokeAssert.Equal(FixedMemoryTestsBase.IsNotFunction, functionException.Message);
	}
	private static unsafe void ReferenceTest<T, T2>(ReadOnlyFixedReference<T> fref, ReadOnlyFixedOffset offset,
		ReadOnlyFixedReference<T2> result)
	{
		HashCode hashResidual = new();
		hashResidual.Add(new IntPtr(Unsafe.AsPointer(ref Unsafe.AsRef(in fref.CreateReadOnlyReference<T>()))));
		hashResidual.Add(sizeof(T2));
		hashResidual.Add(fref.BinaryLength);
		hashResidual.Add(true);

		PInvokeAssert.Equal(0, fref.BinaryOffset);
		PInvokeAssert.Equal(sizeof(T2), offset.BinaryOffset);
		PInvokeAssert.Equal(fref.BinaryLength, result.BinaryLength);
		PInvokeAssert.Equal(fref.BinaryLength, offset.BinaryLength + offset.BinaryOffset);
		PInvokeAssert.Equal(hashResidual.ToHashCode(), offset.GetHashCode());
		PInvokeAssert.False(offset.IsFunction);

		Exception functionException = PInvokeAssert.Throws<InvalidOperationException>(offset.CreateDelegate<Action>);
		PInvokeAssert.Equal(FixedMemoryTestsBase.IsNotFunction, functionException.Message);

		try
		{
			GCHandle.Alloc(Array.Empty<T>(), GCHandleType.Pinned).Free();
#if !NETCOREAPP
			if (RuntimeHelpers.IsReferenceOrContainsReferences<T>())
				throw new ArgumentException(); // Required for Mono?
#endif
		}
		catch (ArgumentException)
		{
			// Managed types
			PInvokeAssert.True(fref.CreateReadOnlyBinarySpan().IsEmpty);
			PInvokeAssert.Equal(typeof(T).IsValueType || fref.IsNullOrEmpty, fref.CreateReadOnlyObjectSpan().IsEmpty);
			return;
		}
		PInvokeAssert.Equal(fref.CreateReadOnlyBinarySpan()[offset.BinaryOffset..].ToArray(),
		                    offset.CreateReadOnlyBinarySpan().ToArray());
		PInvokeAssert.True(fref.CreateReadOnlyObjectSpan().IsEmpty);

		ReadOnlyFixedOffset offset2;
		if (fref.BinaryLength >= sizeof(Boolean))
		{
			_ = fref.GetTransformation<Boolean>(out offset2);
			GetTransformationTest.OffsetTest<T2, Boolean>(offset, offset2);
		}
		if (fref.BinaryLength >= sizeof(Byte))
		{
			_ = fref.GetTransformation<Byte>(out offset2);
			GetTransformationTest.OffsetTest<T2, Byte>(offset, offset2);
		}
		if (fref.BinaryLength >= sizeof(Int16))
		{
			_ = fref.GetTransformation<Int16>(out offset2);
			GetTransformationTest.OffsetTest<T2, Int16>(offset, offset2);
		}
		if (fref.BinaryLength >= sizeof(Char))
		{
			_ = fref.GetTransformation<Char>(out offset2);
			GetTransformationTest.OffsetTest<T2, Char>(offset, offset2);
		}
		if (fref.BinaryLength >= sizeof(Int32))
		{
			_ = fref.GetTransformation<Int32>(out offset2);
			GetTransformationTest.OffsetTest<T2, Int32>(offset, offset2);
		}
		if (fref.BinaryLength >= sizeof(Int64))
		{
			_ = fref.GetTransformation<Int64>(out offset2);
			GetTransformationTest.OffsetTest<T2, Int64>(offset, offset2);
		}
#if NET7_0_OR_GREATER
		if (fref.BinaryLength >= sizeof(Int128))
		{
			_ = fref.GetTransformation<Int128>(out offset2);
			GetTransformationTest.OffsetTest<T2, Int128>(offset, offset2);
		}
#endif
		if (fref.BinaryLength >= sizeof(Single))
		{
			_ = fref.GetTransformation<Single>(out offset2);
			GetTransformationTest.OffsetTest<T2, Single>(offset, offset2);
		}
#if NET5_0_OR_GREATER
		if (fref.BinaryLength >= sizeof(Half))
		{
			_ = fref.GetTransformation<Half>(out offset2);
			GetTransformationTest.OffsetTest<T2, Half>(offset, offset2);
		}
#endif
		if (fref.BinaryLength >= sizeof(Double))
		{
			_ = fref.GetTransformation<Double>(out offset2);
			GetTransformationTest.OffsetTest<T2, Double>(offset, offset2);
		}
		if (fref.BinaryLength >= sizeof(Decimal))
		{
			_ = fref.GetTransformation<Decimal>(out offset2);
			GetTransformationTest.OffsetTest<T2, Decimal>(offset, offset2);
		}
		if (fref.BinaryLength >= sizeof(DateTime))
		{
			_ = fref.GetTransformation<DateTime>(out offset2);
			GetTransformationTest.OffsetTest<T2, DateTime>(offset, offset2);
		}
#if NET6_0_OR_GREATER
		if (fref.BinaryLength >= sizeof(TimeOnly))
		{
			_ = fref.GetTransformation<TimeOnly>(out offset2);
			GetTransformationTest.OffsetTest<T2, TimeOnly>(offset, offset2);
		}
#endif
		if (fref.BinaryLength >= sizeof(TimeSpan))
		{
			_ = fref.GetTransformation<TimeSpan>(out offset2);
			GetTransformationTest.OffsetTest<T2, TimeSpan>(offset, offset2);
		}
	}
	private static unsafe void OffsetTest<T2, T3>(FixedOffset offset1, FixedOffset offset2)
	{
		Boolean equal = sizeof(T2) == sizeof(T3) || offset1.BinaryLength == offset2.BinaryLength;
		PInvokeAssert.Equal(equal, offset1.Equals(offset2));
		PInvokeAssert.Equal(equal, offset1.Equals((Object)offset2));
		PInvokeAssert.False(offset2.IsFunction);
		if (equal)
			PInvokeAssert.Equal(offset1.CreateReadOnlyBinarySpan().ToArray(),
			                    offset2.CreateReadOnlyBinarySpan().ToArray());

		Exception functionException = PInvokeAssert.Throws<InvalidOperationException>(offset2.CreateDelegate<Action>);
		PInvokeAssert.Equal(FixedMemoryTestsBase.IsNotFunction, functionException.Message);
	}
	private static unsafe void OffsetTest<T2, T3>(ReadOnlyFixedOffset offset1, ReadOnlyFixedOffset offset2)
	{
		Boolean equal = sizeof(T2) == sizeof(T3) || offset1.BinaryLength == offset2.BinaryLength;
		PInvokeAssert.Equal(equal, offset1.Equals(offset2));
		PInvokeAssert.Equal(equal, offset1.Equals((Object)offset2));
		PInvokeAssert.False(offset2.IsFunction);
		if (equal)
			PInvokeAssert.Equal(offset1.CreateReadOnlyBinarySpan().ToArray(),
			                    offset2.CreateReadOnlyBinarySpan().ToArray());

		Exception functionException = PInvokeAssert.Throws<InvalidOperationException>(offset2.CreateDelegate<Action>);
		PInvokeAssert.Equal(FixedMemoryTestsBase.IsNotFunction, functionException.Message);
	}
}
#pragma warning restore CS8500