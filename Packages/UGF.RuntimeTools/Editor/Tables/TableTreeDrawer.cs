using System;
using System.Collections.Generic;
using System.Text;
using UGF.EditorTools.Editor.Ids;
using UGF.EditorTools.Editor.IMGUI;
using UGF.EditorTools.Editor.IMGUI.Dropdown;
using UGF.EditorTools.Editor.IMGUI.Scopes;
using UGF.EditorTools.Runtime.Ids;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace UGF.RuntimeTools.Editor.Tables
{
    public class TableTreeDrawer : DrawerBase
    {
        public SerializedObject SerializedObject { get; }
        public TableTreeOptions Options { get; }
        public TableTreeView TreeView { get; }
        public bool DisplayToolbar { get; set; } = true;
        public bool DisplayFooter { get; set; } = true;

        public event TableTreeViewDrawRowCellHandler DrawRowCellValue;

        private readonly Type m_targetType;
        private readonly Action<SerializedProperty> m_entryInitializeHandler;
        private readonly Func<IEnumerable<DropdownItem<int>>> m_searchSelectionItemsHandler;
        private readonly DropdownSelection<DropdownItem<int>> m_searchSelection = new DropdownSelection<DropdownItem<int>>();
        private readonly List<int> m_selectedIndexes = new List<int>();
        private readonly List<TableTreeViewItem> m_selectedItems = new List<TableTreeViewItem>();
        private SearchField m_search;
        private Styles m_styles;

        private class Styles
        {
            public GUIStyle Toolbar { get; } = EditorStyles.toolbar;
            public GUIStyle Footer { get; } = new GUIStyle("IN Footer");
            public GUIStyle FooterSection { get; } = EditorStyles.toolbarButton;
            public GUIContent FooterClipboardResetButton { get; } = new GUIContent(EditorGUIUtility.FindTexture("Toolbar Minus"), "Reset clipboard.");
            public GUIContent FooterSortingResetButton { get; } = new GUIContent(EditorGUIUtility.FindTexture("Toolbar Minus"), "Reset sorting.");
            public GUIContent FooterSelectionResetButton { get; } = new GUIContent(EditorGUIUtility.FindTexture("Toolbar Minus"), "Clear selection.");
            public GUIContent FooterColumnsResetButton { get; } = new GUIContent(EditorGUIUtility.FindTexture("Toolbar Minus"), "Reset columns.");
            public GUIStyle SearchField { get; } = new GUIStyle("ToolbarSearchTextFieldPopup");
            public GUIStyle SearchButtonCancel { get; } = new GUIStyle("ToolbarSearchCancelButton");
            public GUIStyle SearchButtonCancelEmpty { get; } = new GUIStyle("ToolbarSearchCancelButtonEmpty");
            public GUIContent SearchDropdownContent { get; } = new GUIContent(string.Empty, "Select search column.");
            public GUIContent AddButtonContent { get; } = new GUIContent(EditorGUIUtility.FindTexture("Toolbar Plus"), "Add new or duplicate selected entries.");
            public GUIContent RemoveButtonContent { get; } = new GUIContent(EditorGUIUtility.FindTexture("Toolbar Minus"), "Delete selected entries.");
            public GUIContent MenuButtonContent { get; } = new GUIContent(EditorGUIUtility.FindTexture("_Menu"));
            public GUIContent MenuResetSorting { get; } = new GUIContent("Reset Sorting");
            public GUIContent MenuResetPreferences { get; } = new GUIContent("Reset Preferences");
            public GUIContent MenuResetClipboard { get; } = new GUIContent("Reset Clipboard");
            public GUIContent MenuClear { get; } = new GUIContent("Clear");
            public GUIContent AddButtonChildrenContent { get; } = new GUIContent(EditorGUIUtility.FindTexture("Toolbar Plus"), "Add new or duplicate selected children.");
            public GUILayoutOption[] ToolbarButtonOptions { get; } = { GUILayout.Width(50F) };
            public GUILayoutOption[] ToolbarButtonSmallOptions { get; } = { GUILayout.Width(25F) };

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
            TreeView = new TableTreeView(SerializedObject.FindProperty("m_table"), options);

            m_targetType = SerializedObject.targetObject.GetType();
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

            TableTreeSettings.TryStateRead(m_targetType, TreeView.State);

            TreeView.DrawRowCell += OnDrawRowCell;
            TreeView.KeyEventProcessing += OnKeyEventProcessing;

            TreeView.Reload();
            TreeView.multiColumnHeader.ResizeToFit();
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            TableTreeSettings.StateWrite(m_targetType, TreeView.State);
            TableTreeSettings.Save();

            Undo.undoRedoPerformed -= OnUndoOrRedoPerformed;

            TreeView.DrawRowCell -= OnDrawRowCell;
            TreeView.KeyEventProcessing -= OnKeyEventProcessing;
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
                TableTreeSettings.StateWrite(m_targetType, TreeView.State);
                TableTreeSettings.Save();
            }
        }

        protected void DrawToolbar()
        {
            using (new EditorGUILayout.HorizontalScope(m_styles.Toolbar))
            {
                if (GUILayout.Button(m_styles.AddButtonContent, EditorStyles.toolbarButton, m_styles.ToolbarButtonOptions))
                {
                    OnEntryAdd();
                }

                using (new EditorGUI.DisabledScope(!TreeView.HasSelected()))
                {
                    if (GUILayout.Button(m_styles.RemoveButtonContent, EditorStyles.toolbarButton, m_styles.ToolbarButtonOptions))
                    {
                        OnEntryRemove();
                    }
                }

                GUILayout.FlexibleSpace();

                OnDrawSearch();

                Rect rectMenu = GUILayoutUtility.GetRect(m_styles.MenuButtonContent, EditorStyles.toolbarButton, m_styles.ToolbarButtonSmallOptions);

                if (GUI.Button(rectMenu, m_styles.MenuButtonContent, EditorStyles.toolbarButton))
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
                TreeView.OnGUI(position);
            }
        }

        protected void DrawFooter()
        {
            using (new EditorGUILayout.HorizontalScope(m_styles.Footer))
            {
                string path = AssetDatabase.GetAssetPath(SerializedObject.targetObject);
                int columnsVisible = TreeView.multiColumnHeader.state.visibleColumns.Length;
                int columnsTotal = TreeView.multiColumnHeader.state.columns.Length;
                int countVisible = TreeView.VisibleEntryCount;
                int countTotal = TreeView.PropertyEntries.arraySize;

                if (GUILayout.Button($"Path: {path}", m_styles.FooterSection))
                {
                    EditorGUIUtility.PingObject(SerializedObject.targetObject);
                }

                GUILayout.FlexibleSpace();

                if (TableTreeSettings.ClipboardHasAny() && TableTreeSettings.ClipboardTryMatch(m_targetType))
                {
                    var builder = new StringBuilder("Clipboard:");

                    if (TableTreeSettings.ClipboardEntriesCount > 0)
                    {
                        builder.Append($" Entries {TableTreeSettings.ClipboardEntriesCount}");
                    }

                    if (TableTreeSettings.ClipboardChildrenCount > 0)
                    {
                        builder.Append($" Children {TableTreeSettings.ClipboardChildrenCount}");
                    }

                    using (new EditorGUILayout.HorizontalScope(m_styles.FooterSection))
                    {
                        GUILayout.Label(builder.ToString());

                        if (GUILayout.Button(m_styles.FooterClipboardResetButton, EditorStyles.iconButton))
                        {
                            TableTreeSettings.ClipboardClear();
                            TableTreeSettings.Save();
                        }
                    }
                }

                if (TreeView.HasSortColumn)
                {
                    using (new EditorGUILayout.HorizontalScope(m_styles.FooterSection))
                    {
                        GUILayout.Label($"Sorting Column: {TreeView.SortColumn.DisplayName}");

                        if (GUILayout.Button(m_styles.FooterSortingResetButton, EditorStyles.iconButton))
                        {
                            TreeView.ClearSorting();
                        }
                    }
                }

                if (TreeView.HasSelected())
                {
                    int entryCount = TreeView.GetSelectedCount(TableTreeEntryType.Entry);
                    int childCount = TreeView.GetSelectedCount(TableTreeEntryType.Child);

                    var builder = new StringBuilder("Selected:");

                    if (entryCount > 0)
                    {
                        builder.Append($" Entries {entryCount}");
                    }

                    if (childCount > 0)
                    {
                        builder.Append($" Children: {childCount}");
                    }

                    using (new EditorGUILayout.HorizontalScope(m_styles.FooterSection))
                    {
                        GUILayout.Label(builder.ToString());

                        if (GUILayout.Button(m_styles.FooterSelectionResetButton, EditorStyles.iconButton))
                        {
                            TreeView.ClearSelection();
                        }
                    }
                }

                using (new EditorGUILayout.HorizontalScope(m_styles.FooterSection))
                {
                    GUILayout.Label(columnsVisible == columnsTotal
                        ? $"Columns: {columnsTotal}"
                        : $"Columns: {columnsVisible}/{columnsTotal}");

                    if (columnsVisible != columnsTotal)
                    {
                        if (GUILayout.Button(m_styles.FooterColumnsResetButton, EditorStyles.iconButton))
                        {
                            TreeView.ResetColumns();
                        }
                    }
                }

                GUILayout.Label(countVisible == countTotal
                    ? $"Entries: {countTotal}"
                    : $"Entries: {countVisible}/{countTotal}", m_styles.FooterSection);
            }
        }

        protected virtual void OnDrawRowCell(Rect position, TableTreeViewItem item, SerializedProperty serializedProperty, TableTreeColumnOptions column)
        {
            if (serializedProperty.isArray && serializedProperty.propertyType == SerializedPropertyType.Generic)
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
                OnDrawRowCellValue(position, item, serializedProperty, column);
            }
        }

        protected virtual void OnDrawRowCellValue(Rect position, TableTreeViewItem item, SerializedProperty serializedProperty, TableTreeColumnOptions column)
        {
            if (DrawRowCellValue != null)
            {
                DrawRowCellValue.Invoke(position, item, serializedProperty, column);
            }
            else
            {
                position.height = EditorGUIUtility.singleLineHeight;

                EditorGUI.PropertyField(position, serializedProperty, GUIContent.none, false);
            }
        }

        protected virtual void OnDrawRowCellArray(Rect position, TableTreeViewItem item, SerializedProperty serializedProperty, TableTreeColumnOptions column)
        {
            position.height = EditorGUIUtility.singleLineHeight;

            using (new EditorGUI.DisabledScope(true))
            {
                EditorGUI.IntField(position, GUIContent.none, serializedProperty.arraySize);
            }
        }

        protected virtual void OnDrawRowCellChildren(Rect position, TableTreeViewItem item, SerializedProperty serializedProperty, TableTreeColumnOptions column)
        {
            float height = EditorGUIUtility.singleLineHeight;
            float space = EditorGUIUtility.standardVerticalSpacing;

            var rectField = new Rect(position.x, position.y, position.width - height - space, height);
            var rectButton = new Rect(rectField.xMax + space, position.y + 1F, height, height);

            int count = item.PropertyChildrenSize.intValue;

            EditorGUI.PropertyField(rectField, item.PropertyChildrenSize, GUIContent.none);

            if (count != item.PropertyChildrenSize.intValue)
            {
                TreeView.Reload();
                GUIUtility.ExitGUI();
            }

            if (GUI.Button(rectButton, m_styles.AddButtonChildrenContent, EditorStyles.iconButton))
            {
                OnEntryAddChildren(item);
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
                TreeView.SearchColumnIndex = selected.Value;
            }

            TreeView.searchString = m_search.OnGUI(position, TreeView.searchString, m_styles.SearchField, m_styles.SearchButtonCancel, m_styles.SearchButtonCancelEmpty);

            if (string.IsNullOrEmpty(TreeView.searchString) && !m_search.HasFocus())
            {
                using (new EditorGUI.DisabledScope(true))
                {
                    GUI.Label(rectLabel, TreeView.SearchColumn.DisplayName, EditorStyles.miniLabel);
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
            TreeView.GetChildrenParentSelection(m_selectedItems);

            foreach (TableTreeViewItem item in m_selectedItems)
            {
                TreeView.GetChildrenSelectionIndexes(item, m_selectedIndexes);

                TableTreeEditorInternalUtility.PropertyInsert(item.PropertyChildren, m_selectedIndexes);

                m_selectedIndexes.Clear();
            }

            TreeView.GetSelectionIndexes(m_selectedIndexes);

            if (m_selectedIndexes.Count > 0)
            {
                TableTreeEditorInternalUtility.PropertyInsert(TreeView.PropertyEntries, m_selectedIndexes, m_entryInitializeHandler);
            }
            else
            {
                TableTreeEditorInternalUtility.PropertyInsert(TreeView.PropertyEntries, TreeView.PropertyEntries.arraySize, m_entryInitializeHandler);
            }

            m_selectedIndexes.Clear();
            m_selectedItems.Clear();
            TreeView.Apply();
        }

        private void OnEntryAddChildren(TableTreeViewItem item)
        {
            TreeView.GetChildrenSelectionIndexes(item, m_selectedIndexes);

            if (m_selectedIndexes.Count > 0)
            {
                TableTreeEditorInternalUtility.PropertyInsert(item.PropertyChildren, m_selectedIndexes);

                m_selectedIndexes.Clear();
            }
            else
            {
                TableTreeEditorInternalUtility.PropertyInsert(item.PropertyChildren, item.PropertyChildren.arraySize);
            }

            TreeView.Apply();
            TreeView.SetExpanded(item.id, true);
        }

        private void OnEntryRemove()
        {
            TreeView.GetChildrenParentSelection(m_selectedItems);

            foreach (TableTreeViewItem item in m_selectedItems)
            {
                TreeView.GetChildrenSelectionIndexes(item, m_selectedIndexes);

                TableTreeEditorInternalUtility.PropertyRemove(item.PropertyChildren, m_selectedIndexes);

                m_selectedIndexes.Clear();
            }

            TreeView.GetSelectionIndexes(m_selectedIndexes);

            if (m_selectedIndexes.Count > 0)
            {
                TableTreeEditorInternalUtility.PropertyRemove(TreeView.PropertyEntries, m_selectedIndexes);
            }

            m_selectedIndexes.Clear();
            m_selectedItems.Clear();
            TreeView.Apply();
        }

        private void OnEntryCopy()
        {
            TableTreeSettings.ClipboardClear();

            TreeView.GetSelection(m_selectedItems);

            TableTreeSettings.TryClipboardCopyEntries(m_selectedItems);

            m_selectedItems.Clear();
            TreeView.GetChildrenSelection(m_selectedItems);

            TableTreeSettings.TryClipboardCopyChildren(m_selectedItems);

            m_selectedItems.Clear();

            if (TableTreeSettings.ClipboardHasAny())
            {
                TableTreeSettings.ClipboardCopyType(m_targetType);
                TableTreeSettings.Save();
            }
        }

        private void OnEntryPaste()
        {
            if (TableTreeSettings.ClipboardHasAny() && TableTreeSettings.ClipboardTryMatch(m_targetType))
            {
                TableTreeSettingsData.ClipboardData clipboard = TableTreeSettings.Settings.GetData().Clipboard;

                TreeView.GetSelection(m_selectedItems);

                int index = m_selectedItems.Count > 0 ? m_selectedItems[^1].Index : TreeView.PropertyEntries.arraySize;

                foreach (object value in clipboard.Entries)
                {
                    TableTreeEditorInternalUtility.PropertyInsert(TreeView.PropertyEntries, index, m_entryInitializeHandler, value);
                }

                m_selectedItems.Clear();
                TreeView.GetChildrenSelection(m_selectedItems);

                if (m_selectedItems.Count > 0)
                {
                    TableTreeViewItem item = m_selectedItems[^1];
                    var parent = (TableTreeViewItem)item.parent;

                    foreach (object value in clipboard.Children)
                    {
                        TableTreeEditorInternalUtility.PropertyInsert(parent.PropertyChildren, item.Index, value);
                    }
                }

                m_selectedItems.Clear();
                TreeView.Apply();
            }
        }

        private void OnMenuOpen(Rect position)
        {
            var menu = new GenericMenu();

            if (TreeView.HasSortColumn)
            {
                menu.AddItem(m_styles.MenuResetSorting, false, () => TreeView.ClearSorting());
            }
            else
            {
                menu.AddDisabledItem(m_styles.MenuResetSorting);
            }

            if (TableTreeSettings.HasState(m_targetType))
            {
                menu.AddItem(m_styles.MenuResetPreferences, false, () =>
                {
                    TableTreeSettings.TryStateReset(m_targetType, Options);
                    TableTreeSettings.Save();
                    TableTreeSettings.TryStateRead(m_targetType, TreeView.State);

                    TreeView.multiColumnHeader.ResizeToFit();
                });
            }
            else
            {
                menu.AddDisabledItem(m_styles.MenuResetPreferences);
            }

            if (TableTreeSettings.ClipboardHasAny())
            {
                menu.AddItem(m_styles.MenuResetClipboard, false, () =>
                {
                    TableTreeSettings.ClipboardClear();
                    TableTreeSettings.Save();
                });
            }
            else
            {
                menu.AddDisabledItem(m_styles.MenuResetClipboard);
            }

            menu.AddSeparator(string.Empty);

            if (TreeView != null && TreeView.PropertyEntries.arraySize > 0)
            {
                menu.AddItem(m_styles.MenuClear, false, () =>
                {
                    TreeView.PropertyEntries.ClearArray();
                    TreeView.Apply();
                });
            }
            else
            {
                menu.AddDisabledItem(m_styles.MenuClear);
            }

            menu.DropDown(position);
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

        private void OnUndoOrRedoPerformed()
        {
            TreeView.SerializedProperty.serializedObject.Update();
            TreeView.Reload();
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
            string[] names = new string[TreeView.PropertyEntries.arraySize];

            for (int i = 0; i < TreeView.PropertyEntries.arraySize; i++)
            {
                SerializedProperty propertyEntry = TreeView.PropertyEntries.GetArrayElementAtIndex(i);
                SerializedProperty propertyName = propertyEntry.FindPropertyRelative(Options.PropertyNameName);

                names[i] = propertyName.stringValue;
            }

            return ObjectNames.GetUniqueName(names, entryName);
        }
    }
}
