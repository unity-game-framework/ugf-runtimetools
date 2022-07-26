using System;
using System.Collections.Generic;
using UGF.EditorTools.Runtime.Ids;
using UnityEngine;

namespace UGF.RuntimeTools.Runtime.Tables
{
    [Serializable]
    public class Table<TEntry> : ITable where TEntry : class, ITableEntry
    {
        [SerializeField] private List<TEntry> m_entries = new List<TEntry>();

        public List<TEntry> Entries { get { return m_entries; } }

        IEnumerable<ITableEntry> ITable.Entries { get { return Entries; } }

        public T Get<T>(GlobalId id) where T : class, ITableEntry
        {
            return (T)Get(id);
        }

        public ITableEntry Get(GlobalId id)
        {
            return TryGet(id, out ITableEntry entry) ? entry : throw new ArgumentException($"Entry not found by the specified id: '{id}'.");
        }

        public bool TryGet<T>(GlobalId id, out T entry) where T : class, ITableEntry
        {
            if (TryGet(id, out ITableEntry value))
            {
                entry = (T)value;
                return true;
            }

            entry = default;
            return false;
        }

        public bool TryGet(GlobalId id, out ITableEntry entry)
        {
            if (!id.IsValid()) throw new ArgumentException("Value should be valid.", nameof(id));

            for (int i = 0; i < m_entries.Count; i++)
            {
                entry = m_entries[i];

                if (entry.Id == id)
                {
                    return true;
                }
            }

            entry = default;
            return false;
        }
    }
}
