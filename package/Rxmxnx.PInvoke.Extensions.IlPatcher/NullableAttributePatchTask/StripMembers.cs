// ReSharper disable ForeachCanBePartlyConvertedToQueryUsingAnotherGetEnumerator
// ReSharper disable ForeachCanBeConvertedToQueryUsingAnotherGetEnumerator

namespace Rxmxnx.PInvoke.Extensions.IlPatcher;

public partial class NullableAttributePatchTask
{
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
		Boolean modified = typeContext != NullableAttributePatchTask.AnnotatedFlag &&
			NullableAttributePatchTask.StripConstraints(type);
		foreach (MethodDefinition method in type.Methods)
		{
			Byte context = NullableAttributePatchTask.TryGetNullableContext(method, out Byte methodContext) ?
				methodContext :
				typeContext;
			if (context == NullableAttributePatchTask.AnnotatedFlag) continue;
			modified |= NullableAttributePatchTask.StripConstraints(method);
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
}