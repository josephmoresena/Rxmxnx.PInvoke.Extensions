#if NETCOREAPP3_0_OR_GREATER
namespace Rxmxnx.PInvoke;

public partial class NativeUtilities
{
	/// <summary>
	/// Provides a high-level API for loading a native library.
	/// </summary>
	/// <param name="libraryName">The name of the native library to be loaded.</param>
	/// <param name="searchPath">The search path.</param>
	/// <returns>The OS handle for the loaded native library.</returns>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static IntPtr? LoadNativeLib(String? libraryName, DllImportSearchPath? searchPath = default)
	{
		if (String.IsNullOrWhiteSpace(libraryName)) return default;
		if (NativeLibrary.TryLoad(libraryName, Assembly.GetExecutingAssembly(), searchPath, out IntPtr handle) ||
		    (Path.IsPathFullyQualified(libraryName) && NativeLibrary.TryLoad(libraryName, out handle)))
			return handle;
		return default;
	}
	/// <summary>
	/// Provides a high-level API for loading a native library.
	/// </summary>
	/// <param name="libraryName">The name of the native library to be loaded.</param>
	/// <param name="unloadEvent">
	/// An optional event handler that is called when the library is unloaded. The handler's invocation includes a call
	/// to <see cref="NativeLibrary.Free(IntPtr)"/>.
	/// </param>
	/// <param name="searchPath">The search path.</param>
	/// <returns>The OS handle for the loaded native library.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static IntPtr? LoadNativeLib(String? libraryName, ref EventHandler? unloadEvent,
		DllImportSearchPath? searchPath = default)
	{
		// ReSharper disable once HeapView.ClosureAllocation
		IntPtr? handle = NativeUtilities.LoadNativeLib(libraryName, searchPath);
		if (handle.HasValue)
			// ReSharper disable once HeapView.DelegateAllocation
			unloadEvent += (_, _) => NativeLibrary.Free(handle.Value);
		return handle;
	}
	/// <summary>
	/// Gets the <typeparamref name="TDelegate"/> delegate of an exported symbol.
	/// </summary>
	/// <typeparam name="TDelegate">Type of the delegate corresponding to the exported symbol.</typeparam>
	/// <param name="handle">The native library OS handle.</param>
	/// <param name="name">The name of the exported symbol.</param>
	/// <returns><typeparamref name="TDelegate"/> delegate.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static TDelegate? GetNativeMethod<TDelegate>(IntPtr handle, String? name) where TDelegate : Delegate
	{
		if (handle != IntPtr.Zero && NativeLibrary.TryGetExport(handle, name ?? String.Empty, out IntPtr address))
			return Marshal.GetDelegateForFunctionPointer<TDelegate>(address);
		return default;
	}
	/// <summary>
	/// Gets a function pointer of type <typeparamref name="TDelegate"/> of an exported symbol.
	/// </summary>
	/// <typeparam name="TDelegate">Type of the delegate corresponding to the exported symbol.</typeparam>
	/// <param name="handle">The native library OS handle.</param>
	/// <param name="name">The name of the exported symbol.</param>
	/// <returns><typeparamref name="TDelegate"/> delegate.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static FuncPtr<TDelegate> GetNativeMethodPtr<TDelegate>(IntPtr handle, String? name)
		where TDelegate : Delegate
	{
		if (handle != IntPtr.Zero && NativeLibrary.TryGetExport(handle, name ?? String.Empty, out IntPtr address))
			return (FuncPtr<TDelegate>)address;
		return default;
	}
}
#endif