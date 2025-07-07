namespace Rxmxnx.PInvoke.Extensions.IlPatcher;

/// <summary>
/// Value pointer patch task.
/// </summary>
[ExcludeFromCodeCoverage]
public sealed partial class ValuePointerPatchTask : AssemblyPatchTask
{
	/// <inheritdoc/>
	protected override Boolean IlPatch(ModuleDefinition module)
	{
		TypeDefinition readOnlyValPtrTypeDefinition =
			module.Types.First(t => t.Name == ValuePointerPatchTask.ReadOnlyValPtrName);
		TypeDefinition valPtrTypeDefinition = module.Types.First(t => t.Name == ValuePointerPatchTask.ValPtrName);

		TypeDefinition readOnlyFixedContext =
			module.Types.First(t => t.Name == ValuePointerPatchTask.ReadOnlyFixedContextName);
		TypeDefinition fixedContext = module.Types.First(t => t.Name == ValuePointerPatchTask.FixedContextName);
		TypeDefinition iReadOnlyFixedContext =
			module.Types.First(t => t.Name == ValuePointerPatchTask.IReadOnlyFixedContextName);
		TypeDefinition iFixedContext = module.Types.First(t => t.Name == ValuePointerPatchTask.IFixedContextName);

		ValuePointerPatchTask.ImplementGetUnsafeFixedContextMethod(module, readOnlyValPtrTypeDefinition,
		                                                           iReadOnlyFixedContext, readOnlyFixedContext);
		ValuePointerPatchTask.ImplementGetUnsafeFixedContextMethod(module, valPtrTypeDefinition, iFixedContext,
		                                                           fixedContext);
		return true;
	}
	/// <inheritdoc/>
	protected override Boolean DocumentationPatch(XmlDocument xmlDocument)
	{
		XmlNodeList membersNode = xmlDocument.GetElementsByTagName("members");

		if (membersNode[0] is not XmlElement membersElement) return false;

		ValuePointerPatchTask.DocumentGetUnsafeFixedContextMethod(membersElement);
		return true;
	}
}