// ----------------------------------------------------------------------
// @Namespace : Izayoi.Collections.ObservableTimestampedDictionary.Test
// @Class     : ObservableTimestampedDictionaryTest2o
// ----------------------------------------------------------------------
namespace Izayoi.Collections.ObservableTimestampedDictionary.Test
{
    using Izayoi.Collections;
    using System;
    using System.Threading.Tasks;
    using TUnit.Core;

    public class ObservableTimestampedDictionaryTest2o
    {
        #region Add

        [Test]
        [Arguments(11, "value11")]
        [Arguments(12, "value12")]
        [Arguments(13, "value13")]
        public async ValueTask Test2_TryAdd_one(int key, string value)
        {
            using var tsDictionary = new ObservableTimestampedDictionary<int, string>();

            using IDisposable observer = tsDictionary
                .ObserveAdd()
                .Subscribe(async ev =>
                {
                    await Assert.That(ev.Data.Timestamp).IsGreaterThan(0);
                    await Assert.That(ev.Data.Key).IsEqualTo(key);
                    await Assert.That(ev.Data.Value).IsEqualTo(value);
                });

            bool addResult = tsDictionary.TryAdd(key, value);

            await Assert.That(addResult).IsTrue();

            await Assert.That(tsDictionary.Count).IsEqualTo(1);
        }

        #endregion

        #region AddOrUpdate

        [Test]
        public async Task Test1_AddOrUpdate()
        {
            using var tsDictionary = new ObservableTimestampedDictionary<int, string>();

            int key11 = 11;
            int key12 = 12;

            string value1 = "value1";
            string value2 = "value2";
            string value3 = "value3";

            using IDisposable addObserver = tsDictionary
                .ObserveAdd()
                .Subscribe(async ev =>
                {
                    await Assert.That(ev.Data.Timestamp).IsGreaterThan(0);
                    await Assert.That(ev.Data.Key).IsGreaterThan(0);
                });

            using IDisposable updateObserver = tsDictionary
                .ObserveUpdate()
                .Subscribe(async ev =>
                {
                    await Assert.That(ev.OldData.Timestamp).IsLessThan(ev.NewData.Timestamp);

                    await Assert.That(ev.OldData.Key).IsEqualTo(key11);
                    await Assert.That(ev.NewData.Key).IsEqualTo(key11);

                    await Assert.That(ev.OldData.Value).IsEqualTo(value1);
                    await Assert.That(ev.NewData.Value).IsEqualTo(value3);
                });

            TimestampedDictionaryData<int, string> dictionaryData;

            long timestamp1 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            await Task.Delay(10);

            dictionaryData = tsDictionary.AddOrUpdate(key11, value1);

            await Task.Delay(10);

            long timestamp2 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            await Task.Delay(10);

            dictionaryData = tsDictionary.AddOrUpdate(key12, value2);

            await Task.Delay(10);

            long timestamp3 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            await Task.Delay(10);

            dictionaryData = tsDictionary.AddOrUpdate(key11, value3);

            tsDictionary.CheckConsistency();
        }

        #endregion

        #region Update

        [Test]
        public async Task Test2_TryUpdate()
        {
            using var tsDictionary = new ObservableTimestampedDictionary<int, string>();

            int key11 = 11;
            int key12 = 12;

            string value1 = "value1";
            string value2 = "value2";
            string value3 = "value3";

            using IDisposable observer = tsDictionary
                .ObserveUpdate()
                .Subscribe(async ev =>
                {
                    await Assert.That(ev.OldData.Timestamp).IsLessThan(ev.NewData.Timestamp);

                    await Assert.That(ev.OldData.Key).IsEqualTo(key11);
                    await Assert.That(ev.NewData.Key).IsEqualTo(key11);

                    await Assert.That(ev.OldData.Value).IsEqualTo(value1);
                    await Assert.That(ev.NewData.Value).IsEqualTo(value3);
                });

            long timestamp1 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            await Task.Delay(10);

            {
                bool addResult = tsDictionary.TryAdd(key11, value1);

                await Assert.That(addResult).IsTrue();
            }

            await Task.Delay(10);

            long timestamp2 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            await Task.Delay(10);

            {
                bool addResult = tsDictionary.TryAdd(key12, value2);

                await Assert.That(addResult).IsTrue();
            }

            await Task.Delay(10);

            long timestamp3 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            await Task.Delay(10);

            {
                bool updateResult = tsDictionary.TryUpdate(key11, value3);

                await Assert.That(updateResult).IsTrue();
            }

            tsDictionary.CheckConsistency();
        }

        [Test]
        public async Task Test2_TryUpdate_with_comparison()
        {
            using var tsDictionary = new ObservableTimestampedDictionary<int, string>();

            int key11 = 11;
            int key12 = 12;

            string value1 = "value1";
            string value2 = "value2";
            string value3 = "value3";

            using IDisposable observer = tsDictionary
                .ObserveUpdate()
                .Subscribe(async ev =>
                {
                    await Assert.That(ev.OldData.Timestamp).IsLessThan(ev.NewData.Timestamp);

                    await Assert.That(ev.OldData.Key).IsEqualTo(key11);
                    await Assert.That(ev.NewData.Key).IsEqualTo(key11);

                    await Assert.That(ev.OldData.Value).IsEqualTo(value1);
                    await Assert.That(ev.NewData.Value).IsEqualTo(value3);
                });

            long timestamp1 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            await Task.Delay(10);

            {
                bool addResult = tsDictionary.TryAdd(key11, value1);

                await Assert.That(addResult).IsTrue();
            }

            await Task.Delay(10);

            long timestamp2 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            await Task.Delay(10);

            {
                bool addResult = tsDictionary.TryAdd(key12, value2);

                await Assert.That(addResult).IsTrue();
            }

            await Task.Delay(10);

            long timestamp3 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            await Task.Delay(10);

            {
                bool updateResult = tsDictionary.TryUpdate(key11, value3, comparisonValue: value1);

                await Assert.That(updateResult).IsTrue();
            }

            tsDictionary.CheckConsistency();
        }

        #endregion

        #region AddOrUpdate

        [Test]
        public async Task Test2_AddOrUpdate()
        {
            using var tsDictionary = new ObservableTimestampedDictionary<int, string>();

            int key11 = 11;
            int key12 = 12;

            string value1 = "value1";
            string value2 = "value2";
            string value3 = "value3";

            using IDisposable observer = tsDictionary
                .ObserveUpdate()
                .Subscribe(async ev =>
                {
                    await Assert.That(ev.OldData.Timestamp).IsLessThan(ev.NewData.Timestamp);

                    await Assert.That(ev.OldData.Key).IsEqualTo(key11);
                    await Assert.That(ev.NewData.Key).IsEqualTo(key11);

                    await Assert.That(ev.OldData.Value).IsEqualTo(value1);
                    await Assert.That(ev.NewData.Value).IsEqualTo(value3);
                });

            TimestampedDictionaryData<int, string> dictionaryData;

            long timestamp1 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            await Task.Delay(10);

            dictionaryData = tsDictionary.AddOrUpdate(key11, value1);

            await Task.Delay(10);

            long timestamp2 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            await Task.Delay(10);

            dictionaryData = tsDictionary.AddOrUpdate(key12, value2);

            await Task.Delay(10);

            long timestamp3 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            await Task.Delay(10);

            dictionaryData = tsDictionary.AddOrUpdate(key11, value3);

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
            using var tsDictionary = new ObservableTimestampedDictionary<int, string>();

            using IDisposable observer = tsDictionary
                .ObserveRemove()
                .Subscribe(async ev =>
                {
                    await Assert.That(ev.Data.Timestamp).IsGreaterThan(0);
                    await Assert.That(ev.Data.Key).IsEqualTo(key);
                    await Assert.That(ev.Data.Value).IsEqualTo(value);
                });

            {
                bool addResult = tsDictionary.TryAdd(key, value);

                await Assert.That(addResult).IsTrue();
            }

            {
                bool removeResult = tsDictionary.TryRemove(key, out var removeValue);

                await Assert.That(removeResult).IsTrue();
            }

            tsDictionary.CheckConsistency();
        }

        #endregion

        #region Clear

        [Test]
        public void Test2_Clear_zero()
        {
            using var tsDictionary = new ObservableTimestampedDictionary<int, string>();

            using IDisposable observer = tsDictionary
                .ObserveClear()
                .Subscribe(async removedCount =>
                {
                    await Assert.That(removedCount).IsEqualTo(9);
                });

            tsDictionary.Clear();
        }

        [Test]
        public async ValueTask Test2_Clear_one()
        {
            using var tsDictionary = new ObservableTimestampedDictionary<int, string>();

            using IDisposable observer = tsDictionary
                .ObserveClear()
                .Subscribe(async removedCount =>
                {
                    await Assert.That(removedCount).IsEqualTo(1);
                });

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
            using var tsDictionary = new ObservableTimestampedDictionary<int, string>();

            using IDisposable observer = tsDictionary
                .ObserveClear()
                .Subscribe(async removedCount =>
                {
                    await Assert.That(removedCount).IsEqualTo(2);
                });

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
        public async Task Test2_ClearBefore_DateTimeOffset()
        {
            using var tsDictionary = new ObservableTimestampedDictionary<int, string>();

            using IDisposable observer = tsDictionary
                .ObserveClear()
                .Subscribe(async removedCount =>
                {
                    await Assert.That(removedCount).IsEqualTo(2);
                });

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

        #region ObserveCountChange

        [Test]
        public async ValueTask Test2_ObserveCountChange()
        {
            using var tsDictionary = new ObservableTimestampedDictionary<int, string>();

            int key11 = 11;
            int key12 = 12;

            string value1 = "value1";
            string value2 = "value2";

            using IDisposable observer = tsDictionary
                .ObserveCountChange()
                .Subscribe(async count =>
                {
                    await Assert.That(tsDictionary.Count).IsEqualTo(count);
                });

            {
                bool addResult = tsDictionary.TryAdd(key11, value1);

                await Assert.That(addResult).IsTrue();
            }

            {
                bool addResult = tsDictionary.TryAdd(key12, value2);

                await Assert.That(addResult).IsTrue();
            }

            {
                bool removeResult = tsDictionary.TryRemove(key11, out var removeValue);

                await Assert.That(removeResult).IsTrue();
            }

            tsDictionary.CheckConsistency();
        }

        #endregion
    }
}