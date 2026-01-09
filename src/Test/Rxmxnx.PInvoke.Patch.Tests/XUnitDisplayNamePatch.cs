namespace Rxmxnx.PInvoke.Patch.Tests;

/// <summary>
/// DisplayName property patch task.
/// </summary>
[ExcludeFromCodeCoverage]
// ReSharper disable once UnusedType.Global
public class XUnitDisplayNamePatch : TestAssemblyPatchTask
{
	protected override Boolean IlPatch(ModuleDefinition mainModule)
	{
		Boolean modified = false;
		String suffix = $" [{this.TargetFramework}]";

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

				modified |= XUnitDisplayNamePatch.PatchDisplayName(attr, method, suffix, mainModule);
			}
		}

		return modified;
	}

	private static Boolean IsXunitAttribute(CustomAttribute attr)
	{
		String n = attr.AttributeType.FullName;
		return n is "Xunit.FactAttribute" or "Xunit.TheoryAttribute" or "Xunit.SkippableFact.SkippableFactAttribute" or
			"Xunit.SkippableFact.SkippableTheoryAttribute";
	}

	private static Boolean PatchDisplayName(CustomAttribute attr, MethodDefinition method, String suffix,
		ModuleDefinition module)
	{
		TypeReference? stringType = module.TypeSystem.String;
		CustomAttributeNamedArgument existing = attr.Properties.FirstOrDefault(p => p.Name == "DisplayName");

		String baseName;

		if (existing.Name != null)
		{
			baseName = (String)existing.Argument.Value!;
			if (baseName.EndsWith(suffix, StringComparison.Ordinal))
				return false;

			attr.Properties.Remove(existing);
		}
		else
		{
			baseName = method.Name;
		}

		attr.Properties.Add(new("DisplayName", new(stringType, baseName + suffix)));

		return true;
	}
}