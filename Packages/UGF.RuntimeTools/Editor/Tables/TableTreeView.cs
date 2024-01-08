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

        public event TableTreeViewDrawRowCellHandler DrawRowCell;
        public event Action KeyEventProcessing;

        private readonly TableTreeViewState m_state;
        private readonly List<TreeViewItem> m_items = new List<TreeViewItem>();
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
            return new TreeViewItem(0, -1);
        }

        protected override IList<TreeViewItem> BuildRows(TreeViewItem root)
        {
            m_items.Clear();

            int indexer = 0;

            for (int i = 0; i < PropertyEntries.arraySize; i++)
            {
                SerializedProperty propertyElement = PropertyEntries.GetArrayElementAtIndex(i);

                if (OnCheckSearch(propertyElement))
                {
                    m_items.Add(new TableTreeViewItem(indexer++, i, propertyElement, false, Options));
                }

                SerializedProperty propertyChildren = propertyElement.FindPropertyRelative(Options.PropertyChildrenName);

                if (propertyChildren != null)
                {
                    for (int c = 0; c < propertyChildren.arraySize; c++)
                    {
                        SerializedProperty propertyChild = propertyChildren.GetArrayElementAtIndex(c);

                        if (OnCheckSearch(propertyChild))
                        {
                            m_items.Add(new TableTreeViewItem(indexer++, c, propertyChild, true, Options)
                            {
                                depth = 1
                            });
                        }
                    }
                }
            }

            SetupParentsAndChildrenFromDepths(root, m_items);

            if (multiColumnHeader.sortedColumnIndex >= 0)
            {
                TableTreeColumnOptions column = Options.Columns[multiColumnHeader.sortedColumnIndex];
                MultiColumnHeaderState.Column columnState = multiColumnHeader.GetColumn(multiColumnHeader.sortedColumnIndex);

                m_comparer.SetColumn(column, columnState.sortedAscending);
                m_items.Sort(m_comparer);
                m_comparer.ClearColumn();
            }

            return m_items;
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
                        DrawRowCell?.Invoke(position, serializedProperty, column);
                    }
                    else
                    {
                        EditorGUI.PropertyField(position, serializedProperty, GUIContent.none, false);
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
            for (int i = 0; i < m_items.Count; i++)
            {
                item = (TableTreeViewItem)m_items[i];

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

        private bool OnCheckSearch(SerializedProperty serializedProperty)
        {
            if (hasSearch)
            {
                TableTreeColumnOptions column = SearchColumn;
                SerializedProperty propertyValue = serializedProperty.FindPropertyRelative(column.PropertyPath);

                if (propertyValue != null)
                {
                    ITableTreeColumnSearcher searcher = column.Searcher ?? TableTreeColumnSearcher.Default;

                    return searcher.Check(propertyValue, searchString);
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
