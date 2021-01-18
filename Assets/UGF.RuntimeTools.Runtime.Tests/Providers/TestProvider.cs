using System;
using NUnit.Framework;
using UGF.RuntimeTools.Runtime.Providers;

namespace UGF.RuntimeTools.Runtime.Tests.Providers
{
    public class TestProvider
    {
        [Test]
        public void AddAndRemove()
        {
            int counterAdd = 0;
            int counterRemove = 0;
            int counterCleared = 0;
            var provider = new Provider<string, string>();

            provider.Added += (provider1, id, entry) => counterAdd++;
            provider.Removed += (provider1, id, entry) => counterRemove++;
            provider.Cleared += provider1 => counterCleared++;

            Assert.IsEmpty(provider.Entries);
            Assert.AreEqual(0, counterAdd);
            Assert.AreEqual(0, counterRemove);
            Assert.AreEqual(0, counterCleared);

            provider.Add("0", "0");
            provider.Add("1", "1");

            Assert.IsNotEmpty(provider.Entries);
            Assert.AreEqual(2, counterAdd);
            Assert.AreEqual(0, counterRemove);
            Assert.AreEqual(0, counterCleared);

            string entry0 = provider.Get("0");
            string entry1 = provider.Get("1");

            Assert.NotNull(entry0);
            Assert.NotNull(entry1);
            Assert.AreEqual("0", entry0);
            Assert.AreEqual("1", entry1);
            Assert.AreEqual(2, counterAdd);
            Assert.AreEqual(0, counterRemove);
            Assert.AreEqual(0, counterCleared);

            bool result0 = provider.Remove("0");

            Assert.True(result0);
            Assert.AreEqual(1, provider.Entries.Count);
            Assert.AreEqual(2, counterAdd);
            Assert.AreEqual(1, counterRemove);
            Assert.AreEqual(0, counterCleared);

            provider.Clear();

            Assert.IsEmpty(provider.Entries);
            Assert.AreEqual(2, counterAdd);
            Assert.AreEqual(1, counterRemove);
            Assert.AreEqual(1, counterCleared);
        }

        [Test]
        public void AddAndRemoveNulls()
        {
            var provider0 = new Provider<string, string>();
            var provider1 = new Provider<int, int>();

            Assert.Throws<ArgumentNullException>(() => provider0.Add(default, default));
            Assert.DoesNotThrow(() => provider1.Add(default, default));
        }
    }
}
