// ----------------------------------------------------------------------
// @Namespace : Izayoi.Collections.ObservableTimestampedDictionary.Test
// @Class     : ObservableTimestampedDictionaryTest1o
// ----------------------------------------------------------------------
namespace Izayoi.Collections.ObservableTimestampedDictionary.Test
{
    using Izayoi.Collections;
    using NUnit.Framework;
    using System;
    using System.Threading.Tasks;

    [TestFixture]
    public class ObservableTimestampedDictionaryTest1o
    {
        #region Add

        [TestCase("key1", 11)]
        [TestCase("key2", 12)]
        [TestCase("key3", 13)]
        public void Test1_TryAdd_one(string key, int value)
        {
            using var tsDictionary = new ObservableTimestampedDictionary<int>();

            using IDisposable observer = tsDictionary
                .ObserveAdd()
                .Subscribe(ev =>
                {
                    Assert.That(ev.Data.Timestamp, Is.GreaterThan(0));
                    Assert.That(ev.Data.Key, Is.EqualTo(key));
                    Assert.That(ev.Data.Value, Is.EqualTo(value));
                });

            bool addResult = tsDictionary.TryAdd(key, value);

            Assert.That(addResult, Is.True);
        }

        #endregion

        #region Update

        [TestCase]
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
                .Subscribe(ev =>
                {
                    Assert.That(ev.OldData.Timestamp, Is.LessThan(ev.NewData.Timestamp));

                    Assert.That(ev.OldData.Key, Is.EqualTo(key1));
                    Assert.That(ev.NewData.Key, Is.EqualTo(key1));

                    Assert.That(ev.OldData.Value, Is.EqualTo(value11));
                    Assert.That(ev.NewData.Value, Is.EqualTo(value13));
                });

            long timestamp1 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            await Task.Delay(10);

            {
                bool addResult = tsDictionary.TryAdd(key1, value11);

                Assert.That(addResult, Is.True);
            }

            await Task.Delay(10);

            long timestamp2 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            await Task.Delay(10);

            {
                bool addResult = tsDictionary.TryAdd(key2, value12);

                Assert.That(addResult, Is.True);
            }

            await Task.Delay(10);

            long timestamp3 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            await Task.Delay(10);

            {
                bool updateResult = tsDictionary.TryUpdate(key1, value13);

                Assert.That(updateResult, Is.True);
            }

            tsDictionary.CheckConsistency();
        }

        [TestCase]
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
                .Subscribe(ev =>
                {
                    Assert.That(ev.OldData.Timestamp, Is.LessThan(ev.NewData.Timestamp));

                    Assert.That(ev.OldData.Key, Is.EqualTo(key1));
                    Assert.That(ev.NewData.Key, Is.EqualTo(key1));

                    Assert.That(ev.OldData.Value, Is.EqualTo(value11));
                    Assert.That(ev.NewData.Value, Is.EqualTo(value13));
                });

            long timestamp1 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            await Task.Delay(10);

            {
                bool addResult = tsDictionary.TryAdd(key1, value11);

                Assert.That(addResult, Is.True);
            }

            await Task.Delay(10);

            long timestamp2 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            await Task.Delay(10);

            {
                bool addResult = tsDictionary.TryAdd(key2, value12);

                Assert.That(addResult, Is.True);
            }

            await Task.Delay(10);

            long timestamp3 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            await Task.Delay(10);

            {
                bool updateResult = tsDictionary.TryUpdate(key1, value13, comparisonValue: value11);

                Assert.That(updateResult, Is.True);
            }

            tsDictionary.CheckConsistency();
        }

        #endregion

        #region AddOrUpdate

        [TestCase]
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
                .Subscribe(ev =>
                {
                    Assert.That(ev.Data.Timestamp, Is.GreaterThan(0));
                    Assert.That(ev.Data.Value, Is.GreaterThan(0));
                });

            using IDisposable updateObserver = tsDictionary
                .ObserveUpdate()
                .Subscribe(ev =>
                {
                    Assert.That(ev.OldData.Timestamp, Is.LessThan(ev.NewData.Timestamp));

                    Assert.That(ev.OldData.Key, Is.EqualTo(key1));
                    Assert.That(ev.NewData.Key, Is.EqualTo(key1));

                    Assert.That(ev.OldData.Value, Is.EqualTo(value11));
                    Assert.That(ev.NewData.Value, Is.EqualTo(value13));
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

        [TestCase("key1", 11)]
        [TestCase("key2", 12)]
        [TestCase("key3", 13)]
        [TestCase("key4", 14)]
        public void Test1_TryRemove_one(string key, int value)
        {
            using var tsDictionary = new ObservableTimestampedDictionary<int>();

            using IDisposable observer = tsDictionary
                .ObserveRemove()
                .Subscribe(ev =>
                {
                    Assert.That(ev.Data.Timestamp, Is.GreaterThan(0));
                    Assert.That(ev.Data.Key, Is.EqualTo(key));
                    Assert.That(ev.Data.Value, Is.EqualTo(value));
                });

            {
                bool addResult = tsDictionary.TryAdd(key, value);

                Assert.That(addResult, Is.True);
            }

            {
                bool removeResult = tsDictionary.TryRemove(key, out var removeValue);

                Assert.That(removeResult, Is.True);
            }

            tsDictionary.CheckConsistency();
        }

        #endregion

        #region Clear

        [TestCase]
        public void Test1_Clear_zero()
        {
            using var tsDictionary = new ObservableTimestampedDictionary<int>();

            using IDisposable observer = tsDictionary
                .ObserveClear()
                .Subscribe(removedCount =>
                {
                    Assert.That(removedCount, Is.Zero);
                });

            tsDictionary.Clear();
        }

        [TestCase]
        public void Test1_Clear_one()
        {
            using var tsDictionary = new ObservableTimestampedDictionary<int>();

            using IDisposable observer = tsDictionary
                .ObserveClear()
                .Subscribe(removedCount =>
                {
                    Assert.That(removedCount, Is.EqualTo(1));
                });

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
            using var tsDictionary = new ObservableTimestampedDictionary<int>();

            using IDisposable observer = tsDictionary
                .ObserveClear()
                .Subscribe(removedCount =>
                {
                    Assert.That(removedCount, Is.EqualTo(2));
                });

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

        [TestCase]
        public async Task Test1_ClearBefore_DateTimeOffset()
        {
            using var tsDictionary = new ObservableTimestampedDictionary<int>();

            using IDisposable observer = tsDictionary
                .ObserveClear()
                .Subscribe(removedCount =>
                {
                    Assert.That(removedCount, Is.EqualTo(2));
                });

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

        #region ObserveCountChange

        [TestCase]
        public void Test1_ObserveCountChange()
        {
            using var tsDictionary = new ObservableTimestampedDictionary<int>();

            string key1 = "key1";
            string key2 = "key2";

            int value11 = 11;
            int value12 = 12;

            using IDisposable observer = tsDictionary
                .ObserveCountChange()
                .Subscribe(count =>
                {
                    Assert.That(tsDictionary.Count, Is.EqualTo(count));
                });

            {
                bool addResult = tsDictionary.TryAdd(key1, value11);

                Assert.That(addResult, Is.True);
            }

            {
                bool addResult = tsDictionary.TryAdd(key2, value12);

                Assert.That(addResult, Is.True);
            }

            {
                bool removeResult = tsDictionary.TryRemove(key1, out var removeValue);

                Assert.That(removeResult, Is.True);
            }

            tsDictionary.CheckConsistency();
        }

        #endregion
    }
}