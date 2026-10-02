// ReSharper disable ForeachCanBeConvertedToQueryUsingAnotherGetEnumerator
// ReSharper disable ForeachCanBePartlyConvertedToQueryUsingAnotherGetEnumerator

// ReSharper disable ConvertIfStatementToReturnStatement

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
public sealed class NullableAttributePatchTask : AssemblyPatchTask
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
		const String attributeName = "NullablePublicOnlyAttribute";
		if (mainModule.Assembly.CustomAttributes.Any(attribute => attribute.AttributeType.Name == attributeName))
			return true;
		return mainModule.CustomAttributes.Any(attribute => attribute.AttributeType.Name == attributeName);
	}
	/// <summary>
	/// Removes nullable annotations from <paramref name="type"/> that are outside the exported contract.
	/// </summary>
	/// <param name="type">Type to patch.</param>
	/// <returns><see langword="true"/> if an annotation was removed.</returns>
	private static Boolean StripType(TypeDefinition type)
	{
		if (!NullableAttributePatchTask.IsExported(type))
			return NullableAttributePatchTask.StripAll(type);

		Boolean modified = false;
		foreach (FieldDefinition field in type.Fields)
		{
			if (NullableAttributePatchTask.IsExported(field)) continue;
			modified |= NullableAttributePatchTask.StripProvider(field);
		}
		foreach (PropertyDefinition property in type.Properties)
		{
			if (NullableAttributePatchTask.IsContract(property)) continue;
			modified |= NullableAttributePatchTask.StripProvider(property);
			if (!property.HasParameters) continue;
			foreach (ParameterDefinition parameter in property.Parameters)
				modified |= NullableAttributePatchTask.StripProvider(parameter);
		}
		foreach (EventDefinition eventDef in type.Events)
		{
			if (NullableAttributePatchTask.IsContract(eventDef)) continue;
			modified |= NullableAttributePatchTask.StripProvider(eventDef);
		}
		foreach (MethodDefinition method in type.Methods)
		{
			if (NullableAttributePatchTask.IsContract(method)) continue;
			modified |= NullableAttributePatchTask.StripMethod(method);
		}
		return modified;
	}
	/// <summary>
	/// Removes every nullable annotation from a non-exported <paramref name="type"/> and its members.
	/// </summary>
	/// <param name="type">Non-exported type.</param>
	/// <returns><see langword="true"/> if an annotation was removed.</returns>
	private static Boolean StripAll(TypeDefinition type)
	{
		Boolean modified = NullableAttributePatchTask.StripProvider(type);
		foreach (FieldDefinition field in type.Fields)
			modified |= NullableAttributePatchTask.StripProvider(field);
		if (type.HasInterfaces)
			foreach (InterfaceImplementation implementation in type.Interfaces)
				modified |= NullableAttributePatchTask.StripProvider(implementation);
		foreach (PropertyDefinition property in type.Properties)
		{
			if (NullableAttributePatchTask.IsExplicitContract(property)) continue;
			modified |= NullableAttributePatchTask.StripProvider(property);
			if (!property.HasParameters) continue;
			foreach (ParameterDefinition parameter in property.Parameters)
				modified |= NullableAttributePatchTask.StripProvider(parameter);
		}
		foreach (EventDefinition eventDef in type.Events)
		{
			if (NullableAttributePatchTask.IsExplicitContract(eventDef)) continue;
			modified |= NullableAttributePatchTask.StripProvider(eventDef);
		}
		foreach (MethodDefinition method in type.Methods)
		{
			if (method.HasOverrides) continue;
			modified |= NullableAttributePatchTask.StripMethod(method);
		}
		return modified;
	}
	/// <summary>
	/// Removes nullable annotations from <paramref name="method"/>, its signature, and its generic parameters.
	/// </summary>
	/// <param name="method">Method outside the exported contract.</param>
	/// <returns><see langword="true"/> if an annotation was removed.</returns>
	private static Boolean StripMethod(MethodDefinition method)
	{
		Boolean modified = NullableAttributePatchTask.StripProvider(method);
		modified |= NullableAttributePatchTask.StripProvider(method.MethodReturnType);
		if (!method.HasParameters) return modified;
		foreach (ParameterDefinition parameter in method.Parameters)
			modified |= NullableAttributePatchTask.StripProvider(parameter);
		return modified;
	}
	/// <summary>
	/// Removes nullable annotations from generic constraints that do not record an annotated reference.
	/// </summary>
	/// <param name="type">Type whose constraints are reviewed.</param>
	/// <returns><see langword="true"/> if an annotation was removed.</returns>
	/// <remarks>
	/// Flag 2 is the <c>?</c> annotation. Constraint attributes in this assembly use 0 for a struct argument and 1 for
	/// the reference constraint itself. Both <c>TAction?</c> and <c>TFunction?</c> are parameter annotations and are
	/// left in place. When the nearest <c>NullableContextAttribute</c> is 2, a missing constraint attribute is read as
	/// nullable, so that attribute stays.
	/// </remarks>
	private static Boolean StripConstraintAnnotations(TypeDefinition type)
	{
		Byte typeContext = NullableAttributePatchTask.GetNullableContext(type);
		Boolean modified = typeContext != 2 && NullableAttributePatchTask.StripConstraints(type);
		foreach (MethodDefinition method in type.Methods)
		{
			Byte context = NullableAttributePatchTask.TryGetNullableContext(method, out Byte methodContext) ?
				methodContext :
				typeContext;
			if (context == 2) continue;
			modified |= NullableAttributePatchTask.StripConstraints(method);
		}
		return modified;
	}
	/// <summary>
	/// Gets the nullable context that applies to <paramref name="type"/>.
	/// </summary>
	/// <param name="type">Type to inspect.</param>
	/// <returns>The nearest context flag, or 0 when none is declared.</returns>
	private static Byte GetNullableContext(TypeDefinition type)
	{
		if (NullableAttributePatchTask.TryGetNullableContext(type, out Byte flag)) return flag;
		return type.DeclaringType is { } declaring ? NullableAttributePatchTask.GetNullableContext(declaring) : (Byte)0;
	}
	/// <summary>
	/// Reads <c>NullableContextAttribute</c> from <paramref name="provider"/>.
	/// </summary>
	/// <param name="provider">Type or method that may declare the context.</param>
	/// <param name="flag">Context flag when the attribute is present.</param>
	/// <returns><see langword="true"/> when <paramref name="provider"/> declares the attribute.</returns>
	private static Boolean TryGetNullableContext(ICustomAttributeProvider provider, out Byte flag)
	{
		flag = 0;
		if (!provider.HasCustomAttributes) return false;
		foreach (CustomAttribute attribute in provider.CustomAttributes)
		{
			if (attribute.AttributeType.Name != "NullableContextAttribute") continue;
			if (attribute.ConstructorArguments.Count == 0) continue;
			if (attribute.ConstructorArguments[0].Value is not Byte value) continue;
			flag = value;
			return true;
		}
		return false;
	}
	/// <summary>
	/// Removes unannotated nullable attributes from the constraints of <paramref name="provider"/>.
	/// </summary>
	/// <param name="provider">Type or method that declares generic parameters.</param>
	/// <returns><see langword="true"/> if an annotation was removed.</returns>
	private static Boolean StripConstraints(IGenericParameterProvider provider)
	{
		if (!provider.HasGenericParameters) return false;

		Boolean modified = false;
		foreach (GenericParameter parameter in provider.GenericParameters)
		{
			if (!parameter.HasConstraints) continue;
			foreach (GenericParameterConstraint constraint in parameter.Constraints)
				modified |= NullableAttributePatchTask.RemoveUnannotatedConstraint(constraint);
		}
		return modified;
	}
	/// <summary>
	/// Removes a <c>NullableAttribute</c> from <paramref name="constraint"/> when none of its flags is annotated.
	/// </summary>
	/// <param name="constraint">Generic constraint to patch.</param>
	/// <returns><see langword="true"/> if an annotation was removed.</returns>
	private static Boolean RemoveUnannotatedConstraint(GenericParameterConstraint constraint)
	{
		if (!constraint.HasCustomAttributes) return false;

		Boolean modified = false;
		for (Int32 index = constraint.CustomAttributes.Count - 1; index >= 0; index--)
		{
			CustomAttribute attribute = constraint.CustomAttributes[index];
			if (attribute.AttributeType.Name != "NullableAttribute") continue;
			if (NullableAttributePatchTask.HasAnnotatedFlag(attribute)) continue;
			constraint.CustomAttributes.RemoveAt(index);
			modified = true;
		}
		return modified;
	}
	/// <summary>
	/// Determines whether <paramref name="attribute"/> contains the annotated flag (2).
	/// </summary>
	/// <param name="attribute"><c>NullableAttribute</c> to read.</param>
	/// <returns>
	/// <see langword="true"/> when a flag is 2, or when the argument cannot be read and the attribute must stay.
	/// </returns>
	private static Boolean HasAnnotatedFlag(CustomAttribute attribute)
	{
		if (attribute.ConstructorArguments.Count == 0) return true;

		Object value = attribute.ConstructorArguments[0].Value;
		if (value is Byte flag) return flag == 2;
		if (value is Byte[] flags) return Array.Exists(flags, static item => item == 2);
		if (value is not CustomAttributeArgument[] arguments) return true;

		foreach (CustomAttributeArgument argument in arguments)
			if (argument.Value is Byte and 2)
				return true;
		return false;
	}
	/// <summary>
	/// Removes nullable annotations from <paramref name="provider"/> and its generic parameters.
	/// </summary>
	/// <param name="provider">Member that can carry custom attributes.</param>
	/// <returns><see langword="true"/> if an annotation was removed.</returns>
	private static Boolean StripProvider(ICustomAttributeProvider provider)
	{
		Boolean modified = NullableAttributePatchTask.RemoveNullableAttributes(provider);
		if (provider is not IGenericParameterProvider { HasGenericParameters: true, } generic) return modified;

		foreach (GenericParameter parameter in generic.GenericParameters)
		{
			modified |= NullableAttributePatchTask.RemoveNullableAttributes(parameter);
			if (!parameter.HasConstraints) continue;
			foreach (GenericParameterConstraint constraint in parameter.Constraints)
				modified |= NullableAttributePatchTask.RemoveNullableAttributes(constraint);
		}
		return modified;
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
			if (attributeName is not ("NullableAttribute" or "NullableContextAttribute")) continue;
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