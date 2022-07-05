using System;
using System.Collections;

namespace UGF.RuntimeTools.Runtime.Providers
{
    public interface IProvider
    {
        IDictionary Entries { get; }

        event ProviderEntryHandler Added;
        event ProviderEntryHandler Removed;
        event ProviderHandler Cleared;

        void Add(object id, object entry);
        bool Remove(object id);
        void Clear();
        T Get<T>();
        object Get(Type type);
        T Get<T>(object id);
        object Get(object id);
        bool TryGet<T>(out T value);
        bool TryGet(Type type, out object value);
        bool TryGet<T>(object id, out T entry);
        bool TryGet(object id, out object entry);
    }
}
