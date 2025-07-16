// ----------------------------------------------------------------------
// @Namespace : Izayoi.Collections.TimestampedDictionary.Test
// @Class     : TimestampedDictionaryTest1
// ----------------------------------------------------------------------
namespace Izayoi.Collections.TimestampedDictionary.Test
{
    using Izayoi.Collections;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using TUnit.Core;

    public class TimestampedDictionaryTest1
    {
        #region Construct

        [Test]
        public async ValueTask Test1_Construct()
        {
            var tsDictionary = new TimestampedDictionary<int>();

            await Assert.That(tsDictionary.Capacity).IsEqualTo(-1);

            await Assert.That(tsDictionary.Count).IsEqualTo(0);
        }

        [Test]
        [Arguments(-1)]
        [Arguments(1)]
        [Arguments(2)]
        public async ValueTask Test1_Construct_with_correct_capacity(int capacity)
        {
            var tsDictionary = new TimestampedDictionary<int>(capacity);

            await Assert.That(tsDictionary.Capacity).IsEqualTo(capacity);
        }

        [Test]
        [Arguments(0)]
        [Arguments(-2)]
        [Arguments(-3)]
        public async ValueTask Test1_Construct_with_invalid_capacity(int capacity)
        {
            var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                var tsDictionary = new TimestampedDictionary<int>(capacity);
            });

            await Assert.That(exception.Source).IsEqualTo("Izayoi.Collections.TimestampedDictionary");
        }

        [Test]
        [Arguments(0)]
        [Arguments(1)]
        [Arguments(2)]
        public async ValueTask Test1_Construct_with_initial_data(int testPatternIndex)
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

            await Assert.That(testPatternList).HasCount().GreaterThan(testPatternIndex);

            var initialData = testPatternList[testPatternIndex];

            var tsDictionary = new TimestampedDictionary<int>(dictionary: initialData);

            await Assert.That(tsDictionary.Count).IsEqualTo(initialData.Count);
        }

        [Test]
        [Arguments(0)]
        [Arguments(1)]
        [Arguments(2)]
        public async ValueTask Test1_Construct_with_capacity_and_initial_data(int testPatternIndex)
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

            await Assert.That(testPatternList).HasCount().GreaterThan(testPatternIndex);

            var initialData = testPatternList[testPatternIndex];

            var tsDictionary = new TimestampedDictionary<int>(capacity: initialData.Count, dictionary: initialData);

            await Assert.That(tsDictionary.Count).IsEqualTo(initialData.Count);
        }

        [Test]
        public async ValueTask Test1_Construct_with_invalid_capacity_and_initial_data()
        {
            var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                var initialData = new Dictionary<string, int>()
                {
                    { "key1", 11 },
                    { "key2", 12 },
                    { "key3", 13 },
                };

                var tsDictionary = new TimestampedDictionary<int>(capacity: initialData.Count - 1, initialData);
            });

            await Assert.That(exception.Source).IsEqualTo("Izayoi.Collections.TimestampedDictionary");
        }

        #endregion

        #region Property

        [Test]
        public async ValueTask Test1_Property_Count()
        {
            var tsDictionary = new TimestampedDictionary<int>();

            await Assert.That(tsDictionary.Count).IsEqualTo(0);

            {
                bool addResult = tsDictionary.TryAdd("key1", 11);

                await Assert.That(addResult).IsTrue();

                await Assert.That(tsDictionary.Count).IsEqualTo(1);
            }

            {
                bool addResult = tsDictionary.TryAdd("key2", 12);

                await Assert.That(addResult).IsTrue();

                await Assert.That(tsDictionary.Count).IsEqualTo(2);
            }

            {
                bool addResult = tsDictionary.TryAdd("key3", 13);

                await Assert.That(addResult).IsTrue();

                await Assert.That(tsDictionary.Count).IsEqualTo(3);
            }
        }

        #endregion

        #region Add

        [Test]
        [Arguments("key1", 11)]
        [Arguments("key2", 12)]
        [Arguments("key3", 13)]
        public async ValueTask Test1_TryAdd_one(string key, int value)
        {
            var tsDictionary = new TimestampedDictionary<int>();

            bool addResult = tsDictionary.TryAdd(key, value);

            await Assert.That(addResult).IsTrue();

            await Assert.That(tsDictionary.Count).IsEqualTo(1);
        }

        [Test]
        [Arguments(0)]
        [Arguments(1)]
        [Arguments(2)]
        public async ValueTask Test1_TryAdd_any(int testPatternIndex)
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

            await Assert.That(testPatternList).HasCount().GreaterThan(testPatternIndex);

            var inputData = testPatternList[testPatternIndex];

            var tsDictionary = new TimestampedDictionary<int>();

            foreach (var inParam in inputData)
            {
                bool addResult = tsDictionary.TryAdd(inParam.Key, inParam.Value);

                await Assert.That(addResult).IsTrue();
            }

            await Assert.That(tsDictionary.Count).IsEqualTo(inputData.Count);
        }

        [Test]
        [Arguments("key1", 11)]
        public async ValueTask Test1_TryAdd_duplicate_key(string key, int value)
        {
            var tsDictionary = new TimestampedDictionary<int>();

            bool addResult;

            addResult = tsDictionary.TryAdd(key, value);

            await Assert.That(addResult).IsTrue();

            await Assert.That(tsDictionary.Count).IsEqualTo(1);

            addResult = tsDictionary.TryAdd(key, value);

            await Assert.That(addResult).IsFalse();
        }

        #endregion

        #region Add and GetValue

        [Test]
        [Arguments("key1", 11)]
        [Arguments("key2", 12)]
        [Arguments("key3", 13)]
        [Arguments("key4", 14)]
        [Arguments("key5", 15)]
        public async ValueTask Test1_TryAdd_and_TryGetValue_one(string key, int value)
        {
            var tsDictionary = new TimestampedDictionary<int>();

            {
                bool addResult = tsDictionary.TryAdd(key, value);

                await Assert.That(addResult).IsTrue();

                await Assert.That(tsDictionary.Count).IsEqualTo(1);
            }

            {
                bool getResult = tsDictionary.TryGetValue(key, out int getValue);

                await Assert.That(getResult).IsTrue();

                await Assert.That(getValue).IsEqualTo(value);
            }
        }

        [Test]
        [Arguments(0)]
        [Arguments(1)]
        [Arguments(2)]
        [Arguments(3)]
        [Arguments(4)]
        public async ValueTask Test1_TryAdd_and_TryGetValue_any(int testPatternIndex)
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

            await Assert.That(testPatternList).HasCount().GreaterThan(testPatternIndex);

            var inputData = testPatternList[testPatternIndex];

            var tsDictionary = new TimestampedDictionary<int>(dictionary: inputData);

            await Assert.That(tsDictionary.Count).IsEqualTo(inputData.Count);

            foreach (var inParam in inputData)
            {
                bool getResult = tsDictionary.TryGetValue(inParam.Key, out int getValue);

                await Assert.That(getResult).IsTrue();

                await Assert.That(getValue).IsEqualTo(inParam.Value);
            }
        }

        [Test]
        public async ValueTask Test1_TryAdd_and_TryGetValue_on_Capacity_1()
        {
            var tsDictionary = new TimestampedDictionary<int>(capacity: 1);

            {
                bool addResult = tsDictionary.TryAdd("key1", 11);

                await Assert.That(addResult).IsTrue();

                await Assert.That(tsDictionary.Count).IsEqualTo(1);

                bool getResult = tsDictionary.TryGetValue("key1", out var getValue);

                await Assert.That(getResult).IsTrue();

                await Assert.That(getValue).IsEqualTo(11);
            }

            {
                bool addResult = tsDictionary.TryAdd("key2", 12);

                // key1 is removed

                await Assert.That(addResult).IsTrue();

                await Assert.That(tsDictionary.Count).IsEqualTo(1);  // not 2

                bool getResult;

                int getValue;

                getResult = tsDictionary.TryGetValue("key1", out getValue);

                await Assert.That(getResult).IsFalse();

                getResult = tsDictionary.TryGetValue("key2", out getValue);

                await Assert.That(getResult).IsTrue();

                await Assert.That(getValue).IsEqualTo(12);
            }

            tsDictionary.CheckConsistency();
        }

        [Test]
        public async ValueTask Test1_TryAdd_and_TryGetValue_on_Capacity_2()
        {
            var tsDictionary = new TimestampedDictionary<int>(capacity: 2);

            bool addResult;
            bool getResult;
            int getValue;

            {
                addResult = tsDictionary.TryAdd("key1", 11);

                await Assert.That(addResult).IsTrue();

                addResult = tsDictionary.TryAdd("key2", 12);

                await Assert.That(addResult).IsTrue();

                await Assert.That(tsDictionary.Count).IsEqualTo(2);
            }

            {
                addResult = tsDictionary.TryAdd("key3", 13);

                // key1 is removed

                await Assert.That(addResult).IsTrue();

                await Assert.That(tsDictionary.Count).IsEqualTo(2);  // not 3

                getResult = tsDictionary.TryGetValue("key1", out getValue);

                await Assert.That(getResult).IsFalse();

                getResult = tsDictionary.TryGetValue("key2", out getValue);

                await Assert.That(getResult).IsTrue();

                await Assert.That(getValue).IsEqualTo(12);

                getResult = tsDictionary.TryGetValue("key3", out getValue);

                await Assert.That(getResult).IsTrue();

                await Assert.That(getValue).IsEqualTo(13);
            }

            tsDictionary.CheckConsistency();
        }

        #endregion

        #region Update

        [Test]
        public async Task Test1_TryUpdate()
        {
            var tsDictionary = new TimestampedDictionary<int>();

            long timestamp1 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            await Task.Delay(10);

            {
                bool addResult = tsDictionary.TryAdd("key1", 11);

                await Assert.That(addResult).IsTrue();

                await Assert.That(tsDictionary.Count).IsEqualTo(1);
            }

            await Task.Delay(10);

            long timestamp2 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            await Task.Delay(10);

            {
                bool getResult = tsDictionary.TryGetData("key1", out var getData);

                await Assert.That(getResult).IsTrue();

                await Assert.That(getData).IsNotNull();

                await Assert.That(getData.Timestamp).IsBetween(timestamp1, timestamp2);

                await Assert.That(getData.Key).IsEqualTo("key1");

                await Assert.That(getData.Value).IsEqualTo(11);
            }

            {
                bool addResult = tsDictionary.TryAdd("key2", 12);

                await Assert.That(addResult).IsTrue();

                await Assert.That(tsDictionary.Count).IsEqualTo(2);
            }

            {
                var timestampedKeys = tsDictionary.GetTimestampedKeys();

                await Assert.That(timestampedKeys[0].Key).IsEqualTo("key1");
                await Assert.That(timestampedKeys[1].Key).IsEqualTo("key2");
            }

            await Task.Delay(10);

            long timestamp3 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            await Task.Delay(10);

            {
                bool updateResult = tsDictionary.TryUpdate("key1", 13);

                await Assert.That(updateResult).IsTrue();

                await Assert.That(tsDictionary.Count).IsEqualTo(2);
            }

            await Task.Delay(10);

            long timestamp4 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            await Task.Delay(10);

            {
                bool getResult = tsDictionary.TryGetData("key1", out var getData);

                await Assert.That(getResult).IsTrue();

                await Assert.That(getData).IsNotNull();

                await Assert.That(getData.Timestamp).IsBetween(timestamp3, timestamp4);

                await Assert.That(getData.Key).IsEqualTo("key1");

                await Assert.That(getData.Value).IsEqualTo(13);
            }

            {
                var timestampedKeys = tsDictionary.GetTimestampedKeys();

                await Assert.That(timestampedKeys[0].Key).IsEqualTo("key2");
                await Assert.That(timestampedKeys[1].Key).IsEqualTo("key1");

            }

            tsDictionary.CheckConsistency();
        }

        [Test]
        public async Task Test1_TryUpdate_with_comparison()
        {
            var tsDictionary = new TimestampedDictionary<int>();

            long timestamp1 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            await Task.Delay(10);

            {
                bool addResult = tsDictionary.TryAdd("key1", 11);

                await Assert.That(addResult).IsTrue();

                await Assert.That(tsDictionary.Count).IsEqualTo(1);
            }

            await Task.Delay(10);

            long timestamp2 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            await Task.Delay(10);

            {
                bool getResult = tsDictionary.TryGetData("key1", out var getData);

                await Assert.That(getResult).IsTrue();

                await Assert.That(getData).IsNotNull();

                await Assert.That(getData.Timestamp).IsBetween(timestamp1, timestamp2);

                await Assert.That(getData.Key).IsEqualTo("key1");

                await Assert.That(getData.Value).IsEqualTo(11);
            }

            {
                bool addResult = tsDictionary.TryAdd("key2", 12);

                await Assert.That(addResult).IsTrue();

                await Assert.That(tsDictionary.Count).IsEqualTo(2);
            }

            {
                var timestampedKeys = tsDictionary.GetTimestampedKeys();

                await Assert.That(timestampedKeys[0].Key).IsEqualTo("key1");
                await Assert.That(timestampedKeys[1].Key).IsEqualTo("key2");
            }

            await Task.Delay(10);

            long timestamp3 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            await Task.Delay(10);

            {
                bool updateResult = tsDictionary.TryUpdate("key1", 13, comparisonValue: 99);

                await Assert.That(updateResult).IsFalse();

                await Assert.That(tsDictionary.Count).IsEqualTo(2);
            }

            await Task.Delay(10);

            long timestamp4 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            await Task.Delay(10);

            {
                bool updateResult = tsDictionary.TryUpdate("key1", 13, comparisonValue: 11);

                await Assert.That(updateResult).IsTrue();

                await Assert.That(tsDictionary.Count).IsEqualTo(2);
            }

            await Task.Delay(10);

            long timestamp5 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            await Task.Delay(10);

            {
                bool getResult = tsDictionary.TryGetData("key1", out var getData);

                await Assert.That(getResult).IsTrue();

                await Assert.That(getData).IsNotNull();

                await Assert.That(getData.Timestamp).IsBetween(timestamp4, timestamp5);

                await Assert.That(getData.Key).IsEqualTo("key1");

                await Assert.That(getData.Value).IsEqualTo(13);
            }

            {
                var timestampedKeys = tsDictionary.GetTimestampedKeys();

                await Assert.That(timestampedKeys[0].Key).IsEqualTo("key2");
                await Assert.That(timestampedKeys[1].Key).IsEqualTo("key1");
            }

            tsDictionary.CheckConsistency();
        }

        #endregion

        #region AddOrUpdate

        [Test]
        public async Task Test1_AddOrUpdate()
        {
            var tsDictionary = new TimestampedDictionary<int>();

            TimestampedDictionaryData<int> dictionaryData;

            long timestamp1 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            await Task.Delay(10);

            dictionaryData = tsDictionary.AddOrUpdate("key1", 11);

            await Task.Delay(10);

            long timestamp2 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            await Task.Delay(10);

            {
                await Assert.That(dictionaryData).IsNotNull();

                await Assert.That(dictionaryData.Timestamp).IsBetween(timestamp1, timestamp2);

                await Assert.That(dictionaryData.Key).IsEqualTo("key1");

                await Assert.That(dictionaryData.Value).IsEqualTo(11);

                await Assert.That(tsDictionary.Count).IsEqualTo(1);
            }

            await Task.Delay(10);

            long timestamp3 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            await Task.Delay(10);

            dictionaryData = tsDictionary.AddOrUpdate("key2", 12);

            await Task.Delay(10);

            long timestamp4 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            await Task.Delay(10);

            {
                await Assert.That(dictionaryData).IsNotNull();

                await Assert.That(dictionaryData.Timestamp).IsBetween(timestamp3, timestamp4);

                await Assert.That(dictionaryData.Key).IsEqualTo("key2");

                await Assert.That(dictionaryData.Value).IsEqualTo(12);

                await Assert.That(tsDictionary.Count).IsEqualTo(2);
            }

            {
                var timestampedKeys = tsDictionary.GetTimestampedKeys();

                await Assert.That(timestampedKeys[0].Key).IsEqualTo("key1");
                await Assert.That(timestampedKeys[1].Key).IsEqualTo("key2");
            }

            await Task.Delay(10);

            long timestamp5 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            await Task.Delay(10);

            dictionaryData = tsDictionary.AddOrUpdate("key1", 13);

            await Task.Delay(10);

            long timestamp6 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            await Task.Delay(10);

            {
                await Assert.That(dictionaryData).IsNotNull();

                await Assert.That(dictionaryData.Timestamp).IsBetween(timestamp5, timestamp6);

                await Assert.That(dictionaryData.Key).IsEqualTo("key1");

                await Assert.That(dictionaryData.Value).IsEqualTo(13);

                await Assert.That(tsDictionary.Count).IsEqualTo(2);
            }

            {
                var timestampedKeys = tsDictionary.GetTimestampedKeys();

                await Assert.That(timestampedKeys[0].Key).IsEqualTo("key2");
                await Assert.That(timestampedKeys[1].Key).IsEqualTo("key1");
            }

            tsDictionary.CheckConsistency();
        }

        #endregion

        #region Remove

        [Test]
        [Arguments("key1", 11)]
        [Arguments("key2", 12)]
        [Arguments("key3", 13)]
        [Arguments("key4", 14)]
        public async ValueTask Test1_TryRemove_one(string key, int value)
        {
            var tsDictionary = new TimestampedDictionary<int>();

            {
                bool addResult = tsDictionary.TryAdd(key, value);

                await Assert.That(addResult).IsTrue();

                await Assert.That(tsDictionary.Count).IsEqualTo(1);
            }

            {
                bool removeResult = tsDictionary.TryRemove(key, out var removeValue);

                await Assert.That(removeResult).IsTrue();

                await Assert.That(removeValue).IsEqualTo(value);

                await Assert.That(tsDictionary.Count).IsEqualTo(0);
            }

            {
                bool getResult = tsDictionary.TryGetValue(key, out int getValue);

                await Assert.That(getResult).IsFalse();

                await Assert.That(getValue).IsEqualTo(0);
            }

            tsDictionary.CheckConsistency();
        }

        [Test]
        [Arguments(0)]
        [Arguments(1)]
        [Arguments(2)]
        [Arguments(3)]
        public async ValueTask Test1_TryRemove_any(int testPatternIndex)
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

            await Assert.That(testPatternList).HasCount().GreaterThan(testPatternIndex);

            (var inputDic, var removeKeys) = testPatternList[testPatternIndex];

            var tsDictionary = new TimestampedDictionary<int>(dictionary: inputDic);

            await Assert.That(tsDictionary.Count).IsEqualTo(inputDic.Count);

            foreach (var removeKey in removeKeys)
            {
                bool removeResult = tsDictionary.TryRemove(removeKey, out var removeValue);

                await Assert.That(removeResult).IsTrue();

                //await Assert.That(removeValue).IsEqualTo(n));  // @notice
            }

            await Assert.That(tsDictionary.Count).IsEqualTo(inputDic.Count - removeKeys.Count);

            foreach (var inParam in inputDic)
            {
                bool getResult = tsDictionary.TryGetValue(inParam.Key, out int getValue);

                if (removeKeys.Exists(key => key == inParam.Key))
                {
                    await Assert.That(getResult).IsFalse();

                    await Assert.That(getValue).IsEqualTo(0);
                }
                else
                {
                    await Assert.That(getResult).IsTrue();

                    await Assert.That(getValue).IsEqualTo(inParam.Value);
                }
            }

            tsDictionary.CheckConsistency();
        }

        #endregion

        #region Clear

        [Test]
        public async ValueTask Test1_Clear_zero()
        {
            var tsDictionary = new TimestampedDictionary<int>();

            await Assert.That(tsDictionary.Count).IsEqualTo(0);

            tsDictionary.Clear();

            await Assert.That(tsDictionary.Count).IsEqualTo(0);
        }

        [Test]
        public async ValueTask Test1_Clear_one()
        {
            var tsDictionary = new TimestampedDictionary<int>();

            await Assert.That(tsDictionary.Count).IsEqualTo(0);

            {
                bool addResult = tsDictionary.TryAdd("key1", 11);

                await Assert.That(addResult).IsTrue();

                await Assert.That(tsDictionary.Count).IsEqualTo(1);
            }

            tsDictionary.Clear();

            await Assert.That(tsDictionary.Count).IsEqualTo(0);
        }

        [Test]
        public async ValueTask Test1_Clear_two()
        {
            var tsDictionary = new TimestampedDictionary<int>();

            await Assert.That(tsDictionary.Count).IsEqualTo(0);

            {
                bool addResult;

                addResult = tsDictionary.TryAdd("key1", 11);

                await Assert.That(addResult).IsTrue();

                await Assert.That(tsDictionary.Count).IsEqualTo(1);

                addResult = tsDictionary.TryAdd("key2", 12);

                await Assert.That(addResult).IsTrue();

                await Assert.That(tsDictionary.Count).IsEqualTo(2);
            }

            tsDictionary.Clear();

            await Assert.That(tsDictionary.Count).IsEqualTo(0);
        }

        [Test]
        [Arguments(-1, -1)]
        [Arguments(-1, 1)]
        [Arguments(-1, 2)]
        [Arguments(1, -1)]
        [Arguments(1, 1)]
        [Arguments(1, 2)]
        public async ValueTask Test1_Clear_with_newCapacity(int initialCapacity, int newCapacity)
        {
            var tsDictionary = new TimestampedDictionary<int>(initialCapacity);

            await Assert.That(tsDictionary.Capacity).IsEqualTo(initialCapacity);

            tsDictionary.Clear(newCapacity);

            await Assert.That(tsDictionary.Capacity).IsEqualTo(newCapacity);
        }

        [Test]
        [Arguments(-1, 0)]
        [Arguments(-1, -2)]
        [Arguments(-1, -3)]
        [Arguments(1, 0)]
        [Arguments(1, -2)]
        [Arguments(1, -3)]
        public async ValueTask Test1_Clear_with_invalid_newCapacity(int initialCapacity, int newCapacity)
        {
            var tsDictionary = new TimestampedDictionary<int>(initialCapacity);

            await Assert.That(tsDictionary.Capacity).IsEqualTo(initialCapacity);

            var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                tsDictionary.Clear(newCapacity);
            });

            await Assert.That(exception.Source).IsEqualTo("Izayoi.Collections.TimestampedDictionary");
        }

        [Test]
        public async Task Test1_ClearBefore_DateTimeOffset()
        {
            var tsDictionary = new TimestampedDictionary<int>();

            await Assert.That(tsDictionary.Count).IsEqualTo(0);

            bool addResult;

            {
                addResult = tsDictionary.TryAdd("key1", 11);

                await Assert.That(addResult).IsTrue();

                await Assert.That(tsDictionary.Count).IsEqualTo(1);

                addResult = tsDictionary.TryAdd("key2", 12);

                await Assert.That(addResult).IsTrue();

                await Assert.That(tsDictionary.Count).IsEqualTo(2);
            }

            await Task.Delay(10);

            var timestamp = DateTimeOffset.UtcNow;

            await Task.Delay(10);

            {
                addResult = tsDictionary.TryAdd("key3", 13);

                await Assert.That(addResult).IsTrue();

                await Assert.That(tsDictionary.Count).IsEqualTo(3);
            }

            {
                int removedCount = tsDictionary.ClearBefore(timestamp);

                await Assert.That(removedCount).IsEqualTo(2);

                await Assert.That(tsDictionary.Count).IsEqualTo(1);

                bool getResult;

                getResult = tsDictionary.TryGetValue("key1", out _);

                await Assert.That(getResult).IsFalse();

                getResult = tsDictionary.TryGetValue("key2", out _);

                await Assert.That(getResult).IsFalse();

                getResult = tsDictionary.TryGetValue("key3", out var value);

                await Assert.That(getResult).IsTrue();

                await Assert.That(value).IsEqualTo(13);
            }
        }

        #endregion

        #region Add and GetData

        [Test]
        [Arguments("key1", 11)]
        [Arguments("key2", 12)]
        [Arguments("key3", 13)]
        [Arguments("key4", 14)]
        [Arguments("key5", 15)]
        public async Task Test1_TryAdd_and_TryGetData_one(string key, int value)
        {
            var tsDictionary = new TimestampedDictionary<int>();

            long timestamp1 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            await Task.Delay(10);

            {
                bool addResult = tsDictionary.TryAdd(key, value);

                await Assert.That(addResult).IsTrue();

                await Assert.That(tsDictionary.Count).IsEqualTo(1);
            }

            await Task.Delay(10);

            long timestamp2 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            {
                bool getResult = tsDictionary.TryGetData(key, out var getData);

                await Assert.That(getResult).IsTrue();

                await Assert.That(getData).IsNotNull();

                await Assert.That(getData.Timestamp).IsBetween(timestamp1, timestamp2);

                await Assert.That(getData.Key).IsEqualTo(key);

                await Assert.That(getData.Value).IsEqualTo(value);
            }
        }

        [Test]
        [Arguments(0)]
        [Arguments(1)]
        [Arguments(2)]
        [Arguments(3)]
        [Arguments(4)]
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

            await Assert.That(testPatternList).HasCount().GreaterThan(testPatternIndex);

            var inputData = testPatternList[testPatternIndex];

            long timestamp1 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            await Task.Delay(10);

            var tsDictionary = new TimestampedDictionary<int>(dictionary: inputData);

            await Task.Delay(10);

            long timestamp2 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            await Assert.That(tsDictionary.Count).IsEqualTo(inputData.Count);

            foreach (var inParam in inputData)
            {
                bool getResult = tsDictionary.TryGetData(inParam.Key, out var getData);

                await Assert.That(getResult).IsTrue();

                await Assert.That(getData).IsNotNull();

                await Assert.That(getData.Timestamp).IsBetween(timestamp1, timestamp2);

                await Assert.That(getData.Key).IsEqualTo(inParam.Key);

                await Assert.That(getData.Value).IsEqualTo(inParam.Value);
            }
        }

        #endregion

        #region Contains Key

        [Test]
        public async ValueTask Test1_ContainsKey()
        {
            var tsDictionary = new TimestampedDictionary<int>();

            {
                bool containsKey = tsDictionary.ContainsKey("key1");

                await Assert.That(containsKey).IsFalse();
            }

            {
                bool addResult = tsDictionary.TryAdd("key1", 1);

                await Assert.That(addResult).IsTrue();
            }

            {
                bool containsKey = tsDictionary.ContainsKey("key1");

                await Assert.That(containsKey).IsTrue();
            }

            {
                bool removeResult = tsDictionary.TryRemove("key1", out _);

                await Assert.That(removeResult).IsTrue();
            }

            {
                bool containsKey = tsDictionary.ContainsKey("key1");

                await Assert.That(containsKey).IsFalse();
            }
        }

        #endregion

        #region Keys

        [Test]
        public async ValueTask Test1_GetKeys_zero()
        {
            var tsDictionary = new TimestampedDictionary<int>();

            HashSet<string> keySet = tsDictionary.GetKeys();

            await Assert.That(keySet.Count).IsEqualTo(0);
        }

        [Test]
        public async ValueTask Test1_GetKeys_zero_with_remove()
        {
            var tsDictionary = new TimestampedDictionary<int>();

            {
                bool addResult = tsDictionary.TryAdd("key1", 11);

                await Assert.That(addResult).IsTrue();

                HashSet<string> keySet = tsDictionary.GetKeys();

                await Assert.That(keySet.Count).IsEqualTo(1);

                await Assert.That(keySet.First()).IsEqualTo("key1");
            }

            {
                bool removeResult = tsDictionary.TryRemove("key1", out _);

                await Assert.That(removeResult).IsTrue();

                HashSet<string> keySet = tsDictionary.GetKeys();

                await Assert.That(keySet).IsEmpty();
            }
        }

        [Test]
        [Arguments(0)]
        [Arguments(1)]
        [Arguments(2)]
        [Arguments(3)]
        public async ValueTask Test1_GetKeys_any(int testPatternIndex)
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

            await Assert.That(testPatternList).HasCount().GreaterThan(testPatternIndex);

            (var inputDic, var removeKeys) = testPatternList[testPatternIndex];

            var tsDictionary = new TimestampedDictionary<int>(dictionary: inputDic);

            await Assert.That(tsDictionary.Count).IsEqualTo(inputDic.Count);

            foreach (var removeKey in removeKeys)
            {
                bool removeResult = tsDictionary.TryRemove(removeKey, out var removeValue);

                await Assert.That(removeResult).IsTrue();

                //await Assert.That(removeValue).IsEqualTo(n));  // @notice
            }

            await Assert.That(tsDictionary.Count).IsEqualTo(inputDic.Count - removeKeys.Count);

            HashSet<string> keySet = tsDictionary.GetKeys();

            await Assert.That(keySet.Count).IsEqualTo(inputDic.Count - removeKeys.Count);

            foreach (var inParam in inputDic)
            {
                bool contains = keySet.Contains(inParam.Key);

                if (removeKeys.Exists(key => key == inParam.Key))
                {
                    await Assert.That(contains).IsFalse();
                }
                else
                {
                    await Assert.That(contains).IsTrue();
                }
            }

            tsDictionary.CheckConsistency();
        }

        [Test]
        public async ValueTask Test1_GetAddedKeys_any()
        {
            var tsDictionary = new TimestampedDictionary<int>();

            {
                bool addResult;

                addResult = tsDictionary.TryAdd("key1", 11);

                await Assert.That(addResult).IsTrue();

                addResult = tsDictionary.TryAdd("key2", 12);

                await Assert.That(addResult).IsTrue();

                addResult = tsDictionary.TryAdd("key3", 13);

                await Assert.That(addResult).IsTrue();

                addResult = tsDictionary.TryAdd("key4", 14);

                await Assert.That(addResult).IsTrue();

                await Assert.That(tsDictionary.Count).IsEqualTo(4);
            }

            {
                List<string> addedKeys = tsDictionary.GetAddedKeys();

                await Assert.That(addedKeys).HasCount().EqualTo(4);

                await Assert.That(addedKeys[0]).IsEqualTo("key1");
                await Assert.That(addedKeys[1]).IsEqualTo("key2");
                await Assert.That(addedKeys[2]).IsEqualTo("key3");
                await Assert.That(addedKeys[3]).IsEqualTo("key4");
            }

            {
                bool removeResult = tsDictionary.TryRemove("key3", out var removeValue);

                await Assert.That(removeResult).IsTrue();

                await Assert.That(removeValue).IsEqualTo(13);

                await Assert.That(tsDictionary.Count).IsEqualTo(3);
            }

            {
                List<string> addedKeys = tsDictionary.GetAddedKeys();

                await Assert.That(addedKeys).HasCount().EqualTo(3);

                await Assert.That(addedKeys[0]).IsEqualTo("key1");
                await Assert.That(addedKeys[1]).IsEqualTo("key2");
                await Assert.That(addedKeys[2]).IsEqualTo("key4");
            }

            tsDictionary.CheckConsistency();
        }

        [Test]
        public async Task Test1_GetTimestampedKeys_any()
        {
            var tsDictionary = new TimestampedDictionary<int>();

            long beginTimestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            {
                bool addResult;

                await Task.Delay(10);

                addResult = tsDictionary.TryAdd("key1", 11);

                await Assert.That(addResult).IsTrue();

                await Task.Delay(10);

                addResult = tsDictionary.TryAdd("key2", 12);

                await Assert.That(addResult).IsTrue();

                await Task.Delay(10);

                addResult = tsDictionary.TryAdd("key3", 13);

                await Assert.That(addResult).IsTrue();

                await Task.Delay(10);

                addResult = tsDictionary.TryAdd("key4", 14);

                await Assert.That(addResult).IsTrue();

                await Assert.That(tsDictionary.Count).IsEqualTo(4);
            }

            {
                List<TimestampedKey> timestampedKeys = tsDictionary.GetTimestampedKeys();

                await Assert.That(timestampedKeys).HasCount().EqualTo(4);

                await Assert.That(timestampedKeys[0].Key).IsEqualTo("key1");
                await Assert.That(timestampedKeys[1].Key).IsEqualTo("key2");
                await Assert.That(timestampedKeys[2].Key).IsEqualTo("key3");
                await Assert.That(timestampedKeys[3].Key).IsEqualTo("key4");

                await Assert.That(beginTimestamp).IsLessThan(timestampedKeys[0].Timestamp);
                await Assert.That(timestampedKeys[0].Timestamp).IsLessThan(timestampedKeys[1].Timestamp);
                await Assert.That(timestampedKeys[1].Timestamp).IsLessThan(timestampedKeys[2].Timestamp);
                await Assert.That(timestampedKeys[2].Timestamp).IsLessThan(timestampedKeys[3].Timestamp);
            }

            {
                bool removeResult = tsDictionary.TryRemove("key3", out var removeValue);

                await Assert.That(removeResult).IsTrue();

                await Assert.That(removeValue).IsEqualTo(13);

                await Assert.That(tsDictionary.Count).IsEqualTo(3);
            }

            {
                List<TimestampedKey> timestampedKeys = tsDictionary.GetTimestampedKeys();

                await Assert.That(timestampedKeys).HasCount().EqualTo(3);

                await Assert.That(timestampedKeys[0].Key).IsEqualTo("key1");
                await Assert.That(timestampedKeys[1].Key).IsEqualTo("key2");
                await Assert.That(timestampedKeys[2].Key).IsEqualTo("key4");

                await Assert.That(beginTimestamp).IsLessThan(timestampedKeys[0].Timestamp);
                await Assert.That(timestampedKeys[0].Timestamp).IsLessThan(timestampedKeys[1].Timestamp);
                await Assert.That(timestampedKeys[1].Timestamp).IsLessThan(timestampedKeys[2].Timestamp);
            }

            tsDictionary.CheckConsistency();
        }

        #endregion
    }
}