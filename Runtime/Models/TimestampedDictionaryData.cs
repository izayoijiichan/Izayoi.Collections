// ----------------------------------------------------------------------
// @Namespace : Izayoi.Collections
// @Class     : TimestampedDictionaryData
// ----------------------------------------------------------------------
namespace Izayoi.Collections
{
    /// <summary>
    /// Timestamped Dictionary Data
    /// </summary>
    /// <remarks>Key type is string.</remarks>
    /// <typeparam name="TValue"></typeparam>
    public class TimestampedDictionaryData<TValue>
    {
        #region Fields

        /// <summary>The timestamped key.</summary>
        private readonly TimestampedKey timestampedKey;

        /// <summary>The value.</summary>
        private readonly TValue? value;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes an instance of the TimestampedDictionaryData class with the specified timestampedKey and value.
        /// </summary>
        /// <param name="timestampedKey">A timestamped key.</param>
        /// <param name="value">A value.</param>
        public TimestampedDictionaryData(in TimestampedKey timestampedKey, in TValue? value)
        {
            this.timestampedKey = timestampedKey;

            this.value = value;
        }

        #endregion

        #region Properties

        /// <summary>The timestamped key.</summary>
        internal TimestampedKey TimestampedKey => timestampedKey;

        /// <summary>Timestamp when data is added to the dictionary.</summary>
        public long Timestamp => timestampedKey.Timestamp;

        /// <summary>The key.</summary>
        public string Key => timestampedKey.Key;

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
        public void Deconstruct(out long timestamp, out string key, out TValue? value)
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
