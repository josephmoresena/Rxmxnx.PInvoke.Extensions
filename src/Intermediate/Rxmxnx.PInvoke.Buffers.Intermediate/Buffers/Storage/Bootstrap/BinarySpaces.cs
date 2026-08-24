#if NET8_0_OR_GREATER
namespace Rxmxnx.PInvoke.Buffers.Storage.Bootstrap;

/// <summary>
/// Represents a binary space.
/// </summary>
internal interface IBinarySpace
{
	/// <summary>
	/// Dimension of binary space.
	/// </summary>
	static abstract Int32 Dimension { get; }
}

/// <summary>
/// 2^11-1 space.
/// </summary>
internal readonly struct Space11 : IBinarySpace
{
	/// <inheritdoc/>
	public static Int32 Dimension
	{
#if !PACKAGE
		[ExcludeFromCodeCoverage]
#endif
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => 11;
	}
}

/// <summary>
/// 2^16-1 space.
/// </summary>
internal readonly struct Space16 : IBinarySpace
{
	/// <inheritdoc/>
	public static Int32 Dimension
	{
#if !PACKAGE
		[ExcludeFromCodeCoverage]
#endif
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => 16;
	}
}
#endif