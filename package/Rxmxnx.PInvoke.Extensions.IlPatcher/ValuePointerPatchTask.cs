namespace Rxmxnx.PInvoke.Extensions.IlPatcher;

/// <summary>
/// Value pointer patch task.
/// </summary>
[ExcludeFromCodeCoverage]
public sealed partial class ValuePointerPatchTask : AssemblyPatchTask
{
	/// <inheritdoc/>
	protected override Boolean IlPatch(ModuleDefinition mainModule)
	{
		TypeDefinition readOnlyValPtrTypeDefinition =
			mainModule.Types.First(t => t.Name == ValuePointerPatchTask.ReadOnlyValPtrName);
		TypeDefinition valPtrTypeDefinition = mainModule.Types.First(t => t.Name == ValuePointerPatchTask.ValPtrName);

		TypeDefinition readOnlyFixedContext =
			mainModule.Types.First(t => t.Name == ValuePointerPatchTask.ReadOnlyFixedContextName);
		TypeDefinition fixedContext = mainModule.Types.First(t => t.Name == ValuePointerPatchTask.FixedContextName);
		TypeDefinition iReadOnlyFixedContext =
			mainModule.Types.First(t => t.Name == ValuePointerPatchTask.IReadOnlyFixedContextName);
		TypeDefinition iFixedContext = mainModule.Types.First(t => t.Name == ValuePointerPatchTask.IFixedContextName);

		ValuePointerPatchTask.ImplementGetUnsafeFixedContextMethod(mainModule, readOnlyValPtrTypeDefinition,
		                                                           iReadOnlyFixedContext, readOnlyFixedContext);
		ValuePointerPatchTask.ImplementGetUnsafeFixedContextMethod(mainModule, valPtrTypeDefinition, iFixedContext,
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