// ----------------------------------------------------------------------
// @Namespace : Izayoi.Collections
// @Class     : ObservableTimestampedDictionaryAddEvent
// ----------------------------------------------------------------------
#nullable enable
namespace Izayoi.Collections
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Observable Timestamped Dictionary Add Event
    /// </summary>
    /// <remarks>Key type is string.</remarks>
    /// <typeparam name="TValue"></typeparam>
    public readonly struct ObservableTimestampedDictionaryAddEvent<TValue> :
         IEquatable<ObservableTimestampedDictionaryAddEvent<TValue>>
    {
        #region Fields

        private readonly TimestampedDictionaryData<TValue> data;

        #endregion

        #region Constructors

        public ObservableTimestampedDictionaryAddEvent(in TimestampedDictionaryData<TValue> data)
        {
            if (data is null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            this.data = data;
        }

        #endregion

        #region Properties

        public readonly TimestampedDictionaryData<TValue> Data => data;

        #endregion

        #region Methods

        public bool Equals(ObservableTimestampedDictionaryAddEvent<TValue> other)
        {
            if (data.Timestamp != other.Data.Timestamp)
            {
                return false;
            }

            if (data.Key != other.Data.Key)
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
