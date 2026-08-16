#if !NETSTANDARD2_1 && !NETCOREAPP2_0_OR_GREATER
using RuntimeHelpers = Rxmxnx.PInvoke.Internal.FrameworkCompat.RuntimeHelpersCompat;
#endif

namespace Rxmxnx.PInvoke.Internal;

#if !PACKAGE
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
#endif
internal sealed unsafe partial class FixedContext<T>
{
#pragma warning disable CS8500
	/// <summary>
	/// Transforms the current memory context into a different type, and provides a fixed offset that represents the
	/// remaining portion of memory not included in the newly formed context.
	/// </summary>
	/// <typeparam name="TDestination">The type into which the current memory context should be transformed.</typeparam>
	/// <param name="fixedOffset">
	/// Output. Provides a fixed offset that represents the remaining portion of memory that is not included in the new
	/// context.
	/// This is calculated based on the size of the new type compared to the size of the original memory block.
	/// </param>
	/// <param name="isReadOnly">Indicates whether the transformation operation should be performed as a read-only operation.</param>
	/// <returns>
	/// A new instance of FixedContext for the destination type, which represents a fixed memory context of the new type.
	/// </returns>
	/// <remarks>
	/// If the size of the new type exceeds the total length of the current context, the resulting context will be empty, and
	/// <paramref name="fixedOffset"/> will contain all the memory from the original context.
	/// Conversely, if a type of lesser size is chosen, the resulting context will have a greater length, and
	/// <paramref name="fixedOffset"/> will represent the remaining memory not included in the new context.
	/// </remarks>
	public FixedContext<TDestination> GetTransformation<TDestination>(out FixedOffset fixedOffset,
		Boolean isReadOnly = false)
	{
		this.ValidateOperation(isReadOnly);
		this.ValidateTransformation(typeof(TDestination),
		                            !RuntimeHelpers.IsReferenceOrContainsReferences<TDestination>());
		Int32 sizeOf = sizeof(TDestination);
		Int32 count = this.GetCount(sizeOf);
		Int32 offset = count * sizeOf;
		fixedOffset = new(this, offset);
		return new(this, count);
	}
#pragma warning restore CS8500
}