namespace Rxmxnx.PInvoke.Internal.FrameworkCompat;

/// <summary>
/// Prevents the Linker/Trimmer from removing code that it might otherwise think is unused.
/// </summary>
[AttributeUsage(AttributeTargets.All | AttributeTargets.Delegate, AllowMultiple = true)]
internal sealed class PreserveAttribute : Attribute
{
	/// <summary>
	/// If true, ensures all members of the decorated class are preserved
	/// if the class itself is preserved.
	/// </summary>
	public Boolean AllMembers;

	/// <summary>
	/// If true, the member is only preserved if the containing type
	/// is required and preserved by the Linker.
	/// </summary>
	public Boolean Conditional;

	/// <summary>
	/// Initializes a new instance of the <see cref="PreserveAttribute"/>.
	/// </summary>
	public PreserveAttribute() { }

	/// <summary>
	/// Initializes a new instance of the <see cref="PreserveAttribute"/>, ensuring that
	/// the specified <paramref name="type"/> is also preserved.
	/// </summary>
	/// <param name="type">The specific Type to be preserved by the Linker.</param>
	public PreserveAttribute(Type type) { }
}