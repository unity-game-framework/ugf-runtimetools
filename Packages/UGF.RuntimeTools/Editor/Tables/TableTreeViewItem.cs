using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.IMGUI.Controls;

namespace UGF.RuntimeTools.Editor.Tables
{
    internal class TableTreeViewItem : TreeViewItem
    {
        public int Index { get; }
        public SerializedProperty SerializedProperty { get; }
        public bool IsChild { get; }
        public Dictionary<string, SerializedProperty> ColumnProperties { get; } = new Dictionary<string, SerializedProperty>();

        public TableTreeViewItem(int id, int index, SerializedProperty serializedProperty, bool isChild, TableTreeOptions options) : base(id)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));
            if (index < 0) throw new ArgumentOutOfRangeException(nameof(index));

            Index = index;
            SerializedProperty = serializedProperty ?? throw new ArgumentNullException(nameof(serializedProperty));
            IsChild = isChild;

            for (int i = 0; i < options.Columns.Count; i++)
            {
                TableTreeColumnOptions column = options.Columns[i];
                SerializedProperty propertyValue = SerializedProperty.FindPropertyRelative(column.PropertyPath);

                if (propertyValue != null)
                {
                    ColumnProperties.Add(column.PropertyPath, propertyValue);
                }
            }
        }
    }
}
