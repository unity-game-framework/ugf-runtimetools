using System;
using UGF.EditorTools.Editor.Ids;
using UGF.EditorTools.Runtime.Ids;
using UnityEditor;

namespace UGF.RuntimeTools.Editor.Tables
{
    public class TableTreeItemProperty : TableTreeItem
    {
        public SerializedProperty SerializedProperty { get; }
        public string PropertyIdName { get; }

        public TableTreeItemProperty(SerializedProperty serializedProperty, string propertyIdName = "m_id")
        {
            if (string.IsNullOrEmpty(propertyIdName)) throw new ArgumentException("Value cannot be null or empty.", nameof(propertyIdName));

            SerializedProperty = serializedProperty ?? throw new ArgumentNullException(nameof(serializedProperty));
            PropertyIdName = propertyIdName;
        }

        protected override GlobalId OnGetId()
        {
            SerializedProperty propertyId = SerializedProperty.FindPropertyRelative("m_id");

            return GlobalIdEditorUtility.GetGlobalIdFromProperty(propertyId);
        }

        protected override object OnGetValue()
        {
            return SerializedProperty.boxedValue;
        }

        protected override SerializedProperty OnGetProperty(ITableTreeColumn column)
        {
            if (column is not TableTreeColumnProperty columnProperty) throw new ArgumentException($"Table tree colum must be type of '{nameof(TableTreeColumnProperty)}'.");

            return SerializedProperty.FindPropertyRelative(columnProperty.PropertyPath);
        }
    }
}
