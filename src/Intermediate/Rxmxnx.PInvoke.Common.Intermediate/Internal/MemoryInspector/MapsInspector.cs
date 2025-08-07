namespace Rxmxnx.PInvoke.Internal;

internal partial class MemoryInspector
{
	/// <summary>
	/// Unix-like OS memory maps-based implementation of <see cref="MemoryInspector"/> class.
	/// </summary>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
	[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
#endif
	private abstract unsafe partial class MapsInspector : MemoryInspector
	{
		/// <summary>
		/// Minimum time (ms) between maps refresh to avoid redundant access across all threads.
		/// </summary>
		private const Int64 globalReadDelay = 256;
		/// <summary>
		/// Minimum time (ms) between maps refresh within the same thread.
		/// </summary>
		private const Int64 localReadDelay = 512;

		/// <summary>
		/// Last tick count for current thread.
		/// </summary>
		[ThreadStatic]
#if !PACKAGE
		[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS2696)]
#endif
		private static Int64 lastThreadTickCount;

		/// <summary>
		/// Lock object.
		/// </summary>
		private readonly Object _lock = new();
		/// <summary>
		/// Set of read-only memory boundaries.
		/// </summary>
		private readonly SortedSet<MemoryBoundary> _readonlyMemory = [];
		/// <summary>
		/// Set of read-write memory boundaries.
		/// </summary>
		private readonly SortedSet<MemoryBoundary> _readwriteMemory = [];

		/// <summary>
		/// Last ticks count.
		/// </summary>
		private Int64 _lastTickCount = -1;

		/// <summary>
		/// Parameterless constructor.
		/// </summary>
#if !PACKAGE
		[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS1144)]
#endif
		protected MapsInspector()
		{
			try
			{
				this.RefreshMaps();
			}
			catch (Exception)
			{
				// Experimental features.
			}
		}

		/// <inheritdoc/>
		public sealed override Boolean IsLiteral(ReadOnlySpan<Byte> span)
		{
			fixed (void* ptr = &MemoryMarshal.GetReference(span))
			{
				lock (this._lock)
				{
					if (this.TryGetProtection(ptr, out Boolean isReadOnly))
						return isReadOnly;
					this.RefreshMaps();
					return this.TryGetProtection(ptr, out isReadOnly) && isReadOnly;
				}
			}
		}

		/// <summary>
		/// Adds <paramref name="value"/> to maps boundaries.
		/// </summary>
		/// <param name="value">A <see cref="MemoryBoundary"/> instance.</param>
		/// <param name="isReadOnly">Indicates whether <paramref name="value"/> is read-only boundary.</param>
		protected void AddBoundary(MemoryBoundary value, Boolean isReadOnly)
		{
			SortedSet<MemoryBoundary> maps = isReadOnly ? this._readonlyMemory : this._readwriteMemory;
			MapsInspector.AddBoundary(maps, value);
		}
		/// <summary>
		/// Processes memory maps from current process.
		/// </summary>
		protected abstract void ProcessMaps();
		/// <summary>
		/// Refresh memories boundaries.
		/// </summary>
#if !PACKAGE
		[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS2696)]
#endif
		private void RefreshMaps()
		{
			Int64 tickCount
#if NETCOREAPP
				= Environment.TickCount64;
#else
				= DateTime.Now.Ticks;
#endif
			if (tickCount - this._lastTickCount < MapsInspector.globalReadDelay ||
			    tickCount - MapsInspector.lastThreadTickCount < MapsInspector.localReadDelay)
				return;

			this.ProcessMaps();
			this._lastTickCount
#if NETCOREAPP
				= Environment.TickCount64;
#else
				= DateTime.Now.Ticks;
#endif
			MapsInspector.lastThreadTickCount = this._lastTickCount;
		}
		/// <summary>
		/// Tries to retrieve <paramref name="address"/> protection.
		/// </summary>
		/// <param name="address">Address to check.</param>
		/// <param name="isReadOnly"> Output. Indicates whether <paramref name="address"/> is read-only.</param>
		/// <returns>
		/// <see langword="true"/> if <paramref name="address"/> protection is known; otherwise,
		/// <see langword="false"/>.
		/// </returns>
		private Boolean TryGetProtection(void* address, out Boolean isReadOnly)
		{
			if (MapsInspector.IsMappedAddress(this._readonlyMemory, address))
			{
				isReadOnly = true;
				return true;
			}
			if (MapsInspector.IsMappedAddress(this._readwriteMemory, address))
			{
				isReadOnly = false;
				return true;
			}
			Unsafe.SkipInit(out isReadOnly);
			return false;
		}

		/// <summary>
		/// Adds <paramref name="value"/> inside <paramref name="maps"/>.
		/// </summary>
		/// <param name="maps">Mapped section.</param>
		/// <param name="value">A <see cref="MemoryBoundary"/> instance.</param>
		private static void AddBoundary(SortedSet<MemoryBoundary> maps, MemoryBoundary value)
		{
			if (maps.TryGetValue(value, out MemoryBoundary existing))
			{
				if (value.IsEnd != existing.IsEnd)
					maps.Remove(existing);
				return;
			}
			maps.Add(value);
		}
		/// <summary>
		/// Indicates whether <paramref name="address"/> is inside <paramref name="maps"/>.
		/// </summary>
		/// <param name="maps">Mapped section.</param>
		/// <param name="address">Address to check.</param>
		/// <returns>
		/// <see langword="true"/> if <paramref name="address"/> is inside <paramref name="maps"/>; otherwise,
		/// <see langword="false"/>.
		/// </returns>
		private static Boolean IsMappedAddress(SortedSet<MemoryBoundary> maps, void* address)
		{
			if (maps.Count == 0) return false;
			SortedSet<MemoryBoundary> view = maps.GetViewBetween(maps.Min, address);
			if (view.Count == 0) return false;
			MemoryBoundary boundary = view.Max;
			return boundary != default && !boundary.IsEnd;
		}
	}
}