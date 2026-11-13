#if !UAP10_0
namespace Rxmxnx.PInvoke.Internal;

internal partial class MemoryInspector
{
	/// <summary>
	/// BSD OS memory maps-based implementation of <see cref="MemoryInspector"/> class.
	/// </summary>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
	[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
#endif
	private abstract partial class BsdInspector : MapsInspector
	{
		/// <summary>
		/// Appends to <paramref name="bsdInspector"/> the maps from <paramref name="maps"/>.
		/// </summary>
		/// <param name="maps">A pointer to <see cref="IVmMap"/> buffer.</param>
		/// <param name="count">The number of <see cref="IVmMap"/> instances in the buffer.</param>
		/// <param name="bsdInspector">A <see cref="MemoryInspector.BsdInspector"/> instance.</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#if !PACKAGE
		[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
#endif
		private protected static unsafe void AppendMaps<TMap>(void* maps, UInt32 count, BsdInspector bsdInspector)
			where TMap : unmanaged, IVmMap
		{
			ref TMap mapRef = ref Unsafe.AsRef<TMap>(maps);
			while (count > 0)
			{
				Boolean isReadOnly = mapRef.IsReadOnly;
				bsdInspector.AddBoundary(new(mapRef.StartAddress, false), isReadOnly);
				bsdInspector.AddBoundary(new(mapRef.EndAddress, true), isReadOnly);
				mapRef = ref Unsafe.AddByteOffset(ref mapRef, mapRef.StructSize);
				count--;
			}
		}
	}
}
#endif