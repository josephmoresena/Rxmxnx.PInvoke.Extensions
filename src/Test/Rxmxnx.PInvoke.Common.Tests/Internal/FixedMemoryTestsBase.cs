namespace Rxmxnx.PInvoke.Tests.Internal;

/// <summary>
/// Base class for <see cref="FixedMemory"/> tests.
/// </summary>
[ExcludeFromCodeCoverage]
public abstract class FixedMemoryTestsBase
{
	/// <summary>
	/// Message when <see cref="FixedMemory"/> instance is read-only.
	/// </summary>
	protected static readonly String ReadOnlyError = IMessageResource.GetInstance().ReadOnlyInstance;
	/// <summary>
	/// Message when <see cref="FixedMemory"/> instance is invalid.
	/// </summary>
	protected static readonly String InvalidError = IMessageResource.GetInstance().InvalidInstance;
	/// <summary>
	/// Message when <see cref="FixedMemory"/> instance is not a function.
	/// </summary>
	protected static readonly String IsNotFunction = IMessageResource.GetInstance().IsNotFunction;
	/// <summary>
	/// Message when <see cref="FixedMemory"/> instance is a function.
	/// </summary>
	protected static readonly String IsFunction = IMessageResource.GetInstance().IsFunction;
	/// <summary>
	/// Message when <see cref="FixedReference{T}"/> instance is not enough for hold a reference.
	/// </summary>
	protected static readonly String InvalidSizeFormat = IMessageResource.GetInstance()
	                                                                     .InvalidRefTypePointer(
		                                                                     typeof(FixedMemoryTestsBase))
	                                                                     .Replace(
		                                                                     typeof(FixedMemoryTestsBase).ToString(),
		                                                                     "{0}");

	/// <summary>
	/// Fixture instance.
	/// </summary>
	protected static readonly IFixture Fixture = ManagedStruct.Register(new Fixture());
}