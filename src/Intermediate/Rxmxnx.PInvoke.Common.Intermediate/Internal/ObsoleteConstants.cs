#if OBSOLETE_FIXED_INTERFACES || OBSOLTE_DELEGATES
namespace Rxmxnx.PInvoke.Internal;

/// <summary>
/// Internal obsolete constants
/// </summary>
internal static class ObsoleteConstants
{
	/// <summary>
	/// Message for obsolete delegates types.
	/// </summary>
	public const String ObsoleteDelegateTypes = "Use functional interfaces";
	/// <summary>
	/// Message for obsolete CStringSequence delegates types.
	/// </summary>
	public const String ObsoleteSequenceDelegateTypes = "Call to Pin method and then use CreateView extension method.";
	/// <summary>
	/// Message for obsolete delegates extensions.
	/// </summary>
	public const String ObsoleteDelegateExtensions = "Use functional interface extensions";
	/// <summary>
	/// Message for obsolete delegates methods.
	/// </summary>
	public const String ObsoleteDelegateMethods = "Use functional interface method overloads";
	/// <summary>
	/// Error for obsolete delegates.
	/// </summary>
	public const Boolean ErrorDelegate = true;
	/// <summary>
	/// Message for obsolete fixed interfaces types.
	/// </summary>
	public const String ObsoleteFixedInterface = "Use fixed ref-struct types";
	/// <summary>
	/// Message for obsolete fixed interfaces types.
	/// </summary>
	public const String ObsoleteFixedMemoryList = "Use fixed FixedPointerValueList type";
	/// <summary>
	/// Message for obsolete fixed interfaces extensions.
	/// </summary>
	public const String ObsoleteFixedInterfaceExtensions = "Use fixed ref-struct types extensions";
	/// <summary>
	/// Message for obsolete fixed interfaces methods.
	/// </summary>
	public const String ObsoleteFixedInterfaceMethods = "Use fixed ref-struct method overloads";
	/// <summary>
	/// Error for obsolete fixed interfaces.
	/// </summary>
	public const Boolean ErrorFixedInterface = true;
}
#endif