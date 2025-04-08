namespace Rxmxnx.PInvoke.Internal;

internal partial class MemoryInspector
{
	private sealed partial class Linux
	{
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