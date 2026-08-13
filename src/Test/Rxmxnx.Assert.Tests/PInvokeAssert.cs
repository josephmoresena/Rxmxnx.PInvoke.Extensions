using IEnumerable = System.Collections.IEnumerable;
#if !NETCOREAPP && !NETFRAMEWORK && !UAP
using ICollection = System.Collections.ICollection;
#endif

namespace Rxmxnx.PInvoke.Tests;

#if NETCOREAPP || NETFRAMEWORK || UAP
using TestDelegate = Action;
#else
using TestDelegate = TestDelegate;
#endif

#if NETCOREAPP || NETFRAMEWORK || UAP
/// <inheritdoc cref="Assert"/>
#else
/// <inheritdoc cref="Assert"/>
#endif
[ExcludeFromCodeCoverage]
public sealed class PInvokeAssert
{
#if NETCOREAPP || NETFRAMEWORK || UAP
	/// <inheritdoc cref="Assert.True(Boolean)"/>
#else
	/// <inheritdoc cref="Assert.True(Boolean)"/>
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void True([DoesNotReturnIf(false)] Boolean? condition)
#if NETCOREAPP || NETFRAMEWORK || UAP
		=> Assert.True(condition);
#else
		=> Assert.True(condition);
#endif
#if NETCOREAPP || NETFRAMEWORK || UAP
	/// <inheritdoc cref="Assert.False(Boolean)"/>
#else
	/// <inheritdoc cref="Assert.False(Boolean)"/>
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void False([DoesNotReturnIf(true)] Boolean? condition)
#if NETCOREAPP || NETFRAMEWORK || UAP
		=> Assert.False(condition);
#else
		=> Assert.False(condition);
#endif
#if NETCOREAPP || NETFRAMEWORK || UAP
	/// <inheritdoc cref="Assert.Equal{T}(T,T)"/>
#else
	/// <inheritdoc cref="Assert.AreEqual(Object?,Object?)"/>
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void Equal<T>(T? expected, T? actual)
#if NETCOREAPP || NETFRAMEWORK || UAP
		=> Assert.Equal(expected, actual);
#else
		=> Assert.AreEqual(expected, actual);
#endif
#if NETCOREAPP || NETFRAMEWORK || UAP
	/// <inheritdoc cref="Assert.NotEqual{T}(T,T)"/>
#else
	/// <inheritdoc cref="Assert.AreNotEqual(Object?,Object?)"/>
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void NotEqual<T>(T? expected, T? actual)
#if NETCOREAPP || NETFRAMEWORK || UAP
		=> Assert.NotEqual(expected, actual);
#else
		=> Assert.AreNotEqual(expected, actual);
#endif
#if NETCOREAPP || NETFRAMEWORK || UAP
	/// <inheritdoc cref="Assert.Equal{T}(IEnumerable{T}?,IEnumerable{T}?)"/>
#else
	/// <inheritdoc cref="Assert.AreEqual(Object?,Object?)"/>
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void Equal<T>(IEnumerable<T>? expected, IEnumerable<T>? actual)
#if NETCOREAPP || NETFRAMEWORK || UAP
		=> Assert.Equal(expected, actual);
#else
		=> Assert.AreEqual(expected, actual);
#endif
#if NETCOREAPP || NETFRAMEWORK || UAP
	/// <inheritdoc cref="Assert.Throws{T}(Func{Object})"/>
#else
	/// <inheritdoc cref="Assert.Throws{T}(TestDelegate)"/>
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[return: NotNull]
	public static T? Throws<T>(Func<Object?> testCode) where T : Exception
#if NETCOREAPP || NETFRAMEWORK || UAP
		=> Assert.Throws<T>(testCode);
#else
		=> Assert.Throws<T>(() => _ = testCode())!;
#endif
#if NETCOREAPP || NETFRAMEWORK || UAP
	/// <inheritdoc cref="Assert.Throws{T}(TestDelegate)"/>
#else
	/// <inheritdoc cref="Assert.Throws{T}(TestDelegate)"/>
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[return: NotNull]
	public static T? Throws<T>(TestDelegate testCode) where T : Exception
#if NETCOREAPP || NETFRAMEWORK || UAP
		=> Assert.Throws<T>(testCode);
#else
		=> Assert.Throws<T>(testCode)!;
#endif
#if NETCOREAPP || NETFRAMEWORK || UAP
	/// <inheritdoc cref="Assert.NotNull(Object)"/>
#else
	/// <inheritdoc cref="Assert.NotNull(Object)"/>
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void NotNull([NotNull] Object? @object)
#if NETCOREAPP || NETFRAMEWORK || UAP
		=> Assert.NotNull(@object);
#else
#pragma warning disable CS8777
		=> Assert.NotNull(@object);
#pragma warning restore CS8777
#endif
#if NETCOREAPP || NETFRAMEWORK || UAP
	/// <inheritdoc cref="Assert.Null(Object)"/>
#else
	/// <inheritdoc cref="Assert.Null(Object)"/>
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void Null(Object? @object)
#if NETCOREAPP || NETFRAMEWORK || UAP
		=> Assert.Null(@object);
#else
		=> Assert.Null(@object);
#endif
#if NETCOREAPP || NETFRAMEWORK || UAP
	/// <inheritdoc cref="Assert.Same(Object, Object)"/>
#else
	/// <inheritdoc cref="Assert.AreSame(Object, Object)"/>
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void Same(Object? expected, Object? actual)
#if NETCOREAPP || NETFRAMEWORK || UAP
		=> Assert.Same(expected, actual);
#else
		=> Assert.AreSame(expected, actual);
#endif
#if NETCOREAPP || NETFRAMEWORK || UAP
	/// <inheritdoc cref="Assert.ThrowsAny{T}(TestDelegate)"/>
#else
	/// <inheritdoc cref="Assert.Throws{T}(TestDelegate)"/>
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[return: NotNull]
	public static T? ThrowsAny<T>(TestDelegate testCode) where T : Exception
#if NETCOREAPP || NETFRAMEWORK || UAP
		=> Assert.ThrowsAny<T>(testCode);
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
#if NETCOREAPP || NETFRAMEWORK || UAP
	/// <inheritdoc cref="Assert.NotSame(Object?, Object?)"/>
#else
	/// <inheritdoc cref="Assert.AreNotSame(Object?, Object?)"/>
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void NotSame(Object? expected, Object? actual)
#if NETCOREAPP || NETFRAMEWORK || UAP
		=> Assert.NotSame(expected, actual);
#else
		=> Assert.AreNotSame(expected, actual);
#endif
#if NETCOREAPP || NETFRAMEWORK || UAP
	/// <inheritdoc cref="Assert.Empty(System.Collections.IEnumerable)"/>
#else
	/// <inheritdoc cref="Assert.IsEmpty(System.Collections.IEnumerable)"/>
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void Empty(IEnumerable collection)
#if NETCOREAPP || NETFRAMEWORK || UAP
		=> Assert.Empty(collection);
#else
		=> Assert.IsEmpty(collection);
#endif
#if NETCOREAPP || NETFRAMEWORK || UAP
	/// <inheritdoc cref="Assert.IsType{T}(Object?)"/>
#else
	/// <inheritdoc cref="Assert.IsInstanceOf{T}(Object?)"/>
#endif
	[return: NotNull]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static T? IsType<T>([NotNull] Object? @object)
#if NETCOREAPP || NETFRAMEWORK || UAP
		=> Assert.IsType<T>(@object)!;
#else
	{
		Assert.IsInstanceOf<T>(@object);
#pragma warning disable CS8777
		return ((T?)@object)!;
#pragma warning restore CS8777
	}
#endif
#if NETCOREAPP || NETFRAMEWORK || UAP
	/// <inheritdoc cref="Assert.IsType{T}(Object?, Boolean)"/>
#else
	/// <inheritdoc cref="Assert.IsInstanceOf{T}(Object?)"/>
#endif
	[return: NotNull]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static T? IsType<T>([NotNull] Object? @object, Boolean exactMatch)
#if NETCOREAPP || NETFRAMEWORK || UAP
		=> Assert.IsType<T>(@object, exactMatch)!;
#else
	{
		Assert.IsInstanceOf<T>(@object);
		Assert.True(!exactMatch || @object?.GetType() == typeof(T));
#pragma warning disable CS8777
		return ((T?)@object)!;
#pragma warning restore CS8777
	}
#endif
#if NETCOREAPP || NETFRAMEWORK || UAP
	/// <inheritdoc cref="Assert.InRange{T}(T, T, T)"/>
#else
	/// <inheritdoc cref="NUnit.Framework.Is.InRange(Object, Object)"/>
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void InRange<T>(T actual, T low, T high) where T : IComparable
#if NETCOREAPP || NETFRAMEWORK || UAP
		=> Assert.InRange(actual, low, high);
#else
		=> Assert.That(actual, Is.InRange(low, high));
#endif
#if NETCOREAPP || NETFRAMEWORK || UAP
	/// <inheritdoc cref="Assert.Contains{T}(T, IEnumerable{T})"/>
#else
	/// <inheritdoc cref="Assert.Contains(Object, System.Collections.ICollection)"/>
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void Contains<T>(T value, IEnumerable<T> collection)
#if NETCOREAPP || NETFRAMEWORK || UAP
		=> Assert.Contains(value, collection);
#else
		=> Assert.Contains(value, collection as ICollection ?? collection.ToArray());
#endif
#if NETCOREAPP || NETFRAMEWORK || UAP
	/// <inheritdoc cref="Assert.StrictEqual{T}(T, T)"/>
#else
	/// <inheritdoc cref="Assert.AreSame(Object, Object)"/>
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void StrictEqual<T>(T? expected, T? actual)
#if NETCOREAPP || NETFRAMEWORK || UAP
		=> Assert.StrictEqual(expected, actual);
#else
	{
		Assert.AreEqual(expected, actual);
		if (!typeof(T).IsValueType)
			Assert.AreSame(expected, actual);
	}
#endif
#if NETCOREAPP || NETFRAMEWORK || UAP
	/// <inheritdoc cref="Assert.All{T}(IEnumerable{T}, Action{T})"/>
#else
	/// <inheritdoc cref="Assert.That{T}(T, NUnit.Framework.Constraints.IResolveConstraint)"/>
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void All<T>(IEnumerable<T> collection, Action<T> action)
#if NETCOREAPP || NETFRAMEWORK || UAP
		=> Assert.All(collection, action);
#else
	{
		foreach (T item in collection)
			action(item);
	}
#endif
#if NETCOREAPP || NETFRAMEWORK || UAP
	/// <inheritdoc cref="Assert.NotEmpty(System.Collections.IEnumerable)"/>
#else
	/// <inheritdoc cref="Assert.IsNotEmpty(System.Collections.IEnumerable)"/>
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void NotEmpty(IEnumerable collection)
#if NETCOREAPP || NETFRAMEWORK || UAP
		=> Assert.NotEmpty(collection);
#else
		=> Assert.IsNotEmpty(collection);
#endif
}