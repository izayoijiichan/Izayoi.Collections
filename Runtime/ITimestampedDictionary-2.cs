// ----------------------------------------------------------------------
// @Namespace : Izayoi.Collections
// @Interface : ITimestampedDictionary
// ----------------------------------------------------------------------
namespace Izayoi.Collections
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Timestamped Dictionary Interface
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public interface ITimestampedDictionary<TKey, TValue> where TKey : struct
    {
        #region Properties

        /// <summary>Gets the maximum number of key/value pairs that can be contained in the dicionary.</summary>
        /// <remarks>When -1, there is no limit to the number of key/value pairs that can be stored.</remarks>
        int Capacity { get; }

        /// <summary>Gets the number of key/value pairs contained in the dictionary.</summary>
        int Count { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Removes all keys and values from the dictionary.
        /// </summary>
        void Clear();

        /// <summary>
        /// Removes all keys and values from the dictionary and resets its capaticy.
        /// </summary>
        /// <param name="newCapacity">New capacity.</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        void Clear(in int newCapacity);

        /// <summary>
        /// Delete all keys and values before the specified datetime.
        /// </summary>
        /// <param name="datetime">Base datetime of deletion.</param>
        /// <returns>Count of deleted data.</returns>
        int ClearBefore(in DateTimeOffset datetime);

        /// <summary>
        /// Delete all keys and values before the specified unixTimeMilliseconds.
        /// </summary>
        /// <param name="unixTimeMilliseconds">Base unixtime of deletion.</param>
        /// <returns>Count of deleted data.</returns>
        int ClearBefore(in long unixTimeMilliseconds);

        /// <summary>
        /// Determines whether the dictionary contains the specified key.
        /// </summary>
        /// <param name="key">The key to locate in the dictionary.</param>
        /// <returns>true if the dictionary contains an element with the specified key; otherwise, false.</returns>
        bool ContainsKey(in TKey key);

        /// <summary>
        /// Adds a key/value pair to the dictionary if the key does not already exist,
        /// or update.s a key/value pair in the dictionary if the key already exists.
        /// </summary>
        /// <param name="key">The key to be added or whose value should be update.d.</param>
        /// <param name="value">The value to be added for an absent key. It can be null.</param>
        /// <returns>The new value for the key.</returns>
        TimestampedDictionaryData<TKey, TValue> AddOrUpdate(in TKey key, in TValue? value);

        /// <summary>
        /// Attempts to add the specified key and value to the dictionary.
        /// </summary>
        /// <param name="key">The key of the element to add.</param>
        /// <param name="value">The value of the element to add. It can be null.</param>
        /// <returns>true if the key/value pair was added to the dictionary successfully; otherwise, false.</returns>
        bool TryAdd(in TKey key, in TValue? value);

        /// <summary>
        /// Updates the value associated with key to newValue.
        /// </summary>
        /// <param name="key">The key of the element to update.</param>
        /// <param name="newValue">The value that replaces the value of the element that has the specified key.</param>
        /// <returns>true if the key/value pair was replaced to the dictionary successfully; otherwise, false.</returns>
        bool TryUpdate(in TKey key, in TValue? newValue);

        /// <summary>
        /// Updates the value associated with key to newValue.
        /// </summary>
        /// <param name="key">The key of the element to update.</param>
        /// <param name="newValue">The value that replaces the value of the element that has the specified key.</param>
        /// <param name="comparisonValue">The value that is compared with the value of the element that has the specified key.</param>
        /// <returns>true if the key/value pair was replaced to the dictionary successfully; otherwise, false.</returns>
        public bool TryUpdate(in TKey key, in TValue? newValue, in TValue? comparisonValue);

        /// <summary>
        /// Attempts to remove and return the value that has the specified key from the dictionary.
        /// </summary>
        /// <param name="key">The key of the element to remove and return.</param>
        /// <param name="value">
        /// When this method returns, contains the object removed from the dictionary,
        /// or the default value of the TValue type if key does not exist.
        /// </param>
        /// <returns>true if the object was removed successfully; otherwise, false.</returns>
        bool TryRemove(in TKey key, out TValue? value);

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
        bool TryGetValue(in TKey key, out TValue? value);

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
        bool TryGetData(in TKey key, out TimestampedDictionaryData<TKey, TValue>? data);

        /// <summary>
        /// Gets a hashset containing the keys in the dictionary.
        /// </summary>
        /// <returns>A hashset containing the keys in the dictionary.</returns>
        HashSet<TKey> GetKeys();

        /// <summary>
        /// Gets a list containing the keys in the dictionary.
        /// </summary>
        /// <returns>A list containing the keys in the dictionary.</returns>
        /// <remarks>
        /// This list is in the order in which keys were added to the data in the dictionary.
        /// </remarks>
        List<TKey> GetAddedKeys();

        /// <summary>
        /// Gets a list containing the timestamped keys in the dictionary.
        /// </summary>
        /// <returns>A list containing the timestamped keys in the dictionary.</returns>
        /// <remarks>
        /// This list is in the order in which keys were added to the data in the dictionary.
        /// </remarks>
        List<TimestampedKey<TKey>> GetTimestampedKeys();

        /// <summary>
        /// Check the consistency of the dictionary.
        /// </summary>
        /// <remarks>
        /// This method is for debugging.
        /// </remarks>
        /// <exception cref="Exception">This occurs when there is an inconsistency in the dictionary.</exception>
        void CheckConsistency();

        #endregion
    }
}
