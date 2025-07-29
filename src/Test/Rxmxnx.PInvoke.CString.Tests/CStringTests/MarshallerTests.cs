#if !NETCOREAPP
using Fact = NUnit.Framework.TestAttribute;
#endif

namespace Rxmxnx.PInvoke.Tests.CStringTests;

[TestFixture]
[ExcludeFromCodeCoverage]
public sealed unsafe class MarshallerTests
{
	[Fact]
	public void FromManagedTest()
	{
		Int32 index = Random.Shared.Next(0, TestSet.Utf16Text.Count);
		ReadOnlySpan<Byte> utf8Span = TestSet.Utf8Text[index]();
		IntPtr ptr = (IntPtr)Unsafe.AsPointer(ref MemoryMarshal.GetReference(utf8Span));

		MarshallerTests.AssertToUnmanaged(new(TestSet.Utf8Text[index]));
		MarshallerTests.AssertToUnmanaged(TestSet.Utf8NullTerminatedBytes[index]);
		MarshallerTests.AssertToUnmanaged(TestSet.Utf8Bytes[index]);
		MarshallerTests.AssertToUnmanaged((CString)TestSet.Utf16Text[index]);
		MarshallerTests.AssertToUnmanaged(CString.CreateUnsafe(ptr, utf8Span.Length, true));
		MarshallerTests.AssertToUnmanaged(CString.CreateUnsafe(ptr, utf8Span.Length + 1));
	}
	[Fact]
	public void FromUnmanagedTest()
	{
		Int32 index = Random.Shared.Next(0, TestSet.Utf16Text.Count);
		ReadOnlySpan<Byte> utf8Span = TestSet.Utf8Text[index]();
		IntPtr ptr = (IntPtr)Unsafe.AsPointer(ref MemoryMarshal.GetReference(utf8Span));
		CString.Marshaller marshal = new();
		try
		{
			marshal.FromUnmanaged(ptr);
			PInvokeAssert.Equal(IntPtr.Zero, marshal.ToUnmanaged());

			CString? value = marshal.ToManaged();
			PInvokeAssert.NotNull(value);
			PInvokeAssert.True(Unsafe.AreSame(ref MemoryMarshal.GetReference(utf8Span),
			                                  ref MemoryMarshal.GetReference<Byte>(value)));
			PInvokeAssert.StrictEqual(value, marshal.ToManaged());
			PInvokeAssert.True(utf8Span.SequenceEqual(value));
			PInvokeAssert.Equal(ptr, marshal.ToUnmanaged());
		}
		finally
		{
			marshal.Free();
		}
	}
	[Fact]
	public void EmptyTest()
	{
		MarshallerTests.AssertToUnmanaged(null);
		MarshallerTests.AssertToUnmanaged(CString.Zero);
		MarshallerTests.AssertToUnmanaged(CString.Empty);
	}

	private static void AssertToUnmanaged(CString? value)
	{
		fixed (Byte* valPtr = value)
		{
#if NET6_0_OR_GREATER
			ReadOnlySpan<Byte> utfSpan = MemoryMarshal.CreateReadOnlySpanFromNullTerminated(
#else
			ReadOnlySpan<Byte> utfSpan = MemoryMarshalCompat.CreateReadOnlySpanFromNullTerminated(
#endif
				valPtr);
			CString.Marshaller marshal = MarshallerTests.Marshal(value);
			IntPtr ptr = marshal.ToUnmanaged();
			try
			{
#if NETCOREAPP
				Boolean isLiteralOrPinnable = MarshallerTests.IsLiteralOrPinnable(value);
				ReadOnlySpan<Byte> unmanagedSpan = new(ptr.ToPointer(), value?.Length ?? 0);
				Assert.Equal(isLiteralOrPinnable,
				             Unsafe.AreSame(ref MemoryMarshal.GetReference<Byte>(value),
				                            ref MemoryMarshal.GetReference(unmanagedSpan)));
#endif
#if NET6_0_OR_GREATER
				Assert.True(utfSpan.SequenceEqual(MemoryMarshal.CreateReadOnlySpanFromNullTerminated((Byte*)ptr)));
#endif
				PInvokeAssert.Equal(utfSpan.Length,
				                    MemoryMarshalCompat.IndexOfNull(ref MemoryMarshal.GetReference<Byte>(value)));
				PInvokeAssert.Equal(ptr, marshal.ToUnmanaged());
			}
			finally
			{
				marshal.Free();
			}
		}
	}
#if NETCOREAPP
	private static Boolean IsLiteralOrPinnable(CString? value)
	{
		if (value is null || value.IsZero) return true;
		if (!value.IsNullTerminated) return false;

		using MemoryHandle handle = value.TryPin(out Boolean pinned);
		if (pinned) return true;
		ref Byte refB = ref Unsafe.AsRef(in value.GetPinnableReference());
		return !MemoryInspector.MayBeNonLiteral(MemoryMarshal.CreateReadOnlySpan(ref refB, 1));
	}
#endif
	private static CString.Marshaller Marshal(CString? value)
	{
		CString.Marshaller marshal = new();
		marshal.FromManaged(value);
		PInvokeAssert.Null(marshal.ToManaged());
		return marshal;
	}
#if !NET6_0_OR_GREATER
	private static class Random
	{
		public static readonly System.Random Shared = new();
	}
#endif
}