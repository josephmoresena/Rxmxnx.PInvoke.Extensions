namespace Rxmxnx.PInvoke;

public static partial class NativeUtilities
{
	/// <summary>
	/// Aot runtime detector helper class.
	/// </summary>
	private static class AotDetectorHelper
	{
		/// <summary>
		/// Indicates whether the current runtime might be Ahead-of-Time compiled.
		/// </summary>
		private static Boolean? trimmedOrAot;

		/// <summary>
		/// Value task for <see cref="NativeUtilities.MightBeAot"/>.
		/// </summary>
		/// <returns>A <see cref="ValueTask{Boolean}"/> instance.</returns>
		public static async ValueTask<Boolean> IsTrimmedOrAotAsync()
		{
			// Pre-calculated result.
			if (AotDetectorHelper.trimmedOrAot.HasValue) return AotDetectorHelper.trimmedOrAot.Value;
			if (typeof(String).ToString().Contains(nameof(String))) return await AotDetectorHelper.CheckAotAsync();
			// Reflection disabled, is AOT.
			AotDetectorHelper.trimmedOrAot = true;
			return true;
		}

		/// <summary>
		/// Indicates whether the current runtime might be Ahead-of-Time compiled.
		/// </summary>
		/// <returns>
		/// <paramref langword="true"/> if the current runtime might be Ahead-of-Time compiled;
		/// otherwise, <paramref langword="false"/>.
		/// </returns>
		private static async Task<Boolean> CheckAotAsync()
		{
			Object[] anonObjects =
			[
				new
				{
					Item = 1,
					Value = Double.Epsilon,
					Flag = false,
					Generic = AotDetectorHelper.GetAnonGeneric(String.Empty),
				},
				new
				{
					Item = default(Int32?),
					Value = nameof(AotDetectorHelper.GetObjectWithReflection),
					Flag = true,
					Generic = AotDetectorHelper.GetAnonGeneric(Int32.MaxValue),
				},
				new
				{
					Item = String.Empty,
					Value = Array.Empty<GenericStruct<Object>>(),
					Flag = default(Boolean?),
					Generic = AotDetectorHelper.GetAnonGeneric((Double?)1.9),
				},
			];
			try
			{
				MethodInfo? getAnonGenericInfo = typeof(AotDetectorHelper).GetMethod(
					nameof(AotDetectorHelper.GetAnonGeneric), BindingFlags.Static | BindingFlags.NonPublic);
				if (getAnonGenericInfo is null)
				{
					// Missing method, trimmed assembly or AOT
					AotDetectorHelper.trimmedOrAot = true;
					return true;
				}
				Task<Object>[] tasks =
				[
					Task.Factory.StartNew(AotDetectorHelper.GetObjectWithReflection,
					                      new AotCheckState
					                      {
						                      GetAnonObjectInfo = getAnonGenericInfo, Instance = anonObjects[0],
					                      }),
					Task.Factory.StartNew(AotDetectorHelper.GetObjectWithReflection,
					                      new AotCheckState
					                      {
						                      GetAnonObjectInfo = getAnonGenericInfo, Instance = anonObjects[1],
					                      }),
					Task.Factory.StartNew(AotDetectorHelper.GetObjectWithReflection,
					                      new AotCheckState
					                      {
						                      GetAnonObjectInfo = getAnonGenericInfo, Instance = anonObjects[2],
					                      }),
				];
				await Task.WhenAll(tasks);
				// No missing metadata, may not is AOT.
				return false;
			}
			catch (NotSupportedException)
			{
				// Missing native code or metadata, is AOT.
				return true;
			}
		}

		/// <summary>
		/// Retrieves a <see cref="Object"/> using reflection.
		/// </summary>
		/// <param name="state">State object.</param>
		/// <returns>A <see cref="Object"/> instance.</returns>
		private static Object GetObjectWithReflection(Object? state)
		{
			AotCheckState aotState = (AotCheckState)state!;
			return AotDetectorHelper.GetObjectWithReflection(aotState.GetAnonObjectInfo, aotState.Instance);
		}
		/// <summary>
		/// Retrieves a <see cref="Object"/> using reflection.
		/// </summary>
		/// <param name="getAnonGenericInfo">A <see cref="MethodInfo"/> instance.</param>
		/// <param name="anonObj">A <see cref="Object"/> instance.</param>
		/// <returns>A <see cref="Object"/> instance.</returns>
		[UnconditionalSuppressMessage("AOT", "IL3050")]
		private static Object GetObjectWithReflection(MethodInfo getAnonGenericInfo, Object anonObj)
		{
			Int32 iteration = Random.Shared.Next(2, 10);
			Object result = anonObj;
			while (iteration > 0)
			{
				Type currentType = anonObj.GetType();
				MethodInfo genericMethod = AotDetectorHelper.GetGenericMethod(getAnonGenericInfo, currentType);
				result = genericMethod.Invoke(null, [anonObj,])!;
				Type arrayType = result.GetType().MakeArrayType();
				Type genericType = typeof(GenericStruct<>).MakeGenericType(arrayType);
				anonObj = Activator.CreateInstance(genericType)!;
				iteration--;
			}
			return result;
		}
		/// <summary>
		/// Retrieves a generic <see cref="MethodInfo"/>.
		/// </summary>
		/// <param name="getAnonGenericInfo">A <see cref="MethodInfo"/> generic definition.</param>
		/// <param name="currentType">Type for generics.</param>
		/// <returns>A generic method <see cref="MethodInfo"/> instance.</returns>
		[UnconditionalSuppressMessage("AOT", "IL3050")]
		[UnconditionalSuppressMessage("Trimming", "IL2060")]
		private static MethodInfo GetGenericMethod(MethodInfo getAnonGenericInfo, Type currentType)
		{
			MethodInfo genericMethod = getAnonGenericInfo.MakeGenericMethod(currentType);
			return genericMethod;
		}
		/// <summary>
		/// This method creates an anonymous object using <paramref name="val"/>.
		/// </summary>
		/// <typeparam name="T">Type of generic value.</typeparam>
		/// <param name="val">Generic object.</param>
		/// <returns>A <see cref="Object"/> instance.</returns>
		private static Object GetAnonGeneric<T>(T val)
			=> new
			{
				Item = 1, Value = 2.3, Flag = false, Generic = val,
			};

		/// <summary>
		/// Private structure.
		/// </summary>
		/// <typeparam name="T">Type of generic value.</typeparam>
		private readonly struct GenericStruct<T> : IWrapper<T>
		{
			public T Value { get; init; }
		}

		/// <summary>
		/// AOT check state.
		/// </summary>
		private sealed class AotCheckState
		{
			public MethodInfo GetAnonObjectInfo { get; init; } = default!;
			public Object Instance { get; init; } = default!;
		}
	}
}