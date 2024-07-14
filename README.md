# Timestamped Dictionary

This is a dictionary that records datetime when data is added.

## Feature

- When an element is added or updated, a timestamp is recorded on the element.
- It is possible to set an upper limit on the number of elements that can be stored. (By default, it is unlimited)
- When adding elements, the oldest element is deleted if the capacity is exceeded.

## Documentation

|Class|Remarks|
|--|--|
|[TimestampedDictionary<TKey, TValue>](Documentation~/API/TimestampedDictionary-2/TimestampedDictionary.md)||
|TimestampedDictionary\<TValue>|Key type is string.|

## Usage

Generate various types of dictionaries.

~~~csharp
    using Izayoi.Collections;
    using System;

    static void Main()
    {
        // key: int, value: long
        var intKeyDicionary = new TimestampedDictionary<int, long>();

        // key: Guid, value: User
        var userDicionary = new TimestampedDictionary<Guid, User>();

        // key: string, value: int
        var stringKeyDicionary = new TimestampedDictionary<int>();
    }
~~~

See timestamp.

~~~csharp
    using Izayoi.Collections;

    static void Main()
    {
        var dictionary = new TimestampedDictionary<int, string>();

        bool addResult = dictionary.TryAdd(key: 1, value: "a");

        // addResult: true

        bool getResult = dictionary.TryGetValue(key: 1, out var value);

        // getResult: true

        // value: "a"

        bool getDataResult = dictionary.TryGetData(key: 1, out var data);

        // getDataResult: true

        // data.Timestamp: 1234567890, data.Key: 1, data.Value: "a"

        var keys = dictionary.GetTimestampedKeys();

        // keys[0].Timestamp: 1234567890, keys[0].Key: 1
    }
~~~

Delete old data.

~~~csharp
    using Izayoi.Collections;
    using System.Threading.Tasks;

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

        // delete old data
        cache.ClearBefore(datetime);

        bool result1 = cache.TryGetValue(key: 1, out var value1);

        // result1: false, value1: null

        bool result2 = cache.TryGetValue(key: 2, out var value2);

        // result2: false, value2: null

        bool result3 = cache.TryGetValue(key: 3, out var value3);

        // result3: true, value3: "c"
    }
~~~

Enable capacity.

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

        // contains: 1, 2
        containsKey1 = dictionary.ContainsKey(key: 1);

        // containsKey1: true

        bool addResult3 = dictionary.TryAdd(key: 3, value: "c");

        // addResult3: true

        // contains: 2, 3
        containsKey1 = dictionary.ContainsKey(key: 1);

        // containsKey1: false

        bool addResult4 = dictionary.TryAdd(key: 4, value: "d");

        // addResult4: true

        // contains: 3, 4
        bool containsKey2 = dictionary.ContainsKey(key: 2);

        // containsKey2: false
    }
~~~

___
Last updated: 15 July, 2024  
Editor: Izayoi Jiichan

*Copyright (C) 2024 Izayoi Jiichan. All Rights Reserved.*
