using System;
using System.Collections.Generic;
using UGF.EditorTools.Runtime.Ids;
using UGF.RuntimeTools.Runtime.Tables;
using UnityEditor;

namespace UGF.RuntimeTools.Editor.Tables
{
    [InitializeOnLoad]
    internal static class TableEntryCache
    {
        private static readonly Dictionary<GlobalId, string> m_names = new Dictionary<GlobalId, string>();

        static TableEntryCache()
        {
            Update();
        }

        public static void Update()
        {
            Clear();
            Update(TableEditorUtility.FindTableAssetAll(typeof(TableAsset)));
        }

        public static void Update(IReadOnlyList<TableAsset> tables)
        {
            if (tables == null) throw new ArgumentNullException(nameof(tables));

            for (int i = 0; i < tables.Count; i++)
            {
                TableAsset tableAsset = tables[i];
                ITable table = tableAsset.Get();

                foreach (ITableEntry entry in table.Entries)
                {
                    if (entry.Id.IsValid())
                    {
                        m_names[entry.Id] = entry.Name;
                    }
                }
            }
        }

        public static void Clear()
        {
            m_names.Clear();
        }

        public static bool TryGetName(GlobalId id, out string name)
        {
            if (!id.IsValid()) throw new ArgumentException("Value should be valid.", nameof(id));

            return m_names.TryGetValue(id, out name);
        }
    }
}
