// ----------------------------------------------------------------------
// @Namespace : Izayoi.Collections.ObservableTimestampedDictionary.Test
// @Class     : ObservableTimestampedDictionaryTest2
// ----------------------------------------------------------------------
namespace Izayoi.Collections.ObservableTimestampedDictionary.Test
{
    using Izayoi.Collections;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using TUnit.Core;

    public class ObservableTimestampedDictionaryTest2
    {
        #region Construct

        [Test]
        public async ValueTask Test2_Construct()
        {
            var tsDictionary = new ObservableTimestampedDictionary<int, string>();

            await Assert.That(tsDictionary.Capacity).IsEqualTo(-1);

            await Assert.That(tsDictionary.Count).IsEqualTo(0);
        }

        [Test]
        [Arguments(-1)]
        [Arguments(1)]
        [Arguments(2)]
        public async ValueTask Test2_Construct_with_correct_capacity(int capacity)
        {
            var tsDictionary = new ObservableTimestampedDictionary<int, string>(capacity);

            await Assert.That(tsDictionary.Capacity).IsEqualTo(capacity);
        }

        [Test]
        [Arguments(0)]
        [Arguments(-2)]
        [Arguments(-3)]
        public async ValueTask Test2_Construct_with_invalid_capacity(int capacity)
        {
            var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                var tsDictionary = new ObservableTimestampedDictionary<int, string>(capacity);
            });

            await Assert.That(exception.Source).IsEqualTo("Izayoi.Collections.ObservableTimestampedDictionary");
        }

        [Test]
        [Arguments(0)]
        [Arguments(1)]
        [Arguments(2)]
        public async ValueTask Test2_Construct_with_initial_data(int testPatternIndex)
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

            await Assert.That(testPatternList).HasCount().GreaterThan(testPatternIndex);

            var initialData = testPatternList[testPatternIndex];

            var tsDictionary = new ObservableTimestampedDictionary<int, string>(dictionary: initialData);

            await Assert.That(tsDictionary.Count).IsEqualTo(initialData.Count);
        }

        [Test]
        [Arguments(0)]
        [Arguments(1)]
        [Arguments(2)]
        public async ValueTask Test2_Construct_with_capacity_and_initial_data(int testPatternIndex)
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

            await Assert.That(testPatternList).HasCount().GreaterThan(testPatternIndex);

            var initialData = testPatternList[testPatternIndex];

            var tsDictionary = new ObservableTimestampedDictionary<int, string>(capacity: initialData.Count, dictionary: initialData);

            await Assert.That(tsDictionary.Count).IsEqualTo(initialData.Count);
        }

        [Test]
        public async ValueTask Test2_Construct_with_invalid_capacity_and_initial_data()
        {
            var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                var initialData = new Dictionary<int, string>()
                {
                    { 11, "value11" },
                    { 12, "value12" },
                    { 13, "value13" },
                };

                var tsDictionary = new ObservableTimestampedDictionary<int, string>(capacity: initialData.Count - 1, initialData);
            });

            await Assert.That(exception.Source).IsEqualTo("Izayoi.Collections.ObservableTimestampedDictionary");
        }

        #endregion

        #region Property

        [Test]
        public async ValueTask Test2_Property_Count()
        {
            var tsDictionary = new ObservableTimestampedDictionary<int, string>();

            await Assert.That(tsDictionary.Count).IsEqualTo(0);

            {
                bool addResult = tsDictionary.TryAdd(1, "value1");

                await Assert.That(addResult).IsTrue();

                await Assert.That(tsDictionary.Count).IsEqualTo(1);
            }

            {
                bool addResult = tsDictionary.TryAdd(2, "value2");

                await Assert.That(addResult).IsTrue();

                await Assert.That(tsDictionary.Count).IsEqualTo(2);
            }

            {
                bool addResult = tsDictionary.TryAdd(3, "value3");

                await Assert.That(addResult).IsTrue();

                await Assert.That(tsDictionary.Count).IsEqualTo(3);
            }
        }

        #endregion

        #region Add

        [Test]
        [Arguments(11, "value11")]
        [Arguments(12, "value12")]
        [Arguments(13, "value13")]
        public async ValueTask Test2_TryAdd_one(int key, string value)
        {
            var tsDictionary = new ObservableTimestampedDictionary<int, string>();

            bool addResult = tsDictionary.TryAdd(key, value);

            await Assert.That(addResult).IsTrue();

            await Assert.That(tsDictionary.Count).IsEqualTo(1);
        }

        [Test]
        [Arguments(0)]
        [Arguments(1)]
        [Arguments(2)]
        public async ValueTask Test2_TryAdd_any(int testPatternIndex)
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

            await Assert.That(testPatternList).HasCount().GreaterThan(testPatternIndex);

            var inputData = testPatternList[testPatternIndex];

            var tsDictionary = new ObservableTimestampedDictionary<int, string>();

            foreach (var inParam in inputData)
            {
                bool addResult = tsDictionary.TryAdd(inParam.Key, inParam.Value);

                await Assert.That(addResult).IsTrue();
            }

            await Assert.That(tsDictionary.Count).IsEqualTo(inputData.Count);
        }

        [Test]
        [Arguments(11, "value11")]
        public async ValueTask Test2_TryAdd_duplicate_key(int key, string value)
        {
            var tsDictionary = new ObservableTimestampedDictionary<int, string>();

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
        [Arguments(11, "value11")]
        [Arguments(12, "value12")]
        [Arguments(13, "value13")]
        [Arguments(14, "value14")]
        [Arguments(15, "value15")]
        public async ValueTask Test2_TryAdd_and_TryGetValue_one(int key, string value)
        {
            var tsDictionary = new ObservableTimestampedDictionary<int, string>();

            {
                bool addResult = tsDictionary.TryAdd(key, value);

                await Assert.That(addResult).IsTrue();

                await Assert.That(tsDictionary.Count).IsEqualTo(1);
            }

            {
                bool getResult = tsDictionary.TryGetValue(key, out var getValue);

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
        public async ValueTask Test2_TryAdd_and_TryGetValue_any(int testPatternIndex)
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

            await Assert.That(testPatternList).HasCount().GreaterThan(testPatternIndex);

            var inputData = testPatternList[testPatternIndex];

            var tsDictionary = new ObservableTimestampedDictionary<int, string>(dictionary: inputData);

            await Assert.That(tsDictionary.Count).IsEqualTo(inputData.Count);

            foreach (var inParam in inputData)
            {
                bool getResult = tsDictionary.TryGetValue(inParam.Key, out var getValue);

                await Assert.That(getResult).IsTrue();

                await Assert.That(getValue).IsEqualTo(inParam.Value);
            }
        }

        [Test]
        public async ValueTask Test2_TryAdd_and_TryGetValue_on_Capacity_1()
        {
            var tsDictionary = new ObservableTimestampedDictionary<int, string>(capacity: 1);

            {
                bool addResult = tsDictionary.TryAdd(11, "value11");

                await Assert.That(addResult).IsTrue();

                await Assert.That(tsDictionary.Count).IsEqualTo(1);

                bool getResult = tsDictionary.TryGetValue(11, out var getValue);

                await Assert.That(getResult).IsTrue();

                await Assert.That(getValue).IsEqualTo("value11");
            }

            {
                bool addResult = tsDictionary.TryAdd(12, "value12");

                // key 11 is removed

                await Assert.That(addResult).IsTrue();

                await Assert.That(tsDictionary.Count).IsEqualTo(1);  // not 2

                bool getResult;

                string? getValue;

                getResult = tsDictionary.TryGetValue(11, out getValue);

                await Assert.That(getResult).IsFalse();

                getResult = tsDictionary.TryGetValue(12, out getValue);

                await Assert.That(getResult).IsTrue();

                await Assert.That(getValue).IsEqualTo("value12");
            }

            tsDictionary.CheckConsistency();
        }

        [Test]
        public async ValueTask Test2_TryAdd_and_TryGetValue_on_Capacity_2()
        {
            var tsDictionary = new ObservableTimestampedDictionary<int, string>(capacity: 2);

            bool addResult;
            bool getResult;
            string? getValue;

            {
                addResult = tsDictionary.TryAdd(11, "value11");

                await Assert.That(addResult).IsTrue();

                addResult = tsDictionary.TryAdd(12, "value12");

                await Assert.That(addResult).IsTrue();

                await Assert.That(tsDictionary.Count).IsEqualTo(2);
            }

            {
                addResult = tsDictionary.TryAdd(13, "value13");

                // key 11 is removed

                await Assert.That(addResult).IsTrue();

                await Assert.That(tsDictionary.Count).IsEqualTo(2);  // not 3

                getResult = tsDictionary.TryGetValue(11, out getValue);

                await Assert.That(getResult).IsFalse();

                getResult = tsDictionary.TryGetValue(12, out getValue);

                await Assert.That(getResult).IsTrue();

                await Assert.That(getValue).IsEqualTo("value12");

                getResult = tsDictionary.TryGetValue(13, out getValue);

                await Assert.That(getResult).IsTrue();

                await Assert.That(getValue).IsEqualTo("value13");
            }

            tsDictionary.CheckConsistency();
        }

        #endregion

        #region Update

        [Test]
        public async Task Test2_TryUpdate()
        {
            var tsDictionary = new ObservableTimestampedDictionary<int, string>();

            long timestamp1 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            await Task.Delay(10);

            {
                bool addResult = tsDictionary.TryAdd(11, "value1");

                await Assert.That(addResult).IsTrue();

                await Assert.That(tsDictionary.Count).IsEqualTo(1);
            }

            await Task.Delay(10);

            long timestamp2 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            await Task.Delay(10);

            {
                bool getResult = tsDictionary.TryGetData(11, out var getData);

                await Assert.That(getResult).IsTrue();

                await Assert.That(getData).IsNotNull();

                await Assert.That(getData.Timestamp).IsBetween(timestamp1, timestamp2);

                await Assert.That(getData.Key).IsEqualTo(11);

                await Assert.That(getData.Value).IsEqualTo("value1");
            }

            {
                bool addResult = tsDictionary.TryAdd(12, "value2");

                await Assert.That(addResult).IsTrue();

                await Assert.That(tsDictionary.Count).IsEqualTo(2);
            }

            {
                var timestampedKeys = tsDictionary.GetTimestampedKeys();

                await Assert.That(timestampedKeys[0].Key).IsEqualTo(11);
                await Assert.That(timestampedKeys[1].Key).IsEqualTo(12);
            }

            await Task.Delay(10);

            long timestamp3 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            await Task.Delay(10);

            {
                bool updateResult = tsDictionary.TryUpdate(11, "value3");

                await Assert.That(updateResult).IsTrue();

                await Assert.That(tsDictionary.Count).IsEqualTo(2);
            }

            await Task.Delay(10);

            long timestamp4 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            await Task.Delay(10);

            {
                bool getResult = tsDictionary.TryGetData(11, out var getData);

                await Assert.That(getResult).IsTrue();

                await Assert.That(getData).IsNotNull();

                await Assert.That(getData.Timestamp).IsBetween(timestamp3, timestamp4);

                await Assert.That(getData.Key).IsEqualTo(11);

                await Assert.That(getData.Value).IsEqualTo("value3");
            }

            {
                var timestampedKeys = tsDictionary.GetTimestampedKeys();

                await Assert.That(timestampedKeys[0].Key).IsEqualTo(12);
                await Assert.That(timestampedKeys[1].Key).IsEqualTo(11);

            }

            tsDictionary.CheckConsistency();
        }

        [Test]
        public async Task Test2_TryUpdate_with_comparison()
        {
            var tsDictionary = new ObservableTimestampedDictionary<int, string>();

            long timestamp1 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            await Task.Delay(10);

            {
                bool addResult = tsDictionary.TryAdd(11, "value1");

                await Assert.That(addResult).IsTrue();

                await Assert.That(tsDictionary.Count).IsEqualTo(1);
            }

            await Task.Delay(10);

            long timestamp2 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            await Task.Delay(10);

            {
                bool getResult = tsDictionary.TryGetData(11, out var getData);

                await Assert.That(getResult).IsTrue();

                await Assert.That(getData).IsNotNull();

                await Assert.That(getData.Timestamp).IsBetween(timestamp1, timestamp2);

                await Assert.That(getData.Key).IsEqualTo(11);

                await Assert.That(getData.Value).IsEqualTo("value1");
            }

            {
                bool addResult = tsDictionary.TryAdd(12, "value2");

                await Assert.That(addResult).IsTrue();

                await Assert.That(tsDictionary.Count).IsEqualTo(2);
            }

            {
                var timestampedKeys = tsDictionary.GetTimestampedKeys();

                await Assert.That(timestampedKeys[0].Key).IsEqualTo(11);
                await Assert.That(timestampedKeys[1].Key).IsEqualTo(12);
            }

            await Task.Delay(10);

            long timestamp3 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            await Task.Delay(10);

            {
                bool updateResult = tsDictionary.TryUpdate(11, "value3", comparisonValue: "unknown");

                await Assert.That(updateResult).IsFalse();

                await Assert.That(tsDictionary.Count).IsEqualTo(2);
            }

            await Task.Delay(10);

            long timestamp4 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            await Task.Delay(10);

            {
                bool updateResult = tsDictionary.TryUpdate(11, "value3", comparisonValue: "value1");

                await Assert.That(updateResult).IsTrue();

                await Assert.That(tsDictionary.Count).IsEqualTo(2);
            }

            await Task.Delay(10);

            long timestamp5 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            await Task.Delay(10);

            {
                bool getResult = tsDictionary.TryGetData(11, out var getData);

                await Assert.That(getResult).IsTrue();

                await Assert.That(getData).IsNotNull();

                await Assert.That(getData.Timestamp).IsBetween(timestamp4, timestamp5);

                await Assert.That(getData.Key).IsEqualTo(11);

                await Assert.That(getData.Value).IsEqualTo("value3");
            }

            {
                var timestampedKeys = tsDictionary.GetTimestampedKeys();

                await Assert.That(timestampedKeys[0].Key).IsEqualTo(12);
                await Assert.That(timestampedKeys[1].Key).IsEqualTo(11);
            }

            tsDictionary.CheckConsistency();
        }

        #endregion

        #region AddOrUpdate

        [Test]
        public async Task Test2_AddOrUpdate()
        {
            var tsDictionary = new ObservableTimestampedDictionary<int, string>();

            TimestampedDictionaryData<int, string> dictionaryData;

            long timestamp1 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            await Task.Delay(10);

            dictionaryData = tsDictionary.AddOrUpdate(11, "value1");

            await Task.Delay(10);

            long timestamp2 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            await Task.Delay(10);

            {
                await Assert.That(dictionaryData).IsNotNull();

                await Assert.That(dictionaryData.Timestamp).IsBetween(timestamp1, timestamp2);

                await Assert.That(dictionaryData.Key).IsEqualTo(11);

                await Assert.That(dictionaryData.Value).IsEqualTo("value1");

                await Assert.That(tsDictionary.Count).IsEqualTo(1);
            }

            await Task.Delay(10);

            long timestamp3 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            await Task.Delay(10);

            dictionaryData = tsDictionary.AddOrUpdate(12, "value2");

            await Task.Delay(10);

            long timestamp4 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            await Task.Delay(10);

            {
                await Assert.That(dictionaryData).IsNotNull();

                await Assert.That(dictionaryData.Timestamp).IsBetween(timestamp3, timestamp4);

                await Assert.That(dictionaryData.Key).IsEqualTo(12);

                await Assert.That(dictionaryData.Value).IsEqualTo("value2");

                await Assert.That(tsDictionary.Count).IsEqualTo(2);
            }

            {
                var timestampedKeys = tsDictionary.GetTimestampedKeys();

                await Assert.That(timestampedKeys[0].Key).IsEqualTo(11);
                await Assert.That(timestampedKeys[1].Key).IsEqualTo(12);
            }

            await Task.Delay(10);

            long timestamp5 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            await Task.Delay(10);

            dictionaryData = tsDictionary.AddOrUpdate(11, "value3");

            await Task.Delay(10);

            long timestamp6 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            await Task.Delay(10);

            {
                await Assert.That(dictionaryData).IsNotNull();

                await Assert.That(dictionaryData.Timestamp).IsBetween(timestamp5, timestamp6);

                await Assert.That(dictionaryData.Key).IsEqualTo(11);

                await Assert.That(dictionaryData.Value).IsEqualTo("value3");

                await Assert.That(tsDictionary.Count).IsEqualTo(2);
            }

            {
                var timestampedKeys = tsDictionary.GetTimestampedKeys();

                await Assert.That(timestampedKeys[0].Key).IsEqualTo(12);
                await Assert.That(timestampedKeys[1].Key).IsEqualTo(11);
            }

            tsDictionary.CheckConsistency();
        }

        #endregion

        #region Remove

        [Test]
        [Arguments(11, "value11")]
        [Arguments(12, "value12")]
        [Arguments(13, "value13")]
        [Arguments(14, "value14")]
        public async ValueTask Test2_TryRemove_one(int key, string value)
        {
            var tsDictionary = new ObservableTimestampedDictionary<int, string>();

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
                bool getResult = tsDictionary.TryGetValue(key, out var getValue);

                await Assert.That(getResult).IsFalse();

                await Assert.That(getValue).IsNull();
            }

            tsDictionary.CheckConsistency();
        }

        [Test]
        [Arguments(0)]
        [Arguments(1)]
        [Arguments(2)]
        [Arguments(3)]
        public async ValueTask Test2_TryRemove_any(int testPatternIndex)
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

            await Assert.That(testPatternList).HasCount().GreaterThan(testPatternIndex);

            (var inputDic, var removeKeys) = testPatternList[testPatternIndex];

            var tsDictionary = new ObservableTimestampedDictionary<int, string>(dictionary: inputDic);

            await Assert.That(tsDictionary.Count).IsEqualTo(inputDic.Count);

            foreach (var removeKey in removeKeys)
            {
                bool removeResult = tsDictionary.TryRemove(removeKey, out var removeValue);

                await Assert.That(removeResult).IsTrue();

                //await Assert.That(removeValue).IsEqualTo(n);  // @notice
            }

            await Assert.That(tsDictionary.Count).IsEqualTo(inputDic.Count - removeKeys.Count);

            foreach (var inParam in inputDic)
            {
                bool getResult = tsDictionary.TryGetValue(inParam.Key, out var getValue);

                if (removeKeys.Exists(key => key == inParam.Key))
                {
                    await Assert.That(getResult).IsFalse();

                    await Assert.That(getValue).IsNull();
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
        public async ValueTask Test2_Clear_zero()
        {
            var tsDictionary = new ObservableTimestampedDictionary<int, string>();

            await Assert.That(tsDictionary.Count).IsEqualTo(0);

            tsDictionary.Clear();

            await Assert.That(tsDictionary.Count).IsEqualTo(0);
        }

        [Test]
        public async ValueTask Test2_Clear_one()
        {
            var tsDictionary = new ObservableTimestampedDictionary<int, string>();

            await Assert.That(tsDictionary.Count).IsEqualTo(0);

            {
                bool addResult = tsDictionary.TryAdd(11, "value11");

                await Assert.That(addResult).IsTrue();

                await Assert.That(tsDictionary.Count).IsEqualTo(1);
            }

            tsDictionary.Clear();

            await Assert.That(tsDictionary.Count).IsEqualTo(0);
        }

        [Test]
        public async ValueTask Test2_Clear_two()
        {
            var tsDictionary = new ObservableTimestampedDictionary<int, string>();

            await Assert.That(tsDictionary.Count).IsEqualTo(0);

            {
                bool addResult;

                addResult = tsDictionary.TryAdd(11, "value11");

                await Assert.That(addResult).IsTrue();

                await Assert.That(tsDictionary.Count).IsEqualTo(1);

                addResult = tsDictionary.TryAdd(12, "value12");

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
        public async ValueTask Test2_Clear_with_newCapacity(int initialCapacity, int newCapacity)
        {
            var tsDictionary = new ObservableTimestampedDictionary<int, string>(initialCapacity);

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
        public async ValueTask Test2_Clear_with_invalid_newCapacity(int initialCapacity, int newCapacity)
        {
            var tsDictionary = new ObservableTimestampedDictionary<int, string>(initialCapacity);

            await Assert.That(tsDictionary.Capacity).IsEqualTo(initialCapacity);

            var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                tsDictionary.Clear(newCapacity);
            });

            await Assert.That(exception.Source).IsEqualTo("Izayoi.Collections.ObservableTimestampedDictionary");
        }

        [Test]
        public async Task Test2_ClearBefore_DateTimeOffset()
        {
            var tsDictionary = new ObservableTimestampedDictionary<int, string>();

            await Assert.That(tsDictionary.Count).IsEqualTo(0);

            bool addResult;

            {
                addResult = tsDictionary.TryAdd(11, "value11");

                await Assert.That(addResult).IsTrue();

                await Assert.That(tsDictionary.Count).IsEqualTo(1);

                addResult = tsDictionary.TryAdd(12, "value12");

                await Assert.That(addResult).IsTrue();

                await Assert.That(tsDictionary.Count).IsEqualTo(2);
            }

            await Task.Delay(10);

            var timestamp = DateTimeOffset.UtcNow;

            await Task.Delay(10);

            {
                addResult = tsDictionary.TryAdd(13, "value13");

                await Assert.That(addResult).IsTrue();

                await Assert.That(tsDictionary.Count).IsEqualTo(3);
            }

            {
                int removedCount = tsDictionary.ClearBefore(timestamp);

                await Assert.That(removedCount).IsEqualTo(2);

                await Assert.That(tsDictionary.Count).IsEqualTo(1);

                bool getResult;

                getResult = tsDictionary.TryGetValue(11, out _);

                await Assert.That(getResult).IsFalse();

                getResult = tsDictionary.TryGetValue(12, out _);

                await Assert.That(getResult).IsFalse();

                getResult = tsDictionary.TryGetValue(13, out var value);

                await Assert.That(getResult).IsTrue();

                await Assert.That(value).IsEqualTo("value13");
            }
        }

        #endregion

        #region Add and GetData

        [Test]
        [Arguments(11, "value11")]
        [Arguments(12, "value12")]
        [Arguments(13, "value13")]
        [Arguments(14, "value14")]
        [Arguments(15, "value15")]
        public async Task Test2_TryAdd_and_TryGetData_one(int key, string value)
        {
            var tsDictionary = new ObservableTimestampedDictionary<int, string>();

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

            await Assert.That(testPatternList).HasCount().GreaterThan(testPatternIndex);

            var inputData = testPatternList[testPatternIndex];

            long timestamp1 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            await Task.Delay(10);

            var tsDictionary = new ObservableTimestampedDictionary<int, string>(dictionary: inputData);

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
        public async ValueTask Test2_ContainsKey()
        {
            var tsDictionary = new ObservableTimestampedDictionary<int, string>();

            {
                bool containsKey = tsDictionary.ContainsKey(1);

                await Assert.That(containsKey).IsFalse();
            }

            {
                bool addResult = tsDictionary.TryAdd(1, "value1");

                await Assert.That(addResult).IsTrue();
            }

            {
                bool containsKey = tsDictionary.ContainsKey(1);

                await Assert.That(containsKey).IsTrue();
            }

            {
                bool removeResult = tsDictionary.TryRemove(1, out _);

                await Assert.That(removeResult).IsTrue();
            }

            {
                bool containsKey = tsDictionary.ContainsKey(1);

                await Assert.That(containsKey).IsFalse();
            }
        }

        #endregion

        #region Keys

        [Test]
        public async ValueTask Test2_GetKeys_zero()
        {
            var tsDictionary = new ObservableTimestampedDictionary<int, string>();

            HashSet<int> keySet = tsDictionary.GetKeys();

            await Assert.That(keySet).IsEmpty();
        }

        [Test]
        public async ValueTask Test2_GetKeys_zero_with_remove()
        {
            var tsDictionary = new ObservableTimestampedDictionary<int, string>();

            {
                bool addResult = tsDictionary.TryAdd(11, "va1ue11");

                await Assert.That(addResult).IsTrue();

                HashSet<int> keySet = tsDictionary.GetKeys();

                await Assert.That(keySet.Count).IsEqualTo(1);

                await Assert.That(keySet.First()).IsEqualTo(11);
            }

            {
                bool removeResult = tsDictionary.TryRemove(11, out _);

                await Assert.That(removeResult).IsTrue();

                HashSet<int> keySet = tsDictionary.GetKeys();

                await Assert.That(keySet).IsEmpty();
            }
        }

        [Test]
        [Arguments(0)]
        [Arguments(1)]
        [Arguments(2)]
        [Arguments(3)]
        public async ValueTask Test2_GetKeys_any(int testPatternIndex)
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

            await Assert.That(testPatternList).HasCount().GreaterThan(testPatternIndex);

            (var inputDic, var removeKeys) = testPatternList[testPatternIndex];

            var tsDictionary = new ObservableTimestampedDictionary<int, string>(dictionary: inputDic);

            await Assert.That(tsDictionary.Count).IsEqualTo(inputDic.Count);

            foreach (var removeKey in removeKeys)
            {
                bool removeResult = tsDictionary.TryRemove(removeKey, out var removeValue);

                await Assert.That(removeResult).IsTrue();

                //await Assert.That(removeValue).IsEqualTo(n);  // @notice
            }

            await Assert.That(tsDictionary.Count).IsEqualTo(inputDic.Count - removeKeys.Count);

            HashSet<int> keySet = tsDictionary.GetKeys();

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
        public async ValueTask Test2_GetAddedKeys_any()
        {
            var tsDictionary = new ObservableTimestampedDictionary<int, string>();

            {
                bool addResult;

                addResult = tsDictionary.TryAdd(11, "value11");

                await Assert.That(addResult).IsTrue();

                addResult = tsDictionary.TryAdd(12, "value12");

                await Assert.That(addResult).IsTrue();

                addResult = tsDictionary.TryAdd(13, "value13");

                await Assert.That(addResult).IsTrue();

                addResult = tsDictionary.TryAdd(14, "value14");

                await Assert.That(addResult).IsTrue();

                await Assert.That(tsDictionary.Count).IsEqualTo(4);
            }

            {
                List<int> addedKeys = tsDictionary.GetAddedKeys();

                await Assert.That(addedKeys).HasCount().EqualTo(4);

                await Assert.That(addedKeys[0]).IsEqualTo(11);
                await Assert.That(addedKeys[1]).IsEqualTo(12);
                await Assert.That(addedKeys[2]).IsEqualTo(13);
                await Assert.That(addedKeys[3]).IsEqualTo(14);
            }

            {
                bool removeResult = tsDictionary.TryRemove(13, out var removeValue);

                await Assert.That(removeResult).IsTrue();

                await Assert.That(removeValue).IsEqualTo("value13");

                await Assert.That(tsDictionary.Count).IsEqualTo(3);
            }

            {
                List<int> addedKeys = tsDictionary.GetAddedKeys();

                await Assert.That(addedKeys).HasCount().EqualTo(3);

                await Assert.That(addedKeys[0]).IsEqualTo(11);
                await Assert.That(addedKeys[1]).IsEqualTo(12);
                await Assert.That(addedKeys[2]).IsEqualTo(14);
            }

            tsDictionary.CheckConsistency();
        }

        [Test]
        public async Task Test2_GetTimestampedKeys_any()
        {
            var tsDictionary = new ObservableTimestampedDictionary<int, string>();

            long beginTimestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            {
                bool addResult;

                await Task.Delay(10);

                addResult = tsDictionary.TryAdd(11, "value11");

                await Assert.That(addResult).IsTrue();

                await Task.Delay(10);

                addResult = tsDictionary.TryAdd(12, "value12");

                await Assert.That(addResult).IsTrue();

                await Task.Delay(10);

                addResult = tsDictionary.TryAdd(13, "value13");

                await Assert.That(addResult).IsTrue();

                await Task.Delay(10);

                addResult = tsDictionary.TryAdd(14, "value14");

                await Assert.That(addResult).IsTrue();

                await Assert.That(tsDictionary.Count).IsEqualTo(4);
            }

            {
                List<TimestampedKey<int>> timestampedKeys = tsDictionary.GetTimestampedKeys();

                await Assert.That(timestampedKeys).HasCount().EqualTo(4);

                await Assert.That(timestampedKeys[0].Key).IsEqualTo(11);
                await Assert.That(timestampedKeys[1].Key).IsEqualTo(12);
                await Assert.That(timestampedKeys[2].Key).IsEqualTo(13);
                await Assert.That(timestampedKeys[3].Key).IsEqualTo(14);

                await Assert.That(beginTimestamp).IsLessThan(timestampedKeys[0].Timestamp);
                await Assert.That(timestampedKeys[0].Timestamp).IsLessThan(timestampedKeys[1].Timestamp);
                await Assert.That(timestampedKeys[1].Timestamp).IsLessThan(timestampedKeys[2].Timestamp);
                await Assert.That(timestampedKeys[2].Timestamp).IsLessThan(timestampedKeys[3].Timestamp);
            }

            {
                bool removeResult = tsDictionary.TryRemove(13, out var removeValue);

                await Assert.That(removeResult).IsTrue();

                await Assert.That(removeValue).IsEqualTo("value13");

                await Assert.That(tsDictionary.Count).IsEqualTo(3);
            }

            {
                List<TimestampedKey<int>> timestampedKeys = tsDictionary.GetTimestampedKeys();

                await Assert.That(timestampedKeys).HasCount().EqualTo(3);

                await Assert.That(timestampedKeys[0].Key).IsEqualTo(11);
                await Assert.That(timestampedKeys[1].Key).IsEqualTo(12);
                await Assert.That(timestampedKeys[2].Key).IsEqualTo(14);

                await Assert.That(beginTimestamp).IsLessThan(timestampedKeys[0].Timestamp);
                await Assert.That(timestampedKeys[0].Timestamp).IsLessThan(timestampedKeys[1].Timestamp);
                await Assert.That(timestampedKeys[1].Timestamp).IsLessThan(timestampedKeys[2].Timestamp);
            }

            tsDictionary.CheckConsistency();
        }

        #endregion
    }
}