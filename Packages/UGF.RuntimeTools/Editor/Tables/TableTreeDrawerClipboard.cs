using System;
using System.Collections.Generic;
using UGF.EditorTools.Runtime.IMGUI.Types;
using UGF.RuntimeTools.Runtime.Tables;
using UnityEditor;
using UnityEngine;

namespace UGF.RuntimeTools.Editor.Tables
{
    internal class TableTreeDrawerClipboard
    {
        public string Key { get; } = nameof(TableTreeDrawerClipboard);
        public ClipboardData Data { get; } = new ClipboardData();

        [Serializable]
        public class ClipboardData
        {
            [SerializeField] private TypeReference<TableAsset> m_type;
            [SerializeReference] private List<object> m_entries = new List<object>();
            [SerializeReference] private List<object> m_children = new List<object>();

            public TypeReference<TableAsset> Type { get { return m_type; } set { m_type = value; } }
            public List<object> Entries { get { return m_entries; } }
            public List<object> Children { get { return m_children; } }
        }

        public void Copy()
        {
            Write();
            Read();
        }

        public void Read()
        {
            if (EditorPrefs.HasKey(Key))
            {
                string data = EditorPrefs.GetString(Key);

                EditorJsonUtility.FromJsonOverwrite(data, Data);
            }
        }

        public void Write()
        {
            string data = EditorJsonUtility.ToJson(Data);

            EditorPrefs.SetString(Key, data);
        }

        public bool TryMatch(Type type)
        {
            return Data.Type.HasValue && Data.Type.TryGet(out Type value) && value == type;
        }

        public bool HasAny()
        {
            return Data.Entries.Count > 0 || Data.Children.Count > 0;
        }

        public void CopyType(Type type)
        {
            TypeReference<TableAsset> reference = Data.Type;

            reference.Set(type);

            Data.Type = reference;
        }

        public bool TryCopyEntries(IReadOnlyList<TableTreeViewItem> items)
        {
            return OnTryCopy(items, Data.Entries);
        }

        public bool TryCopyChildren(IReadOnlyList<TableTreeViewItem> items)
        {
            return OnTryCopy(items, Data.Children);
        }

        public void Clear()
        {
            Data.Type.Clear();
            Data.Entries.Clear();
            Data.Children.Clear();
        }

        private bool OnTryCopy(IReadOnlyList<TableTreeViewItem> items, ICollection<object> values)
        {
            if (items == null) throw new ArgumentNullException(nameof(items));
            if (values == null) throw new ArgumentNullException(nameof(values));

            for (int i = 0; i < items.Count; i++)
            {
                TableTreeViewItem item = items[i];

                object value;

                try
                {
                    value = item.SerializedProperty.boxedValue;
                }
                catch (Exception exception)
                {
                    Debug.LogWarning($"Table entry can not be copied.\n{exception}");
                    return false;
                }

                values.Add(value);
            }

            return true;
        }
    }
}
