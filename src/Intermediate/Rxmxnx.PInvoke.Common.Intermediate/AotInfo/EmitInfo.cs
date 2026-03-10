using DynamicMethod = System.Reflection.Emit.DynamicMethod;
using OpCodes = System.Reflection.Emit.OpCodes;
using ILGenerator = System.Reflection.Emit.ILGenerator;

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
		/// Indicates whether <paramref name="methodBase"/> is dynamic.
		/// </summary>
		/// <param name="methodBase">A <see cref="MethodBase"/> instance.</param>
		/// <returns>
		/// <see langword="true"/> if <paramref name="methodBase"/> is a dynamic method or its assembly is dynamic;
		/// otherwise <see langword="false"/>.
		/// </returns>
#if !PACKAGE
		[ExcludeFromCodeCoverage]
#endif
		public static Boolean IsDynamicMethod(MethodBase methodBase)
			=> methodBase is DynamicMethod || methodBase.Module.Assembly.IsDynamic;

		/// <summary>
		/// Indicates whether <see cref="System.Reflection.Emit"/> namespace is supported in the current runtime.
		/// </summary>
		/// <returns>
		/// <see langword="true"/> if a dynamic type was successfully emitted in the current runtime; otherwise,
		/// <see langword="false"/>.
		/// </returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[UnconditionalSuppressMessage("AOT", "IL3050")]
		[UnconditionalSuppressMessage("Trimming", "IL2026")]
#if !PACKAGE
		[SuppressMessage("ReSharper", "HeapView.BoxingAllocation")]
#endif
		private static Boolean EmitCode()
		{
			try
			{
				DynamicMethod method = new($"MyDynamicMethod_{Guid.NewGuid():N}", typeof(MethodBase), Type.EmptyTypes,
				                           typeof(Object).Module, true);
				ILGenerator il = method.GetILGenerator();
				il.Emit(OpCodes.Call, typeof(MethodBase).GetMethod(nameof(MethodBase.GetCurrentMethod))!);
				il.Emit(OpCodes.Ret);
				return method.Invoke(null, []) is not null;
			}
			catch (Exception)
			{
				// Any exception at runtime indicates that System.Reflection.Emit is not allowed.
				return false;
			}
		}
	}
}