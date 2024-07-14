# TimestampedDictionary<TKey, TValue> Constructors

Initializes an instance of the [TimestampedDictionary<TKey, TValue>](TimestampedDictionary.md) class.

## TimestampedDictionary(in int, in IDictionary<TKey, TValue>?, in IEqualityComparer<TKey>?)

~~~csharp
public TimestampedDictionary(in int capacity = -1, in IDictionary<TKey, TValue>? dictionary = null, in IEqualityComparer<TKey>? comparer = null);
~~~

### Parameters

#### `capacity` int

The maximum number of key/value pairs that can be contained in the dicionary.  
`-1` is specified, or not specified (default), the capacity is unlimited.  
When adding pairs, the oldest pairs is deleted if the limit is exceeded.

#### `dictionary` IDictionary<TKey, TValue>?

The IDictionary<TKey,TValue> whose elements are copied to the new TimestampedDictionary<TKey,TValue>.

#### `comparer` IEqualityComparer<TKey>

The IEqualityComparer<T> implementation to use when comparing keys, or null to use the default EqualityComparer<T> for the type of the key.

### Exceptions

#### ArgumentOutOfRangeException

- `capacity` is zero, or `capacity` is less than -1.
- `dictionary` count is greater than `capacity`.
