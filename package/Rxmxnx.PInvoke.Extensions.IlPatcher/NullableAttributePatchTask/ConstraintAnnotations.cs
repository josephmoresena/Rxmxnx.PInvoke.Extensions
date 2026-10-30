// ReSharper disable ForeachCanBePartlyConvertedToQueryUsingAnotherGetEnumerator

namespace Rxmxnx.PInvoke.Extensions.IlPatcher;

public partial class NullableAttributePatchTask
{
	/// <summary>
	/// Gets the nullable context that applies to <paramref name="type"/>.
	/// </summary>
	/// <param name="type">Type to inspect.</param>
	/// <returns>The nearest context flag, or 0 when none is declared.</returns>
	private static Byte GetNullableContext(TypeDefinition type)
	{
		TypeDefinition? typeDefinition = type;
		while (typeDefinition is not null)
		{
			if (NullableAttributePatchTask.TryGetNullableContext(typeDefinition, out Byte flag)) return flag;
			typeDefinition = typeDefinition.DeclaringType;
		}
		return 0;
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
			if (attribute.AttributeType.Name != NullableAttributePatchTask.NullableContextAttributeName) continue;
			if (attribute.ConstructorArguments.Count == 0) continue;
			if (attribute.ConstructorArguments[0].Value is not Byte value) continue;
			flag = value;
			return true;
		}
		return false;
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
			if (attribute.AttributeType.Name != NullableAttributePatchTask.NullableAttributeName) continue;
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
		switch (attribute.ConstructorArguments[0].Value)
		{
			case Byte flag:
				return flag == NullableAttributePatchTask.AnnotatedFlag;
			case Byte[] flags:
				return Array.Exists(flags, static item => item == NullableAttributePatchTask.AnnotatedFlag);
			case CustomAttributeArgument[] arguments:
				foreach (CustomAttributeArgument argument in arguments)
					if (argument.Value is Byte value && value == NullableAttributePatchTask.AnnotatedFlag)
						return true;
				return false;
			default:
				return true;
		}
	}
}