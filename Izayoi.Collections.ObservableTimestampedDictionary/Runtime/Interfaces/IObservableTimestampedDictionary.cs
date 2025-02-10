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
    /// ObservableTimestamped Dictionary Interface
    /// </summary>
    /// <remarks>Key type is string.</remarks>
    /// <typeparam name="TValue"></typeparam>
    public interface IObservableTimestampedDictionary<TValue> :
        ITimestampedDictionary<TValue>,
        IDisposable
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

        //bool ContainsKey(in string key);

        //TimestampedDictionaryData<TValue> AddOrUpdate(in string key, in TValue? value);

        //bool TryAdd(in string key, in TValue? value);

        //bool TryUpdate(in string key, in TValue? newValue);

        //public bool TryUpdate(in string key, in TValue? newValue, in TValue? comparisonValue);

        //bool TryRemove(in string key, out TValue? value);
        //bool TryGetValue(in string key, out TValue? value);

        //bool TryGetData(in string key, out TimestampedDictionaryData<TValue>? data);

        //HashSet<string> GetKeys();

        //List<string> GetAddedKeys();

        //List<TimestampedKey> GetTimestampedKeys();

        //void CheckConsistency();

        IObservable<ObservableTimestampedDictionaryAddEvent<TValue>> ObserveAdd();

        IObservable<ObservableTimestampedDictionaryRemoveEvent<TValue>> ObserveRemove();

        IObservable<ObservableTimestampedDictionaryUpdateEvent<TValue>> ObserveUpdate();

        IObservable<int> ObserveCountChange();

        IObservable<int> ObserveClear();

        //void Dispose();

        #endregion
    }
}
