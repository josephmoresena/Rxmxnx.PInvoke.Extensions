namespace Rxmxnx.PInvoke;

public static partial class SystemInfo
{
	/// <summary>
	/// Specifies all interfaces implemented by a type.
	/// </summary>
	// ReSharper disable once MemberCanBePrivate.Global
#if NET5_0
	internal const DynamicallyAccessedMemberTypes InterfaceAccess = (DynamicallyAccessedMemberTypes)0x2000;
#else
	internal const DynamicallyAccessedMemberTypes InterfaceAccess = DynamicallyAccessedMemberTypes.Interfaces;
#endif

#if !PACKAGE || TEMP_PACKAGE
	/// <summary>
	/// Target framework for the current build.
	/// </summary>
	public const String CompilationFramework =
#if NET10_0_OR_GREATER
			".NET 10.0"
#elif NET9_0_OR_GREATER
			".NET 9.0"
#elif NET8_0_OR_GREATER
			".NET 8.0"
#elif NET7_0_OR_GREATER
			".NET 7.0"
#elif NET6_0_OR_GREATER
			".NET 6.0"
#elif NET5_0_OR_GREATER
			".NET 5.0"
#elif NETCOREAPP3_1
			".NET Core 3.1"
#elif NETCOREAPP3_0
			".NET Core 3.0"
#elif NETCOREAPP2_2
			".NET Core 2.2"
#elif NETCOREAPP2_1
			".NET Core 2.1"
#elif NETCOREAPP2_0
			".NET Core 2.0"
#elif NETSTANDARD2_1
			".NET Standard 2.1"
#elif NET472_OR_GREATER
			".NET Framework 4.7.2"
#elif NET47_OR_GREATER
			".NET Framework 4.7.0"
#elif NET462_OR_GREATER
			".NET Framework 4.6.2"
#elif NET461_OR_GREATER
			".NET Framework 4.6.1"
#elif NET45_OR_GREATER
			".NET Framework 4.5.0"
#elif UAP10_0_16299
			".NET for Windows Universal 10.0.16299"
#elif UAP10_0
			".NET for Windows Universal"
#else
			".NET Standard 2.0"
#endif
		;
#endif
	/// <summary>
	/// Retrieves the number of interfaces implemented by <typeparamref name="T"/> type.
	/// </summary>
	/// <typeparam name="T">Generic type.</typeparam>
	/// <returns>Number of interfaces implemented by <typeparamref name="T"/> type.</returns>
	internal static Int32 CountInterfaces<[DynamicallyAccessedMembers(SystemInfo.InterfaceAccess)] T>()
		=> typeof(T).GetInterfaces().Length;
	/// <summary>
	/// Indicates whether <typeparamref name="T"/> implements <see cref="IEquatable{T}"/> interface.
	/// </summary>
	/// <typeparam name="T">Generic managed type.</typeparam>
	/// <returns>
	/// <see langword="true"/> if <typeparamref name="T"/> implements <see cref="IEquatable{T}"/> interface; otherwise,
	/// <see langword="false"/>.
	/// </returns>
	internal static Boolean IsSelfEquatable<[DynamicallyAccessedMembers(SystemInfo.InterfaceAccess)] T>()
	{
		Type equatable = typeof(IEquatable<T>);
		ReadOnlySpan<Type> interfaces = typeof(T).GetInterfaces();
		foreach (Type t in interfaces)
		{
			if (equatable == t)
				return true;
		}
		return false;
	}
}