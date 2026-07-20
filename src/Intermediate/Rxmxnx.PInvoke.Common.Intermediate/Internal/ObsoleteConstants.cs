#if OBSOLETE_FIXED_INTERFACES || OBSOLTE_DELEGATES
namespace Rxmxnx.PInvoke.Internal;

/// <summary>
/// Internal obsolete constants
/// </summary>
internal static class ObsoleteConstants
{
	/// <summary>
	/// Message for obsolete delegates.
	/// </summary>
	public const String ObsoleteDelegate = "Use functional interface extensions";
	/// <summary>
	/// Error for obsolete delegates.
	/// </summary>
	public const Boolean ErrorDelegate = true;
	/// <summary>
	/// Message for obsolete fixed interfaces.
	/// </summary>
	public const String ObsoleteFixedInterface = "Use fixed ref-struct types";
	/// <summary>
	/// Error for obsolete fixed interfaces.
	/// </summary>
	public const Boolean ErrorFixedInterface = true;
}
#endif