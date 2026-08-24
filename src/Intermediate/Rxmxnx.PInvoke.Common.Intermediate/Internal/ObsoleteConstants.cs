#if OBSOLETE_FIXED_INTERFACES || OBSOLTE_DELEGATES
namespace Rxmxnx.PInvoke.Internal;

/// <summary>
/// Internal obsolete constants
/// </summary>
internal static class ObsoleteConstants
{
#if NETSTANDARD2_1 || NETCOREAPP3_0_OR_GREATER
	/// <summary>
	/// Message for obsolete delegates types.
	/// </summary>
	public const String ObsoleteDelegateTypes = "Use functional interfaces";
	/// <summary>
	/// Message for obsolete delegates extensions.
	/// </summary>
	public const String ObsoleteDelegateExtensions = "Use functional interface extensions";
	/// <summary>
	/// Message for obsolete delegates methods.
	/// </summary>
	public const String ObsoleteDelegateMethods = "Use functional interface method overloads";
#endif
	/// <summary>
	/// Message for obsolete fixed interfaces types.
	/// </summary>
	public const String ObsoleteFixedInterface = "Use fixed ref-struct types";
#if NETSTANDARD2_1 || NETCOREAPP3_0_OR_GREATER
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
	/// Error for obsolete delegates.
	/// </summary>
#if !PACKAGE || OBSOLETE_FIXED_INTERFACES
	public const Boolean ErrorDelegate = true;
#else
	public const Boolean ErrorDelegate = false;
#endif
#endif
	/// <summary>
	/// Error for obsolete fixed interfaces.
	/// </summary>
#if !PACKAGE
	public const Boolean ErrorFixedInterface = true;
#else
	public const Boolean ErrorFixedInterface = false;
#endif
}
#endif