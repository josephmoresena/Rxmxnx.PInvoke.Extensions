﻿namespace Rxmxnx.PInvoke.Tests.Internal.FixedReferenceTests;

[ExcludeFromCodeCoverage]
[SuppressMessage("csharpsquid", "S2699")]
public sealed class GetTransformationTest : FixedReferenceTestsBase
{
	[Fact]
	internal void BooleanTest() => this.Test<Boolean>();
	[Fact]
	internal void ByteTest() => this.Test<Byte>();
	[Fact]
	internal void Int16Test() => this.Test<Int16>();
	[Fact]
	internal void CharTest() => this.Test<Char>();
	[Fact]
	internal void Int32Test() => this.Test<Int32>();
	[Fact]
	internal void Int64Test() => this.Test<Int64>();
	[Fact]
	internal void Int128Test() => this.Test<Int128>();
	[Fact]
	internal void GuidTest() => this.Test<Guid>();
	[Fact]
	internal void SingleTest() => this.Test<Single>();
	[Fact]
	internal void HalfTest() => this.Test<Half>();
	[Fact]
	internal void DoubleTest() => this.Test<Double>();
	[Fact]
	internal void DecimalTest() => this.Test<Decimal>();
	[Fact]
	internal void DateTimeTest() => this.Test<DateTime>();
	[Fact]
	internal void TimeOnlyTest() => this.Test<TimeOnly>();
	[Fact]
	internal void TimeSpanTest() => this.Test<TimeSpan>();

	private void Test<T>() where T : unmanaged
	{
		T value = FixedMemoryTestsBase.fixture.Create<T>();
		this.WithFixed(ref value, GetTransformationTest.Test);
		this.WithFixed(ref value, GetTransformationTest.ReadOnlyTest);
	}

	private static void Test<T>(FixedReference<T> fref, IntPtr ptr) where T : unmanaged
	{
		Boolean isReadOnly = fref.IsReadOnly;

		GetTransformationTest.Test<T, Boolean>(fref);
		GetTransformationTest.Test<T, Byte>(fref);
		GetTransformationTest.Test<T, Int16>(fref);
		GetTransformationTest.Test<T, Char>(fref);
		GetTransformationTest.Test<T, Int32>(fref);
		GetTransformationTest.Test<T, Int64>(fref);
		GetTransformationTest.Test<T, Int128>(fref);
		GetTransformationTest.Test<T, Single>(fref);
		GetTransformationTest.Test<T, Half>(fref);
		GetTransformationTest.Test<T, Double>(fref);
		GetTransformationTest.Test<T, Decimal>(fref);
		GetTransformationTest.Test<T, DateTime>(fref);
		GetTransformationTest.Test<T, TimeOnly>(fref);
		GetTransformationTest.Test<T, TimeSpan>(fref);

		fref.Unload();
		Assert.Equal(FixedMemoryTestsBase.InvalidError,
		             Assert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, Boolean>(fref, true))
		                   .Message);
		Assert.Equal(FixedMemoryTestsBase.InvalidError,
		             Assert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, Byte>(fref, true))
		                   .Message);
		Assert.Equal(FixedMemoryTestsBase.InvalidError,
		             Assert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, Int16>(fref, true))
		                   .Message);
		Assert.Equal(FixedMemoryTestsBase.InvalidError,
		             Assert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, Char>(fref, true))
		                   .Message);
		Assert.Equal(FixedMemoryTestsBase.InvalidError,
		             Assert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, Int32>(fref, true))
		                   .Message);
		Assert.Equal(FixedMemoryTestsBase.InvalidError,
		             Assert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, Int64>(fref, true))
		                   .Message);
		Assert.Equal(FixedMemoryTestsBase.InvalidError,
		             Assert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, Int128>(fref, true))
		                   .Message);
		Assert.Equal(FixedMemoryTestsBase.InvalidError,
		             Assert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, Single>(fref, true))
		                   .Message);
		Assert.Equal(FixedMemoryTestsBase.InvalidError,
		             Assert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, Half>(fref, true))
		                   .Message);
		Assert.Equal(FixedMemoryTestsBase.InvalidError,
		             Assert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, Double>(fref, true))
		                   .Message);
		Assert.Equal(FixedMemoryTestsBase.InvalidError,
		             Assert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, Decimal>(fref, true))
		                   .Message);
		Assert.Equal(FixedMemoryTestsBase.InvalidError,
		             Assert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, DateTime>(fref, true))
		                   .Message);
		Assert.Equal(FixedMemoryTestsBase.InvalidError,
		             Assert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, TimeOnly>(fref, true))
		                   .Message);
		Assert.Equal(FixedMemoryTestsBase.InvalidError,
		             Assert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, TimeSpan>(fref, true))
		                   .Message);
	}
	private static void ReadOnlyTest<T>(ReadOnlyFixedReference<T> fref, IntPtr ptr) where T : unmanaged
	{
		Boolean isReadOnly = fref.IsReadOnly;

		GetTransformationTest.Test<T, Boolean>(fref);
		GetTransformationTest.Test<T, Byte>(fref);
		GetTransformationTest.Test<T, Int16>(fref);
		GetTransformationTest.Test<T, Char>(fref);
		GetTransformationTest.Test<T, Int32>(fref);
		GetTransformationTest.Test<T, Int64>(fref);
		GetTransformationTest.Test<T, Int128>(fref);
		GetTransformationTest.Test<T, Single>(fref);
		GetTransformationTest.Test<T, Half>(fref);
		GetTransformationTest.Test<T, Double>(fref);
		GetTransformationTest.Test<T, Decimal>(fref);
		GetTransformationTest.Test<T, DateTime>(fref);
		GetTransformationTest.Test<T, TimeOnly>(fref);
		GetTransformationTest.Test<T, TimeSpan>(fref);

		fref.Unload();
		Assert.Equal(FixedMemoryTestsBase.InvalidError,
		             Assert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, Boolean>(fref, true))
		                   .Message);
		Assert.Equal(FixedMemoryTestsBase.InvalidError,
		             Assert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, Byte>(fref, true))
		                   .Message);
		Assert.Equal(FixedMemoryTestsBase.InvalidError,
		             Assert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, Int16>(fref, true))
		                   .Message);
		Assert.Equal(FixedMemoryTestsBase.InvalidError,
		             Assert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, Char>(fref, true))
		                   .Message);
		Assert.Equal(FixedMemoryTestsBase.InvalidError,
		             Assert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, Int32>(fref, true))
		                   .Message);
		Assert.Equal(FixedMemoryTestsBase.InvalidError,
		             Assert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, Int64>(fref, true))
		                   .Message);
		Assert.Equal(FixedMemoryTestsBase.InvalidError,
		             Assert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, Int128>(fref, true))
		                   .Message);
		Assert.Equal(FixedMemoryTestsBase.InvalidError,
		             Assert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, Single>(fref, true))
		                   .Message);
		Assert.Equal(FixedMemoryTestsBase.InvalidError,
		             Assert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, Half>(fref, true))
		                   .Message);
		Assert.Equal(FixedMemoryTestsBase.InvalidError,
		             Assert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, Double>(fref, true))
		                   .Message);
		Assert.Equal(FixedMemoryTestsBase.InvalidError,
		             Assert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, Decimal>(fref, true))
		                   .Message);
		Assert.Equal(FixedMemoryTestsBase.InvalidError,
		             Assert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, DateTime>(fref, true))
		                   .Message);
		Assert.Equal(FixedMemoryTestsBase.InvalidError,
		             Assert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, TimeOnly>(fref, true))
		                   .Message);
		Assert.Equal(FixedMemoryTestsBase.InvalidError,
		             Assert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, TimeSpan>(fref, true))
		                   .Message);
	}
	private static unsafe void Test<T, T2>(FixedReference<T> fref, Boolean unloaded = false)
		where T : unmanaged where T2 : unmanaged
	{
		if (!unloaded && sizeof(T2) > fref.BinaryLength)
		{
			Exception invalidSize =
				Assert.Throws<InsufficientMemoryException>(() => fref.CreateReadOnlyReference<T2>());
			Assert.Equal(String.Format(FixedMemoryTestsBase.InvalidSizeFormat, typeof(T2)), invalidSize.Message);
			return;
		}

		FixedReference<T2> result = fref.GetTransformation<T2>(out FixedOffset offset, true);
		Assert.NotNull(result);
		GetTransformationTest.ReferenceTest(fref, offset, result);
		FixedReference<T2> result2 = fref.GetTransformation<T2>(out FixedOffset offset2);
		Assert.NotNull(result2);
		Assert.Equal(offset, offset2);
		Assert.Equal(result, result2);
	}
	private static unsafe void Test<T, T2>(ReadOnlyFixedReference<T> fref, Boolean unloaded = false)
		where T : unmanaged where T2 : unmanaged
	{
		if (!unloaded && sizeof(T2) > fref.BinaryLength)
		{
			Exception invalidSize =
				Assert.Throws<InsufficientMemoryException>(() => fref.CreateReadOnlyReference<T2>());
			Assert.Equal(String.Format(FixedMemoryTestsBase.InvalidSizeFormat, typeof(T2)), invalidSize.Message);
			return;
		}

		ReadOnlyFixedReference<T2> result = fref.GetTransformation<T2>(out ReadOnlyFixedOffset offset);
		Assert.NotNull(result);
		GetTransformationTest.ReferenceTest(fref, offset, result);
	}
	private static unsafe void ReferenceTest<T, T2>(FixedReference<T> fref, FixedOffset offset,
		FixedReference<T2> result) where T : unmanaged where T2 : unmanaged
	{
		HashCode hashResidual = new();
		hashResidual.Add(new IntPtr(Unsafe.AsPointer(ref UnsafeLegacy.AsRef(in fref.CreateReadOnlyReference<Byte>()))));
		hashResidual.Add(sizeof(T2));
		hashResidual.Add(fref.BinaryLength);
		hashResidual.Add(false);

		Assert.Equal(0, fref.BinaryOffset);
		Assert.Equal(sizeof(T2), offset.BinaryOffset);
		Assert.Equal(fref.CreateReadOnlyBinarySpan()[offset.BinaryOffset..].ToArray(),
		             offset.CreateReadOnlyBinarySpan().ToArray());
		Assert.Equal(fref.BinaryLength, result.BinaryLength);
		Assert.Equal(fref.BinaryLength, offset.BinaryLength + offset.BinaryOffset);
		Assert.Equal(hashResidual.ToHashCode(), offset.GetHashCode());
		Assert.False(offset.IsFunction);

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
		if (fref.BinaryLength >= sizeof(Int128))
		{
			_ = fref.GetTransformation<Int128>(out offset2, true);
			GetTransformationTest.OffsetTest<T2, Int128>(offset, offset2);
		}
		if (fref.BinaryLength >= sizeof(Single))
		{
			_ = fref.GetTransformation<Single>(out offset2, true);
			GetTransformationTest.OffsetTest<T2, Single>(offset, offset2);
		}
		if (fref.BinaryLength >= sizeof(Half))
		{
			_ = fref.GetTransformation<Half>(out offset2, true);
			GetTransformationTest.OffsetTest<T2, Half>(offset, offset2);
		}
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
		if (fref.BinaryLength >= sizeof(TimeOnly))
		{
			_ = fref.GetTransformation<TimeOnly>(out offset2, true);
			GetTransformationTest.OffsetTest<T2, TimeOnly>(offset, offset2);
		}
		if (fref.BinaryLength >= sizeof(TimeSpan))
		{
			_ = fref.GetTransformation<TimeSpan>(out offset2, true);
			GetTransformationTest.OffsetTest<T2, TimeSpan>(offset, offset2);
		}

		Exception functionException = Assert.Throws<InvalidOperationException>(() => offset.CreateDelegate<Action>());
		Assert.Equal(FixedMemoryTestsBase.IsNotFunction, functionException.Message);
	}
	private static unsafe void ReferenceTest<T, T2>(ReadOnlyFixedReference<T> fref, ReadOnlyFixedOffset offset,
		ReadOnlyFixedReference<T2> result) where T : unmanaged where T2 : unmanaged
	{
		HashCode hashResidual = new();
		hashResidual.Add(new IntPtr(Unsafe.AsPointer(ref UnsafeLegacy.AsRef(in fref.CreateReadOnlyReference<Byte>()))));
		hashResidual.Add(sizeof(T2));
		hashResidual.Add(fref.BinaryLength);
		hashResidual.Add(true);

		Assert.Equal(0, fref.BinaryOffset);
		Assert.Equal(sizeof(T2), offset.BinaryOffset);
		Assert.Equal(fref.CreateReadOnlyBinarySpan()[offset.BinaryOffset..].ToArray(),
		             offset.CreateReadOnlyBinarySpan().ToArray());
		Assert.Equal(fref.BinaryLength, result.BinaryLength);
		Assert.Equal(fref.BinaryLength, offset.BinaryLength + offset.BinaryOffset);
		Assert.Equal(hashResidual.ToHashCode(), offset.GetHashCode());
		Assert.False(offset.IsFunction);

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
		if (fref.BinaryLength >= sizeof(Int128))
		{
			_ = fref.GetTransformation<Int128>(out offset2);
			GetTransformationTest.OffsetTest<T2, Int128>(offset, offset2);
		}
		if (fref.BinaryLength >= sizeof(Single))
		{
			_ = fref.GetTransformation<Single>(out offset2);
			GetTransformationTest.OffsetTest<T2, Single>(offset, offset2);
		}
		if (fref.BinaryLength >= sizeof(Half))
		{
			_ = fref.GetTransformation<Half>(out offset2);
			GetTransformationTest.OffsetTest<T2, Half>(offset, offset2);
		}
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
		if (fref.BinaryLength >= sizeof(TimeOnly))
		{
			_ = fref.GetTransformation<TimeOnly>(out offset2);
			GetTransformationTest.OffsetTest<T2, TimeOnly>(offset, offset2);
		}
		if (fref.BinaryLength >= sizeof(TimeSpan))
		{
			_ = fref.GetTransformation<TimeSpan>(out offset2);
			GetTransformationTest.OffsetTest<T2, TimeSpan>(offset, offset2);
		}

		Exception functionException = Assert.Throws<InvalidOperationException>(() => offset.CreateDelegate<Action>());
		Assert.Equal(FixedMemoryTestsBase.IsNotFunction, functionException.Message);
	}
	private static unsafe void OffsetTest<T2, T3>(FixedOffset offset1, FixedOffset offset2)
		where T2 : unmanaged where T3 : unmanaged
	{
		Boolean equal = sizeof(T2) == sizeof(T3) || offset1.BinaryLength == offset2.BinaryLength;
		Assert.Equal(equal, offset1.Equals(offset2));
		Assert.Equal(equal, offset1.Equals((Object)offset2));
		Assert.False(offset2.IsFunction);
		if (equal)
			Assert.Equal(offset1.CreateReadOnlyBinarySpan().ToArray(), offset2.CreateReadOnlyBinarySpan().ToArray());

		Exception functionException = Assert.Throws<InvalidOperationException>(() => offset2.CreateDelegate<Action>());
		Assert.Equal(FixedMemoryTestsBase.IsNotFunction, functionException.Message);
	}
	private static unsafe void OffsetTest<T2, T3>(ReadOnlyFixedOffset offset1, ReadOnlyFixedOffset offset2)
		where T2 : unmanaged where T3 : unmanaged
	{
		Boolean equal = sizeof(T2) == sizeof(T3) || offset1.BinaryLength == offset2.BinaryLength;
		Assert.Equal(equal, offset1.Equals(offset2));
		Assert.Equal(equal, offset1.Equals((Object)offset2));
		Assert.False(offset2.IsFunction);
		if (equal)
			Assert.Equal(offset1.CreateReadOnlyBinarySpan().ToArray(), offset2.CreateReadOnlyBinarySpan().ToArray());

		Exception functionException = Assert.Throws<InvalidOperationException>(() => offset2.CreateDelegate<Action>());
		Assert.Equal(FixedMemoryTestsBase.IsNotFunction, functionException.Message);
	}
}