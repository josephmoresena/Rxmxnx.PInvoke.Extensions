namespace Rxmxnx.PInvoke.Tests.CStringTests;

[ExcludeFromCodeCoverage]
public sealed unsafe class Marshaller
{
	[Fact]
	internal void FromManagedTest()
	{
		Int32 index = Random.Shared.Next(0, TestSet.Utf16Text.Count);
		ReadOnlySpan<Byte> utf8Span = TestSet.Utf8Text[index]();
		IntPtr ptr = (IntPtr)Unsafe.AsPointer(ref MemoryMarshal.GetReference(utf8Span));

		Marshaller.AssertToUnmanaged(new(TestSet.Utf8Text[index]));
		Marshaller.AssertToUnmanaged(TestSet.Utf8NullTerminatedBytes[index]);
		Marshaller.AssertToUnmanaged(TestSet.Utf8Bytes[index]);
		Marshaller.AssertToUnmanaged((CString)TestSet.Utf16Text[index]);
		Marshaller.AssertToUnmanaged(CString.CreateUnsafe(ptr, utf8Span.Length, true));
		Marshaller.AssertToUnmanaged(CString.CreateUnsafe(ptr, utf8Span.Length + 1));
	}
	[Fact]
	internal void FromUnmanagedTest()
	{
		Int32 index = Random.Shared.Next(0, TestSet.Utf16Text.Count);
		ReadOnlySpan<Byte> utf8Span = TestSet.Utf8Text[index]();
		IntPtr ptr = (IntPtr)Unsafe.AsPointer(ref MemoryMarshal.GetReference(utf8Span));
		CString.Marshaller marshal = new();
		try
		{
			marshal.FromUnmanaged(ptr);
			Assert.Equal(IntPtr.Zero, marshal.ToUnmanaged());

			CString? value = marshal.ToManaged();
			Assert.NotNull(value);
			Assert.True(Unsafe.AreSame(ref MemoryMarshal.GetReference(utf8Span),
			                           ref MemoryMarshal.GetReference<Byte>(value)));
			Assert.StrictEqual(value, marshal.ToManaged());
			Assert.True(utf8Span.SequenceEqual(value));
			Assert.Equal(ptr, marshal.ToUnmanaged());
		}
		finally
		{
			marshal.Free();
		}
	}
	[Fact]
	internal void EmptyTest()
	{
		Marshaller.AssertToUnmanaged(null);
		Marshaller.AssertToUnmanaged(CString.Zero);
		Marshaller.AssertToUnmanaged(CString.Empty);
	}

	private static void AssertToUnmanaged(CString? value)
	{
		ReadOnlySpan<Byte> utfSpan = MemoryMarshal.CreateReadOnlySpanFromNullTerminated(
			(Byte*)Unsafe.AsPointer(ref MemoryMarshal.GetReference<Byte>(value)));
		CString.Marshaller marshal = Marshaller.Marshal(value);
		IntPtr ptr = marshal.ToUnmanaged();
		try
		{
			Boolean isLiteralOrPinnable = Marshaller.IsLiteralOrPinnable(value);
			ReadOnlySpan<Byte> unmanagedSpan = new(ptr.ToPointer(), value?.Length ?? 0);
			Assert.Equal(isLiteralOrPinnable,
			             Unsafe.AreSame(ref MemoryMarshal.GetReference<Byte>(value),
			                            ref MemoryMarshal.GetReference(unmanagedSpan)));
			Assert.True(utfSpan.SequenceEqual(MemoryMarshal.CreateReadOnlySpanFromNullTerminated((Byte*)ptr)));
			Assert.Equal(utfSpan.Length, MemoryMarshalCompat.IndexOfNull(ref MemoryMarshal.GetReference<Byte>(value)));
			Assert.Equal(ptr, marshal.ToUnmanaged());
		}
		finally
		{
			marshal.Free();
		}
	}
	private static Boolean IsLiteralOrPinnable(CString? value)
	{
		if (value is null) return true;
		if (!value.IsNullTerminated) return false;

		using MemoryHandle handle = value.TryPin(out Boolean pinned);
		if (pinned) return true;
		try
		{
			return MemoryInspector.Instance.IsLiteral(value.AsSpan());
		}
		catch (PlatformNotSupportedException)
		{
			return false;
		}
	}
	private static CString.Marshaller Marshal(CString? value)
	{
		CString.Marshaller marshal = new();
		marshal.FromManaged(value);
		Assert.Null(marshal.ToManaged());
		return marshal;
	}
}