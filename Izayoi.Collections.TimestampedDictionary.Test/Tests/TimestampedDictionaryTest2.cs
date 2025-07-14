// ----------------------------------------------------------------------
// @Namespace : Izayoi.Collections.TimestampedDictionary.Test
// @Class     : TimestampedDictionaryTest2
// ----------------------------------------------------------------------
namespace Izayoi.Collections.TimestampedDictionary.Test
{
    using Izayoi.Collections;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class TimestampedDictionaryTest2
    {
        #region Construct

        [TestCase]
        public void Test2_Construct()
        {
            var tsDictionary = new TimestampedDictionary<int, string>();

            Assert.That(tsDictionary.Capacity, Is.EqualTo(-1));

            Assert.That(tsDictionary, Is.Empty);
        }

        [TestCase(-1)]
        [TestCase(1)]
        [TestCase(2)]
        public void Test2_Construct_with_correct_capacity(int capacity)
        {
            var tsDictionary = new TimestampedDictionary<int, string>(capacity);

            Assert.That(tsDictionary.Capacity, Is.EqualTo(capacity));
        }

        [TestCase(0)]
        [TestCase(-2)]
        [TestCase(-3)]
        public void Test2_Construct_with_invalid_capacity(int capacity)
        {
            var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                var tsDictionary = new TimestampedDictionary<int, string>(capacity);
            });

            Assert.That(exception.Source, Is.EqualTo("Izayoi.Collections.TimestampedDictionary"));
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
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

            Assert.That(testPatternList, Has.Count.GreaterThan(testPatternIndex));

            var initialData = testPatternList[testPatternIndex];

            var tsDictionary = new TimestampedDictionary<int, string>(dictionary: initialData);

            Assert.That(tsDictionary.Count, Is.EqualTo(initialData.Count));
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
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

            Assert.That(testPatternList, Has.Count.GreaterThan(testPatternIndex));

            var initialData = testPatternList[testPatternIndex];

            var tsDictionary = new TimestampedDictionary<int, string>(capacity: initialData.Count, dictionary: initialData);

            Assert.That(tsDictionary.Count, Is.EqualTo(initialData.Count));
        }

        [TestCase]
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

            Assert.That(exception.Source, Is.EqualTo("Izayoi.Collections.TimestampedDictionary"));
        }

        #endregion

        #region Property

        [TestCase]
        public void Test2_Property_Count()
        {
            var tsDictionary = new TimestampedDictionary<int, string>();

            Assert.That(tsDictionary, Is.Empty);

            {
                bool addResult = tsDictionary.TryAdd(1, "value1");

                Assert.That(addResult, Is.True);

                Assert.That(tsDictionary, Has.Count.EqualTo(1));
            }

            {
                bool addResult = tsDictionary.TryAdd(2, "value2");

                Assert.That(addResult, Is.True);

                Assert.That(tsDictionary, Has.Count.EqualTo(2));
            }

            {
                bool addResult = tsDictionary.TryAdd(3, "value3");

                Assert.That(addResult, Is.True);

                Assert.That(tsDictionary, Has.Count.EqualTo(3));
            }
        }

        #endregion

        #region Add

        [TestCase(11, "value11")]
        [TestCase(12, "value12")]
        [TestCase(13, "value13")]
        public void Test2_TryAdd_one(int key, string value)
        {
            var tsDictionary = new TimestampedDictionary<int, string>();

            bool addResult = tsDictionary.TryAdd(key, value);

            Assert.That(addResult, Is.True);

            Assert.That(tsDictionary, Has.Count.EqualTo(1));
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
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

            Assert.That(testPatternList, Has.Count.GreaterThan(testPatternIndex));

            var inputData = testPatternList[testPatternIndex];

            var tsDictionary = new TimestampedDictionary<int, string>();

            foreach (var inParam in inputData)
            {
                bool addResult = tsDictionary.TryAdd(inParam.Key, inParam.Value);

                Assert.That(addResult, Is.True);
            }

            Assert.That(tsDictionary, Has.Count.EqualTo(inputData.Count));
        }

        [TestCase(11, "value11")]
        public void Test2_TryAdd_duplicate_key(int key, string value)
        {
            var tsDictionary = new TimestampedDictionary<int, string>();

            bool addResult;

            addResult = tsDictionary.TryAdd(key, value);

            Assert.That(addResult, Is.True);

            Assert.That(tsDictionary, Has.Count.EqualTo(1));

            addResult = tsDictionary.TryAdd(key, value);

            Assert.That(addResult, Is.False);
        }

        #endregion

        #region Add and GetValue

        [TestCase(11, "value11")]
        [TestCase(12, "value12")]
        [TestCase(13, "value13")]
        [TestCase(14, "value14")]
        [TestCase(15, "value15")]
        public void Test2_TryAdd_and_TryGetValue_one(int key, string value)
        {
            var tsDictionary = new TimestampedDictionary<int, string>();

            {
                bool addResult = tsDictionary.TryAdd(key, value);

                Assert.That(addResult, Is.True);

                Assert.That(tsDictionary, Has.Count.EqualTo(1));
            }

            {
                bool getResult = tsDictionary.TryGetValue(key, out var getValue);

                Assert.That(getResult, Is.True);

                Assert.That(getValue, Is.EqualTo(value));
            }
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
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

            Assert.That(testPatternList, Has.Count.GreaterThan(testPatternIndex));

            var inputData = testPatternList[testPatternIndex];

            var tsDictionary = new TimestampedDictionary<int, string>(dictionary: inputData);

            Assert.That(tsDictionary, Has.Count.EqualTo(inputData.Count));

            foreach (var inParam in inputData)
            {
                bool getResult = tsDictionary.TryGetValue(inParam.Key, out var getValue);

                Assert.That(getResult, Is.True);

                Assert.That(getValue, Is.EqualTo(inParam.Value));
            }
        }

        [TestCase]
        public void Test2_TryAdd_and_TryGetValue_on_Capacity_1()
        {
            var tsDictionary = new TimestampedDictionary<int, string>(capacity: 1);

            {
                bool addResult = tsDictionary.TryAdd(11, "value11");

                Assert.That(addResult, Is.True);

                Assert.That(tsDictionary, Has.Count.EqualTo(1));

                bool getResult = tsDictionary.TryGetValue(11, out var getValue);

                Assert.That(getResult, Is.True);

                Assert.That(getValue, Is.EqualTo("value11"));
            }

            {
                bool addResult = tsDictionary.TryAdd(12, "value12");

                // key 11 is removed

                Assert.That(addResult, Is.True);

                Assert.That(tsDictionary, Has.Count.EqualTo(1));  // not 2

                bool getResult;

                string? getValue;

                getResult = tsDictionary.TryGetValue(11, out getValue);

                Assert.That(getResult, Is.False);

                getResult = tsDictionary.TryGetValue(12, out getValue);

                Assert.That(getResult, Is.True);

                Assert.That(getValue, Is.EqualTo("value12"));
            }

            tsDictionary.CheckConsistency();
        }

        [TestCase]
        public void Test2_TryAdd_and_TryGetValue_on_Capacity_2()
        {
            var tsDictionary = new TimestampedDictionary<int, string>(capacity: 2);

            bool addResult;
            bool getResult;
            string? getValue;

            {
                addResult = tsDictionary.TryAdd(11, "value11");

                Assert.That(addResult, Is.True);

                addResult = tsDictionary.TryAdd(12, "value12");

                Assert.That(addResult, Is.True);

                Assert.That(tsDictionary, Has.Count.EqualTo(2));
            }

            {
                addResult = tsDictionary.TryAdd(13, "value13");

                // key 11 is removed

                Assert.That(addResult, Is.True);

                Assert.That(tsDictionary, Has.Count.EqualTo(2));  // not 3

                getResult = tsDictionary.TryGetValue(11, out getValue);

                Assert.That(getResult, Is.False);

                getResult = tsDictionary.TryGetValue(12, out getValue);

                Assert.That(getResult, Is.True);

                Assert.That(getValue, Is.EqualTo("value12"));

                getResult = tsDictionary.TryGetValue(13, out getValue);

                Assert.That(getResult, Is.True);

                Assert.That(getValue, Is.EqualTo("value13"));
            }

            tsDictionary.CheckConsistency();
        }

        #endregion

        #region Update

        [TestCase]
        public async Task Test2_TryUpdate()
        {
            var tsDictionary = new TimestampedDictionary<int, string>();

            long timestamp1 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            await Task.Delay(10);

            {
                bool addResult = tsDictionary.TryAdd(11, "value1");

                Assert.That(addResult, Is.True);

                Assert.That(tsDictionary, Has.Count.EqualTo(1));
            }

            await Task.Delay(10);

            long timestamp2 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            await Task.Delay(10);

            {
                bool getResult = tsDictionary.TryGetData(11, out var getData);

                Assert.That(getResult, Is.True);

                Assert.That(getData, Is.Not.Null);

                Assert.That(getData.Timestamp, Is.InRange(timestamp1, timestamp2));

                Assert.That(getData.Key, Is.EqualTo(11));

                Assert.That(getData.Value, Is.EqualTo("value1"));
            }

            {
                bool addResult = tsDictionary.TryAdd(12, "value2");

                Assert.That(addResult, Is.True);

                Assert.That(tsDictionary, Has.Count.EqualTo(2));
            }

            {
                var timestampedKeys = tsDictionary.GetTimestampedKeys();

                Assert.That(timestampedKeys[0].Key, Is.EqualTo(11));
                Assert.That(timestampedKeys[1].Key, Is.EqualTo(12));
            }

            await Task.Delay(10);

            long timestamp3 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            await Task.Delay(10);

            {
                bool updateResult = tsDictionary.TryUpdate(11, "value3");

                Assert.That(updateResult, Is.True);

                Assert.That(tsDictionary, Has.Count.EqualTo(2));
            }

            await Task.Delay(10);

            long timestamp4 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            await Task.Delay(10);

            {
                bool getResult = tsDictionary.TryGetData(11, out var getData);

                Assert.That(getResult, Is.True);

                Assert.That(getData, Is.Not.Null);

                Assert.That(getData.Timestamp, Is.InRange(timestamp3, timestamp4));

                Assert.That(getData.Key, Is.EqualTo(11));

                Assert.That(getData.Value, Is.EqualTo("value3"));
            }

            {
                var timestampedKeys = tsDictionary.GetTimestampedKeys();

                Assert.That(timestampedKeys[0].Key, Is.EqualTo(12));
                Assert.That(timestampedKeys[1].Key, Is.EqualTo(11));

            }

            tsDictionary.CheckConsistency();
        }

        [TestCase]
        public async Task Test2_TryUpdate_with_comparison()
        {
            var tsDictionary = new TimestampedDictionary<int, string>();

            long timestamp1 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            await Task.Delay(10);

            {
                bool addResult = tsDictionary.TryAdd(11, "value1");

                Assert.That(addResult, Is.True);

                Assert.That(tsDictionary, Has.Count.EqualTo(1));
            }

            await Task.Delay(10);

            long timestamp2 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            await Task.Delay(10);

            {
                bool getResult = tsDictionary.TryGetData(11, out var getData);

                Assert.That(getResult, Is.True);

                Assert.That(getData, Is.Not.Null);

                Assert.That(getData.Timestamp, Is.InRange(timestamp1, timestamp2));

                Assert.That(getData.Key, Is.EqualTo(11));

                Assert.That(getData.Value, Is.EqualTo("value1"));
            }

            {
                bool addResult = tsDictionary.TryAdd(12, "value2");

                Assert.That(addResult, Is.True);

                Assert.That(tsDictionary, Has.Count.EqualTo(2));
            }

            {
                var timestampedKeys = tsDictionary.GetTimestampedKeys();

                Assert.That(timestampedKeys[0].Key, Is.EqualTo(11));
                Assert.That(timestampedKeys[1].Key, Is.EqualTo(12));
            }

            await Task.Delay(10);

            long timestamp3 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            await Task.Delay(10);

            {
                bool updateResult = tsDictionary.TryUpdate(11, "value3", comparisonValue: "unknown");

                Assert.That(updateResult, Is.False);

                Assert.That(tsDictionary, Has.Count.EqualTo(2));
            }

            await Task.Delay(10);

            long timestamp4 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            await Task.Delay(10);

            {
                bool updateResult = tsDictionary.TryUpdate(11, "value3", comparisonValue: "value1");

                Assert.That(updateResult, Is.True);

                Assert.That(tsDictionary, Has.Count.EqualTo(2));
            }

            await Task.Delay(10);

            long timestamp5 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            await Task.Delay(10);

            {
                bool getResult = tsDictionary.TryGetData(11, out var getData);

                Assert.That(getResult, Is.True);

                Assert.That(getData, Is.Not.Null);

                Assert.That(getData.Timestamp, Is.InRange(timestamp4, timestamp5));

                Assert.That(getData.Key, Is.EqualTo(11));

                Assert.That(getData.Value, Is.EqualTo("value3"));
            }

            {
                var timestampedKeys = tsDictionary.GetTimestampedKeys();

                Assert.That(timestampedKeys[0].Key, Is.EqualTo(12));
                Assert.That(timestampedKeys[1].Key, Is.EqualTo(11));
            }

            tsDictionary.CheckConsistency();
        }

        #endregion

        #region AddOrUpdate

        [TestCase]
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
                Assert.That(dictionaryData, Is.Not.Null);

                Assert.That(dictionaryData.Timestamp, Is.InRange(timestamp1, timestamp2));

                Assert.That(dictionaryData.Key, Is.EqualTo(11));

                Assert.That(dictionaryData.Value, Is.EqualTo("value1"));

                Assert.That(tsDictionary, Has.Count.EqualTo(1));
            }

            await Task.Delay(10);

            long timestamp3 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            await Task.Delay(10);

            dictionaryData = tsDictionary.AddOrUpdate(12, "value2");

            await Task.Delay(10);

            long timestamp4 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            await Task.Delay(10);

            {
                Assert.That(dictionaryData, Is.Not.Null);

                Assert.That(dictionaryData.Timestamp, Is.InRange(timestamp3, timestamp4));

                Assert.That(dictionaryData.Key, Is.EqualTo(12));

                Assert.That(dictionaryData.Value, Is.EqualTo("value2"));

                Assert.That(tsDictionary, Has.Count.EqualTo(2));
            }

            {
                var timestampedKeys = tsDictionary.GetTimestampedKeys();

                Assert.That(timestampedKeys[0].Key, Is.EqualTo(11));
                Assert.That(timestampedKeys[1].Key, Is.EqualTo(12));
            }

            await Task.Delay(10);

            long timestamp5 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            await Task.Delay(10);

            dictionaryData = tsDictionary.AddOrUpdate(11, "value3");

            await Task.Delay(10);

            long timestamp6 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            await Task.Delay(10);

            {
                Assert.That(dictionaryData, Is.Not.Null);

                Assert.That(dictionaryData.Timestamp, Is.InRange(timestamp5, timestamp6));

                Assert.That(dictionaryData.Key, Is.EqualTo(11));

                Assert.That(dictionaryData.Value, Is.EqualTo("value3"));

                Assert.That(tsDictionary, Has.Count.EqualTo(2));
            }

            {
                var timestampedKeys = tsDictionary.GetTimestampedKeys();

                Assert.That(timestampedKeys[0].Key, Is.EqualTo(12));
                Assert.That(timestampedKeys[1].Key, Is.EqualTo(11));
            }

            tsDictionary.CheckConsistency();
        }

        #endregion

        #region Remove

        [TestCase(11, "value11")]
        [TestCase(12, "value12")]
        [TestCase(13, "value13")]
        [TestCase(14, "value14")]
        public void Test2_TryRemove_one(int key, string value)
        {
            var tsDictionary = new TimestampedDictionary<int, string>();

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
                bool getResult = tsDictionary.TryGetValue(key, out var getValue);

                Assert.That(getResult, Is.False);

                Assert.That(getValue, Is.Null);
            }

            tsDictionary.CheckConsistency();
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
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

            Assert.That(testPatternList, Has.Count.GreaterThan(testPatternIndex));

            (var inputDic, var removeKeys) = testPatternList[testPatternIndex];

            var tsDictionary = new TimestampedDictionary<int, string>(dictionary: inputDic);

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
                bool getResult = tsDictionary.TryGetValue(inParam.Key, out var getValue);

                if (removeKeys.Exists(key => key == inParam.Key))
                {
                    Assert.That(getResult, Is.False);

                    Assert.That(getValue, Is.Null);
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
        public void Test2_Clear_zero()
        {
            var tsDictionary = new TimestampedDictionary<int, string>();

            Assert.That(tsDictionary, Is.Empty);

            tsDictionary.Clear();

            Assert.That(tsDictionary, Is.Empty);
        }

        [TestCase]
        public void Test2_Clear_one()
        {
            var tsDictionary = new TimestampedDictionary<int, string>();

            Assert.That(tsDictionary, Is.Empty);

            {
                bool addResult = tsDictionary.TryAdd(11, "value11");

                Assert.That(addResult, Is.True);

                Assert.That(tsDictionary, Has.Count.EqualTo(1));
            }

            tsDictionary.Clear();

            Assert.That(tsDictionary, Is.Empty);
        }

        [TestCase]
        public void Test2_Clear_two()
        {
            var tsDictionary = new TimestampedDictionary<int, string>();

            Assert.That(tsDictionary, Is.Empty);

            {
                bool addResult;

                addResult = tsDictionary.TryAdd(11, "value11");

                Assert.That(addResult, Is.True);

                Assert.That(tsDictionary, Has.Count.EqualTo(1));

                addResult = tsDictionary.TryAdd(12, "value12");

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
        public void Test2_Clear_with_newCapacity(int initialCapacity, int newCapacity)
        {
            var tsDictionary = new TimestampedDictionary<int, string>(initialCapacity);

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
        public void Test2_Clear_with_invalid_newCapacity(int initialCapacity, int newCapacity)
        {
            var tsDictionary = new TimestampedDictionary<int, string>(initialCapacity);

            Assert.That(tsDictionary.Capacity, Is.EqualTo(initialCapacity));

            var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                tsDictionary.Clear(newCapacity);
            });

            Assert.That(exception.Source, Is.EqualTo("Izayoi.Collections.TimestampedDictionary"));
        }

        [TestCase]
        public async Task Test2_ClearBefore_DateTimeOffset()
        {
            var tsDictionary = new TimestampedDictionary<int, string>();

            Assert.That(tsDictionary, Is.Empty);

            bool addResult;

            {
                addResult = tsDictionary.TryAdd(11, "value11");

                Assert.That(addResult, Is.True);

                Assert.That(tsDictionary, Has.Count.EqualTo(1));

                addResult = tsDictionary.TryAdd(12, "value12");

                Assert.That(addResult, Is.True);

                Assert.That(tsDictionary, Has.Count.EqualTo(2));
            }

            await Task.Delay(10);

            var timestamp = DateTimeOffset.UtcNow;

            await Task.Delay(10);

            {
                addResult = tsDictionary.TryAdd(13, "value13");

                Assert.That(addResult, Is.True);

                Assert.That(tsDictionary, Has.Count.EqualTo(3));
            }

            {
                int removedCount = tsDictionary.ClearBefore(timestamp);

                Assert.That(removedCount, Is.EqualTo(2));

                Assert.That(tsDictionary, Has.Count.EqualTo(1));

                bool getResult;

                getResult = tsDictionary.TryGetValue(11, out _);

                Assert.That(getResult, Is.False);

                getResult = tsDictionary.TryGetValue(12, out _);

                Assert.That(getResult, Is.False);

                getResult = tsDictionary.TryGetValue(13, out var value);

                Assert.That(getResult, Is.True);

                Assert.That(value, Is.EqualTo("value13"));
            }
        }

        #endregion

        #region Add and GetData

        [TestCase(11, "value11")]
        [TestCase(12, "value12")]
        [TestCase(13, "value13")]
        [TestCase(14, "value14")]
        [TestCase(15, "value15")]
        public async Task Test2_TryAdd_and_TryGetData_one(int key, string value)
        {
            var tsDictionary = new TimestampedDictionary<int, string>();

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

            Assert.That(testPatternList, Has.Count.GreaterThan(testPatternIndex));

            var inputData = testPatternList[testPatternIndex];

            long timestamp1 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            await Task.Delay(10);

            var tsDictionary = new TimestampedDictionary<int, string>(dictionary: inputData);

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
        public void Test2_ContainsKey()
        {
            var tsDictionary = new TimestampedDictionary<int, string>();

            {
                bool containsKey = tsDictionary.ContainsKey(1);

                Assert.That(containsKey, Is.False);
            }

            {
                bool addResult = tsDictionary.TryAdd(1, "value1");

                Assert.That(addResult, Is.True);
            }

            {
                bool containsKey = tsDictionary.ContainsKey(1);

                Assert.That(containsKey, Is.True);
            }

            {
                bool removeResult = tsDictionary.TryRemove(1, out _);

                Assert.That(removeResult, Is.True);
            }

            {
                bool containsKey = tsDictionary.ContainsKey(1);

                Assert.That(containsKey, Is.False);
            }
        }

        #endregion

        #region Keys

        [TestCase]
        public void Test2_GetKeys_zero()
        {
            var tsDictionary = new TimestampedDictionary<int, string>();

            HashSet<int> keySet = tsDictionary.GetKeys();

            Assert.That(keySet, Is.Empty);
        }

        [TestCase]
        public void Test2_GetKeys_zero_with_remove()
        {
            var tsDictionary = new TimestampedDictionary<int, string>();

            {
                bool addResult = tsDictionary.TryAdd(11, "va1ue11");

                Assert.That(addResult, Is.True);

                HashSet<int> keySet = tsDictionary.GetKeys();

                Assert.That(keySet, Has.Count.EqualTo(1));

                Assert.That(keySet.First(), Is.EqualTo(11));
            }

            {
                bool removeResult = tsDictionary.TryRemove(11, out _);

                Assert.That(removeResult, Is.True);

                HashSet<int> keySet = tsDictionary.GetKeys();

                Assert.That(keySet, Is.Empty);
            }
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
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

            Assert.That(testPatternList, Has.Count.GreaterThan(testPatternIndex));

            (var inputDic, var removeKeys) = testPatternList[testPatternIndex];

            var tsDictionary = new TimestampedDictionary<int, string>(dictionary: inputDic);

            Assert.That(tsDictionary.Count, Is.EqualTo(inputDic.Count));

            foreach (var removeKey in removeKeys)
            {
                bool removeResult = tsDictionary.TryRemove(removeKey, out var removeValue);

                Assert.That(removeResult, Is.True);

                //Assert.That(removeValue, Is.EqualTo(n));  // @notice
            }

            Assert.That(tsDictionary.Count, Is.EqualTo(inputDic.Count - removeKeys.Count));

            HashSet<int> keySet = tsDictionary.GetKeys();

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
        public void Test2_GetAddedKeys_any()
        {
            var tsDictionary = new TimestampedDictionary<int, string>();

            {
                bool addResult;

                addResult = tsDictionary.TryAdd(11, "value11");

                Assert.That(addResult, Is.True);

                addResult = tsDictionary.TryAdd(12, "value12");

                Assert.That(addResult, Is.True);

                addResult = tsDictionary.TryAdd(13, "value13");

                Assert.That(addResult, Is.True);

                addResult = tsDictionary.TryAdd(14, "value14");

                Assert.That(addResult, Is.True);

                Assert.That(tsDictionary, Has.Count.EqualTo(4));
            }

            {
                List<int> addedKeys = tsDictionary.GetAddedKeys();

                Assert.That(addedKeys, Has.Count.EqualTo(4));

                Assert.That(addedKeys[0], Is.EqualTo(11));
                Assert.That(addedKeys[1], Is.EqualTo(12));
                Assert.That(addedKeys[2], Is.EqualTo(13));
                Assert.That(addedKeys[3], Is.EqualTo(14));
            }

            {
                bool removeResult = tsDictionary.TryRemove(13, out var removeValue);

                Assert.That(removeResult, Is.True);

                Assert.That(removeValue, Is.EqualTo("value13"));

                Assert.That(tsDictionary, Has.Count.EqualTo(3));
            }

            {
                List<int> addedKeys = tsDictionary.GetAddedKeys();

                Assert.That(addedKeys, Has.Count.EqualTo(3));

                Assert.That(addedKeys[0], Is.EqualTo(11));
                Assert.That(addedKeys[1], Is.EqualTo(12));
                Assert.That(addedKeys[2], Is.EqualTo(14));
            }

            tsDictionary.CheckConsistency();
        }

        [TestCase]
        public async Task Test2_GetTimestampedKeys_any()
        {
            var tsDictionary = new TimestampedDictionary<int, string>();

            long beginTimestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            {
                bool addResult;

                await Task.Delay(10);

                addResult = tsDictionary.TryAdd(11, "value11");

                Assert.That(addResult, Is.True);

                await Task.Delay(10);

                addResult = tsDictionary.TryAdd(12, "value12");

                Assert.That(addResult, Is.True);

                await Task.Delay(10);

                addResult = tsDictionary.TryAdd(13, "value13");

                Assert.That(addResult, Is.True);

                await Task.Delay(10);

                addResult = tsDictionary.TryAdd(14, "value14");

                Assert.That(addResult, Is.True);

                Assert.That(tsDictionary, Has.Count.EqualTo(4));
            }

            {
                List<TimestampedKey<int>> timestampedKeys = tsDictionary.GetTimestampedKeys();

                Assert.That(timestampedKeys, Has.Count.EqualTo(4));

                Assert.That(timestampedKeys[0].Key, Is.EqualTo(11));
                Assert.That(timestampedKeys[1].Key, Is.EqualTo(12));
                Assert.That(timestampedKeys[2].Key, Is.EqualTo(13));
                Assert.That(timestampedKeys[3].Key, Is.EqualTo(14));

                Assert.That(beginTimestamp, Is.LessThan(timestampedKeys[0].Timestamp));
                Assert.That(timestampedKeys[0].Timestamp, Is.LessThan(timestampedKeys[1].Timestamp));
                Assert.That(timestampedKeys[1].Timestamp, Is.LessThan(timestampedKeys[2].Timestamp));
                Assert.That(timestampedKeys[2].Timestamp, Is.LessThan(timestampedKeys[3].Timestamp));
            }

            {
                bool removeResult = tsDictionary.TryRemove(13, out var removeValue);

                Assert.That(removeResult, Is.True);

                Assert.That(removeValue, Is.EqualTo("value13"));

                Assert.That(tsDictionary, Has.Count.EqualTo(3));
            }

            {
                List<TimestampedKey<int>> timestampedKeys = tsDictionary.GetTimestampedKeys();

                Assert.That(timestampedKeys, Has.Count.EqualTo(3));

                Assert.That(timestampedKeys[0].Key, Is.EqualTo(11));
                Assert.That(timestampedKeys[1].Key, Is.EqualTo(12));
                Assert.That(timestampedKeys[2].Key, Is.EqualTo(14));

                Assert.That(beginTimestamp, Is.LessThan(timestampedKeys[0].Timestamp));
                Assert.That(timestampedKeys[0].Timestamp, Is.LessThan(timestampedKeys[1].Timestamp));
                Assert.That(timestampedKeys[1].Timestamp, Is.LessThan(timestampedKeys[2].Timestamp));
            }

            tsDictionary.CheckConsistency();
        }

        #endregion
    }
}