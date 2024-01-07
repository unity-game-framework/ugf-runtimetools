using System;
using System.Collections.Generic;
using System.Linq;
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
        public SerializedObject SerializedObject { get { return m_serializedObject ?? throw new ArgumentException("Value not specified."); } }
        public bool HasSerializedObject { get { return m_serializedObject != null; } }
        public IReadOnlyList<TableTreeDrawerColumn> Columns { get { return m_columns ?? throw new ArgumentException("Value not specified."); } }
        public string PropertyIdName { get; set; } = "m_id";
        public string PropertyNameName { get; set; } = "m_name";
        public bool UnlockIds { get; set; }
        public bool DisplayToolbar { get; set; } = true;
        public bool DisplayFooter { get; set; } = true;

        private readonly HashSet<GlobalId> m_selectionIds = new HashSet<GlobalId>();
        private readonly DropdownSelection<DropdownItem<int>> m_searchSelection = new DropdownSelection<DropdownItem<int>>();
        private readonly Func<IEnumerable<DropdownItem<int>>> m_searchSelectionItemsHandler;
        private SerializedObject m_serializedObject;
        private IReadOnlyList<TableTreeDrawerColumn> m_columns;
        private TableTreeView m_treeView;
        private SearchField m_search;
        private Styles m_styles;

        private static readonly HashSet<object> m_clipboardEntries = new HashSet<object>();
        private static Type m_clipboardTableType;

        private class Styles
        {
            public GUIStyle Toolbar { get; } = EditorStyles.toolbar;
            public GUIStyle Footer { get; } = new GUIStyle("IN Footer");
            public GUIStyle SearchField { get; } = new GUIStyle("ToolbarSearchTextFieldPopup");
            public GUIStyle SearchButtonCancel { get; } = new GUIStyle("ToolbarSearchCancelButton");
            public GUIStyle SearchButtonCancelEmpty { get; } = new GUIStyle("ToolbarSearchCancelButtonEmpty");
            public GUIContent SearchDropdownContent { get; } = new GUIContent(string.Empty, "Select search column.");
            public GUIContent AddButtonContent { get; } = new GUIContent(EditorGUIUtility.FindTexture("Toolbar Plus"), "Add new or duplicate selected entries.");
            public GUIContent RemoveButtonContent { get; } = new GUIContent(EditorGUIUtility.FindTexture("Toolbar Minus"), "Delete selected entries.");
            public GUIContent MenuButtonContent { get; } = new GUIContent(EditorGUIUtility.FindTexture("_Menu"));

            public GUILayoutOption[] TableLayoutOptions { get; } =
            {
                GUILayout.ExpandWidth(true),
                GUILayout.ExpandHeight(true)
            };
        }

        public TableTreeDrawer()
        {
            m_searchSelection.Dropdown.RootName = "Search Column";
            m_searchSelection.Dropdown.MinimumWidth = 300F;
            m_searchSelectionItemsHandler = OnGetSearchSelectionItems;
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            Undo.undoRedoPerformed += OnUndoOrRedoPerformed;

            m_treeView = new TableTreeView(SerializedObject.FindProperty("m_table"), Columns);
            m_treeView.RowDraw += OnDrawRow;
            m_treeView.RowCellDraw += OnDrawRowCell;
            m_treeView.KeyEventProcessing += OnKeyEventProcessing;

            m_treeView.Reload();
            m_treeView.multiColumnHeader.ResizeToFit();
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            Undo.undoRedoPerformed -= OnUndoOrRedoPerformed;

            m_treeView.RowDraw -= OnDrawRow;
            m_treeView.RowCellDraw -= OnDrawRowCell;
            m_treeView.KeyEventProcessing -= OnKeyEventProcessing;
            m_treeView = null;

            SerializedObject.Dispose();
        }

        public void SetTarget(TableAsset asset)
        {
            SetTarget(asset, TableTreeEditorUtility.GetEntryColumns(asset));
        }

        public void SetTarget(TableAsset asset, IReadOnlyList<TableTreeDrawerColumn> columns)
        {
            if (asset == null) throw new ArgumentNullException(nameof(asset));

            m_serializedObject = new SerializedObject(asset);
            m_columns = columns;
        }

        public void ClearTarget()
        {
            m_serializedObject = null;
            m_columns = null;
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
            if (m_treeView != null)
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
            else
            {
                using (new EditorGUILayout.VerticalScope(GUILayout.ExpandHeight(true)))
                {
                    EditorGUILayout.HelpBox("No table data to display.", MessageType.Info);
                }
            }
        }

        protected void DrawFooter()
        {
            using (new EditorGUILayout.HorizontalScope(m_styles.Footer))
            {
                string path = HasSerializedObject
                    ? AssetDatabase.GetAssetPath(SerializedObject.targetObject)
                    : "None";

                GUILayout.Label($"Path: {path}");
                GUILayout.FlexibleSpace();

                if (HasSerializedObject)
                {
                    MultiColumnHeaderState.Column column = m_treeView.multiColumnHeader.GetColumn(m_treeView.SearchColumnIndex);
                    var table = (TableAsset)SerializedObject.targetObject;
                    int count = table.Get().Entries.Count();

                    if (OnHasClipboard())
                    {
                        GUILayout.Label($"Clipboard Entries: {m_clipboardEntries.Count}");
                    }

                    GUILayout.Label($"Search Column: {column.headerContent.text}");
                    GUILayout.Label($"Count: {count}");
                }
            }
        }

        protected virtual void OnDrawEntryGUI(Rect position, SerializedProperty serializedProperty, int rowIndex)
        {
        }

        protected virtual void OnDrawEntryCellGUI(Rect position, SerializedProperty serializedProperty, int rowIndex, int columnIndex)
        {
            EditorGUI.PropertyField(position, serializedProperty, GUIContent.none, false);
        }

        private void OnDrawSearch()
        {
            MultiColumnHeaderState.Column column = m_treeView.multiColumnHeader.GetColumn(m_treeView.SearchColumnIndex);

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
                    GUI.Label(rectLabel, column.headerContent.text, EditorStyles.miniLabel);
                }
            }
        }

        private IEnumerable<DropdownItem<int>> OnGetSearchSelectionItems()
        {
            var items = new List<DropdownItem<int>>();

            for (int i = 0; i < m_columns.Count; i++)
            {
                TableTreeDrawerColumn column = m_columns[i];

                items.Add(new DropdownItem<int>(column.DisplayName, i)
                {
                    Priority = m_columns.Count - i
                });
            }

            return items;
        }

        private void OnEntryAdd()
        {
            OnCollectSelection();

            if (m_selectionIds.Count > 0)
            {
                for (int i = 0; i < m_treeView.PropertyEntries.arraySize; i++)
                {
                    SerializedProperty propertyElement = m_treeView.PropertyEntries.GetArrayElementAtIndex(i);
                    SerializedProperty propertyId = propertyElement.FindPropertyRelative(PropertyIdName);
                    GlobalId id = GlobalIdEditorUtility.GetGlobalIdFromProperty(propertyId);

                    if (m_selectionIds.Contains(id))
                    {
                        OnEntryInsert(i++);
                    }
                }

                m_selectionIds.Clear();
            }
            else
            {
                OnEntryInsert(m_treeView.PropertyEntries.arraySize);
            }

            m_treeView.SerializedProperty.serializedObject.ApplyModifiedProperties();
            m_treeView.Reload();
        }

        private void OnEntryInsert(int index, object value = null)
        {
            m_treeView.PropertyEntries.InsertArrayElementAtIndex(index);

            index = Mathf.Min(index + 1, m_treeView.PropertyEntries.arraySize - 1);

            SerializedProperty propertyEntry = m_treeView.PropertyEntries.GetArrayElementAtIndex(index);

            if (value != null)
            {
                propertyEntry.boxedValue = value;
            }

            SerializedProperty propertyId = propertyEntry.FindPropertyRelative(PropertyIdName);
            SerializedProperty propertyName = propertyEntry.FindPropertyRelative(PropertyNameName);
            string entryName = propertyName.stringValue;

            GlobalIdEditorUtility.SetGlobalIdToProperty(propertyId, GlobalId.Generate());

            propertyName.stringValue = OnGetUniqueName(!string.IsNullOrEmpty(entryName) ? entryName : "Entry");
        }

        private void OnEntryRemove()
        {
            OnCollectSelection();

            if (m_selectionIds.Count > 0)
            {
                for (int i = 0; i < m_treeView.PropertyEntries.arraySize; i++)
                {
                    SerializedProperty propertyElement = m_treeView.PropertyEntries.GetArrayElementAtIndex(i);
                    SerializedProperty propertyId = propertyElement.FindPropertyRelative(PropertyIdName);
                    GlobalId id = GlobalIdEditorUtility.GetGlobalIdFromProperty(propertyId);

                    if (m_selectionIds.Contains(id))
                    {
                        m_treeView.PropertyEntries.DeleteArrayElementAtIndex(i--);
                    }
                }

                m_selectionIds.Clear();
                m_treeView.SerializedProperty.serializedObject.ApplyModifiedProperties();
                m_treeView.Reload();
            }
        }

        private void OnEntryCopy()
        {
            if (m_treeView.HasSelection())
            {
                IList<int> selection = m_treeView.GetSelection();

                for (int i = 0; i < selection.Count; i++)
                {
                    int id = selection[i];

                    if (m_treeView.TryGetItem(id, out TableTreeViewItem item))
                    {
                        object value;

                        try
                        {
                            value = item.SerializedProperty.boxedValue;
                        }
                        catch (Exception exception)
                        {
                            Debug.LogWarning($"Table entry can not be copied.\n{exception}");
                            break;
                        }

                        m_clipboardEntries.Add(value);
                    }
                }

                if (m_clipboardEntries.Count > 0)
                {
                    m_clipboardTableType = SerializedObject.targetObject.GetType();
                }
            }
        }

        private void OnEntryPaste()
        {
            if (OnHasClipboard())
            {
                foreach (object value in m_clipboardEntries)
                {
                    OnEntryInsert(m_treeView.PropertyEntries.arraySize, value);
                }

                m_clipboardEntries.Clear();
                m_treeView.SerializedProperty.serializedObject.ApplyModifiedProperties();
                m_treeView.Reload();
            }
        }

        private void OnCollectSelection()
        {
            if (m_treeView.HasSelection())
            {
                IList<int> selection = m_treeView.GetSelection();

                for (int i = 0; i < selection.Count; i++)
                {
                    int id = selection[i];

                    if (m_treeView.TryGetItem(id, out TableTreeViewItem item))
                    {
                        m_selectionIds.Add(item.GetId());
                    }
                }
            }
        }

        private void OnMenuOpen(Rect position)
        {
            var menu = new GenericMenu();

            menu.AddItem(new GUIContent("Ping Asset"), false, () => EditorGUIUtility.PingObject(SerializedObject.targetObject));
            menu.AddItem(new GUIContent("Unlock Ids"), UnlockIds, () => UnlockIds = !UnlockIds);
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
            m_treeView.PropertyEntries.serializedObject.ApplyModifiedProperties();
            m_treeView.Reload();
        }

        private void OnDrawRow(Rect position, int rowIndex, TableTreeViewItem rowItem)
        {
            OnDrawEntryGUI(position, rowItem.SerializedProperty, rowIndex);
        }

        private void OnDrawRowCell(Rect position, int rowIndex, TableTreeViewItem rowItem, int columnIndex, TableTreeViewColumnState columnState)
        {
            SerializedProperty propertyValue = rowItem.SerializedProperty.FindPropertyRelative(columnState.PropertyName);

            using (new EditorGUI.DisabledScope(!UnlockIds && propertyValue.name == PropertyIdName))
            {
                OnDrawEntryCellGUI(position, propertyValue, rowIndex, columnIndex);
            }
        }

        private void OnKeyEventProcessing()
        {
            Event current = Event.current;

            if (current.type == EventType.ValidateCommand)
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
            m_treeView.SerializedProperty.serializedObject.Update();
            m_treeView.Reload();
        }

        private bool OnHasClipboard()
        {
            return m_clipboardTableType != null
                   && m_clipboardTableType == SerializedObject.targetObject.GetType()
                   && m_clipboardEntries.Count > 0;
        }

        private string OnGetUniqueName(string entryName)
        {
            string[] names = new string[m_treeView.PropertyEntries.arraySize];

            for (int i = 0; i < m_treeView.PropertyEntries.arraySize; i++)
            {
                SerializedProperty propertyEntry = m_treeView.PropertyEntries.GetArrayElementAtIndex(i);
                SerializedProperty propertyName = propertyEntry.FindPropertyRelative(PropertyNameName);

                names[i] = propertyName.stringValue;
            }

            return ObjectNames.GetUniqueName(names, entryName);
        }
    }
}
