# ObservableTimestampedDictionary<TKey, TValue> Constructors

Initializes an instance of the [ObservableTimestampedDictionary<TKey, TValue>](ObservableTimestampedDictionary.md) class.

## ObservableTimestampedDictionary(in int, in IDictionary<TKey, TValue>?, in IEqualityComparer<TKey>?)

~~~csharp
public ObservableTimestampedDictionary(in int capacity = -1, in IDictionary<TKey, TValue>? dictionary = null, in IEqualityComparer<TKey>? comparer = null);
~~~

### Parameters

#### `capacity` int

The maximum number of key/value pairs that can be contained in the dicionary.  
`-1` is specified, or not specified (default), the capacity is unlimited.  
When adding pairs, the oldest pairs is deleted if the limit is exceeded.

#### `dictionary` IDictionary<TKey, TValue>?

The IDictionary<TKey,TValue> whose elements are copied to the new ObservableTimestampedDictionary<TKey,TValue>.

#### `comparer` IEqualityComparer<TKey>

The IEqualityComparer<T> implementation to use when comparing keys, or null to use the default EqualityComparer<T> for the type of the key.

### Exceptions

#### ArgumentOutOfRangeException

- `capacity` is zero, or `capacity` is less than -1.
- `dictionary` count is greater than `capacity`.
