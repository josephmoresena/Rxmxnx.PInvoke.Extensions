namespace Rxmxnx.PInvoke.Internal;

/// <summary>
/// Helper class for <see cref="IManagedBuffer{T}"/> implementations.
/// </summary>
/// <typeparam name="T">The type of items in the buffer.</typeparam>
internal static class ManagedBuffer<T>
{
	/// <summary>
	/// Appends all components from <paramref name="component"/> instance.
	/// </summary>
	/// <param name="component">A <see cref="BufferTypeMetadata{T}"/> instance.</param>
	/// <param name="storage">An <see cref="IMetadataStorage"/> instance.</param>
	public static void AppendComponent(BufferTypeMetadata<T> component, IMetadataStorage storage)
	{
		if (!storage.TryAdd(component)) return;
		Int32 count = component.ComponentCount;
		for (Int32 index = 0; index < count; index++)
			ManagedBuffer<T>.AppendComponent((BufferTypeMetadata<T>)component[index], storage);
	}
}