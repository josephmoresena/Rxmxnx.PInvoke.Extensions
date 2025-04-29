namespace Rxmxnx.PInvoke.Tests.PointerExtensionsTests;

[ExcludeFromCodeCoverage]
public sealed class GetUnsafeReadOnlySpanFromNullTerminatedTest
{
	private static ReadOnlySpan<Byte> ByteSpan => "This is UTF-8 text span."u8;
	private static ReadOnlySpan<Char> CharSpan
		=>
		[
			'T', 'h', 'i', 's', ' ', 'i', 's', ' ', 'U', 'T', 'F', '-', '8', ' ', 't', 'e', 'x', 't', ' ', 's', 'p',
			'a', 'n', '.', '\0',
		];

	[Fact]
	internal void Test()
	{
		ReadOnlyValPtr<Byte> bytePtr = GetUnsafeReadOnlySpanFromNullTerminatedTest.ByteSpan.GetUnsafeValPtr();
		ReadOnlyValPtr<Char> charPtr = GetUnsafeReadOnlySpanFromNullTerminatedTest.CharSpan.GetUnsafeValPtr();

		Assert.True(Unsafe.AreSame(ref MemoryMarshal.GetReference(GetUnsafeReadOnlySpanFromNullTerminatedTest.ByteSpan),
		                           in bytePtr.Reference));
		Assert.True(Unsafe.AreSame(ref MemoryMarshal.GetReference(GetUnsafeReadOnlySpanFromNullTerminatedTest.CharSpan),
		                           in charPtr.Reference));

		ReadOnlySpan<Byte> byteSpan = bytePtr.GetUnsafeReadOnlySpanFromNullTerminated();
		ReadOnlySpan<Char> charSpan = charPtr.GetUnsafeReadOnlySpanFromNullTerminated();

		Assert.True(Unsafe.AreSame(ref MemoryMarshal.GetReference(GetUnsafeReadOnlySpanFromNullTerminatedTest.ByteSpan),
		                           ref MemoryMarshal.GetReference(byteSpan)));
		Assert.True(Unsafe.AreSame(ref MemoryMarshal.GetReference(GetUnsafeReadOnlySpanFromNullTerminatedTest.CharSpan),
		                           ref MemoryMarshal.GetReference(charSpan)));

		Assert.Equal(GetUnsafeReadOnlySpanFromNullTerminatedTest.ByteSpan.Length, byteSpan.Length);
		Assert.Equal(GetUnsafeReadOnlySpanFromNullTerminatedTest.CharSpan[..^1].Length, charSpan.Length);

		Assert.True(byteSpan.SequenceEqual(GetUnsafeReadOnlySpanFromNullTerminatedTest.ByteSpan));
		Assert.True(charSpan.SequenceEqual(GetUnsafeReadOnlySpanFromNullTerminatedTest.CharSpan[..^1]));
	}
}