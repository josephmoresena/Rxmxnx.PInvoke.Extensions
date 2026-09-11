#if NET462_OR_GREATER || NETSTANDARD2_0
namespace Rxmxnx.PInvoke.Internal.FrameworkCompat;

/// <summary>
/// Provides utility functions or compatibility features for handling UTF-8 encoding
/// within the internal framework compatibility layer.
/// </summary>
[SecurityCritical]
#if !PACKAGE
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
#endif
internal static unsafe class Utf8Compat
{
	/// <summary>
	/// Indicates whether the <see cref="Utf8"/> class is not available.
	/// </summary>
	private static Boolean utf8NotAllowed;

	/// <inheritdoc cref="Utf8.FromUtf16(ReadOnlySpan{Char}, Span{Byte}, out Int32, out Int32, Boolean, Boolean)"/>
	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void FromUtf16(ReadOnlySpan<Char> source, Span<Byte> destination, out Int32 charsRead,
		out Int32 bytesWritten)
	{
		if (!Volatile.Read(ref Utf8Compat.utf8NotAllowed))
			try
			{
				Utf8.FromUtf16(source, destination, out charsRead, out bytesWritten);
				return;
			}
			catch (TypeAccessException)
			{
				Volatile.Write(ref Utf8Compat.utf8NotAllowed, true);
			}
		fixed (Char* sourcePtr = source)
		fixed (Byte* destinationPtr = destination)
		{
			Encoding.UTF8.GetEncoder().Convert(sourcePtr, source.Length, destinationPtr, destination.Length, true,
			                                   out charsRead, out bytesWritten, out _);
		}
	}
	/// <inheritdoc cref="Utf8.ToUtf16(ReadOnlySpan{Byte}, Span{Char}, out Int32, out Int32, Boolean, Boolean)"/>
	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void ToUtf16(ReadOnlySpan<Byte> source, Span<Char> destination, out Int32 bytesRead,
		out Int32 charsWritten)
	{
		if (!Volatile.Read(ref Utf8Compat.utf8NotAllowed))
			try
			{
				Utf8.ToUtf16(source, destination, out bytesRead, out charsWritten);
				return;
			}
			catch (TypeAccessException)
			{
				Volatile.Write(ref Utf8Compat.utf8NotAllowed, true);
			}
		fixed (Byte* sourcePtr = source)
		fixed (Char* destinationPtr = destination)
		{
			Encoding.UTF8.GetDecoder().Convert(sourcePtr, source.Length, destinationPtr, destination.Length, true,
			                                   out bytesRead, out charsWritten, out _);
		}
	}
}
#endif