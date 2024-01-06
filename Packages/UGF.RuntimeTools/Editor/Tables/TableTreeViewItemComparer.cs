using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.IMGUI.Controls;

namespace UGF.RuntimeTools.Editor.Tables
{
    internal class TableTreeViewItemComparer : IComparer<TreeViewItem>
    {
        public TableTreeViewColumnState Column { get { return m_column ?? throw new ArgumentException("Value not specified."); } }
        public bool HasColumn { get { return m_column != null; } }

        private TableTreeViewColumnState m_column;

        public void SetColumn(TableTreeViewColumnState column)
        {
            m_column = column ?? throw new ArgumentNullException(nameof(column));
        }

        public void ClearColumn()
        {
            m_column = default;
        }

        public int Compare(TreeViewItem x, TreeViewItem y)
        {
            if (ReferenceEquals(x, y)) return 0;
            if (ReferenceEquals(null, y)) return 1;
            if (ReferenceEquals(null, x)) return -1;

            return OnCompare((TableTreeViewItem)x, (TableTreeViewItem)y);
        }

        private int OnCompare(TableTreeViewItem x, TableTreeViewItem y)
        {
            if (HasColumn)
            {
                SerializedProperty propertyX = x.SerializedProperty.FindPropertyRelative(Column.PropertyName);
                SerializedProperty propertyY = y.SerializedProperty.FindPropertyRelative(Column.PropertyName);
                IComparer<SerializedProperty> comparer = Column.PropertyComparer ?? TableTreeDrawerColumnPropertyComparer.Default;

                return m_column.sortedAscending
                    ? comparer.Compare(propertyX, propertyY)
                    : comparer.Compare(propertyY, propertyX);
            }

            return 0;
        }
    }
}
