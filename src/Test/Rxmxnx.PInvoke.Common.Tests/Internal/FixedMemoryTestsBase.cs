﻿namespace Rxmxnx.PInvoke.Tests.Internal;

/// <summary>
/// Base class for <see cref="FixedMemory"/> tests.
/// </summary>
[ExcludeFromCodeCoverage]
public abstract class FixedMemoryTestsBase
{
	/// <summary>
	/// Message when <see cref="FixedMemory"/> instance is read-only.
	/// </summary>
	public static readonly String ReadOnlyError = "The current instance is read-only.";
	/// <summary>
	/// Message when <see cref="FixedMemory"/> instance is invalid.
	/// </summary>
	public static readonly String InvalidError = "The current instance is not valid.";
	/// <summary>
	/// Message when <see cref="FixedMemory"/> instance is not a function.
	/// </summary>
	public static readonly String IsNotFunction = "The current instance is not a function.";
	/// <summary>
	/// Message when <see cref="FixedMemory"/> instance is a function.
	/// </summary>
	public static readonly String IsFunction = "The current instance is a function.";
	/// <summary>
	/// Message when <see cref="FixedReference{T}"/> instance is not enough for hold a reference.
	/// </summary>
	public static readonly String InvalidSizeFormat =
		"The current instance is insufficient to contain a value of {0} type.";

	/// <summary>
	/// Fixture instance.
	/// </summary>
	protected static readonly IFixture fixture = new Fixture();
}