using System.Collections.Generic;

namespace UGF.RuntimeTools.Runtime.Providers
{
    public interface IProvider<TId, TEntry> : IProvider
    {
        new IReadOnlyDictionary<TId, TEntry> Entries { get; }

        new event ProviderEntryHandler<TId, TEntry> Added;
        new event ProviderEntryHandler<TId, TEntry> Removed;

        void Add(TId id, TEntry entry);
        bool Remove(TId id);
        T Get<T>(TId id) where T : TEntry;
        TEntry Get(TId id);
        bool TryGet<T>(TId id, out T entry) where T : TEntry;
        bool TryGet(TId id, out TEntry entry);
    }
}
