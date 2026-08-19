namespace Rxmxnx.PInvoke;

public readonly ref partial struct FixedPointerValue
{
	/// <summary>
	/// Attempts to create a read-only binary context value from the current instance.
	/// </summary>
	/// <param name="binaryContext">Output. Created instance.</param>
	/// <returns>
	/// <see langword="true"/> if the context instance was successfully created; otherwise, <see langword="false"/>.
	/// </returns>
	public Boolean TryGetReadOnlyBinaryContext(out ReadOnlyFixedContextValue<Byte> binaryContext)
	{
#if NETSTANDARD2_0_OR_GREATER || NETCOREAPP || NETFRAMEWORK || UAP10_0_16299
		if (this.IsUnmanaged && this.Type is not { IsValueType: false, })
#else
		if (this.IsUnmanaged && this.Type?.GetTypeInfo() is not { IsValueType: false, })
#endif
		{
			binaryContext = new(this);
			return true;
		}
		binaryContext = default;
		return false;
	}
	/// <summary>
	/// Attempts to create a read-only object context value from the current instance.
	/// </summary>
	/// <param name="objectContext">Output. Created instance.</param>
	/// <returns>
	/// <see langword="true"/> if the context instance was successfully created; otherwise, <see langword="false"/>.
	/// </returns>
	public Boolean TryGetReadOnlyObjectContext(out ReadOnlyFixedContextValue<Object> objectContext)
	{
#if NETSTANDARD2_0_OR_GREATER || NETCOREAPP || NETFRAMEWORK || UAP10_0_16299
		if (!this.IsUnmanaged && this.Type is { IsValueType: false, })
#else
		if (!this.IsUnmanaged && this.Type?.GetTypeInfo() is { IsValueType: false, })
#endif
		{
			objectContext = new(this);
			return true;
		}
		objectContext = default;
		return false;
	}
	/// <summary>
	/// Attempts to create a read-only binary context value from the current instance.
	/// </summary>
	/// <param name="binaryContext">Output. Created instance.</param>
	/// <returns>
	/// <see langword="true"/> if the context instance was successfully created; otherwise, <see langword="false"/>.
	/// </returns>
	public Boolean TryGetBinaryContext(out FixedContextValue<Byte> binaryContext)
	{
#if NETSTANDARD2_0_OR_GREATER || NETCOREAPP || NETFRAMEWORK || UAP10_0_16299
		if (!this.IsReadOnly && this.IsUnmanaged && this.Type is not { IsValueType: false, })
#else
		if (!this.IsReadOnly && this.IsUnmanaged && this.Type?.GetTypeInfo() is not { IsValueType: false, })
#endif
		{
			binaryContext = new(this);
			return true;
		}
		binaryContext = default;
		return false;
	}
	/// <summary>
	/// Attempts to create a read-only object context value from the current instance.
	/// </summary>
	/// <param name="objectContext">Output. Created instance.</param>
	/// <returns>
	/// <see langword="true"/> if the context instance was successfully created; otherwise, <see langword="false"/>.
	/// </returns>
	public Boolean TryGetObjectContext(out FixedContextValue<Object> objectContext)
	{
#if NETSTANDARD2_0_OR_GREATER || NETCOREAPP || NETFRAMEWORK || UAP10_0_16299
		if (!this.IsReadOnly && !this.IsUnmanaged && this.Type is { IsValueType: false, })
#else
		if (!this.IsReadOnly && this.IsUnmanaged && this.Type?.GetTypeInfo() is { IsValueType: false, })
#endif
		{
			objectContext = new(this);
			return true;
		}
		objectContext = default;
		return false;
	}
}