using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace UGF.RuntimeTools.Editor.Tables
{
    public class TableTreeView : TreeView
    {
        public SerializedProperty SerializedProperty { get; }
        public TableTreeOptions Options { get; }
        public SerializedProperty PropertyEntries { get; }
        public int SearchColumnIndex { get { return m_state.SearchColumnIndex; } set { m_state.SearchColumnIndex = value; } }
        public TableTreeColumnOptions SearchColumn { get { return HasSearchColumn ? Options.Columns[m_state.SearchColumnIndex] : throw new ArgumentException("Value not specified."); } }
        public bool HasSearchColumn { get { return m_state.SearchColumnIndex >= 0 && m_state.SearchColumnIndex < Options.Columns.Count; } }
        public TableTreeColumnOptions SortColumn { get { return HasSortColumn ? Options.Columns[multiColumnHeader.sortedColumnIndex] : throw new ArgumentException("Value not specified."); } }
        public bool HasSortColumn { get { return multiColumnHeader.sortedColumnIndex >= 0 && multiColumnHeader.sortedColumnIndex < Options.Columns.Count; } }
        public int ItemsCount { get { return m_items.Count; } }
        public int VisibleCount { get; private set; }
        public int VisibleEntryCount { get; private set; }
        public int ColumnCount { get { return multiColumnHeader.state.columns.Length; } }
        public int ColumnVisibleCount { get { return multiColumnHeader.state.visibleColumns.Length; } }

        public event TableTreeViewDrawRowCellHandler DrawRowCell;
        public event Action KeyEventProcessing;

        private readonly TableTreeViewState m_state;
        private readonly TableTreeViewItemComparer m_comparer = new TableTreeViewItemComparer();
        private readonly Dictionary<int, TableTreeViewItem> m_items = new Dictionary<int, TableTreeViewItem>();
        private readonly List<TreeViewItem> m_rows = new List<TreeViewItem>();

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
            m_items.Clear();

            var root = new TreeViewItem(0, -1);

            for (int i = 0; i < PropertyEntries.arraySize; i++)
            {
                SerializedProperty propertyElement = PropertyEntries.GetArrayElementAtIndex(i);
                int id = TableTreeEditorInternalUtility.GetEntryId(propertyElement, Options);

                var item = new TableTreeViewItem(id, TableTreeEntryType.Entry, i, propertyElement, Options);

                root.AddChild(item);

                m_items.Add(item.id, item);

                SerializedProperty propertyChildren = propertyElement.FindPropertyRelative(Options.PropertyChildrenName);

                if (propertyChildren != null)
                {
                    for (int c = 0; c < propertyChildren.arraySize; c++)
                    {
                        SerializedProperty propertyChild = propertyChildren.GetArrayElementAtIndex(c);
                        int childId = HashCode.Combine(id, c);

                        var child = new TableTreeViewItem(childId, TableTreeEntryType.Child, c, propertyChild, Options)
                        {
                            depth = 1
                        };

                        item.AddChild(child);

                        m_items.Add(child.id, child);
                    }
                }
            }

            if (root.hasChildren)
            {
                if (HasSortColumn)
                {
                    MultiColumnHeaderState.Column columnState = multiColumnHeader.GetColumn(multiColumnHeader.sortedColumnIndex);

                    m_comparer.SetColumn(SortColumn, columnState.sortedAscending);

                    OnSort(root);

                    m_comparer.ClearColumn();
                }

                SetupDepthsFromParentsAndChildren(root);
            }

            return root;
        }

        protected override IList<TreeViewItem> BuildRows(TreeViewItem root)
        {
            m_rows.Clear();

            IList<TreeViewItem> items;

            if (root.hasChildren)
            {
                if (hasSearch)
                {
                    OnSearchRows(root, m_rows, searchString);

                    items = m_rows;
                }
                else
                {
                    items = base.BuildRows(root);
                }
            }
            else
            {
                items = ArraySegment<TreeViewItem>.Empty;
            }

            VisibleCount = items.Count;
            VisibleEntryCount = 0;

            for (int i = 0; i < items.Count; i++)
            {
                var item = (TableTreeViewItem)items[i];

                if (item.EntryType == TableTreeEntryType.Entry)
                {
                    VisibleEntryCount++;
                }
            }

            return items;
        }

        protected override void RowGUI(RowGUIArgs args)
        {
            var rowItem = (TableTreeViewItem)args.item;
            int count = args.GetNumVisibleColumns();
            float spacing = EditorGUIUtility.standardVerticalSpacing;

            for (int i = 0; i < count; i++)
            {
                int columnIndex = args.GetColumn(i);

                if (columnIndex >= 0 && columnIndex < Options.Columns.Count)
                {
                    TableTreeColumnOptions column = Options.Columns[columnIndex];

                    if (rowItem.ColumnProperties.TryGetValue(column, out SerializedProperty serializedProperty))
                    {
                        Rect position = args.GetCellRect(i);

                        position.yMin += spacing;
                        position.yMax -= spacing;

                        if (columnIndex == columnIndexForTreeFoldouts)
                        {
                            if (Options.TryGetChildrenColumn(out _))
                            {
                                position.xMin += foldoutWidth + spacing * 3F;
                            }

                            position.xMax -= spacing;
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

        public bool HasSelected()
        {
            for (int i = 0; i < state.selectedIDs.Count; i++)
            {
                int id = state.selectedIDs[i];

                if (TryGetItem(id, out _))
                {
                    return true;
                }
            }

            return false;
        }

        public int GetSelectedCount(TableTreeEntryType entryType)
        {
            int count = 0;

            for (int i = 0; i < state.selectedIDs.Count; i++)
            {
                int id = state.selectedIDs[i];

                if (TryGetItem(id, out TableTreeViewItem item) && item.EntryType == entryType)
                {
                    count++;
                }
            }

            return count;
        }

        public void GetChildrenSelectionIndexes(TableTreeViewItem item, ICollection<int> indexes)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            if (indexes == null) throw new ArgumentNullException(nameof(indexes));

            if (item.hasChildren)
            {
                for (int i = 0; i < item.children.Count; i++)
                {
                    var child = (TableTreeViewItem)item.children[i];

                    if (state.selectedIDs.Contains(child.id))
                    {
                        indexes.Add(child.Index);
                    }
                }
            }
        }

        public void GetSelectionIndexes(ICollection<int> indexes)
        {
            if (indexes == null) throw new ArgumentNullException(nameof(indexes));

            for (int i = 0; i < state.selectedIDs.Count; i++)
            {
                int id = state.selectedIDs[i];

                if (TryGetItem(id, out TableTreeViewItem item) && item.EntryType == TableTreeEntryType.Entry)
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

        public void GetChildrenSelection(ICollection<TableTreeViewItem> selection)
        {
            if (selection == null) throw new ArgumentNullException(nameof(selection));

            for (int i = 0; i < state.selectedIDs.Count; i++)
            {
                int id = state.selectedIDs[i];

                if (TryGetItem(id, out TableTreeViewItem item) && item.EntryType == TableTreeEntryType.Child)
                {
                    selection.Add(item);
                }
            }
        }

        public void GetSelection(ICollection<TableTreeViewItem> selection)
        {
            if (selection == null) throw new ArgumentNullException(nameof(selection));

            for (int i = 0; i < state.selectedIDs.Count; i++)
            {
                int id = state.selectedIDs[i];

                if (TryGetItem(id, out TableTreeViewItem item) && item.EntryType == TableTreeEntryType.Entry)
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

        public void ResetColumns()
        {
            int[] columns = new int[multiColumnHeader.state.columns.Length];

            for (int i = 0; i < columns.Length; i++)
            {
                columns[i] = i;
            }

            multiColumnHeader.state.visibleColumns = columns;
        }

        public TableTreeViewItem GetItem(int id)
        {
            return TryGetItem(id, out TableTreeViewItem item) ? item : throw new ArgumentException($"Table tree view item not found by the specified id: '{id}'.");
        }

        public bool TryGetItem(int id, out TableTreeViewItem item)
        {
            return m_items.TryGetValue(id, out item);
        }

        private void OnSearchRows(TreeViewItem root, ICollection<TreeViewItem> rows, string search)
        {
            for (int i = 0; i < root.children.Count; i++)
            {
                var item = (TableTreeViewItem)root.children[i];
                bool itemMatch = false;

                if (OnSearch(item, search))
                {
                    rows.Add(item);
                    itemMatch = true;
                }

                if (item.hasChildren)
                {
                    bool itemAdded = false;

                    for (int c = 0; c < item.children.Count; c++)
                    {
                        var child = (TableTreeViewItem)item.children[c];

                        if (itemMatch || OnSearch(child, search))
                        {
                            if (!itemMatch && !itemAdded)
                            {
                                rows.Add(item);
                                itemAdded = true;
                            }

                            rows.Add(child);
                        }
                    }
                }
            }
        }

        private bool OnSearch(TableTreeViewItem item, string search)
        {
            TableTreeColumnOptions column = SearchColumn;

            if (item.ColumnProperties.TryGetValue(column, out SerializedProperty serializedProperty))
            {
                ITableTreeColumnSearcher searcher = column.Searcher ?? TableTreeColumnSearcher.Default;

                return searcher.Check(serializedProperty, search);
            }

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
