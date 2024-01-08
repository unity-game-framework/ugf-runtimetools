using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace UGF.RuntimeTools.Editor.Tables
{
    internal static class TableTreeEditorInternalUtility
    {
        public static TableTreeViewState CreateState(TableTreeOptions options)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));

            var columns = new MultiColumnHeaderState.Column[options.Columns.Count];

            for (int i = 0; i < options.Columns.Count; i++)
            {
                TableTreeColumnOptions column = options.Columns[i];

                columns[i] = new MultiColumnHeaderState.Column
                {
                    headerContent = new GUIContent(column.DisplayName)
                };
            }

            return new TableTreeViewState
            {
                Header = new MultiColumnHeaderState(columns)
            };
        }

        public static List<FieldInfo> GetSerializedFields(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            var fields = new List<FieldInfo>();

            while (type != null)
            {
                foreach (FieldInfo field in type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic))
                {
                    if (field.IsDefined(typeof(SerializeField)) || field.IsDefined(typeof(SerializeReference)))
                    {
                        fields.Add(field);
                    }
                }

                type = type.BaseType;
            }

            fields.Sort(TableTreeEntryFieldComparer.Default);

            return fields;
        }

        public static Type GetTableEntryType(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            Type[] genericArguments = type.GetGenericArguments();

            if (genericArguments.Length != 1)
            {
                throw new ArgumentException("Table entry type is unknown.");
            }

            return genericArguments[0];
        }

        public static Type GetTableEntryChildrenType(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            if (type.IsArray)
            {
                return type.GetElementType();
            }

            if (type.GetGenericTypeDefinition() == typeof(List<>))
            {
                Type[] genericArguments = type.GetGenericArguments();

                return genericArguments[0];
            }

            throw new ArgumentException("Table entry children type is unknown.");
        }
    }
}
