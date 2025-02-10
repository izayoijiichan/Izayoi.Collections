// ----------------------------------------------------------------------
// @Namespace : Izayoi.Collections.ObservableTimestampedDictionary.Test
// @Class     : ObservableTimestampedDictionaryTest1o
// ----------------------------------------------------------------------
namespace Izayoi.Collections.ObservableTimestampedDictionary.Test
{
    using Izayoi.Collections;
    using System;
    using System.Threading.Tasks;

    public class ObservableTimestampedDictionaryTest1o
    {
        #region Add

        [Theory]
        [InlineData("key1", 11)]
        [InlineData("key2", 12)]
        [InlineData("key3", 13)]
        public void Test1_TryAdd_one(string key, int value)
        {
            using var tsDictionary = new ObservableTimestampedDictionary<int>();

            using IDisposable observer = tsDictionary
                .ObserveAdd()
                .Subscribe(ev =>
                {
                    Assert.True(ev.Data.Timestamp > 0);
                    Assert.Equal(key, ev.Data.Key);
                    Assert.Equal(value, ev.Data.Value);
                });

            bool addResult = tsDictionary.TryAdd(key, value);

            Assert.True(addResult);
        }

        #endregion

        #region Update

        [Fact]
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
                    Assert.True(ev.OldData.Timestamp < ev.NewData.Timestamp);

                    Assert.Equal(key1, ev.OldData.Key);
                    Assert.Equal(key1, ev.NewData.Key);

                    Assert.Equal(value11, ev.OldData.Value);
                    Assert.Equal(value13, ev.NewData.Value);
                });

            long timestamp1 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            await Task.Delay(10);

            {
                bool addResult = tsDictionary.TryAdd(key1, value11);

                Assert.True(addResult);
            }

            await Task.Delay(10);

            long timestamp2 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            await Task.Delay(10);

            {
                bool addResult = tsDictionary.TryAdd(key2, value12);

                Assert.True(addResult);
            }

            await Task.Delay(10);

            long timestamp3 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            await Task.Delay(10);

            {
                bool updateResult = tsDictionary.TryUpdate(key1, value13);

                Assert.True(updateResult);
            }

            tsDictionary.CheckConsistency();
        }

        [Fact]
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
                    Assert.True(ev.OldData.Timestamp < ev.NewData.Timestamp);

                    Assert.Equal(key1, ev.OldData.Key);
                    Assert.Equal(key1, ev.NewData.Key);

                    Assert.Equal(value11, ev.OldData.Value);
                    Assert.Equal(value13, ev.NewData.Value);
                });

            long timestamp1 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            await Task.Delay(10);

            {
                bool addResult = tsDictionary.TryAdd(key1, value11);

                Assert.True(addResult);
            }

            await Task.Delay(10);

            long timestamp2 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            await Task.Delay(10);

            {
                bool addResult = tsDictionary.TryAdd(key2, value12);

                Assert.True(addResult);
            }

            await Task.Delay(10);

            long timestamp3 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            await Task.Delay(10);

            {
                bool updateResult = tsDictionary.TryUpdate(key1, value13, comparisonValue: value11);

                Assert.True(updateResult);
            }

            tsDictionary.CheckConsistency();
        }

        #endregion

        #region AddOrUpdate

        [Fact]
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
                    Assert.True(ev.Data.Timestamp > 0);
                    Assert.True(ev.Data.Value > 0);
                });

            using IDisposable updateObserver = tsDictionary
                .ObserveUpdate()
                .Subscribe(ev =>
                {
                    Assert.True(ev.OldData.Timestamp < ev.NewData.Timestamp);

                    Assert.Equal(key1, ev.OldData.Key);
                    Assert.Equal(key1, ev.NewData.Key);

                    Assert.Equal(value11, ev.OldData.Value);
                    Assert.Equal(value13, ev.NewData.Value);
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

        [Theory]
        [InlineData("key1", 11)]
        [InlineData("key2", 12)]
        [InlineData("key3", 13)]
        [InlineData("key4", 14)]
        public void Test1_TryRemove_one(string key, int value)
        {
            using var tsDictionary = new ObservableTimestampedDictionary<int>();

            using IDisposable observer = tsDictionary
                .ObserveRemove()
                .Subscribe(ev =>
                {
                    Assert.True(ev.Data.Timestamp > 0);
                    Assert.Equal(key, ev.Data.Key);
                    Assert.Equal(value, ev.Data.Value);
                });

            {
                bool addResult = tsDictionary.TryAdd(key, value);

                Assert.True(addResult);
            }

            {
                bool removeResult = tsDictionary.TryRemove(key, out var removeValue);

                Assert.True(removeResult);
            }

            tsDictionary.CheckConsistency();
        }

        #endregion

        #region Clear

        [Fact]
        public void Test1_Clear_zero()
        {
            using var tsDictionary = new ObservableTimestampedDictionary<int>();

            using IDisposable observer = tsDictionary
                .ObserveClear()
                .Subscribe(removedCount =>
                {
                    Assert.Equal(0, removedCount);
                });

            tsDictionary.Clear();
        }

        [Fact]
        public void Test1_Clear_one()
        {
            using var tsDictionary = new ObservableTimestampedDictionary<int>();

            using IDisposable observer = tsDictionary
                .ObserveClear()
                .Subscribe(removedCount =>
                {
                    Assert.Equal(1, removedCount);
                });

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

            using IDisposable observer = tsDictionary
                .ObserveClear()
                .Subscribe(removedCount =>
                {
                    Assert.Equal(2, removedCount);
                });

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

        [Fact]
        public async Task Test1_ClearBefore_DateTimeOffset()
        {
            using var tsDictionary = new ObservableTimestampedDictionary<int>();

            using IDisposable observer = tsDictionary
                .ObserveClear()
                .Subscribe(removedCount =>
                {
                    Assert.Equal(2, removedCount);
                });

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

        #region ObserveCountChange

        [Fact]
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
                    Assert.Equal(tsDictionary.Count, count);
                });

            {
                bool addResult = tsDictionary.TryAdd(key1, value11);

                Assert.True(addResult);
            }

            {
                bool addResult = tsDictionary.TryAdd(key2, value12);

                Assert.True(addResult);
            }

            {
                bool removeResult = tsDictionary.TryRemove(key1, out var removeValue);

                Assert.True(removeResult);
            }

            tsDictionary.CheckConsistency();
        }

        #endregion
    }
}