namespace Rxmxnx.PInvoke.Tests.Internal.FixedContextTests;

[ExcludeFromCodeCoverage]
[SuppressMessage("csharpsquid", "S2699")]
#pragma warning disable CS8500
public sealed class GetTransformationTest : FixedContextTestsBase
{
	[Fact]
	internal void BooleanTest() => GetTransformationTest.Test<Boolean>();
	[Fact]
	internal void ByteTest() => GetTransformationTest.Test<Byte>();
	[Fact]
	internal void Int16Test() => GetTransformationTest.Test<Int16>();
	[Fact]
	internal void CharTest() => GetTransformationTest.Test<Char>();
	[Fact]
	internal void Int32Test() => GetTransformationTest.Test<Int32>();
	[Fact]
	internal void Int64Test() => GetTransformationTest.Test<Int64>();
	[Fact]
	internal void Int128Test() => GetTransformationTest.Test<Int128>();
	[Fact]
	internal void GuidTest() => GetTransformationTest.Test<Guid>();
	[Fact]
	internal void SingleTest() => GetTransformationTest.Test<Single>();
	[Fact]
	internal void HalfTest() => GetTransformationTest.Test<Half>();
	[Fact]
	internal void DoubleTest() => GetTransformationTest.Test<Double>();
	[Fact]
	internal void DecimalTest() => GetTransformationTest.Test<Decimal>();
	[Fact]
	internal void DateTimeTest() => GetTransformationTest.Test<DateTime>();
	[Fact]
	internal void TimeOnlyTest() => GetTransformationTest.Test<TimeOnly>();
	[Fact]
	internal void TimeSpanTest() => GetTransformationTest.Test<TimeSpan>();
	[Fact]
	internal void ManagedStructTest() => GetTransformationTest.Test<ManagedStruct>();
	[Fact]
	internal void StringTest() => GetTransformationTest.Test<String>();
	private static void Test<T>()
	{
		T[] values = FixedMemoryTestsBase.Fixture.CreateMany<T>().ToArray();
		FixedContextTestsBase.WithFixed(values, GetTransformationTest.Test);
		FixedContextTestsBase.WithFixed<T>(values, GetTransformationTest.ReadOnlyTest);
	}
	private static void Test<T>(FixedContext<T> ctx, T[] arr)
	{
		try
		{
			GCHandle.Alloc(arr, GCHandleType.Pinned).Free();
			GetTransformationTest.Test<T, Boolean>(ctx);
			GetTransformationTest.Test<T, Byte>(ctx);
			GetTransformationTest.Test<T, Int16>(ctx);
			GetTransformationTest.Test<T, Char>(ctx);
			GetTransformationTest.Test<T, Int32>(ctx);
			GetTransformationTest.Test<T, Int64>(ctx);
			GetTransformationTest.Test<T, Int128>(ctx);
			GetTransformationTest.Test<T, Single>(ctx);
			GetTransformationTest.Test<T, Half>(ctx);
			GetTransformationTest.Test<T, Double>(ctx);
			GetTransformationTest.Test<T, Decimal>(ctx);
			GetTransformationTest.Test<T, DateTime>(ctx);
			GetTransformationTest.Test<T, TimeOnly>(ctx);
			GetTransformationTest.Test<T, TimeSpan>(ctx);

			Assert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, ManagedStruct>(ctx));
			Assert.Throws<InvalidOperationException>(
				() => GetTransformationTest.Test<T, WrapperStruct<ManagedStruct>>(ctx));
			Assert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, String>(ctx));
			Assert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, Object>(ctx));
		}
		catch (Exception)
		{
			Assert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, Boolean>(ctx));
			Assert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, Byte>(ctx));
			Assert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, Int16>(ctx));
			Assert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, Char>(ctx));
			Assert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, Int32>(ctx));
			Assert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, Int64>(ctx));
			Assert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, Int128>(ctx));
			Assert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, Single>(ctx));
			Assert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, Half>(ctx));
			Assert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, Double>(ctx));
			Assert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, Decimal>(ctx));
			Assert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, DateTime>(ctx));
			Assert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, TimeOnly>(ctx));
			Assert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, TimeSpan>(ctx));

			Assert.Throws<InvalidOperationException>(
				() => GetTransformationTest.Test<T, WrapperStruct<ManagedStruct>>(ctx));
			Assert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, WrapperStruct<String>>(ctx));
			Assert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, WrapperStruct<Object>>(ctx));

			if (typeof(T).IsValueType)
			{
				GetTransformationTest.Test<T, ManagedStruct>(ctx);

				Assert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, String>(ctx));
				Assert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, Object>(ctx));
			}
			else
			{
				Assert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, ManagedStruct>(ctx));
				GetTransformationTest.Test<T, String>(ctx);
				GetTransformationTest.Test<T, Object>(ctx);
			}
		}

		ctx.Unload();
		Assert.Equal(FixedMemoryTestsBase.InvalidError,
		             Assert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, Boolean>(ctx))
		                   .Message);
		Assert.Equal(FixedMemoryTestsBase.InvalidError,
		             Assert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, Byte>(ctx)).Message);
		Assert.Equal(FixedMemoryTestsBase.InvalidError,
		             Assert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, Int16>(ctx)).Message);
		Assert.Equal(FixedMemoryTestsBase.InvalidError,
		             Assert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, Char>(ctx)).Message);
		Assert.Equal(FixedMemoryTestsBase.InvalidError,
		             Assert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, Int32>(ctx)).Message);
		Assert.Equal(FixedMemoryTestsBase.InvalidError,
		             Assert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, Int64>(ctx)).Message);
		Assert.Equal(FixedMemoryTestsBase.InvalidError,
		             Assert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, Int128>(ctx))
		                   .Message);
		Assert.Equal(FixedMemoryTestsBase.InvalidError,
		             Assert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, Single>(ctx))
		                   .Message);
		Assert.Equal(FixedMemoryTestsBase.InvalidError,
		             Assert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, Half>(ctx)).Message);
		Assert.Equal(FixedMemoryTestsBase.InvalidError,
		             Assert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, Double>(ctx))
		                   .Message);
		Assert.Equal(FixedMemoryTestsBase.InvalidError,
		             Assert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, Decimal>(ctx))
		                   .Message);
		Assert.Equal(FixedMemoryTestsBase.InvalidError,
		             Assert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, DateTime>(ctx))
		                   .Message);
		Assert.Equal(FixedMemoryTestsBase.InvalidError,
		             Assert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, TimeOnly>(ctx))
		                   .Message);
		Assert.Equal(FixedMemoryTestsBase.InvalidError,
		             Assert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, TimeSpan>(ctx))
		                   .Message);
	}
	private static void ReadOnlyTest<T>(ReadOnlyFixedContext<T> ctx, T[] arr)
	{
		try
		{
			GCHandle.Alloc(arr, GCHandleType.Pinned).Free();
			GetTransformationTest.Test<T, Boolean>(ctx);
			GetTransformationTest.Test<T, Byte>(ctx);
			GetTransformationTest.Test<T, Int16>(ctx);
			GetTransformationTest.Test<T, Char>(ctx);
			GetTransformationTest.Test<T, Int32>(ctx);
			GetTransformationTest.Test<T, Int64>(ctx);
			GetTransformationTest.Test<T, Int128>(ctx);
			GetTransformationTest.Test<T, Single>(ctx);
			GetTransformationTest.Test<T, Half>(ctx);
			GetTransformationTest.Test<T, Double>(ctx);
			GetTransformationTest.Test<T, Decimal>(ctx);
			GetTransformationTest.Test<T, DateTime>(ctx);
			GetTransformationTest.Test<T, TimeOnly>(ctx);
			GetTransformationTest.Test<T, TimeSpan>(ctx);

			Assert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, ManagedStruct>(ctx));
			Assert.Throws<InvalidOperationException>(
				() => GetTransformationTest.Test<T, WrapperStruct<ManagedStruct>>(ctx));
			Assert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, String>(ctx));
			Assert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, Object>(ctx));
		}
		catch (Exception)
		{
			Assert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, Boolean>(ctx));
			Assert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, Byte>(ctx));
			Assert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, Int16>(ctx));
			Assert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, Char>(ctx));
			Assert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, Int32>(ctx));
			Assert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, Int64>(ctx));
			Assert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, Int128>(ctx));
			Assert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, Single>(ctx));
			Assert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, Half>(ctx));
			Assert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, Double>(ctx));
			Assert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, Decimal>(ctx));
			Assert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, DateTime>(ctx));
			Assert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, TimeOnly>(ctx));
			Assert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, TimeSpan>(ctx));

			Assert.Throws<InvalidOperationException>(
				() => GetTransformationTest.Test<T, WrapperStruct<ManagedStruct>>(ctx));
			Assert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, WrapperStruct<String>>(ctx));
			Assert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, WrapperStruct<Object>>(ctx));

			if (typeof(T).IsValueType)
			{
				GetTransformationTest.Test<T, ManagedStruct>(ctx);

				Assert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, String>(ctx));
				Assert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, Object>(ctx));
			}
			else
			{
				Assert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, ManagedStruct>(ctx));
				GetTransformationTest.Test<T, String>(ctx);
				GetTransformationTest.Test<T, Object>(ctx);
			}
		}

		ctx.Unload();
		Assert.Equal(FixedMemoryTestsBase.InvalidError,
		             Assert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, Boolean>(ctx))
		                   .Message);
		Assert.Equal(FixedMemoryTestsBase.InvalidError,
		             Assert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, Byte>(ctx)).Message);
		Assert.Equal(FixedMemoryTestsBase.InvalidError,
		             Assert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, Int16>(ctx)).Message);
		Assert.Equal(FixedMemoryTestsBase.InvalidError,
		             Assert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, Char>(ctx)).Message);
		Assert.Equal(FixedMemoryTestsBase.InvalidError,
		             Assert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, Int32>(ctx)).Message);
		Assert.Equal(FixedMemoryTestsBase.InvalidError,
		             Assert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, Int64>(ctx)).Message);
		Assert.Equal(FixedMemoryTestsBase.InvalidError,
		             Assert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, Int128>(ctx))
		                   .Message);
		Assert.Equal(FixedMemoryTestsBase.InvalidError,
		             Assert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, Single>(ctx))
		                   .Message);
		Assert.Equal(FixedMemoryTestsBase.InvalidError,
		             Assert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, Half>(ctx)).Message);
		Assert.Equal(FixedMemoryTestsBase.InvalidError,
		             Assert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, Double>(ctx))
		                   .Message);
		Assert.Equal(FixedMemoryTestsBase.InvalidError,
		             Assert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, Decimal>(ctx))
		                   .Message);
		Assert.Equal(FixedMemoryTestsBase.InvalidError,
		             Assert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, DateTime>(ctx))
		                   .Message);
		Assert.Equal(FixedMemoryTestsBase.InvalidError,
		             Assert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, TimeOnly>(ctx))
		                   .Message);
		Assert.Equal(FixedMemoryTestsBase.InvalidError,
		             Assert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, TimeSpan>(ctx))
		                   .Message);
	}
	private static void Test<T, T2>(FixedContext<T> ctx)
	{
		FixedContext<T2> result = ctx.GetTransformation<T2>(out FixedOffset offset, true);
		Assert.NotNull(result);
		GetTransformationTest.ContextTest(ctx, offset, result);

		FixedContext<T2> result2 = ctx.GetTransformation<T2>(out FixedOffset offset2);
		Assert.NotNull(result2);
		Assert.Equal(offset, offset2);
		Assert.Equal(result, result2);
	}
	private static void Test<T, T2>(ReadOnlyFixedContext<T> ctx)
	{
		ReadOnlyFixedContext<T2> result = ctx.GetTransformation<T2>(out ReadOnlyFixedOffset offset);
		Assert.NotNull(result);
		GetTransformationTest.ContextTest(ctx, offset, result);

		ReadOnlyFixedContext<T2> result2 = ctx.GetTransformation<T2>(out ReadOnlyFixedOffset offset2);
		Assert.NotNull(result2);
		Assert.Equal(offset, offset2);
		Assert.Equal(result, result2);
	}
	private static unsafe void ContextTest<T, T2>(FixedContext<T> ctx, FixedOffset offset, FixedContext<T2> result)
	{
		Int32 countT2 = ctx.BinaryLength / sizeof(T2);
		Int32 offsetT2 = countT2 * sizeof(T2);
		HashCode hashResidual = new();
		hashResidual.Add(new IntPtr(Unsafe.AsPointer(ref UnsafeLegacy.AsRef(in ctx.CreateReadOnlyReference<T>()))));
		hashResidual.Add(offsetT2);
		hashResidual.Add(ctx.BinaryLength);
		hashResidual.Add(false);

		Assert.Equal(countT2, result.Count);
		Assert.Equal(0, ctx.BinaryOffset);
		Assert.Equal(offsetT2, offset.BinaryOffset);

		Assert.Equal(ctx.BinaryLength, result.BinaryLength);
		Assert.Equal(ctx.BinaryLength, offset.BinaryLength + offset.BinaryOffset);
		Assert.Equal(hashResidual.ToHashCode(), offset.GetHashCode());

		try
		{
			GCHandle.Alloc(Array.Empty<T>(), GCHandleType.Pinned).Free();
		}
		catch (Exception)
		{
			// Managed types
			return;
		}

		Assert.Equal(ctx.CreateReadOnlyBinarySpan()[offset.BinaryOffset..].ToArray(),
		             offset.CreateReadOnlyBinarySpan().ToArray());
		Assert.Equal(ctx.CreateBinarySpan()[offset.BinaryOffset..].ToArray(), offset.CreateBinarySpan().ToArray());

		_ = ctx.GetTransformation<Boolean>(out FixedOffset offset2, true);
		GetTransformationTest.OffsetTest<T2, Boolean>(offset, offset2);
		_ = ctx.GetTransformation<Byte>(out offset2, true);
		GetTransformationTest.OffsetTest<T2, Byte>(offset, offset2);
		_ = ctx.GetTransformation<Int16>(out offset2, true);
		GetTransformationTest.OffsetTest<T2, Int16>(offset, offset2);
		_ = ctx.GetTransformation<Char>(out offset2, true);
		GetTransformationTest.OffsetTest<T2, Char>(offset, offset2);
		_ = ctx.GetTransformation<Int32>(out offset2, true);
		GetTransformationTest.OffsetTest<T2, Int32>(offset, offset2);
		_ = ctx.GetTransformation<Int64>(out offset2, true);
		GetTransformationTest.OffsetTest<T2, Int64>(offset, offset2);
		_ = ctx.GetTransformation<Int128>(out offset2, true);
		GetTransformationTest.OffsetTest<T2, Int128>(offset, offset2);
		_ = ctx.GetTransformation<Single>(out offset2, true);
		GetTransformationTest.OffsetTest<T2, Single>(offset, offset2);
		_ = ctx.GetTransformation<Half>(out offset2, true);
		GetTransformationTest.OffsetTest<T2, Half>(offset, offset2);
		_ = ctx.GetTransformation<Double>(out offset2, true);
		GetTransformationTest.OffsetTest<T2, Double>(offset, offset2);
		_ = ctx.GetTransformation<Decimal>(out offset2, true);
		GetTransformationTest.OffsetTest<T2, Decimal>(offset, offset2);
		_ = ctx.GetTransformation<DateTime>(out offset2, true);
		GetTransformationTest.OffsetTest<T2, DateTime>(offset, offset2);
		_ = ctx.GetTransformation<TimeOnly>(out offset2, true);
		GetTransformationTest.OffsetTest<T2, TimeOnly>(offset, offset2);
		_ = ctx.GetTransformation<TimeSpan>(out offset2, true);
		GetTransformationTest.OffsetTest<T2, TimeSpan>(offset, offset2);

		Exception functionException = Assert.Throws<InvalidOperationException>(offset.CreateDelegate<Action>);
		Assert.Equal(FixedMemoryTestsBase.IsNotFunction, functionException.Message);
	}
	private static unsafe void ContextTest<T, T2>(ReadOnlyFixedContext<T> ctx, ReadOnlyFixedOffset offset,
		ReadOnlyFixedContext<T2> result)
	{
		Int32 countT2 = ctx.BinaryLength / sizeof(T2);
		Int32 offsetT2 = countT2 * sizeof(T2);
		HashCode hashResidual = new();
		hashResidual.Add(new IntPtr(Unsafe.AsPointer(ref UnsafeLegacy.AsRef(in ctx.CreateReadOnlyReference<T>()))));
		hashResidual.Add(offsetT2);
		hashResidual.Add(ctx.BinaryLength);
		hashResidual.Add(true);

		Assert.Equal(countT2, result.Count);
		Assert.Equal(0, ctx.BinaryOffset);
		Assert.Equal(offsetT2, offset.BinaryOffset);
		Assert.Equal(ctx.BinaryLength, result.BinaryLength);
		Assert.Equal(ctx.BinaryLength, offset.BinaryLength + offset.BinaryOffset);
		Assert.Equal(hashResidual.ToHashCode(), offset.GetHashCode());

		try
		{
			GCHandle.Alloc(Array.Empty<T>(), GCHandleType.Pinned).Free();
		}
		catch (Exception)
		{
			// Managed types
			return;
		}

		Assert.Equal(ctx.CreateReadOnlyBinarySpan()[offset.BinaryOffset..].ToArray(),
		             offset.CreateReadOnlyBinarySpan().ToArray());

		Assert.Equal(FixedMemoryTestsBase.ReadOnlyError,
		             Assert.Throws<InvalidOperationException>(() => offset.CreateBinarySpan()).Message);

		_ = ctx.GetTransformation<Boolean>(out ReadOnlyFixedOffset offset2);
		GetTransformationTest.OffsetTest<T2, Boolean>(offset, offset2);
		_ = ctx.GetTransformation<Byte>(out offset2);
		GetTransformationTest.OffsetTest<T2, Byte>(offset, offset2);
		_ = ctx.GetTransformation<Int16>(out offset2);
		GetTransformationTest.OffsetTest<T2, Int16>(offset, offset2);
		_ = ctx.GetTransformation<Char>(out offset2);
		GetTransformationTest.OffsetTest<T2, Char>(offset, offset2);
		_ = ctx.GetTransformation<Int32>(out offset2);
		GetTransformationTest.OffsetTest<T2, Int32>(offset, offset2);
		_ = ctx.GetTransformation<Int64>(out offset2);
		GetTransformationTest.OffsetTest<T2, Int64>(offset, offset2);
		_ = ctx.GetTransformation<Int128>(out offset2);
		GetTransformationTest.OffsetTest<T2, Int128>(offset, offset2);
		_ = ctx.GetTransformation<Single>(out offset2);
		GetTransformationTest.OffsetTest<T2, Single>(offset, offset2);
		_ = ctx.GetTransformation<Half>(out offset2);
		GetTransformationTest.OffsetTest<T2, Half>(offset, offset2);
		_ = ctx.GetTransformation<Double>(out offset2);
		GetTransformationTest.OffsetTest<T2, Double>(offset, offset2);
		_ = ctx.GetTransformation<Decimal>(out offset2);
		GetTransformationTest.OffsetTest<T2, Decimal>(offset, offset2);
		_ = ctx.GetTransformation<DateTime>(out offset2);
		GetTransformationTest.OffsetTest<T2, DateTime>(offset, offset2);
		_ = ctx.GetTransformation<TimeOnly>(out offset2);
		GetTransformationTest.OffsetTest<T2, TimeOnly>(offset, offset2);
		_ = ctx.GetTransformation<TimeSpan>(out offset2);
		GetTransformationTest.OffsetTest<T2, TimeSpan>(offset, offset2);

		Exception functionException = Assert.Throws<InvalidOperationException>(offset.CreateDelegate<Action>);
		Assert.Equal(FixedMemoryTestsBase.IsNotFunction, functionException.Message);
	}
	private static unsafe void OffsetTest<T2, T3>(FixedOffset offset1, FixedOffset offset2)
	{
		Boolean equal = sizeof(T2) == sizeof(T3) || offset1.BinaryLength == offset2.BinaryLength;
		Assert.Equal(equal, offset1.Equals(offset2));
		Assert.Equal(equal, offset1.Equals((Object)offset2));
		Assert.False(offset2.IsFunction);
		if (equal)
			Assert.Equal(offset1.CreateReadOnlyBinarySpan().ToArray(), offset2.CreateReadOnlyBinarySpan().ToArray());

		Exception functionException = Assert.Throws<InvalidOperationException>(offset2.CreateDelegate<Action>);
		Assert.Equal(FixedMemoryTestsBase.IsNotFunction, functionException.Message);
	}
	private static unsafe void OffsetTest<T2, T3>(ReadOnlyFixedOffset offset1, ReadOnlyFixedOffset offset2)
	{
		Boolean equal = sizeof(T2) == sizeof(T3) || offset1.BinaryLength == offset2.BinaryLength;
		Assert.Equal(equal, offset1.Equals(offset2));
		Assert.Equal(equal, offset1.Equals((Object)offset2));
		Assert.False(offset2.IsFunction);
		if (equal)
			Assert.Equal(offset1.CreateReadOnlyBinarySpan().ToArray(), offset2.CreateReadOnlyBinarySpan().ToArray());

		Exception functionException = Assert.Throws<InvalidOperationException>(offset2.CreateDelegate<Action>);
		Assert.Equal(FixedMemoryTestsBase.IsNotFunction, functionException.Message);
	}
}
#pragma warning restore CS8500