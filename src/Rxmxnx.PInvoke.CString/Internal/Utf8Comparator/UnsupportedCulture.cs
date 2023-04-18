namespace Rxmxnx.PInvoke.Internal;

internal partial class Utf8Comparator<TChar>
{
    /// <summary>
    /// Unsupported cultures set.
    /// </summary>
    internal static readonly IReadOnlySet<String> UnsupportedCultures = new HashSet<String>
    {
        "br", "cs", "cu", "cy", "dsb", "hsb", "ig",
        "is", "ku", "om", "pl", "se", "sk", "smn",
        "sq", "th", "tk", "uz", "wo"
    };
}
