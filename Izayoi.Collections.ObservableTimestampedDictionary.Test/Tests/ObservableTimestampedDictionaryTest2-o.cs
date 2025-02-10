// ----------------------------------------------------------------------
// @Namespace : Izayoi.Collections.ObservableTimestampedDictionary.Test
// @Class     : ObservableTimestampedDictionaryTest2o
// ----------------------------------------------------------------------
namespace Izayoi.Collections.ObservableTimestampedDictionary.Test
{
    using Izayoi.Collections;
    using System;
    using System.Threading.Tasks;

    public class ObservableTimestampedDictionaryTest2o
    {
        #region Add

        [Theory]
        [InlineData(11, "value11")]
        [InlineData(12, "value12")]
        [InlineData(13, "value13")]
        public void Test2_TryAdd_one(int key, string value)
        {
            using var tsDictionary = new ObservableTimestampedDictionary<int, string>();

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

            Assert.Equal(1, tsDictionary.Count);
        }

        #endregion

        #region AddOrUpdate

        [Fact]
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
                    Assert.True(ev.Data.Timestamp > 0);
                    Assert.True(ev.Data.Key > 0);
                });

            using IDisposable updateObserver = tsDictionary
                .ObserveUpdate()
                .Subscribe(ev =>
                {
                    Assert.True(ev.OldData.Timestamp < ev.NewData.Timestamp);

                    Assert.Equal(key11, ev.OldData.Key);
                    Assert.Equal(key11, ev.NewData.Key);

                    Assert.Equal(value1, ev.OldData.Value);
                    Assert.Equal(value3, ev.NewData.Value);
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

        [Fact]
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
                    Assert.True(ev.OldData.Timestamp < ev.NewData.Timestamp);

                    Assert.Equal(key11, ev.OldData.Key);
                    Assert.Equal(key11, ev.NewData.Key);

                    Assert.Equal(value1, ev.OldData.Value);
                    Assert.Equal(value3, ev.NewData.Value);
                });

            long timestamp1 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            await Task.Delay(10);

            {
                bool addResult = tsDictionary.TryAdd(key11, value1);

                Assert.True(addResult);
            }

            await Task.Delay(10);

            long timestamp2 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            await Task.Delay(10);

            {
                bool addResult = tsDictionary.TryAdd(key12, value2);

                Assert.True(addResult);
            }

            await Task.Delay(10);

            long timestamp3 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            await Task.Delay(10);

            {
                bool updateResult = tsDictionary.TryUpdate(key11, value3);

                Assert.True(updateResult);
            }

            tsDictionary.CheckConsistency();
        }

        [Fact]
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
                    Assert.True(ev.OldData.Timestamp < ev.NewData.Timestamp);

                    Assert.Equal(key11, ev.OldData.Key);
                    Assert.Equal(key11, ev.NewData.Key);

                    Assert.Equal(value1, ev.OldData.Value);
                    Assert.Equal(value3, ev.NewData.Value);
                });

            long timestamp1 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            await Task.Delay(10);

            {
                bool addResult = tsDictionary.TryAdd(key11, value1);

                Assert.True(addResult);
            }

            await Task.Delay(10);

            long timestamp2 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            await Task.Delay(10);

            {
                bool addResult = tsDictionary.TryAdd(key12, value2);

                Assert.True(addResult);
            }

            await Task.Delay(10);

            long timestamp3 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            await Task.Delay(10);

            {
                bool updateResult = tsDictionary.TryUpdate(key11, value3, comparisonValue: value1);

                Assert.True(updateResult);
            }

            tsDictionary.CheckConsistency();
        }

        #endregion

        #region AddOrUpdate

        [Fact]
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
                    Assert.True(ev.OldData.Timestamp < ev.NewData.Timestamp);

                    Assert.Equal(key11, ev.OldData.Key);
                    Assert.Equal(key11, ev.NewData.Key);

                    Assert.Equal(value1, ev.OldData.Value);
                    Assert.Equal(value3, ev.NewData.Value);
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

        [Theory]
        [InlineData(11, "value11")]
        [InlineData(12, "value12")]
        [InlineData(13, "value13")]
        [InlineData(14, "value14")]
        public void Test2_TryRemove_one(int key, string value)
        {
            using var tsDictionary = new ObservableTimestampedDictionary<int, string>();

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
        public void Test2_Clear_zero()
        {
            using var tsDictionary = new ObservableTimestampedDictionary<int, string>();

            using IDisposable observer = tsDictionary
                .ObserveClear()
                .Subscribe(removedCount =>
                {
                    Assert.Equal(0, removedCount);
                });

            tsDictionary.Clear();
        }

        [Fact]
        public void Test2_Clear_one()
        {
            using var tsDictionary = new ObservableTimestampedDictionary<int, string>();

            using IDisposable observer = tsDictionary
                .ObserveClear()
                .Subscribe(removedCount =>
                {
                    Assert.Equal(1, removedCount);
                });

            Assert.Equal(0, tsDictionary.Count);

            {
                bool addResult = tsDictionary.TryAdd(11, "value11");

                Assert.True(addResult);

                Assert.Equal(1, tsDictionary.Count);
            }

            tsDictionary.Clear();

            Assert.Equal(0, tsDictionary.Count);
        }

        [Fact]
        public void Test2_Clear_two()
        {
            using var tsDictionary = new ObservableTimestampedDictionary<int, string>();

            using IDisposable observer = tsDictionary
                .ObserveClear()
                .Subscribe(removedCount =>
                {
                    Assert.Equal(2, removedCount);
                });

            Assert.Equal(0, tsDictionary.Count);

            {
                bool addResult;

                addResult = tsDictionary.TryAdd(11, "value11");

                Assert.True(addResult);

                Assert.Equal(1, tsDictionary.Count);

                addResult = tsDictionary.TryAdd(12, "value12");

                Assert.True(addResult);

                Assert.Equal(2, tsDictionary.Count);
            }

            tsDictionary.Clear();

            Assert.Equal(0, tsDictionary.Count);
        }

        [Fact]
        public async Task Test2_ClearBefore_DateTimeOffset()
        {
            using var tsDictionary = new ObservableTimestampedDictionary<int, string>();

            using IDisposable observer = tsDictionary
                .ObserveClear()
                .Subscribe(removedCount =>
                {
                    Assert.Equal(2, removedCount);
                });

            Assert.Equal(0, tsDictionary.Count);

            bool addResult;

            {
                addResult = tsDictionary.TryAdd(11, "value11");

                Assert.True(addResult);

                Assert.Equal(1, tsDictionary.Count);

                addResult = tsDictionary.TryAdd(12, "value12");

                Assert.True(addResult);

                Assert.Equal(2, tsDictionary.Count);
            }

            await Task.Delay(10);

            var timestamp = DateTimeOffset.UtcNow;

            await Task.Delay(10);

            {
                addResult = tsDictionary.TryAdd(13, "value13");

                Assert.True(addResult);

                Assert.Equal(3, tsDictionary.Count);
            }

            {
                int removedCount = tsDictionary.ClearBefore(timestamp);

                Assert.Equal(2, removedCount);

                Assert.Equal(1, tsDictionary.Count);

                bool getResult;

                getResult = tsDictionary.TryGetValue(11, out _);

                Assert.False(getResult);

                getResult = tsDictionary.TryGetValue(12, out _);

                Assert.False(getResult);

                getResult = tsDictionary.TryGetValue(13, out var value);

                Assert.True(getResult);

                Assert.Equal("value13", value);
            }
        }

        #endregion

        #region ObserveCountChange

        [Fact]
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
                    Assert.Equal(tsDictionary.Count, count);
                });

            {
                bool addResult = tsDictionary.TryAdd(key11, value1);

                Assert.True(addResult);
            }

            {
                bool addResult = tsDictionary.TryAdd(key12, value2);

                Assert.True(addResult);
            }

            {
                bool removeResult = tsDictionary.TryRemove(key11, out var removeValue);

                Assert.True(removeResult);
            }

            tsDictionary.CheckConsistency();
        }

        #endregion
    }
}