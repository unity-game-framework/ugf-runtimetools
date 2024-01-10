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

            TableTreeOptions options = CreateOptions(asset.Get().GetType());

            ShowWindow(asset, options);
        }

        public static void ShowWindow(TableAsset asset, TableTreeOptions options)
        {
            if (asset == null) throw new ArgumentNullException(nameof(asset));
            if (options == null) throw new ArgumentNullException(nameof(options));

            var window = EditorWindow.GetWindow<TableTreeWindow>(asset.name, false);

            window.minSize = new Vector2(500F, 500F);
            window.SetTarget(asset, options);
            window.Show();
        }

        public static TableTreeOptions CreateOptions(Type tableType)
        {
            if (tableType == null) throw new ArgumentNullException(nameof(tableType));

            Type entryType = TableTreeEditorInternalUtility.GetTableEntryType(tableType);

            if (!TryGetEntryChildrenPropertyName(entryType, out string childrenPropertyName))
            {
                childrenPropertyName = "m_children";
            }

            List<TableTreeColumnOptions> columns = CreateColumnOptions(entryType);

            return new TableTreeOptions(columns)
            {
                PropertyChildrenName = childrenPropertyName
            };
        }

        public static List<TableTreeColumnOptions> CreateColumnOptions(Type entryType)
        {
            if (entryType == null) throw new ArgumentNullException(nameof(entryType));

            var columns = new List<TableTreeColumnOptions>();

            if (!TryGetEntryChildrenPropertyName(entryType, out string childrenPropertyName))
            {
                childrenPropertyName = "m_children";
            }

            List<FieldInfo> fields = TableTreeEditorInternalUtility.GetSerializedFields(entryType);

            for (int i = 0; i < fields.Count; i++)
            {
                FieldInfo field = fields[i];

                string displayName = ObjectNames.NicifyVariableName(field.Name);

                columns.Add(new TableTreeColumnOptions(field.Name, displayName, TableTreeEntryType.Entry));

                if (field.Name == childrenPropertyName)
                {
                    Type type = TableTreeEditorInternalUtility.GetTableEntryChildrenType(field.FieldType);

                    CreateColumnOptionsFromFields(columns, TableTreeEditorInternalUtility.GetSerializedFields(type), TableTreeEntryType.Child);
                }
            }

            return columns;
        }

        public static void CreateColumnOptionsFromFields(ICollection<TableTreeColumnOptions> columns, IReadOnlyList<FieldInfo> fields, TableTreeEntryType entryType)
        {
            if (columns == null) throw new ArgumentNullException(nameof(columns));
            if (fields == null) throw new ArgumentNullException(nameof(fields));

            for (int i = 0; i < fields.Count; i++)
            {
                FieldInfo field = fields[i];

                string displayName = ObjectNames.NicifyVariableName(field.Name);

                columns.Add(new TableTreeColumnOptions(field.Name, displayName, entryType));
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
