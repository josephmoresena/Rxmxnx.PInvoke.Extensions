#if !NETSTANDARD2_1 && !NETCOREAPP3_0_OR_GREATER
using IEnumerator = System.Collections.IEnumerator;
using IEnumerable = System.Collections.IEnumerable;
#endif

namespace Rxmxnx.PInvoke;

public abstract partial class BufferTypeMetadata : IEnumerableSequence<BufferTypeMetadata>
{

#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	BufferTypeMetadata IEnumerableSequence<BufferTypeMetadata>.GetItem(Int32 index) => this[index];
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	Int32 IEnumerableSequence<BufferTypeMetadata>.GetSize() => this.ComponentCount;
#if PACKAGE && NETSTANDARD2_1
	IEnumerator<BufferTypeMetadata> IEnumerable<BufferTypeMetadata>.GetEnumerator() 
		=> IEnumerableSequence.CreateEnumerator(this);
	IEnumerator IEnumerable.GetEnumerator() => IEnumerableSequence.CreateEnumerator(this);
#elif !NETSTANDARD2_1 && !NETCOREAPP3_0_OR_GREATER
	IEnumerator<BufferTypeMetadata> IEnumerable<BufferTypeMetadata>.GetEnumerator() => this.CreateDefaultEnumerator();
	IEnumerator IEnumerable.GetEnumerator() => this.CreateDefaultEnumerator();
#endif
}