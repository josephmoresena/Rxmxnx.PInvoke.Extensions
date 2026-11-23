// ReSharper disable MemberCanBeProtected.Global

namespace Rxmxnx.PInvoke.Internal;

#if !PACKAGE
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
#endif
internal unsafe partial class MemoryInspector
{
#if !PACKAGE || !NETSTANDARD2_1 && !NETCOREAPP2_1_OR_GREATER
	/// <summary>
	/// Indicates whether given span represents a literal or hardcoded memory region.
	/// </summary>
	/// <typeparam name="T">Type of items in <paramref name="span"/>.</typeparam>
	/// <param name="span">A read-only span of <typeparamref name="T"/> items.</param>
	/// <returns>
	/// <see langword="true"/> if the given span represents a literal o hardcoded memory region;
	/// otherwise, <see langword="false"/>.
	/// </returns>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
#if !PACKAGE && (NETSTANDARD2_1 || NETCOREAPP2_1_OR_GREATER)
	[ExcludeFromCodeCoverage]
#endif
	public Boolean IsLiteral<T>(ReadOnlySpan<T> span)
	{
#pragma warning disable CS8500
		fixed (void* ptr = &MemoryMarshal.GetReference(span))
#pragma warning restore CS8500
			return this.IsReadOnlyAddress(ptr);
	}
#endif
#if NETSTANDARD2_1 || NETCOREAPP2_1_OR_GREATER
	/// <summary>
	/// Indicates whether a given span represents a literal or hardcoded memory region.
	/// </summary>
	/// <param name="span">A read-only span of bytes.</param>
	/// <returns>
	/// <see langword="true"/> if the given span represents a literal o hardcoded memory region;
	/// otherwise, <see langword="false"/>.
	/// </returns>
	public Boolean IsLiteral(ReadOnlySpan<Byte> span)
	{
		fixed (void* ptr = &MemoryMarshal.GetReference(span))
			return this.IsReadOnlyAddress(ptr);
	}
#endif
}