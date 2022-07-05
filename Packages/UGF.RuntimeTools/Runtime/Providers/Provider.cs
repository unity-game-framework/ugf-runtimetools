using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace UGF.RuntimeTools.Runtime.Providers
{
    public class Provider<TId, TEntry> : IProvider<TId, TEntry>
    {
        public IReadOnlyDictionary<TId, TEntry> Entries { get { return m_entriesReadOnly; } }
        public IEqualityComparer<TId> Comparer { get; }

        public event ProviderEntryHandler<TId, TEntry> Added;
        public event ProviderEntryHandler<TId, TEntry> Removed;
        public event ProviderHandler Cleared;

        IDictionary IProvider.Entries { get { return m_entriesReadOnly; } }

        event ProviderEntryHandler IProvider.Added { add { m_addedHandler += value; } remove { m_addedHandler -= value; } }
        event ProviderEntryHandler IProvider.Removed { add { m_removedHandler += value; } remove { m_removedHandler -= value; } }

        private readonly Dictionary<TId, TEntry> m_entries;
        private readonly ReadOnlyDictionary<TId, TEntry> m_entriesReadOnly;
        private readonly Type m_typeId = typeof(TId);
        private readonly Type m_typeValue = typeof(TEntry);
        private ProviderEntryHandler m_addedHandler;
        private ProviderEntryHandler m_removedHandler;

        public Provider() : this(EqualityComparer<TId>.Default)
        {
        }

        public Provider(IEqualityComparer<TId> comparer)
        {
            Comparer = comparer ?? throw new ArgumentNullException(nameof(comparer));

            m_entries = new Dictionary<TId, TEntry>(comparer);
            m_entriesReadOnly = new ReadOnlyDictionary<TId, TEntry>(m_entries);
        }

        public void Add(TId id, TEntry entry)
        {
            if (m_typeId.IsClass && EqualityComparer<TId>.Default.Equals(id, default)) throw new ArgumentNullException(nameof(id));
            if (m_typeValue.IsClass && EqualityComparer<TEntry>.Default.Equals(entry, default)) throw new ArgumentNullException(nameof(entry));

            OnAdd(id, entry);

            Added?.Invoke(this, id, entry);
            m_addedHandler?.Invoke(this, id, entry);
        }

        public bool Remove(TId id)
        {
            if (m_typeId.IsClass && EqualityComparer<TId>.Default.Equals(id, default)) throw new ArgumentNullException(nameof(id));

            if (m_entries.TryGetValue(id, out TEntry entry))
            {
                OnRemove(id, entry);

                Removed?.Invoke(this, id, entry);
                m_removedHandler?.Invoke(this, id, entry);
                return true;
            }

            return false;
        }

        public void Clear()
        {
            OnClear();

            Cleared?.Invoke(this);
        }

        public T Get<T>()
        {
            return (T)Get(typeof(T));
        }

        public object Get(Type type)
        {
            return TryGet(type, out object value) ? value : throw new ArgumentException($"Value not found by the specified type: '{type}'.");
        }

        public T Get<T>(TId id) where T : TEntry
        {
            return (T)Get(id);
        }

        public TEntry Get(TId id)
        {
            return TryGet(id, out TEntry entry) ? entry : throw new ArgumentException($"Value not found by the specified key: '{id}'.");
        }

        public bool TryGet<T>(out T value)
        {
            if (TryGet(typeof(T), out object result))
            {
                value = (T)result;
                return true;
            }

            value = default;
            return false;
        }

        public bool TryGet(Type type, out object value)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            return OnTryGet(type, out value);
        }

        public bool TryGet<T>(TId id, out T entry) where T : TEntry
        {
            if (TryGet(id, out TEntry value))
            {
                entry = (T)value;
                return true;
            }

            entry = default;
            return false;
        }

        public bool TryGet(TId id, out TEntry entry)
        {
            if (m_typeId.IsClass && EqualityComparer<TId>.Default.Equals(id, default)) throw new ArgumentNullException(nameof(id));

            return OnTryGet(id, out entry);
        }

        public Dictionary<TId, TEntry>.Enumerator GetEnumerator()
        {
            return m_entries.GetEnumerator();
        }

        protected virtual void OnAdd(TId id, TEntry entry)
        {
            m_entries.Add(id, entry);
        }

        protected virtual bool OnRemove(TId id, TEntry entry)
        {
            return m_entries.Remove(id);
        }

        protected virtual void OnClear()
        {
            m_entries.Clear();
        }

        protected virtual bool OnTryGet(Type type, out object value)
        {
            foreach ((_, TEntry entry) in m_entries)
            {
                value = entry;

                if (type.IsInstanceOfType(value))
                {
                    return true;
                }
            }

            value = default;
            return false;
        }

        protected virtual bool OnTryGet(TId id, out TEntry entry)
        {
            return m_entries.TryGetValue(id, out entry);
        }

        void IProvider.Add(object id, object entry)
        {
            Add((TId)id, (TEntry)entry);
        }

        bool IProvider.Remove(object id)
        {
            return Remove((TId)id);
        }

        T IProvider.Get<T>(object id)
        {
            return (T)(object)Get((TId)id);
        }

        object IProvider.Get(object id)
        {
            return Get((TId)id);
        }

        bool IProvider.TryGet<T>(object id, out T entry)
        {
            if (TryGet((TId)id, out TEntry value))
            {
                entry = (T)(object)value;
                return true;
            }

            entry = default;
            return false;
        }

        bool IProvider.TryGet(object id, out object entry)
        {
            if (TryGet((TId)id, out TEntry value))
            {
                entry = value;
                return true;
            }

            entry = default;
            return false;
        }
    }
}
