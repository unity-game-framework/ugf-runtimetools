using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace UGF.RuntimeTools.Editor.Tables
{
    internal class TableTreeView : TreeView
    {
        public SerializedProperty SerializedProperty { get; }
        public ITableTree TableTree { get; }
        public SerializedProperty PropertyEntries { get; }
        public int SearchColumnIndex { get { return m_state.SearchColumnIndex; } set { m_state.SearchColumnIndex = value; } }
        public ITableTreeColumn SearchColumn { get { return TableTree.Columns[SearchColumnIndex]; } }

        public event TableTreeViewDrawRowCellHandler DrawRowCell;
        public event Action KeyEventProcessing;

        private readonly TableTreeViewState m_state;
        private readonly List<ITableTreeItem> m_items = new List<ITableTreeItem>();
        private readonly List<TreeViewItem> m_views = new List<TreeViewItem>();
        private readonly TableTreeViewItemComparer m_comparer = new TableTreeViewItemComparer();

        public TableTreeView(SerializedProperty serializedProperty, ITableTree tableTree) : this(serializedProperty, tableTree, TableTreeEditorInternalUtility.CreateState(tableTree))
        {
        }

        public TableTreeView(SerializedProperty serializedProperty, ITableTree tableTree, TableTreeViewState state) : base(state, new MultiColumnHeader(state.Header))
        {
            SerializedProperty = serializedProperty ?? throw new ArgumentNullException(nameof(serializedProperty));
            TableTree = tableTree ?? throw new ArgumentNullException(nameof(tableTree));
            PropertyEntries = SerializedProperty.FindPropertyRelative("m_entries");

            m_state = state;

            cellMargin = EditorGUIUtility.standardVerticalSpacing;
            rowHeight = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing * 2F;
            showAlternatingRowBackgrounds = true;
            enableItemHovering = true;

            multiColumnHeader.sortingChanged += OnSortingChanged;
        }

        protected override TreeViewItem BuildRoot()
        {
            return new TreeViewItem(0, -1);
        }

        protected override IList<TreeViewItem> BuildRows(TreeViewItem root)
        {
            m_items.Clear();
            m_views.Clear();

            TableTree.GetItems(m_items);

            int indexer = 0;

            for (int i = 0; i < m_items.Count; i++)
            {
                ITableTreeItem item = m_items[i];

                if (OnCheckSearch(item))
                {
                    m_views.Add(new TableTreeViewItem(indexer++, item));

                    foreach (ITableTreeItem child in item.Children)
                    {
                        m_views.Add(new TableTreeViewItem(indexer++, child)
                        {
                            depth = 1
                        });
                    }
                }
            }

            SetupParentsAndChildrenFromDepths(root, m_views);

            if (multiColumnHeader.sortedColumnIndex >= 0)
            {
                ITableTreeColumn column = TableTree.Columns[multiColumnHeader.sortedColumnIndex];
                MultiColumnHeaderState.Column columnState = multiColumnHeader.GetColumn(multiColumnHeader.sortedColumnIndex);

                m_comparer.SetColumn(column, columnState.sortedAscending);
                m_views.Sort(m_comparer);
                m_comparer.ClearColumn();
            }

            return m_views;
        }

        protected override void RowGUI(RowGUIArgs args)
        {
            var rowItem = (TableTreeViewItem)args.item;
            int count = args.GetNumVisibleColumns();
            float spacing = EditorGUIUtility.standardVerticalSpacing;

            for (int i = 0; i < count; i++)
            {
                Rect position = args.GetCellRect(i);
                int columnIndex = args.GetColumn(i);
                ITableTreeColumn column = TableTree.Columns[columnIndex];

                position.xMin += spacing;
                position.xMax -= spacing;
                position.yMin += spacing;
                position.yMax -= spacing;

                if (columnIndex == columnIndexForTreeFoldouts)
                {
                    position.xMin += foldoutWidth + spacing * 2F;
                }

                if (DrawRowCell != null)
                {
                    DrawRowCell?.Invoke(position, rowItem.Item, column);
                }
                else
                {
                    if (rowItem.Item.TryGetProperty(column, out SerializedProperty propertyValue))
                    {
                        EditorGUI.PropertyField(position, propertyValue, GUIContent.none, false);
                    }
                }
            }
        }

        public void Apply()
        {
            SerializedProperty.serializedObject.ApplyModifiedProperties();
            Reload();
        }

        public void ClearSelection()
        {
            SetSelection(ArraySegment<int>.Empty);
        }

        public TableTreeViewItem GetItem(int id)
        {
            return TryGetItem(id, out TableTreeViewItem item) ? item : throw new ArgumentException($"Table tree view item not found by the specified id: '{id}'.");
        }

        public bool TryGetItem(int id, out TableTreeViewItem item)
        {
            for (int i = 0; i < m_views.Count; i++)
            {
                item = (TableTreeViewItem)m_views[i];

                if (item.id == id)
                {
                    return true;
                }
            }

            item = default;
            return false;
        }

        private void OnSortingChanged(MultiColumnHeader header)
        {
            Reload();
        }

        private bool OnCheckSearch(ITableTreeItem item)
        {
            if (hasSearch)
            {
                ITableTreeColumn column = SearchColumn;

                if (item.TryGetProperty(column, out SerializedProperty propertyValue))
                {
                    return column.Searcher.Check(propertyValue, searchString);
                }
            }

            return true;
        }

        protected override void KeyEvent()
        {
            KeyEventProcessing?.Invoke();
        }
    }
}
