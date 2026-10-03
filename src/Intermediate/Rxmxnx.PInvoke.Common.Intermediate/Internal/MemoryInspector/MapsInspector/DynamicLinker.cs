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
			/// <c>RTLD_NOLOAD</c>. Return a handle only when the named library is already mapped.
			/// </summary>
			private const Int32 noLoad = 4;

			/// <summary>
			/// Looks up <paramref name="symbolName"/> in an already mapped library.
			/// </summary>
			/// <param name="libraryName">First library name to look up.</param>
			/// <param name="symbolName">Symbol to locate.</param>
			/// <param name="libraryName2">
			/// Second library name. Used only when <paramref name="libraryName"/> is not mapped.
			/// </param>
			/// <returns>
			/// A pointer to the symbol when found; otherwise, <see cref="IntPtr.Zero"/>.
			/// </returns>
			public static IntPtr GetExport(ReadOnlySpan<Byte> libraryName, ReadOnlySpan<Byte> symbolName,
				ReadOnlySpan<Byte> libraryName2)
			{
				IntPtr library = DynamicLinker.LoadLibrary(libraryName);
				if (library == IntPtr.Zero && (library = DynamicLinker.LoadLibrary(libraryName2)) == IntPtr.Zero)
					return IntPtr.Zero;
				fixed (Byte* symbolNamePtr = &MemoryMarshal.GetReference(symbolName))
					return DynamicLinker.LookupSymbol(library, symbolNamePtr);
			}

			/// <summary>
			/// Returns a handle for <paramref name="libraryName"/> when that library is already mapped.
			/// </summary>
			/// <param name="libraryName">Library name to look up.</param>
			/// <returns>
			/// The existing handle, or <see cref="IntPtr.Zero"/> when the name is empty or the library is not mapped.
			/// </returns>
			private static IntPtr LoadLibrary(ReadOnlySpan<Byte> libraryName)
			{
				if (libraryName.IsEmpty) return IntPtr.Zero;
				fixed (Byte* libraryNamePtr = &MemoryMarshal.GetReference(libraryName))
					return DynamicLinker.OpenLibrary(libraryNamePtr, DynamicLinker.loadNow | DynamicLinker.noLoad);
			}
#pragma warning disable SYSLIB1054
			[DllImport("libdl", EntryPoint = "dlopen", SetLastError = false)]
			private static extern IntPtr OpenLibrary(Byte* libraryName, Int32 mode);
			[DllImport("libdl", EntryPoint = "dlsym", SetLastError = false)]
			private static extern IntPtr LookupSymbol(IntPtr library, Byte* symbolName);
#pragma warning restore SYSLIB1054
		}
	}
}
#endif