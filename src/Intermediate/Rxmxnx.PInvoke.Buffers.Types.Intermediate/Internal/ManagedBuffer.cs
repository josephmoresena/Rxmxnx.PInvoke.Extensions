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
	/// <param name="manager">A <see cref="IMetadataStore"/> instance.</param>
	public static void AppendComponent(BufferTypeMetadata<T> component, IMetadataStore manager)
	{
		if (!manager.TryAdd(component)) return;
		foreach (BufferTypeMetadata<T> metadataComponent in component.Components.Span)
			ManagedBuffer<T>.AppendComponent(metadataComponent, manager);
	}
}