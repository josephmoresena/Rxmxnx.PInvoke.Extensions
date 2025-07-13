namespace Rxmxnx.PInvoke.Tests.PointerExtensionsTests;

[ExcludeFromCodeCoverage]
[SuppressMessage("csharpsquid", "S2699")]
public sealed unsafe class GetUnsafeReadOnlySpanFromNullTerminatedTest
{
	private static ReadOnlySpan<Byte> ByteSpan => "This is UTF-8 text span."u8;
	private static ReadOnlySpan<Char> CharSpan
		=>
		[
			'T', 'h', 'i', 's', ' ', 'i', 's', ' ', 'U', 'T', 'F', '-', '8', ' ', 't', 'e', 'x', 't', ' ', 's', 'p',
			'a', 'n', '.', '\0',
		];

	[Fact]
	internal void CharTest()
	{
		fixed (Char* sourcePtr = &MemoryMarshal.GetReference(GetUnsafeReadOnlySpanFromNullTerminatedTest.CharSpan))
		{
			ReadOnlyValPtr<Char> charPtr = sourcePtr;
			ReadOnlySpan<Char> charSpan = charPtr.GetUnsafeReadOnlySpanFromNullTerminated();

			Assert.True(Unsafe.AreSame(
				            ref MemoryMarshal.GetReference(GetUnsafeReadOnlySpanFromNullTerminatedTest.CharSpan),
#if NET8_0_OR_GREATER
				            in charPtr.Reference));
#else
				            ref Unsafe.AsRef(in charPtr.Reference)));
#endif
			Assert.True(Unsafe.AreSame(
				            ref MemoryMarshal.GetReference(GetUnsafeReadOnlySpanFromNullTerminatedTest.CharSpan),
				            ref MemoryMarshal.GetReference(charSpan)));
			Assert.Equal(GetUnsafeReadOnlySpanFromNullTerminatedTest.CharSpan[..^1].Length, charSpan.Length);
			Assert.True(charSpan.SequenceEqual(GetUnsafeReadOnlySpanFromNullTerminatedTest.CharSpan[..^1]));
		}
	}
	[Fact]
	internal void ByteTest()
	{
		fixed (Byte* sourcePtr = &MemoryMarshal.GetReference(GetUnsafeReadOnlySpanFromNullTerminatedTest.ByteSpan))
		{
			ReadOnlyValPtr<Byte> bytePtr = sourcePtr;
			ReadOnlySpan<Byte> byteSpan = bytePtr.GetUnsafeReadOnlySpanFromNullTerminated();

			Assert.True(Unsafe.AreSame(
				            ref MemoryMarshal.GetReference(GetUnsafeReadOnlySpanFromNullTerminatedTest.ByteSpan),
#if NET8_0_OR_GREATER
				            in bytePtr.Reference));
#else
				            ref Unsafe.AsRef(in bytePtr.Reference)));
#endif
			Assert.True(Unsafe.AreSame(
				            ref MemoryMarshal.GetReference(GetUnsafeReadOnlySpanFromNullTerminatedTest.ByteSpan),
				            ref MemoryMarshal.GetReference(byteSpan)));
			Assert.Equal(GetUnsafeReadOnlySpanFromNullTerminatedTest.ByteSpan.Length, byteSpan.Length);
			Assert.True(byteSpan.SequenceEqual(GetUnsafeReadOnlySpanFromNullTerminatedTest.ByteSpan));
		}
	}
}