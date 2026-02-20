// 
// Copyright (c) 2018 Microsoft
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
// 

using System;
using System.Buffers;
using System.Buffers.Binary;
using System.Buffers.Text;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

[assembly: CompilationRelaxations(8)]
[assembly: RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: TypeForwardedTo(typeof(MemoryExtensions))]
[assembly: TypeForwardedTo(typeof(Memory<>))]
[assembly: TypeForwardedTo(typeof(ReadOnlyMemory<>))]
[assembly: TypeForwardedTo(typeof(ReadOnlySpan<>))]
[assembly: TypeForwardedTo(typeof(SequencePosition))]
[assembly: TypeForwardedTo(typeof(Span<>))]
[assembly: TypeForwardedTo(typeof(BuffersExtensions))]
[assembly: TypeForwardedTo(typeof(IBufferWriter<>))]
[assembly: TypeForwardedTo(typeof(IMemoryOwner<>))]
[assembly: TypeForwardedTo(typeof(IPinnable))]
[assembly: TypeForwardedTo(typeof(MemoryManager<>))]
[assembly: TypeForwardedTo(typeof(MemoryHandle))]
[assembly: TypeForwardedTo(typeof(MemoryPool<>))]
[assembly: TypeForwardedTo(typeof(OperationStatus))]
[assembly: TypeForwardedTo(typeof(ReadOnlySequenceSegment<>))]
[assembly: TypeForwardedTo(typeof(ReadOnlySequence<>))]
[assembly: TypeForwardedTo(typeof(StandardFormat))]
[assembly: TypeForwardedTo(typeof(BinaryPrimitives))]
[assembly: TypeForwardedTo(typeof(Base64))]
[assembly: TypeForwardedTo(typeof(Utf8Formatter))]
[assembly: TypeForwardedTo(typeof(Utf8Parser))]
[assembly: TypeForwardedTo(typeof(MemoryMarshal))]
[assembly: TypeForwardedTo(typeof(SequenceMarshal))]