// ----------------------------------------------------------------------
// @Namespace : Izayoi.Collections.TimestampedDictionary.Test
// @Class     : TimestampedDictionaryTest1
// ----------------------------------------------------------------------
namespace Izayoi.Collections.TimestampedDictionary.Test
{
    using Izayoi.Collections;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    [TestFixture]
    public class TimestampedDictionaryTest1
    {
        #region Construct

        [TestCase]
        public void Test1_Construct()
        {
            var tsDictionary = new TimestampedDictionary<int>();

            Assert.That(tsDictionary.Capacity, Is.EqualTo(-1));

            Assert.That(tsDictionary.Count, Is.Zero);
        }

        [TestCase(-1)]
        [TestCase(1)]
        [TestCase(2)]
        public void Test1_Construct_with_correct_capacity(int capacity)
        {
            var tsDictionary = new TimestampedDictionary<int>(capacity);

            Assert.That(tsDictionary.Capacity, Is.EqualTo(capacity));
        }

        [TestCase(0)]
        [TestCase(-2)]
        [TestCase(-3)]
        public void Test1_Construct_with_invalid_capacity(int capacity)
        {
            var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                var tsDictionary = new TimestampedDictionary<int>(capacity);
            });

            Assert.That(exception.Source, Is.EqualTo("Izayoi.Collections.TimestampedDictionary"));
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
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

            Assert.That(testPatternList, Has.Count.GreaterThan(testPatternIndex));

            var initialData = testPatternList[testPatternIndex];

            var tsDictionary = new TimestampedDictionary<int>(dictionary: initialData);

            Assert.That(tsDictionary.Count, Is.EqualTo(initialData.Count));
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
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

            Assert.That(testPatternList, Has.Count.GreaterThan(testPatternIndex));

            var initialData = testPatternList[testPatternIndex];

            var tsDictionary = new TimestampedDictionary<int>(capacity: initialData.Count, dictionary: initialData);

            Assert.That(tsDictionary.Count, Is.EqualTo(initialData.Count));
        }

        [TestCase]
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

                var tsDictionary = new TimestampedDictionary<int>(capacity: initialData.Count - 1, initialData);
            });

            Assert.That(exception.Source, Is.EqualTo("Izayoi.Collections.TimestampedDictionary"));
        }

        #endregion

        #region Property

        [TestCase]
        public void Test1_Property_Count()
        {
            var tsDictionary = new TimestampedDictionary<int>();

            Assert.That(tsDictionary.Count, Iz.Zero);

            {
                bool addResult = tsDictionary.TryAdd("key1", 11);

                Assert.That(addResult, Is.True);

                Assert.That(tsDictionary, Has.Count.EqualTo(1));
            }

            {
                bool addResult = tsDictionary.TryAdd("key2", 12);

                Assert.That(addResult, Is.True);

                Assert.That(tsDictionary, Has.Count.EqualTo(2));
            }

            {
                bool addResult = tsDictionary.TryAdd("key3", 13);

                Assert.That(addResult, Is.True);

                Assert.That(tsDictionary, Has.Count.EqualTo(3));
            }
        }

        #endregion

        #region Add

        [TestCase("key1", 11)]
        [TestCase("key2", 12)]
        [TestCase("key3", 13)]
        public void Test1_TryAdd_one(string key, int value)
        {
            var tsDictionary = new TimestampedDictionary<int>();

            bool addResult = tsDictionary.TryAdd(key, value);

            Assert.That(addResult, Is.True);

            Assert.That(tsDictionary, Has.Count.EqualTo(1));
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
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

            Assert.That(testPatternList, Has.Count.GreaterThan(testPatternIndex));

            var inputData = testPatternList[testPatternIndex];

            var tsDictionary = new TimestampedDictionary<int>();

            foreach (var inParam in inputData)
            {
                bool addResult = tsDictionary.TryAdd(inParam.Key, inParam.Value);

                Assert.That(addResult, Is.True);
            }

            Assert.That(tsDictionary, Has.Count.EqualTo(inputData.Count));
        }

        [TestCase("key1", 11)]
        public void Test1_TryAdd_duplicate_key(string key, int value)
        {
            var tsDictionary = new TimestampedDictionary<int>();

            bool addResult;

            addResult = tsDictionary.TryAdd(key, value);

            Assert.That(addResult, Is.True);

            Assert.That(tsDictionary, Has.Count.EqualTo(1));

            addResult = tsDictionary.TryAdd(key, value);

            Assert.That(addResult, Is.False);
        }

        #endregion

        #region Add and GetValue

        [TestCase("key1", 11)]
        [TestCase("key2", 12)]
        [TestCase("key3", 13)]
        [TestCase("key4", 14)]
        [TestCase("key5", 15)]
        public void Test1_TryAdd_and_TryGetValue_one(string key, int value)
        {
            var tsDictionary = new TimestampedDictionary<int>();

            {
                bool addResult = tsDictionary.TryAdd(key, value);

                Assert.That(addResult, Is.True);

                Assert.That(tsDictionary, Has.Count.EqualTo(1));
            }

            {
                bool getResult = tsDictionary.TryGetValue(key, out int getValue);

                Assert.That(getResult, Is.True);

                Assert.That(getValue, Is.EqualTo(value));
            }
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
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

            Assert.That(testPatternList, Has.Count.GreaterThan(testPatternIndex));

            var inputData = testPatternList[testPatternIndex];

            var tsDictionary = new TimestampedDictionary<int>(dictionary: inputData);

            Assert.That(tsDictionary, Has.Count.EqualTo(inputData.Count));

            foreach (var inParam in inputData)
            {
                bool getResult = tsDictionary.TryGetValue(inParam.Key, out int getValue);

                Assert.That(getResult, Is.True);

                Assert.That(getValue, Is.EqualTo(inParam.Value));
            }
        }

        [TestCase]
        public void Test1_TryAdd_and_TryGetValue_on_Capacity_1()
        {
            var tsDictionary = new TimestampedDictionary<int>(capacity: 1);

            {
                bool addResult = tsDictionary.TryAdd("key1", 11);

                Assert.That(addResult, Is.True);

                Assert.That(tsDictionary, Has.Count.EqualTo(1));

                bool getResult = tsDictionary.TryGetValue("key1", out var getValue);

                Assert.That(getResult, Is.True);

                Assert.That(getValue, Is.EqualTo(11));
            }

            {
                bool addResult = tsDictionary.TryAdd("key2", 12);

                // key1 is removed

                Assert.That(addResult, Is.True);

                Assert.That(tsDictionary, Has.Count.EqualTo(1));  // not 2

                bool getResult;

                int getValue;

                getResult = tsDictionary.TryGetValue("key1", out getValue);

                Assert.That(getResult, Is.False);

                getResult = tsDictionary.TryGetValue("key2", out getValue);

                Assert.That(getResult, Is.True);

                Assert.That(getValue, Is.EqualTo(12));
            }

            tsDictionary.CheckConsistency();
        }

        [TestCase]
        public void Test1_TryAdd_and_TryGetValue_on_Capacity_2()
        {
            var tsDictionary = new TimestampedDictionary<int>(capacity: 2);

            bool addResult;
            bool getResult;
            int getValue;

            {
                addResult = tsDictionary.TryAdd("key1", 11);

                Assert.That(addResult, Is.True);

                addResult = tsDictionary.TryAdd("key2", 12);

                Assert.That(addResult, Is.True);

                Assert.That(tsDictionary, Has.Count.EqualTo(2));
            }

            {
                addResult = tsDictionary.TryAdd("key3", 13);

                // key1 is removed

                Assert.That(addResult, Is.True);

                Assert.That(tsDictionary, Has.Count.EqualTo(2));  // not 3

                getResult = tsDictionary.TryGetValue("key1", out getValue);

                Assert.That(getResult, Is.False);

                getResult = tsDictionary.TryGetValue("key2", out getValue);

                Assert.That(getResult, Is.True);

                Assert.That(getValue, Is.EqualTo(12));

                getResult = tsDictionary.TryGetValue("key3", out getValue);

                Assert.That(getResult, Is.True);

                Assert.That(getValue, Is.EqualTo(13));
            }

            tsDictionary.CheckConsistency();
        }

        #endregion

        #region Update

        [TestCase]
        public async Task Test1_TryUpdate()
        {
            var tsDictionary = new TimestampedDictionary<int>();

            long timestamp1 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            await Task.Delay(10);

            {
                bool addResult = tsDictionary.TryAdd("key1", 11);

                Assert.That(addResult, Is.True);

                Assert.That(tsDictionary, Has.Count.EqualTo(1));
            }

            await Task.Delay(10);

            long timestamp2 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            await Task.Delay(10);

            {
                bool getResult = tsDictionary.TryGetData("key1", out var getData);

                Assert.That(getResult, Is.True);

                Assert.That(getData, Is.Not.Null);

                Assert.That(getData.Timestamp, Is.InRange(timestamp1, timestamp2));

                Assert.That(getData.Key, Is.EqualTo("key1"));

                Assert.That(getData.Value, Is.EqualTo(11));
            }

            {
                bool addResult = tsDictionary.TryAdd("key2", 12);

                Assert.That(addResult, Is.True);

                Assert.That(tsDictionary, Has.Count.EqualTo(2));
            }

            {
                var timestampedKeys = tsDictionary.GetTimestampedKeys();

                Assert.That(timestampedKeys[0].Key, Is.EqualTo("key1"));
                Assert.That(timestampedKeys[1].Key, Is.EqualTo("key2"));
            }

            await Task.Delay(10);

            long timestamp3 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            await Task.Delay(10);

            {
                bool updateResult = tsDictionary.TryUpdate("key1", 13);

                Assert.That(updateResult, Is.True);

                Assert.That(tsDictionary, Has.Count.EqualTo(2));
            }

            await Task.Delay(10);

            long timestamp4 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            await Task.Delay(10);

            {
                bool getResult = tsDictionary.TryGetData("key1", out var getData);

                Assert.That(getResult, Is.True);

                Assert.That(getData, Is.Not.Null);

                Assert.That(getData.Timestamp, Is.InRange(timestamp3, timestamp4));

                Assert.That(getData.Key, Is.EqualTo("key1"));

                Assert.That(getData.Value, Is.EqualTo(13));
            }

            {
                var timestampedKeys = tsDictionary.GetTimestampedKeys();

                Assert.That(timestampedKeys[0].Key, Is.EqualTo("key2"));
                Assert.That(timestampedKeys[1].Key, Is.EqualTo("key1"));

            }

            tsDictionary.CheckConsistency();
        }

        [TestCase]
        public async Task Test1_TryUpdate_with_comparison()
        {
            var tsDictionary = new TimestampedDictionary<int>();

            long timestamp1 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            await Task.Delay(10);

            {
                bool addResult = tsDictionary.TryAdd("key1", 11);

                Assert.That(addResult, Is.True);

                Assert.That(tsDictionary, Has.Count.EqualTo(1));
            }

            await Task.Delay(10);

            long timestamp2 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            await Task.Delay(10);

            {
                bool getResult = tsDictionary.TryGetData("key1", out var getData);

                Assert.That(getResult, Is.True);

                Assert.That(getData, Is.Not.Null);

                Assert.That(getData.Timestamp, Is.InRange(timestamp1, timestamp2));

                Assert.That(getData.Key, Is.EqualTo("key1"));

                Assert.That(getData.Value, Is.EqualTo(11));
            }

            {
                bool addResult = tsDictionary.TryAdd("key2", 12);

                Assert.That(addResult, Is.True);

                Assert.That(tsDictionary, Has.Count.EqualTo(2));
            }

            {
                var timestampedKeys = tsDictionary.GetTimestampedKeys();

                Assert.That(timestampedKeys[0].Key, Is.EqualTo("key1"));
                Assert.That(timestampedKeys[1].Key, Is.EqualTo("key2"));
            }

            await Task.Delay(10);

            long timestamp3 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            await Task.Delay(10);

            {
                bool updateResult = tsDictionary.TryUpdate("key1", 13, comparisonValue: 99);

                Assert.That(updateResult, Is.False);

                Assert.That(tsDictionary, Has.Count.EqualTo(2));
            }

            await Task.Delay(10);

            long timestamp4 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            await Task.Delay(10);

            {
                bool updateResult = tsDictionary.TryUpdate("key1", 13, comparisonValue: 11);

                Assert.That(updateResult, Is.True);

                Assert.That(tsDictionary, Has.Count.EqualTo(2));
            }

            await Task.Delay(10);

            long timestamp5 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            await Task.Delay(10);

            {
                bool getResult = tsDictionary.TryGetData("key1", out var getData);

                Assert.That(getResult, Is.True);

                Assert.That(getData, Is.Not.Null);

                Assert.That(getData.Timestamp, Is.InRange(timestamp4, timestamp5));

                Assert.That(getData.Key, Is.EqualTo("key1"));

                Assert.That(getData.Value, Is.EqualTo(13));
            }

            {
                var timestampedKeys = tsDictionary.GetTimestampedKeys();

                Assert.That(timestampedKeys[0].Key, Is.EqualTo("key2"));
                Assert.That(timestampedKeys[1].Key, Is.EqualTo("key1"));
            }

            tsDictionary.CheckConsistency();
        }

        #endregion

        #region AddOrUpdate

        [TestCase]
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
                Assert.That(dictionaryData, Is.Not.Null);

                Assert.That(dictionaryData.Timestamp, Is.InRange(timestamp1, timestamp2));

                Assert.That(dictionaryData.Key, Is.EqualTo("key1"));

                Assert.That(dictionaryData.Value, Is.EqualTo(11));

                Assert.That(tsDictionary, Has.Count.EqualTo(1));
            }

            await Task.Delay(10);

            long timestamp3 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            await Task.Delay(10);

            dictionaryData = tsDictionary.AddOrUpdate("key2", 12);

            await Task.Delay(10);

            long timestamp4 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            await Task.Delay(10);

            {
                Assert.That(dictionaryData, Is.Not.Null);

                Assert.That(dictionaryData.Timestamp, Is.InRange(timestamp3, timestamp4));

                Assert.That(dictionaryData.Key, Is.EqualTo("key2"));

                Assert.That(dictionaryData.Value, Is.EqualTo(12));

                Assert.That(tsDictionary, Has.Count.EqualTo(2));
            }

            {
                var timestampedKeys = tsDictionary.GetTimestampedKeys();

                Assert.That(timestampedKeys[0].Key, Is.EqualTo("key1"));
                Assert.That(timestampedKeys[1].Key, Is.EqualTo("key2"));
            }

            await Task.Delay(10);

            long timestamp5 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            await Task.Delay(10);

            dictionaryData = tsDictionary.AddOrUpdate("key1", 13);

            await Task.Delay(10);

            long timestamp6 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            await Task.Delay(10);

            {
                Assert.That(dictionaryData, Is.Not.Null);

                Assert.That(dictionaryData.Timestamp, Is.InRange(timestamp5, timestamp6));

                Assert.That(dictionaryData.Key, Is.EqualTo("key1"));

                Assert.That(dictionaryData.Value, Is.EqualTo(13));

                Assert.That(tsDictionary, Has.Count.EqualTo(2));
            }

            {
                var timestampedKeys = tsDictionary.GetTimestampedKeys();

                Assert.That(timestampedKeys[0].Key, Is.EqualTo("key2"));
                Assert.That(timestampedKeys[1].Key, Is.EqualTo("key1"));
            }

            tsDictionary.CheckConsistency();
        }

        #endregion

        #region Remove

        [TestCase("key1", 11)]
        [TestCase("key2", 12)]
        [TestCase("key3", 13)]
        [TestCase("key4", 14)]
        public void Test1_TryRemove_one(string key, int value)
        {
            var tsDictionary = new TimestampedDictionary<int>();

            {
                bool addResult = tsDictionary.TryAdd(key, value);

                Assert.That(addResult, Is.True);

                Assert.That(tsDictionary, Has.Count.EqualTo(1));
            }

            {
                bool removeResult = tsDictionary.TryRemove(key, out var removeValue);

                Assert.That(removeResult, Is.True);

                Assert.That(removeValue, Is.EqualTo(value));

                Assert.That(tsDictionary, Is.Empty);
            }

            {
                bool getResult = tsDictionary.TryGetValue(key, out int getValue);

                Assert.That(getResult, Is.False);

                Assert.That(getValue, Is.Zero);
            }

            tsDictionary.CheckConsistency();
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
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

            Assert.That(testPatternList, Has.Count.GreaterThan(testPatternIndex));

            (var inputDic, var removeKeys) = testPatternList[testPatternIndex];

            var tsDictionary = new TimestampedDictionary<int>(dictionary: inputDic);

            Assert.That(tsDictionary.Count, Is.EqualTo(inputDic.Count));

            foreach (var removeKey in removeKeys)
            {
                bool removeResult = tsDictionary.TryRemove(removeKey, out var removeValue);

                Assert.That(removeResult, Is.True);

                //Assert.That(removeValue, Is.EqualTo(n));  // @notice
            }

            Assert.That(tsDictionary.Count, Is.EqualTo(inputDic.Count - removeKeys.Count));

            foreach (var inParam in inputDic)
            {
                bool getResult = tsDictionary.TryGetValue(inParam.Key, out int getValue);

                if (removeKeys.Exists(key => key == inParam.Key))
                {
                    Assert.That(getResult, Is.False);

                    Assert.That(getValue, Is.Zero);
                }
                else
                {
                    Assert.That(getResult, Is.True);

                    Assert.That(getValue, Is.EqualTo(inParam.Value));
                }
            }

            tsDictionary.CheckConsistency();
        }

        #endregion

        #region Clear

        [TestCase]
        public void Test1_Clear_zero()
        {
            var tsDictionary = new TimestampedDictionary<int>();

            Assert.That(tsDictionary, Is.Empty);

            tsDictionary.Clear();

            Assert.That(tsDictionary, Is.Empty);
        }

        [TestCase]
        public void Test1_Clear_one()
        {
            var tsDictionary = new TimestampedDictionary<int>();

            Assert.That(tsDictionary, Is.Empty);

            {
                bool addResult = tsDictionary.TryAdd("key1", 11);

                Assert.That(addResult, Is.True);

                Assert.That(tsDictionary, Has.Count.EqualTo(1));
            }

            tsDictionary.Clear();

            Assert.That(tsDictionary, Is.Empty);
        }

        [TestCase]
        public void Test1_Clear_two()
        {
            var tsDictionary = new TimestampedDictionary<int>();

            Assert.That(tsDictionary, Is.Empty);

            {
                bool addResult;

                addResult = tsDictionary.TryAdd("key1", 11);

                Assert.That(addResult, Is.True);

                Assert.That(tsDictionary, Has.Count.EqualTo(1));

                addResult = tsDictionary.TryAdd("key2", 12);

                Assert.That(addResult, Is.True);

                Assert.That(tsDictionary, Has.Count.EqualTo(2));
            }

            tsDictionary.Clear();

            Assert.That(tsDictionary, Is.Empty);
        }

        [TestCase(-1, -1)]
        [TestCase(-1, 1)]
        [TestCase(-1, 2)]
        [TestCase(1, -1)]
        [TestCase(1, 1)]
        [TestCase(1, 2)]
        public void Test1_Clear_with_newCapacity(int initialCapacity, int newCapacity)
        {
            var tsDictionary = new TimestampedDictionary<int>(initialCapacity);

            Assert.That(tsDictionary.Capacity, Is.EqualTo(initialCapacity));

            tsDictionary.Clear(newCapacity);

            Assert.That(tsDictionary.Capacity, Is.EqualTo(newCapacity));
        }

        [TestCase(-1, 0)]
        [TestCase(-1, -2)]
        [TestCase(-1, -3)]
        [TestCase(1, 0)]
        [TestCase(1, -2)]
        [TestCase(1, -3)]
        public void Test1_Clear_with_invalid_newCapacity(int initialCapacity, int newCapacity)
        {
            var tsDictionary = new TimestampedDictionary<int>(initialCapacity);

            Assert.That(tsDictionary.Capacity, Is.EqualTo(initialCapacity));

            var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                tsDictionary.Clear(newCapacity);
            });

            Assert.That(exception.Source, Is.EqualTo("Izayoi.Collections.TimestampedDictionary"));
        }

        [TestCase]
        public async Task Test1_ClearBefore_DateTimeOffset()
        {
            var tsDictionary = new TimestampedDictionary<int>();

            Assert.That(tsDictionary, Is.Empty);

            bool addResult;

            {
                addResult = tsDictionary.TryAdd("key1", 11);

                Assert.That(addResult, Is.True);

                Assert.That(tsDictionary, Has.Count.EqualTo(1));

                addResult = tsDictionary.TryAdd("key2", 12);

                Assert.That(addResult, Is.True);

                Assert.That(tsDictionary, Has.Count.EqualTo(2));
            }

            await Task.Delay(10);

            var timestamp = DateTimeOffset.UtcNow;

            await Task.Delay(10);

            {
                addResult = tsDictionary.TryAdd("key3", 13);

                Assert.That(addResult, Is.True);

                Assert.That(tsDictionary, Has.Count.EqualTo(3));
            }

            {
                int removedCount = tsDictionary.ClearBefore(timestamp);

                Assert.That(removedCount, Is.EqualTo(2));

                Assert.That(tsDictionary, Has.Count.EqualTo(1));

                bool getResult;

                getResult = tsDictionary.TryGetValue("key1", out _);

                Assert.That(getResult, Is.False);

                getResult = tsDictionary.TryGetValue("key2", out _);

                Assert.That(getResult, Is.False);

                getResult = tsDictionary.TryGetValue("key3", out var value);

                Assert.That(getResult, Is.True);

                Assert.That(value, Is.EqualTo(13));
            }
        }

        #endregion

        #region Add and GetData

        [TestCase("key1", 11)]
        [TestCase("key2", 12)]
        [TestCase("key3", 13)]
        [TestCase("key4", 14)]
        [TestCase("key5", 15)]
        public async Task Test1_TryAdd_and_TryGetData_one(string key, int value)
        {
            var tsDictionary = new TimestampedDictionary<int>();

            long timestamp1 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            await Task.Delay(10);

            {
                bool addResult = tsDictionary.TryAdd(key, value);

                Assert.That(addResult, Is.True);

                Assert.That(tsDictionary, Has.Count.EqualTo(1));
            }

            await Task.Delay(10);

            long timestamp2 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            {
                bool getResult = tsDictionary.TryGetData(key, out var getData);

                Assert.That(getResult, Is.True);

                Assert.That(getData, Is.Not.Null);

                Assert.That(getData.Timestamp, Is.InRange(timestamp1, timestamp2));

                Assert.That(getData.Key, Is.EqualTo(key));

                Assert.That(getData.Value, Is.EqualTo(value));
            }
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
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

            Assert.That(testPatternList, Has.Count.GreaterThan(testPatternIndex));

            var inputData = testPatternList[testPatternIndex];

            long timestamp1 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            await Task.Delay(10);

            var tsDictionary = new TimestampedDictionary<int>(dictionary: inputData);

            await Task.Delay(10);

            long timestamp2 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            Assert.That(tsDictionary, Has.Count.EqualTo(inputData.Count));

            foreach (var inParam in inputData)
            {
                bool getResult = tsDictionary.TryGetData(inParam.Key, out var getData);

                Assert.That(getResult, Is.True);

                Assert.That(getData, Is.Not.Null);

                Assert.That(getData.Timestamp, Is.InRange(timestamp1, timestamp2));

                Assert.That(getData.Key, Is.EqualTo(inParam.Key));

                Assert.That(getData.Value, Is.EqualTo(inParam.Value));
            }
        }

        #endregion

        #region Contains Key

        [TestCase]
        public void Test1_ContainsKey()
        {
            var tsDictionary = new TimestampedDictionary<int>();

            {
                bool containsKey = tsDictionary.ContainsKey("key1");

                Assert.That(containsKey, Is.False);
            }

            {
                bool addResult = tsDictionary.TryAdd("key1", 1);

                Assert.That(addResult, Is.True);
            }

            {
                bool containsKey = tsDictionary.ContainsKey("key1");

                Assert.That(containsKey, Is.True);
            }

            {
                bool removeResult = tsDictionary.TryRemove("key1", out _);

                Assert.That(removeResult, Is.True);
            }

            {
                bool containsKey = tsDictionary.ContainsKey("key1");

                Assert.That(containsKey, Is.False);
            }
        }

        #endregion

        #region Keys

        [TestCase]
        public void Test1_GetKeys_zero()
        {
            var tsDictionary = new TimestampedDictionary<int>();

            HashSet<string> keySet = tsDictionary.GetKeys();

            Assert.That(keySet, Is.Empty);
        }

        [TestCase]
        public void Test1_GetKeys_zero_with_remove()
        {
            var tsDictionary = new TimestampedDictionary<int>();

            {
                bool addResult = tsDictionary.TryAdd("key1", 11);

                Assert.That(addResult, Is.True);

                HashSet<string> keySet = tsDictionary.GetKeys();

                Assert.That(keySet, Has.Count.EqualTo(1));

                Assert.That(keySet.First(), Is.EqualTo("key1"));
            }

            {
                bool removeResult = tsDictionary.TryRemove("key1", out _);

                Assert.That(removeResult, Is.True);

                HashSet<string> keySet = tsDictionary.GetKeys();

                Assert.That(keySet, Is.Empty);
            }
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
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

            Assert.That(testPatternList, Has.Count.GreaterThan(testPatternIndex));

            (var inputDic, var removeKeys) = testPatternList[testPatternIndex];

            var tsDictionary = new TimestampedDictionary<int>(dictionary: inputDic);

            Assert.That(tsDictionary.Count, Is.EqualTo(inputDic.Count));

            foreach (var removeKey in removeKeys)
            {
                bool removeResult = tsDictionary.TryRemove(removeKey, out var removeValue);

                Assert.That(removeResult, Is.True);

                //Assert.That(removeValue, Is.EqualTo(n));  // @notice
            }

            Assert.That(tsDictionary.Count, Is.EqualTo(inputDic.Count - removeKeys.Count));

            HashSet<string> keySet = tsDictionary.GetKeys();

            Assert.That(keySet.Count, Is.EqualTo(inputDic.Count - removeKeys.Count));

            foreach (var inParam in inputDic)
            {
                bool contains = keySet.Contains(inParam.Key);

                if (removeKeys.Exists(key => key == inParam.Key))
                {
                    Assert.That(contains, Is.False);
                }
                else
                {
                    Assert.That(contains, Is.True);
                }
            }

            tsDictionary.CheckConsistency();
        }

        [TestCase]
        public void Test1_GetAddedKeys_any()
        {
            var tsDictionary = new TimestampedDictionary<int>();

            {
                bool addResult;

                addResult = tsDictionary.TryAdd("key1", 11);

                Assert.That(addResult, Is.True);

                addResult = tsDictionary.TryAdd("key2", 12);

                Assert.That(addResult, Is.True);

                addResult = tsDictionary.TryAdd("key3", 13);

                Assert.That(addResult, Is.True);

                addResult = tsDictionary.TryAdd("key4", 14);

                Assert.That(addResult, Is.True);

                Assert.That(tsDictionary, Has.Count.EqualTo(4));
            }

            {
                List<string> addedKeys = tsDictionary.GetAddedKeys();

                Assert.That(addedKeys, Has.Count.EqualTo(4));

                Assert.That(addedKeys[0], Is.EqualTo("key1"));
                Assert.That(addedKeys[1], Is.EqualTo("key2"));
                Assert.That(addedKeys[2], Is.EqualTo("key3"));
                Assert.That(addedKeys[3], Is.EqualTo("key4"));
            }

            {
                bool removeResult = tsDictionary.TryRemove("key3", out var removeValue);

                Assert.That(removeResult, Is.True);

                Assert.That(removeValue, Is.EqualTo(13));

                Assert.That(tsDictionary, Has.Count.EqualTo(3));
            }

            {
                List<string> addedKeys = tsDictionary.GetAddedKeys();

                Assert.That(addedKeys, Has.Count.EqualTo(3));

                Assert.That(addedKeys[0], Is.EqualTo("key1"));
                Assert.That(addedKeys[1], Is.EqualTo("key2"));
                Assert.That(addedKeys[2], Is.EqualTo("key4"));
            }

            tsDictionary.CheckConsistency();
        }

        [TestCase]
        public async Task Test1_GetTimestampedKeys_any()
        {
            var tsDictionary = new TimestampedDictionary<int>();

            long beginTimestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            {
                bool addResult;

                await Task.Delay(10);

                addResult = tsDictionary.TryAdd("key1", 11);

                Assert.That(addResult, Is.True);

                await Task.Delay(10);

                addResult = tsDictionary.TryAdd("key2", 12);

                Assert.That(addResult, Is.True);

                await Task.Delay(10);

                addResult = tsDictionary.TryAdd("key3", 13);

                Assert.That(addResult, Is.True);

                await Task.Delay(10);

                addResult = tsDictionary.TryAdd("key4", 14);

                Assert.That(addResult, Is.True);

                Assert.That(tsDictionary, Has.Count.EqualTo(4));
            }

            {
                List<TimestampedKey> timestampedKeys = tsDictionary.GetTimestampedKeys();

                Assert.That(timestampedKeys, Has.Count.EqualTo(4));

                Assert.That(timestampedKeys[0].Key, Is.EqualTo("key1"));
                Assert.That(timestampedKeys[1].Key, Is.EqualTo("key2"));
                Assert.That(timestampedKeys[2].Key, Is.EqualTo("key3"));
                Assert.That(timestampedKeys[3].Key, Is.EqualTo("key4"));

                Assert.That(beginTimestamp, Is.LessThan(timestampedKeys[0].Timestamp));
                Assert.That(timestampedKeys[0].Timestamp, Is.LessThan(timestampedKeys[1].Timestamp));
                Assert.That(timestampedKeys[1].Timestamp, Is.LessThan(timestampedKeys[2].Timestamp));
                Assert.That(timestampedKeys[2].Timestamp, Is.LessThan(timestampedKeys[3].Timestamp));
            }

            {
                bool removeResult = tsDictionary.TryRemove("key3", out var removeValue);

                Assert.That(removeResult, Is.True);

                Assert.That(removeValue, Is.EqualTo(13));

                Assert.That(tsDictionary, Has.Count.EqualTo(3));
            }

            {
                List<TimestampedKey> timestampedKeys = tsDictionary.GetTimestampedKeys();

                Assert.That(timestampedKeys, Has.Count.EqualTo(3));

                Assert.That(timestampedKeys[0].Key, Is.EqualTo("key1"));
                Assert.That(timestampedKeys[1].Key, Is.EqualTo("key2"));
                Assert.That(timestampedKeys[2].Key, Is.EqualTo("key4"));

                Assert.That(beginTimestamp, Is.LessThan(timestampedKeys[0].Timestamp));
                Assert.That(timestampedKeys[0].Timestamp, Is.LessThan(timestampedKeys[1].Timestamp));
                Assert.That(timestampedKeys[1].Timestamp, Is.LessThan(timestampedKeys[2].Timestamp));
            }

            tsDictionary.CheckConsistency();
        }

        #endregion
    }
}