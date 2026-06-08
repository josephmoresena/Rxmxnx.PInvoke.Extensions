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
	/// <param name="storage">A <see cref="IMetadataStorage"/> instance.</param>
	public static void AppendComponent(BufferTypeMetadata<T> component, IMetadataStorage storage)
	{
		if (!storage.TryAdd(component)) return;
		foreach (BufferTypeMetadata<T> metadataComponent in component.Components.Span)
			ManagedBuffer<T>.AppendComponent(metadataComponent, storage);
	}
}