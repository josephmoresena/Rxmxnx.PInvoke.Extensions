using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Cecil.Rocks;

namespace Rxmxnx.PInvoke.Extensions.Il.Patcher;

public partial class PatchAssemblyTask
{
	/// <summary>
	/// Implements <c>GetUnsafeFixedContext&lt;&gt;(Int32, System.IDisposable)</c> method in generic
	/// <paramref name="ptrType"/>.
	/// </summary>
	/// <param name="module"><c>Rxmxnx.PInvoke.Extensions</c> module.</param>
	/// <param name="ptrType">Value pointer type.</param>
	/// <param name="interfaceType">Fixed interface type.</param>
	/// <param name="fixedClassType">Fixed class type.</param>
	private static void ImplementGetUnsafeFixedContextMethod(ModuleDefinition module, TypeDefinition ptrType,
		TypeDefinition interfaceType, TypeDefinition fixedClassType)
	{
		// .IDisposable type name
		const String iDisposableName = "IDisposable";
		// <>.CreateDisposable(, Int32, System.IDisposable) method name.
		const String createDisposableName = "CreateDisposable";
		// GetUnsafeFixedContext<>(Int32, System.IDisposable) method name.
		const String getUnsafeFixedContextName = "GetUnsafeFixedContext";
		// GetUnsafeFixedContext<>(Int32, System.IDisposable) method attributes.
		const MethodAttributes getUnsafeFixedContextAttributes = MethodAttributes.Public | MethodAttributes.HideBySig;

		// Interface full name.
		String interfaceName = interfaceType.FullName;
		// T parameter.
		GenericParameter genericParameter = ptrType.GenericParameters[0];
		// Generic value pointer type.
		GenericInstanceType genericType = ptrType.MakeGenericInstanceType(genericParameter);
		// Generic fixed disposable interface type.
		GenericInstanceType? genericIDisposable = module
		                                          .ImportReference(
			                                          interfaceType.NestedTypes.First(nt => nt.Name == iDisposableName))
		                                          .MakeGenericInstanceType(genericParameter);

		// <>.CreateDisposable(, Int32, System.IDisposable) method definition.
		MethodDefinition? createDisposableMethod = fixedClassType.Methods.First(
			m => m.IsStatic && m.Name == createDisposableName && m.Parameters.Count == 3 &&
				m.ReturnType.FullName.Contains(interfaceName));

		// Generic fixed class type.
		GenericInstanceType genericClassType = new(fixedClassType);
		genericClassType.GenericArguments.Add(genericParameter);

		// <T>.CreateDisposable(<T>, Int32, System.IDisposable) method definition.
		MethodReference createDisposableReference =
			new(createDisposableMethod.Name, createDisposableMethod.ReturnType, genericClassType) { HasThis = false, };

		createDisposableReference.Parameters.Add(new(genericType));
		createDisposableReference.Parameters.Add(new(createDisposableMethod.Parameters[1].ParameterType));
		createDisposableReference.Parameters.Add(new(createDisposableMethod.Parameters[2].ParameterType));

		// GetUnsafeFixedContext<>(Int32, System.IDisposable)
		MethodDefinition getUnsafeFixedContextMethod = new(getUnsafeFixedContextName,
		                                                   getUnsafeFixedContextAttributes, genericIDisposable);
		// Add method parameters
		getUnsafeFixedContextMethod.Parameters.Add(createDisposableMethod.Parameters[1]);
		getUnsafeFixedContextMethod.Parameters.Add(createDisposableMethod.Parameters[2]);

		ILProcessor? il = getUnsafeFixedContextMethod.Body.GetILProcessor();

		// 'this'
		il.Emit(OpCodes.Ldarg_0);
		// ValPtr<T>
		il.Emit(OpCodes.Ldobj, module.ImportReference(genericType));
		// count
		il.Emit(OpCodes.Ldarg_1);
		// disposable
		il.Emit(OpCodes.Ldarg_2);
		// Call <T>.CreateDisposable(<T>, Int32, System.IDisposable)
		il.Emit(OpCodes.Call, module.ImportReference(createDisposableReference));
		// return
		il.Emit(OpCodes.Ret);

		// Add method
		genericType.Resolve().Methods.Add(getUnsafeFixedContextMethod);
	}
}