// ----------------------------------------------------------------------
// @Namespace : Izayoi.Collections.TimestampedDictionary.Test
// @Class     : TimestampedDictionaryTest2
// ----------------------------------------------------------------------
namespace Izayoi.Collections.TimestampedDictionary.Test
{
    using Izayoi.Collections;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class TimestampedDictionaryTest2
    {
        #region Construct

        [Fact]
        public void Test2_Construct()
        {
            var tsDictionary = new TimestampedDictionary<int, string>();

            Assert.Equal(-1, tsDictionary.Capacity);

            Assert.Equal(0, tsDictionary.Count);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(1)]
        [InlineData(2)]
        public void Test2_Construct_with_correct_capacity(int capacity)
        {
            var tsDictionary = new TimestampedDictionary<int, string>(capacity);

            Assert.Equal(capacity, tsDictionary.Capacity);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-2)]
        [InlineData(-3)]
        public void Test2_Construct_with_invalid_capacity(int capacity)
        {
            var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                var tsDictionary = new TimestampedDictionary<int, string>(capacity);
            });

            Assert.Equal("Izayoi.Collections.TimestampedDictionary", exception.Source);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        public void Test2_Construct_with_initial_data(int testPatternIndex)
        {
            var testPatternList = new List<Dictionary<int, string>>()
            {
                new Dictionary<int, string>()
                {
                    { 11, "value11" },
                    { 12, "value12" },
                },
                new Dictionary<int, string>()
                {
                    { 11, "value11" },
                    { 12, "value12" },
                    { 13, "value13" },
                },
                new Dictionary<int, string>()
                {
                    { 11, "value11" },
                    { 12, "value12" },
                    { 13, "value13" },
                    { 14, "value14" },
                },
            };

            Assert.True(testPatternIndex < testPatternList.Count);

            var initialData = testPatternList[testPatternIndex];

            var tsDictionary = new TimestampedDictionary<int, string>(dictionary: initialData);

            Assert.Equal(initialData.Count, tsDictionary.Count);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        public void Test2_Construct_with_capacity_and_initial_data(int testPatternIndex)
        {
            var testPatternList = new List<Dictionary<int, string>>()
            {
                new Dictionary<int, string>()
                {
                    { 11, "value11" },
                    { 12, "value12" },
                },
                new Dictionary<int, string>()
                {
                    { 11, "value11" },
                    { 12, "value12" },
                    { 13, "value13" },
                },
                new Dictionary<int, string>()
                {
                    { 11, "value11" },
                    { 12, "value12" },
                    { 13, "value13" },
                    { 14, "value14" },
                },
            };

            Assert.True(testPatternIndex < testPatternList.Count);

            var initialData = testPatternList[testPatternIndex];

            var tsDictionary = new TimestampedDictionary<int, string>(capacity: initialData.Count, dictionary: initialData);

            Assert.Equal(initialData.Count, tsDictionary.Count);
        }

        [Fact]
        public void Test2_Construct_with_invalid_capacity_and_initial_data()
        {
            var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                var initialData = new Dictionary<int, string>()
                {
                    { 11, "value11" },
                    { 12, "value12" },
                    { 13, "value13" },
                };

                var tsDictionary = new TimestampedDictionary<int, string>(capacity: initialData.Count - 1, initialData);
            });

            Assert.Equal("Izayoi.Collections.TimestampedDictionary", exception.Source);
        }

        #endregion

        #region Property

        [Fact]
        public void Test2_Property_Count()
        {
            var tsDictionary = new TimestampedDictionary<int, string>();

            Assert.Equal(0, tsDictionary.Count);

            {
                bool addResult = tsDictionary.TryAdd(1, "value1");

                Assert.True(addResult);

                Assert.Equal(1, tsDictionary.Count);
            }

            {
                bool addResult = tsDictionary.TryAdd(2, "value2");

                Assert.True(addResult);

                Assert.Equal(2, tsDictionary.Count);
            }

            {
                bool addResult = tsDictionary.TryAdd(3, "value3");

                Assert.True(addResult);

                Assert.Equal(3, tsDictionary.Count);
            }
        }

        #endregion

        #region Add

        [Theory]
        [InlineData(11, "value11")]
        [InlineData(12, "value12")]
        [InlineData(13, "value13")]
        public void Test2_TryAdd_one(int key, string value)
        {
            var tsDictionary = new TimestampedDictionary<int, string>();

            bool addResult = tsDictionary.TryAdd(key, value);

            Assert.True(addResult);

            Assert.Equal(1, tsDictionary.Count);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        public void Test2_TryAdd_any(int testPatternIndex)
        {
            var testPatternList = new List<Dictionary<int, string>>()
            {
                new Dictionary<int, string>()
                {
                    { 11, "value11" },
                    { 12, "value12" },
                },
                new Dictionary<int, string>()
                {
                    { 11, "value11" },
                    { 12, "value12" },
                    { 13, "value13" },
                },
                new Dictionary<int, string>()
                {
                    { 11, "value11" },
                    { 12, "value12" },
                    { 13, "value13" },
                    { 14, "value14" },
                },
                new Dictionary<int, string>()
                {
                    { 11, "value13" },
                    { 12, "value14" },
                    { 13, "value11" },
                    { 14, "value12" },
                },
            };

            Assert.True(testPatternIndex < testPatternList.Count);

            var inputData = testPatternList[testPatternIndex];

            var tsDictionary = new TimestampedDictionary<int, string>();

            foreach (var inParam in inputData)
            {
                bool addResult = tsDictionary.TryAdd(inParam.Key, inParam.Value);

                Assert.True(addResult);
            }

            Assert.Equal(inputData.Count, tsDictionary.Count);
        }

        [Theory]
        [InlineData(11, "value11")]
        public void Test2_TryAdd_duplicate_key(int key, string value)
        {
            var tsDictionary = new TimestampedDictionary<int, string>();

            bool addResult;

            addResult = tsDictionary.TryAdd(key, value);

            Assert.True(addResult);

            Assert.Equal(1, tsDictionary.Count);

            addResult = tsDictionary.TryAdd(key, value);

            Assert.False(addResult);
        }

        #endregion

        #region Add and GetValue

        [Theory]
        [InlineData(11, "value11")]
        [InlineData(12, "value12")]
        [InlineData(13, "value13")]
        [InlineData(14, "value14")]
        [InlineData(15, "value15")]
        public void Test2_TryAdd_and_TryGetValue_one(int key, string value)
        {
            var tsDictionary = new TimestampedDictionary<int, string>();

            {
                bool addResult = tsDictionary.TryAdd(key, value);

                Assert.True(addResult);

                Assert.Equal(1, tsDictionary.Count);
            }

            {
                bool getResult = tsDictionary.TryGetValue(key, out var getValue);

                Assert.True(getResult);

                Assert.Equal(value, getValue);
            }
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        public void Test2_TryAdd_and_TryGetValue_any(int testPatternIndex)
        {
            var testPatternList = new List<Dictionary<int, string>>()
            {
                new Dictionary<int, string>()
                {
                    { 11, "value11" },
                    { 12, "value12" },
                },
                new Dictionary<int, string>()
                {
                    { 11, "value11" },
                    { 12, "value12" },
                    { 13, "value13" },
                },
                new Dictionary<int, string>()
                {
                    { 11, "value11" },
                    { 12, "value12" },
                    { 13, "value13" },
                    { 14, "value14" },
                },
                new Dictionary<int, string>()
                {
                    { 11, "value11" },
                    { 12, "value12" },
                    { 13, "value13" },
                    { 14, "value14" },
                    { 15, "value15" },
                },
                new Dictionary<int, string>()
                {
                    { 11, "value14" },
                    { 12, "value12" },
                    { 13, "value15" },
                    { 14, "value11" },
                    { 15, "value13" },
                },
            };

            Assert.True(testPatternIndex < testPatternList.Count);

            var inputData = testPatternList[testPatternIndex];

            var tsDictionary = new TimestampedDictionary<int, string>(dictionary: inputData);

            Assert.Equal(inputData.Count, tsDictionary.Count);

            foreach (var inParam in inputData)
            {
                bool getResult = tsDictionary.TryGetValue(inParam.Key, out var getValue);

                Assert.True(getResult);

                Assert.Equal(inParam.Value, getValue);
            }
        }

        [Fact]
        public void Test2_TryAdd_and_TryGetValue_on_Capacity_1()
        {
            var tsDictionary = new TimestampedDictionary<int, string>(capacity: 1);

            {
                bool addResult = tsDictionary.TryAdd(11, "value11");

                Assert.True(addResult);

                Assert.Equal(1, tsDictionary.Count);

                bool getResult = tsDictionary.TryGetValue(11, out var getValue);

                Assert.True(getResult);

                Assert.Equal("value11", getValue);
            }

            {
                bool addResult = tsDictionary.TryAdd(12, "value12");

                // key 11 is removed

                Assert.True(addResult);

                Assert.Equal(1, tsDictionary.Count);  // not 2

                bool getResult;

                string? getValue;

                getResult = tsDictionary.TryGetValue(11, out getValue);

                Assert.False(getResult);

                getResult = tsDictionary.TryGetValue(12, out getValue);

                Assert.True(getResult);

                Assert.Equal("value12", getValue);
            }

            tsDictionary.CheckConsistency();
        }

        [Fact]
        public void Test2_TryAdd_and_TryGetValue_on_Capacity_2()
        {
            var tsDictionary = new TimestampedDictionary<int, string>(capacity: 2);

            bool addResult;
            bool getResult;
            string? getValue;

            {
                addResult = tsDictionary.TryAdd(11, "value11");

                Assert.True(addResult);

                addResult = tsDictionary.TryAdd(12, "value12");

                Assert.True(addResult);

                Assert.Equal(2, tsDictionary.Count);
            }

            {
                addResult = tsDictionary.TryAdd(13, "value13");

                // key 11 is removed

                Assert.True(addResult);

                Assert.Equal(2, tsDictionary.Count);  // not 3

                getResult = tsDictionary.TryGetValue(11, out getValue);

                Assert.False(getResult);

                getResult = tsDictionary.TryGetValue(12, out getValue);

                Assert.True(getResult);

                Assert.Equal("value12", getValue);

                getResult = tsDictionary.TryGetValue(13, out getValue);

                Assert.True(getResult);

                Assert.Equal("value13", getValue);
            }

            tsDictionary.CheckConsistency();
        }

        #endregion

        #region Update

        [Fact]
        public async Task Test2_TryUpdate()
        {
            var tsDictionary = new TimestampedDictionary<int, string>();

            long timestamp1 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            await Task.Delay(10);

            {
                bool addResult = tsDictionary.TryAdd(11, "value1");

                Assert.True(addResult);

                Assert.Equal(1, tsDictionary.Count);
            }

            await Task.Delay(10);

            long timestamp2 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            await Task.Delay(10);

            {
                bool getResult = tsDictionary.TryGetData(11, out var getData);

                Assert.True(getResult);

                Assert.NotNull(getData);

                Assert.InRange(getData.Timestamp, timestamp1, timestamp2);

                Assert.Equal(11, getData.Key);

                Assert.Equal("value1", getData.Value);
            }

            {
                bool addResult = tsDictionary.TryAdd(12, "value2");

                Assert.True(addResult);

                Assert.Equal(2, tsDictionary.Count);
            }

            {
                var timestampedKeys = tsDictionary.GetTimestampedKeys();

                Assert.Equal(11, timestampedKeys[0].Key);
                Assert.Equal(12, timestampedKeys[1].Key);
            }

            await Task.Delay(10);

            long timestamp3 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            await Task.Delay(10);

            {
                bool updateResult = tsDictionary.TryUpdate(11, "value3");

                Assert.True(updateResult);

                Assert.Equal(2, tsDictionary.Count);
            }

            await Task.Delay(10);

            long timestamp4 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            await Task.Delay(10);

            {
                bool getResult = tsDictionary.TryGetData(11, out var getData);

                Assert.True(getResult);

                Assert.NotNull(getData);

                Assert.InRange(getData.Timestamp, timestamp3, timestamp4);

                Assert.Equal(11, getData.Key);

                Assert.Equal("value3", getData.Value);
            }

            {
                var timestampedKeys = tsDictionary.GetTimestampedKeys();

                Assert.Equal(12, timestampedKeys[0].Key);
                Assert.Equal(11, timestampedKeys[1].Key);
            }

            tsDictionary.CheckConsistency();
        }

        [Fact]
        public async Task Test2_TryUpdate_with_comparison()
        {
            var tsDictionary = new TimestampedDictionary<int, string>();

            long timestamp1 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            await Task.Delay(10);

            {
                bool addResult = tsDictionary.TryAdd(11, "value1");

                Assert.True(addResult);

                Assert.Equal(1, tsDictionary.Count);
            }

            await Task.Delay(10);

            long timestamp2 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            await Task.Delay(10);

            {
                bool getResult = tsDictionary.TryGetData(11, out var getData);

                Assert.True(getResult);

                Assert.NotNull(getData);

                Assert.InRange(getData.Timestamp, timestamp1, timestamp2);

                Assert.Equal(11, getData.Key);

                Assert.Equal("value1", getData.Value);
            }

            {
                bool addResult = tsDictionary.TryAdd(12, "value2");

                Assert.True(addResult);

                Assert.Equal(2, tsDictionary.Count);
            }

            {
                var timestampedKeys = tsDictionary.GetTimestampedKeys();

                Assert.Equal(11, timestampedKeys[0].Key);
                Assert.Equal(12, timestampedKeys[1].Key);
            }

            await Task.Delay(10);

            long timestamp3 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            await Task.Delay(10);

            {
                bool updateResult = tsDictionary.TryUpdate(11, "value3", comparisonValue: "unknown");

                Assert.False(updateResult);

                Assert.Equal(2, tsDictionary.Count);
            }

            await Task.Delay(10);

            long timestamp4 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            await Task.Delay(10);

            {
                bool updateResult = tsDictionary.TryUpdate(11, "value3", comparisonValue: "value1");

                Assert.True(updateResult);

                Assert.Equal(2, tsDictionary.Count);
            }

            await Task.Delay(10);

            long timestamp5 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            await Task.Delay(10);

            {
                bool getResult = tsDictionary.TryGetData(11, out var getData);

                Assert.True(getResult);

                Assert.NotNull(getData);

                Assert.InRange(getData.Timestamp, timestamp4, timestamp5);

                Assert.Equal(11, getData.Key);

                Assert.Equal("value3", getData.Value);
            }

            {
                var timestampedKeys = tsDictionary.GetTimestampedKeys();

                Assert.Equal(12, timestampedKeys[0].Key);
                Assert.Equal(11, timestampedKeys[1].Key);
            }

            tsDictionary.CheckConsistency();
        }

        #endregion

        #region AddOrUpdate

        [Fact]
        public async Task Test2_AddOrUpdate()
        {
            var tsDictionary = new TimestampedDictionary<int, string>();

            TimestampedDictionaryData<int, string> dictionaryData;

            long timestamp1 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            await Task.Delay(10);

            dictionaryData = tsDictionary.AddOrUpdate(11, "value1");

            await Task.Delay(10);

            long timestamp2 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            await Task.Delay(10);

            {
                Assert.NotNull(dictionaryData);

                Assert.InRange(dictionaryData.Timestamp, timestamp1, timestamp2);

                Assert.Equal(11, dictionaryData.Key);

                Assert.Equal("value1", dictionaryData.Value);

                Assert.Equal(1, tsDictionary.Count);
            }

            await Task.Delay(10);

            long timestamp3 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            await Task.Delay(10);

            dictionaryData = tsDictionary.AddOrUpdate(12, "value2");

            await Task.Delay(10);

            long timestamp4 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            await Task.Delay(10);

            {
                Assert.NotNull(dictionaryData);

                Assert.InRange(dictionaryData.Timestamp, timestamp3, timestamp4);

                Assert.Equal(12, dictionaryData.Key);

                Assert.Equal("value2", dictionaryData.Value);

                Assert.Equal(2, tsDictionary.Count);
            }

            {
                var timestampedKeys = tsDictionary.GetTimestampedKeys();

                Assert.Equal(11, timestampedKeys[0].Key);
                Assert.Equal(12, timestampedKeys[1].Key);
            }

            await Task.Delay(10);

            long timestamp5 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            await Task.Delay(10);

            dictionaryData = tsDictionary.AddOrUpdate(11, "value3");

            await Task.Delay(10);

            long timestamp6 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            await Task.Delay(10);

            {
                Assert.NotNull(dictionaryData);

                Assert.InRange(dictionaryData.Timestamp, timestamp5, timestamp6);

                Assert.Equal(11, dictionaryData.Key);

                Assert.Equal("value3", dictionaryData.Value);

                Assert.Equal(2, tsDictionary.Count);
            }

            {
                var timestampedKeys = tsDictionary.GetTimestampedKeys();

                Assert.Equal(12, timestampedKeys[0].Key);
                Assert.Equal(11, timestampedKeys[1].Key);
            }

            tsDictionary.CheckConsistency();
        }

        #endregion

        #region Remove

        [Theory]
        [InlineData(11, "value11")]
        [InlineData(12, "value12")]
        [InlineData(13, "value13")]
        [InlineData(14, "value14")]
        public void Test2_TryRemove_one(int key, string value)
        {
            var tsDictionary = new TimestampedDictionary<int, string>();

            {
                bool addResult = tsDictionary.TryAdd(key, value);

                Assert.True(addResult);

                Assert.Equal(1, tsDictionary.Count);
            }

            {
                bool removeResult = tsDictionary.TryRemove(key, out var removeValue);

                Assert.True(removeResult);

                Assert.Equal(value, removeValue);

                Assert.Equal(0, tsDictionary.Count);
            }

            {
                bool getResult = tsDictionary.TryGetValue(key, out var getValue);

                Assert.False(getResult);

                Assert.Null(getValue);
            }

            tsDictionary.CheckConsistency();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void Test2_TryRemove_any(int testPatternIndex)
        {
            var testPatternList = new List<(Dictionary<int, string> inputDic, List<int> removeKeys)>()
            {
                (
                    new Dictionary<int, string>()
                    {
                        { 11, "value11" },
                        { 12, "value12" },
                    },
                    new List<int>()
                    {
                        { 12 },
                    }
                ),
                (
                    new Dictionary<int, string>()
                    {
                        { 11, "value11" },
                        { 12, "value12" },
                    },
                    new List<int>()
                    {
                        { 11 },
                    }
                ),
                (
                    new Dictionary<int, string>()
                    {
                        { 11, "value11" },
                        { 12, "value12" },
                    },
                    new List<int>()
                    {
                        { 11 },
                        { 12 },
                    }
                ),
                (
                    new Dictionary<int, string>()
                    {
                        { 11, "value11" },
                        { 12, "value12" },
                        { 13, "value13" },
                    },
                    new List<int>()
                    {
                        { 12 },
                    }
                ),
            };

            Assert.True(testPatternIndex < testPatternList.Count);

            (var inputDic, var removeKeys ) = testPatternList[testPatternIndex];

            var tsDictionary = new TimestampedDictionary<int, string>(dictionary: inputDic);

            Assert.Equal(inputDic.Count, tsDictionary.Count);

            foreach (var removeKey in removeKeys)
            {
                bool removeResult = tsDictionary.TryRemove(removeKey, out var removeValue);

                Assert.True(removeResult);

                //Assert.Equal(n, removeValue);  // @notice
            }

            Assert.Equal(inputDic.Count - removeKeys.Count, tsDictionary.Count);

            foreach (var inParam in inputDic)
            {
                bool getResult = tsDictionary.TryGetValue(inParam.Key, out var getValue);

                if (removeKeys.Exists(key => key == inParam.Key))
                {
                    Assert.False(getResult);

                    Assert.Null(getValue);
                }
                else
                {
                    Assert.True(getResult);

                    Assert.Equal(inParam.Value, getValue);
                }
            }

            tsDictionary.CheckConsistency();
        }

        #endregion

        #region Clear

        [Fact]
        public void Test2_Clear_zero()
        {
            var tsDictionary = new TimestampedDictionary<int, string>();

            Assert.Equal(0, tsDictionary.Count);

            tsDictionary.Clear();

            Assert.Equal(0, tsDictionary.Count);
        }

        [Fact]
        public void Test2_Clear_one()
        {
            var tsDictionary = new TimestampedDictionary<int, string>();

            Assert.Equal(0, tsDictionary.Count);

            {
                bool addResult = tsDictionary.TryAdd(11, "value11");

                Assert.True(addResult);

                Assert.Equal(1, tsDictionary.Count);
            }

            tsDictionary.Clear();

            Assert.Equal(0, tsDictionary.Count);
        }

        [Fact]
        public void Test2_Clear_two()
        {
            var tsDictionary = new TimestampedDictionary<int, string>();

            Assert.Equal(0, tsDictionary.Count);

            {
                bool addResult;

                addResult = tsDictionary.TryAdd(11, "value11");

                Assert.True(addResult);

                Assert.Equal(1, tsDictionary.Count);

                addResult = tsDictionary.TryAdd(12, "value12");

                Assert.True(addResult);

                Assert.Equal(2, tsDictionary.Count);
            }

            tsDictionary.Clear();

            Assert.Equal(0, tsDictionary.Count);
        }

        [Theory]
        [InlineData(-1, -1)]
        [InlineData(-1, 1)]
        [InlineData(-1, 2)]
        [InlineData(1, -1)]
        [InlineData(1, 1)]
        [InlineData(1, 2)]
        public void Test2_Clear_with_newCapacity(int initialCapacity, int newCapacity)
        {
            var tsDictionary = new TimestampedDictionary<int, string>(initialCapacity);

            Assert.Equal(initialCapacity, tsDictionary.Capacity);

            tsDictionary.Clear(newCapacity);

            Assert.Equal(newCapacity, tsDictionary.Capacity);
        }

        [Theory]
        [InlineData(-1, 0)]
        [InlineData(-1, -2)]
        [InlineData(-1, -3)]
        [InlineData(1, 0)]
        [InlineData(1, -2)]
        [InlineData(1, -3)]
        public void Test2_Clear_with_invalid_newCapacity(int initialCapacity, int newCapacity)
        {
            var tsDictionary = new TimestampedDictionary<int, string>(initialCapacity);

            Assert.Equal(initialCapacity, tsDictionary.Capacity);

            var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                tsDictionary.Clear(newCapacity);
            });

            Assert.Equal("Izayoi.Collections.TimestampedDictionary", exception.Source);
        }

        [Fact]
        public async Task Test2_ClearBefore_DateTimeOffset()
        {
            var tsDictionary = new TimestampedDictionary<int, string>();

            Assert.Equal(0, tsDictionary.Count);

            bool addResult;

            {
                addResult = tsDictionary.TryAdd(11, "value11");

                Assert.True(addResult);

                Assert.Equal(1, tsDictionary.Count);

                addResult = tsDictionary.TryAdd(12, "value12");

                Assert.True(addResult);

                Assert.Equal(2, tsDictionary.Count);
            }

            await Task.Delay(10);

            var timestamp = DateTimeOffset.UtcNow;

            await Task.Delay(10);

            {
                addResult = tsDictionary.TryAdd(13, "value13");

                Assert.True(addResult);

                Assert.Equal(3, tsDictionary.Count);
            }

            {
                int removedCount = tsDictionary.ClearBefore(timestamp);

                Assert.Equal(2, removedCount);

                Assert.Equal(1, tsDictionary.Count);

                bool getResult;

                getResult = tsDictionary.TryGetValue(11, out _);

                Assert.False(getResult);

                getResult = tsDictionary.TryGetValue(12, out _);

                Assert.False(getResult);

                getResult = tsDictionary.TryGetValue(13, out var value);

                Assert.True(getResult);

                Assert.Equal("value13", value);
            }
        }

        #endregion

        #region Add and GetData

        [Theory]
        [InlineData(11, "value11")]
        [InlineData(12, "value12")]
        [InlineData(13, "value13")]
        [InlineData(14, "value14")]
        [InlineData(15, "value15")]
        public async Task Test2_TryAdd_and_TryGetData_one(int key, string value)
        {
            var tsDictionary = new TimestampedDictionary<int, string>();

            long timestamp1 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            await Task.Delay(10);

            {
                bool addResult = tsDictionary.TryAdd(key, value);

                Assert.True(addResult);

                Assert.Equal(1, tsDictionary.Count);
            }

            await Task.Delay(10);

            long timestamp2 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            {
                bool getResult = tsDictionary.TryGetData(key, out var getData);

                Assert.True(getResult);

                Assert.NotNull(getData);

                Assert.InRange(getData.Timestamp, timestamp1, timestamp2);

                Assert.Equal(key, getData.Key);

                Assert.Equal(value, getData.Value);
            }
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        public async Task Test2_TryAdd_and_TryGetData_any(int testPatternIndex)
        {
            var testPatternList = new List<Dictionary<int, string>>()
            {
                new Dictionary<int, string>()
                {
                    { 11, "value11" },
                    { 12, "value12" },
                },
                new Dictionary<int, string>()
                {
                    { 11, "value11" },
                    { 12, "value12" },
                    { 13, "value13" },
                },
                new Dictionary<int, string>()
                {
                    { 11, "value11" },
                    { 12, "value12" },
                    { 13, "value13" },
                    { 14, "value14" },
                },
                new Dictionary<int, string>()
                {
                    { 11, "value11" },
                    { 12, "value12" },
                    { 13, "value13" },
                    { 14, "value14" },
                    { 15, "value15" },
                },
                new Dictionary<int, string>()
                {
                    { 11, "value14" },
                    { 12, "value12" },
                    { 13, "value15" },
                    { 14, "value11" },
                    { 15, "value13" },
                },
            };

            Assert.True(testPatternIndex < testPatternList.Count);

            var inputData = testPatternList[testPatternIndex];

            long timestamp1 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            await Task.Delay(10);

            var tsDictionary = new TimestampedDictionary<int, string>(dictionary: inputData);

            await Task.Delay(10);

            long timestamp2 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            Assert.Equal(inputData.Count, tsDictionary.Count);

            foreach (var inParam in inputData)
            {
                bool getResult = tsDictionary.TryGetData(inParam.Key, out var getData);

                Assert.True(getResult);

                Assert.NotNull(getData);

                Assert.InRange(getData.Timestamp, timestamp1, timestamp2);

                Assert.Equal(inParam.Key, getData.Key);

                Assert.Equal(inParam.Value, getData.Value);
            }
        }

        #endregion

        #region Contains Key

        [Fact]
        public void Test2_ContainsKey()
        {
            var tsDictionary = new TimestampedDictionary<int, string>();

            {
                bool containsKey = tsDictionary.ContainsKey(1);

                Assert.False(containsKey);
            }

            {
                bool addResult = tsDictionary.TryAdd(1, "value1");

                Assert.True(addResult);
            }

            {
                bool containsKey = tsDictionary.ContainsKey(1);

                Assert.True(containsKey);
            }

            {
                bool removeResult = tsDictionary.TryRemove(1, out _);

                Assert.True(removeResult);
            }

            {
                bool containsKey = tsDictionary.ContainsKey(1);

                Assert.False(containsKey);
            }
        }

        #endregion

        #region Keys

        [Fact]
        public void Test2_GetKeys_zero()
        {
            var tsDictionary = new TimestampedDictionary<int, string>();

            HashSet<int> keySet = tsDictionary.GetKeys();

            Assert.Empty(keySet);
        }

        [Fact]
        public void Test2_GetKeys_zero_with_remove()
        {
            var tsDictionary = new TimestampedDictionary<int, string>();

            {
                bool addResult = tsDictionary.TryAdd(11, "va1ue11");

                Assert.True(addResult);

                HashSet<int> keySet = tsDictionary.GetKeys();

                Assert.Single(keySet);

                Assert.Equal(11, keySet.First());
            }

            {
                bool removeResult = tsDictionary.TryRemove(11, out _);

                Assert.True(removeResult);

                HashSet<int> keySet = tsDictionary.GetKeys();

                Assert.Empty(keySet);
            }
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void Test2_GetKeys_any(int testPatternIndex)
        {
            var testPatternList = new List<(Dictionary<int, string> inputDic, List<int> removeKeys)>()
            {
                (
                    new Dictionary<int, string>()
                    {
                        { 11, "value11" },
                        { 12, "value12" },
                    },
                    new List<int>()
                    {
                        { 12 },
                    }
                ),
                (
                    new Dictionary<int, string>()
                    {
                        { 11, "value11" },
                        { 12, "value12" },
                    },
                    new List<int>()
                    {
                        { 11 },
                    }
                ),
                (
                    new Dictionary<int, string>()
                    {
                        { 11, "value11" },
                        { 12, "value12" },
                    },
                    new List<int>()
                    {
                        { 11 },
                        { 12 },
                    }
                ),
                (
                    new Dictionary<int, string>()
                    {
                        { 11, "value11" },
                        { 12, "value12" },
                        { 13, "value13" },
                    },
                    new List<int>()
                    {
                        { 12 },
                    }
                ),
            };

            Assert.True(testPatternIndex < testPatternList.Count);

            (var inputDic, var removeKeys) = testPatternList[testPatternIndex];

            var tsDictionary = new TimestampedDictionary<int, string>(dictionary: inputDic);

            Assert.Equal(inputDic.Count, tsDictionary.Count);

            foreach (var removeKey in removeKeys)
            {
                bool removeResult = tsDictionary.TryRemove(removeKey, out var removeValue);

                Assert.True(removeResult);

                //Assert.Equal(n, removeValue);  // @notice
            }

            Assert.Equal(inputDic.Count - removeKeys.Count, tsDictionary.Count);

            HashSet<int> keySet = tsDictionary.GetKeys();

            Assert.Equal(inputDic.Count - removeKeys.Count, keySet.Count);

            foreach (var inParam in inputDic)
            {
                bool contains = keySet.Contains(inParam.Key);

                if (removeKeys.Exists(key => key == inParam.Key))
                {
                    Assert.False(contains);
                }
                else
                {
                    Assert.True(contains);
                }
            }

            tsDictionary.CheckConsistency();
        }

        [Fact]
        public void Test2_GetAddedKeys_any()
        {
            var tsDictionary = new TimestampedDictionary<int, string>();

            {
                bool addResult;

                addResult = tsDictionary.TryAdd(11, "value11");

                Assert.True(addResult);

                addResult = tsDictionary.TryAdd(12, "value12");

                Assert.True(addResult);

                addResult = tsDictionary.TryAdd(13, "value13");

                Assert.True(addResult);

                addResult = tsDictionary.TryAdd(14, "value14");

                Assert.True(addResult);

                Assert.Equal(4, tsDictionary.Count);
            }

            {
                List<int> addedKeys = tsDictionary.GetAddedKeys();

                Assert.Equal(4, addedKeys.Count);

                Assert.Equal(11, addedKeys[0]);
                Assert.Equal(12, addedKeys[1]);
                Assert.Equal(13, addedKeys[2]);
                Assert.Equal(14, addedKeys[3]);
            }

            {
                bool removeResult = tsDictionary.TryRemove(13, out var removeValue);

                Assert.True(removeResult);

                Assert.Equal("value13", removeValue);

                Assert.Equal(3, tsDictionary.Count);
            }

            {
                List<int> addedKeys = tsDictionary.GetAddedKeys();

                Assert.Equal(3, addedKeys.Count);

                Assert.Equal(11, addedKeys[0]);
                Assert.Equal(12, addedKeys[1]);
                Assert.Equal(14, addedKeys[2]);
            }

            tsDictionary.CheckConsistency();
        }

        [Fact]
        public async Task Test2_GetTimestampedKeys_any()
        {
            var tsDictionary = new TimestampedDictionary<int, string>();

            long beginTimestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            {
                bool addResult;

                await Task.Delay(10);

                addResult = tsDictionary.TryAdd(11, "value11");

                Assert.True(addResult);

                await Task.Delay(10);

                addResult = tsDictionary.TryAdd(12, "value12");

                Assert.True(addResult);

                await Task.Delay(10);

                addResult = tsDictionary.TryAdd(13, "value13");

                Assert.True(addResult);

                await Task.Delay(10);

                addResult = tsDictionary.TryAdd(14, "value14");

                Assert.True(addResult);

                Assert.Equal(4, tsDictionary.Count);
            }

            {
                List<TimestampedKey<int>> timestampedKeys = tsDictionary.GetTimestampedKeys();

                Assert.Equal(4, timestampedKeys.Count);

                Assert.Equal(11, timestampedKeys[0].Key);
                Assert.Equal(12, timestampedKeys[1].Key);
                Assert.Equal(13, timestampedKeys[2].Key);
                Assert.Equal(14, timestampedKeys[3].Key);

                Assert.True(timestampedKeys[0].Timestamp > beginTimestamp);
                Assert.True(timestampedKeys[1].Timestamp > timestampedKeys[0].Timestamp);
                Assert.True(timestampedKeys[2].Timestamp > timestampedKeys[1].Timestamp);
                Assert.True(timestampedKeys[3].Timestamp > timestampedKeys[2].Timestamp);
            }

            {
                bool removeResult = tsDictionary.TryRemove(13, out var removeValue);

                Assert.True(removeResult);

                Assert.Equal("value13", removeValue);

                Assert.Equal(3, tsDictionary.Count);
            }

            {
                List<TimestampedKey<int>> timestampedKeys = tsDictionary.GetTimestampedKeys();

                Assert.Equal(3, timestampedKeys.Count);

                Assert.Equal(11, timestampedKeys[0].Key);
                Assert.Equal(12, timestampedKeys[1].Key);
                Assert.Equal(14, timestampedKeys[2].Key);

                Assert.True(timestampedKeys[0].Timestamp > beginTimestamp);
                Assert.True(timestampedKeys[1].Timestamp > timestampedKeys[0].Timestamp);
                Assert.True(timestampedKeys[2].Timestamp > timestampedKeys[1].Timestamp);
            }

            tsDictionary.CheckConsistency();
        }

        #endregion
    }
}