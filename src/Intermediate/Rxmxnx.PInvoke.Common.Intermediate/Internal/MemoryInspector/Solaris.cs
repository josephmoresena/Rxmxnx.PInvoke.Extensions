namespace Rxmxnx.PInvoke.Internal;

internal partial class MemoryInspector
{
	/// <summary>
	/// Solaris OS implementation of <see cref="MemoryInspector"/> class.
	/// </summary>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
	[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
#endif
	private sealed unsafe partial class Solaris : MapsInspector
	{
		/// <summary>
		/// <c>/proc/self/maps</c> file name.
		/// </summary>
		private const String mapsFileName = "/proc/self/map";

		/// <inheritdoc/>
		protected override void ProcessMaps()
		{
			ReadOnlySpan<ProcessMap> maps = Solaris.GetMaps();
			foreach (ProcessMap map in maps)
			{
				Boolean isReadOnly = map.Flags is MapFlags.Read and not MapFlags.Write;
				this.AddBoundary(new(map.Address, false), isReadOnly);
#if NET7_0_OR_GREATER
				this.AddBoundary(new(map.Address + map.Size, true), isReadOnly);
#else
				this.AddBoundary(new((UInt64)map.Address + (UInt64)map.Size, true), false);
#endif
			}
		}
		/// <summary>
		/// Retrieves the current process maps.
		/// </summary>
		/// <returns>A read-only span of <see cref="ProcessMaps"/> values.</returns>
		private static ReadOnlySpan<ProcessMap> GetMaps()
		{
			Byte[] bytes = File.ReadAllBytes(Solaris.mapsFileName);
			Int32 mapsCount = (Int32)(bytes.LongLength / sizeof(ProcessMap));
			ref ProcessMap mapRef = ref Unsafe.As<Byte, ProcessMap>(ref MemoryMarshal.GetReference(bytes.AsSpan()));
			ReadOnlySpan<ProcessMap> maps = MemoryMarshal.CreateReadOnlySpan(ref mapRef, mapsCount);
			return maps;
		}
	}
}