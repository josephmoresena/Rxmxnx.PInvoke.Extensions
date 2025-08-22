namespace Rxmxnx.PInvoke;

public partial class NativeUtilities
{
	/// <summary>
	/// Retrieves the decimal value of <paramref name="hexCharacter"/>.
	/// </summary>
	/// <param name="hexCharacter">ASCII hexadecimal character.</param>
	/// <returns>Decimal value from <paramref name="hexCharacter"/>.</returns>
	internal static Byte GetDecimalValue(Byte hexCharacter)
		=> hexCharacter switch
		{
			(Byte)'a' => 10,
			(Byte)'A' => 10,
			(Byte)'b' => 11,
			(Byte)'B' => 11,
			(Byte)'c' => 12,
			(Byte)'C' => 12,
			(Byte)'d' => 13,
			(Byte)'D' => 13,
			(Byte)'e' => 14,
			(Byte)'E' => 14,
			(Byte)'f' => 15,
			(Byte)'F' => 15,
			(Byte)'1' => 1,
			(Byte)'2' => 2,
			(Byte)'3' => 3,
			(Byte)'4' => 4,
			(Byte)'5' => 5,
			(Byte)'6' => 6,
			(Byte)'7' => 7,
			(Byte)'8' => 8,
			(Byte)'9' => 9,
			_ => 0,
		};
}