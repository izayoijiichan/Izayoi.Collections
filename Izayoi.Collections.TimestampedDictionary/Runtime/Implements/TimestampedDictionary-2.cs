// ----------------------------------------------------------------------
// @Namespace : Izayoi.Collections
// @Class     : TimestampedDictionary
// ----------------------------------------------------------------------
#nullable enable
namespace Izayoi.Collections
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;

    /// <summary>
    /// Timestamped Dictionary
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class TimestampedDictionary<TKey, TValue> : ITimestampedDictionary<TKey, TValue> where TKey : struct
    {
        #region Fields

        /// <summary>Gets the maximum number of key/value pairs that can be contained in the dictionary.</summary>
        /// <remarks>When -1, there is no limit to the number of key/value pairs that can be stored.</remarks>
        private int capacity;

        /// <summary></summary>
        private readonly ConcurrentDictionary<TKey, TimestampedDictionaryData<TKey, TValue>> dictionary;

        /// <summary>List of TimestampedKey.</summary>
        private readonly LinkedList<TimestampedKey<TKey>> timestampedKeyList;

        /// <summary>An object for locking.</summary>
        private readonly object lockObject;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes an instance of the TimestampedDictionary class with the specified capacity, dictionary and comparer.
        /// </summary>
        /// <param name="capacity">The maximum number of key/value pairs that can be contained in the dictionary. When -1, there is no limit.</param>
        /// <param name="dictionary">The IDictionary whose elements are copied to the new TimestampedDictionary.</param>
        /// <param name="comparer">The IEqualityComparer<T> implementation to use when comparing keys, or null to use the default EqualityComparer<T> for the type of the key.</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public TimestampedDictionary(in int capacity = -1, in IDictionary<TKey, TValue>? dictionary = null, in IEqualityComparer<TKey>? comparer = null)
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
                timestampedKeyList = new LinkedList<TimestampedKey<TKey>>();

                if (comparer is null)
                {
                    this.dictionary = new ConcurrentDictionary<TKey, TimestampedDictionaryData<TKey, TValue>>();
                }
                else
                {
                    this.dictionary = new ConcurrentDictionary<TKey, TimestampedDictionaryData<TKey, TValue>>(comparer);
                }
            }
            else
            {
                if (capacity != -1 && dictionary.Count > capacity)
                {
                    throw new ArgumentOutOfRangeException(nameof(dictionary));
                }

                lock (lockObject)
                {
                    timestampedKeyList = new LinkedList<TimestampedKey<TKey>>();

                    if (comparer is null)
                    {
                        this.dictionary = new ConcurrentDictionary<TKey, TimestampedDictionaryData<TKey, TValue>>();
                    }
                    else
                    {
                        this.dictionary = new ConcurrentDictionary<TKey, TimestampedDictionaryData<TKey, TValue>>(comparer);
                    }

                    foreach ((TKey key, TValue value) in dictionary)
                    {
                        var timestampedKey = new TimestampedKey<TKey>(key);

                        var timestampedDictionaryData = new TimestampedDictionaryData<TKey, TValue>(timestampedKey, value);

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
            dictionary.Clear();

            timestampedKeyList.Clear();
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
                var targetKeyList = new List<TimestampedKey<TKey>>();

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
                    return 0;
                }

                int removedCount = 0;

                var errorKeyList = new List<TimestampedKey<TKey>>(0);

                foreach (var timestampedKey in targetKeyList)
                {
                    if (!dictionary.TryRemove(timestampedKey.Key, out _))
                    {
                        errorKeyList.Add(timestampedKey);

                        // @notice
                        timestampedKeyList.Remove(timestampedKey);

                        continue;
                    }

                    timestampedKeyList.Remove(timestampedKey);

                    removedCount++;
                }

                if (errorKeyList.Count > 0)
                {
                    //throw new Exception();
                }

                return removedCount;
            }
        }

        /// <summary>
        /// Determines whether the dictionary contains the specified key.
        /// </summary>
        /// <param name="key">The key to locate in the dictionary.</param>
        /// <returns>true if the dictionary contains an element with the specified key; otherwise, false.</returns>
        public bool ContainsKey(in TKey key)
        {
            return dictionary.ContainsKey(key);
        }

        /// <summary>
        /// Adds a key/value pair to the dictionary if the key does not already exist,
        /// or update.s a key/value pair in the dictionary if the key already exists.
        /// </summary>
        /// <param name="key">The key to be added or whose value should be update.d.</param>
        /// <param name="value">The value to be added for an absent key. It can be null.</param>
        /// <returns>The new value for the key.</returns>
        public TimestampedDictionaryData<TKey, TValue> AddOrUpdate(in TKey key, in TValue? value)
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

            var newTimestampedKey = new TimestampedKey<TKey>(key);

            var newTimestampedDictionaryData = new TimestampedDictionaryData<TKey, TValue>(newTimestampedKey, value);

            lock (lockObject)
            {
                if (dictionary.TryGetValue(key, out var oldData))
                {
                    if (dictionary.TryUpdate(key, newTimestampedDictionaryData, oldData))
                    {
                        timestampedKeyList.Remove(oldData.TimestampedKey);

                        timestampedKeyList.AddLast(newTimestampedKey);
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
        public bool TryAdd(in TKey key, in TValue? value)
        {
            if (dictionary.ContainsKey(key))
            {
                return false;
            }

            if (capacity != -1)
            {
                if (timestampedKeyList.Count >= capacity)
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

            lock (lockObject)
            {
                var timestampedKey = new TimestampedKey<TKey>(key);

                var timestampedDictionaryData = new TimestampedDictionaryData<TKey, TValue>(timestampedKey, value);

                if (!dictionary.TryAdd(key, timestampedDictionaryData))
                {
                    return false;
                }

                timestampedKeyList.AddLast(timestampedKey);
            }

            return true;
        }

        /// <summary>
        /// Updates the value associated with key to newValue.
        /// </summary>
        /// <param name="key">The key of the element to update.</param>
        /// <param name="newValue">The value that replaces the value of the element that has the specified key.</param>
        /// <returns>true if the key/value pair was replaced to the dictionary successfully; otherwise, false.</returns>
        public bool TryUpdate(in TKey key, in TValue? newValue)
        {
            lock (lockObject)
            {
                if (!dictionary.TryGetValue(key, out var oldTimestampedDictionaryData))
                {
                    return false;
                }

                var newTimestampedKey = new TimestampedKey<TKey>(key);

                var newTimestampedDictionaryData = new TimestampedDictionaryData<TKey, TValue>(newTimestampedKey, newValue);

                if (!dictionary.TryUpdate(key, newTimestampedDictionaryData, oldTimestampedDictionaryData))
                {
                    return false;
                }

                timestampedKeyList.Remove(oldTimestampedDictionaryData.TimestampedKey);

                timestampedKeyList.AddLast(newTimestampedKey);
            }

            return true;
        }

        /// <summary>
        /// Updates the value associated with key to newValue.
        /// </summary>
        /// <param name="key">The key of the element to update.</param>
        /// <param name="newValue">The value that replaces the value of the element that has the specified key.</param>
        /// <param name="comparisonValue">The value that is compared with the value of the element that has the specified key.</param>
        /// <returns>true if the key/value pair was replaced to the dictionary successfully; otherwise, false.</returns>
        public bool TryUpdate(in TKey key, in TValue? newValue, in TValue? comparisonValue)
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

                var newTimestampedKey = new TimestampedKey<TKey>(key);

                var newTimestampedDictionaryData = new TimestampedDictionaryData<TKey, TValue>(newTimestampedKey, newValue);

                if (!dictionary.TryUpdate(key, newTimestampedDictionaryData, oldTimestampedDictionaryData))
                {
                    return false;
                }

                timestampedKeyList.Remove(oldTimestampedDictionaryData.TimestampedKey);

                timestampedKeyList.AddLast(newTimestampedKey);
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
        public bool TryRemove(in TKey key, out TValue? value)
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
        public bool TryGetValue(in TKey key, out TValue? value)
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
        public bool TryGetData(in TKey key, out TimestampedDictionaryData<TKey, TValue>? data)
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
        public HashSet<TKey> GetKeys()
        {
            return new HashSet<TKey>(dictionary.Keys);
        }

        /// <summary>
        /// Gets a list containing the keys in the dictionary.
        /// </summary>
        /// <returns>A list containing the keys in the dictionary.</returns>
        /// <remarks>
        /// This list is in the order in which keys were added to the data in the dictionary.
        /// </remarks>
        public List<TKey> GetAddedKeys()
        {
            var list = new List<TKey>();

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
        public List<TimestampedKey<TKey>> GetTimestampedKeys()
        {
            var list = new List<TimestampedKey<TKey>>();

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
    }
}
