using System.Collections.Generic;

namespace UGF.RuntimeTools.Runtime.Providers
{
    public interface IProvider<TId, TEntry>
    {
        IReadOnlyDictionary<TId, TEntry> Entries { get; }

        event ProviderEntryHandler<TId, TEntry> Added;
        event ProviderEntryHandler<TId, TEntry> Removed;
        event ProviderHandler Cleared;

        void Add(TId id, TEntry entry);
        bool Remove(TId id);
        void Clear();
        T Get<T>(TId id) where T : TEntry;
        TEntry Get(TId id);
        bool TryGet<T>(TId id, out T entry) where T : TEntry;
        bool TryGet(TId id, out TEntry entry);
    }
}
