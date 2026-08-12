namespace Rxmxnx.PInvoke.Internal.FrameworkCompat;

/// <summary>
/// <see cref="RuntimeHelpers"/> compatibility utilities for internal use.
/// </summary>
#if !PACKAGE
[ExcludeFromCodeCoverage]
#endif
internal static class RuntimeHelpersCompat
{
	/// <summary>
	/// Returns a value that indicates whether the specified type is a reference type or a value type that contains references.
	/// </summary>
	/// <typeparam name="T">The type.</typeparam>
	/// <returns>
	/// <see langword="true"/> if the given type is reference type or value type that contains references; otherwise,
	/// <see langword="false"/>.
	/// </returns>
#if NETSTANDARD2_1 || NETCOREAPP2_0_OR_GREATER
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Boolean IsReferenceOrContainsReferences<T>() => RuntimeHelpers.IsReferenceOrContainsReferences<T>();
#else
	public static Boolean IsReferenceOrContainsReferences<T>() => !Info<T>.IsUnmanaged;

	/// <summary>
	/// Returns a value that indicates whether the specified type is a reference type or a value type that contains references.
	/// </summary>
	/// <param name="type">The CLR type.</param>
	/// <returns>
	/// <see langword="true"/> if the given type is reference type or value type that contains references; otherwise,
	/// <see langword="false"/>.
	/// </returns>
	private static Boolean IsReferenceOrContainsReferences(Type type)
	{
		if (type.IsPrimitive || RuntimeHelpersCompat.IsPointerType(type))
			return false;
		if (!type.IsValueType)
			return true;
		if (Nullable.GetUnderlyingType(type) is { } underlyingType)
			type = underlyingType;
		// ReSharper disable once ConvertIfStatementToReturnStatement
		if (type.IsEnum)
			return false;
		return type.GetTypeInfo().DeclaredFields
		           .Any(f => !f.IsStatic && RuntimeHelpersCompat.IsReferenceOrContainsReferences(f.FieldType));
	}
	/// <summary>
	/// Indicates whether <paramref name="type"/> is a pointer type.
	/// </summary>
	/// <param name="type">A CLR type.</param>
	/// <returns>
	/// <see langword="true"/> if <paramref name="type"/> is a pointer type; otherwise, <see langword="false"/>.
	/// </returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static Boolean IsPointerType(Type type)
		=> type.IsPointer || type == typeof(IntPtr) || type == typeof(UIntPtr);

	/// <summary>
	/// Generic type info class.
	/// </summary>
	/// <typeparam name="T">The type.</typeparam>
	private static class Info<T>
	{
		/// <summary>
		/// Indicates whether the current type is unmanaged.
		/// </summary>
		public static readonly Boolean IsUnmanaged = !RuntimeHelpersCompat.IsReferenceOrContainsReferences(typeof(T));
	}
#endif
}