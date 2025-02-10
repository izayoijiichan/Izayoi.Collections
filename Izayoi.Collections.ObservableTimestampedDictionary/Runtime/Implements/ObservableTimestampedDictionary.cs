// ----------------------------------------------------------------------
// @Namespace : Izayoi.Collections
// @Class     : ObservableTimestampedDictionary
// ----------------------------------------------------------------------
#nullable enable
namespace Izayoi.Collections
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Reactive.Linq;
    using System.Reactive.Subjects;

    /// <summary>
    /// Observable Timestamped Dictionary
    /// </summary>
    /// <remarks>Key type is string.</remarks>
    /// <typeparam name="TValue"></typeparam>
    public class ObservableTimestampedDictionary<TValue> :
        IObservableTimestampedDictionary<TValue>,
        IDisposable
    {
        #region Fields

        /// <summary>Gets the maximum number of key/value pairs that can be contained in the dictionary.</summary>
        /// <remarks>When -1, there is no limit to the number of key/value pairs that can be stored.</remarks>
        private int capacity;

        /// <summary></summary>
        private readonly ConcurrentDictionary<string, TimestampedDictionaryData<TValue>> dictionary;

        /// <summary>List of TimestampedKey.</summary>
        private readonly LinkedList<TimestampedKey> timestampedKeyList;

        /// <summary>An object for locking.</summary>
        private readonly object lockObject;

        /// <summary></summary>
        private bool isDisposed;

        /// <summary></summary>
        private Subject<ObservableTimestampedDictionaryAddEvent<TValue>>? addSubject;

        /// <summary></summary>
        private Subject<ObservableTimestampedDictionaryRemoveEvent<TValue>>? removeSubject;

        /// <summary></summary>
        private Subject<ObservableTimestampedDictionaryUpdateEvent<TValue>>? updateSubject;

        /// <summary>int: current count</summary>
        private Subject<int>? countChangeSubject;

        /// <summary>int: removed count</summary>
        private Subject<int>? clearSubject;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes an instance of the ObservableTimestampedDictionary class with the specified capacity and dictionary.
        /// </summary>
        /// <param name="capacity">The maximum number of key/value pairs that can be contained in the dictionary. When -1, there is no limit.</param>
        /// <param name="dictionary">The IDictionary whose elements are copied to the new TimestampedDictionary.</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public ObservableTimestampedDictionary(in int capacity = -1, in IDictionary<string, TValue>? dictionary = null)
        {
            if (capacity == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(capacity));
            }

            if (capacity < -1)
            {
                throw new ArgumentOutOfRangeException(nameof(capacity));
            }

            this.capacity = capacity;

            lockObject = new object();

            if (dictionary is null || dictionary.Count == 0)
            {
                timestampedKeyList = new LinkedList<TimestampedKey>();

                this.dictionary = new ConcurrentDictionary<string, TimestampedDictionaryData<TValue>>();
            }
            else
            {
                if (capacity != -1 && dictionary.Count > capacity)
                {
                    throw new ArgumentOutOfRangeException(nameof(dictionary));
                }

                lock (lockObject)
                {
                    timestampedKeyList = new LinkedList<TimestampedKey>();

                    this.dictionary = new ConcurrentDictionary<string, TimestampedDictionaryData<TValue>>();

                    foreach ((string key, TValue value) in dictionary)
                    {
                        var timestampedKey = new TimestampedKey(key);

                        var timestampedDictionaryData = new TimestampedDictionaryData<TValue>(timestampedKey, value);

                        if (!this.dictionary.TryAdd(key, timestampedDictionaryData))
                        {
                            throw new ArgumentException(nameof(dictionary));
                        }

                        timestampedKeyList.AddLast(timestampedKey);
                    }
                }
            }
        }

        #endregion

        #region Properties

        /// <summary>Gets the maximum number of key/value pairs that can be contained in the dictionary.</summary>
        /// <remarks>When -1, there is no limit to the number of key/value pairs that can be stored.</remarks>
        public int Capacity => capacity;

        /// <summary>Gets the number of key/value pairs contained in the dictionary.</summary>
        public int Count => dictionary.Count;

        #endregion

        #region Methods

        /// <summary>
        /// Removes all keys and values from the dictionary.
        /// </summary>
        public void Clear()
        {
            if (dictionary.Count == 0)
            {
                clearSubject?.OnNext(0);

                return;
            }

            lock (lockObject)
            {
                int removedCount = dictionary.Count;

                dictionary.Clear();

                timestampedKeyList.Clear();

                clearSubject?.OnNext(removedCount);

                countChangeSubject?.OnNext(0);
            }
        }

        /// <summary>
        /// Removes all keys and values from the dictionary and resets its capacity.
        /// </summary>
        /// <param name="newCapacity">New capacity.</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public void Clear(in int newCapacity)
        {
            if (newCapacity == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(newCapacity));
            }

            if (newCapacity < -1)
            {
                throw new ArgumentOutOfRangeException(nameof(newCapacity));
            }

            Clear();

            capacity = newCapacity;
        }

        /// <summary>
        /// Delete all keys and values before the specified datetime.
        /// </summary>
        /// <param name="datetime">Base datetime of deletion.</param>
        /// <returns>Count of deleted data.</returns>
        public int ClearBefore(in DateTimeOffset datetime)
        {
            return ClearBefore(datetime.ToUnixTimeMilliseconds());
        }

        /// <summary>
        /// Delete all keys and values before the specified unixTimeMilliseconds.
        /// </summary>
        /// <param name="unixTimeMilliseconds">Base unixtime of deletion.</param>
        /// <returns>Count of deleted data.</returns>
        public int ClearBefore(in long unixTimeMilliseconds)
        {
            lock (lockObject)
            {
                var targetKeyList = new List<TimestampedKey>();

                foreach (var timestampedKey in timestampedKeyList)
                {
                    if (timestampedKey.Timestamp >= unixTimeMilliseconds)
                    {
                        break;
                    }

                    targetKeyList.Add(timestampedKey);
                }

                if (targetKeyList.Count == 0)
                {
                    // @notice
                    clearSubject?.OnNext(0);

                    return 0;
                }

                int removedCount = 0;

                var errorKeyList = new List<TimestampedKey>(0);

                foreach (var timestampedKey in targetKeyList)
                {
                    if (!dictionary.TryRemove(timestampedKey.Key, out var removedData))
                    {
                        errorKeyList.Add(timestampedKey);

                        // @notice
                        timestampedKeyList.Remove(timestampedKey);

                        continue;
                    }

                    timestampedKeyList.Remove(timestampedKey);

                    removedCount++;

                    //removeSubject?.OnNext(new ObservableTimestampedDictionaryRemoveEvent<TValue>(removedData));
                }

                if (errorKeyList.Count > 0)
                {
                    //throw new Exception();
                }

                // @notice
                clearSubject?.OnNext(removedCount);

                if (removedCount > 0)
                {
                    countChangeSubject?.OnNext(dictionary.Count);
                }

                return removedCount;
            }
        }

        /// <summary>
        /// Determines whether the dictionary contains the specified key.
        /// </summary>
        /// <param name="key">The key to locate in the dictionary.</param>
        /// <returns>true if the dictionary contains an element with the specified key; otherwise, false.</returns>
        public bool ContainsKey(in string key)
        {
            return dictionary.ContainsKey(key);
        }

        /// <summary>
        /// Adds a key/value pair to the dictionary if the key does not already exist,
        /// or update a key/value pair in the dictionary if the key already exists.
        /// </summary>
        /// <param name="key">The key to be added or whose value should be update.d.</param>
        /// <param name="value">The value to be added for an absent key. It can be null.</param>
        /// <returns>The new value for the key.</returns>
        public TimestampedDictionaryData<TValue> AddOrUpdate(in string key, in TValue? value)
        {
            if (capacity != -1)
            {
                if (timestampedKeyList.Count >= capacity)
                {
                    if (!dictionary.ContainsKey(key))
                    {
                        lock (lockObject)
                        {
                            var oldestTimestampedKey = timestampedKeyList.First;

                            if (!dictionary.TryRemove(oldestTimestampedKey.Value.Key, out _))
                            {
                                throw new Exception($"dictionary.TryRemove(key: {oldestTimestampedKey.Value.Key})");
                            }

                            timestampedKeyList.RemoveFirst();
                        }
                    }
                }
            }

            var newTimestampedKey = new TimestampedKey(key);

            var newTimestampedDictionaryData = new TimestampedDictionaryData<TValue>(newTimestampedKey, value);

            lock (lockObject)
            {
                if (dictionary.TryGetValue(key, out var oldData))
                {
                    if (dictionary.TryUpdate(key, newTimestampedDictionaryData, oldData))
                    {
                        timestampedKeyList.Remove(oldData.TimestampedKey);

                        timestampedKeyList.AddLast(newTimestampedKey);

                        updateSubject?.OnNext(new ObservableTimestampedDictionaryUpdateEvent<TValue>(oldData, newTimestampedDictionaryData));
                    }
                    else
                    {
                        throw new Exception($"dictionary.TryUpdate(key: {key})");
                    }
                }
                else
                {
                    if (dictionary.TryAdd(key, newTimestampedDictionaryData))
                    {
                        timestampedKeyList.AddLast(newTimestampedKey);

                        addSubject?.OnNext(new ObservableTimestampedDictionaryAddEvent<TValue>(newTimestampedDictionaryData));

                        countChangeSubject?.OnNext(dictionary.Count);
                    }
                    else
                    {
                        throw new Exception($"dictionary.TryAdd(key: {key})");
                    }
                }
            }

            return newTimestampedDictionaryData;
        }

        /// <summary>
        /// Attempts to add the specified key and value to the dictionary.
        /// </summary>
        /// <param name="key">The key of the element to add.</param>
        /// <param name="value">The value of the element to add. It can be null.</param>
        /// <returns>true if the key/value pair was added to the dictionary successfully; otherwise, false.</returns>
        public bool TryAdd(in string key, in TValue? value)
        {
            if (dictionary.ContainsKey(key))
            {
                return false;
            }

            int beforeCount = dictionary.Count;

            if (capacity != -1)
            {
                if (timestampedKeyList.Count >= capacity)
                {
                    lock (lockObject)
                    {
                        var oldestTimestampedKey = timestampedKeyList.First;

                        if (!dictionary.TryRemove(oldestTimestampedKey.Value.Key, out var removedData))
                        {
                            throw new Exception($"dictionary.TryRemove(key: {oldestTimestampedKey.Value.Key})");
                        }

                        timestampedKeyList.RemoveFirst();

                        removeSubject?.OnNext(new ObservableTimestampedDictionaryRemoveEvent<TValue>(removedData));
                    }
                }
            }

            lock (lockObject)
            {
                var timestampedKey = new TimestampedKey(key);

                var timestampedDictionaryData = new TimestampedDictionaryData<TValue>(timestampedKey, value);

                if (!dictionary.TryAdd(key, timestampedDictionaryData))
                {
                    return false;
                }

                timestampedKeyList.AddLast(timestampedKey);

                addSubject?.OnNext(new ObservableTimestampedDictionaryAddEvent<TValue>(timestampedDictionaryData));

                if (dictionary.Count != beforeCount)
                {
                    countChangeSubject?.OnNext(dictionary.Count);
                }
            }

            return true;
        }

        /// <summary>
        /// Updates the value associated with key to newValue.
        /// </summary>
        /// <param name="key">The key of the element to update.</param>
        /// <param name="newValue">The value that updates the value of the element that has the specified key. It can be null.</param>
        /// <returns>true if the key/value pair was updated to the dictionary successfully; otherwise, false.</returns>
        public bool TryUpdate(in string key, in TValue? newValue)
        {
            lock (lockObject)
            {
                if (!dictionary.TryGetValue(key, out var oldTimestampedDictionaryData))
                {
                    return false;
                }

                var newTimestampedKey = new TimestampedKey(key);

                var newTimestampedDictionaryData = new TimestampedDictionaryData<TValue>(newTimestampedKey, newValue);

                if (!dictionary.TryUpdate(key, newTimestampedDictionaryData, oldTimestampedDictionaryData))
                {
                    return false;
                }

                timestampedKeyList.Remove(oldTimestampedDictionaryData.TimestampedKey);

                timestampedKeyList.AddLast(newTimestampedKey);

                updateSubject?.OnNext(new ObservableTimestampedDictionaryUpdateEvent<TValue>(oldTimestampedDictionaryData, newTimestampedDictionaryData));
            }

            return true;
        }

        /// <summary>
        /// Updates the value associated with key to newValue.
        /// </summary>
        /// <param name="key">The key of the element to update.</param>
        /// <param name="newValue">The value that updates the value of the element that has the specified key.</param>
        /// <param name="comparisonValue">The value that is compared with the value of the element that has the specified key.</param>
        /// <returns>true if the key/value pair was updated to the dictionary successfully; otherwise, false.</returns>
        public bool TryUpdate(in string key, in TValue? newValue, in TValue? comparisonValue)
        {
            lock (lockObject)
            {
                if (!dictionary.TryGetValue(key, out var oldTimestampedDictionaryData))
                {
                    return false;
                }

                if (oldTimestampedDictionaryData.Value != null && comparisonValue != null)
                {
                    if (!oldTimestampedDictionaryData.Value.Equals(comparisonValue))
                    {
                        return false;
                    }
                }

                var newTimestampedKey = new TimestampedKey(key);

                var newTimestampedDictionaryData = new TimestampedDictionaryData<TValue>(newTimestampedKey, newValue);

                if (!dictionary.TryUpdate(key, newTimestampedDictionaryData, oldTimestampedDictionaryData))
                {
                    return false;
                }

                timestampedKeyList.Remove(oldTimestampedDictionaryData.TimestampedKey);

                timestampedKeyList.AddLast(newTimestampedKey);

                updateSubject?.OnNext(new ObservableTimestampedDictionaryUpdateEvent<TValue>(oldTimestampedDictionaryData, newTimestampedDictionaryData));
            }

            return true;
        }

        /// <summary>
        /// Attempts to remove and return the value that has the specified key from the dictionary.
        /// </summary>
        /// <param name="key">The key of the element to remove and return.</param>
        /// <param name="value">
        /// When this method returns, contains the object removed from the dictionary,
        /// or the default value of the TValue type if key does not exist.
        /// </param>
        /// <returns>true if the object was removed successfully; otherwise, false.</returns>
        public bool TryRemove(in string key, out TValue? value)
        {
            value = default;

            if (!dictionary.TryGetValue(key, out var timestampedDictionaryData))
            {
                return false;
            }

            lock (lockObject)
            {
                if (!timestampedKeyList.Contains(timestampedDictionaryData.TimestampedKey))
                {
                    return false;
                }

                if (!dictionary.TryRemove(key, out var removedData))
                {
                    return false;
                }

                value = removedData.Value;

                timestampedKeyList.Remove(removedData.TimestampedKey);

                removeSubject?.OnNext(new ObservableTimestampedDictionaryRemoveEvent<TValue>(removedData));

                countChangeSubject?.OnNext(dictionary.Count);
            }

            return true;
        }

        /// <summary>
        /// Gets the value associated with the specified key.
        /// </summary>
        /// <param name="key">The key of the value to get.</param>
        /// <param name="value">
        /// When this method returns, contains the value associated with the specified key, if the key is found;
        /// otherwise, the default value for the type of the value parameter.
        /// This parameter is passed uninitialized.
        /// </param>
        /// <returns>true if the dictionary contains an element with the specified key; otherwise, false.</returns>
        public bool TryGetValue(in string key, out TValue? value)
        {
            value = default;

            if (!dictionary.TryGetValue(key, out var timestampedDictionaryData))
            {
                return false;
            }

            value = timestampedDictionaryData.Value;

            return true;
        }

        /// <summary>
        /// Gets the data associated with the specified key.
        /// </summary>
        /// <param name="key">The key of the value to get.</param>
        /// <param name="data">
        /// When this method returns, contains the value associated with the specified key, if the key is found;
        /// otherwise, the default value for the type of the timestamped dictionary data.
        /// This parameter is passed uninitialized.
        /// </param>
        /// <returns>true if the dictionary contains an element with the specified key; otherwise, false.</returns>
        public bool TryGetData(in string key, out TimestampedDictionaryData<TValue>? data)
        {
            data = default;

            if (!dictionary.TryGetValue(key, out var timestampedDictionaryData))
            {
                return false;
            }

            data = timestampedDictionaryData;

            return true;
        }

        /// <summary>
        /// Gets a hashset containing the keys in the dictionary.
        /// </summary>
        /// <returns>A hashset containing the keys in the dictionary.</returns>
        public HashSet<string> GetKeys()
        {
            return new HashSet<string>(dictionary.Keys);
        }

        /// <summary>
        /// Gets a list containing the keys in the dictionary.
        /// </summary>
        /// <returns>A list containing the keys in the dictionary.</returns>
        /// <remarks>
        /// This list is in the order in which keys were added to the data in the dictionary.
        /// </remarks>
        public List<string> GetAddedKeys()
        {
            var list = new List<string>();

            foreach (var timestampedKey in timestampedKeyList)
            {
                list.Add(timestampedKey.Key);
            }

            return list;
        }

        /// <summary>
        /// Gets a list containing the timestamped keys in the dictionary.
        /// </summary>
        /// <returns>A list containing the timestamped keys in the dictionary.</returns>
        /// <remarks>
        /// This list is in the order in which keys were added to the data in the dictionary.
        /// </remarks>
        public List<TimestampedKey> GetTimestampedKeys()
        {
            var list = new List<TimestampedKey>();

            foreach (var timestampedKey in timestampedKeyList)
            {
                list.Add(timestampedKey);
            }

            return list;
        }

        /// <summary>
        /// Check the consistency of the dictionary.
        /// </summary>
        /// <remarks>
        /// This method is for debugging.
        /// </remarks>
        /// <exception cref="Exception">This occurs when there is an inconsistency in the dictionary.</exception>
        public void CheckConsistency()
        {
            if (dictionary.Count != timestampedKeyList.Count)
            {
                throw new Exception($"key.Count: {timestampedKeyList.Count}, data.Count: {dictionary.Count}");
            }
        }

        #endregion

        #region Observe

        public IObservable<ObservableTimestampedDictionaryAddEvent<TValue>> ObserveAdd()
        {
            if (isDisposed)
            {
                return Observable.Empty<ObservableTimestampedDictionaryAddEvent<TValue>>();
            }

            return addSubject ??= new Subject<ObservableTimestampedDictionaryAddEvent<TValue>>();
        }

        public IObservable<ObservableTimestampedDictionaryRemoveEvent<TValue>> ObserveRemove()
        {
            if (isDisposed)
            {
                return Observable.Empty<ObservableTimestampedDictionaryRemoveEvent<TValue>>();
            }

            return removeSubject ??= new Subject<ObservableTimestampedDictionaryRemoveEvent<TValue>>();
        }

        public IObservable<ObservableTimestampedDictionaryUpdateEvent<TValue>> ObserveUpdate()
        {
            if (isDisposed)
            {
                return Observable.Empty<ObservableTimestampedDictionaryUpdateEvent<TValue>>();
            }

            return updateSubject ??= new Subject<ObservableTimestampedDictionaryUpdateEvent<TValue>>();
        }

        public IObservable<int> ObserveCountChange()
        {
            if (isDisposed)
            {
                return Observable.Empty<int>();
            }

            return countChangeSubject ??= new Subject<int>();
        }

        public IObservable<int> ObserveClear()
        {
            if (isDisposed)
            {
                return Observable.Empty<int>();
            }

            return clearSubject ??= new Subject<int>();
        }

        #endregion

        #region Dispose

        public void Dispose()
        {
            Dispose(disposing: true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (isDisposed)
            {
                return;
            }

            if (disposing)
            {
                DisposeSubject(ref addSubject);
                DisposeSubject(ref removeSubject);
                DisposeSubject(ref updateSubject);
                DisposeSubject(ref countChangeSubject);
                DisposeSubject(ref clearSubject);
            }

            isDisposed = true;
        }

        private void DisposeSubject<TSubject>(ref Subject<TSubject>? subject)
        {
            if (subject is null)
            {
                return;
            }

            try
            {
                subject.OnCompleted();
            }
            finally
            {
                try
                {
                    subject.Dispose();
                }
                finally
                {
                    subject = null;
                }
            }
        }

        #endregion
    }
}
