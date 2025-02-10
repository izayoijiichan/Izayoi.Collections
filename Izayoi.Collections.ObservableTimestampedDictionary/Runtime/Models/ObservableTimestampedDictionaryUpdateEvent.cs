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
    /// <remarks>Key type is string.</remarks>
    /// <typeparam name="TValue"></typeparam>
    public readonly struct ObservableTimestampedDictionaryUpdateEvent<TValue>
    {
        #region Fields

        private readonly TimestampedDictionaryData<TValue> oldData;

        private readonly TimestampedDictionaryData<TValue> newData;

        #endregion

        #region Constructors

        public ObservableTimestampedDictionaryUpdateEvent(
            in TimestampedDictionaryData<TValue> oldData,
            in TimestampedDictionaryData<TValue> newData)
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

        public readonly TimestampedDictionaryData<TValue> OldData => oldData;

        public readonly TimestampedDictionaryData<TValue> NewData => newData;

        #endregion
    }
}
