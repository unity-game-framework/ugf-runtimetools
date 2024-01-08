using System;
using System.Collections.Generic;
using System.Reflection;
using UGF.RuntimeTools.Runtime.Tables;
using UnityEditor;
using UnityEngine;

namespace UGF.RuntimeTools.Editor.Tables
{
    public static class TableTreeEditorUtility
    {
        public static void ShowWindow(TableAsset asset)
        {
            if (asset == null) throw new ArgumentNullException(nameof(asset));

            var window = EditorWindow.GetWindow<TableTreeWindow>(asset.name, false);

            window.minSize = new Vector2(500F, 500F);
            window.SetTarget(asset);
            window.Show();
        }

        public static void ShowWindow(TableAsset asset, ITableTree tableTree)
        {
            if (asset == null) throw new ArgumentNullException(nameof(asset));
            if (tableTree == null) throw new ArgumentNullException(nameof(tableTree));

            var window = EditorWindow.GetWindow<TableTreeWindow>(asset.name, false);

            window.minSize = new Vector2(500F, 500F);
            window.SetTarget(asset, tableTree);
            window.Show();
        }

        public static ITableTree CreateTableTree(SerializedObject serializedObject)
        {
            if (serializedObject == null) throw new ArgumentNullException(nameof(serializedObject));

            Type tableType = ((TableAsset)serializedObject.targetObject).Get().GetType();
            Type tableEntryType = TableTreeEditorInternalUtility.GetTableEntryType(tableType);
            List<TableTreeColumnProperty> columns = CreateColumnsFromFields(tableEntryType);

            if (!TryGetEntryChildrenPropertyName(tableEntryType, out string childrenPropertyName))
            {
                childrenPropertyName = "m_children";
            }

            return new TableTreeProperty(serializedObject.FindProperty("m_table"), columns, childrenPropertyName);
        }

        public static List<TableTreeColumnProperty> CreateColumnsFromFields(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            var columns = new List<TableTreeColumnProperty>();

            if (!TryGetEntryChildrenPropertyName(type, out string childrenPropertyName))
            {
                childrenPropertyName = string.Empty;
            }

            List<FieldInfo> fields = TableTreeEditorInternalUtility.GetSerializedFields(type);

            CreateColumnsFromFields(columns, fields);

            for (int i = 0; i < fields.Count; i++)
            {
                FieldInfo field = fields[i];

                if (!string.IsNullOrEmpty(childrenPropertyName) && field.Name == childrenPropertyName)
                {
                    CreateColumnsFromFields(columns, TableTreeEditorInternalUtility.GetTableEntryChildrenType(field.FieldType));
                    break;
                }
            }

            return columns;
        }

        public static void CreateColumnsFromFields(ICollection<TableTreeColumnProperty> columns, Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            CreateColumnsFromFields(columns, TableTreeEditorInternalUtility.GetSerializedFields(type));
        }

        public static void CreateColumnsFromFields(ICollection<TableTreeColumnProperty> columns, IReadOnlyList<FieldInfo> fields)
        {
            if (columns == null) throw new ArgumentNullException(nameof(columns));
            if (fields == null) throw new ArgumentNullException(nameof(fields));

            for (int i = 0; i < fields.Count; i++)
            {
                FieldInfo field = fields[i];

                var displayName = new GUIContent(ObjectNames.NicifyVariableName(field.Name));
                var column = new TableTreeColumnProperty(displayName, field.Name);

                columns.Add(column);
            }
        }

        public static bool TryGetEntryChildrenPropertyName(Type entryType, out string propertyName)
        {
            if (entryType == null) throw new ArgumentNullException(nameof(entryType));

            var attribute = entryType.GetCustomAttribute<TableEntryChildrenAttribute>();

            if (attribute != null)
            {
                propertyName = attribute.PropertyName;
                return true;
            }

            propertyName = default;
            return false;
        }
    }
}
