using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.IMGUI.Controls;

namespace UGF.RuntimeTools.Editor.Tables
{
    public class TableTreeViewItem : TreeViewItem
    {
        public int Index { get; }
        public SerializedProperty SerializedProperty { get; }
        public bool IsChild { get; }
        public SerializedProperty PropertyChildren { get { return m_propertyChildren ?? throw new ArgumentException("Value not specified."); } }
        public bool HasPropertyChildren { get { return m_propertyChildren != null; } }
        public Dictionary<TableTreeColumnOptions, SerializedProperty> ColumnProperties { get; } = new Dictionary<TableTreeColumnOptions, SerializedProperty>();

        private readonly SerializedProperty m_propertyChildren;

        public TableTreeViewItem(int id, int index, SerializedProperty serializedProperty, bool isChild, TableTreeOptions options) : base(id)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));
            if (index < 0) throw new ArgumentOutOfRangeException(nameof(index));

            Index = index;
            SerializedProperty = serializedProperty ?? throw new ArgumentNullException(nameof(serializedProperty));
            IsChild = isChild;

            m_propertyChildren = SerializedProperty.FindPropertyRelative(options.PropertyChildrenName);

            for (int i = 0; i < options.Columns.Count; i++)
            {
                TableTreeColumnOptions column = options.Columns[i];
                bool columnCheck = isChild ? column.IsChild : !column.IsChild;

                if (columnCheck)
                {
                    SerializedProperty propertyValue = SerializedProperty.FindPropertyRelative(column.PropertyName);

                    if (propertyValue != null)
                    {
                        ColumnProperties.Add(column, propertyValue);
                    }
                }
            }

            if (isChild && options.TryGetChildrenColumn(out TableTreeColumnOptions childrenColumn) && TableTreeEditorInternalUtility.IsSingleFieldProperty(SerializedProperty))
            {
                ColumnProperties.Add(childrenColumn, SerializedProperty);
            }
        }
    }
}
