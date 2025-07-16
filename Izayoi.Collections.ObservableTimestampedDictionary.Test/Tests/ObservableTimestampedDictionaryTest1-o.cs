// ----------------------------------------------------------------------
// @Namespace : Izayoi.Collections.ObservableTimestampedDictionary.Test
// @Class     : ObservableTimestampedDictionaryTest1o
// ----------------------------------------------------------------------
namespace Izayoi.Collections.ObservableTimestampedDictionary.Test
{
    using Izayoi.Collections;
    using System;
    using System.Threading.Tasks;
    using TUnit.Core;

    public class ObservableTimestampedDictionaryTest1o
    {
        #region Add

        [Test]
        [Arguments("key1", 11)]
        [Arguments("key2", 12)]
        [Arguments("key3", 13)]
        public async ValueTask Test1_TryAdd_one(string key, int value)
        {
            using var tsDictionary = new ObservableTimestampedDictionary<int>();

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
        }

        #endregion

        #region Update

        [Test]
        public async Task Test1_TryUpdate()
        {
            using var tsDictionary = new ObservableTimestampedDictionary<int>();

            string key1 = "key1";
            string key2 = "key2";

            int value11 = 11;
            int value12 = 12;
            int value13 = 13;

            using IDisposable observer = tsDictionary
                .ObserveUpdate()
                .Subscribe(async ev =>
                {
                    await Assert.That(ev.OldData.Timestamp).IsLessThan(ev.NewData.Timestamp);

                    await Assert.That(ev.OldData.Key).IsEqualTo(key1);
                    await Assert.That(ev.NewData.Key).IsEqualTo(key1);

                    await Assert.That(ev.OldData.Value).IsEqualTo(value11);
                    await Assert.That(ev.NewData.Value).IsEqualTo(value13);
                });

            long timestamp1 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            await Task.Delay(10);

            {
                bool addResult = tsDictionary.TryAdd(key1, value11);

                await Assert.That(addResult).IsTrue();
            }

            await Task.Delay(10);

            long timestamp2 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            await Task.Delay(10);

            {
                bool addResult = tsDictionary.TryAdd(key2, value12);

                await Assert.That(addResult).IsTrue();
            }

            await Task.Delay(10);

            long timestamp3 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            await Task.Delay(10);

            {
                bool updateResult = tsDictionary.TryUpdate(key1, value13);

                await Assert.That(updateResult).IsTrue();
            }

            tsDictionary.CheckConsistency();
        }

        [Test]
        public async Task Test1_TryUpdate_with_comparison()
        {
            using var tsDictionary = new ObservableTimestampedDictionary<int>();

            string key1 = "key1";
            string key2 = "key2";

            int value11 = 11;
            int value12 = 12;
            int value13 = 13;

            using IDisposable observer = tsDictionary
                .ObserveUpdate()
                .Subscribe(async ev =>
                {
                    await Assert.That(ev.OldData.Timestamp).IsLessThan(ev.NewData.Timestamp);

                    await Assert.That(ev.OldData.Key).IsEqualTo(key1);
                    await Assert.That(ev.NewData.Key).IsEqualTo(key1);

                    await Assert.That(ev.OldData.Value).IsEqualTo(value11);
                    await Assert.That(ev.NewData.Value).IsEqualTo(value13);
                });

            long timestamp1 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            await Task.Delay(10);

            {
                bool addResult = tsDictionary.TryAdd(key1, value11);

                await Assert.That(addResult).IsTrue();
            }

            await Task.Delay(10);

            long timestamp2 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            await Task.Delay(10);

            {
                bool addResult = tsDictionary.TryAdd(key2, value12);

                await Assert.That(addResult).IsTrue();
            }

            await Task.Delay(10);

            long timestamp3 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            await Task.Delay(10);

            {
                bool updateResult = tsDictionary.TryUpdate(key1, value13, comparisonValue: value11);

                await Assert.That(updateResult).IsTrue();
            }

            tsDictionary.CheckConsistency();
        }

        #endregion

        #region AddOrUpdate

        [Test]
        public async Task Test1_AddOrUpdate()
        {
            using var tsDictionary = new ObservableTimestampedDictionary<int>();

            string key1 = "key1";
            string key2 = "key2";

            int value11 = 11;
            int value12 = 12;
            int value13 = 13;

            using IDisposable addObserver = tsDictionary
                .ObserveAdd()
                .Subscribe(async ev =>
                {
                    await Assert.That(ev.Data.Timestamp).IsGreaterThan(0);
                    await Assert.That(ev.Data.Value).IsGreaterThan(0);
                });

            using IDisposable updateObserver = tsDictionary
                .ObserveUpdate()
                .Subscribe(async ev =>
                {
                    await Assert.That(ev.OldData.Timestamp).IsLessThan(ev.NewData.Timestamp);
                    await Assert.That(ev.OldData.Timestamp).IsLessThan(ev.NewData.Timestamp);

                    await Assert.That(ev.OldData.Key).IsEqualTo(key1);
                    await Assert.That(ev.NewData.Key).IsEqualTo(key1);

                    await Assert.That(ev.OldData.Value).IsEqualTo(value11);
                    await Assert.That(ev.NewData.Value).IsEqualTo(value13);
                });

            TimestampedDictionaryData<int> dictionaryData;

            long timestamp1 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            await Task.Delay(10);

            dictionaryData = tsDictionary.AddOrUpdate(key1, value11);

            await Task.Delay(10);

            long timestamp2 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            await Task.Delay(10);

            dictionaryData = tsDictionary.AddOrUpdate(key2, value12);

            await Task.Delay(10);

            long timestamp3 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            await Task.Delay(10);

            dictionaryData = tsDictionary.AddOrUpdate(key1, value13);

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
            using var tsDictionary = new ObservableTimestampedDictionary<int>();

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
        public void Test1_Clear_zero()
        {
            using var tsDictionary = new ObservableTimestampedDictionary<int>();

            using IDisposable observer = tsDictionary
                .ObserveClear()
                .Subscribe(async removedCount =>
                {
                    await Assert.That(removedCount).IsEqualTo(0);
                });

            tsDictionary.Clear();
        }

        [Test]
        public async ValueTask Test1_Clear_one()
        {
            using var tsDictionary = new ObservableTimestampedDictionary<int>();

            using IDisposable observer = tsDictionary
                .ObserveClear()
                .Subscribe(async removedCount =>
                {
                    await Assert.That(removedCount).IsEqualTo(1);
                });

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
            using var tsDictionary = new ObservableTimestampedDictionary<int>();

            using IDisposable observer = tsDictionary
                .ObserveClear()
                .Subscribe(async removedCount =>
                {
                    await Assert.That(removedCount).IsEqualTo(2);
                });

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
        public async Task Test1_ClearBefore_DateTimeOffset()
        {
            using var tsDictionary = new ObservableTimestampedDictionary<int>();

            using IDisposable observer = tsDictionary
                .ObserveClear()
                .Subscribe(async removedCount =>
                {
                    await Assert.That(removedCount).IsEqualTo(2);
                });

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

        #region ObserveCountChange

        [Test]
        public async ValueTask Test1_ObserveCountChange()
        {
            using var tsDictionary = new ObservableTimestampedDictionary<int>();

            string key1 = "key1";
            string key2 = "key2";

            int value11 = 11;
            int value12 = 12;

            using IDisposable observer = tsDictionary
                .ObserveCountChange()
                .Subscribe(async count =>
                {
                    await Assert.That(tsDictionary.Count).IsEqualTo(count);
                });

            {
                bool addResult = tsDictionary.TryAdd(key1, value11);

                await Assert.That(addResult).IsTrue();
            }

            {
                bool addResult = tsDictionary.TryAdd(key2, value12);

                await Assert.That(addResult).IsTrue();
            }

            {
                bool removeResult = tsDictionary.TryRemove(key1, out var removeValue);

                await Assert.That(removeResult).IsTrue();
            }

            tsDictionary.CheckConsistency();
        }

        #endregion
    }
}