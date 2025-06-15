﻿`Rxmxnx.PInvoke.Extensions` supports the use of two types of buffers: binary and non-binary. The maximum capacity of any
buffer is limited to 2<sup>15</sup> elements. However, this maximum capacity may not always be allocatable at runtime.

Internally, all reference types utilize buffers of type `Object`. Only not unmanaged value types require
the use of buffers specific to their type, the unmanaged ones uses stackalloc.

---

## Binary Buffers

Binary buffers are implemented with the structures `Atomic<T>` and `Composite<TBufferA, TBufferB, T>`.

A buffer of type `Composite<TBufferA, TBufferB, T>` is considered binary **if and only if** both `TBufferA` and
`TBufferB` are binary buffers, and the capacity of `TBufferA` is less than or equal to that of `TBufferB`.

### Auto-Composition

Binary buffers can automatically combine to form buffers with higher capacities. This auto-composition occurs under the
following conditions:

- The feature switch `PInvoke.DisableBufferAutoComposition` is disabled.
- The runtime environment is JIT, or if it is AOT, metadata for the composed binary buffer is preserved and reflection
  is accessible at runtime.

#### Native AOT

In a Native AOT runtime, binary buffer composition requires metadata preservation through a Runtime Directives file.
Below is an example of the metadata preservation needed to compose a binary buffer with a capacity of 10 elements of any
reference type Composite(2<sup>1</sup>, 2<sup>3</sup>, `Object`).

**Notes**:

* 2<sup>3</sup> is Composite(2<sup>2</sup>, 2<sup>2</sup>, `Object`), 2<sup>2</sup> is Composite(2<sup>1</sup>, 2<sup>
  1</sup>, `Object`), 2<sup>1</sup> is
  Composite(2<sup>0</sup>, 2<sup>0</sup>, `Object`) and 2<sup>0</sup> is Atomic(`Object`).
* Once a buffer is composed, it becomes available for use. This process is executed only once for each capacity.

```xml

<Directives xmlns="http://schemas.microsoft.com/netfx/2013/01/metadata">
    <Application>
        <Assembly Name="Rxmxnx.PInvoke.Extensions">
            <Type Name="
            Rxmxnx.PInvoke.Buffers.Composite`3[
                Rxmxnx.PInvoke.Buffers.Composite`3[
                    Rxmxnx.PInvoke.Buffers.Atomic`1[System.Object], 
                    Rxmxnx.PInvoke.Buffers.Atomic`1[System.Object], 
                    System.Object],
                Rxmxnx.PInvoke.Buffers.Composite`3[
                    Rxmxnx.PInvoke.Buffers.Composite`3[
                        Rxmxnx.PInvoke.Buffers.Composite`3[
                            Rxmxnx.PInvoke.Buffers.Atomic`1[System.Object], 
                            Rxmxnx.PInvoke.Buffers.Atomic`1[System.Object], 
                            System.Object], 
                        Rxmxnx.PInvoke.Buffers.Composite`3[
                            Rxmxnx.PInvoke.Buffers.Atomic`1[System.Object], 
                            Rxmxnx.PInvoke.Buffers.Atomic`1[System.Object], 
                            System.Object], 
                        System.Object], 
                    Rxmxnx.PInvoke.Buffers.Composite`3[
                        Rxmxnx.PInvoke.Buffers.Composite`3[
                            Rxmxnx.PInvoke.Buffers.Atomic`1[System.Object], 
                            Rxmxnx.PInvoke.Buffers.Atomic`1[System.Object], 
                            System.Object], 
                        Rxmxnx.PInvoke.Buffers.Composite`3[
                            Rxmxnx.PInvoke.Buffers.Atomic`1[System.Object], 
                            Rxmxnx.PInvoke.Buffers.Atomic`1[System.Object], 
                            System.Object], 
                        System.Object], 
                    System.Object], 
                System.Object]" Dynamic="Required All"/>
        </Assembly>
    </Application>
</Directives>
```

### Preparation

Binary buffers can be statically prepared to allocate a specific number of elements using auto-composition to cache the
buffer metadata with the required size.

**Note:** It is ideal for scenarios where buffer allocation needs to occur as transparently as possible, without
requiring additional allocations.

---

## Non-Binary Buffers

Non-binary buffers are implemented with the structure `NonBinarySpace<TArray, T>`, where `TArray` is the structure
capable of storing elements of type `T`.

---

## Buffer Registration

Buffers can be statically registered to avoid runtime auto-composition, which may involve additional allocations and
reflection during buffer allocation.

There are three buffer registration options:

1. For `Object` type.
2. For generic `struct` type.
3. For generic nullable `struct` type.

**Note:** In .NET 5.0 and earlier, reflection is used by these methods due to the lack of support
for [static virtual members in interfaces](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/interface#static-abstract-and-virtual-members).

## Binary buffer Preparation

Binary buffers can be statically prepared for a given count number elements. This method requires the use of reflection
and relies on the auto-composition feature.