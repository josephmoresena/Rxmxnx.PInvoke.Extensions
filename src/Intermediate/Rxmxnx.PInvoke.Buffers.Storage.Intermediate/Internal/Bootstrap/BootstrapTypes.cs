#if NET8_0_OR_GREATER
using O = System.Object;
using O1 = Rxmxnx.PInvoke.Buffers.Atomic<System.Object>;
using O2 =
	Rxmxnx.PInvoke.Buffers.Composite<Rxmxnx.PInvoke.Buffers.Atomic<System.Object>,
		Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, System.Object>;
using O4 =
	Rxmxnx.PInvoke.Buffers.Composite<
		Rxmxnx.PInvoke.Buffers.Composite<Rxmxnx.PInvoke.Buffers.Atomic<System.Object>,
			Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, System.Object>, Rxmxnx.PInvoke.Buffers.Composite<
			Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, System.Object>,
		System.Object>;
using O8 =
	Rxmxnx.PInvoke.Buffers.Composite<
		Rxmxnx.PInvoke.Buffers.Composite<
			Rxmxnx.PInvoke.Buffers.Composite<Rxmxnx.PInvoke.Buffers.Atomic<System.Object>,
				Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, System.Object>,
			Rxmxnx.PInvoke.Buffers.Composite<Rxmxnx.PInvoke.Buffers.Atomic<System.Object>,
				Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, System.Object>, System.Object>, Rxmxnx.PInvoke.Buffers.
		Composite<
			Rxmxnx.PInvoke.Buffers.Composite<Rxmxnx.PInvoke.Buffers.Atomic<System.Object>,
				Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, System.Object>,
			Rxmxnx.PInvoke.Buffers.Composite<Rxmxnx.PInvoke.Buffers.Atomic<System.Object>,
				Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, System.Object>, System.Object>, System.Object>;
using O16 =
	Rxmxnx.PInvoke.Buffers.Composite<
		Rxmxnx.PInvoke.Buffers.Composite<
			Rxmxnx.PInvoke.Buffers.Composite<
				Rxmxnx.PInvoke.Buffers.Composite<Rxmxnx.PInvoke.Buffers.Atomic<System.Object>,
					Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, System.Object>,
				Rxmxnx.PInvoke.Buffers.Composite<Rxmxnx.PInvoke.Buffers.Atomic<System.Object>,
					Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, System.Object>, System.Object>,
			Rxmxnx.PInvoke.Buffers.Composite<
				Rxmxnx.PInvoke.Buffers.Composite<Rxmxnx.PInvoke.Buffers.Atomic<System.Object>,
					Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, System.Object>,
				Rxmxnx.PInvoke.Buffers.Composite<Rxmxnx.PInvoke.Buffers.Atomic<System.Object>,
					Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, System.Object>, System.Object>, System.Object>, Rxmxnx
		.PInvoke.Buffers.Composite<
			Rxmxnx.PInvoke.Buffers.Composite<
				Rxmxnx.PInvoke.Buffers.Composite<Rxmxnx.PInvoke.Buffers.Atomic<System.Object>,
					Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, System.Object>,
				Rxmxnx.PInvoke.Buffers.Composite<Rxmxnx.PInvoke.Buffers.Atomic<System.Object>,
					Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, System.Object>, System.Object>, Rxmxnx.PInvoke.Buffers
			.Composite<
				Rxmxnx.PInvoke.Buffers.Composite<Rxmxnx.PInvoke.Buffers.Atomic<System.Object>,
					Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, System.Object>,
				Rxmxnx.PInvoke.Buffers.Composite<Rxmxnx.PInvoke.Buffers.Atomic<System.Object>,
					Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, System.Object>, System.Object>, System.Object>,
		System.Object>;
using O32 =
	Rxmxnx.PInvoke.Buffers.Composite<
		Rxmxnx.PInvoke.Buffers.Composite<
			Rxmxnx.PInvoke.Buffers.Composite<
				Rxmxnx.PInvoke.Buffers.Composite<
					Rxmxnx.PInvoke.Buffers.Composite<Rxmxnx.PInvoke.Buffers.Atomic<System.Object>,
						Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, System.Object>,
					Rxmxnx.PInvoke.Buffers.Composite<Rxmxnx.PInvoke.Buffers.Atomic<System.Object>,
						Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, System.Object>, System.Object>,
				Rxmxnx.PInvoke.Buffers.Composite<
					Rxmxnx.PInvoke.Buffers.Composite<Rxmxnx.PInvoke.Buffers.Atomic<System.Object>,
						Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, System.Object>,
					Rxmxnx.PInvoke.Buffers.Composite<Rxmxnx.PInvoke.Buffers.Atomic<System.Object>,
						Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, System.Object>, System.Object>, System.Object>,
			Rxmxnx.PInvoke.Buffers.Composite<
				Rxmxnx.PInvoke.Buffers.Composite<
					Rxmxnx.PInvoke.Buffers.Composite<Rxmxnx.PInvoke.Buffers.Atomic<System.Object>,
						Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, System.Object>,
					Rxmxnx.PInvoke.Buffers.Composite<Rxmxnx.PInvoke.Buffers.Atomic<System.Object>,
						Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, System.Object>, System.Object>,
				Rxmxnx.PInvoke.Buffers.Composite<
					Rxmxnx.PInvoke.Buffers.Composite<Rxmxnx.PInvoke.Buffers.Atomic<System.Object>,
						Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, System.Object>,
					Rxmxnx.PInvoke.Buffers.Composite<Rxmxnx.PInvoke.Buffers.Atomic<System.Object>,
						Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, System.Object>, System.Object>, System.Object>,
			System.Object>, Rxmxnx.PInvoke.Buffers.Composite<
			Rxmxnx.PInvoke.Buffers.Composite<
				Rxmxnx.PInvoke.Buffers.Composite<
					Rxmxnx.PInvoke.Buffers.Composite<Rxmxnx.PInvoke.Buffers.Atomic<System.Object>,
						Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, System.Object>,
					Rxmxnx.PInvoke.Buffers.Composite<Rxmxnx.PInvoke.Buffers.Atomic<System.Object>,
						Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, System.Object>, System.Object>,
				Rxmxnx.PInvoke.Buffers.Composite<
					Rxmxnx.PInvoke.Buffers.Composite<Rxmxnx.PInvoke.Buffers.Atomic<System.Object>,
						Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, System.Object>,
					Rxmxnx.PInvoke.Buffers.Composite<Rxmxnx.PInvoke.Buffers.Atomic<System.Object>,
						Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, System.Object>, System.Object>, System.Object>,
			Rxmxnx.PInvoke.Buffers.Composite<
				Rxmxnx.PInvoke.Buffers.Composite<
					Rxmxnx.PInvoke.Buffers.Composite<Rxmxnx.PInvoke.Buffers.Atomic<System.Object>,
						Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, System.Object>,
					Rxmxnx.PInvoke.Buffers.Composite<Rxmxnx.PInvoke.Buffers.Atomic<System.Object>,
						Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, System.Object>, System.Object>, Rxmxnx.PInvoke.
				Buffers.Composite<
					Rxmxnx.PInvoke.Buffers.Composite<Rxmxnx.PInvoke.Buffers.Atomic<System.Object>,
						Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, System.Object>,
					Rxmxnx.PInvoke.Buffers.Composite<Rxmxnx.PInvoke.Buffers.Atomic<System.Object>,
						Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, System.Object>, System.Object>, System.Object>,
			System.Object>, System.Object>;
using O64 =
	Rxmxnx.PInvoke.Buffers.Composite<
		Rxmxnx.PInvoke.Buffers.Composite<
			Rxmxnx.PInvoke.Buffers.Composite<
				Rxmxnx.PInvoke.Buffers.Composite<
					Rxmxnx.PInvoke.Buffers.Composite<
						Rxmxnx.PInvoke.Buffers.Composite<Rxmxnx.PInvoke.Buffers.Atomic<System.Object>,
							Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, System.Object>,
						Rxmxnx.PInvoke.Buffers.Composite<Rxmxnx.PInvoke.Buffers.Atomic<System.Object>,
							Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, System.Object>, System.Object>,
					Rxmxnx.PInvoke.Buffers.Composite<
						Rxmxnx.PInvoke.Buffers.Composite<Rxmxnx.PInvoke.Buffers.Atomic<System.Object>,
							Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, System.Object>,
						Rxmxnx.PInvoke.Buffers.Composite<Rxmxnx.PInvoke.Buffers.Atomic<System.Object>,
							Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, System.Object>, System.Object>, System.Object>
				,
				Rxmxnx.PInvoke.Buffers.Composite<
					Rxmxnx.PInvoke.Buffers.Composite<
						Rxmxnx.PInvoke.Buffers.Composite<Rxmxnx.PInvoke.Buffers.Atomic<System.Object>,
							Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, System.Object>,
						Rxmxnx.PInvoke.Buffers.Composite<Rxmxnx.PInvoke.Buffers.Atomic<System.Object>,
							Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, System.Object>, System.Object>,
					Rxmxnx.PInvoke.Buffers.Composite<
						Rxmxnx.PInvoke.Buffers.Composite<Rxmxnx.PInvoke.Buffers.Atomic<System.Object>,
							Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, System.Object>,
						Rxmxnx.PInvoke.Buffers.Composite<Rxmxnx.PInvoke.Buffers.Atomic<System.Object>,
							Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, System.Object>, System.Object>, System.Object>
				, System.Object>,
			Rxmxnx.PInvoke.Buffers.Composite<
				Rxmxnx.PInvoke.Buffers.Composite<
					Rxmxnx.PInvoke.Buffers.Composite<
						Rxmxnx.PInvoke.Buffers.Composite<Rxmxnx.PInvoke.Buffers.Atomic<System.Object>,
							Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, System.Object>,
						Rxmxnx.PInvoke.Buffers.Composite<Rxmxnx.PInvoke.Buffers.Atomic<System.Object>,
							Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, System.Object>, System.Object>,
					Rxmxnx.PInvoke.Buffers.Composite<
						Rxmxnx.PInvoke.Buffers.Composite<Rxmxnx.PInvoke.Buffers.Atomic<System.Object>,
							Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, System.Object>,
						Rxmxnx.PInvoke.Buffers.Composite<Rxmxnx.PInvoke.Buffers.Atomic<System.Object>,
							Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, System.Object>, System.Object>, System.Object>
				,
				Rxmxnx.PInvoke.Buffers.Composite<
					Rxmxnx.PInvoke.Buffers.Composite<
						Rxmxnx.PInvoke.Buffers.Composite<Rxmxnx.PInvoke.Buffers.Atomic<System.Object>,
							Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, System.Object>,
						Rxmxnx.PInvoke.Buffers.Composite<Rxmxnx.PInvoke.Buffers.Atomic<System.Object>,
							Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, System.Object>, System.Object>,
					Rxmxnx.PInvoke.Buffers.Composite<
						Rxmxnx.PInvoke.Buffers.Composite<Rxmxnx.PInvoke.Buffers.Atomic<System.Object>,
							Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, System.Object>,
						Rxmxnx.PInvoke.Buffers.Composite<Rxmxnx.PInvoke.Buffers.Atomic<System.Object>,
							Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, System.Object>, System.Object>, System.Object>
				, System.Object>, System.Object>, Rxmxnx.PInvoke.Buffers.Composite<
			Rxmxnx.PInvoke.Buffers.Composite<
				Rxmxnx.PInvoke.Buffers.Composite<
					Rxmxnx.PInvoke.Buffers.Composite<
						Rxmxnx.PInvoke.Buffers.Composite<Rxmxnx.PInvoke.Buffers.Atomic<System.Object>,
							Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, System.Object>,
						Rxmxnx.PInvoke.Buffers.Composite<Rxmxnx.PInvoke.Buffers.Atomic<System.Object>,
							Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, System.Object>, System.Object>,
					Rxmxnx.PInvoke.Buffers.Composite<
						Rxmxnx.PInvoke.Buffers.Composite<Rxmxnx.PInvoke.Buffers.Atomic<System.Object>,
							Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, System.Object>,
						Rxmxnx.PInvoke.Buffers.Composite<Rxmxnx.PInvoke.Buffers.Atomic<System.Object>,
							Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, System.Object>, System.Object>, System.Object>
				, Rxmxnx.PInvoke.Buffers.Composite<
					Rxmxnx.PInvoke.Buffers.Composite<
						Rxmxnx.PInvoke.Buffers.Composite<Rxmxnx.PInvoke.Buffers.Atomic<System.Object>,
							Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, System.Object>,
						Rxmxnx.PInvoke.Buffers.Composite<Rxmxnx.PInvoke.Buffers.Atomic<System.Object>,
							Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, System.Object>, System.Object>,
					Rxmxnx.PInvoke.Buffers.Composite<
						Rxmxnx.PInvoke.Buffers.Composite<Rxmxnx.PInvoke.Buffers.Atomic<System.Object>,
							Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, System.Object>,
						Rxmxnx.PInvoke.Buffers.Composite<Rxmxnx.PInvoke.Buffers.Atomic<System.Object>,
							Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, System.Object>, System.Object>, System.Object>
				, System.Object>, Rxmxnx.PInvoke.Buffers.Composite<
				Rxmxnx.PInvoke.Buffers.Composite<
					Rxmxnx.PInvoke.Buffers.Composite<
						Rxmxnx.PInvoke.Buffers.Composite<Rxmxnx.PInvoke.Buffers.Atomic<System.Object>,
							Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, System.Object>,
						Rxmxnx.PInvoke.Buffers.Composite<Rxmxnx.PInvoke.Buffers.Atomic<System.Object>,
							Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, System.Object>, System.Object>,
					Rxmxnx.PInvoke.Buffers.Composite<
						Rxmxnx.PInvoke.Buffers.Composite<Rxmxnx.PInvoke.Buffers.Atomic<System.Object>,
							Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, System.Object>,
						Rxmxnx.PInvoke.Buffers.Composite<Rxmxnx.PInvoke.Buffers.Atomic<System.Object>,
							Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, System.Object>, System.Object>, System.Object>
				, Rxmxnx.PInvoke.Buffers.Composite<
					Rxmxnx.PInvoke.Buffers.Composite<
						Rxmxnx.PInvoke.Buffers.Composite<Rxmxnx.PInvoke.Buffers.Atomic<System.Object>,
							Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, System.Object>,
						Rxmxnx.PInvoke.Buffers.Composite<Rxmxnx.PInvoke.Buffers.Atomic<System.Object>,
							Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, System.Object>, System.Object>, Rxmxnx.PInvoke
					.Buffers.Composite<
						Rxmxnx.PInvoke.Buffers.Composite<Rxmxnx.PInvoke.Buffers.Atomic<System.Object>,
							Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, System.Object>,
						Rxmxnx.PInvoke.Buffers.Composite<Rxmxnx.PInvoke.Buffers.Atomic<System.Object>,
							Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, System.Object>, System.Object>, System.Object>
				, System.Object>, System.Object>, System.Object>;

namespace Rxmxnx.PInvoke.Internal.Bootstrap;

/// <summary>
/// Internal store with 2^5-1 binary space.
/// </summary>
internal abstract class G31<T> : BootstrapMetadataStorage<Composite<
	Composite<Composite<Composite<O1, O2, O>, O4, O>, O8, O>, O16, O>, T>;

/// <summary>
/// Internal store with 2^7-1 binary space.
/// </summary>
internal abstract class G127<T> : BootstrapMetadataStorage<Composite<
	Composite<Composite<Composite<Composite<Composite<O1, O2, O>, O4, O>, O8, O>, O16, O>, O32, O>, O64, O>, T>;

/// <summary>
/// Internal store with 2^11-1 binary space.
/// </summary>
#if !PACKAGE
[ExcludeFromCodeCoverage]
#endif
internal abstract class G2047<T>(Boolean withSlots)
	: BootstrapMetadataStorage<Composite<
			Composite<Composite<
					Composite<Composite<Composite<
							Composite<Composite<Composite<Composite<O1, O2, O>, O4, O>, O8, O>, O16, O>, O32, O>, O64,
						O>,
						Composite<O64, O64, O>, O>, Composite<Composite<O64, O64, O>, Composite<O64, O64, O>, O>, O>,
				Composite<Composite<Composite<O64, O64, O>, Composite<O64, O64, O>, O>,
					Composite<Composite<O64, O64, O>, Composite<O64, O64, O>, O>, O>, O>, Composite<
				Composite<Composite<Composite<O64, O64, O>, Composite<O64, O64, O>, O>,
					Composite<Composite<O64, O64, O>, Composite<O64, O64, O>, O>, O>, Composite<
					Composite<Composite<O64, O64, O>, Composite<O64, O64, O>, O>,
					Composite<Composite<O64, O64, O>, Composite<O64, O64, O>, O>, O>, O>, O>, T>
#if !PACKAGE
		, IBinarySlotsOwner<T>
#endif
{
	/// <summary>
	/// Additional slots.
	/// </summary>
	public BufferTypeMetadata<T>?[]?[] Slots { get; } = withSlots ? new BufferTypeMetadata<T>?[]?[5] : [];

	/// <summary>
	/// Prepares the current instance for <paramref name="count"/>.
	/// </summary>
	/// <param name="count">Requested count.</param>
	public MetadataStorage<T> PrepareFor(UInt16 count)
	{
		if (count <= this.Capacity)
			return this; // Nothing to prepare.
		ValidationUtilities.ThrowIfInvalidSequenceIndex(
			count - 1, this.Slots.Length == 0 ? this.Capacity : UInt16.MaxValue);
		MetadataStorage<T>.InitializePages(count, this.Capacity + 1, ref this.Slots[0]);
		return this;
	}
}
#endif