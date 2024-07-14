# TimestampedDictionary<TKey, TValue> TryUpdate

This is one of the methods of the [TimestampedDictionary<TKey, TValue>](TimestampedDictionary.md) class.

## TryUpdate(in TKey key, in TValue? newValue)

Updates the value associated with key to newValue.

~~~csharp
public bool TryUpdate(in TKey key, in TValue? newValue);
~~~

### Parameters

#### `key` TKey

The key of the element to update.

#### `newValue` TValue?

The value that replaces the value of the element that has the specified key.

### Returns

Boolean

true if the key/value pair was replaced to the dictionary successfully; otherwise, false.

### Examples

~~~csharp
    using Izayoi.Collections;

    static void Main()
    {
        var dictionary = new TimestampedDictionary<int, string>();

        bool addResult = dictionary.TryAdd(key: 1, value: "a");

        // addResult: true

        bool updateResult = dictionary.TryUpdate(key: 1, newValue: "b");

        // updateResult: true

        bool getResult = dictionary.TryGetValue(key: 1, out var getValue);

        // getResult: true, getValue: "b"
    }
~~~

## TryUpdate(in TKey key, in TValue? newValue, in TValue? comparisonValue)

Updates the value associated with key to newValue.

~~~csharp
public bool TryUpdate(in TKey key, in TValue? newValue, in TValue? comparisonValue);
~~~

### Parameters

#### `key` TKey

The key of the element to update.

#### `newValue` TValue?

The value that replaces the value of the element that has the specified key.

#### `comparisonValue` TValue?

The value that is compared with the value of the element that has the specified key.

### Returns

Boolean

`true` if the key/value pair was replaced to the dictionary successfully; otherwise, `false`.

### Examples

~~~csharp
    using Izayoi.Collections;

    static void Main()
    {
        var dictionary = new TimestampedDictionary<int, string>();

        bool addResult = dictionary.TryAdd(key: 1, value: "a");

        // addResult: true

        bool updateResult1 = dictionary.TryUpdate(key: 1, newValue: "b", comparisonValue: "A");

        // updateResult1: false

        bool updateResult2 = dictionary.TryUpdate(key: 1, newValue: "b", comparisonValue: "a");

        // updateResult2: true

        bool getResult = dictionary.TryGetValue(key: 1, out var getValue);

        // getResult: true, getValue: "b"
    }
~~~
