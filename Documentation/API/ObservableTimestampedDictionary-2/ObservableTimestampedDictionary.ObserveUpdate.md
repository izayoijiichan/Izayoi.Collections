# ObservableTimestampedDictionary<TKey, TValue> ObserveUpdate

This is one of the methods of the [ObservableTimestampedDictionary<TKey, TValue>](ObservableTimestampedDictionary.md) class.

## IObservable<ObservableTimestampedDictionaryUpdateEvent<TKey, TValue>> ObserveUpdate()

Makes it observable when an element is updated to a dictionary.

~~~csharp
public IObservable<ObservableTimestampedDictionaryUpdateEvent<TKey, TValue>> ObserveUpdate();
~~~

### Returns

`IObservable<ObservableTimestampedDictionaryUpdateEvent<TKey, TValue>>`

Returns an object that allows you to observe element updates to the dictionary.

### Examples

~~~csharp
    using Izayoi.Collections;

    static void Main()
    {
        using var dictionary = new ObservableTimestampedDictionary<int, string>();

        using var observeClass = new ObserveClass(dictionary);

        dictionary.TryAdd(key: 1, value: "a");

        dictionary.TryUpdate(key: 1, value: "b");

        // fire OnUpdate()

        // ev.OldValue.Timestamp: (1234567890)
        // ev.OldValue.Key: 1
        // ev.OldValue.Value: "a"

        // ev.NewValue.Timestamp: (1234567890)
        // ev.NewValue.Key: 1
        // ev.NewValue.Value: "b"

        observeClass.Dispose();

        dictionary.Dispose();
    }

    public class ObserveClass() : IDisposable
    {
        private IDisposable updateObserver;

        public ObserveClass(ObservableTimestampedDictionary<int, string> dictionary)
        {
            updateObserver = dictionary
                .ObserveUpdate()
                .Subscribe(ev => OnUpdate(ev));
        }

        public void Dispose()
        {
            updateObserver.Dispose();
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
    }
~~~
