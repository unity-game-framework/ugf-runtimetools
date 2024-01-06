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
        public SerializedProperty PropertyEntries { get; }
        public int SearchColumnIndex { get; set; }

        public event TableTreeViewDrawRowHandler RowDraw;
        public event TableTreeViewDrawRowCellHandler RowCellDraw;

        private readonly List<TreeViewItem> m_items = new List<TreeViewItem>();
        private readonly TableTreeViewItemComparer m_comparer = new TableTreeViewItemComparer();

        public TableTreeView(SerializedProperty serializedProperty, IReadOnlyList<TableTreeDrawerColumn> columns) : this(serializedProperty, TableTreeEditorInternalUtility.CreateState(columns))
        {
        }

        public TableTreeView(SerializedProperty serializedProperty, TableTreeViewState state) : base(state, new MultiColumnHeader(state.Header))
        {
            SerializedProperty = serializedProperty ?? throw new ArgumentNullException(nameof(serializedProperty));
            PropertyEntries = SerializedProperty.FindPropertyRelative("m_entries");

            showAlternatingRowBackgrounds = true;
            cellMargin = EditorGUIUtility.standardVerticalSpacing;
            rowHeight = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing * 2F;

            multiColumnHeader.sortingChanged += OnSortingChanged;
        }

        protected override TreeViewItem BuildRoot()
        {
            return new TreeViewItem(0, -1);
        }

        protected override IList<TreeViewItem> BuildRows(TreeViewItem root)
        {
            m_items.Clear();

            for (int i = 0; i < PropertyEntries.arraySize; i++)
            {
                SerializedProperty propertyElement = PropertyEntries.GetArrayElementAtIndex(i);

                if (OnCheckSearch(propertyElement))
                {
                    var item = new TableTreeViewItem(i, propertyElement);

                    m_items.Add(item);
                }
            }

            SetupParentsAndChildrenFromDepths(root, m_items);

            if (multiColumnHeader.sortedColumnIndex >= 0)
            {
                m_items.Sort(m_comparer);
            }

            return m_items;
        }

        protected override void RowGUI(RowGUIArgs args)
        {
            int rowIndex = args.row;
            var rowItem = (TableTreeViewItem)args.item;
            int count = args.GetNumVisibleColumns();
            float spacing = EditorGUIUtility.standardVerticalSpacing;

            RowDraw?.Invoke(args.rowRect, args.row, rowItem);

            for (int i = 0; i < count; i++)
            {
                Rect position = args.GetCellRect(i);
                int columnIndex = args.GetColumn(i);
                var columnState = (TableTreeViewColumnState)multiColumnHeader.GetColumn(columnIndex);

                position.xMin += spacing;
                position.xMax -= spacing;
                position.yMin += spacing;
                position.yMax -= spacing;

                if (RowCellDraw != null)
                {
                    RowCellDraw.Invoke(position, rowIndex, rowItem, columnIndex, columnState);
                }
                else
                {
                    SerializedProperty propertyValue = rowItem.SerializedProperty.FindPropertyRelative(columnState.PropertyName);

                    EditorGUI.PropertyField(position, propertyValue, GUIContent.none, false);
                }
            }
        }

        private void OnSortingChanged(MultiColumnHeader header)
        {
            var column = (TableTreeViewColumnState)multiColumnHeader.GetColumn(multiColumnHeader.sortedColumnIndex);

            m_comparer.SetColumn(column);

            Reload();

            m_comparer.ClearColumn();
        }

        private bool OnCheckSearch(SerializedProperty serializedProperty)
        {
            if (hasSearch)
            {
                var column = (TableTreeViewColumnState)multiColumnHeader.GetColumn(SearchColumnIndex);

                SerializedProperty propertyValue = serializedProperty.FindPropertyRelative(column.PropertyName);

                return column.SearchHandler.Check(propertyValue, searchString);
            }

            return true;
        }
    }
}
