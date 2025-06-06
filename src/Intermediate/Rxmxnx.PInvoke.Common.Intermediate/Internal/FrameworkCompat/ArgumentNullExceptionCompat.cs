#if !NET6_0_OR_GREATER
namespace Rxmxnx.PInvoke.Internal.FrameworkCompat;

/// <summary>
/// <see cref="ArgumentNullException"/> compatibility utilities for internal use.
/// </summary>
internal static class ArgumentNullExceptionCompat
{
	/// <summary>
	/// Throws an <see cref="ArgumentNullException"/> if <paramref name="argument"/> is null.
	/// </summary>
	/// <param name="argument">The reference type argument to validate as non-null.</param>
	/// <param name="paramName">The name of the parameter with which <paramref name="argument"/> corresponds.</param>
	public static void ThrowIfNull<T>([NotNull] T? argument,
		[CallerArgumentExpression(nameof(argument))] String? paramName = null)
	{
		if (argument is null) throw new ArgumentNullException(paramName);
	}
}
#endif