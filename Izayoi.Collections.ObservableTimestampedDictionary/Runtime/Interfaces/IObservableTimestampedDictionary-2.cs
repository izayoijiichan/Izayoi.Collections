// ----------------------------------------------------------------------
// @Namespace : Izayoi.Collections
// @Interface : IObservableTimestampedDictionary
// ----------------------------------------------------------------------
#nullable enable
namespace Izayoi.Collections
{
    using System;
    //using System.Collections.Generic;

    /// <summary>
    /// Observable Timestamped Dictionary Interface
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public interface IObservableTimestampedDictionary<TKey, TValue> :
        ITimestampedDictionary<TKey, TValue>,
        IDisposable
        where TKey : struct
    {
        #region Properties

        //int Capacity { get; }

        //int Count { get; }

        #endregion

        #region Methods

        //void Clear();

        //void Clear(in int newCapacity);

        //int ClearBefore(in DateTimeOffset datetime);

        //int ClearBefore(in long unixTimeMilliseconds);

        //bool ContainsKey(in TKey key);

        //TimestampedDictionaryData<TKey, TValue> AddOrUpdate(in TKey key, in TValue? value);

        //bool TryAdd(in TKey key, in TValue? value);

        //bool TryUpdate(in TKey key, in TValue? newValue);

        //public bool TryUpdate(in TKey key, in TValue? newValue, in TValue? comparisonValue);

        //bool TryRemove(in TKey key, out TValue? value);

        //bool TryGetValue(in TKey key, out TValue? value);

        //bool TryGetData(in TKey key, out TimestampedDictionaryData<TKey, TValue>? data);

        //HashSet<TKey> GetKeys();

        //List<TKey> GetAddedKeys();

        //List<TimestampedKey<TKey>> GetTimestampedKeys();

        //void CheckConsistency();

        IObservable<ObservableTimestampedDictionaryAddEvent<TKey, TValue>> ObserveAdd();

        IObservable<ObservableTimestampedDictionaryRemoveEvent<TKey, TValue>> ObserveRemove();

        IObservable<ObservableTimestampedDictionaryUpdateEvent<TKey, TValue>> ObserveUpdate();

        IObservable<int> ObserveCountChange();

        IObservable<int> ObserveClear();

        //void Dispose();

        #endregion
    }
}
