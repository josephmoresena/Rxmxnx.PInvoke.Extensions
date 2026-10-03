#if !UAP10_0
namespace Rxmxnx.PInvoke.Internal;

internal partial class MemoryInspector
{
	/// <summary>
	/// Linux OS implementation of <see cref="MemoryInspector"/> class.
	/// </summary>
#if !PACKAGE
	[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
#endif
	private sealed partial class Linux
	{
		/// <summary>
		/// Calls <c>dladdr</c> when an already loaded <c>libdl</c> exports it.
		/// </summary>
		private static unsafe class DynamicLinkerLinux
		{
#if NET5_0_OR_GREATER
			/// <summary>
			/// <c>dladdr</c> exported by <c>libdl</c>, or <see langword="null"/> when that export is missing.
			/// </summary>
			private static delegate* unmanaged<IntPtr, void*, Int32> LocateImage { get; }
#else
			/// <summary>
			/// <see langword="true"/> when <c>libdl</c> exports <c>dladdr</c>.
			/// </summary>
			private static readonly Boolean allowPinvoke;
#endif

			/// <summary>
			/// Resolves <c>dladdr</c> from <c>libdl.so.2</c>, then from <c>libdl.so</c>.
			/// </summary>
#if !PACKAGE
			[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS3963)]
#endif
			static DynamicLinkerLinux()
			{
				IntPtr symbol = DynamicLinker.GetExport("libdl.so.2"u8, "dladdr"u8, "libdl.so"u8);
#if NET5_0_OR_GREATER
				DynamicLinkerLinux.LocateImage = (delegate* unmanaged<IntPtr, void*, Int32>)symbol;
#else
				DynamicLinkerLinux.allowPinvoke = symbol != IntPtr.Zero;
#endif
			}

			/// <summary>
			/// Locates <paramref name="address"/> through <c>libdl</c> when that library exports <c>dladdr</c>.
			/// </summary>
			/// <param name="address">Function address.</param>
			/// <param name="located">
			/// Native <c>dladdr</c> result when this method returns <see langword="true"/>. Uninitialized otherwise.
			/// </param>
			/// <returns>
			/// <see langword="true"/> when <c>libdl</c> exported <c>dladdr</c>; otherwise, <see langword="false"/>.
			/// </returns>
			public static Boolean TryLocateImage(IntPtr address, out Int32 located)
			{
#if NET5_0_OR_GREATER
				if (DynamicLinkerLinux.LocateImage == default)
#else
				if (!DynamicLinkerLinux.allowPinvoke)
#endif
				{
					Unsafe.SkipInit(out located);
					return false;
				}
				Byte* image = stackalloc Byte[4 * IntPtr.Size];
				located = DynamicLinkerLinux.LocateImage(address, image);
				return true;
			}

#if !NET5_0_OR_GREATER
#pragma warning disable SYSLIB1054
			[DllImport("libdl", EntryPoint = "dladdr", SetLastError = false)]
			private static extern Int32 LocateImage(IntPtr address, void* info);
#pragma warning restore SYSLIB1054
#endif
		}
	}
}
#endif