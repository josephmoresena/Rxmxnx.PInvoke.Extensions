namespace Rxmxnx.PInvoke.Extensions.IlPatcher;

/// <summary>
/// Removes nullable annotations that are not part of the exported contract.
/// </summary>
/// <remarks>
/// <c>nullablePublicOnly</c> already omits private and internal methods. Properties have no accessibility of their own,
/// so the compiler annotates them from the declaring type, and a nested type keeps a copy of its outer type parameter.
/// This patch removes those leftovers. Explicit interface implementations stay: they are the exported contract.
/// A generic constraint also receives <c>NullableAttribute</c> because the constraint type is a reference, even when
/// every type argument is a struct. Those flags are only 0 or 1, and they are removed when the surrounding nullable
/// context is not annotated. Annotated parameters such as <c>TAction?</c> and <c>TFunction?</c> stay. A method whose
/// context is annotated keeps the constraint attribute: without it, the constraint is read as nullable.
/// </remarks>
[ExcludeFromCodeCoverage]
// ReSharper disable once UnusedType.Global
public sealed partial class NullableAttributePatchTask : AssemblyPatchTask
{
	/// <inheritdoc/>
	protected override Boolean IlPatch(ModuleDefinition mainModule)
	{
		if (!NullableAttributePatchTask.HasNullablePublicOnly(mainModule)) return false;

		Boolean modified = false;
		foreach (TypeDefinition type in mainModule.GetTypes())
		{
			modified |= NullableAttributePatchTask.StripType(type);
			modified |= NullableAttributePatchTask.StripConstraintAnnotations(type);
		}
		return modified;
	}

	/// <summary>
	/// Determines whether <paramref name="mainModule"/> was compiled with <c>nullablePublicOnly</c>.
	/// </summary>
	/// <param name="mainModule">Main module of the loaded assembly.</param>
	/// <returns><see langword="true"/> when <c>NullablePublicOnlyAttribute</c> is present.</returns>
	private static Boolean HasNullablePublicOnly(ModuleDefinition mainModule)
	{
		return mainModule.Assembly.CustomAttributes.Any(attribute => attribute.AttributeType.Name ==
			                                                NullableAttributePatchTask
				                                                .NullablePublicOnlyAttributeName) ||
			mainModule.CustomAttributes.Any(attribute => attribute.AttributeType.Name ==
				                                NullableAttributePatchTask.NullablePublicOnlyAttributeName);
	}
	/// <summary>
	/// Removes <c>NullableAttribute</c> and <c>NullableContextAttribute</c> from <paramref name="provider"/>.
	/// </summary>
	/// <param name="provider">Member that can carry custom attributes.</param>
	/// <returns><see langword="true"/> if an annotation was removed.</returns>
	private static Boolean RemoveNullableAttributes(ICustomAttributeProvider provider)
	{
		if (!provider.HasCustomAttributes) return false;

		Boolean modified = false;
		for (Int32 index = provider.CustomAttributes.Count - 1; index >= 0; index--)
		{
			String attributeName = provider.CustomAttributes[index].AttributeType.Name;
			if (attributeName is not (NullableAttributePatchTask.NullableAttributeName or
			    NullableAttributePatchTask.NullableContextAttributeName)) continue;
			provider.CustomAttributes.RemoveAt(index);
			modified = true;
		}
		return modified;
	}
	/// <summary>
	/// Determines whether <paramref name="type"/> is visible outside the assembly.
	/// </summary>
	/// <param name="type">Type to test.</param>
	/// <returns><see langword="true"/> when <paramref name="type"/> is exported.</returns>
	private static Boolean IsExported(TypeDefinition type)
	{
		// ReSharper disable once InvertIf
		if (type.DeclaringType is { } declaring)
		{
			TypeAttributes nested = type.Attributes & TypeAttributes.VisibilityMask;
			if (nested is not (TypeAttributes.NestedPublic or TypeAttributes.NestedFamily or
			    TypeAttributes.NestedFamORAssem))
				return false;
			return NullableAttributePatchTask.IsExported(declaring);
		}
		return (type.Attributes & TypeAttributes.VisibilityMask) == TypeAttributes.Public;
	}
	/// <summary>
	/// Determines whether <paramref name="field"/> belongs to the exported contract.
	/// </summary>
	/// <param name="field">Field to test.</param>
	/// <returns><see langword="true"/> when <paramref name="field"/> is exported.</returns>
	private static Boolean IsExported(FieldDefinition field)
	{
		if (!NullableAttributePatchTask.IsExported(field.DeclaringType)) return false;
		FieldAttributes access = field.Attributes & FieldAttributes.FieldAccessMask;
		return access is FieldAttributes.Public or FieldAttributes.Family or FieldAttributes.FamORAssem;
	}
	/// <summary>
	/// Determines whether <paramref name="method"/> belongs to the exported contract.
	/// </summary>
	/// <param name="method">Method to test.</param>
	/// <returns>
	/// <see langword="true"/> when <paramref name="method"/> is exported or implements an interface member.
	/// </returns>
	private static Boolean IsContract(MethodDefinition method)
		=> method.HasOverrides || NullableAttributePatchTask.IsVisible(method);
	/// <summary>
	/// Determines whether <paramref name="method"/> is public, protected, or protected internal.
	/// </summary>
	/// <param name="method">Method to test.</param>
	/// <returns><see langword="true"/> when <paramref name="method"/> is visible outside the assembly.</returns>
	private static Boolean IsVisible(MethodDefinition method)
	{
		if (!NullableAttributePatchTask.IsExported(method.DeclaringType)) return false;
		MethodAttributes access = method.Attributes & MethodAttributes.MemberAccessMask;
		return access is MethodAttributes.Public or MethodAttributes.Family or MethodAttributes.FamORAssem;
	}
	/// <summary>
	/// Determines whether <paramref name="property"/> belongs to the exported contract.
	/// </summary>
	/// <param name="property">Property to test.</param>
	/// <returns><see langword="true"/> when an accessor of <paramref name="property"/> is exported.</returns>
	private static Boolean IsContract(PropertyDefinition property)
		=> property.GetMethod is { } getter && NullableAttributePatchTask.IsContract(getter) ||
			property.SetMethod is { } setter && NullableAttributePatchTask.IsContract(setter);
	/// <summary>
	/// Determines whether <paramref name="property"/> is an explicit interface implementation.
	/// </summary>
	/// <param name="property">Property declared by a non-exported type.</param>
	/// <returns><see langword="true"/> when an accessor overrides an interface member.</returns>
	private static Boolean IsExplicitContract(PropertyDefinition property)
		=> property.GetMethod is { HasOverrides: true, } || property.SetMethod is { HasOverrides: true, };
	/// <summary>
	/// Determines whether <paramref name="eventDef"/> belongs to the exported contract.
	/// </summary>
	/// <param name="eventDef">Event to test.</param>
	/// <returns><see langword="true"/> when an accessor of <paramref name="eventDef"/> is exported.</returns>
	private static Boolean IsContract(EventDefinition eventDef)
		=> eventDef.AddMethod is { } add && NullableAttributePatchTask.IsContract(add) ||
			eventDef.RemoveMethod is { } remove && NullableAttributePatchTask.IsContract(remove);
	/// <summary>
	/// Determines whether <paramref name="eventDef"/> is an explicit interface implementation.
	/// </summary>
	/// <param name="eventDef">Event declared by a non-exported type.</param>
	/// <returns><see langword="true"/> when an accessor overrides an interface member.</returns>
	private static Boolean IsExplicitContract(EventDefinition eventDef)
		=> eventDef.AddMethod is { HasOverrides: true, } || eventDef.RemoveMethod is { HasOverrides: true, };
}