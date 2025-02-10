# Observable Timestamped Dictionary

This is a observable timestamped dictionary.

## Feature

- When an element is added or updated, a timestamp is recorded on the element.
- It is possible to set an upper limit on the number of elements that can be stored. (By default, it is unlimited)
- When adding elements, the oldest element is deleted if the capacity is exceeded.
- It is possible to monitor the addition, updating and deletion of elements.

## Documentation

|Class|Remarks|
|--|--|
|[ObservableTimestampedDictionary<TKey, TValue>](../Documentation/API/ObservableTimestampedDictionary-2/ObservableTimestampedDictionary.md)||
|ObservableTimestampedDictionary\<TValue>|Key type is string.|


## Usage

Generate various types of dictionaries.

~~~csharp
    using Izayoi.Collections;

    static void Main()
    {
        using var dictionary = new ObservableTimestampedDictionary<int, string>();

        using var observeClass = new ObserveClass(dictionary);

        dictionary.TryAdd(key: 1, value: "a");

        // fire OnAdd()

        // fire OnCountChange()
        // count: 1

        dictionary.TryAdd(key: 2, value: "b");

        // fire OnAdd()

        // fire OnCountChange()
        // count: 2

        dictionary.TryUpdate(key: 1, value: "c");

        // fire OnUpdate()

        // ev.OldValue.Timestamp: (1234567890)
        // ev.OldValue.Key: 1
        // ev.OldValue.Value: "a"

        // ev.NewValue.Timestamp: (1234567890)
        // ev.NewValue.Key: 1
        // ev.NewValue.Value: "c"

        dictionary.TryRemove(key: 1);

        // fire OnRemove()

        // fire OnCountChange()
        // count: 1

        dictionary.Clear();

        // fire OnClear()
        // removeCount: 1

        // fire OnCountChange()
        // count: 0

        observeClass.Dispose();

        dictionary.Dispose();
    }

    public class ObserveClass() : IDisposable
    {
        private IDisposable addObserver;
        private IDisposable removeObserver;
        private IDisposable updateObserver;
        private IDisposable countChangeObserver;
        private IDisposable clearObserver;

        public ObserveClass(ObservableTimestampedDictionary<int, string> dictionary)
        {
            addObserver = dictionary
                .ObserveAdd()
                .Subscribe(ev => OnAdd(ev));

            removeObserver = dictionary
                .ObserveRemove()
                .Subscribe(ev => OnRemove(ev));

            updateObserver = dictionary
                .ObserveUpdate()
                .Subscribe(ev => OnUpdate(ev));

            countChangeObserver = dictionary
                .ObserveCountChange()
                .Subscribe(count => OnCountChange(count));

            clearObserver = dictionary
                .ObserveClear()
                .Subscribe(removeCount => OnClear(removeCount));
        }

        public void Dispose()
        {
            addObserver.Dispose();
            removeObserver.Dispose();
            updateObserver.Dispose();
            countChangeObserver.Dispose();
            clearObserver.Dispose();
        }

        private void OnAdd(ObservableTimestampedDictionaryAddEvent<TKey, TValue> ev)
        {
            // ev.Timestamp
            // ev.Key
            // ev.Value
        }

        private void OnRemove(ObservableTimestampedDictionaryRemoveEvent<TKey, TValue> ev)
        {
            // ev.Timestamp
            // ev.Key
            // ev.Value
        }

        private void OnUpdate(ObservableTimestampedDictionaryUpdateEvent<TKey, TValue> ev)
        {
            // ev.OldValue.Timestamp
            // ev.OldValue.Key
            // ev.OldValue.Value

            // ev.NewValue.Timestamp
            // ev.NewValue.Key
            // ev.NewValue.Value
        }

        private void OnCountChange(int count)
        {
            //logger.Debug($"count: {count}");
        }

        private void OnClear(int removeCount)
        {
            //logger.Debug($"removeCount: {removeCount}");
        }
    }
~~~

___
Last updated: 11 February, 2025  
Editor: Izayoi Jiichan

*Copyright (C) 2025 Izayoi Jiichan. All Rights Reserved.*
