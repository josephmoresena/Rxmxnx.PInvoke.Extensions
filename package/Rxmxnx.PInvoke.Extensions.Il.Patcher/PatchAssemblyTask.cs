using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Cecil.Rocks;

using Task = Microsoft.Build.Utilities.Task;

namespace Rxmxnx.PInvoke.Extensions.Il.Patcher;

public sealed class PatchAssemblyTask : Task
{
	private static readonly ReaderParameters readParameters = new() { ReadWrite = true, };

	public String? OutputPath { get; set; }
	public String? TargetFramework { get; set; }

	public override Boolean Execute()
	{
		if (String.IsNullOrEmpty(this.OutputPath) || String.IsNullOrEmpty(this.TargetFramework)) return true;

		DirectoryInfo directory = new(this.OutputPath);
		FileInfo[] assemblyFiles = directory.GetFiles("Rxmxnx.PInvoke.Extensions.dll", SearchOption.AllDirectories);

		try
		{
			String assemblyPath = assemblyFiles.First(f => f.FullName.Contains(this.TargetFramework)).FullName;
			PatchAssemblyTask.PatchAssembly(assemblyPath);
			return true;
		}
		catch (Exception ex)
		{
			this.Log.LogError($"Error: {ex.Message}");
			return false;
		}
	}
	private static void PatchAssembly(String assemblyPath)
	{
		using AssemblyDefinition? assembly =
			AssemblyDefinition.ReadAssembly(assemblyPath, PatchAssemblyTask.readParameters);
		using ModuleDefinition? module = assembly.MainModule;

		TypeDefinition? readOnlyValPtrTypeDefinition = module.Types.First(t => t.Name == "ReadOnlyValPtr`1");
		TypeDefinition? valPtrTypeDefinition = module.Types.First(t => t.Name == "ValPtr`1");

		TypeDefinition? readOnlyFixedContext = module.Types.First(t => t.Name.StartsWith("ReadOnlyFixedContext`"));
		TypeDefinition? fixedContext = module.Types.First(t => t.Name.StartsWith("FixedContext`"));
		TypeDefinition? iReadOnlyFixedContext = module.Types.First(t => t.Name.StartsWith("IReadOnlyFixedContext`"));
		TypeDefinition? iFixedContext = module.Types.First(t => t.Name.StartsWith("IReadOnlyFixedContext`"));

		PatchAssemblyTask.AddMethod(module, readOnlyValPtrTypeDefinition, iReadOnlyFixedContext, readOnlyFixedContext);
		PatchAssemblyTask.AddMethod(module, valPtrTypeDefinition, iFixedContext, fixedContext);

		assembly.Write();
	}

	public static void Main(String[] args) { PatchAssemblyTask.PatchAssembly(args[0]); }

	private static void AddMethod(ModuleDefinition moduleDefinition, TypeDefinition type, TypeDefinition returnType,
		TypeDefinition classType)
	{
		GenericParameter genericParameter = type.GenericParameters[0];
		MethodDefinition getUnsafeFixedContextMethod = new("GetUnsafeFixedContext",
		                                                   MethodAttributes.Assembly | MethodAttributes.HideBySig,
		                                                   moduleDefinition.ImportReference(
			                                                                   returnType.Resolve().NestedTypes
				                                                                   .First(
					                                                                   nt => nt.Name == "IDisposable"))
		                                                                   .MakeGenericInstanceType(genericParameter));
		ILProcessor? il = getUnsafeFixedContextMethod.Body.GetILProcessor();
		FieldDefinition valueField = type.MakeGenericInstanceType(genericParameter).Resolve().Fields
		                                 .First(f => f.Name == "_value");
		MethodDefinition? classNew = classType.MakeGenericInstanceType(genericParameter).Resolve().Methods
		                                      .First(m => m.IsConstructor && m.Parameters.Count == 2 &&
			                                             m.Parameters[0].ParameterType.IsPointer &&
			                                             m.Parameters[1].ParameterType.IsPrimitive);
		
		MethodDefinition? toDisposableMethod = classType.MakeGenericInstanceType(genericParameter).Resolve().Methods
		                                                .First(m => m.Name == "ToDisposable");

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

		type.MakeGenericInstanceType(genericParameter).Resolve().Methods.Add(getUnsafeFixedContextMethod);
	}
}