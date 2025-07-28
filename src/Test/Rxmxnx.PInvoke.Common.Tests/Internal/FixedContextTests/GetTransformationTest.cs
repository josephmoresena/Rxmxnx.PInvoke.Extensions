#if !NETCOREAPP
using Fact = NUnit.Framework.TestAttribute;
#endif

namespace Rxmxnx.PInvoke.Tests.Internal.FixedContextTests;

[TestFixture]
[ExcludeFromCodeCoverage]
[SuppressMessage("csharpsquid", "S2699")]
#pragma warning disable CS8500
public sealed class GetTransformationTest : FixedContextTestsBase
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
		T[] values = FixedMemoryTestsBase.Fixture.CreateMany<T>().ToArray();
		FixedContextTestsBase.WithFixed(values, GetTransformationTest.Test);
		FixedContextTestsBase.WithFixed(values, GetTransformationTest.ReadOnlyTest);
	}
	private static void Test<T>(FixedContext<T> ctx, T[] arr)
	{
		try
		{
			GCHandle.Alloc(arr, GCHandleType.Pinned).Free();
#if !NETCOREAPP
			if (RuntimeHelpers.IsReferenceOrContainsReferences<T>())
				throw new ArgumentException(); // Required for Mono?
#endif
			GetTransformationTest.Test<T, Boolean>(ctx);
			GetTransformationTest.Test<T, Byte>(ctx);
			GetTransformationTest.Test<T, Int16>(ctx);
			GetTransformationTest.Test<T, Char>(ctx);
			GetTransformationTest.Test<T, Int32>(ctx);
			GetTransformationTest.Test<T, Int64>(ctx);
#if NET7_0_OR_GREATER
			GetTransformationTest.Test<T, Int128>(ctx);
#endif
			GetTransformationTest.Test<T, Single>(ctx);
#if NET5_0_OR_GREATER
			GetTransformationTest.Test<T, Half>(ctx);
#endif
			GetTransformationTest.Test<T, Double>(ctx);
			GetTransformationTest.Test<T, Decimal>(ctx);
			GetTransformationTest.Test<T, DateTime>(ctx);
#if NET6_0_OR_GREATER
			GetTransformationTest.Test<T, TimeOnly>(ctx);
#endif
			GetTransformationTest.Test<T, TimeSpan>(ctx);

			PInvokeAssert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, ManagedStruct>(ctx));
			PInvokeAssert.Throws<InvalidOperationException>(() => GetTransformationTest
				                                                .Test<T, WrapperStruct<ManagedStruct>>(ctx));
			PInvokeAssert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, String>(ctx));
			PInvokeAssert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, Object>(ctx));
		}
		catch (ArgumentException)
		{
			PInvokeAssert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, Boolean>(ctx));
			PInvokeAssert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, Byte>(ctx));
			PInvokeAssert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, Int16>(ctx));
			PInvokeAssert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, Char>(ctx));
			PInvokeAssert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, Int32>(ctx));
			PInvokeAssert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, Int64>(ctx));
#if NET7_0_OR_GREATER
			Assert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, Int128>(ctx));
#endif
			PInvokeAssert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, Single>(ctx));
#if NET5_0_OR_GREATER
			Assert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, Half>(ctx));
#endif
			PInvokeAssert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, Double>(ctx));
			PInvokeAssert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, Decimal>(ctx));
			PInvokeAssert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, DateTime>(ctx));
#if NET6_0_OR_GREATER
			Assert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, TimeOnly>(ctx));
#endif
			PInvokeAssert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, TimeSpan>(ctx));

			PInvokeAssert.Throws<InvalidOperationException>(() => GetTransformationTest
				                                                .Test<T, WrapperStruct<ManagedStruct>>(ctx));
			PInvokeAssert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, WrapperStruct<String>>(
				                                                ctx));
			PInvokeAssert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, WrapperStruct<Object>>(
				                                                ctx));

			if (typeof(T).IsValueType)
			{
				GetTransformationTest.Test<T, ManagedStruct>(ctx);

				PInvokeAssert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, String>(ctx));
				PInvokeAssert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, Object>(ctx));
			}
			else
			{
				PInvokeAssert.Throws<InvalidOperationException>(() => GetTransformationTest
					                                                .Test<T, ManagedStruct>(ctx));
				GetTransformationTest.Test<T, String>(ctx);
				GetTransformationTest.Test<T, Object>(ctx);
			}
		}

		ctx.Unload();
		PInvokeAssert.Equal(FixedMemoryTestsBase.InvalidError,
		                    PInvokeAssert
			                    .Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, Boolean>(ctx))
			                    .Message);
		PInvokeAssert.Equal(FixedMemoryTestsBase.InvalidError,
		                    PInvokeAssert
			                    .Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, Byte>(ctx))
			                    .Message);
		PInvokeAssert.Equal(FixedMemoryTestsBase.InvalidError,
		                    PInvokeAssert
			                    .Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, Int16>(ctx))
			                    .Message);
		PInvokeAssert.Equal(FixedMemoryTestsBase.InvalidError,
		                    PInvokeAssert
			                    .Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, Char>(ctx))
			                    .Message);
		PInvokeAssert.Equal(FixedMemoryTestsBase.InvalidError,
		                    PInvokeAssert
			                    .Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, Int32>(ctx))
			                    .Message);
		PInvokeAssert.Equal(FixedMemoryTestsBase.InvalidError,
		                    PInvokeAssert
			                    .Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, Int64>(ctx))
			                    .Message);
#if NET7_0_OR_GREATER
		Assert.Equal(FixedMemoryTestsBase.InvalidError,
		             Assert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, Int128>(ctx))
		                   .Message);
#endif
		PInvokeAssert.Equal(FixedMemoryTestsBase.InvalidError,
		                    PInvokeAssert
			                    .Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, Single>(ctx))
			                    .Message);
#if NET5_0_OR_GREATER
		Assert.Equal(FixedMemoryTestsBase.InvalidError,
		             Assert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, Half>(ctx)).Message);
#endif
		PInvokeAssert.Equal(FixedMemoryTestsBase.InvalidError,
		                    PInvokeAssert
			                    .Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, Double>(ctx))
			                    .Message);
		PInvokeAssert.Equal(FixedMemoryTestsBase.InvalidError,
		                    PInvokeAssert
			                    .Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, Decimal>(ctx))
			                    .Message);
		PInvokeAssert.Equal(FixedMemoryTestsBase.InvalidError,
		                    PInvokeAssert
			                    .Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, DateTime>(ctx))
			                    .Message);
#if NET6_0_OR_GREATER
		Assert.Equal(FixedMemoryTestsBase.InvalidError,
		             Assert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, TimeOnly>(ctx))
		                   .Message);
#endif
		PInvokeAssert.Equal(FixedMemoryTestsBase.InvalidError,
		                    PInvokeAssert
			                    .Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, TimeSpan>(ctx))
			                    .Message);
	}
	private static void ReadOnlyTest<T>(ReadOnlyFixedContext<T> ctx, T[] arr)
	{
		try
		{
			GCHandle.Alloc(arr, GCHandleType.Pinned).Free();
#if !NETCOREAPP
			if (RuntimeHelpers.IsReferenceOrContainsReferences<T>())
				throw new ArgumentException(); // Required for Mono?
#endif
			GetTransformationTest.Test<T, Boolean>(ctx);
			GetTransformationTest.Test<T, Byte>(ctx);
			GetTransformationTest.Test<T, Int16>(ctx);
			GetTransformationTest.Test<T, Char>(ctx);
			GetTransformationTest.Test<T, Int32>(ctx);
			GetTransformationTest.Test<T, Int64>(ctx);
#if NET7_0_OR_GREATER
			GetTransformationTest.Test<T, Int128>(ctx);
#endif
			GetTransformationTest.Test<T, Single>(ctx);
#if NET5_0_OR_GREATER
			GetTransformationTest.Test<T, Half>(ctx);
#endif
			GetTransformationTest.Test<T, Double>(ctx);
			GetTransformationTest.Test<T, Decimal>(ctx);
			GetTransformationTest.Test<T, DateTime>(ctx);
#if NET6_0_OR_GREATER
			GetTransformationTest.Test<T, TimeOnly>(ctx);
#endif
			GetTransformationTest.Test<T, TimeSpan>(ctx);

			PInvokeAssert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, ManagedStruct>(ctx));
			PInvokeAssert.Throws<InvalidOperationException>(() => GetTransformationTest
				                                                .Test<T, WrapperStruct<ManagedStruct>>(ctx));
			PInvokeAssert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, String>(ctx));
			PInvokeAssert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, Object>(ctx));
		}
		catch (ArgumentException)
		{
			PInvokeAssert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, Boolean>(ctx));
			PInvokeAssert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, Byte>(ctx));
			PInvokeAssert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, Int16>(ctx));
			PInvokeAssert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, Char>(ctx));
			PInvokeAssert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, Int32>(ctx));
			PInvokeAssert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, Int64>(ctx));
#if NET7_0_OR_GREATER
			Assert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, Int128>(ctx));
#endif
			PInvokeAssert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, Single>(ctx));
#if NET5_0_OR_GREATER
			Assert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, Half>(ctx));
#endif
			PInvokeAssert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, Double>(ctx));
			PInvokeAssert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, Decimal>(ctx));
			PInvokeAssert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, DateTime>(ctx));
#if NET6_0_OR_GREATER
			Assert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, TimeOnly>(ctx));
#endif
			PInvokeAssert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, TimeSpan>(ctx));

			PInvokeAssert.Throws<InvalidOperationException>(() => GetTransformationTest
				                                                .Test<T, WrapperStruct<ManagedStruct>>(ctx));
			PInvokeAssert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, WrapperStruct<String>>(
				                                                ctx));
			PInvokeAssert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, WrapperStruct<Object>>(
				                                                ctx));

			if (typeof(T).IsValueType)
			{
				GetTransformationTest.Test<T, ManagedStruct>(ctx);

				PInvokeAssert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, String>(ctx));
				PInvokeAssert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, Object>(ctx));
			}
			else
			{
				PInvokeAssert.Throws<InvalidOperationException>(() => GetTransformationTest
					                                                .Test<T, ManagedStruct>(ctx));
				GetTransformationTest.Test<T, String>(ctx);
				GetTransformationTest.Test<T, Object>(ctx);
			}
		}

		ctx.Unload();
		PInvokeAssert.Equal(FixedMemoryTestsBase.InvalidError,
		                    PInvokeAssert
			                    .Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, Boolean>(ctx))
			                    .Message);
		PInvokeAssert.Equal(FixedMemoryTestsBase.InvalidError,
		                    PInvokeAssert
			                    .Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, Byte>(ctx))
			                    .Message);
		PInvokeAssert.Equal(FixedMemoryTestsBase.InvalidError,
		                    PInvokeAssert
			                    .Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, Int16>(ctx))
			                    .Message);
		PInvokeAssert.Equal(FixedMemoryTestsBase.InvalidError,
		                    PInvokeAssert
			                    .Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, Char>(ctx))
			                    .Message);
		PInvokeAssert.Equal(FixedMemoryTestsBase.InvalidError,
		                    PInvokeAssert
			                    .Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, Int32>(ctx))
			                    .Message);
		PInvokeAssert.Equal(FixedMemoryTestsBase.InvalidError,
		                    PInvokeAssert
			                    .Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, Int64>(ctx))
			                    .Message);
#if NET7_0_OR_GREATER
		Assert.Equal(FixedMemoryTestsBase.InvalidError,
		             Assert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, Int128>(ctx))
		                   .Message);
#endif
		PInvokeAssert.Equal(FixedMemoryTestsBase.InvalidError,
		                    PInvokeAssert
			                    .Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, Single>(ctx))
			                    .Message);
#if NET5_0_OR_GREATER
		Assert.Equal(FixedMemoryTestsBase.InvalidError,
		             Assert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, Half>(ctx)).Message);
#endif
		PInvokeAssert.Equal(FixedMemoryTestsBase.InvalidError,
		                    PInvokeAssert
			                    .Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, Double>(ctx))
			                    .Message);
		PInvokeAssert.Equal(FixedMemoryTestsBase.InvalidError,
		                    PInvokeAssert
			                    .Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, Decimal>(ctx))
			                    .Message);
		PInvokeAssert.Equal(FixedMemoryTestsBase.InvalidError,
		                    PInvokeAssert
			                    .Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, DateTime>(ctx))
			                    .Message);
#if NET6_0_OR_GREATER
		Assert.Equal(FixedMemoryTestsBase.InvalidError,
		             Assert.Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, TimeOnly>(ctx))
		                   .Message);
#endif
		PInvokeAssert.Equal(FixedMemoryTestsBase.InvalidError,
		                    PInvokeAssert
			                    .Throws<InvalidOperationException>(() => GetTransformationTest.Test<T, TimeSpan>(ctx))
			                    .Message);
	}
	private static void Test<T, T2>(FixedContext<T> ctx)
	{
		FixedContext<T2> result = ctx.GetTransformation<T2>(out FixedOffset offset, true);
		PInvokeAssert.NotNull(result);
		GetTransformationTest.ContextTest(ctx, offset, result);

		FixedContext<T2> result2 = ctx.GetTransformation<T2>(out FixedOffset offset2);
		PInvokeAssert.NotNull(result2);
		PInvokeAssert.Equal(offset, offset2);
		PInvokeAssert.Equal(result, result2);
	}
	private static void Test<T, T2>(ReadOnlyFixedContext<T> ctx)
	{
		ReadOnlyFixedContext<T2> result = ctx.GetTransformation<T2>(out ReadOnlyFixedOffset offset);
		PInvokeAssert.NotNull(result);
		GetTransformationTest.ContextTest(ctx, offset, result);

		ReadOnlyFixedContext<T2> result2 = ctx.GetTransformation<T2>(out ReadOnlyFixedOffset offset2);
		PInvokeAssert.NotNull(result2);
		PInvokeAssert.Equal(offset, offset2);
		PInvokeAssert.Equal(result, result2);
	}
	private static unsafe void ContextTest<T, T2>(FixedContext<T> ctx, FixedOffset offset, FixedContext<T2> result)
	{
		Int32 countT2 = ctx.BinaryLength / sizeof(T2);
		Int32 offsetT2 = countT2 * sizeof(T2);
		HashCode hashResidual = new();
		hashResidual.Add(new IntPtr(Unsafe.AsPointer(ref Unsafe.AsRef(in ctx.CreateReadOnlyReference<T>()))));
		hashResidual.Add(offsetT2);
		hashResidual.Add(ctx.BinaryLength);
		hashResidual.Add(false);

		PInvokeAssert.Equal(countT2, result.Count);
		PInvokeAssert.Equal(0, ctx.BinaryOffset);
		PInvokeAssert.Equal(offsetT2, offset.BinaryOffset);

		PInvokeAssert.Equal(ctx.BinaryLength, result.BinaryLength);
		PInvokeAssert.Equal(ctx.BinaryLength, offset.BinaryLength + offset.BinaryOffset);
		PInvokeAssert.Equal(hashResidual.ToHashCode(), offset.GetHashCode());

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
			PInvokeAssert.True(ctx.CreateBinarySpan().IsEmpty);
			PInvokeAssert.True(ctx.CreateReadOnlyBinarySpan().IsEmpty);
			PInvokeAssert.Equal(typeof(T).IsValueType || ctx.IsNullOrEmpty, ctx.CreateReadOnlyObjectSpan().IsEmpty);
			PInvokeAssert.Equal(typeof(T).IsValueType || ctx.IsNullOrEmpty, ctx.CreateObjectSpan().IsEmpty);
			return;
		}

		PInvokeAssert.Equal(ctx.CreateReadOnlyBinarySpan()[offset.BinaryOffset..].ToArray(),
		                    offset.CreateReadOnlyBinarySpan().ToArray());
		PInvokeAssert.True(ctx.CreateReadOnlyObjectSpan().IsEmpty);
		PInvokeAssert.True(offset.CreateReadOnlyObjectSpan().IsEmpty);
		PInvokeAssert.Equal(ctx.CreateBinarySpan()[offset.BinaryOffset..].ToArray(),
		                    offset.CreateBinarySpan().ToArray());
		PInvokeAssert.True(ctx.CreateObjectSpan().IsEmpty);
		PInvokeAssert.True(offset.CreateObjectSpan().IsEmpty);

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
#if NET7_0_OR_GREATER
		_ = ctx.GetTransformation<Int128>(out offset2, true);
		GetTransformationTest.OffsetTest<T2, Int128>(offset, offset2);
#endif
		_ = ctx.GetTransformation<Single>(out offset2, true);
		GetTransformationTest.OffsetTest<T2, Single>(offset, offset2);
#if NET5_0_OR_GREATER
		_ = ctx.GetTransformation<Half>(out offset2, true);
		GetTransformationTest.OffsetTest<T2, Half>(offset, offset2);
#endif
		_ = ctx.GetTransformation<Double>(out offset2, true);
		GetTransformationTest.OffsetTest<T2, Double>(offset, offset2);
		_ = ctx.GetTransformation<Decimal>(out offset2, true);
		GetTransformationTest.OffsetTest<T2, Decimal>(offset, offset2);
		_ = ctx.GetTransformation<DateTime>(out offset2, true);
		GetTransformationTest.OffsetTest<T2, DateTime>(offset, offset2);
#if NET6_0_OR_GREATER
		_ = ctx.GetTransformation<TimeOnly>(out offset2, true);
		GetTransformationTest.OffsetTest<T2, TimeOnly>(offset, offset2);
#endif
		_ = ctx.GetTransformation<TimeSpan>(out offset2, true);
		GetTransformationTest.OffsetTest<T2, TimeSpan>(offset, offset2);
	}
	private static unsafe void ContextTest<T, T2>(ReadOnlyFixedContext<T> ctx, ReadOnlyFixedOffset offset,
		ReadOnlyFixedContext<T2> result)
	{
		Int32 countT2 = ctx.BinaryLength / sizeof(T2);
		Int32 offsetT2 = countT2 * sizeof(T2);
		HashCode hashResidual = new();
		hashResidual.Add(new IntPtr(Unsafe.AsPointer(ref Unsafe.AsRef(in ctx.CreateReadOnlyReference<T>()))));
		hashResidual.Add(offsetT2);
		hashResidual.Add(ctx.BinaryLength);
		hashResidual.Add(true);

		PInvokeAssert.Equal(countT2, result.Count);
		PInvokeAssert.Equal(0, ctx.BinaryOffset);
		PInvokeAssert.Equal(offsetT2, offset.BinaryOffset);
		PInvokeAssert.Equal(ctx.BinaryLength, result.BinaryLength);
		PInvokeAssert.Equal(ctx.BinaryLength, offset.BinaryLength + offset.BinaryOffset);
		PInvokeAssert.Equal(hashResidual.ToHashCode(), offset.GetHashCode());

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
			PInvokeAssert.True(ctx.CreateReadOnlyBinarySpan().IsEmpty);
			PInvokeAssert.Equal(typeof(T).IsValueType || ctx.IsNullOrEmpty, ctx.CreateReadOnlyObjectSpan().IsEmpty);
			return;
		}

		PInvokeAssert.Equal(ctx.CreateReadOnlyBinarySpan()[offset.BinaryOffset..].ToArray(),
		                    offset.CreateReadOnlyBinarySpan().ToArray());
		PInvokeAssert.True(ctx.CreateReadOnlyObjectSpan().IsEmpty);

		PInvokeAssert.Equal(FixedMemoryTestsBase.ReadOnlyError,
		                    PInvokeAssert.Throws<InvalidOperationException>(() => offset.CreateBinarySpan()).Message);
		PInvokeAssert.Equal(FixedMemoryTestsBase.ReadOnlyError,
		                    PInvokeAssert.Throws<InvalidOperationException>(() => offset.CreateObjectSpan()).Message);

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
#if NET7_0_OR_GREATER
		_ = ctx.GetTransformation<Int128>(out offset2);
		GetTransformationTest.OffsetTest<T2, Int128>(offset, offset2);
#endif
		_ = ctx.GetTransformation<Single>(out offset2);
		GetTransformationTest.OffsetTest<T2, Single>(offset, offset2);
#if NET5_0_OR_GREATER
		_ = ctx.GetTransformation<Half>(out offset2);
		GetTransformationTest.OffsetTest<T2, Half>(offset, offset2);
#endif
		_ = ctx.GetTransformation<Double>(out offset2);
		GetTransformationTest.OffsetTest<T2, Double>(offset, offset2);
		_ = ctx.GetTransformation<Decimal>(out offset2);
		GetTransformationTest.OffsetTest<T2, Decimal>(offset, offset2);
		_ = ctx.GetTransformation<DateTime>(out offset2);
		GetTransformationTest.OffsetTest<T2, DateTime>(offset, offset2);
#if NET6_0_OR_GREATER
		_ = ctx.GetTransformation<TimeOnly>(out offset2);
		GetTransformationTest.OffsetTest<T2, TimeOnly>(offset, offset2);
#endif
		_ = ctx.GetTransformation<TimeSpan>(out offset2);
		GetTransformationTest.OffsetTest<T2, TimeSpan>(offset, offset2);
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