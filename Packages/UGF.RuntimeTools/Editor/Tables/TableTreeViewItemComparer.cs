using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.IMGUI.Controls;

namespace UGF.RuntimeTools.Editor.Tables
{
    internal class TableTreeViewItemComparer : IComparer<TreeViewItem>
    {
        public ITableTreeColumn Column { get { return m_column ?? throw new ArgumentException("Value not specified."); } }
        public bool HasColumn { get { return m_column != null; } }

        private ITableTreeColumn m_column;
        private bool m_ascending;

        public void SetColumn(ITableTreeColumn column, bool ascending)
        {
            m_column = column ?? throw new ArgumentNullException(nameof(column));
            m_ascending = ascending;
        }

        public void ClearColumn()
        {
            m_column = default;
            m_ascending = false;
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
                SerializedProperty propertyX = x.Item.GetProperty(Column);
                SerializedProperty propertyY = y.Item.GetProperty(Column);

                IComparer<SerializedProperty> comparer = Column.Comparer ?? TableTreeColumnPropertyComparer.Default;

                return m_ascending
                    ? comparer.Compare(propertyX, propertyY)
                    : comparer.Compare(propertyY, propertyX);
            }

            return 0;
        }
    }
}
