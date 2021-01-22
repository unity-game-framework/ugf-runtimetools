using System;
using UnityEngine;

namespace UGF.RuntimeTools.Runtime.Providers
{
    public abstract class ProviderInstanceComponentBase : MonoBehaviour
    {
        private void Start()
        {
            OnAddProvider();
        }

        private void OnDestroy()
        {
            OnRemoveProvider();
        }

        protected virtual void OnAddProvider()
        {
            OnAddProvider(ProviderInstance.Providers);
        }

        protected virtual void OnRemoveProvider()
        {
            OnRemoveProvider(ProviderInstance.Providers);
        }

        protected abstract void OnAddProvider(IProvider<Type, IProvider> providers);
        protected abstract void OnRemoveProvider(IProvider<Type, IProvider> providers);
    }
}
