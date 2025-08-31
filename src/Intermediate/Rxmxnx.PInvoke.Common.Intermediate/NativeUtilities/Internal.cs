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
	/// <summary>
	/// Returns a reference to the element of the array at index 0.
	/// </summary>
	/// <typeparam name="T">The type of items in the array.</typeparam>
	/// <param name="array">A <see typeparamref="T"/> array.</param>
	/// <returns>A reference to the element at index 0.</returns>
	internal static ref T GetArrayDataReference<T>(T[] array)
		=> ref array.Length > 0 ?
			ref Unsafe.AsRef(in array[0]) :
			ref MemoryMarshal.GetReference(new ReadOnlyMemory<T>(array).Span);
}