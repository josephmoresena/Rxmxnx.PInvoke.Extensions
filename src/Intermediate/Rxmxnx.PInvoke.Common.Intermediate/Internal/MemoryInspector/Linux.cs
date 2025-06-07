namespace Rxmxnx.PInvoke.Internal;

internal partial class MemoryInspector
{
	/// <summary>
	/// Linux OS implementation of <see cref="MemoryInspector"/> class.
	/// </summary>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
	[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
#endif
	private sealed unsafe partial class Linux : MemoryInspector
	{
		/// <summary>
		/// <c>/proc/self/maps</c> file name.
		/// </summary>
		private const String MapsFile = "/proc/self/maps";
		/// <summary>
		/// Minimum time (ms) between reads of <c>/proc/self/maps</c> to avoid redundant access across all threads.
		/// </summary>
		private const Int64 GlobalFileReadDelay = 256;
		/// <summary>
		/// Minimum time (ms) between reads of <c>/proc/self/maps</c> within the same thread.
		/// </summary>
		private const Int64 LocalFileReadDelay = 512;
		/// <summary>
		/// Token permission length.
		/// </summary>
		private const Int32 PermissionTokenLength = 6;

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
		public Linux() => this.RefreshMaps();

		/// <inheritdoc/>
		public override Boolean IsLiteral<T>(ReadOnlySpan<T> span)
		{
#pragma warning disable CS8500
			fixed (void* ptr = &MemoryMarshal.GetReference(span))
#pragma warning restore CS8500
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
			if (Linux.IsMappedAddress(this._readonlyMemory, address))
			{
				isReadOnly = true;
				return true;
			}
			if (Linux.IsMappedAddress(this._readwriteMemory, address))
			{
				isReadOnly = false;
				return true;
			}
#if !NETCOREAPP && PACKAGE || NETCOREAPP3_1_OR_GREATERR
			Unsafe.SkipInit(out isReadOnly);
#else
			isReadOnly = false;
#endif
			return false;
		}
		/// <summary>
		/// Reads <c>/proc/self/maps</c> file and refresh memories boundaries.
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
			if (tickCount - this._lastTickCount < Linux.GlobalFileReadDelay ||
			    tickCount - Linux.lastThreadTickCount < Linux.LocalFileReadDelay)
				return;

			this.ParseMaps(File.ReadAllBytes(Linux.MapsFile));
			this._lastTickCount
#if NETCOREAPP
				= Environment.TickCount64;
#else
				= DateTime.Now.Ticks;
#endif
			Linux.lastThreadTickCount = this._lastTickCount;
		}
		/// <summary>
		/// Parses <paramref name="mapsBytes"/> to addresses boundary.
		/// </summary>
		/// <param name="mapsBytes">Read <c>/proc/self/maps</c> bytes.</param>
		private void ParseMaps(Span<Byte> mapsBytes)
		{
			FileState state = new(mapsBytes) { ReadBytes = IntPtr.Size == 4 ? 23 : 39, };
			while (state.Buffer.Length > 4)
			{
				state.Index = Linux.GetPermissionIndex(state.Buffer[..state.ReadBytes], out Boolean isReadOnly);
				this.CreateMaps(state, isReadOnly);

				state.Offset = state.Index > 0 ? state.Index + Linux.PermissionTokenLength : 0;
				state.Buffer = state.Buffer[state.Offset..];
				state.Index = state.Buffer.IndexOf((Byte)MapsTokens.NewLine); // End of the line.
				state.Offset = state.Index + 1;
				state.Buffer = state.Buffer[state.Offset..];
				if (state.Buffer.Length < state.ReadBytes)
					state.ReadBytes = state.Buffer.Length; // End of the buffer.
			}
		}
		/// <summary>
		/// Creates addresses maps.
		/// </summary>
		/// <param name="state">A <see cref="FileState"/> instance.</param>
		/// <param name="isReadOnly">Indicates whether addresses are for read-only section.</param>
		private void CreateMaps(FileState state, Boolean isReadOnly)
		{
			if (state.Index <= 3) return;

			state.Offset = state.Buffer[..state.Index].IndexOf((Byte)MapsTokens.NewLine) + 1;
			ReadOnlySpan<Byte> temp = state.Buffer[state.Offset..state.Index];
			state.Auxiliar = temp.IndexOf((Byte)MapsTokens.Hyphen);

			if (state.Auxiliar <= 0) return;

			ReadOnlySpan<Byte> beginSpan = temp[..state.Auxiliar];
			ReadOnlySpan<Byte> endSpan = temp[(beginSpan.Length + 1)..];

			this.AddBoundary(new(beginSpan, false), isReadOnly);
			this.AddBoundary(new(endSpan, true), isReadOnly);
		}
		/// <summary>
		/// Adds <paramref name="value"/> to maps boundaries.
		/// </summary>
		/// <param name="value">A <see cref="MemoryBoundary"/> instance.</param>
		/// <param name="isReadOnly">Indicates whether <paramref name="value"/> is read-only boundary.</param>
		private void AddBoundary(MemoryBoundary value, Boolean isReadOnly)
		{
			SortedSet<MemoryBoundary> maps = isReadOnly ? this._readonlyMemory : this._readwriteMemory;
			Linux.AddBoundary(maps, value);
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
		/// <summary>
		/// Retrieves permission index.
		/// </summary>
		/// <param name="buffer">A read-only buffer.</param>
		/// <param name="isReadOnly"> Output. Indicates whether <paramref name="buffer"/> is read-only permission.</param>
		/// <returns>Index of permission token in <paramref name="buffer"/>.</returns>
		private static Int32 GetPermissionIndex(ReadOnlySpan<Byte> buffer, out Boolean isReadOnly)
		{
			Int32 index = 0;
			ReadOnlySpan<MapsTokens> bAsToken = MemoryMarshal.Cast<Byte, MapsTokens>(buffer);
			while (bAsToken.Length >= Linux.PermissionTokenLength)
			{
				if (Linux.IsSectionPermission(bAsToken, out isReadOnly))
					return index;
				bAsToken = bAsToken[1..];
				index++;
			}
			isReadOnly = false;
			return -1;
		}
		/// <summary>
		/// Indicates <paramref name="buffer"/> is a section permission ASCII text.
		/// </summary>
		/// <param name="buffer">A <see cref="MapsTokens"/> read-only span.</param>
		/// <param name="isReadOnly"> Output. Indicates whether <paramref name="buffer"/> is read-only permission.</param>
		/// <returns>
		/// <see langword="true"/> if <paramref name="buffer"/> is section permission ASCII text; otherwise,
		/// <see langword="false"/>.
		/// </returns>
		private static Boolean IsSectionPermission(ReadOnlySpan<MapsTokens> buffer, out Boolean isReadOnly)
		{
			isReadOnly = false;
			// "??????"
			if (buffer[0] is not MapsTokens.Space || buffer[1] is not MapsTokens.Read)
				return false;
			// " r????"
			if (buffer[4] is not MapsTokens.Private and not MapsTokens.Shared || buffer[5] is not MapsTokens.Space)
				return false;
			// " r??[ps] "
			if (buffer[2] is not MapsTokens.Write and not MapsTokens.Hyphen ||
			    buffer[3] is not MapsTokens.Hyphen and not MapsTokens.Execute)
				return false;
			// " r[-w][-x][ps] "
			isReadOnly = buffer[2] is MapsTokens.Hyphen;
			return true;
		}
	}
}