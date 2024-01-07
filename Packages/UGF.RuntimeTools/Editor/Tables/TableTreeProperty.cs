using System;
using System.Collections.Generic;
using UnityEditor;

namespace UGF.RuntimeTools.Editor.Tables
{
    public class TableTreeProperty : TableTree
    {
        public SerializedProperty SerializedProperty { get; }

        public TableTreeProperty(SerializedProperty serializedProperty, IReadOnlyList<ITableTreeColumn> columns) : base(columns)
        {
            SerializedProperty = serializedProperty ?? throw new ArgumentNullException(nameof(serializedProperty));
        }

        protected override void OnGetItems(ICollection<ITableTreeItem> items)
        {
            for (int i = 0; i < SerializedProperty.arraySize; i++)
            {
                SerializedProperty propertyElement = SerializedProperty.GetArrayElementAtIndex(i);

                items.Add(new TableTreeItemProperty(propertyElement));
            }
        }
    }
}
