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

		String? assemblyPath = default;
		try
		{
			assemblyPath = assemblyFiles
			               .First(f => f.FullName.Contains(this.TargetFramework,
			                                               StringComparison.InvariantCultureIgnoreCase)).FullName;
			PatchAssemblyTask.PatchAssembly(assemblyPath);
			return true;
		}
		catch (Exception ex)
		{
			this.Log.LogError(
				$"Error: {ex.Message} Dir: {directory.FullName} T: {this.TargetFramework} FF:{assemblyFiles.Length} F: {assemblyPath}");
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
		TypeDefinition? iFixedContext = module.Types.First(t => t.Name.StartsWith("IFixedContext`"));

		PatchAssemblyTask.AddMethod(module, readOnlyValPtrTypeDefinition, iReadOnlyFixedContext, readOnlyFixedContext);
		PatchAssemblyTask.AddMethod(module, valPtrTypeDefinition, iFixedContext, fixedContext);

		assembly.Write();
	}

	public static void Main(String[] args) { PatchAssemblyTask.PatchAssembly(args[0]); }

	private static void AddMethod(ModuleDefinition moduleDefinition, TypeDefinition type, TypeDefinition returnType,
		TypeDefinition classType)
	{
		GenericParameter genericParameter = type.GenericParameters[0];

		// ValPtr<T> or ReadOnlyValPtr<T>
		GenericInstanceType genericType = type.MakeGenericInstanceType(genericParameter);
		// IFixedContext<T>.IDisposable or IReadOnlyFixedContext<T>.IDisposable
		GenericInstanceType? genericIDisposable = moduleDefinition
		                                          .ImportReference(
			                                          returnType.Resolve().NestedTypes
			                                                    .First(nt => nt.Name == "IDisposable"))
		                                          .MakeGenericInstanceType(genericParameter);
		// FixedContext<T> or ReadOnlyFixedContext<T>
		GenericInstanceType? genericClassType = moduleDefinition
		                                        .ImportReference(classType).MakeGenericInstanceType(genericParameter);

		MethodDefinition getUnsafeFixedContextMethod = new("GetUnsafeFixedContext",
		                                                   MethodAttributes.Private | MethodAttributes.HideBySig,
		                                                   genericIDisposable);

		// Method parameters
		getUnsafeFixedContextMethod.Parameters.Add(new("count", ParameterAttributes.None,
		                                               moduleDefinition.ImportReference(typeof(Int32))));
		getUnsafeFixedContextMethod.Parameters.Add(new("disposable", ParameterAttributes.Optional,
		                                               moduleDefinition.ImportReference(typeof(IDisposable))));

		// Obtener referencia al método CreateDisposable<T>
		MethodDefinition? createDisposableMethod = genericClassType.Resolve().Methods.First(
			m => m.IsStatic && m.Name == "CreateDisposable" && m.Parameters.Count == 3 &&
				m.ReturnType.FullName.Contains(returnType.FullName));

		ILProcessor? il = getUnsafeFixedContextMethod.Body.GetILProcessor();

		// 'this'
		il.Emit(OpCodes.Ldarg_0);
		// ValPtr<T>
		il.Emit(OpCodes.Ldobj, moduleDefinition.ImportReference(genericType));
		// count
		il.Emit(OpCodes.Ldarg_1);
		// disposable
		il.Emit(OpCodes.Ldarg_2);
		// CreateDisposable
		il.Emit(OpCodes.Call, moduleDefinition.ImportReference(createDisposableMethod));
		// return
		il.Emit(OpCodes.Ret);

		// Add method
		genericType.Resolve().Methods.Add(getUnsafeFixedContextMethod);
	}
}