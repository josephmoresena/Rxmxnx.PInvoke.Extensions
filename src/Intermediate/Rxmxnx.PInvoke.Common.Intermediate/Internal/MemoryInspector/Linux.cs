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
		/// Token permission length.
		/// </summary>
		private const Int32 PermissionTokenLength = 6;

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
			Unsafe.SkipInit(out isReadOnly);
			return false;
		}

		/// <summary>
		/// Reads <c>/proc/self/maps</c> file and refresh memories boundaries.
		/// </summary>
		private void RefreshMaps()
		{
			Span<Byte> buffer = stackalloc Byte[IntPtr.Size == 4 ? 23 : 39];
			FileState state = new(buffer);

			using NativeFile mapsFile = NativeFile.OpenSelfMaps();
			while ((state.ReadBytes = mapsFile.Read(buffer)) > 4)
			{
				state.Index = Linux.GetPermissionIndex(state.Buffer[..state.ReadBytes], out Boolean isReadOnly);
				this.CreateMaps(state, isReadOnly);

				state.Offset = state.Index > 0 ? state.Index : 0;
				state.Index = state.Buffer[state.Offset..state.ReadBytes].IndexOf((Byte)MapsTokens.NewLine);
				if (state.Index > 0)
					mapsFile.Seek(-state.ReadBytes + state.Index + state.Offset + 1);
				else
					Linux.PrepareNextPosition(state, in mapsFile);
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

			ReadOnlySpan<Byte> temp = state.Buffer[..state.Index];
			state.Auxiliar = temp.IndexOf((Byte)MapsTokens.Hyphen);

			if (state.Auxiliar <= 0) return;

			ReadOnlySpan<Byte> beginSpan = temp[..state.Auxiliar];
			ReadOnlySpan<Byte> endSpan = temp[(beginSpan.Length + 1)..];

			try
			{
				this.AddBoundary(new(beginSpan, false), isReadOnly);
				this.AddBoundary(new(endSpan, true), isReadOnly);
			}
			catch (Exception ex)
			{
#if !PACKAGE
				throw new(
					$"S: {IntPtr.Size} B: {Encoding.UTF8.GetString(beginSpan)} E: {Encoding.UTF8.GetString(endSpan)}",
					ex);
#else
				throw;
#endif
			}
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
		/// Prepares the maps file for next addresses reading position.
		/// </summary>
		/// <param name="state">A <see cref="FileState"/> instance.</param>
		/// <param name="mapsFile">A <see cref="NativeFile"/> instance.</param>
		private static void PrepareNextPosition(FileState state, in NativeFile mapsFile)
		{
			do
			{
				if ((state.ReadBytes = mapsFile.Read(state.Buffer)) <= 4) break;
				state.Index = state.Buffer[..state.ReadBytes].IndexOf((Byte)MapsTokens.NewLine);
			} while (state.Index < 0);

			if (state.ReadBytes > 4 && state.Index > -1) mapsFile.Seek(-state.ReadBytes + state.Index + 1);
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
			SortedSet<MemoryBoundary> view = maps.GetViewBetween(default, address);
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

		/// <summary>
		/// Maps file Tokens
		/// </summary>
		private enum MapsTokens : Byte
		{
			Hyphen = (Byte)'-',
			Space = (Byte)' ',
			NewLine = (Byte)'\n',
			Read = (Byte)'r',
			Write = (Byte)'w',
			Execute = (Byte)'x',
			Private = (Byte)'p',
			Shared = (Byte)'s',
		}
	}
}