# TimestampedDictionary<TKey, TValue> ClearBefore

This is one of the methods of the [TimestampedDictionary<TKey, TValue>](TimestampedDictionary.md) class.

## ClearBefore(in DateTimeOffset datetime)

Delete all keys and values before the specified datetime.

~~~csharp
public int ClearBefore(in DateTimeOffset datetime);
~~~

### Parameters

#### `datetime` DateTimeOffset

Base datetime of deletion.

### Returns

Int32

Count of deleted data.

### Remarks

Does not include the datetime specified.

### Examples

~~~csharp
    using Izayoi.Collections;

    static async Task Main()
    {
        var cache = new TimestampedDictionary<int, string>();

        _ = cache.TryAdd(key: 1, value: "a");
        _ = cache.TryAdd(key: 2, value: "b");

        await Task.Delay(1000);

        DateTimeOffset datetime = DateTimeOffset.UtcNow;

        await Task.Delay(1000);

        _ = cache.TryAdd(key: 3, value: "c");

        await Task.Delay(1000);

        cache.ClearBefore(datetime);

        bool result1 = cache.TryGetValue(key: 1, out var value1);

        // result1: false, value1: null

        bool result2 = cache.TryGetValue(key: 2, out var value2);

        // result2: false, value2: null

        bool result3 = cache.TryGetValue(key: 3, out var value3);

        // result3: true, value3: "c"
    }
~~~

## ClearBefore(in long unixTimeMilliseconds)

Delete all keys and values before the specified unixTimeMilliseconds.

~~~csharp
public int ClearBefore(in long unixTimeMilliseconds);
~~~

### Parameters

#### `unixTimeMilliseconds` Uint64

Base unixtime of deletion.

### Returns

Int32

Count of deleted data.

### Remarks

Does not include the datetime specified.

### Examples

~~~csharp
    using Izayoi.Collections;

    static async Task Main()
    {
        var cache = new TimestampedDictionary<int, string>();

        _ = cache.TryAdd(key: 1, value: "a");
        _ = cache.TryAdd(key: 2, value: "b");

        await Task.Delay(1000);

        long unixTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

        await Task.Delay(1000);

        _ = cache.TryAdd(key: 3, value: "c");

        await Task.Delay(1000);

        cache.ClearBefore(unixTime);

        bool result1 = cache.TryGetValue(key: 1, out var value1);

        // result1: false, value1: null

        bool result2 = cache.TryGetValue(key: 2, out var value2);

        // result2: false, value2: null

        bool result3 = cache.TryGetValue(key: 3, out var value3);

        // result3: true, value3: "c"
    }
~~~
