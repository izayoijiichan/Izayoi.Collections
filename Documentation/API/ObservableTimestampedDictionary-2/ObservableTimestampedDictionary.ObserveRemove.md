# ObservableTimestampedDictionary<TKey, TValue> ObserveRemove

This is one of the methods of the [ObservableTimestampedDictionary<TKey, TValue>](ObservableTimestampedDictionary.md) class.

## IObservable<ObservableTimestampedDictionaryRemoveEvent<TKey, TValue>> ObserveRemove()

Makes it observable when an element is Removed to a dictionary.

~~~csharp
public IObservable<ObservableTimestampedDictionaryRemoveEvent<TKey, TValue>> ObserveRemove();
~~~

### Returns

`IObservable<ObservableTimestampedDictionaryRemoveEvent<TKey, TValue>>`

Returns an object that allows you to observe the removal of elements from the dictionary.

### Examples

~~~csharp
    using Izayoi.Collections;

    static void Main()
    {
        using var dictionary = new ObservableTimestampedDictionary<int, string>();

        using var observeClass = new ObserveClass(dictionary);

        dictionary.TryAdd(key: 1, value: "a");

        dictionary.TryRemove(key: 1);

        // fire OnRemove()

        observeClass.Dispose();

        dictionary.Dispose();
    }

    public class ObserveClass() : IDisposable
    {
        private IDisposable removeObserver;

        public ObserveClass(ObservableTimestampedDictionary<int, string> dictionary)
        {
            removeObserver = dictionary
                .ObserveRemove()
                .Subscribe(ev => OnRemove(ev));
        }

        public void Dispose()
        {
            removeObserver.Dispose();
        }

        private void OnRemove(ObservableTimestampedDictionaryRemoveEvent<TKey, TValue> ev)
        {
            // ev.Timestamp
            // ev.Key
            // ev.Value
        }
    }
~~~
