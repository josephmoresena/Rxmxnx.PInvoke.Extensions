namespace Rxmxnx.PInvoke.Internal;

internal interface ILinkInspector
{
	/// <summary>
	/// Indicates whether the function pointer of <paramref name="methodHandle"/> references to a native linked image.
	/// </summary>
	/// <param name="methodHandle">A <see langword="RuntimeMethodHandle"/> value.</param>
	/// <returns>
	/// <see langword="true"/> if the function pointer references an R/RX memory section; otherwise,
	/// <see langword="false"/>.
	/// </returns>
	Boolean IsImageMethod(RuntimeMethodHandle methodHandle);
}