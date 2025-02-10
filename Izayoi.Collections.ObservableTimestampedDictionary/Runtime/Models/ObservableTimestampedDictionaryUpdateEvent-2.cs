// ----------------------------------------------------------------------
// @Namespace : Izayoi.Collections
// @Class     : ObservableTimestampedDictionaryUpdateEvent
// ----------------------------------------------------------------------
#nullable enable
namespace Izayoi.Collections
{
    using System;

    /// <summary>
    /// Observable Timestamped Dictionary Update Event
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public readonly struct ObservableTimestampedDictionaryUpdateEvent<TKey, TValue> where TKey : struct
    {
        #region Fields

        private readonly TimestampedDictionaryData<TKey, TValue> oldData;

        private readonly TimestampedDictionaryData<TKey, TValue> newData;

        #endregion

        #region Constructors

        public ObservableTimestampedDictionaryUpdateEvent(
            in TimestampedDictionaryData<TKey, TValue> oldData,
            in TimestampedDictionaryData<TKey, TValue> newData)
        {
            if (oldData is null)
            {
                throw new ArgumentNullException(nameof(oldData));
            }

            if (newData is null)
            {
                throw new ArgumentNullException(nameof(newData));
            }

            this.oldData = oldData;

            this.newData = newData;
        }

        #endregion

        #region Properties

        public readonly TimestampedDictionaryData<TKey, TValue> OldData => oldData;

        public readonly TimestampedDictionaryData<TKey, TValue> NewData => newData;

        #endregion
    }
}
