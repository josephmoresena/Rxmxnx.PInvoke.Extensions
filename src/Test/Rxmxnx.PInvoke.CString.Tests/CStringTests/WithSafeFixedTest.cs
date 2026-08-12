namespace Rxmxnx.PInvoke.Tests.CStringTests;

[TestFixture]
[ExcludeFromCodeCoverage]
public sealed unsafe class WithSafeFixedTest
{
	[Fact]
	public void Test()
	{
		using TestMemoryHandle handle = new();
		List<Int32> indices = TestSet.GetIndices();
#pragma warning disable CS0612
		indices.ForEach(i => WithSafeFixedTest.ExecuteTest(TestSet.GetCString(i, handle)));
#pragma warning restore CS0612
	}
	[Fact]
	public void NullTest()
	{
		FunctionalInterface? i = new();
		CString? value = default;
		value.WithSafeFixed(i);
		value.WithSafeFixed(i, out value);
		PInvokeAssert.NotNull(value);
		PInvokeAssert.Equal(1, i.AcceptCount);
		PInvokeAssert.Equal(1, i.ApplyCount);
		i = default;
		value.WithSafeFixed(i);
		value.WithSafeFixed(i, out CString result);
		PInvokeAssert.Null(result);
	}

	[Obsolete]
	private static void ExecuteTest(CString? value)
	{
		if (value is null) return;
#if NETSTANDARD2_1 && !LEGACY || NETCOREAPP3_0_OR_GREATER
		value.WithSafeFixed(WithSafeFixedTest.ActionMethod);
		value.WithSafeFixed(value, WithSafeFixedTest.ActionMethod);
		PInvokeAssert.Equal(value, value.WithSafeFixed(WithSafeFixedTest.FunctionMethod));
		PInvokeAssert.Equal(value, value.WithSafeFixed(value, WithSafeFixedTest.FunctionMethod));
#endif
		value.WithSafeFixed(new ReadOnlyFixedAction());
		value.WithSafeFixed(new ReadOnlyFixedFunction(), out CString result);
		PInvokeAssert.Equal(value, result);
	}

#if NETSTANDARD2_1 && !LEGACY || NETCOREAPP3_0_OR_GREATER
	private static void ActionMethod(in IReadOnlyFixedMemory fmem)
	{
		ReadOnlySpan<Byte> span = fmem.Bytes;
		if (fmem.Pointer == IntPtr.Zero)
		{
			PInvokeAssert.True(span.IsEmpty);
			return;
		}
		fixed (void* ptr = span)
		{
			if (span.Length != 0)
				PInvokeAssert.Equal(fmem.Pointer, new(ptr));
			else if (fmem.Pointer != IntPtr.Zero)
				fixed (void* ptrEmpty = CString.Empty)
					PInvokeAssert.Equal(fmem.Pointer, new(ptrEmpty));
		}
#if NETCOREAPP
		GCHandle handle = GCHandle.FromIntPtr(fmem.Pointer);
		Assert.True(handle.IsAllocated);
#endif
	}
	private static void ActionMethod(in IReadOnlyFixedMemory fmem, CString cstr)
	{
		IReadOnlyFixedMemory fmem2 = fmem;

		WithSafeFixedTest.ActionMethod(fmem);
		if (fmem2.Bytes.Length > 0)
			PInvokeAssert.Equal(cstr, new(() => fmem2.Bytes));
		WithSafeFixedTest.BinaryPointerTest(fmem, cstr);
	}

	private static CString FunctionMethod(in IReadOnlyFixedMemory fmem)
	{
		WithSafeFixedTest.ActionMethod(fmem);
		if (fmem.Bytes.Length > 0)
			return CString.Create(fmem.Bytes);
		return fmem.Pointer != IntPtr.Zero ? CString.Empty : CString.Zero;
	}
	private static CString FunctionMethod(in IReadOnlyFixedMemory fmem, CString cstr)
	{
		WithSafeFixedTest.ActionMethod(fmem, cstr);
		if (fmem.Bytes.Length > 0)
			return CString.Create(fmem.Bytes);
		return fmem.Pointer != IntPtr.Zero ? CString.Empty : CString.Zero;
	}

	private static void BinaryPointerTest(IReadOnlyFixedMemory fmem, CString cstr)
	{
		IntPtr? ptr = WithSafeFixedTest.GetPointerFromBytes(cstr);
		if (ptr.HasValue)
			PInvokeAssert.Equal(fmem.Pointer, ptr);
	}
	private static IntPtr? GetPointerFromBytes(CString cstr)
	{
		if (Object.ReferenceEquals(CString.Empty, cstr)) return default;
		try
		{
			fixed (void* ptr = CString.GetBytes(cstr))
				return new(ptr);
		}
		catch (Exception)
		{
			return default;
		}
	}
#endif
	private readonly struct ReadOnlyFixedAction : IReadOnlyFixedContextAction<Byte>
	{
		public void Accept(scoped ReadOnlyFixedContextValue<Byte> fixedContext)
		{
			ReadOnlySpan<Byte> span = fixedContext.Bytes;
			if (fixedContext.Pointer == IntPtr.Zero)
			{
				PInvokeAssert.True(span.IsEmpty);
				return;
			}
			fixed (void* ptr = span)
			{
				if (span.Length != 0)
					PInvokeAssert.Equal(fixedContext.Pointer, new(ptr));
				else if (fixedContext.Pointer != IntPtr.Zero)
					fixed (void* ptrEmpty = CString.Empty)
						PInvokeAssert.Equal(fixedContext.Pointer, new(ptrEmpty));
			}
#if NETCOREAPP
			GCHandle handle = GCHandle.FromIntPtr(fixedContext.Pointer);
			Assert.True(handle.IsAllocated);
#endif
		}
	}

	private readonly struct ReadOnlyFixedFunction : IReadOnlyFixedContextFunction<Byte, CString>
	{
		public CString Apply(scoped ReadOnlyFixedContextValue<Byte> fixedContext)
		{
			new ReadOnlyFixedAction().Accept(fixedContext);
			if (fixedContext.Bytes.Length > 0)
				return CString.Create(fixedContext.Bytes);
			return fixedContext.Pointer != IntPtr.Zero ? CString.Empty : CString.Zero;
		}
	}

	private sealed class FunctionalInterface : IReadOnlyFixedContextAction<Byte>,
		IReadOnlyFixedContextFunction<Byte, CString>
	{
		public Int32 AcceptCount { get; private set; }
		public Int32 ApplyCount{ get; private set; }
		
		public void Accept(scoped ReadOnlyFixedContextValue<Byte> ctx)
		{
			this.AcceptCount++;
			new ReadOnlyFixedAction().Accept(ctx);
		}
		public CString Apply(scoped ReadOnlyFixedContextValue<Byte> ctx)
		{
			this.ApplyCount++;
			return new ReadOnlyFixedFunction().Apply(ctx);
		}
	}
}