#if !UAP
#if NETFRAMEWORK && !NET46_OR_GREATER
using Array = Rxmxnx.PInvoke.Internal.FrameworkCompat.ArrayCompat;
#endif

namespace Rxmxnx.PInvoke;

#if !PACKAGE
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS3011)]
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS3267)]
#endif
public static partial class AotInfo
{
	/// <summary>
	/// Indicates whether the executing frame is AOT.
	/// </summary>
	/// <returns><see langword="true"/> if executing frame is AOT; otherwise, <see langword="false"/>.</returns>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	[UnconditionalSuppressMessage("Trimming", "IL2026")]
	private static Boolean IsAotFrame()
	{
		Debug.Assert(MonoInfo.MonoAssemblyNameType is not null);
#if !NETCOREAPP && !NETFRAMEWORK && !NETSTANDARD2_0_OR_GREATER
		if (typeof(MethodBase).GetProperty(nameof(MethodBase.MethodHandle)) is not { } handle)
			// Unable to find MethodBase.MethodHandle with reflection.
			return true;
#endif
		StackTrace stackTrace = new();
#if !NETCOREAPP
		// ReSharper disable once UseCollectionExpression
		ReadOnlySpan<StackFrame?> frames = stackTrace.GetFrames() ?? Array.Empty<StackFrame?>();
#else
		ReadOnlySpan<StackFrame?> frames = stackTrace.GetFrames();
#endif
		foreach (StackFrame? frame in frames)
		{
			if (frame?.GetMethod() is not { } methodBase) continue;
			if (EmitInfo.IsDynamicMethod(methodBase)) return false;
#if !NETCOREAPP && !NETFRAMEWORK && !NETSTANDARD2_0_OR_GREATER
			// MethodBase.MethodHandle is not available on older UWP, to avoid static symbol resolution use reflection.
			if (handle.GetValue(methodBase) is not RuntimeMethodHandle methodHandle)
				// Unable to get RuntimeMethodHandle from current instance.
				return true;
#else
			RuntimeMethodHandle methodHandle = methodBase.MethodHandle;
#endif
			if (!AotInfo.IsImageMethodUnsafe(methodHandle)) return false;
		}
		return true;
	}
#if !NET6_0_OR_GREATER
	/// <summary>
	/// Indicates whether JIT is enabled in the current runtime.
	/// </summary>
	/// <returns>
	/// <see langword="true"/> if Jit is enabled; otherwise, <see langword="false"/>.
	/// </returns>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[UnconditionalSuppressMessage("Trimming", "IL2070")]
#if !PACKAGE
	[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS3776)]
	[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS907)]
#endif
	private static Boolean IsJitEnabled()
	{
#if NET5_0_OR_GREATER
		if (TrimInfo.IsMobileTrimmedXnu())
		{
			// iOS, tvOS, watchOS, and macCatalyst are always AOT.
			return false;
		}
#endif
		try
		{
			if (!TrimInfo.StringTypeNameContainsString())
			{
				// If reflection is disabled, the runtime is AOT.
				AotInfo.reflectionDisabled = true;
				return false;
			}
			Boolean isXamarinMac = false;
#if !NET5_0_OR_GREATER
			Boolean isAndroid = false;
#else
			if (TrimInfo.IsDesktopTrimmedPlatform())
				goto JitInfoCheck; // Skip Mono Runtime checks.
			if (OperatingSystem.IsAndroid())
				goto AotFrameCheck; // Skip XNU checks.
#endif
#if NETSTANDARD2_1 || NETCOREAPP2_1_OR_GREATER
			foreach (Assembly assembly in AotInfo.GetAssembliesSpan())
#else
			foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
#endif
			{
				if (String.IsNullOrWhiteSpace(assembly.FullName) || assembly.IsDynamic) continue;
				switch (AotInfo.GetAssemblyName(assembly.FullName))
				{
					case "Xamarin.iOS":
					case "Xamarin.MacCatalyst":
					case "Xamarin.TVOS":
					case "Xamarin.WatchOS":
					case "Microsoft.iOS":
					case "Microsoft.MacCatalyst":
					case "Microsoft.tvOS":
					case "Microsoft.watchOS":
#if NETSTANDARD2_0
					// .NET Native detection
					case "SharedLibrary.McgInterop":
#endif
						return false;
#if !NET5_0_OR_GREATER
					case "Mono.Android":
						isAndroid = true;
						break;
					case "Xamarin.Mac":
#endif
					case "Microsoft.macOS":
						isXamarinMac = true;
						break;
				}
			}
#if NET5_0_OR_GREATER
			AotFrameCheck:
#endif

			if (MonoInfo.MonoAssemblyNameType is not null)
			{
				if (MemoryInspector.IsSupported)
				{
					// IL2CPP -> Empty literal.
					if (MemoryInspector.Instance.IsLiteral(TrimInfo.EmptyUt8Text())) return false;
					// Xamarin.Mac keeps a usable JIT under AOT, so emit cannot decide. Mac ARM64 and Rosetta
					// otherwise stay on emit.
					if (!AotInfo.IsAvoidableEmitCheck() && !isXamarinMac)
						goto EmitCheck;
					return !AotInfo.IsAotFrame();
				}
#if !NET5_0_OR_GREATER
				if (isAndroid)
#else
				if (OperatingSystem.IsAndroid())
#endif
					goto JitInfoCheck;
				goto EmitCheck; // Avoid CoreCLR checks.
			}

			JitInfoCheck:
			// Tries to retrieve JIT information using reflection.
			Type? jitInfoType = typeof(RuntimeInformation).Assembly.GetType("System.Runtime.JitInfo");
			Boolean? isJitEnabled = AotInfo.IsJitEnabled(jitInfoType);
			if (isJitEnabled.HasValue && isJitEnabled.Value)
				return true;
		}
		// If exception, might be AOT.
		catch (Exception)
		{
			return false;
		}
		EmitCheck:
#if NET5_0_OR_GREATER
		if (TrimInfo.IsDesktopTrimmedPlatform() || OperatingSystem.IsAndroid())
			return false; // Avoid using System.Reflection.Emit on .NET 5.0
#endif
		// System.Reflection.Emit is not allowed in AOT/IL2CPP.
		return EmitInfo.IsEmitAllowed;
	}
	/// <summary>
	/// Indicates whether JIT is enabled in the current runtime using reflection.
	/// </summary>
	/// <param name="jitInfoType">CLR type of System.Runtime.JitInfo class.</param>
	/// <returns>
	/// <see langword="true"/> if Jit is enabled, <see langword="false"/> if Jit is disabled;
	/// otherwise, <see langword="null"/>.
	/// </returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[UnconditionalSuppressMessage("AOT", "IL3050")]
	[UnconditionalSuppressMessage("Trimming", "IL2070")]
	private static Boolean? IsJitEnabled(Type? jitInfoType)
	{
		const BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;
		if (jitInfoType is null) return AotInfo.IsJitEnabled(typeof(RuntimeHelpers), bindingFlags);
#if !NET5_0_OR_GREATER
		Type typeofFunc = typeof(Func<Boolean, Int64>);
#endif
		Func<Boolean, Int64>? getCompiledIlBytes = jitInfoType.GetMethod("GetCompiledILBytes", bindingFlags)
#if !NET5_0_OR_GREATER
		                                                      ?.CreateDelegate(typeofFunc) as Func<Boolean, Int64>;
#else
		                                                      ?.CreateDelegate<Func<Boolean, Int64>>();
#endif
		Int64? reflectionBytes = getCompiledIlBytes?.Invoke(false);

		if (reflectionBytes.GetValueOrDefault() != 0L)
			return true;

		Func<Boolean, Int64>? getCompiledMethodCount = jitInfoType.GetMethod("GetCompiledMethodCount", bindingFlags)
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

		return default; // Unable to retrieve JIT information.
	}
	/// <summary>
	/// Indicates whether JIT is enabled in the current runtime using reflection.
	/// </summary>
	/// <param name="runtimeHelpersType">CLR type of <see cref="RuntimeHelpers"/> class.</param>
	/// <param name="bindingFlags">Method binding flags.</param>
	/// <returns>
	/// <see langword="true"/> if Jit is enabled, <see langword="false"/> if Jit is disabled;
	/// otherwise, <see langword="null"/>.
	/// </returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[UnconditionalSuppressMessage("AOT", "IL3050")]
	[UnconditionalSuppressMessage("Trimming", "IL2070")]
	private static Boolean? IsJitEnabled(Type runtimeHelpersType, BindingFlags bindingFlags)
	{
		Func<Int64>? getIlBytesJitted = runtimeHelpersType.GetMethod("GetILBytesJitted", bindingFlags)
#if !NET5_0_OR_GREATER
		                                                  ?.CreateDelegate(typeof(Func<Int64>)) as Func<Int64>;
#else
		                                                  ?.CreateDelegate<Func<Int64>>();
#endif
		Int64? reflectionBytes = getIlBytesJitted?.Invoke();

		if (reflectionBytes.GetValueOrDefault() != 0L)
			return true;

		Func<Int32>? getCompiledMethodCount = runtimeHelpersType.GetMethod("GetMethodsJittedCount", bindingFlags)
#if !NET5_0_OR_GREATER
		                                                        ?.CreateDelegate(typeof(Func<Int32>)) as Func<Int32>;
#else
		                                                        ?.CreateDelegate<Func<Int32>>();
#endif
		Int32? methodCount = getCompiledMethodCount?.Invoke();

		if (methodCount.GetValueOrDefault() != 0)
			return true;

		if (reflectionBytes.HasValue || methodCount.HasValue)
			return false;

		return default; // Unable to retrieve JIT information.
	}
#if NETSTANDARD2_1 || NETCOREAPP2_1_OR_GREATER
	/// <inheritdoc cref="AppDomain.GetAssemblies()"/>
	/// <returns>A read-only span of assemblies in this application domain.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static ReadOnlySpan<Assembly> GetAssembliesSpan()
	{
		Assembly[] array = AppDomain.CurrentDomain.GetAssemblies();
		return MemoryMarshal.CreateReadOnlySpan(ref NativeUtilities.GetArrayDataReference(array), array.Length);
	}
#endif
	/// <summary>
	/// Retrieves the assembly name from its full name.
	/// </summary>
	/// <param name="assemblyFullName">The full name of an assembly.</param>
	/// <returns>The name of the assembly.</returns>
	private static String GetAssemblyName(String assemblyFullName)
	{
		Int32 assemblyNameLength = assemblyFullName.IndexOf(',');
		return assemblyNameLength < 0 ? assemblyFullName : assemblyFullName[..assemblyNameLength];
	}
	/// <summary>
	/// Indicates whether the System.Reflection.Emit check is avoidable at the AOT detection.
	/// </summary>
	/// <returns>
	/// <see langword="true"/> if the System.Reflection.Emit is avoidable at the AOT detection; otherwise,
	/// <see langword="false"/>.
	/// </returns>
	private static Boolean IsAvoidableEmitCheck()
		=> !SystemInfo.IsMac || (RuntimeInformation.ProcessArchitecture is not Architecture.Arm64 &&
			!MemoryInspector.Instance.IsEmulated);
#else
	/// <summary>
	/// Indicates whether the current platform is Desktop or Android.
	/// </summary>
	/// <returns>
	/// <see langword="true"/> if current platform is Desktop or Android; otherwise, <see langword="false"/>.
	/// </returns>
	private static Boolean IsDesktopOrAndroid() => TrimInfo.IsDesktopTrimmedPlatform() || OperatingSystem.IsAndroid();
	/// <summary>
	/// Indicates whether the current runtime is Mono ahead-of-time.
	/// </summary>
	/// <returns>
	/// <see langword="true"/> if Mono AOT is enabled; otherwise, <see langword="false"/>.
	/// </returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static Boolean IsMonoAot()
	{
		if (MonoInfo.MonoAssemblyNameType is null || !MemoryInspector.IsSupported) return false;
		try
		{
			// Mono/Xamarin AOT -> AotFrame. IL2CPP -> Empty literal.
			return AotInfo.IsAotFrame() || MemoryInspector.Instance.IsLiteral(TrimInfo.EmptyUt8Text());
		}
		// If exception, might be AOT.
		catch (Exception)
		{
			return true;
		}
	}
#endif
}
#endif