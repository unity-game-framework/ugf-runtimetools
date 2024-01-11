using System;
using System.Collections.Generic;
using System.Text;
using UGF.EditorTools.Editor.Ids;
using UGF.EditorTools.Editor.IMGUI.Dropdown;
using UGF.EditorTools.Editor.IMGUI.PropertyDrawers;
using UGF.EditorTools.Runtime.Ids;
using UGF.RuntimeTools.Runtime.Tables;
using UnityEditor;
using UnityEngine;

namespace UGF.RuntimeTools.Editor.Tables
{
    [CustomPropertyDrawer(typeof(TableEntryDropdownAttribute), true)]
    internal class TableEntryDropdownPropertyDrawer : PropertyDrawerTyped<TableEntryDropdownAttribute>
    {
        private readonly DropdownSelection<DropdownItem<GlobalId>> m_selection = new DropdownSelection<DropdownItem<GlobalId>>();
        private readonly Func<IEnumerable<DropdownItem<GlobalId>>> m_itemsHandler;
        private readonly StringBuilder m_builder = new StringBuilder();
        private Styles m_styles;

        private class Styles
        {
            public GUIContent NoneContent { get; } = new GUIContent("None");
            public GUIContent MissingContent { get; } = new GUIContent("Missing");
            public GUIContent UntitledContent { get; } = new GUIContent("Untitled");
        }

        public TableEntryDropdownPropertyDrawer() : base(SerializedPropertyType.Generic)
        {
            m_selection.Dropdown.RootName = "Entries";
            m_selection.Dropdown.MinimumHeight = 300F;
            m_itemsHandler = GetItems;
        }

        protected override void OnDrawProperty(Rect position, SerializedProperty serializedProperty, GUIContent label)
        {
            m_styles ??= new Styles();

            GlobalId id = GlobalIdEditorUtility.GetGlobalIdFromProperty(serializedProperty);
            GUIContent content = m_styles.NoneContent;

            if (id != GlobalId.Empty)
            {
                if (TableEntryCache.TryGetNameCollection(id, out TableEntryCache.EntryNameCollection nameCollection))
                {
                    string contentText = string.Empty;

                    m_builder.Clear();

                    foreach ((GUID tableGuid, HashSet<string> names) in nameCollection)
                    {
                        string tableName = TableEntryCache.GetTableName(tableGuid);

                        m_builder.Append(tableName);
                        m_builder.Append(": ");

                        int index = 0;

                        foreach (string name in names)
                        {
                            if (string.IsNullOrEmpty(contentText))
                            {
                                contentText = nameCollection.Count > 1 ? $"{name} ({nameCollection.Count})" : name;
                            }

                            m_builder.Append(name);

                            if (index++ < names.Count - 1)
                            {
                                m_builder.Append(", ");
                            }
                        }

                        m_builder.AppendLine();
                    }

                    content = !string.IsNullOrEmpty(contentText) ? new GUIContent(contentText) : m_styles.UntitledContent;
                    content.tooltip = m_builder.ToString();
                }
                else
                {
                    content = m_styles.MissingContent;
                    content.tooltip = $"Entry name not found by specified id: '{id}'.";
                }
            }

            if (DropdownEditorGUIUtility.Dropdown(position, label, content, m_selection, m_itemsHandler, out DropdownItem<GlobalId> selected))
            {
                GlobalIdEditorUtility.SetGlobalIdToProperty(serializedProperty, selected.Value);
            }
        }

        public override float GetPropertyHeight(SerializedProperty serializedProperty, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight;
        }

        private IEnumerable<DropdownItem<GlobalId>> GetItems()
        {
            var items = new List<DropdownItem<GlobalId>>();
            IReadOnlyList<TableAsset> tables = TableEditorUtility.FindTableAssetAll(Attribute.TableType);

            items.Add(new DropdownItem<GlobalId>("None", GlobalId.Empty)
            {
                Priority = int.MaxValue
            });

            for (int i = 0; i < tables.Count; i++)
            {
                TableAsset asset = tables[i];
                ITable table = asset.Get();

                for (int index = 0; index < table.Entries.Count; index++)
                {
                    ITableEntry entry = table.Entries[index];

                    items.Add(new DropdownItem<GlobalId>(entry.Name, entry.Id)
                    {
                        Path = asset.name
                    });
                }
            }

            return items;
        }
    }
}
