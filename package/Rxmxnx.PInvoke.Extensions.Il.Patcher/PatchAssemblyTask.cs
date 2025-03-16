using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Cecil.Rocks;

using Task = Microsoft.Build.Utilities.Task;

namespace Rxmxnx.PInvoke.Extensions.Il.Patcher;

public sealed class PatchAssemblyTask : Task
{
	public String? OutputPath { get; set; }
	public String? TargetFramework { get; set; }

	public override Boolean Execute()
	{
		if (String.IsNullOrEmpty(this.OutputPath) || String.IsNullOrEmpty(this.TargetFramework)) return true;

		ReaderParameters readParameters = new() { ReadWrite = true, };

		DirectoryInfo directory = new(this.OutputPath);
		FileInfo[] assemblyFiles = directory.GetFiles("Rxmxnx.PInvoke.Extensions.dll");

		try
		{
			using AssemblyDefinition? assembly =
				AssemblyDefinition.ReadAssembly(assemblyFiles.First().FullName, readParameters);
			using ModuleDefinition? module = assembly.MainModule;

			TypeDefinition? readOnlyValPtrTypeDefinition = module.Types.First(t => t.Name == "ReadOnlyValPtr`1");
			TypeDefinition? valPtrTypeDefinition = module.Types.First(t => t.Name == "ValPtr`1");

			GenericInstanceType? readOnlyFixedContext = module.Types
			                                                  .First(t => t.Name.StartsWith("ReadOnlyFixedContext`"))
			                                                  .MakeGenericInstanceType(
				                                                  readOnlyValPtrTypeDefinition.GenericParameters[0]);
			GenericInstanceType? fixedContext = module.Types.First(t => t.Name.StartsWith("FixedContext`"))
			                                          .MakeGenericInstanceType(
				                                          valPtrTypeDefinition.GenericParameters[0]);
			GenericInstanceType? iReadOnlyFixedContext = module.Types
			                                                   .First(t => t.Name.StartsWith("IReadOnlyFixedContext`"))
			                                                   .MakeGenericInstanceType(
				                                                   readOnlyValPtrTypeDefinition.GenericParameters[0]);
			GenericInstanceType? iFixedContext = module.Types.First(t => t.Name.StartsWith("IReadOnlyFixedContext`"))
			                                           .MakeGenericInstanceType(
				                                           valPtrTypeDefinition.GenericParameters[0]);

			PatchAssemblyTask.AddMethod(module, readOnlyValPtrTypeDefinition, iReadOnlyFixedContext,
			                            readOnlyFixedContext);
			PatchAssemblyTask.AddMethod(module, valPtrTypeDefinition, iFixedContext, fixedContext);

			assembly.Write();
			return true;
		}
		catch (Exception ex)
		{
			this.Log.LogError($"Error: {ex.Message}");
			return false;
		}
	}

	public static void Main(String[] args) { }

	private static void AddMethod(ModuleDefinition moduleDefinition, TypeDefinition type,
		GenericInstanceType returnType, GenericInstanceType classType)
	{
		MethodDefinition getUnsafeFixedContextMethod = new("GetUnsafeFixedContext",
		                                                   MethodAttributes.Assembly | MethodAttributes.HideBySig,
		                                                   moduleDefinition.ImportReference(returnType));
		ILProcessor? il = getUnsafeFixedContextMethod.Body.GetILProcessor();
		FieldDefinition valueField = type.Fields.First(f => f.Name == "_value");
		MethodDefinition? classNew = classType.Resolve().Methods
		                                      .First(m => m.IsConstructor && m.Parameters.Count == 2 &&
			                                             m.Parameters[0].ParameterType.IsPointer &&
			                                             m.Parameters[1].ParameterType.IsPrimitive);
		MethodDefinition? toDisposableMethod = classType.Resolve().Methods.First(m => m.Name == "ToDisposable");

		getUnsafeFixedContextMethod.Parameters.Add(new("count", ParameterAttributes.None,
		                                               moduleDefinition.ImportReference(typeof(Int32))));
		getUnsafeFixedContextMethod.Parameters.Add(new("disposable", ParameterAttributes.Optional,
		                                               moduleDefinition.ImportReference(typeof(IDisposable))));

		// _value
		il.Emit(OpCodes.Ldarg_0);
		il.Emit(OpCodes.Ldfld, valueField);

		// Count
		il.Emit(OpCodes.Ldarg_1);

		// New
		il.Emit(OpCodes.Newobj, moduleDefinition.ImportReference(classNew));

		// disposable
		il.Emit(OpCodes.Ldarg_2);

		il.Emit(OpCodes.Call, moduleDefinition.ImportReference(toDisposableMethod));

		// Return
		il.Emit(OpCodes.Ret);

		type.Methods.Add(getUnsafeFixedContextMethod);
	}
}