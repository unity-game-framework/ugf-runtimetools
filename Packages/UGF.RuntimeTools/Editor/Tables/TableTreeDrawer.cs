using System;
using System.Collections.Generic;
using System.Linq;
using UGF.EditorTools.Editor.IMGUI;
using UGF.EditorTools.Editor.IMGUI.Dropdown;
using UGF.EditorTools.Editor.IMGUI.Scopes;
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
        public bool UnlockIds { get; set; }
        public bool DisplayToolbar { get; set; } = true;
        public bool DisplayFooter { get; set; } = true;

        private readonly DropdownSelection<DropdownItem<int>> m_searchSelection = new DropdownSelection<DropdownItem<int>>();
        private readonly Func<IEnumerable<DropdownItem<int>>> m_searchSelectionItemsHandler;
        private SerializedObject m_serializedObject;
        private IReadOnlyList<TableTreeDrawerColumn> m_columns;
        private TableTreeView m_treeView;
        private SearchField m_search;
        private Styles m_styles;

        private class Styles
        {
            public GUIStyle Toolbar { get; } = EditorStyles.toolbar;
            public GUIStyle Footer { get; } = new GUIStyle("IN Footer");
            public GUIContent AddButtonContent { get; } = new GUIContent(EditorGUIUtility.FindTexture("Toolbar Plus"), "Add new entry.");
            public GUIContent RemoveButtonContent { get; } = new GUIContent(EditorGUIUtility.FindTexture("Toolbar Minus"), "Delete current entry.");
            public GUIContent MenuButtonContent { get; } = new GUIContent(EditorGUIUtility.FindTexture("_Menu"));

            public GUILayoutOption[] TableLayoutOptions { get; } =
            {
                GUILayout.ExpandWidth(true),
                GUILayout.ExpandHeight(true)
            };

            public GUILayoutOption[] SearchDropdownLayoutOptions { get; } =
            {
                GUILayout.MaxWidth(100F)
            };
        }

        public TableTreeDrawer()
        {
            m_searchSelectionItemsHandler = GetSearchSelectionItems;
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            m_treeView = new TableTreeView(SerializedObject.FindProperty("m_table"), Columns);
            m_treeView.RowDraw += OnDrawRow;
            m_treeView.RowCellDraw += OnDrawRowCell;

            m_treeView.Reload();
            m_treeView.multiColumnHeader.ResizeToFit();
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            m_treeView.RowDraw -= OnDrawRow;
            m_treeView.RowCellDraw -= OnDrawRowCell;
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
                OnDrawSearch();

                GUILayout.FlexibleSpace();

                using (new EditorGUI.DisabledScope(false))
                {
                    if (TableEditorGUIInternalUtility.DrawToolbarButton(m_styles.RemoveButtonContent))
                    {
                    }
                }

                if (TableEditorGUIInternalUtility.DrawToolbarButton(m_styles.AddButtonContent))
                {
                }

                if (TableEditorGUIInternalUtility.DrawToolbarButton(m_styles.MenuButtonContent, out Rect rectMenu, 25F))
                {
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

                if (HasSerializedObject)
                {
                    var table = (TableAsset)SerializedObject.targetObject;
                    int count = table.Get().Entries.Count();

                    GUILayout.FlexibleSpace();
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
            GUIContent dropdownContent = m_treeView.multiColumnHeader.GetColumn(m_treeView.SearchColumnIndex).headerContent;

            if (DropdownEditorGUIUtility.Dropdown(GUIContent.none, dropdownContent, m_searchSelection, m_searchSelectionItemsHandler, out DropdownItem<int> selected, FocusType.Keyboard, EditorStyles.toolbarDropDown, m_styles.SearchDropdownLayoutOptions))
            {
                m_treeView.SearchColumnIndex = selected.Value;
            }

            Rect rectSearch = GUILayoutUtility.GetRect(250F, EditorGUIUtility.singleLineHeight);

            rectSearch.xMin += EditorGUIUtility.standardVerticalSpacing;
            rectSearch.yMin += EditorGUIUtility.standardVerticalSpacing;

            m_treeView.searchString = m_search.OnToolbarGUI(rectSearch, m_treeView.searchString);
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

        private IEnumerable<DropdownItem<int>> GetSearchSelectionItems()
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
    }
}
