using System.Collections;

namespace Rxmxnx.PInvoke.Tests;

#if NETCOREAPP
using TestDelegate = Action;
#else
using TestDelegate = TestDelegate;
#endif

#if NETCOREAPP
/// <inheritdoc cref="Xunit.Assert"/>
#else
/// <inheritdoc cref="NUnit.Framework.Assert"/>
#endif
[ExcludeFromCodeCoverage]
public sealed class PInvokeAssert
{
#if NETCOREAPP
	/// <inheritdoc cref="Xunit.Assert.True(Boolean)"/>
#else
	/// <inheritdoc cref="NUnit.Framework.Assert.True(Boolean)"/>
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void True([DoesNotReturnIf(false)] Boolean? isUnmanaged)
#if NETCOREAPP
		=> Xunit.Assert.True(isUnmanaged);
#else
		=> Assert.True(isUnmanaged);
#endif
#if NETCOREAPP
	/// <inheritdoc cref="Xunit.Assert.False(Boolean)"/>
#else
	/// <inheritdoc cref="NUnit.Framework.Assert.False(Boolean)"/>
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void False([DoesNotReturnIf(true)] Boolean? isUnmanaged)
#if NETCOREAPP
		=> Xunit.Assert.False(isUnmanaged);
#else
		=> Assert.False(isUnmanaged);
#endif
#if NETCOREAPP
	/// <inheritdoc cref="Xunit.Assert.Equal{T}(T,T)"/>
#else
	/// <inheritdoc cref="NUnit.Framework.Assert.AreEqual(Object?,Object?)"/>
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void Equal<T>(T? expected, T? actual)
#if NETCOREAPP
		=> Xunit.Assert.Equal(expected, actual);
#else
		=> Assert.AreEqual(expected, actual);
#endif
#if NETCOREAPP
	/// <inheritdoc cref="Xunit.Assert.NotEqual{T}(T,T)"/>
#else
	/// <inheritdoc cref="NUnit.Framework.Assert.AreNotEqual(Object?,Object?)"/>
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void NotEqual<T>(T? expected, T? actual)
#if NETCOREAPP
		=> Xunit.Assert.NotEqual(expected, actual);
#else
		=> Assert.AreNotEqual(expected, actual);
#endif
#if NETCOREAPP
	/// <inheritdoc cref="Xunit.Assert.Equal{T}(IEnumerable{T}?,IEnumerable{T}?)"/>
#else
	/// <inheritdoc cref="NUnit.Framework.Assert.AreEqual(Object?,Object?)"/>
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void Equal<T>(IEnumerable<T>? expected, IEnumerable<T>? actual)
#if NETCOREAPP
		=> Xunit.Assert.Equal(expected, actual);
#else
		=> Assert.AreEqual(expected, actual);
#endif
#if NETCOREAPP
	/// <inheritdoc cref="Xunit.Assert.Throws{T}(Func{Object})"/>
#else
	/// <inheritdoc cref="NUnit.Framework.Assert.Throws{T}(TestDelegate)"/>
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[return: NotNull]
	public static T? Throws<T>(Func<Object?> testCode) where T : Exception
#if NETCOREAPP
		=> Xunit.Assert.Throws<T>(testCode);
#else
		=> Assert.Throws<T>(() => _ = testCode())!;
#endif
#if NETCOREAPP
	/// <inheritdoc cref="Xunit.Assert.Throws{T}(TestDelegate)"/>
#else
	/// <inheritdoc cref="NUnit.Framework.Assert.Throws{T}(TestDelegate)"/>
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[return: NotNull]
	public static T? Throws<T>(TestDelegate testCode) where T : Exception
#if NETCOREAPP
		=> Xunit.Assert.Throws<T>(testCode);
#else
		=> Assert.Throws<T>(testCode)!;
#endif
#if NETCOREAPP
	/// <inheritdoc cref="Xunit.Assert.NotNull(Object)"/>
#else
	/// <inheritdoc cref="NUnit.Framework.Assert.NotNull(Object)"/>
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void NotNull([NotNull] Object? @object)
#if NETCOREAPP
		=> Xunit.Assert.NotNull(@object);
#else
#pragma warning disable CS8777
		=> Assert.NotNull(@object);
#pragma warning restore CS8777
#endif
#if NETCOREAPP
	/// <inheritdoc cref="Xunit.Assert.Null(Object)"/>
#else
	/// <inheritdoc cref="NUnit.Framework.Assert.Null(Object)"/>
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void Null(Object? @object)
#if NETCOREAPP
		=> Xunit.Assert.Null(@object);
#else
		=> Assert.Null(@object);
#endif
#if NETCOREAPP
	/// <inheritdoc cref="Xunit.Assert.Same(Object, Object)"/>
#else
	/// <inheritdoc cref="NUnit.Framework.Assert.AreSame(Object, Object)"/>
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void Same(Object? expected, Object? actual)
#if NETCOREAPP
		=> Xunit.Assert.Same(expected, actual);
#else
		=> Assert.AreSame(expected, actual);
#endif
#if NETCOREAPP
	/// <inheritdoc cref="Xunit.Assert.ThrowsAny{T}(TestDelegate)"/>
#else
	/// <inheritdoc cref="NUnit.Framework.Assert.Throws{T}(TestDelegate)"/>
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[return: NotNull]
	public static T? ThrowsAny<T>(TestDelegate testCode) where T : Exception
#if NETCOREAPP
		=> Xunit.Assert.ThrowsAny<T>(testCode);
#else
	{
		Exception? ex = Assert.Throws<Exception>(() =>
		{
			try
			{
				testCode();
			}
			catch (T e)
			{
				throw new("", e);
			}
		});
		Assert.IsInstanceOf<T>(ex?.InnerException);
		return (T)ex!.InnerException!;
	}
#endif
#if NETCOREAPP
	/// <inheritdoc cref="Xunit.Assert.NotSame(Object?, Object?)"/>
#else
	/// <inheritdoc cref="NUnit.Framework.Assert.AreNotSame(Object?, Object?)"/>
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void NotSame(Object? expected, Object? actual)
#if NETCOREAPP
		=> Xunit.Assert.NotSame(expected, actual);
#else
		=> Assert.AreNotSame(expected, actual);
#endif
#if NETCOREAPP
	/// <inheritdoc cref="Xunit.Assert.Empty(System.Collections.IEnumerable)"/>
#else
	/// <inheritdoc cref="NUnit.Framework.Assert.IsEmpty(System.Collections.IEnumerable)"/>
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void Empty(IEnumerable collection)
#if NETCOREAPP
		=> Xunit.Assert.Empty(collection);
#else
		=> Assert.IsEmpty(collection);
#endif
#if NETCOREAPP
	/// <inheritdoc cref="Xunit.Assert.IsType{T}(Object?)"/>
#else
	/// <inheritdoc cref="NUnit.Framework.Assert.IsInstanceOf{T}(Object?)"/>
#endif
	[return: NotNull]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static T? IsType<T>([NotNull] Object? @object)
#if NETCOREAPP
		=> Xunit.Assert.IsType<T>(@object)!;
#else
	{
		Assert.IsInstanceOf<T>(@object);
#pragma warning disable CS8777
		return ((T?)@object)!;
#pragma warning restore CS8777
	}
#endif
#if NETCOREAPP
	/// <inheritdoc cref="Xunit.Assert.IsType{T}(Object?, Boolean)"/>
#else
	/// <inheritdoc cref="NUnit.Framework.Assert.IsInstanceOf{T}(Object?)"/>
#endif
	[return: NotNull]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static T? IsType<T>([NotNull] Object? @object, Boolean exactMatch)
#if NETCOREAPP
		=> Xunit.Assert.IsType<T>(@object, exactMatch)!;
#else
	{
		Assert.IsInstanceOf<T>(@object);
		Assert.True(!exactMatch || @object?.GetType() == typeof(T));
#pragma warning disable CS8777
		return ((T?)@object)!;
#pragma warning restore CS8777
	}
#endif
#if NETCOREAPP
	/// <inheritdoc cref="Xunit.Assert.InRange{T}(T, T, T)"/>
#else
	/// <inheritdoc cref="NUnit.Framework.Is.InRange(Object, Object)"/>
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void InRange<T>(T actual, T low, T high) where T : IComparable
#if NETCOREAPP
		=> Xunit.Assert.InRange(actual, low, high);
#else
		=> Assert.That(actual, Is.InRange(low, high));
#endif
#if NETCOREAPP
	/// <inheritdoc cref="Xunit.Assert.Contains{T}(T, IEnumerable{T})"/>
#else
	/// <inheritdoc cref="NUnit.Framework.Assert.Contains(Object, System.Collections.ICollection)"/>
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void Contains<T>(T value, IEnumerable<T> collection)
#if NETCOREAPP
		=> Xunit.Assert.Contains(value, collection);
#else
		=> Assert.Contains(value, collection as ICollection ?? collection.ToArray());
#endif
#if NETCOREAPP
	/// <inheritdoc cref="Xunit.Assert.StrictEqual{T}(T, T)"/>
#else
	/// <inheritdoc cref="NUnit.Framework.Assert.AreSame(Object, Object)"/>
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void StrictEqual<T>(T? expected, T? actual)
#if NETCOREAPP
		=> Xunit.Assert.StrictEqual(expected, actual);
#else
	{
		Assert.AreEqual(expected, actual);
		Assert.AreSame(expected, actual);
	}
#endif
#if NETCOREAPP
	/// <inheritdoc cref="Xunit.Assert.All{T}(IEnumerable{T}, Action{T})"/>
#else
	/// <inheritdoc cref="NUnit.Framework.Assert.That{T}(T, NUnit.Framework.Constraints.IResolveConstraint)"/>
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void All<T>(IEnumerable<T> collection, Action<T> action)
#if NETCOREAPP
		=> Xunit.Assert.All(collection, action);
#else
	{
		foreach (T item in collection)
			action(item);
	}
#endif
#if NETCOREAPP
	/// <inheritdoc cref="Xunit.Assert.NotEmpty(System.Collections.IEnumerable)"/>
#else
	/// <inheritdoc cref="NUnit.Framework.Assert.IsNotEmpty(System.Collections.IEnumerable)"/>
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void NotEmpty(IEnumerable collection)
#if NETCOREAPP
		=> Xunit.Assert.NotEmpty(collection);
#else
		=> Assert.IsNotEmpty(collection);
#endif
}