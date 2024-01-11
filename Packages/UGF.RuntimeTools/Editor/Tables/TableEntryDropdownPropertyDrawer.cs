using System;
using System.Collections.Generic;
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
                if (TableEditorUtility.TryGetEntryNameFromCache(id, out string name))
                {
                    content = !string.IsNullOrEmpty(name) ? new GUIContent(name) : m_styles.UntitledContent;
                }
                else
                {
                    content = m_styles.MissingContent;
                }
            }

            if (DropdownEditorGUIUtility.Dropdown(position, label, content, m_selection, m_itemsHandler, out DropdownItem<GlobalId> selected))
            {
                GlobalIdEditorUtility.SetGlobalIdToProperty(serializedProperty, selected.Value);
            }
        }

        private IEnumerable<DropdownItem<GlobalId>> GetItems()
        {
            var items = new List<DropdownItem<GlobalId>>();
            IReadOnlyList<TableAsset> tables = TableEditorUtility.FindTableAssetAll(Attribute.TableType);

            TableEntryCache.Update(tables);

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

        public override float GetPropertyHeight(SerializedProperty serializedProperty, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight;
        }
    }
}
