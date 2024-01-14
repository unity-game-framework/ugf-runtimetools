using System;
using System.Collections.Generic;
using UGF.EditorTools.Editor.Ids;
using UGF.EditorTools.Editor.IMGUI;
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
        private readonly List<string> m_names = new List<string>();
        private readonly DropdownSelection<DropdownItem<TableName>> m_namesSelection = new DropdownSelection<DropdownItem<TableName>>();
        private Styles m_styles;

        private class Styles
        {
            public GUIContent NameContent { get; } = new GUIContent();
            public GUIContent NoneContent { get; } = new GUIContent("None");
            public GUIContent MissingContent { get; } = new GUIContent("Missing");
            public GUIContent UntitledContent { get; } = new GUIContent("Untitled");
            public GUIContent OpenWindowContent { get; } = new GUIContent(EditorGUIUtility.FindTexture("HorizontalLayoutGroup Icon"), "Open table window.");
        }

        private struct TableName
        {
            public TableAsset Asset { get; }
            public GlobalId EntryId { get; }

            public TableName(TableAsset asset, GlobalId entryId)
            {
                if (asset == null) throw new ArgumentNullException(nameof(asset));
                if (!entryId.IsValid()) throw new ArgumentException("Value should be valid.", nameof(entryId));

                Asset = asset;
                EntryId = entryId;
            }
        }

        public TableEntryDropdownPropertyDrawer() : base(SerializedPropertyType.Generic)
        {
            m_selection.Dropdown.RootName = "Entries";
            m_selection.Dropdown.MinimumHeight = 300F;
            m_itemsHandler = OnGetItems;
            m_namesSelection.Dropdown.RootName = "Tables and Names";
            m_namesSelection.Dropdown.MinimumWidth = 200F;
            m_namesSelection.Dropdown.MinimumHeight = 300F;
        }

        protected override void OnDrawProperty(Rect position, SerializedProperty serializedProperty, GUIContent label)
        {
            m_styles ??= new Styles();

            GlobalId id = GlobalIdEditorUtility.GetGlobalIdFromProperty(serializedProperty);
            GUIContent content = m_styles.NoneContent;

            if (id != GlobalId.Empty)
            {
                TableEditorUtility.TryGetEntryNameFromCache(id, m_names);

                if (m_names.Count > 0)
                {
                    string name = m_names[0];

                    if (!string.IsNullOrEmpty(name))
                    {
                        content = m_styles.NameContent;

                        content.text = m_names.Count > 1
                            ? $"{name} ({m_names.Count})"
                            : name;
                    }
                    else
                    {
                        content = m_styles.UntitledContent;
                    }
                }
                else
                {
                    content = m_styles.MissingContent;
                }

                m_names.Clear();
            }

            float height = EditorGUIUtility.singleLineHeight;
            float space = EditorGUIUtility.standardVerticalSpacing;

            var rectDropdown = new Rect(position.x, position.y, position.width - height - space, position.height);
            var rectTable = new Rect(rectDropdown.xMax + space, position.y + 1F, height, position.height);

            if (DropdownEditorGUIUtility.Dropdown(rectDropdown, label, content, m_selection, m_itemsHandler, out DropdownItem<GlobalId> selected))
            {
                GlobalIdEditorUtility.SetGlobalIdToProperty(serializedProperty, selected.Value);
            }

            using (new EditorGUI.DisabledScope(!id.IsValid()))
            {
                bool result = GUI.Button(rectTable, m_styles.OpenWindowContent, EditorStyles.iconButton);
                int controlId = EditorIMGUIUtility.GetLastControlId();

                if (result)
                {
                    DropdownEditorGUIUtility.ShowDropdown(controlId, rectTable, m_namesSelection, OnGetNames(id));
                }

                if (DropdownEditorGUIUtility.CheckDropdown(controlId, m_namesSelection, out DropdownItem<TableName> selectedTable))
                {
                    TableTreeEditorUtility.ShowWindow(selectedTable.Value.Asset, id);
                }
            }
        }

        public override float GetPropertyHeight(SerializedProperty serializedProperty, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight;
        }

        private IEnumerable<DropdownItem<GlobalId>> OnGetItems()
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

        private IEnumerable<DropdownItem<TableName>> OnGetNames(GlobalId id)
        {
            if (!id.IsValid()) throw new ArgumentException("Value should be valid.", nameof(id));

            var items = new List<DropdownItem<TableName>>();

            if (TableEntryCache.TryGetNameCollection(id, out TableEntryCache.EntryNameCollection nameCollection))
            {
                foreach ((GUID tableId, HashSet<string> names) in nameCollection)
                {
                    string path = AssetDatabase.GUIDToAssetPath(tableId);
                    var asset = AssetDatabase.LoadAssetAtPath<TableAsset>(path);

                    if (asset != null)
                    {
                        foreach (string name in names)
                        {
                            items.Add(new DropdownItem<TableName>($"{asset.name}: {name}", new TableName(asset, id)));
                        }
                    }
                }
            }

            return items;
        }
    }
}
