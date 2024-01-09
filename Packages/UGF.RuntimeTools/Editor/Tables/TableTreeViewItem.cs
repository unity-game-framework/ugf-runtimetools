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
        public SerializedProperty PropertyChildren { get { return m_propertyChildren ?? throw new ArgumentException("Value not specified."); } }
        public bool HasPropertyChildren { get { return m_propertyChildren != null; } }
        public Dictionary<string, SerializedProperty> ColumnProperties { get; } = new Dictionary<string, SerializedProperty>();

        private readonly SerializedProperty m_propertyChildren;

        public TableTreeViewItem(int id, int index, SerializedProperty serializedProperty, TableTreeOptions options) : base(id)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));
            if (index < 0) throw new ArgumentOutOfRangeException(nameof(index));

            Index = index;
            SerializedProperty = serializedProperty ?? throw new ArgumentNullException(nameof(serializedProperty));

            for (int i = 0; i < options.Columns.Count; i++)
            {
                TableTreeColumnOptions column = options.Columns[i];
                SerializedProperty propertyValue = SerializedProperty.FindPropertyRelative(column.PropertyPath);

                if (propertyValue != null)
                {
                    if (m_propertyChildren == null && propertyValue.name == options.PropertyChildrenName)
                    {
                        m_propertyChildren = propertyValue;
                    }

                    ColumnProperties.Add(column.PropertyPath, propertyValue);
                }
            }
        }
    }
}
