// ReSharper disable InconsistentNaming

#if !UAP10_0
namespace Rxmxnx.PInvoke.Internal;

internal partial class MemoryInspector
{
	/// <summary>
	/// Linux OS implementation of <see cref="MemoryInspector"/> class.
	/// </summary>
	private sealed partial class Linux
	{
		/// <summary>
		/// Interop API for <c>libdl</c>.
		/// </summary>
#if !PACKAGE
		[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
#endif
		private static unsafe class DynamicLinker
		{
			/// <summary>
			/// <c>libdl.so</c>. Android exports <c>dladdr</c> from this object.
			/// </summary>
			private const String libdl_so = "libdl.so";
			/// <summary>
			/// <c>libdl.so.2</c>. Glibc exports <c>dladdr</c> from this object, including 2.34 and later.
			/// </summary>
			private const String libdl_so_2 = "libdl.so.2";
			/// <summary>
			/// Represents the string constant <c>dladdr</c>, which corresponds to the symbol used for resolving
			/// dynamic linking information within a Linux environment.
			/// </summary>
			private const String dladdr = "dladdr";

#if NET5_0_OR_GREATER
			private static readonly delegate* unmanaged<IntPtr, void*, Int32> locateImage;
#else
			/// <summary>
			/// Shared object that exports <c>dladdr</c>.
			/// <see langword="null"/> selects <see cref="MapsInspector.StandardC"/>.
			/// </summary>
			private static readonly String? sharedObject;
#endif

			/// <summary>
			/// Static constructor.
			/// </summary>
#if !PACKAGE
			[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS3963)]
#endif
			static DynamicLinker()
			{
#if NET5_0_OR_GREATER
				if ((NativeLibrary.TryLoad(DynamicLinker.libdl_so_2, out IntPtr libHandle) ||
					    NativeLibrary.TryLoad(DynamicLinker.libdl_so, out libHandle)) &&
				    NativeLibrary.TryGetExport(libHandle, DynamicLinker.dladdr, out IntPtr symbol))
					DynamicLinker.locateImage = (delegate* unmanaged<IntPtr, void*, Int32>)symbol;
#elif NETCOREAPP3_0_OR_GREATER
				if (NativeLibrary.TryLoad(DynamicLinker.libdl_so_2, out IntPtr handle))
				{
					NativeLibrary.Free(handle);
					DynamicLinker.sharedObject = DynamicLinker.libdl_so_2;
				}
				else if (NativeLibrary.TryLoad(DynamicLinker.libdl_so, out handle))
				{
					NativeLibrary.Free(handle);
					DynamicLinker.sharedObject = DynamicLinker.libdl_so;
				}
#else
				if (DynamicLinker.IsGlibc())
					DynamicLinker.sharedObject = DynamicLinker.libdl_so_2;
				else if (File.Exists("/system/build.prop")) // Is Android
					DynamicLinker.sharedObject = DynamicLinker.libdl_so;
#endif
			}

			/// <summary>
			/// Locates the image associated with the specified memory address within the dynamically linked library.
			/// </summary>
			/// <param name="address">
			/// A pointer to the memory address for which the image information is to be located.
			/// </param>
			/// <returns>
			/// An integer indicating whether the image was successfully located.
			/// A non-zero value indicates success, while a zero value indicates failure.
			/// </returns>
			public static Int32 LocateImage(IntPtr address)
			{
				Byte* image = stackalloc Byte[4 * IntPtr.Size];
#if NET5_0_OR_GREATER
				// ReSharper disable once ConvertIfStatementToReturnStatement
				if (DynamicLinker.locateImage != default)
					return DynamicLinker.locateImage(address, image);
				return StandardC.LocateImage(address, image);
#else
				return DynamicLinker.sharedObject switch
				{
					DynamicLinker.libdl_so => DynamicLinker.LocateImage(address, image),
					DynamicLinker.libdl_so_2 => DynamicLinker.LocateImage2(address, image),
					_ => StandardC.LocateImage(address, image),
				};
#endif
			}

#if !NETCOREAPP3_0_OR_GREATER
			/// <summary>
			/// Indicates whether libc reports a glibc version.
			/// </summary>
			/// <returns><see langword="true"/> when <c>confstr</c> returns a glibc version.</returns>
			private static Boolean IsGlibc()
			{
				const Int32 gnuLibcVersion = 2; // _CS_GNU_LIBC_VERSION
				const Int32 valueLength = 32;
				Byte* value = stackalloc Byte[valueLength];
				UIntPtr written = DynamicLinker.GetConfigurationString(gnuLibcVersion, value, (UIntPtr)valueLength);
				return written.ToUInt64() > 5 && new ReadOnlySpan<Byte>(value, 5).SequenceEqual("glibc"u8);
			}
#pragma warning disable SYSLIB1054
			[DllImport("libc", EntryPoint = "confstr", SetLastError = false)]
			private static extern UIntPtr GetConfigurationString(Int32 name, Byte* value, UIntPtr length);
#pragma warning restore SYSLIB1054
#endif
#if !NET5_0_OR_GREATER
#pragma warning disable SYSLIB1054
			[DllImport(DynamicLinker.libdl_so, EntryPoint = DynamicLinker.dladdr, SetLastError = false)]
			private static extern Int32 LocateImage(IntPtr address, void* info);
			[DllImport(DynamicLinker.libdl_so_2, EntryPoint = DynamicLinker.dladdr, SetLastError = false)]
			private static extern Int32 LocateImage2(IntPtr address, void* info);
#pragma warning restore SYSLIB1054
#endif
		}
	}
}
#endif