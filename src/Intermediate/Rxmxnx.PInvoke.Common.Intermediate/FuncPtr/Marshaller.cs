namespace Rxmxnx.PInvoke;

public readonly partial struct FuncPtr<TDelegate>
{
#if NET7_0_OR_GREATER || !PACKAGE
	/// <summary>
	/// Custom marshaller for value pointers.
	/// </summary>
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
#if NET7_0_OR_GREATER
	[CustomMarshaller(typeof(FuncPtr<>), MarshalMode.Default, typeof(FuncPtr<>.Marshaller))]
#endif
	public static class Marshaller
	{
		/// <summary>
		/// Converts a <see cref="FuncPtr{TDelegate}"/> pointer to a raw pointer.
		/// </summary>
		/// <param name="managed">A <see cref="FuncPtr{TDelegate}"/> pointer to convert.</param>
		/// <returns>Raw pointer.</returns>
		public static IntPtr ConvertToUnmanaged(FuncPtr<TDelegate> managed) => managed.Pointer;
		/// <summary>
		/// Converts a raw pointer to a <see cref="FuncPtr{T}"/> pointer.
		/// </summary>
		/// <param name="unmanaged">A raw pointer to convert.</param>
		/// <returns>A <see cref="FuncPtr{TDelegate}"/> pointer.</returns>
		public static FuncPtr<TDelegate> ConvertToManaged(IntPtr unmanaged) => (FuncPtr<TDelegate>)unmanaged;
	}
#endif
}