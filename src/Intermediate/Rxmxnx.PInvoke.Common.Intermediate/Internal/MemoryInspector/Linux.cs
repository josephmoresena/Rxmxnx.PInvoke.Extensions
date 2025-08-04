namespace Rxmxnx.PInvoke.Internal;

internal partial class MemoryInspector
{
	/// <summary>
	/// Linux OS implementation of <see cref="MemoryInspector"/> class.
	/// </summary>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	private sealed partial class Linux : MapsInspector
	{
		/// <summary>
		/// <c>/proc/self/maps</c> file name.
		/// </summary>
		private const String mapsFileName = "/proc/self/maps";
		/// <summary>
		/// Token permission length.
		/// </summary>
		private const Int32 permissionTokenLength = 6;

		/// <inheritdoc/>
		protected override void ProcessMaps() => this.ParseMaps(File.ReadAllBytes(Linux.mapsFileName));

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

				state.Offset = state.Index > 0 ? state.Index + Linux.permissionTokenLength : 0;
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
		/// Retrieves permission index.
		/// </summary>
		/// <param name="buffer">A read-only buffer.</param>
		/// <param name="isReadOnly"> Output. Indicates whether <paramref name="buffer"/> is read-only permission.</param>
		/// <returns>Index of permission token in <paramref name="buffer"/>.</returns>
		private static Int32 GetPermissionIndex(ReadOnlySpan<Byte> buffer, out Boolean isReadOnly)
		{
			Int32 index = 0;
			ReadOnlySpan<MapsTokens> bAsToken = MemoryMarshal.Cast<Byte, MapsTokens>(buffer);
			while (bAsToken.Length >= Linux.permissionTokenLength)
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