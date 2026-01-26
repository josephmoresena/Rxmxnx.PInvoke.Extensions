namespace Rxmxnx.PInvoke.Patch.Tests;

/// <summary>
/// DisplayName property patch task.
/// </summary>
[ExcludeFromCodeCoverage]
// ReSharper disable once UnusedType.Global
public class XUnitDisplayNamePatch : TestAssemblyPatchTask
{
	/// <inheritdoc/>
	[SuppressMessage("csharpsquid", "S3973")]
	protected override Boolean IlPatch(ModuleDefinition mainModule)
	{
		Boolean modified = false;
		String tfm = $" [{this.TargetFramework}]";
		TypeReference stringType = mainModule.TypeSystem.String;

		foreach (TypeDefinition? type in mainModule.Types)
		foreach (MethodDefinition? method in type.Methods)
		{
			if (!method.HasCustomAttributes)
				continue;

			// ReSharper disable once ForeachCanBeConvertedToQueryUsingAnotherGetEnumerator
			foreach (CustomAttribute? attr in method.CustomAttributes)
			{
				if (!XUnitDisplayNamePatch.IsXunitAttribute(attr))
					continue;

				modified |= XUnitDisplayNamePatch.PatchDisplayName(attr, stringType, tfm,
				                                                   $"{type.FullName}.{method.Name}");
			}
		}

		return modified;
	}

	/// <summary>
	/// Indicates whether <paramref name="attr"/> is an xUnit method attribute.
	/// </summary>
	/// <param name="attr">A <see cref="CustomAttribute"/> instance.</param>
	/// <returns>
	/// <see langword="true"/> if <paramref name="attr"/> is an xUnit method attribute; otherwise, <see langword="false"/>.
	/// </returns>
	private static Boolean IsXunitAttribute(CustomAttribute attr)
	{
		String n = attr.AttributeType.FullName;
		return n is "Xunit.FactAttribute" or "Xunit.TheoryAttribute" or "Xunit.SkippableFactAttribute" or
			"Xunit.SkippableTheoryAttribute";
	}
	/// <summary>
	/// Patches <paramref name="attr"/> instance adding <paramref name="tfm"/> to the <c>DisplayName</c> property.
	/// </summary>
	/// <param name="attr">An xUnit method attribute.</param>
	/// <param name="stringType">System.String type from the loaded assembly.</param>
	/// <param name="tfm">Suffix display name.</param>
	/// <param name="methodName">Test method name.</param>
	/// <returns>
	/// <see langword="true"/> if <see cref="attr"/> was modified; otherwise; <see langword="false"/>.
	/// </returns>
	private static Boolean PatchDisplayName(CustomAttribute attr, TypeReference stringType, String tfm,
		String methodName)
	{
		const String propName = "DisplayName";
		CustomAttributeNamedArgument existing = attr.Properties.FirstOrDefault(p => p.Name == propName);

		String displayName = methodName;
		if (existing.Name != null)
		{
			displayName = (String)existing.Argument.Value!;
			if (displayName.EndsWith(tfm, StringComparison.Ordinal))
				return false;

			attr.Properties.Remove(existing);
		}
		attr.Properties.Add(new(propName, new(stringType, displayName + tfm)));

		return true;
	}
}