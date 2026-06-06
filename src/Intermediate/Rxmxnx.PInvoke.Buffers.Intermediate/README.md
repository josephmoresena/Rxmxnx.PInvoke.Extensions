`Rxmxnx.PInvoke.Extensions` supports the use of two types of buffers: binary and non-binary. The maximum capacity of any
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

## Binary buffer Preparation

Binary buffers can be statically prepared for a given count number elements. This method requires the use of reflection
and relies on the auto-composition feature.

## Buffer metadata storage

Until version 2.9.0 of `Rxmxnx.PInvoke.Extensions`, there was only a single mechanism for storing managed buffer
metadata.

After several optimizations were introduced, a new storage system was implemented for .NET 8.0 and later that leverages
the managed buffer infrastructure itself. However, to provide alternatives better suited to different application
requirements, the following feature switches were introduced:

* `PInvoke.MaxBufferCapacity31`: Restricts buffer storage and composition to a maximum binary capacity of 31 elements,
  with a binary space limit of 2<sup>5</sup> + 2<sup>4</sup> + 2<sup>3</sup> + 2<sup>2</sup> + 2<sup>1</sup> + 2<sup>
  0</sup>.
* `PInvoke.MaxBufferCapacity127`: Restricts buffer storage and composition to a maximum binary capacity of 127 elements,
  with a binary space limit of 2<sup>6</sup> + 2<sup>5</sup> + 2<sup>4</sup> + 2<sup>3</sup> + 2<sup>2</sup> + 2<sup>
  1</sup> + 2<sup>0</sup>.
* `PInvoke.MaxBufferCapacity2047`: Restricts buffer storage and composition to a maximum binary capacity of 2047
  elements, with a binary space limit of 2<sup>10</sup> + 2<sup>9</sup> + 2<sup>8</sup> + 2<sup>7</sup> + 2<sup>
  6</sup> + 2<sup>5</sup> + 2<sup>4</sup> + 2<sup>3</sup> + 2<sup>2</sup> + 2<sup>1</sup> + 2<sup>0</sup>.
* `PInvoke.ClassicBufferStorage`: Uses the storage mechanism implemented for assemblies targeting earlier framework
  versions. In other words, a storage system that is not based on the managed buffer infrastructure itself.

**Notes:**

- By default, on .NET 8.0 and later, the storage system uses a managed-buffer-based binary space of 2047 elements, while
  still allowing extension to support larger binary metadata capacities.
- Storage systems based on managed buffer binary spaces of 2<sup>N</sup> - 1 elements preload `2N - 1` buffer metadata
  instances; specifically, metadata ranging from 2<sup>N</sup> - 1 down to 2<sup>2</sup> - 1, plus additional metadata
  from 2<sup>N-1</sup> down to 2<sup>0</sup>. This behavior is particularly beneficial for AOT compilations.