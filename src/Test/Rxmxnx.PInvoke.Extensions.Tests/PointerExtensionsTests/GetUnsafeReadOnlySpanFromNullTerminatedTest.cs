#if !NETCOREAPP
using Fact = NUnit.Framework.TestAttribute;
#endif

namespace Rxmxnx.PInvoke.Tests.PointerExtensionsTests;

[TestFixture]
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
	public void CharTest()
	{
		fixed (Char* sourcePtr = &MemoryMarshal.GetReference(GetUnsafeReadOnlySpanFromNullTerminatedTest.CharSpan))
		{
			ReadOnlyValPtr<Char> charPtr = sourcePtr;
			ReadOnlySpan<Char> charSpan = charPtr.GetUnsafeReadOnlySpanFromNullTerminated();

			PInvokeAssert.True(Unsafe.AreSame(
				                   ref MemoryMarshal.GetReference(GetUnsafeReadOnlySpanFromNullTerminatedTest.CharSpan),
#if NET8_0_OR_GREATER
				                   in charPtr.Reference));
#else
				            ref Unsafe.AsRef(in charPtr.Reference)));
#endif
			PInvokeAssert.True(Unsafe.AreSame(
				                   ref MemoryMarshal.GetReference(GetUnsafeReadOnlySpanFromNullTerminatedTest.CharSpan),
				                   ref MemoryMarshal.GetReference(charSpan)));
			PInvokeAssert.Equal(GetUnsafeReadOnlySpanFromNullTerminatedTest.CharSpan[..^1].Length, charSpan.Length);
			PInvokeAssert.True(charSpan.SequenceEqual(GetUnsafeReadOnlySpanFromNullTerminatedTest.CharSpan[..^1]));
		}
	}
	[Fact]
	public void ByteTest()
	{
		fixed (Byte* sourcePtr = &MemoryMarshal.GetReference(GetUnsafeReadOnlySpanFromNullTerminatedTest.ByteSpan))
		{
			ReadOnlyValPtr<Byte> bytePtr = sourcePtr;
			ReadOnlySpan<Byte> byteSpan = bytePtr.GetUnsafeReadOnlySpanFromNullTerminated();

			PInvokeAssert.True(Unsafe.AreSame(
				                   ref MemoryMarshal.GetReference(GetUnsafeReadOnlySpanFromNullTerminatedTest.ByteSpan),
#if NET8_0_OR_GREATER
				                   in bytePtr.Reference));
#else
				            ref Unsafe.AsRef(in bytePtr.Reference)));
#endif
			PInvokeAssert.True(Unsafe.AreSame(
				                   ref MemoryMarshal.GetReference(GetUnsafeReadOnlySpanFromNullTerminatedTest.ByteSpan),
				                   ref MemoryMarshal.GetReference(byteSpan)));
			PInvokeAssert.Equal(GetUnsafeReadOnlySpanFromNullTerminatedTest.ByteSpan.Length, byteSpan.Length);
			PInvokeAssert.True(byteSpan.SequenceEqual(GetUnsafeReadOnlySpanFromNullTerminatedTest.ByteSpan));
		}
	}
}