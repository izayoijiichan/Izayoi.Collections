// ----------------------------------------------------------------------
// @Namespace : Izayoi.Collections.ObservableTimestampedDictionary.Test
// @Class     : ObservableTimestampedDictionaryTest2o
// ----------------------------------------------------------------------
namespace Izayoi.Collections.ObservableTimestampedDictionary.Test
{
    using Izayoi.Collections;
    using NUnit.Framework;
    using System;
    using System.Threading.Tasks;

    [TestFixture]
    public class ObservableTimestampedDictionaryTest2o
    {
        #region Add

        [TestCase(11, "value11")]
        [TestCase(12, "value12")]
        [TestCase(13, "value13")]
        public void Test2_TryAdd_one(int key, string value)
        {
            using var tsDictionary = new ObservableTimestampedDictionary<int, string>();

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

            Assert.That(tsDictionary, Has.Count.EqualTo(1));
        }

        #endregion

        #region AddOrUpdate

        [TestCase]
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
                .Subscribe(ev =>
                {
                    Assert.That(ev.Data.Timestamp, Is.GreaterThan(0));
                    Assert.That(ev.Data.Key, Is.GreaterThan(0));
                });

            using IDisposable updateObserver = tsDictionary
                .ObserveUpdate()
                .Subscribe(ev =>
                {
                    Assert.That(ev.OldData.Timestamp, Is.LessThan(ev.NewData.Timestamp));

                    Assert.That(ev.OldData.Key, Is.EqualTo(key11));
                    Assert.That(ev.NewData.Key, Is.EqualTo(key11));

                    Assert.That(ev.OldData.Value, Is.EqualTo(value1));
                    Assert.That(ev.NewData.Value, Is.EqualTo(value3));
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

        [TestCase]
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
                .Subscribe(ev =>
                {
                    Assert.That(ev.OldData.Timestamp, Is.LessThan(ev.NewData.Timestamp));

                    Assert.That(ev.OldData.Key, Is.EqualTo(key11));
                    Assert.That(ev.NewData.Key, Is.EqualTo(key11));

                    Assert.That(ev.OldData.Value, Is.EqualTo(value1));
                    Assert.That(ev.NewData.Value, Is.EqualTo(value3));
                });

            long timestamp1 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            await Task.Delay(10);

            {
                bool addResult = tsDictionary.TryAdd(key11, value1);

                Assert.That(addResult, Is.True);
            }

            await Task.Delay(10);

            long timestamp2 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            await Task.Delay(10);

            {
                bool addResult = tsDictionary.TryAdd(key12, value2);

                Assert.That(addResult, Is.True);
            }

            await Task.Delay(10);

            long timestamp3 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            await Task.Delay(10);

            {
                bool updateResult = tsDictionary.TryUpdate(key11, value3);

                Assert.That(updateResult, Is.True);
            }

            tsDictionary.CheckConsistency();
        }

        [TestCase]
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
                .Subscribe(ev =>
                {
                    Assert.That(ev.OldData.Timestamp, Is.LessThan(ev.NewData.Timestamp));

                    Assert.That(ev.OldData.Key, Is.EqualTo(key11));
                    Assert.That(ev.NewData.Key, Is.EqualTo(key11));

                    Assert.That(ev.OldData.Value, Is.EqualTo(value1));
                    Assert.That(ev.NewData.Value, Is.EqualTo(value3));
                });

            long timestamp1 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            await Task.Delay(10);

            {
                bool addResult = tsDictionary.TryAdd(key11, value1);

                Assert.That(addResult, Is.True);
            }

            await Task.Delay(10);

            long timestamp2 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            await Task.Delay(10);

            {
                bool addResult = tsDictionary.TryAdd(key12, value2);

                Assert.That(addResult, Is.True);
            }

            await Task.Delay(10);

            long timestamp3 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            await Task.Delay(10);

            {
                bool updateResult = tsDictionary.TryUpdate(key11, value3, comparisonValue: value1);

                Assert.That(updateResult, Is.True);
            }

            tsDictionary.CheckConsistency();
        }

        #endregion

        #region AddOrUpdate

        [TestCase]
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
                .Subscribe(ev =>
                {
                    Assert.That(ev.OldData.Timestamp, Is.LessThan(ev.NewData.Timestamp));

                    Assert.That(ev.OldData.Key, Is.EqualTo(key11));
                    Assert.That(ev.NewData.Key, Is.EqualTo(key11));

                    Assert.That(ev.OldData.Value, Is.EqualTo(value1));
                    Assert.That(ev.NewData.Value, Is.EqualTo(value3));
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

        [TestCase(11, "value11")]
        [TestCase(12, "value12")]
        [TestCase(13, "value13")]
        [TestCase(14, "value14")]
        public void Test2_TryRemove_one(int key, string value)
        {
            using var tsDictionary = new ObservableTimestampedDictionary<int, string>();

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
        public void Test2_Clear_zero()
        {
            using var tsDictionary = new ObservableTimestampedDictionary<int, string>();

            using IDisposable observer = tsDictionary
                .ObserveClear()
                .Subscribe(removedCount =>
                {
                    Assert.That(removedCount, Is.Zero);
                });

            tsDictionary.Clear();
        }

        [TestCase]
        public void Test2_Clear_one()
        {
            using var tsDictionary = new ObservableTimestampedDictionary<int, string>();

            using IDisposable observer = tsDictionary
                .ObserveClear()
                .Subscribe(removedCount =>
                {
                    Assert.That(removedCount, Is.EqualTo(1));
                });

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
            using var tsDictionary = new ObservableTimestampedDictionary<int, string>();

            using IDisposable observer = tsDictionary
                .ObserveClear()
                .Subscribe(removedCount =>
                {
                    Assert.That(removedCount, Is.EqualTo(2));
                });

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

        [TestCase]
        public async Task Test2_ClearBefore_DateTimeOffset()
        {
            using var tsDictionary = new ObservableTimestampedDictionary<int, string>();

            using IDisposable observer = tsDictionary
                .ObserveClear()
                .Subscribe(removedCount =>
                {
                    Assert.That(removedCount, Is.EqualTo(2));
                });

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

        #region ObserveCountChange

        [TestCase]
        public void Test2_ObserveCountChange()
        {
            using var tsDictionary = new ObservableTimestampedDictionary<int, string>();

            int key11 = 11;
            int key12 = 12;

            string value1 = "value1";
            string value2 = "value2";

            using IDisposable observer = tsDictionary
                .ObserveCountChange()
                .Subscribe(count =>
                {
                    Assert.That(tsDictionary.Count, Is.EqualTo(count));
                });

            {
                bool addResult = tsDictionary.TryAdd(key11, value1);

                Assert.That(addResult, Is.True);
            }

            {
                bool addResult = tsDictionary.TryAdd(key12, value2);

                Assert.That(addResult, Is.True);
            }

            {
                bool removeResult = tsDictionary.TryRemove(key11, out var removeValue);

                Assert.That(removeResult, Is.True);
            }

            tsDictionary.CheckConsistency();
        }

        #endregion
    }
}