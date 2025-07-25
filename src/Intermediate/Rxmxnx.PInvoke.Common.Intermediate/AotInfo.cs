#if !NET6_0_OR_GREATER
using AssemblyBuilder = System.Reflection.Emit.AssemblyBuilder;
using AssemblyBuilderAccess = System.Reflection.Emit.AssemblyBuilderAccess;
#endif

namespace Rxmxnx.PInvoke;

/// <summary>
/// Provides information about the Ahead-of-Time compilation.
/// </summary>
#if !PACKAGE
[ExcludeFromCodeCoverage]
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS3011)]
#endif
public static class AotInfo
{
	/// <summary>
	/// Indicates whether the current runtime is ahead-of-time.
	/// </summary>
	private static readonly Boolean isAotRuntime =
#if !NET6_0_OR_GREATER
		!AotInfo.IsJitEnabled();
#else
		JitInfo.GetCompiledILBytes() == 0L && JitInfo.GetCompiledMethodCount() == 0;
#endif

	/// <summary>
	/// Indicates whether runtime reflection is disabled.
	/// </summary>
	private static Boolean? reflectionDisabled;

	/// <summary>
	/// Indicates whether runtime reflection is disabled.
	/// </summary>
	public static Boolean IsReflectionDisabled
	{
		get
		{
#if NET6_0_OR_GREATER
			if (!AotInfo.IsNativeAot)
				return false;
#endif
			return AotInfo.reflectionDisabled ??= !AotInfo.StringTypeNameContainsString();
		}
	}
	/// <summary>
	/// Indicates whether the current runtime is Native AOT.
	/// </summary>
	public static Boolean IsNativeAot => AotInfo.isAotRuntime;

#if !NET6_0_OR_GREATER
	/// <summary>
	/// Indicates whether JIT is enabled in the current runtime.
	/// </summary>
	/// <returns>
	/// <see langword="true"/> if Jit is enabled; otherwise, <see langword="false"/>.
	/// </returns>
#if NET5_0_OR_GREATER
	[UnconditionalSuppressMessage("Trimming", "IL2070")]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static Boolean IsJitEnabled()
	{
		try
		{
			if (!AotInfo.StringTypeNameContainsString())
				// If reflection disabled, is AOT.
				return false;

			if (Type.GetType("System.Runtime.JitInfo") is { } typeJitInfo)
			{
				// Tries to retrieve JIT information using reflection.
				Boolean? isJitEnabled = AotInfo.IsJitEnabled(typeJitInfo);
				if (isJitEnabled.HasValue)
					return isJitEnabled.Value;
			}

			foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies().AsSpan())
			{
				if (String.IsNullOrEmpty(assembly.FullName)) continue;
				if (assembly.FullName.StartsWith("System.Reflection.Emit")) break;
				if (!assembly.FullName.Contains("Il2Cpp", StringComparison.OrdinalIgnoreCase)) continue;

				// IL2CPP is a AOT mode.
				return false;
			}

			// System.Reflection.Emit is not allowed in NativeAOT.
			return AotInfo.IsEmitSupported();
		}
		// If exception, might be AOT.
		catch (Exception)
		{
			return false;
		}
	}
	/// <summary>
	/// Indicates whether JIT is enabled in the current runtime using reflection.
	/// </summary>
	/// <param name="typeJitInfo">CLR type of System.Runtime.JitInfo class.</param>
	/// <returns>
	/// <see langword="true"/> if Jit is enabled, <see langword="false"/> if Jit is disabled;
	/// otherwise, <see langword="null"/>.
	/// </returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
#if NET5_0_OR_GREATER
	[UnconditionalSuppressMessage("AOT", "IL3050")]
	[UnconditionalSuppressMessage("Trimming", "IL2070")]
#endif
	private static Boolean? IsJitEnabled(Type typeJitInfo)
	{
		const BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;

#if !NET5_0_OR_GREATER
		Type typeofFunc = typeof(Func<Boolean, Int64>);
#endif
		Func<Boolean, Int64>? getCompiledIlBytes = typeJitInfo.GetMethod("GetCompiledILBytes", bindingFlags)
#if !NET5_0_OR_GREATER
		                                                      ?.CreateDelegate(typeofFunc) as Func<Boolean, Int64>;
#else
		                                                      ?.CreateDelegate<Func<Boolean, Int64>>();
#endif
		Int64? reflectionBytes = getCompiledIlBytes?.Invoke(false);

		if (reflectionBytes.GetValueOrDefault() != 0L)
			return true;

		Func<Boolean, Int64>? getCompiledMethodCount = typeJitInfo.GetMethod("GetCompiledMethodCount", bindingFlags)
#if !NET5_0_OR_GREATER
		                                                          ?.CreateDelegate(typeofFunc) as Func<Boolean, Int64>;
#else
		                                                          ?.CreateDelegate<Func<Boolean, Int64>>();
#endif
		Int64? methodCount = getCompiledMethodCount?.Invoke(false);

		if (methodCount.GetValueOrDefault() != 0L)
			return true;

		if (reflectionBytes.HasValue || methodCount.HasValue)
			return false;

		return default; // Unabled to retrieve JIT information.
	}
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
	private static Boolean IsEmitSupported()
	{
		Type? type = AssemblyBuilder
		             .DefineDynamicAssembly(new($"MyDynamicAssembly_{Guid.NewGuid():N}"), AssemblyBuilderAccess.Run)
		             .DefineDynamicModule($"MyDynamicModule_{Guid.NewGuid():N}")
		             .DefineType($"MyDynamicModule_{Guid.NewGuid():N}", TypeAttributes.NotPublic).CreateType();
		return type is not null && Activator.CreateInstance(type) is not null && type.Assembly.IsDynamic;
	}
#endif
	/// <summary>
	/// Indicates whether <see cref="String"/> type name contains the <c>String</c> word.
	/// </summary>
	/// <returns>
	/// <see langword="true"/> if <see cref="String"/> type name contains the <c>String</c> word;
	/// otherwise, <see langword="false"/>.
	/// </returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static Boolean StringTypeNameContainsString()
		=> typeof(String).ToString().AsSpan().EndsWith(nameof(String).AsSpan());
}