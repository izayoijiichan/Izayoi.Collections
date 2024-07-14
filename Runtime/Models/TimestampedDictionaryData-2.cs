// ----------------------------------------------------------------------
// @Namespace : Izayoi.Collections
// @Class     : TimestampedDictionaryData
// ----------------------------------------------------------------------
namespace Izayoi.Collections
{
    /// <summary>
    /// Timestamped Dictionary Data
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class TimestampedDictionaryData<TKey, TValue> where TKey : struct
    {
        #region Fields

        /// <summary>The timestamped key.</summary>
        private readonly TimestampedKey<TKey> timestampedKey;

        /// <summary>The value.</summary>
        private readonly TValue? value;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes an instance of the TimestampedDictionaryData class with the specified timestampedKey and value.
        /// </summary>
        /// <param name="timestampedKey">A timestamped key.</param>
        /// <param name="value">A value.</param>
        public TimestampedDictionaryData(in TimestampedKey<TKey> timestampedKey, in TValue? value)
        {
            this.timestampedKey = timestampedKey;

            this.value = value;
        }

        #endregion

        #region Properties

        /// <summary>The timestamped key.</summary>
        internal TimestampedKey<TKey> TimestampedKey => timestampedKey;

        /// <summary>Timestamp when data is added to the dictionary.</summary>
        public long Timestamp => timestampedKey.Timestamp;

        /// <summary>The key.</summary>
        public TKey Key => timestampedKey.Key;

        /// <summary>The value.</summary>
        public TValue? Value => value;

        #endregion

        #region Methods

        /// <summary>
        /// Deconstructs the current object.
        /// </summary>
        /// <param name="timestamp">Timestamp when data is added to the dictionary.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public void Deconstruct(out long timestamp, out TKey key, out TValue? value)
        {
            timestamp = timestampedKey.Timestamp;

            key = timestampedKey.Key;

            value = this.value;
        }

        public override string ToString()
        {
            return $"{nameof(Timestamp)}: {timestampedKey.Timestamp}, {nameof(Key)}: {timestampedKey.Key}";
        }

        #endregion
    }
}
