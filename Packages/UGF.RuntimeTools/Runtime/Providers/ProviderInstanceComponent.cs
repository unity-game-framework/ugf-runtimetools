using System;

namespace UGF.RuntimeTools.Runtime.Providers
{
    public abstract class ProviderInstanceComponent<TProvider> : ProviderInstanceComponentBase where TProvider : class, IProvider
    {
        protected override void OnAddProvider(IProvider<Type, IProvider> providers)
        {
            TProvider provider = OnCreateProvider();

            providers.Add(typeof(TProvider), provider);
        }

        protected override void OnRemoveProvider(IProvider<Type, IProvider> providers)
        {
            providers.Remove(typeof(TProvider));
        }

        protected abstract TProvider OnCreateProvider();
    }
}
