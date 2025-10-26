using System.Runtime.CompilerServices;

namespace Rxmxnx.PInvoke.ApplicationTest;

public static class RefStructHelper
{
	public static void PointerFeature<T>(Span<T> span, Boolean inStack)
	{
		ref Span<T> refSpan = ref span;
		ValPtr<Span<T>> valRefSpan = NativeUtilities.GetUnsafeValPtrFromRef(ref span);

		Console.WriteLine($"Unsafe Span Address: 0x{span.GetUnsafeIntPtr():x8}");
		NativeUtilities.WithSafeFixed(ref refSpan, inStack, RefStructHelper.UseFixedRefSpan);
		Console.WriteLine($"Span Pointer: 0x{valRefSpan.Pointer:x8}");
		Console.WriteLine($"Ref Span vs Span Pointer: {Unsafe.AreSame(ref refSpan, ref valRefSpan.Reference)}");
	}
	public static void PointerFeature<T>(ReadOnlySpan<T> span, Boolean inStack)
	{
		ref ReadOnlySpan<T> refSpan = ref span;
		ValPtr<ReadOnlySpan<T>> valRefSpan = NativeUtilities.GetUnsafeValPtrFromRef(ref span);

		Console.WriteLine($"Unsafe Read-only Span Address: 0x{span.GetUnsafeIntPtr():x8}");
		NativeUtilities.WithSafeFixed(ref refSpan, inStack, RefStructHelper.UseFixedRefSpan);
		Console.WriteLine($"Read-only Span Pointer: 0x{valRefSpan.Pointer:x8}");
		Console.WriteLine(
			$"Ref Read-only Span vs Read-only Span Pointer: {Unsafe.AreSame(ref refSpan, ref valRefSpan.Reference)}");
	}

	private static void UseFixedRefSpan<T>(in IFixedReference<Span<T>> frs, Boolean inStack)
	{
		ReadOnlyValPtr<Span<T>> spanPtr = NativeUtilities.GetUnsafeValPtr(in frs.Reference);

		Console.WriteLine($"Span Reference Length: {frs.Reference.Length}");
		Console.WriteLine($"Unsafe Span Read-only Pointer: 0x{spanPtr.Pointer:x8}");
		Console.WriteLine(
			$"Span Fixed Reference vs Span Read-only Pointer: {Unsafe.AreSame(ref frs.Reference, in spanPtr.Reference)}");
		if (!inStack) return;

		ValPtr<T> valPtr = spanPtr.Reference.GetUnsafeValPtr();
		using IFixedContext<T>.IDisposable f = valPtr.GetUnsafeFixedContext(spanPtr.Reference.Length);
		Console.WriteLine($"Unsafe Span Memory Pointer: 0x{f.Pointer:x8}");
	}
	private static void UseFixedRefSpan<T>(in IFixedReference<ReadOnlySpan<T>> frs, Boolean inStack)
	{
		ReadOnlyValPtr<ReadOnlySpan<T>> spanPtr = NativeUtilities.GetUnsafeValPtr(in frs.Reference);

		Console.WriteLine($"Span Reference Length: {frs.Reference.Length}");
		Console.WriteLine($"Unsafe Read-only Span Read-only Pointer: 0x{spanPtr.Pointer:x8}");
		Console.WriteLine(
			$"Read-only Span Fixed Reference vs Read-only Span Read-only Pointer: {Unsafe.AreSame(ref frs.Reference, in spanPtr.Reference)}");
		if (!inStack) return;

		ReadOnlyValPtr<T> valPtr = spanPtr.Reference.GetUnsafeValPtr();
		using IReadOnlyFixedContext<T>.IDisposable f = valPtr.GetUnsafeFixedContext(spanPtr.Reference.Length);
		Console.WriteLine($"Unsafe Read-only Span Memory Pointer: 0x{f.Pointer:x8}");
	}
}