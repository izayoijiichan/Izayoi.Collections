// ----------------------------------------------------------------------
// @Namespace : Izayoi.Collections.ObservableTimestampedDictionary.Test
// @Class     : ObservableTimestampedDictionaryTest1
// ----------------------------------------------------------------------
namespace Izayoi.Collections.ObservableTimestampedDictionary.Test
{
    using Izayoi.Collections;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class ObservableTimestampedDictionaryTest1
    {
        #region Construct

        [Fact]
        public void Test1_Construct()
        {
            using var tsDictionary = new ObservableTimestampedDictionary<int>();

            Assert.Equal(-1, tsDictionary.Capacity);

            Assert.Equal(0, tsDictionary.Count);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(1)]
        [InlineData(2)]
        public void Test1_Construct_with_correct_capacity(int capacity)
        {
            using var tsDictionary = new ObservableTimestampedDictionary<int>(capacity);

            Assert.Equal(capacity, tsDictionary.Capacity);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-2)]
        [InlineData(-3)]
        public void Test1_Construct_with_invalid_capacity(int capacity)
        {
            var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                using var tsDictionary = new ObservableTimestampedDictionary<int>(capacity);
            });

            Assert.Equal("Izayoi.Collections.ObservableTimestampedDictionary", exception.Source);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        public void Test1_Construct_with_initial_data(int testPatternIndex)
        {
            var testPatternList = new List<Dictionary<string, int>>()
            {
                new Dictionary<string, int>()
                {
                    { "key1", 11 },
                    { "key2", 12 },
                },
                new Dictionary<string, int>()
                {
                    { "key1", 11 },
                    { "key2", 12 },
                    { "key3", 13 },
                },
                new Dictionary<string, int>()
                {
                    { "key1", 11 },
                    { "key2", 12 },
                    { "key3", 13 },
                    { "key4", 14 },
                },
            };

            Assert.True(testPatternIndex < testPatternList.Count);

            var initialData = testPatternList[testPatternIndex];

            using var tsDictionary = new ObservableTimestampedDictionary<int>(dictionary: initialData);

            Assert.Equal(initialData.Count, tsDictionary.Count);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        public void Test1_Construct_with_capacity_and_initial_data(int testPatternIndex)
        {
            var testPatternList = new List<Dictionary<string, int>>()
            {
                new Dictionary<string, int>()
                {
                    { "key1", 11 },
                    { "key2", 12 },
                },
                new Dictionary<string, int>()
                {
                    { "key1", 11 },
                    { "key2", 12 },
                    { "key3", 13 },
                },
                new Dictionary<string, int>()
                {
                    { "key1", 11 },
                    { "key2", 12 },
                    { "key3", 13 },
                    { "key4", 14 },
                },
            };

            Assert.True(testPatternIndex < testPatternList.Count);

            var initialData = testPatternList[testPatternIndex];

            using var tsDictionary = new ObservableTimestampedDictionary<int>(capacity: initialData.Count, dictionary: initialData);

            Assert.Equal(initialData.Count, tsDictionary.Count);
        }

        [Fact]
        public void Test1_Construct_with_invalid_capacity_and_initial_data()
        {
            var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                var initialData = new Dictionary<string, int>()
                {
                    { "key1", 11 },
                    { "key2", 12 },
                    { "key3", 13 },
                };

                using var tsDictionary = new ObservableTimestampedDictionary<int>(capacity: initialData.Count - 1, initialData);
            });

            Assert.Equal("Izayoi.Collections.ObservableTimestampedDictionary", exception.Source);
        }

        #endregion

        #region Property

        [Fact]
        public void Test1_Property_Count()
        {
            using var tsDictionary = new ObservableTimestampedDictionary<int>();

            Assert.Equal(0, tsDictionary.Count);

            {
                bool addResult = tsDictionary.TryAdd("key1", 11);

                Assert.True(addResult);

                Assert.Equal(1, tsDictionary.Count);
            }

            {
                bool addResult = tsDictionary.TryAdd("key2", 12);

                Assert.True(addResult);

                Assert.Equal(2, tsDictionary.Count);
            }

            {
                bool addResult = tsDictionary.TryAdd("key3", 13);

                Assert.True(addResult);

                Assert.Equal(3, tsDictionary.Count);
            }
        }

        #endregion

        #region Add

        [Theory]
        [InlineData("key1", 11)]
        [InlineData("key2", 12)]
        [InlineData("key3", 13)]
        public void Test1_TryAdd_one(string key, int value)
        {
            using var tsDictionary = new ObservableTimestampedDictionary<int>();

            bool addResult = tsDictionary.TryAdd(key, value);

            Assert.True(addResult);

            Assert.Equal(1, tsDictionary.Count);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        public void Test1_TryAdd_any(int testPatternIndex)
        {
            var testPatternList = new List<Dictionary<string, int>>()
            {
                new Dictionary<string, int>()
                {
                    { "key1", 11 },
                    { "key2", 12 },
                },
                new Dictionary<string, int>()
                {
                    { "key1", 11 },
                    { "key2", 12 },
                    { "key3", 13 },
                },
                new Dictionary<string, int>()
                {
                    { "key1", 11 },
                    { "key2", 12 },
                    { "key3", 13 },
                    { "key4", 14 },
                },
                new Dictionary<string, int>()
                {
                    { "key1", 13 },
                    { "key2", 14 },
                    { "key3", 11 },
                    { "key4", 12 },
                },
            };

            Assert.True(testPatternIndex < testPatternList.Count);

            var inputData = testPatternList[testPatternIndex];

            using var tsDictionary = new ObservableTimestampedDictionary<int>();

            foreach (var inParam in inputData)
            {
                bool addResult = tsDictionary.TryAdd(inParam.Key, inParam.Value);

                Assert.True(addResult);
            }

            Assert.Equal(inputData.Count, tsDictionary.Count);
        }

        [Theory]
        [InlineData("key1", 11)]
        public void Test1_TryAdd_duplicate_key(string key, int value)
        {
            using var tsDictionary = new ObservableTimestampedDictionary<int>();

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
        [InlineData("key1", 11)]
        [InlineData("key2", 12)]
        [InlineData("key3", 13)]
        [InlineData("key4", 14)]
        [InlineData("key5", 15)]
        public void Test1_TryAdd_and_TryGetValue_one(string key, int value)
        {
            using var tsDictionary = new ObservableTimestampedDictionary<int>();

            {
                bool addResult = tsDictionary.TryAdd(key, value);

                Assert.True(addResult);

                Assert.Equal(1, tsDictionary.Count);
            }

            {
                bool getResult = tsDictionary.TryGetValue(key, out int getValue);

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
        public void Test1_TryAdd_and_TryGetValue_any(int testPatternIndex)
        {
            var testPatternList = new List<Dictionary<string, int>>()
            {
                new Dictionary<string, int>()
                {
                    { "key1", 11 },
                    { "key2", 12 },
                },
                new Dictionary<string, int>()
                {
                    { "key1", 11 },
                    { "key2", 12 },
                    { "key3", 13 },
                },
                new Dictionary<string, int>()
                {
                    { "key1", 11 },
                    { "key2", 12 },
                    { "key3", 13 },
                    { "key4", 14 },
                },
                new Dictionary<string, int>()
                {
                    { "key1", 11 },
                    { "key2", 12 },
                    { "key3", 13 },
                    { "key4", 14 },
                    { "key5", 15 },
                },
                new Dictionary<string, int>()
                {
                    { "key1", 14 },
                    { "key2", 12 },
                    { "key3", 15 },
                    { "key4", 11 },
                    { "key5", 13 },
                },
            };

            Assert.True(testPatternIndex < testPatternList.Count);

            var inputData = testPatternList[testPatternIndex];

            using var tsDictionary = new ObservableTimestampedDictionary<int>(dictionary: inputData);

            Assert.Equal(inputData.Count, tsDictionary.Count);

            foreach (var inParam in inputData)
            {
                bool getResult = tsDictionary.TryGetValue(inParam.Key, out int getValue);

                Assert.True(getResult);

                Assert.Equal(inParam.Value, getValue);
            }
        }

        [Fact]
        public void Test1_TryAdd_and_TryGetValue_on_Capacity_1()
        {
            using var tsDictionary = new ObservableTimestampedDictionary<int>(capacity: 1);

            {
                bool addResult = tsDictionary.TryAdd("key1", 11);

                Assert.True(addResult);

                Assert.Equal(1, tsDictionary.Count);

                bool getResult = tsDictionary.TryGetValue("key1", out var getValue);

                Assert.True(getResult);

                Assert.Equal(11, getValue);
            }

            {
                bool addResult = tsDictionary.TryAdd("key2", 12);

                // key1 is removed

                Assert.True(addResult);

                Assert.Equal(1, tsDictionary.Count);  // not 2

                bool getResult;

                int getValue;

                getResult = tsDictionary.TryGetValue("key1", out getValue);

                Assert.False(getResult);

                getResult = tsDictionary.TryGetValue("key2", out getValue);

                Assert.True(getResult);

                Assert.Equal(12, getValue);
            }

            tsDictionary.CheckConsistency();
        }

        [Fact]
        public void Test1_TryAdd_and_TryGetValue_on_Capacity_2()
        {
            using var tsDictionary = new ObservableTimestampedDictionary<int>(capacity: 2);

            bool addResult;
            bool getResult;
            int getValue;

            {
                addResult = tsDictionary.TryAdd("key1", 11);

                Assert.True(addResult);

                addResult = tsDictionary.TryAdd("key2", 12);

                Assert.True(addResult);

                Assert.Equal(2, tsDictionary.Count);
            }

            {
                addResult = tsDictionary.TryAdd("key3", 13);

                // key1 is removed

                Assert.True(addResult);

                Assert.Equal(2, tsDictionary.Count);  // not 3

                getResult = tsDictionary.TryGetValue("key1", out getValue);

                Assert.False(getResult);

                getResult = tsDictionary.TryGetValue("key2", out getValue);

                Assert.True(getResult);

                Assert.Equal(12, getValue);

                getResult = tsDictionary.TryGetValue("key3", out getValue);

                Assert.True(getResult);

                Assert.Equal(13, getValue);
            }

            tsDictionary.CheckConsistency();
        }

        #endregion

        #region Update

        [Fact]
        public async Task Test1_TryUpdate()
        {
            using var tsDictionary = new ObservableTimestampedDictionary<int>();

            long timestamp1 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            await Task.Delay(10);

            {
                bool addResult = tsDictionary.TryAdd("key1", 11);

                Assert.True(addResult);

                Assert.Equal(1, tsDictionary.Count);
            }

            await Task.Delay(10);

            long timestamp2 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            await Task.Delay(10);

            {
                bool getResult = tsDictionary.TryGetData("key1", out var getData);

                Assert.True(getResult);

                Assert.NotNull(getData);

                Assert.InRange(getData.Timestamp, timestamp1, timestamp2);

                Assert.Equal("key1", getData.Key);

                Assert.Equal(11, getData.Value);
            }

            {
                bool addResult = tsDictionary.TryAdd("key2", 12);

                Assert.True(addResult);

                Assert.Equal(2, tsDictionary.Count);
            }

            {
                var timestampedKeys = tsDictionary.GetTimestampedKeys();

                Assert.Equal("key1", timestampedKeys[0].Key);
                Assert.Equal("key2", timestampedKeys[1].Key);
            }

            await Task.Delay(10);

            long timestamp3 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            await Task.Delay(10);

            {
                bool updateResult = tsDictionary.TryUpdate("key1", 13);

                Assert.True(updateResult);

                Assert.Equal(2, tsDictionary.Count);
            }

            await Task.Delay(10);

            long timestamp4 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            await Task.Delay(10);

            {
                bool getResult = tsDictionary.TryGetData("key1", out var getData);

                Assert.True(getResult);

                Assert.NotNull(getData);

                Assert.InRange(getData.Timestamp, timestamp3, timestamp4);

                Assert.Equal("key1", getData.Key);

                Assert.Equal(13, getData.Value);
            }

            {
                var timestampedKeys = tsDictionary.GetTimestampedKeys();

                Assert.Equal("key2", timestampedKeys[0].Key);
                Assert.Equal("key1", timestampedKeys[1].Key);
            }

            tsDictionary.CheckConsistency();
        }

        [Fact]
        public async Task Test1_TryUpdate_with_comparison()
        {
            using var tsDictionary = new ObservableTimestampedDictionary<int>();

            long timestamp1 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            await Task.Delay(10);

            {
                bool addResult = tsDictionary.TryAdd("key1", 11);

                Assert.True(addResult);

                Assert.Equal(1, tsDictionary.Count);
            }

            await Task.Delay(10);

            long timestamp2 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            await Task.Delay(10);

            {
                bool getResult = tsDictionary.TryGetData("key1", out var getData);

                Assert.True(getResult);

                Assert.NotNull(getData);

                Assert.InRange(getData.Timestamp, timestamp1, timestamp2);

                Assert.Equal("key1", getData.Key);

                Assert.Equal(11, getData.Value);
            }

            {
                bool addResult = tsDictionary.TryAdd("key2", 12);

                Assert.True(addResult);

                Assert.Equal(2, tsDictionary.Count);
            }

            {
                var timestampedKeys = tsDictionary.GetTimestampedKeys();

                Assert.Equal("key1", timestampedKeys[0].Key);
                Assert.Equal("key2", timestampedKeys[1].Key);
            }

            await Task.Delay(10);

            long timestamp3 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            await Task.Delay(10);

            {
                bool updateResult = tsDictionary.TryUpdate("key1", 13, comparisonValue: 99);

                Assert.False(updateResult);

                Assert.Equal(2, tsDictionary.Count);
            }

            await Task.Delay(10);

            long timestamp4 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            await Task.Delay(10);

            {
                bool updateResult = tsDictionary.TryUpdate("key1", 13, comparisonValue: 11);

                Assert.True(updateResult);

                Assert.Equal(2, tsDictionary.Count);
            }

            await Task.Delay(10);

            long timestamp5 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            await Task.Delay(10);

            {
                bool getResult = tsDictionary.TryGetData("key1", out var getData);

                Assert.True(getResult);

                Assert.NotNull(getData);

                Assert.InRange(getData.Timestamp, timestamp4, timestamp5);

                Assert.Equal("key1", getData.Key);

                Assert.Equal(13, getData.Value);
            }

            {
                var timestampedKeys = tsDictionary.GetTimestampedKeys();

                Assert.Equal("key2", timestampedKeys[0].Key);
                Assert.Equal("key1", timestampedKeys[1].Key);
            }

            tsDictionary.CheckConsistency();
        }

        #endregion

        #region AddOrUpdate

        [Fact]
        public async Task Test1_AddOrUpdate()
        {
            using var tsDictionary = new ObservableTimestampedDictionary<int>();

            TimestampedDictionaryData<int> dictionaryData;

            long timestamp1 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            await Task.Delay(10);

            dictionaryData = tsDictionary.AddOrUpdate("key1", 11);

            await Task.Delay(10);

            long timestamp2 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            await Task.Delay(10);

            {
                Assert.NotNull(dictionaryData);

                Assert.InRange(dictionaryData.Timestamp, timestamp1, timestamp2);

                Assert.Equal("key1", dictionaryData.Key);

                Assert.Equal(11, dictionaryData.Value);

                Assert.Equal(1, tsDictionary.Count);
            }

            await Task.Delay(10);

            long timestamp3 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            await Task.Delay(10);

            dictionaryData = tsDictionary.AddOrUpdate("key2", 12);

            await Task.Delay(10);

            long timestamp4 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            await Task.Delay(10);

            {
                Assert.NotNull(dictionaryData);

                Assert.InRange(dictionaryData.Timestamp, timestamp3, timestamp4);

                Assert.Equal("key2", dictionaryData.Key);

                Assert.Equal(12, dictionaryData.Value);

                Assert.Equal(2, tsDictionary.Count);
            }

            {
                var timestampedKeys = tsDictionary.GetTimestampedKeys();

                Assert.Equal("key1", timestampedKeys[0].Key);
                Assert.Equal("key2", timestampedKeys[1].Key);
            }

            await Task.Delay(10);

            long timestamp5 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            await Task.Delay(10);

            dictionaryData = tsDictionary.AddOrUpdate("key1", 13);

            await Task.Delay(10);

            long timestamp6 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            await Task.Delay(10);

            {
                Assert.NotNull(dictionaryData);

                Assert.InRange(dictionaryData.Timestamp, timestamp5, timestamp6);

                Assert.Equal("key1", dictionaryData.Key);

                Assert.Equal(13, dictionaryData.Value);

                Assert.Equal(2, tsDictionary.Count);
            }

            {
                var timestampedKeys = tsDictionary.GetTimestampedKeys();

                Assert.Equal("key2", timestampedKeys[0].Key);
                Assert.Equal("key1", timestampedKeys[1].Key);
            }

            tsDictionary.CheckConsistency();
        }

        #endregion

        #region Remove

        [Theory]
        [InlineData("key1", 11)]
        [InlineData("key2", 12)]
        [InlineData("key3", 13)]
        [InlineData("key4", 14)]
        public void Test1_TryRemove_one(string key, int value)
        {
            using var tsDictionary = new ObservableTimestampedDictionary<int>();

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
                bool getResult = tsDictionary.TryGetValue(key, out int getValue);

                Assert.False(getResult);

                Assert.Equal(0, getValue);
            }

            tsDictionary.CheckConsistency();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void Test1_TryRemove_any(int testPatternIndex)
        {
            var testPatternList = new List<(Dictionary<string, int> inputDic, List<string> removeKeys)>()
            {
                (
                    new Dictionary<string, int>()
                    {
                        { "key1", 11 },
                        { "key2", 12 },
                    },
                    new List<string>()
                    {
                        { "key2" },
                    }
                ),
                (
                    new Dictionary<string, int>()
                    {
                        { "key1", 11 },
                        { "key2", 12 },
                    },
                    new List<string>()
                    {
                        { "key1" },
                    }
                ),
                (
                    new Dictionary<string, int>()
                    {
                        { "key1", 11 },
                        { "key2", 12 },
                    },
                    new List<string>()
                    {
                        { "key1" },
                        { "key2" }
                    }
                ),
                (
                    new Dictionary<string, int>()
                    {
                        { "key1", 11 },
                        { "key2", 12 },
                        { "key3", 13 },
                    },
                    new List<string>()
                    {
                        { "key2" }
                    }
                ),
            };

            Assert.True(testPatternIndex < testPatternList.Count);

            (var inputDic, var removeKeys ) = testPatternList[testPatternIndex];

            using var tsDictionary = new ObservableTimestampedDictionary<int>(dictionary: inputDic);

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
                bool getResult = tsDictionary.TryGetValue(inParam.Key, out int getValue);

                if (removeKeys.Exists(key => key == inParam.Key))
                {
                    Assert.False(getResult);

                    Assert.Equal(0, getValue);
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
        public void Test1_Clear_zero()
        {
            using var tsDictionary = new ObservableTimestampedDictionary<int>();

            Assert.Equal(0, tsDictionary.Count);

            tsDictionary.Clear();

            Assert.Equal(0, tsDictionary.Count);
        }

        [Fact]
        public void Test1_Clear_one()
        {
            using var tsDictionary = new ObservableTimestampedDictionary<int>();

            Assert.Equal(0, tsDictionary.Count);

            {
                bool addResult = tsDictionary.TryAdd("key1", 11);

                Assert.True(addResult);

                Assert.Equal(1, tsDictionary.Count);
            }

            tsDictionary.Clear();

            Assert.Equal(0, tsDictionary.Count);
        }

        [Fact]
        public void Test1_Clear_two()
        {
            using var tsDictionary = new ObservableTimestampedDictionary<int>();

            Assert.Equal(0, tsDictionary.Count);

            {
                bool addResult;

                addResult = tsDictionary.TryAdd("key1", 11);

                Assert.True(addResult);

                Assert.Equal(1, tsDictionary.Count);

                addResult = tsDictionary.TryAdd("key2", 12);

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
        public void Test1_Clear_with_newCapacity(int initialCapacity, int newCapacity)
        {
            using var tsDictionary = new ObservableTimestampedDictionary<int>(initialCapacity);

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
        public void Test1_Clear_with_invalid_newCapacity(int initialCapacity, int newCapacity)
        {
            using var tsDictionary = new ObservableTimestampedDictionary<int>(initialCapacity);

            Assert.Equal(initialCapacity, tsDictionary.Capacity);

            var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                tsDictionary.Clear(newCapacity);
            });

            Assert.Equal("Izayoi.Collections.ObservableTimestampedDictionary", exception.Source);
        }

        [Fact]
        public async Task Test1_ClearBefore_DateTimeOffset()
        {
            using var tsDictionary = new ObservableTimestampedDictionary<int>();

            Assert.Equal(0, tsDictionary.Count);

            bool addResult;

            {
                addResult = tsDictionary.TryAdd("key1", 11);

                Assert.True(addResult);

                Assert.Equal(1, tsDictionary.Count);

                addResult = tsDictionary.TryAdd("key2", 12);

                Assert.True(addResult);

                Assert.Equal(2, tsDictionary.Count);
            }

            await Task.Delay(10);

            var timestamp = DateTimeOffset.UtcNow;

            await Task.Delay(10);

            {
                addResult = tsDictionary.TryAdd("key3", 13);

                Assert.True(addResult);

                Assert.Equal(3, tsDictionary.Count);
            }

            {
                int removedCount = tsDictionary.ClearBefore(timestamp);

                Assert.Equal(2, removedCount);

                Assert.Equal(1, tsDictionary.Count);

                bool getResult;

                getResult = tsDictionary.TryGetValue("key1", out _);

                Assert.False(getResult);

                getResult = tsDictionary.TryGetValue("key2", out _);

                Assert.False(getResult);

                getResult = tsDictionary.TryGetValue("key3", out var value);

                Assert.True(getResult);

                Assert.Equal(13, value);
            }
        }

        #endregion

        #region Add and GetData

        [Theory]
        [InlineData("key1", 11)]
        [InlineData("key2", 12)]
        [InlineData("key3", 13)]
        [InlineData("key4", 14)]
        [InlineData("key5", 15)]
        public async Task Test1_TryAdd_and_TryGetData_one(string key, int value)
        {
            using var tsDictionary = new ObservableTimestampedDictionary<int>();

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
        public async Task Test1_TryAdd_and_TryGetData_any(int testPatternIndex)
        {
            var testPatternList = new List<Dictionary<string, int>>()
            {
                new Dictionary<string, int>()
                {
                    { "key1", 11 },
                    { "key2", 12 },
                },
                new Dictionary<string, int>()
                {
                    { "key1", 11 },
                    { "key2", 12 },
                    { "key3", 13 },
                },
                new Dictionary<string, int>()
                {
                    { "key1", 11 },
                    { "key2", 12 },
                    { "key3", 13 },
                    { "key4", 14 },
                },
                new Dictionary<string, int>()
                {
                    { "key1", 11 },
                    { "key2", 12 },
                    { "key3", 13 },
                    { "key4", 14 },
                    { "key5", 15 },
                },
                new Dictionary<string, int>()
                {
                    { "key1", 14 },
                    { "key2", 12 },
                    { "key3", 15 },
                    { "key4", 11 },
                    { "key5", 13 },
                },
            };

            Assert.True(testPatternIndex < testPatternList.Count);

            var inputData = testPatternList[testPatternIndex];

            long timestamp1 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            await Task.Delay(10);

            using var tsDictionary = new ObservableTimestampedDictionary<int>(dictionary: inputData);

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
        public void Test1_ContainsKey()
        {
            using var tsDictionary = new ObservableTimestampedDictionary<int>();

            {
                bool containsKey = tsDictionary.ContainsKey("key1");

                Assert.False(containsKey);
            }

            {
                bool addResult = tsDictionary.TryAdd("key1", 1);

                Assert.True(addResult);
            }

            {
                bool containsKey = tsDictionary.ContainsKey("key1");

                Assert.True(containsKey);
            }

            {
                bool removeResult = tsDictionary.TryRemove("key1", out _);

                Assert.True(removeResult);
            }

            {
                bool containsKey = tsDictionary.ContainsKey("key1");

                Assert.False(containsKey);
            }
        }

        #endregion

        #region Keys

        [Fact]
        public void Test1_GetKeys_zero()
        {
            using var tsDictionary = new ObservableTimestampedDictionary<int>();

            HashSet<string> keySet = tsDictionary.GetKeys();

            Assert.Empty(keySet);
        }

        [Fact]
        public void Test1_GetKeys_zero_with_remove()
        {
            using var tsDictionary = new ObservableTimestampedDictionary<int>();

            {
                bool addResult = tsDictionary.TryAdd("key1", 11);

                Assert.True(addResult);

                HashSet<string> keySet = tsDictionary.GetKeys();

                Assert.Single(keySet);

                Assert.Equal("key1", keySet.First());
            }

            {
                bool removeResult = tsDictionary.TryRemove("key1", out _);

                Assert.True(removeResult);

                HashSet<string> keySet = tsDictionary.GetKeys();

                Assert.Empty(keySet);
            }
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void Test1_GetKeys_any(int testPatternIndex)
        {
            var testPatternList = new List<(Dictionary<string, int> inputDic, List<string> removeKeys)>()
            {
                (
                    new Dictionary<string, int>()
                    {
                        { "key1", 11 },
                        { "key2", 12 },
                    },
                    new List<string>()
                    {
                        { "key2" },
                    }
                ),
                (
                    new Dictionary<string, int>()
                    {
                        { "key1", 11 },
                        { "key2", 12 },
                    },
                    new List<string>()
                    {
                        { "key1" },
                    }
                ),
                (
                    new Dictionary<string, int>()
                    {
                        { "key1", 11 },
                        { "key2", 12 },
                    },
                    new List<string>()
                    {
                        { "key1" },
                        { "key2" }
                    }
                ),
                (
                    new Dictionary<string, int>()
                    {
                        { "key1", 11 },
                        { "key2", 12 },
                        { "key3", 13 },
                    },
                    new List<string>()
                    {
                        { "key2" }
                    }
                ),
            };

            Assert.True(testPatternIndex < testPatternList.Count);

            (var inputDic, var removeKeys) = testPatternList[testPatternIndex];

            using var tsDictionary = new ObservableTimestampedDictionary<int>(dictionary: inputDic);

            Assert.Equal(inputDic.Count, tsDictionary.Count);

            foreach (var removeKey in removeKeys)
            {
                bool removeResult = tsDictionary.TryRemove(removeKey, out var removeValue);

                Assert.True(removeResult);

                //Assert.Equal(n, removeValue);  // @notice
            }

            Assert.Equal(inputDic.Count - removeKeys.Count, tsDictionary.Count);

            HashSet<string> keySet = tsDictionary.GetKeys();

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
        public void Test1_GetAddedKeys_any()
        {
            using var tsDictionary = new ObservableTimestampedDictionary<int>();

            {
                bool addResult;

                addResult = tsDictionary.TryAdd("key1", 11);

                Assert.True(addResult);

                addResult = tsDictionary.TryAdd("key2", 12);

                Assert.True(addResult);

                addResult = tsDictionary.TryAdd("key3", 13);

                Assert.True(addResult);

                addResult = tsDictionary.TryAdd("key4", 14);

                Assert.True(addResult);

                Assert.Equal(4, tsDictionary.Count);
            }

            {
                List<string> addedKeys = tsDictionary.GetAddedKeys();

                Assert.Equal(4, addedKeys.Count);

                Assert.Equal("key1", addedKeys[0]);
                Assert.Equal("key2", addedKeys[1]);
                Assert.Equal("key3", addedKeys[2]);
                Assert.Equal("key4", addedKeys[3]);
            }

            {
                bool removeResult = tsDictionary.TryRemove("key3", out var removeValue);

                Assert.True(removeResult);

                Assert.Equal(13, removeValue);

                Assert.Equal(3, tsDictionary.Count);
            }

            {
                List<string> addedKeys = tsDictionary.GetAddedKeys();

                Assert.Equal(3, addedKeys.Count);

                Assert.Equal("key1", addedKeys[0]);
                Assert.Equal("key2", addedKeys[1]);
                Assert.Equal("key4", addedKeys[2]);
            }

            tsDictionary.CheckConsistency();
        }

        [Fact]
        public async Task Test1_GetTimestampedKeys_any()
        {
            using var tsDictionary = new ObservableTimestampedDictionary<int>();

            long beginTimestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            {
                bool addResult;

                await Task.Delay(10);

                addResult = tsDictionary.TryAdd("key1", 11);

                Assert.True(addResult);

                await Task.Delay(10);

                addResult = tsDictionary.TryAdd("key2", 12);

                Assert.True(addResult);

                await Task.Delay(10);

                addResult = tsDictionary.TryAdd("key3", 13);

                Assert.True(addResult);

                await Task.Delay(10);

                addResult = tsDictionary.TryAdd("key4", 14);

                Assert.True(addResult);

                Assert.Equal(4, tsDictionary.Count);
            }

            {
                List<TimestampedKey> timestampedKeys = tsDictionary.GetTimestampedKeys();

                Assert.Equal(4, timestampedKeys.Count);

                Assert.Equal("key1", timestampedKeys[0].Key);
                Assert.Equal("key2", timestampedKeys[1].Key);
                Assert.Equal("key3", timestampedKeys[2].Key);
                Assert.Equal("key4", timestampedKeys[3].Key);

                Assert.True(timestampedKeys[0].Timestamp > beginTimestamp);
                Assert.True(timestampedKeys[1].Timestamp > timestampedKeys[0].Timestamp);
                Assert.True(timestampedKeys[2].Timestamp > timestampedKeys[1].Timestamp);
                Assert.True(timestampedKeys[3].Timestamp > timestampedKeys[2].Timestamp);
            }

            {
                bool removeResult = tsDictionary.TryRemove("key3", out var removeValue);

                Assert.True(removeResult);

                Assert.Equal(13, removeValue);

                Assert.Equal(3, tsDictionary.Count);
            }

            {
                List<TimestampedKey> timestampedKeys = tsDictionary.GetTimestampedKeys();

                Assert.Equal(3, timestampedKeys.Count);

                Assert.Equal("key1", timestampedKeys[0].Key);
                Assert.Equal("key2", timestampedKeys[1].Key);
                Assert.Equal("key4", timestampedKeys[2].Key);

                Assert.True(timestampedKeys[0].Timestamp > beginTimestamp);
                Assert.True(timestampedKeys[1].Timestamp > timestampedKeys[0].Timestamp);
                Assert.True(timestampedKeys[2].Timestamp > timestampedKeys[1].Timestamp);
            }

            tsDictionary.CheckConsistency();
        }

        #endregion
    }
}