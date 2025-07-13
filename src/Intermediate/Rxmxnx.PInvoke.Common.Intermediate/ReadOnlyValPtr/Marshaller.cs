namespace Rxmxnx.PInvoke;

public readonly partial struct ReadOnlyValPtr<T>
{
#if !PACKAGE || NET7_0_OR_GREATER
	/// <summary>
	/// Custom marshaller for value pointers.
	/// </summary>
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
#if NET7_0_OR_GREATER
	[CustomMarshaller(typeof(ReadOnlyValPtr<>), MarshalMode.Default, typeof(ReadOnlyValPtr<>.Marshaller))]
#endif
	public static class Marshaller
	{
		/// <summary>
		/// Converts a <see cref="ReadOnlyValPtr{T}"/> pointer to a raw pointer.
		/// </summary>
		/// <param name="managed">A <see cref="ReadOnlyValPtr{T}"/> pointer to convert.</param>
		/// <returns>Raw pointer.</returns>
		public static IntPtr ConvertToUnmanaged(ReadOnlyValPtr<T> managed) => managed.Pointer;
		/// <summary>
		/// Converts a raw pointer to a <see cref="ReadOnlyValPtr{T}"/> pointer.
		/// </summary>
		/// <param name="unmanaged">A raw pointer to convert.</param>
		/// <returns>A <see cref="ReadOnlyValPtr{T}"/> pointer.</returns>
		public static ReadOnlyValPtr<T> ConvertToManaged(IntPtr unmanaged) => (ValPtr<T>)unmanaged;
	}
#endif
}