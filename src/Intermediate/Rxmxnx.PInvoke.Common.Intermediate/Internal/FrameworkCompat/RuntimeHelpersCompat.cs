#if !NETSTANDARD2_1 && !NETCOREAPP2_0_OR_GREATER
namespace Rxmxnx.PInvoke.Internal.FrameworkCompat;

/// <summary>
/// <see cref="RuntimeHelpers"/> compatibility utilities for internal use.
/// </summary>
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
		if (type.GetTypeInfo().IsPrimitive)
			return false;
		if (!type.GetTypeInfo().IsValueType)
			return true;
		if (Nullable.GetUnderlyingType(type) is { } underlyingType)
			type = underlyingType;
		// ReSharper disable once ConvertIfStatementToReturnStatement
		if (type.GetTypeInfo().IsEnum)
			return false;
		return type.GetTypeInfo().DeclaredFields
		           .Any(f => !f.IsStatic && RuntimeHelpersCompat.IsReferenceOrContainsReferences(f.FieldType));
	}

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
}
#endif