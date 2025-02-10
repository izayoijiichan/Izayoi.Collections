# TimestampedDictionary<TKey, TValue> TryAdd

This is one of the methods of the [TimestampedDictionary<TKey, TValue>](TimestampedDictionary.md) class.

## TryAdd(in TKey key, in TValue? value)

Attempts to add the specified key and value to the dictionary.

~~~csharp
public bool TryAdd(in TKey key, in TValue? value);
~~~

### Parameters

#### `key` TKey

The key of the element to add.

#### `value` TValue?

The value of the element to add. It can be null.

### Returns

Boolean

`true` if the key/value pair was added to the dictionary successfully; otherwise, `false`.

### Remarks

When adding pairs, the oldest pairs is deleted if the limit is exceeded.  
If `capacity` is -1, pairs can be added without restriction.

### Examples

~~~csharp
    using Izayoi.Collections;

    static void Main()
    {
        var dictionary = new TimestampedDictionary<int, string>(capacity: 2);

        bool containsKey1;

        bool addResult1 = dictionary.TryAdd(key: 1, value: "a");

        // addResult1: true

        bool addResult2 = dictionary.TryAdd(key: 2, value: "b");

        // addResult1: true

        // contains; 1, 2
        containsKey1 = dictionary.ContainsKey(key: 1);

        // containsKey1: true

        bool addResult3 = dictionary.TryAdd(key: 3, value: "c");

        // addResult3: true

        // contains; 2, 3
        containsKey1 = dictionary.ContainsKey(key: 1);

        // containsKey1: false

        bool addResult4 = dictionary.TryAdd(key: 4, value: "d");

        // addResult4: true

        // contains: 3, 4
        bool containsKey2 = dictionary.ContainsKey(key: 2);

        // containsKey2: false
    }
~~~
