# ObservableTimestampedDictionary<TKey, TValue> ObserveAdd

This is one of the methods of the [ObservableTimestampedDictionary<TKey, TValue>](ObservableTimestampedDictionary.md) class.

## IObservable<ObservableTimestampedDictionaryAddEvent<TKey, TValue>> ObserveAdd()

Makes it observable when an element is added to a dictionary.

~~~csharp
public IObservable<ObservableTimestampedDictionaryAddEvent<TKey, TValue>> ObserveAdd();
~~~

### Returns

`IObservable<ObservableTimestampedDictionaryAddEvent<TKey, TValue>>`

Returns an object that allows you to observe the addition of elements to the dictionary.

### Examples

~~~csharp
    using Izayoi.Collections;

    static void Main()
    {
        using var dictionary = new ObservableTimestampedDictionary<int, string>();

        using var observeClass = new ObserveClass(dictionary);

        dictionary.TryAdd(key: 1, value: "a");

        // fire OnAdd()

        observeClass.Dispose();

        dictionary.Dispose();
    }

    public class ObserveClass() : IDisposable
    {
        private IDisposable addObserver;

        public ObserveClass(ObservableTimestampedDictionary<int, string> dictionary)
        {
            addObserver = dictionary
                .ObserveAdd()
                .Subscribe(ev => OnAdd(ev));
        }

        public void Dispose()
        {
            addObserver.Dispose();
        }

        private void OnAdd(ObservableTimestampedDictionaryAddEvent<TKey, TValue> ev)
        {
            // ev.Timestamp
            // ev.Key
            // ev.Value
        }
    }
~~~
