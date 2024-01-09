using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UGF.EditorTools.Editor.Ids;
using UGF.EditorTools.Editor.IMGUI;
using UGF.EditorTools.Editor.IMGUI.Dropdown;
using UGF.EditorTools.Editor.IMGUI.Scopes;
using UGF.EditorTools.Runtime.Ids;
using UGF.RuntimeTools.Runtime.Tables;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace UGF.RuntimeTools.Editor.Tables
{
    public class TableTreeDrawer : DrawerBase
    {
        public SerializedObject SerializedObject { get; }
        public TableTreeOptions Options { get; }
        public bool UnlockIds { get; set; }
        public bool DisplayToolbar { get; set; } = true;
        public bool DisplayFooter { get; set; } = true;

        private readonly TableTreeView m_treeView;
        private readonly TableTreeViewPreferences m_treeViewPreferences;
        private readonly Action<SerializedProperty> m_entryInitializeHandler;
        private readonly Func<IEnumerable<DropdownItem<int>>> m_searchSelectionItemsHandler;
        private readonly DropdownSelection<DropdownItem<int>> m_searchSelection = new DropdownSelection<DropdownItem<int>>();
        private readonly List<int> m_selectedIndexes = new List<int>();
        private readonly List<TableTreeViewItem> m_selectedItems = new List<TableTreeViewItem>();
        private SearchField m_search;
        private Styles m_styles;

        private static readonly TableTreeDrawerClipboard m_clipboard = new TableTreeDrawerClipboard();

        private class Styles
        {
            public GUIStyle Toolbar { get; } = EditorStyles.toolbar;
            public GUIStyle Footer { get; } = new GUIStyle("IN Footer");
            public GUIStyle FooterSection { get; } = EditorStyles.toolbarButton;
            public GUIStyle SearchField { get; } = new GUIStyle("ToolbarSearchTextFieldPopup");
            public GUIStyle SearchButtonCancel { get; } = new GUIStyle("ToolbarSearchCancelButton");
            public GUIStyle SearchButtonCancelEmpty { get; } = new GUIStyle("ToolbarSearchCancelButtonEmpty");
            public GUIContent SearchDropdownContent { get; } = new GUIContent(string.Empty, "Select search column.");
            public GUIContent AddButtonContent { get; } = new GUIContent(EditorGUIUtility.FindTexture("Toolbar Plus"), "Add new or duplicate selected entries.");
            public GUIContent RemoveButtonContent { get; } = new GUIContent(EditorGUIUtility.FindTexture("Toolbar Minus"), "Delete selected entries.");
            public GUIContent MenuButtonContent { get; } = new GUIContent(EditorGUIUtility.FindTexture("_Menu"));
            public GUIContent AddButtonChildrenContent { get; } = new GUIContent(EditorGUIUtility.FindTexture("Toolbar Plus"), "Add new or duplicate selected children.");

            public GUILayoutOption[] TableLayoutOptions { get; } =
            {
                GUILayout.ExpandWidth(true),
                GUILayout.ExpandHeight(true)
            };
        }

        public TableTreeDrawer(SerializedObject serializedObject, TableTreeOptions options)
        {
            SerializedObject = serializedObject ?? throw new ArgumentNullException(nameof(serializedObject));
            Options = options ?? throw new ArgumentNullException(nameof(options));

            m_treeView = new TableTreeView(SerializedObject.FindProperty("m_table"), options);
            m_treeViewPreferences = new TableTreeViewPreferences(m_treeView, SerializedObject.targetObject);
            m_entryInitializeHandler = OnEntryInitialize;
            m_searchSelectionItemsHandler = OnGetSearchSelectionItems;
            m_searchSelection.Dropdown.RootName = "Search Column";
            m_searchSelection.Dropdown.MinimumWidth = 300F;
            m_searchSelection.Dropdown.MinimumHeight = 300F;
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            Undo.undoRedoPerformed += OnUndoOrRedoPerformed;

            m_clipboard.Read();

            m_treeView.DrawRowCell += OnDrawRowCell;
            m_treeView.KeyEventProcessing += OnKeyEventProcessing;
            m_treeViewPreferences.Read();

            m_treeView.Reload();
            m_treeView.multiColumnHeader.ResizeToFit();
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            m_treeViewPreferences.Write();

            Undo.undoRedoPerformed -= OnUndoOrRedoPerformed;

            m_treeView.DrawRowCell -= OnDrawRowCell;
            m_treeView.KeyEventProcessing -= OnKeyEventProcessing;
        }

        public void DrawGUILayout()
        {
            m_styles ??= new Styles();
            m_search ??= new SearchField();

            if (DisplayToolbar)
            {
                DrawToolbar();
            }

            DrawTable();

            if (DisplayFooter)
            {
                DrawFooter();
            }

            if (GUI.changed)
            {
                m_treeViewPreferences.Write();
            }
        }

        protected void DrawToolbar()
        {
            using (new EditorGUILayout.HorizontalScope(m_styles.Toolbar))
            {
                if (TableEditorGUIInternalUtility.DrawToolbarButton(m_styles.AddButtonContent))
                {
                    OnEntryAdd();
                }

                using (new EditorGUI.DisabledScope(!m_treeView.HasSelection()))
                {
                    if (TableEditorGUIInternalUtility.DrawToolbarButton(m_styles.RemoveButtonContent))
                    {
                        OnEntryRemove();
                    }
                }

                GUILayout.FlexibleSpace();

                OnDrawSearch();

                if (TableEditorGUIInternalUtility.DrawToolbarButton(m_styles.MenuButtonContent, out Rect rectMenu, 25F))
                {
                    OnMenuOpen(rectMenu);
                }
            }
        }

        protected void DrawTable()
        {
            Rect position;

            using (var scope = new EditorGUILayout.VerticalScope(GUIStyle.none, m_styles.TableLayoutOptions))
            {
                position = scope.rect;
            }

            using (new SerializedObjectUpdateScope(SerializedObject))
            {
                m_treeView.OnGUI(position);
            }
        }

        protected virtual void OnDrawRowCell(Rect position, TableTreeViewItem item, SerializedProperty serializedProperty, TableTreeColumnOptions column)
        {
            if (serializedProperty.propertyType == SerializedPropertyType.Generic && serializedProperty.isArray)
            {
                if (serializedProperty.name == Options.PropertyChildrenName)
                {
                    OnDrawRowCellChildren(position, item, serializedProperty, column);
                }
                else
                {
                    OnDrawRowCellArray(position, item, serializedProperty, column);
                }
            }
            else
            {
                using (new EditorGUI.DisabledScope(!UnlockIds && serializedProperty.name == Options.PropertyIdName))
                {
                    EditorGUI.PropertyField(position, serializedProperty, GUIContent.none, false);
                }
            }
        }

        protected virtual void OnDrawRowCellArray(Rect position, TableTreeViewItem item, SerializedProperty serializedProperty, TableTreeColumnOptions column)
        {
            using (new EditorGUI.DisabledScope(true))
            {
                EditorGUI.IntField(position, GUIContent.none, serializedProperty.arraySize);
            }
        }

        protected virtual void OnDrawRowCellChildren(Rect position, TableTreeViewItem item, SerializedProperty serializedProperty, TableTreeColumnOptions column)
        {
            float height = EditorGUIUtility.singleLineHeight;
            float space = EditorGUIUtility.standardVerticalSpacing;

            var rectField = new Rect(position.x, position.y, position.width - height - space, position.height);
            var rectButton = new Rect(rectField.xMax + space, position.y, height, height);

            using (new EditorGUI.DisabledScope(true))
            {
                EditorGUI.IntField(rectField, GUIContent.none, serializedProperty.arraySize);
            }

            if (GUI.Button(rectButton, m_styles.AddButtonChildrenContent, EditorStyles.iconButton))
            {
                OnEntryAddChildren(item);
            }
        }

        protected void DrawFooter()
        {
            using (new EditorGUILayout.HorizontalScope(m_styles.Footer))
            {
                string path = AssetDatabase.GetAssetPath(SerializedObject.targetObject);
                int columnsVisible = m_treeView.multiColumnHeader.state.visibleColumns.Length;
                int columnsTotal = m_treeView.multiColumnHeader.state.columns.Length;
                int countVisible = m_treeView.Count;
                int countTotal = ((TableAsset)SerializedObject.targetObject).Get().Entries.Count();

                GUILayout.Label($"Path: {path}", m_styles.FooterSection);
                GUILayout.FlexibleSpace();

                if (OnClipboardMatch())
                {
                    var builder = new StringBuilder("Clipboard:");

                    if (m_clipboard.Data.Entries.Count > 0)
                    {
                        builder.Append($" Entries {m_clipboard.Data.Entries.Count}");
                    }

                    if (m_clipboard.Data.Children.Count > 0)
                    {
                        builder.Append($" Children {m_clipboard.Data.Children.Count}");
                    }

                    GUILayout.Label(builder.ToString(), m_styles.FooterSection);
                }

                GUILayout.Label($"Search Column: {m_treeView.SearchColumn.DisplayName}", m_styles.FooterSection);

                GUILayout.Label(columnsVisible == columnsTotal
                    ? $"Columns: {columnsTotal}"
                    : $"Columns: {columnsVisible}/{columnsTotal}", m_styles.FooterSection);

                GUILayout.Label(countVisible == countTotal
                    ? $"Entries: {countTotal}"
                    : $"Entries: {countVisible}/{countTotal}", m_styles.FooterSection);
            }
        }

        private void OnDrawSearch()
        {
            float height = EditorGUIUtility.singleLineHeight;
            float spacing = EditorGUIUtility.standardVerticalSpacing;

            Rect position = GUILayoutUtility.GetRect(300F, height);

            position.xMin += spacing;
            position.xMax -= spacing;
            position.yMin += spacing;

            var rectButton = new Rect(position.x, position.y, position.height, position.height);
            var rectLabel = new Rect(rectButton.xMax + spacing, position.y, position.width - rectButton.width - spacing, position.height);

            if (DropdownEditorGUIUtility.Dropdown(rectButton, GUIContent.none, m_styles.SearchDropdownContent, m_searchSelection, m_searchSelectionItemsHandler, out DropdownItem<int> selected, FocusType.Keyboard, GUIStyle.none))
            {
                m_treeView.SearchColumnIndex = selected.Value;
            }

            m_treeView.searchString = m_search.OnGUI(position, m_treeView.searchString, m_styles.SearchField, m_styles.SearchButtonCancel, m_styles.SearchButtonCancelEmpty);

            if (!m_search.HasFocus())
            {
                using (new EditorGUI.DisabledScope(true))
                {
                    GUI.Label(rectLabel, m_treeView.SearchColumn.DisplayName, EditorStyles.miniLabel);
                }
            }
        }

        private IEnumerable<DropdownItem<int>> OnGetSearchSelectionItems()
        {
            var items = new List<DropdownItem<int>>();

            for (int i = 0; i < Options.Columns.Count; i++)
            {
                TableTreeColumnOptions column = Options.Columns[i];

                items.Add(new DropdownItem<int>(column.DisplayName, i)
                {
                    Priority = Options.Columns.Count - i
                });
            }

            return items;
        }

        private void OnEntryAdd()
        {
            m_treeView.GetChildrenParentSelection(m_selectedItems);

            foreach (TableTreeViewItem item in m_selectedItems)
            {
                m_treeView.GetChildrenSelectionIndexes(item, m_selectedIndexes);

                TableTreeDrawerEditorUtility.PropertyInsert(item.PropertyChildren, m_selectedIndexes);

                m_selectedIndexes.Clear();
            }

            m_treeView.GetSelectionIndexes(m_selectedIndexes);

            if (m_selectedIndexes.Count > 0)
            {
                TableTreeDrawerEditorUtility.PropertyInsert(m_treeView.PropertyEntries, m_selectedIndexes, m_entryInitializeHandler);
            }
            else
            {
                TableTreeDrawerEditorUtility.PropertyInsert(m_treeView.PropertyEntries, m_treeView.PropertyEntries.arraySize, m_entryInitializeHandler);
            }

            m_selectedIndexes.Clear();
            m_selectedItems.Clear();
            m_treeView.Apply();
        }

        private void OnEntryAddChildren(TableTreeViewItem item)
        {
            m_treeView.GetChildrenSelectionIndexes(item, m_selectedIndexes);

            if (m_selectedIndexes.Count > 0)
            {
                TableTreeDrawerEditorUtility.PropertyInsert(item.PropertyChildren, m_selectedIndexes);

                m_selectedIndexes.Clear();
            }
            else
            {
                TableTreeDrawerEditorUtility.PropertyInsert(item.PropertyChildren, item.PropertyChildren.arraySize);
            }

            m_treeView.Apply();
        }

        private void OnEntryRemove()
        {
            m_treeView.GetChildrenParentSelection(m_selectedItems);

            foreach (TableTreeViewItem item in m_selectedItems)
            {
                m_treeView.GetChildrenSelectionIndexes(item, m_selectedIndexes);

                TableTreeDrawerEditorUtility.PropertyRemove(item.PropertyChildren, m_selectedIndexes);

                m_selectedIndexes.Clear();
            }

            m_treeView.GetSelectionIndexes(m_selectedIndexes);

            if (m_selectedIndexes.Count > 0)
            {
                TableTreeDrawerEditorUtility.PropertyRemove(m_treeView.PropertyEntries, m_selectedIndexes);
            }

            m_selectedIndexes.Clear();
            m_selectedItems.Clear();
            m_treeView.Apply();
        }

        private void OnEntryCopy()
        {
            m_clipboard.Clear();
            m_treeView.GetSelection(m_selectedItems);

            m_clipboard.TryCopyEntries(m_selectedItems);

            m_selectedItems.Clear();
            m_treeView.GetChildrenSelection(m_selectedItems);

            m_clipboard.TryCopyChildren(m_selectedItems);

            m_selectedItems.Clear();

            if (m_clipboard.HasAny())
            {
                m_clipboard.CopyType(SerializedObject.targetObject.GetType());
                m_clipboard.Write();
            }
        }

        private void OnEntryPaste()
        {
            if (OnClipboardMatch())
            {
                m_treeView.GetSelection(m_selectedItems);

                int index = m_selectedItems.Count > 0 ? m_selectedItems[^1].Index : m_treeView.PropertyEntries.arraySize;

                foreach (object value in m_clipboard.Data.Entries)
                {
                    TableTreeDrawerEditorUtility.PropertyInsert(m_treeView.PropertyEntries, index, m_entryInitializeHandler, value);
                }

                m_selectedItems.Clear();
                m_treeView.GetChildrenSelection(m_selectedItems);

                if (m_selectedItems.Count > 0)
                {
                    TableTreeViewItem item = m_selectedItems[^1];
                    var parent = (TableTreeViewItem)item.parent;

                    foreach (object value in m_clipboard.Data.Children)
                    {
                        TableTreeDrawerEditorUtility.PropertyInsert(parent.PropertyChildren, item.Index, value);
                    }
                }

                m_selectedItems.Clear();
                m_treeView.Apply();
            }
        }

        private void OnMenuOpen(Rect position)
        {
            var menu = new GenericMenu();

            menu.AddItem(new GUIContent("Ping Asset"), false, () => EditorGUIUtility.PingObject(SerializedObject.targetObject));
            menu.AddItem(new GUIContent("Unlock Ids"), UnlockIds, () => UnlockIds = !UnlockIds);
            menu.AddItem(new GUIContent("Reset Sorting"), false, () => m_treeView.ClearSorting());
            menu.AddItem(new GUIContent("Reset Preferences"), false, m_treeViewPreferences.Reset);
            menu.AddSeparator(string.Empty);

            if (m_treeView != null && m_treeView.PropertyEntries.arraySize > 0)
            {
                menu.AddItem(new GUIContent("Clear"), false, OnMenuClear);
            }
            else
            {
                menu.AddDisabledItem(new GUIContent("Clear"));
            }

            menu.DropDown(position);
        }

        private void OnMenuClear()
        {
            m_treeView.PropertyEntries.ClearArray();
            m_treeView.Apply();
        }

        private void OnKeyEventProcessing()
        {
            Event current = Event.current;

            if (current.type == EventType.ValidateCommand)
            {
                if (current.commandName is "SoftDelete" or "Copy" or "Paste" or "Duplicate")
                {
                    current.Use();
                }
            }

            if (current.type == EventType.ExecuteCommand)
            {
                if (current.commandName == "SoftDelete")
                {
                    OnEntryRemove();

                    current.Use();
                }

                if (current.commandName == "Copy")
                {
                    OnEntryCopy();

                    current.Use();
                }

                if (current.commandName == "Paste")
                {
                    OnEntryPaste();

                    current.Use();
                }

                if (current.commandName == "Duplicate")
                {
                    OnEntryAdd();

                    current.Use();
                }
            }
        }

        private bool OnClipboardMatch()
        {
            return m_clipboard.HasAny() && m_clipboard.TryMatch(SerializedObject.targetObject.GetType());
        }

        private void OnUndoOrRedoPerformed()
        {
            m_treeView.SerializedProperty.serializedObject.Update();
            m_treeView.Reload();
        }

        private void OnEntryInitialize(SerializedProperty serializedProperty)
        {
            SerializedProperty propertyId = serializedProperty.FindPropertyRelative(Options.PropertyIdName);
            SerializedProperty propertyName = serializedProperty.FindPropertyRelative(Options.PropertyNameName);
            string entryName = propertyName.stringValue;

            GlobalIdEditorUtility.SetGlobalIdToProperty(propertyId, GlobalId.Generate());

            propertyName.stringValue = OnGetUniqueName(!string.IsNullOrEmpty(entryName) ? entryName : "Entry");
        }

        private string OnGetUniqueName(string entryName)
        {
            string[] names = new string[m_treeView.PropertyEntries.arraySize];

            for (int i = 0; i < m_treeView.PropertyEntries.arraySize; i++)
            {
                SerializedProperty propertyEntry = m_treeView.PropertyEntries.GetArrayElementAtIndex(i);
                SerializedProperty propertyName = propertyEntry.FindPropertyRelative(Options.PropertyNameName);

                names[i] = propertyName.stringValue;
            }

            return ObjectNames.GetUniqueName(names, entryName);
        }
    }
}
