using System;
using System.Collections.Generic;
using System.Reflection;
using UGF.RuntimeTools.Runtime.Tables;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace UGF.RuntimeTools.Editor.Tables
{
    internal static class TableTreeEditorInternalUtility
    {
        public static TableTreeViewState CreateState(IReadOnlyList<TableTreeDrawerColumn> columns)
        {
            if (columns == null) throw new ArgumentNullException(nameof(columns));

            return new TableTreeViewState
            {
                Header = CreateHeaderState(columns)
            };
        }

        public static MultiColumnHeaderState CreateHeaderState(IReadOnlyList<TableTreeDrawerColumn> columns)
        {
            if (columns == null) throw new ArgumentNullException(nameof(columns));

            var columnStates = new MultiColumnHeaderState.Column[columns.Count];

            for (int i = 0; i < columns.Count; i++)
            {
                TableTreeDrawerColumn column = columns[i];

                columnStates[i] = new TableTreeViewColumnState
                {
                    PropertyName = column.PropertyName,
                    PropertyComparer = column.PropertyComparer,
                    SearchHandler = column.SearchHandler,
                    headerContent = new GUIContent(column.DisplayName)
                };
            }

            return new MultiColumnHeaderState(columnStates);
        }

        public static List<FieldInfo> GetEntryFields(Type type)
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
                throw new ArgumentException($"Table header state can be created from '{typeof(Table<>)}' generic type only.");
            }

            return genericArguments[0];
        }
    }
}
