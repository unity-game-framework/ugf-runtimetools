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
        public TableTreeOptions Options { get; }
        public SerializedProperty PropertyEntries { get; }
        public int SearchColumnIndex { get { return m_state.SearchColumnIndex; } set { m_state.SearchColumnIndex = value; } }
        public TableTreeColumnOptions SearchColumn { get { return Options.Columns[SearchColumnIndex]; } }
        public int Count { get { return rootItem.hasChildren ? rootItem.children.Count : 0; } }

        public event TableTreeViewDrawRowCellHandler DrawRowCell;
        public event Action KeyEventProcessing;

        private readonly TableTreeViewState m_state;
        private readonly TableTreeViewItemComparer m_comparer = new TableTreeViewItemComparer();

        public TableTreeView(SerializedProperty serializedProperty, TableTreeOptions options) : this(serializedProperty, options, TableTreeEditorInternalUtility.CreateState(options))
        {
        }

        public TableTreeView(SerializedProperty serializedProperty, TableTreeOptions options, TableTreeViewState state) : base(state, new MultiColumnHeader(state.Header))
        {
            SerializedProperty = serializedProperty ?? throw new ArgumentNullException(nameof(serializedProperty));
            Options = options ?? throw new ArgumentNullException(nameof(options));
            PropertyEntries = SerializedProperty.FindPropertyRelative(Options.PropertyEntriesName);

            m_state = state;

            cellMargin = EditorGUIUtility.standardVerticalSpacing;
            rowHeight = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing * 2F;
            showAlternatingRowBackgrounds = true;
            enableItemHovering = true;

            multiColumnHeader.sortingChanged += OnSortingChanged;
        }

        protected override TreeViewItem BuildRoot()
        {
            var root = new TreeViewItem(0, -1);
            int indexer = 1;

            for (int i = 0; i < PropertyEntries.arraySize; i++)
            {
                SerializedProperty propertyElement = PropertyEntries.GetArrayElementAtIndex(i);

                var item = new TableTreeViewItem(indexer++, i, propertyElement, Options);

                root.AddChild(item);

                SerializedProperty propertyChildren = propertyElement.FindPropertyRelative(Options.PropertyChildrenName);

                if (propertyChildren != null)
                {
                    for (int c = 0; c < propertyChildren.arraySize; c++)
                    {
                        SerializedProperty propertyChild = propertyChildren.GetArrayElementAtIndex(c);

                        item.AddChild(new TableTreeViewItem(indexer++, c, propertyChild, Options)
                        {
                            depth = 1
                        });
                    }
                }
            }

            if (root.hasChildren)
            {
                if (multiColumnHeader.sortedColumnIndex >= 0)
                {
                    TableTreeColumnOptions column = Options.Columns[multiColumnHeader.sortedColumnIndex];
                    MultiColumnHeaderState.Column columnState = multiColumnHeader.GetColumn(multiColumnHeader.sortedColumnIndex);

                    m_comparer.SetColumn(column, columnState.sortedAscending);

                    OnSort(root);

                    m_comparer.ClearColumn();
                }

                SetupDepthsFromParentsAndChildren(root);
            }

            return root;
        }

        protected override IList<TreeViewItem> BuildRows(TreeViewItem root)
        {
            return root.hasChildren ? base.BuildRows(root) : ArraySegment<TreeViewItem>.Empty;
        }

        protected override void RowGUI(RowGUIArgs args)
        {
            var rowItem = (TableTreeViewItem)args.item;
            int count = args.GetNumVisibleColumns();
            float spacing = EditorGUIUtility.standardVerticalSpacing;

            for (int i = 0; i < count; i++)
            {
                int columnIndex = args.GetColumn(i);
                TableTreeColumnOptions column = Options.Columns[columnIndex];

                if (rowItem.ColumnProperties.TryGetValue(column.PropertyPath, out SerializedProperty serializedProperty))
                {
                    Rect position = args.GetCellRect(i);

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
                        DrawRowCell?.Invoke(position, rowItem, serializedProperty, column);
                    }
                    else
                    {
                        EditorGUI.PropertyField(position, serializedProperty, GUIContent.none, false);
                    }
                }
            }
        }

        protected override bool DoesItemMatchSearch(TreeViewItem item, string search)
        {
            var view = (TableTreeViewItem)item;
            TableTreeColumnOptions column = SearchColumn;

            if (view.ColumnProperties.TryGetValue(column.PropertyPath, out SerializedProperty serializedProperty))
            {
                ITableTreeColumnSearcher searcher = column.Searcher ?? TableTreeColumnSearcher.Default;

                return searcher.Check(serializedProperty, search);
            }

            return false;
        }

        protected override void KeyEvent()
        {
            KeyEventProcessing?.Invoke();
        }

        public void Apply()
        {
            SerializedProperty.serializedObject.ApplyModifiedProperties();
            Reload();
        }

        public void GetChildrenSelectionIndexes(TableTreeViewItem item, ICollection<int> indexes)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            if (indexes == null) throw new ArgumentNullException(nameof(indexes));

            for (int i = 0; i < item.children.Count; i++)
            {
                var child = (TableTreeViewItem)item.children[i];

                if (state.selectedIDs.Contains(child.id))
                {
                    indexes.Add(child.Index);
                }
            }
        }

        public void GetChildrenSelection(TableTreeViewItem item, ICollection<TableTreeViewItem> selection)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            if (selection == null) throw new ArgumentNullException(nameof(selection));
            if (!item.hasChildren) throw new ArgumentException("Table tree item must have children.");

            for (int i = 0; i < item.children.Count; i++)
            {
                var child = (TableTreeViewItem)item.children[i];

                if (state.selectedIDs.Contains(child.id))
                {
                    selection.Add(child);
                }
            }
        }

        public void GetSelectionIndexes(ICollection<int> indexes)
        {
            if (indexes == null) throw new ArgumentNullException(nameof(indexes));

            for (int i = 0; i < state.selectedIDs.Count; i++)
            {
                int id = state.selectedIDs[i];

                if (TryGetItem(id, out TableTreeViewItem item) && item.HasPropertyChildren)
                {
                    indexes.Add(item.Index);
                }
            }
        }

        public void GetChildrenParentSelection(ICollection<TableTreeViewItem> selection)
        {
            if (selection == null) throw new ArgumentNullException(nameof(selection));

            for (int i = 0; i < state.selectedIDs.Count; i++)
            {
                int id = state.selectedIDs[i];

                if (TryGetItem(id, out TableTreeViewItem item))
                {
                    if (!item.HasPropertyChildren)
                    {
                        var parent = (TableTreeViewItem)item.parent;

                        if (!selection.Contains(parent))
                        {
                            selection.Add(parent);
                        }
                    }
                }
            }
        }

        public void GetSelection(ICollection<TableTreeViewItem> selection)
        {
            if (selection == null) throw new ArgumentNullException(nameof(selection));

            for (int i = 0; i < state.selectedIDs.Count; i++)
            {
                int id = state.selectedIDs[i];

                if (TryGetItem(id, out TableTreeViewItem item) && item.HasPropertyChildren)
                {
                    selection.Add(item);
                }
            }
        }

        public void ClearSelection()
        {
            SetSelection(ArraySegment<int>.Empty);
        }

        public void ClearSorting()
        {
            multiColumnHeader.sortedColumnIndex = -1;

            Reload();
        }

        public TableTreeViewItem GetItem(int id)
        {
            return TryGetItem(id, out TableTreeViewItem item) ? item : throw new ArgumentException($"Table tree view item not found by the specified id: '{id}'.");
        }

        public bool TryGetItem(int id, out TableTreeViewItem item)
        {
            if (rootItem.hasChildren)
            {
                for (int i = 0; i < rootItem.children.Count; i++)
                {
                    item = (TableTreeViewItem)rootItem.children[i];

                    if (item.id == id || TryGetItem(item, id, out item))
                    {
                        return true;
                    }
                }
            }

            item = default;
            return false;
        }

        public bool TryGetItem(TableTreeViewItem item, int id, out TableTreeViewItem child)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));

            if (item.hasChildren)
            {
                for (int i = 0; i < item.children.Count; i++)
                {
                    child = (TableTreeViewItem)item.children[i];

                    if (child.id == id)
                    {
                        return true;
                    }
                }
            }

            child = default;
            return false;
        }

        private void OnSort(TreeViewItem item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));

            if (item.hasChildren)
            {
                item.children.Sort(m_comparer);

                for (int i = 0; i < item.children.Count; i++)
                {
                    TreeViewItem child = item.children[i];

                    OnSort(child);
                }
            }
        }

        private void OnSortingChanged(MultiColumnHeader header)
        {
            Reload();
        }
    }
}
