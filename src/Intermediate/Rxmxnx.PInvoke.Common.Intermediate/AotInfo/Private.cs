namespace Rxmxnx.PInvoke;

#if !PACKAGE
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS3011)]
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS3267)]
#endif
public static partial class AotInfo
{
#if !NET6_0_OR_GREATER
	/// <summary>
	/// Indicates whether JIT is enabled in the current runtime.
	/// </summary>
	/// <returns>
	/// <see langword="true"/> if Jit is enabled; otherwise, <see langword="false"/>.
	/// </returns>
	[UnconditionalSuppressMessage("Trimming", "IL2070")]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static Boolean IsJitEnabled()
	{
		try
		{
			if (!TrimInfo.StringTypeNameContainsString())
			{
				// If reflection disabled, is AOT.
				AotInfo.reflectionDisabled = true;
				return false;
			}

			if (typeof(RuntimeInformation).Assembly.GetType("System.Runtime.JitInfo") is { } typeJitInfo)
			{
				// Tries to retrieve JIT information using reflection.
				Boolean? isJitEnabled = AotInfo.IsJitEnabled(typeJitInfo);
				if (isJitEnabled.HasValue && isJitEnabled.Value)
					return true;
			}

			foreach (Assembly assembly in AotInfo.GetAssembliesSpan())
			{
				if (String.IsNullOrEmpty(assembly.FullName)) continue;
				if (assembly.FullName.StartsWith("System.Reflection.Emit")) break;
				if (!assembly.FullName.Contains("Il2Cpp", StringComparison.OrdinalIgnoreCase)) continue;

				// IL2CPP is an AOT mode.
				return false;
			}

// 			if (MonoInfo.MonoRuntimeType is not null)
// 			{
// 				// Mono/Xamarin
// 				StackTrace stackTrace = new();
// #if !NETCOREAPP
// 				StackFrame?[] frames = stackTrace.GetFrames() ?? [];
// #else
// 				StackFrame?[] frames = stackTrace.GetFrames();
// #endif
// 				Boolean isAot = true;
// 				foreach (StackFrame? frame in frames)
// 					if (frame is null)
// 						continue;
// 				return isAot;
// 			}

			// System.Reflection.Emit is not allowed in AOT.
			return EmitInfo.IsEmitAllowed;
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
	[UnconditionalSuppressMessage("AOT", "IL3050")]
	[UnconditionalSuppressMessage("Trimming", "IL2070")]
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
	/// <inheritdoc cref="AppDomain.GetAssemblies()"/>
	/// <returns>A read-only span of assemblies in this application domain.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static ReadOnlySpan<Assembly> GetAssembliesSpan()
	{
		Assembly[] array = AppDomain.CurrentDomain.GetAssemblies();
		return MemoryMarshal.CreateReadOnlySpan(ref NativeUtilities.GetArrayDataReference(array), array.Length);
	}
	// private static Boolean IsNativeMethod(RuntimeMethodHandle methodHandle)
	// {
	// 	
	// }
#endif
}