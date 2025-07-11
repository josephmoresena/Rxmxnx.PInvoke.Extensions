#if !PACKAGE || !NET6_0_OR_GREATER
namespace Rxmxnx.PInvoke.Internal.FrameworkCompat;

internal static unsafe partial class MemoryMarshalCompat
{
	/// <summary>
	/// Helper class for managed reference array data retrieving.
	/// </summary>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
	[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS3218)]
#endif
	private abstract class ArrayReferenceHelper
	{
#pragma warning disable CA2263
		/// <summary>
		/// Singleton instance.
		/// </summary>
		private static ArrayReferenceHelper? instance;

		/// <summary>
		/// Current platform <see cref="ArrayReferenceHelper"/> instance.
		/// </summary>
		public static ArrayReferenceHelper Instance
			=> ArrayReferenceHelper.instance ??= ArrayReferenceHelper.GetInstance();

		/// <summary>
		/// Returns a reference to the 0th element of <paramref name="array"/>.
		/// </summary>
		/// <param name="array">A <see cref="Array"/> instance.</param>
		public abstract ref Byte GetArrayDataReference(Array array);

		/// <summary>
		/// Retrieves the <see cref="ArrayReferenceHelper"/> instance for current runtime.
		/// </summary>
		/// <returns>The <see cref="ArrayReferenceHelper"/> instance for current runtime.</returns>
#if !PACKAGE
		[ExcludeFromCodeCoverage]
#endif
		private static ArrayReferenceHelper GetInstance()
		{
			ValidationUtilities.ThrowIfNoReflection();

			if (typeof(MemoryMarshal).GetMethod("GetArrayDataReference", 0, [typeof(Array),]) is { } methodInfoNet6)
				return new MemoryMarshalImpl(methodInfoNet6);

			const BindingFlags bindingFlags = BindingFlags.Static | BindingFlags.NonPublic;
			Type typeofRuntimeHelpers = typeof(RuntimeHelpers);

			if (typeofRuntimeHelpers.GetMethod("GetMethodTable", bindingFlags) is { } methodInfoNet5)
				return new RuntimeHelpersImpl(methodInfoNet5, bindingFlags);
			if (typeofRuntimeHelpers.GetMethod("GetObjectMethodTablePointer", bindingFlags) is { } methodInfoCore)
				return new RuntimeHelpersImpl(methodInfoCore, bindingFlags);
			return new ArrayImpl();
		}

		/// <summary>
		/// Implementation using <see cref="MemoryMarshal"/> class.
		/// </summary>
		/// <param name="getArrayDataReference">
		/// The <see cref="MethodInfo"/> of <see cref="MemoryMarshal"/> method.
		/// </param>
		private sealed class MemoryMarshalImpl(MethodInfo getArrayDataReference) : ArrayReferenceHelper
		{
			/// <summary>
			/// Delegate to <c>MemoryMarshal.GetArrayDataReference(Array)</c>.
			/// </summary>
			private readonly GetArrayDataReferenceDelegate _delegate =
#if !NET5_0_OR_GREATER
				(GetArrayDataReferenceDelegate)getArrayDataReference.CreateDelegate(
					typeof(GetArrayDataReferenceDelegate));
#else
				getArrayDataReference.CreateDelegate<GetArrayDataReferenceDelegate>();
#endif

			/// <inheritdoc/>
			public override ref Byte GetArrayDataReference(Array array) => ref this._delegate(array);

			/// <summary>
			/// Delegate for <c>MemoryMarshal.GetArrayDataReference(Array)</c>.
			/// </summary>
			private delegate ref Byte GetArrayDataReferenceDelegate(Array array);
		}

		/// <summary>
		/// Implementation using <see cref="RuntimeHelpers"/> class.
		/// </summary>
		private sealed class RuntimeHelpersImpl : ArrayReferenceHelper
		{
			/// <summary>
			/// Internal delegate.
			/// </summary>
			private readonly Func<Object, IntPtr>? _delegate;
			/// <summary>
			/// Generic delegate.
			/// </summary>
			private readonly Func<Delegate, Array, IntPtr>? _genericDelegate;
			/// <summary>
			/// Native delegate.
			/// </summary>
			private readonly Delegate? _nativeDelegate;

			/// <summary>
			/// Constructor.
			/// </summary>
			/// <param name="getMethodTablePointer">
			/// The <see cref="MethodInfo"/> of <see cref="RuntimeHelpers"/> method.
			/// </param>
			/// <param name="bindingFlags">A <see cref="BindingFlags"/> value.</param>
			public RuntimeHelpersImpl(MethodInfo getMethodTablePointer, BindingFlags bindingFlags)
			{
				if (getMethodTablePointer.Name == "GetObjectMethodTablePointer")
				{
					this._delegate =
#if !NET5_0_OR_GREATER
						(Func<Object, IntPtr>)getMethodTablePointer.CreateDelegate(typeof(Func<Object, IntPtr>));
#else
						getMethodTablePointer.CreateDelegate<Func<Object, IntPtr>>();
#endif
					return;
				}

				Type methodTableType = getMethodTablePointer.ReturnType.GetElementType()!;
				Type delegateType = typeof(GetMethodTablePointerDelegate<>).MakeGenericType(methodTableType);
				Type currentType = typeof(RuntimeHelpersImpl);

				this._nativeDelegate = getMethodTablePointer.CreateDelegate(delegateType);
				this._genericDelegate =
#if !NET5_0_OR_GREATER
					(Func<Delegate, Array, IntPtr>)
#endif
					currentType.GetMethod(nameof(RuntimeHelpersImpl.GetMethodTablePointer), bindingFlags)!
					           .MakeGenericMethod(methodTableType)
#if !NET5_0_OR_GREATER
					           .CreateDelegate(typeof(Func<Delegate, Array, IntPtr>));
#else
					           .CreateDelegate<Func<Delegate, Array, IntPtr>>();
#endif
			}

			/// <inheritdoc/>
			public override ref Byte GetArrayDataReference(Array array)
			{
				IntPtr methodTablePtr = this._delegate?.Invoke(array) ??
					this._genericDelegate!.Invoke(this._nativeDelegate!, array);
				ref MethodTableImpl mtpRef = ref Unsafe.AsRef<MethodTableImpl>(methodTablePtr.ToPointer());
				return ref MemoryMarshalCompat.GetArrayDataReference(mtpRef, array);
			}

			/// <summary>
			/// Returns a reference to the 0th element of <paramref name="array"/>.
			/// </summary>
			/// <param name="func">A <see cref="Delegate"/> instance.</param>
			/// <param name="array">A <see cref="Array"/> instance.</param>
			private static IntPtr GetMethodTablePointer<TMethodTable>(Delegate func, Array array)
				where TMethodTable : unmanaged
			{
				GetMethodTablePointerDelegate<TMethodTable> getMethodTablePointer =
					(GetMethodTablePointerDelegate<TMethodTable>)func;
				return new(getMethodTablePointer(array));
			}

			/// <inheritdoc/>
			private delegate TMethodTable* GetMethodTablePointerDelegate<TMethodTable>(Object obj)
				where TMethodTable : unmanaged;
		}

		/// <summary>
		/// Implementation using <see cref="Array"/> class.
		/// </summary>
		private sealed class ArrayImpl : ArrayReferenceHelper
		{
			/// <inheritdoc/>
			public override ref Byte GetArrayDataReference(Array array) => ref Unsafe.As<MonoRawData>(array).Data;
		}
#pragma warning restore CA2263
	}
}
#endif