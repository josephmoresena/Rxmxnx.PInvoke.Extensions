#pragma warning disable CA1041

namespace Rxmxnx.PInvoke.Tests.FixedContextValueTests;

[TestFixture]
[ExcludeFromCodeCoverage]
[SuppressMessage("csharpsquid", "S2699")]
public sealed class CreateValueTest : FixedContextTestsBase
{
	private static readonly IFixture fixture = new Fixture();

	[Fact]
#if (NETSTANDARD2_1 && !LEGACY) || NETCOREAPP3_0_OR_GREATER
	[Obsolete]
#endif
	public void ByteTest() => CreateValueTest.Test<Byte>();
	[Fact]
#if (NETSTANDARD2_1 && !LEGACY) || NETCOREAPP3_0_OR_GREATER
	[Obsolete]
#endif
	public void Int16Test() => CreateValueTest.Test<Int16>();
	[Fact]
#if (NETSTANDARD2_1 && !LEGACY) || NETCOREAPP3_0_OR_GREATER
	[Obsolete]
#endif
	public void Int32Test() => CreateValueTest.Test<Int32>();
	[Fact]
#if (NETSTANDARD2_1 && !LEGACY) || NETCOREAPP3_0_OR_GREATER
	[Obsolete]
#endif
	public void Int64Test() => CreateValueTest.Test<Int64>();
	[Fact]
#if (NETSTANDARD2_1 && !LEGACY) || NETCOREAPP3_0_OR_GREATER
	[Obsolete]
#endif
	public void GuidTest() => CreateValueTest.Test<Guid>();

#if (NETSTANDARD2_1 && !LEGACY) || NETCOREAPP3_0_OR_GREATER
	[Obsolete]
#endif
	private static void Test<T>() where T : unmanaged
	{
		T[] values = CreateValueTest.fixture.CreateMany<T>(8).ToArray();
		FixedContextTestsBase.WithFixed(values, CreateValueTest.AssertFromFixedContext);
		FixedContextTestsBase.WithFixed(values, CreateValueTest.AssertFromReadOnlyFixedContext);
#if NETSTANDARD2_1 || NETCOREAPP3_0_OR_GREATER
		CreateValueTest.AssertEmpty();
#endif
	}

#if (NETSTANDARD2_1 && !LEGACY) || NETCOREAPP3_0_OR_GREATER
	[Obsolete]
#endif
	private static void AssertFromFixedContext<T>(FixedContext<T> ctx, T[] values) where T : unmanaged
	{
		FixedContextValue<T> value = FixedContextValue<T>.CreateValue(ctx);
		CreateValueTest.AssertContext(value, (ctx as IFixedPointer).Pointer, values);

#if NETSTANDARD2_1 || NETCOREAPP3_0_OR_GREATER
		using IFixedContext<T>.IDisposable disposable = ctx.ToDisposable(null);
		FixedContextValue<T> fromDisposable = FixedContextValue<T>.CreateValue(disposable);
		CreateValueTest.AssertContext(fromDisposable, (ctx as IFixedPointer).Pointer, values);
#endif
	}
#if (NETSTANDARD2_1 && !LEGACY) || NETCOREAPP3_0_OR_GREATER
	[Obsolete]
#endif
	private static void AssertFromReadOnlyFixedContext<T>(ReadOnlyFixedContext<T> ctx, T[] values) where T : unmanaged
	{
		ReadOnlyFixedContextValue<T> value = ReadOnlyFixedContextValue<T>.CreateValue(ctx);
		CreateValueTest.AssertReadOnlyContext(value, (ctx as IFixedPointer).Pointer, values);

#if NETSTANDARD2_1 || NETCOREAPP3_0_OR_GREATER
		using IReadOnlyFixedContext<T>.IDisposable disposable = ctx.ToDisposable(null);
		ReadOnlyFixedContextValue<T> fromDisposable = ReadOnlyFixedContextValue<T>.CreateValue(disposable);
		CreateValueTest.AssertReadOnlyContext(fromDisposable, (ctx as IFixedPointer).Pointer, values);
#endif
	}
#if NETSTANDARD2_1 || NETCOREAPP3_0_OR_GREATER
#if (NETSTANDARD2_1 && !LEGACY) || NETCOREAPP3_0_OR_GREATER
	[Obsolete]
#endif
	private static void AssertEmpty()
	{
		FixedContextValue<Int32> empty = FixedContextValue<Int32>.CreateValue(FixedContext<Int32>.Empty);
		PInvokeAssert.True(empty.IsNullOrEmpty);
		PInvokeAssert.Equal(IntPtr.Zero, empty.Pointer);

		ReadOnlyFixedContextValue<Int32> readOnlyEmpty =
			ReadOnlyFixedContextValue<Int32>.CreateValue(ReadOnlyFixedContext<Int32>.Empty);
		PInvokeAssert.True(readOnlyEmpty.IsNullOrEmpty);
		PInvokeAssert.Equal(IntPtr.Zero, readOnlyEmpty.Pointer);

		FixedContextValue<Int32> emptyDisposable =
			FixedContextValue<Int32>.CreateValue(FixedContext<Int32>.EmptyDisposable);
		ReadOnlyFixedContextValue<Int32> readOnlyEmptyDisposable =
			ReadOnlyFixedContextValue<Int32>.CreateValue(ReadOnlyFixedContext<Int32>.EmptyDisposable);
		PInvokeAssert.True(emptyDisposable.IsNullOrEmpty);
		PInvokeAssert.True(readOnlyEmptyDisposable.IsNullOrEmpty);
	}
#endif

	private static void AssertContext<T>(FixedContextValue<T> value, IntPtr pointer, ReadOnlySpan<T> expected)
		where T : unmanaged
	{
		PInvokeAssert.Equal(pointer, value.Pointer);
		PInvokeAssert.Equal(expected.Length, value.Values.Length);
		PInvokeAssert.True(Unsafe.AreSame(ref MemoryMarshal.GetReference(expected),
		                                  ref MemoryMarshal.GetReference(value.Values)));
		for (Int32 i = 0; i < expected.Length; i++)
			PInvokeAssert.Equal(expected[i], value.Values[i]);
	}
	private static void AssertReadOnlyContext<T>(ReadOnlyFixedContextValue<T> value, IntPtr pointer,
		ReadOnlySpan<T> expected) where T : unmanaged
	{
		PInvokeAssert.Equal(pointer, value.Pointer);
		PInvokeAssert.Equal(expected.Length, value.Values.Length);
		PInvokeAssert.True(Unsafe.AreSame(ref MemoryMarshal.GetReference(expected),
		                                  ref MemoryMarshal.GetReference(value.Values)));
		for (Int32 i = 0; i < expected.Length; i++)
			PInvokeAssert.Equal(expected[i], value.Values[i]);
	}
}