# ObservableTimestampedDictionary<TKey, TValue> ObserveClear

This is one of the methods of the [ObservableTimestampedDictionary<TKey, TValue>](ObservableTimestampedDictionary.md) class.

## IObservable<int> ObserveClear()

Makes dictionary clearing observable.

~~~csharp
public IObservable<int> ObserveClear();
~~~

### Returns

`IObservable<int>`

Returns the object that monitors the clearing of the dictionary.

### Examples

~~~csharp
    using Izayoi.Collections;

    static void Main()
    {
        using var dictionary = new ObservableTimestampedDictionary<int, string>();

        using var observeClass = new ObserveClass(dictionary);

        dictionary.TryAdd(key: 1, value: "a");

        dictionary.Clear();

        // fire OnClear()

        // removeCount: 1

        observeClass.Dispose();

        dictionary.Dispose();
    }

    public class ObserveClass() : IDisposable
    {
        private IDisposable clearObserver;

        public ObserveClass(ObservableTimestampedDictionary<int, string> dictionary)
        {
            clearObserver = dictionary
                .ObserveClear()
                .Subscribe(removeCount => OnClear(removeCount));
        }

        public void Dispose()
        {
            clearObserver.Dispose();
        }

        private void OnClear(int removeCount)
        {
            //logger.Debug($"removeCount: {removeCount}");
        }
    }
~~~
