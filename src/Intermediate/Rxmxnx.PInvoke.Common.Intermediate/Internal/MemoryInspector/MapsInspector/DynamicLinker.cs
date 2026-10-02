#if !UAP10_0
namespace Rxmxnx.PInvoke.Internal;

internal partial class MemoryInspector
{
	private abstract partial class MapsInspector
	{
		/// <summary>
		/// Interop API for <c>libdl</c>.
		/// </summary>
#if !PACKAGE
		[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
#endif
		protected static unsafe class DynamicLinker
		{
			/// <summary>
			/// <c>RTLD_NOW</c>.
			/// </summary>
			private const Int32 loadNow = 2;
			/// <summary>
			/// <c>RTLD_NOLOAD</c>. Do not map <c>libdl</c> when the process has not loaded it.
			/// </summary>
			private const Int32 noLoad = 4;

#if !NET5_0_OR_GREATER
			/// <summary>
			/// Managed call to <c>dladdr</c>.
			/// </summary>
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			private delegate Int32 LocateImageDelegate(IntPtr address, void* image);
#endif
			/// <summary>
			/// Loaded <c>dladdr</c> export, when <c>libdl</c> provides it.
			/// </summary>
#if NET5_0_OR_GREATER
			private static readonly delegate* unmanaged[Cdecl]<IntPtr, void*, Int32> locateImage =
				DynamicLinker.Resolve();
#else
			private static readonly LocateImageDelegate? locateImage = DynamicLinker.Resolve();
#endif

			/// <summary>
			/// Locates <paramref name="address"/> through <c>libdl</c> when that library exports <c>dladdr</c>.
			/// </summary>
			/// <param name="address">Function address.</param>
			/// <param name="located">Native <c>dladdr</c> result.</param>
			/// <returns>
			/// <see langword="true"/> when <c>libdl</c> exported <c>dladdr</c>; otherwise, <see langword="false"/>.
			/// </returns>
			public static Boolean TryLocateImage(IntPtr address, out Int32 located)
			{
				if (DynamicLinker.locateImage == default)
				{
					located = 0;
					return false;
				}
				Span<Byte> image = stackalloc Byte[4 * IntPtr.Size];
				fixed (void* imagePtr = &MemoryMarshal.GetReference(image))
					located = DynamicLinker.locateImage(address, imagePtr);
				return true;
			}
			/// <summary>
			/// Resolves <c>dladdr</c> from an already loaded <c>libdl</c>.
			/// </summary>
			/// <returns>The exported function, or <see langword="null"/> when <c>libdl</c> does not provide it.</returns>
#if NET5_0_OR_GREATER
			private static delegate* unmanaged[Cdecl]<IntPtr, void*, Int32> Resolve()
#else
			private static LocateImageDelegate? Resolve()
#endif
			{
				try
				{
					IntPtr library =
						DynamicLinker.OpenLibrary("libdl.so.2", DynamicLinker.loadNow | DynamicLinker.noLoad);
					if (library == IntPtr.Zero)
						library = DynamicLinker.OpenLibrary("libdl.so", DynamicLinker.loadNow | DynamicLinker.noLoad);
					if (library == IntPtr.Zero) return null;
					IntPtr symbol = DynamicLinker.LookupSymbol(library, "dladdr");
					if (symbol == IntPtr.Zero) return null;
#if NET5_0_OR_GREATER
					return (delegate* unmanaged[Cdecl]<IntPtr, void*, Int32>)symbol;
#else
					return (LocateImageDelegate)Marshal.GetDelegateForFunctionPointer(
						symbol, typeof(LocateImageDelegate));
#endif
				}
				catch (DllNotFoundException)
				{
					return default;
				}
				catch (EntryPointNotFoundException)
				{
					return default;
				}
			}
#pragma warning disable SYSLIB1054
			[DllImport("libdl", EntryPoint = "dlopen", CharSet = CharSet.Ansi, SetLastError = false)]
			private static extern IntPtr OpenLibrary(String libraryName, Int32 mode);
			[DllImport("libdl", EntryPoint = "dlsym", CharSet = CharSet.Ansi, SetLastError = false)]
			private static extern IntPtr LookupSymbol(IntPtr library, String symbolName);
#pragma warning restore SYSLIB1054
		}
	}
}
#endif