using System;
using NUnit.Framework;
using UGF.RuntimeTools.Runtime.Providers;

namespace UGF.RuntimeTools.Runtime.Tests.Providers
{
    public class TestProviderInstance
    {
        [TearDown]
        public void Teardown()
        {
            ProviderInstance<IProvider>.Clear();
        }

        [Test]
        public void GetNull()
        {
            Assert.Throws<InvalidOperationException>(() => Assert.Null(ProviderInstance<IProvider>.Instance));
        }

        [Test]
        public void SetNull()
        {
            Assert.Throws<ArgumentNullException>(() => ProviderInstance<IProvider>.Instance = null);
        }

        [Test]
        public void SetAndGetAndClear()
        {
            var provider = new Provider<string, string>();

            ProviderInstance<IProvider>.Instance = provider;

            Assert.True(ProviderInstance<IProvider>.HasInstance);
            Assert.AreEqual(provider, ProviderInstance<IProvider>.Instance);

            ProviderInstance<IProvider>.Clear();

            Assert.False(ProviderInstance<IProvider>.HasInstance);
        }
    }
}
