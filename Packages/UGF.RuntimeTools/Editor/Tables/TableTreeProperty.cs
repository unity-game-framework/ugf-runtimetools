using System;
using System.Collections.Generic;
using UnityEditor;

namespace UGF.RuntimeTools.Editor.Tables
{
    public class TableTreeProperty : TableTree<TableTreeColumnProperty>
    {
        public SerializedProperty SerializedProperty { get; }
        public SerializedProperty PropertyEntries { get; }
        public string PropertyChildrenName { get; }

        public TableTreeProperty(SerializedProperty serializedProperty, IReadOnlyList<TableTreeColumnProperty> columns, string propertyChildrenName = "m_children") : base(columns)
        {
            if (string.IsNullOrEmpty(propertyChildrenName)) throw new ArgumentException("Value cannot be null or empty.", nameof(propertyChildrenName));

            SerializedProperty = serializedProperty ?? throw new ArgumentNullException(nameof(serializedProperty));
            PropertyEntries = SerializedProperty.FindPropertyRelative("m_entries");
            PropertyChildrenName = propertyChildrenName;
        }

        protected override void OnGetItems(ICollection<ITableTreeItem> items)
        {
            for (int i = 0; i < PropertyEntries.arraySize; i++)
            {
                SerializedProperty propertyElement = PropertyEntries.GetArrayElementAtIndex(i);

                var item = new TableTreeItemProperty(propertyElement, Columns, PropertyChildrenName);

                items.Add(item);
            }
        }
    }
}
