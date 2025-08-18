namespace Rxmxnx.PInvoke.Tests.ValueRegionTests;

[ExcludeFromCodeCoverage]
public abstract class ValueRegionTestBase
{
	/// <summary>
	/// Fixture instance.
	/// </summary>
	protected static readonly IFixture Fixture = new Fixture();

	/// <summary>
	/// Creates a new <see cref="ValueRegion{T}"/> instance from <paramref name="array"/>.
	/// </summary>
	/// <typeparam name="T">Unmanaged type of the items in the sequence.</typeparam>
	/// <param name="array">A <typeparamref name="T"/> array.</param>
	/// <param name="handles">Collection in which to apend used handle.</param>
	/// <returns>A new <see cref="ValueRegion{T}"/> instance from <paramref name="array"/>.</returns>
	protected static ValueRegion<T> Create<T>(T[] array, ICollection<GCHandle> handles)
		=> ValueRegionTestBase.Create(array, handles, out _);

	/// <summary>
	/// Creates a new <see cref="ValueRegion{T}"/> instance from <paramref name="array"/>.
	/// </summary>
	/// <typeparam name="T">Unmanaged type of the items in the sequence.</typeparam>
	/// <param name="array">A <typeparamref name="T"/> array.</param>
	/// <param name="handles">Collection in which to apend used handle.</param>
	/// <param name="isReference">Indicates whether the created region is a reference.</param>
	/// <returns>A new <see cref="ValueRegion{T}"/> instance from <paramref name="array"/>.</returns>
	protected static unsafe ValueRegion<T> Create<T>(T[] array, ICollection<GCHandle> handles, out Boolean isReference)
	{
		isReference = false;
		switch (Random.Shared.Next(default, 12))
		{
			case 0:
			case 1:
				return ValueRegion<T>.Create(array);
			case 2:
			case 4:
				return ValueRegion<T>.Create(() => array);
			case 3:
			case 6:
				return ValueRegion<T>.Create(array, a => a.AsSpan());
			case 7:
			case 10:
				return ValueRegion<T>.Create(array, a => a.AsSpan(), (o, t) =>
				{
#if !NETCOREAPP
					if (t == GCHandleType.Pinned && RuntimeHelpers.IsReferenceOrContainsReferences<T>())
						throw new ArgumentException(); // Required for Mono?
#endif
					return GCHandle.Alloc(o, t);
				});
			default:
#if !NETCOREAPP
				if (RuntimeHelpers.IsReferenceOrContainsReferences<T>())
					throw new ArgumentException(); // Required for Mono?
#endif
#pragma warning disable CS8500
				handles.Add(GCHandle.Alloc(array, GCHandleType.Pinned));
				isReference = true;
				fixed (void* ptr = array)
					return ValueRegion<T>.Create(new(ptr), array.Length);
#pragma warning restore CS8500
		}
	}
#if !NET6_0_OR_GREATER
	private static class Random
	{
		public static readonly System.Random Shared = new();
	}
#endif
}