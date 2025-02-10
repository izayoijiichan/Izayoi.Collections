# ObservableTimestampedDictionary<TKey, TValue> ObserveCountChange

This is one of the methods of the [ObservableTimestampedDictionary<TKey, TValue>](ObservableTimestampedDictionary.md) class.

## IObservable<int> ObserveCountChange()

Allows a dictionary to be observed for changes to the count of elements.

~~~csharp
public IObservable<int> ObserveCountChange();
~~~

### Returns

`IObservable<int>`

Returns an object that monitors changes to the number of elements in the dictionary.

### Examples

~~~csharp
    using Izayoi.Collections;

    static void Main()
    {
        using var dictionary = new ObservableTimestampedDictionary<int, string>();

        using var observeClass = new ObserveClass(dictionary);

        dictionary.TryAdd(key: 1, value: "a");

        // fire OnCountChange()

        // count: 1

        dictionary.TryRemove(key: 1);

        // fire OnCountChange()

        // count: 0

        observeClass.Dispose();

        dictionary.Dispose();
    }

    public class ObserveClass() : IDisposable
    {
        private IDisposable countChangeObserver;

        public ObserveClass(ObservableTimestampedDictionary<int, string> dictionary)
        {
            countChangeObserver = dictionary
                .ObserveCountChange()
                .Subscribe(count => OnCountChange(count));
        }

        public void Dispose()
        {
            countChangeObserver.Dispose();
        }

        private void OnCountChange(int count)
        {
            //logger.Debug($"count: {count}");
        }
    }
~~~
