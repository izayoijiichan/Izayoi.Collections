// ----------------------------------------------------------------------
// @Namespace : Izayoi.Collections
// @Struct    : TimestampedKey
// ----------------------------------------------------------------------
#nullable enable
namespace Izayoi.Collections
{
    using System;

    /// <summary>
    /// Timestamped Key
    /// </summary>
    /// <remarks>Key type is string.</remarks>
    public struct TimestampedKey
    {
        #region Fields

        /// <summary>Timestamp of when this object was created.</summary>
        private readonly long timestamp;

        /// <summary>The original key.</summary>
        private readonly string originalKey;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes an instance of the TimestampedKey class with the specified key.
        /// </summary>
        /// <param name="key">A key.</param>
        public TimestampedKey(in string key)
        {
            timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            originalKey = key;
        }

        /// <summary>
        /// Initializes an instance of the TimestampedKey class with the specified key and timestamp.
        /// </summary>
        /// <param name="key">A key.</param>
        /// <param name="timestamp">A timestamp.</param>
        public TimestampedKey(in string key, in long timestamp)
        {
            this.timestamp = timestamp;

            originalKey = key;
        }

        #endregion

        #region Properties

        /// <summary>Timestamp of when this object was created.</summary>
        public long Timestamp => timestamp;

        /// <summary>The key.</summary>
        public string Key => originalKey;

        #endregion

        #region Methods

        /// <summary>
        /// Deconstructs the current object.
        /// </summary>
        /// <param name="timestamp">Timestamp when data is added to the dictionary.</param>
        /// <param name="key">The key.</param>
        public readonly void Deconstruct(out long timestamp, out string key)
        {
            timestamp = this.timestamp;

            key = originalKey;
        }

        /// <summary>
        /// Indicates whether this instance and a specified object are equal.
        /// </summary>
        /// <param name="other".</param>
        /// <returns>true if specified object and this instance represent the same value; otherwise, false.</returns>
        public readonly bool Equals(TimestampedKey other)
        {
            return
                timestamp.Equals(other.timestamp) &&
                originalKey.Equals(other.originalKey);
        }

        public override readonly bool Equals(object? other)
        {
            if (other is TimestampedKey otherTimestampedKey)
            {
                return Equals(otherTimestampedKey);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(timestamp, originalKey);
        }

        public override string ToString()
        {
            return $"{nameof(Timestamp)}: {timestamp}, Key: {originalKey}";
        }

        #endregion

        #region Operators

        public static bool operator ==(in TimestampedKey key1, in TimestampedKey key2)
        {
            return key1.Equals(key2);
        }

        public static bool operator !=(in TimestampedKey key1, in TimestampedKey key2)
        {
            return !key1.Equals(key2);
        }

        #endregion
    }
}
