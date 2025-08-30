using System.Reflection.Emit;

using TypeBuilder = System.Reflection.Emit.TypeBuilder;
using OpCodes = System.Reflection.Emit.OpCodes;
using MethodBuilder = System.Reflection.Emit.MethodBuilder;
using ILGenerator = System.Reflection.Emit.ILGenerator;
using AssemblyBuilder = System.Reflection.Emit.AssemblyBuilder;
using AssemblyBuilderAccess = System.Reflection.Emit.AssemblyBuilderAccess;

namespace Rxmxnx.PInvoke;

#if !PACKAGE
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS3011)]
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS3267)]
#endif
public static partial class AotInfo
{
	/// <summary>
	/// Provides information about the System.Reflection.Emit runtime.
	/// </summary>
	private static class EmitInfo
	{
		/// <inheritdoc cref="EmitInfo.IsEmitAllowed"/>
		private static Boolean? isEmitAllowed;

		/// <summary>
		/// Indicates whether the current runtime allows the use of System.Reflection.Emit namespace.
		/// </summary>
		public static Boolean IsEmitAllowed => EmitInfo.isEmitAllowed ??= EmitInfo.EmitCode();

		/// <summary>
		/// Indicates whether <see cref="System.Reflection.Emit"/> namespace is supported in the current runtime.
		/// </summary>
		/// <returns>
		/// <see langword="true"/> if a dynamic type was successfully emitted in the current runtime; otherwise,
		/// <see langword="false"/>.
		/// </returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#if NET5_0_OR_GREATER
		[UnconditionalSuppressMessage("AOT", "IL3050")]
#endif
		private static Boolean EmitCode()
		{
			try
			{
				TypeBuilder typeBuilder = AssemblyBuilder
				                          .DefineDynamicAssembly(new($"MyDynamicAssembly_{Guid.NewGuid():N}"),
				                                                 AssemblyBuilderAccess.Run)
				                          .DefineDynamicModule($"MyDynamicModule_{Guid.NewGuid():N}")
				                          .DefineType($"MyDynamicModule_{Guid.NewGuid():N}", TypeAttributes.NotPublic);
				MethodBuilder methodBuilder = typeBuilder.DefineMethod($"MyDynamicMethod_{Guid.NewGuid():N}",
				                                                       MethodAttributes.Public, typeof(Object),
				                                                       [typeof(Int32),]);
				ILGenerator ilGenerator = methodBuilder.GetILGenerator();
				Int32 input = typeBuilder.GetHashCode() - methodBuilder.GetHashCode() + ilGenerator.GetHashCode();
				LocalBuilder localSum = ilGenerator.DeclareLocal(typeof(Int32));
				Label elseLabel = ilGenerator.DefineLabel();

				ilGenerator.Emit(OpCodes.Ldarg_1);
				ilGenerator.Emit(OpCodes.Ldc_I4_1);
				ilGenerator.Emit(OpCodes.Add);
				ilGenerator.Emit(OpCodes.Stloc, localSum);

				ilGenerator.Emit(OpCodes.Ldloc, localSum);
				ilGenerator.Emit(OpCodes.Ldc_I4_0);
				ilGenerator.Emit(OpCodes.Ble_S, elseLabel);

				ilGenerator.Emit(OpCodes.Ldarg_0);
				ilGenerator.Emit(OpCodes.Ret);

				ilGenerator.MarkLabel(elseLabel);
				ilGenerator.Emit(OpCodes.Ldarg_0);
				ilGenerator.Emit(OpCodes.Call, typeof(Object).GetMethod("GetType")!);
				ilGenerator.Emit(OpCodes.Ret);

				return typeBuilder.CreateType() is { } type && Activator.CreateInstance(type) is { } obj &&
					type.Assembly.IsDynamic &&
					type.GetMethod(methodBuilder.Name)?.Invoke(obj, [input,]) is { } result &&
					(input + 1 > 0 ? obj.Equals(result) : type.Equals(result));
			}
			catch (Exception)
			{
				// Any exception at runtime indicates that System.Reflection.Emit is not allowed.
				return false;
			}
		}
	}
}