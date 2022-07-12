using System;
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
        new T Get<T>() where T : TEntry;
        new TEntry Get(Type type);
        T Get<T>(TId id) where T : TEntry;
        TEntry Get(TId id);
        new bool TryGet<T>(out T value) where T : TEntry;
        bool TryGet(Type type, out TEntry value);
        bool TryGet<T>(TId id, out T entry) where T : TEntry;
        bool TryGet(TId id, out TEntry entry);
    }
}
