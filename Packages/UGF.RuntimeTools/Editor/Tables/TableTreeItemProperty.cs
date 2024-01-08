using System;
using System.Collections.Generic;
using UGF.EditorTools.Editor.Ids;
using UGF.EditorTools.Runtime.Ids;
using UnityEditor;

namespace UGF.RuntimeTools.Editor.Tables
{
    public class TableTreeItemProperty : TableTreeItem<TableTreeColumnProperty>
    {
        public SerializedProperty SerializedProperty { get; }
        public SerializedProperty PropertyId { get; }
        public IReadOnlyList<TableTreeColumnProperty> Columns { get; }
        public string PropertyIdName { get; }
        public string PropertyChildrenName { get; }

        private readonly Dictionary<string, SerializedProperty> m_properties = new Dictionary<string, SerializedProperty>();

        public TableTreeItemProperty(SerializedProperty serializedProperty, IReadOnlyList<TableTreeColumnProperty> columns, string propertyChildrenName = "m_children", string propertyIdName = "m_id")
        {
            if (string.IsNullOrEmpty(propertyIdName)) throw new ArgumentException("Value cannot be null or empty.", nameof(propertyIdName));
            if (string.IsNullOrEmpty(propertyChildrenName)) throw new ArgumentException("Value cannot be null or empty.", nameof(propertyChildrenName));

            SerializedProperty = serializedProperty ?? throw new ArgumentNullException(nameof(serializedProperty));
            PropertyId = SerializedProperty.FindPropertyRelative(propertyIdName);
            Columns = columns ?? throw new ArgumentNullException(nameof(columns));
            PropertyIdName = propertyIdName;
            PropertyChildrenName = propertyChildrenName;

            for (int i = 0; i < columns.Count; i++)
            {
                TableTreeColumnProperty column = columns[i];

                SerializedProperty property = SerializedProperty.FindPropertyRelative(column.PropertyPath);

                if (property != null)
                {
                    m_properties.Add(column.PropertyPath, property);

                    if (property.name == PropertyChildrenName)
                    {
                        AddChildren(property);
                    }
                }
            }
        }

        public void AddChildren(SerializedProperty serializedProperty)
        {
            if (serializedProperty == null) throw new ArgumentNullException(nameof(serializedProperty));

            for (int i = 0; i < serializedProperty.arraySize; i++)
            {
                SerializedProperty propertyElement = serializedProperty.GetArrayElementAtIndex(i);

                Children.Add(new TableTreeItemProperty(propertyElement, Columns, PropertyChildrenName, PropertyIdName));
            }
        }

        protected override GlobalId OnGetId()
        {
            return GlobalIdEditorUtility.GetGlobalIdFromProperty(PropertyId);
        }

        protected override object OnGetValue()
        {
            return SerializedProperty.boxedValue;
        }

        protected override bool OnTryGetProperty(TableTreeColumnProperty column, out SerializedProperty serializedProperty)
        {
            return m_properties.TryGetValue(column.PropertyPath, out serializedProperty);
        }
    }
}
