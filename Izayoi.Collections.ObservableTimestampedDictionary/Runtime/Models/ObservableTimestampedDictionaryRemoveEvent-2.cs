// ----------------------------------------------------------------------
// @Namespace : Izayoi.Collections
// @Class     : ObservableTimestampedDictionaryRemoveEvent
// ----------------------------------------------------------------------
#nullable enable
namespace Izayoi.Collections
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Observable Timestamped Dictionary Remove Event
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public readonly struct ObservableTimestampedDictionaryRemoveEvent<TKey, TValue> :
         IEquatable<ObservableTimestampedDictionaryRemoveEvent<TKey, TValue>>
        where TKey : struct
    {
        #region Fields

        private readonly TimestampedDictionaryData<TKey, TValue> data;

        #endregion

        #region Constructors

        public ObservableTimestampedDictionaryRemoveEvent(in TimestampedDictionaryData<TKey, TValue> data)
        {
            if (data is null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            this.data = data;
        }

        #endregion

        #region Properties

        public readonly TimestampedDictionaryData<TKey, TValue> Data => data;

        #endregion

        #region Methods

        public bool Equals(ObservableTimestampedDictionaryRemoveEvent<TKey, TValue> other)
        {
            if (data.Timestamp != other.Data.Timestamp)
            {
                return false;
            }

            if (EqualityComparer<TKey>.Default.Equals(data.Key, other.Data.Key) == false)
            {
                return false;
            }

            if (data.Value is null)
            {
                if (other.Data.Value is null)
                {
                    return true;
                }

                return false;
            }
            else
            {
                if (other.Data.Value is null)
                {
                    return false;
                }

                return EqualityComparer<TValue>.Default.Equals(data.Value, other.Data.Value);
            }
        }

        #endregion
    }
}
