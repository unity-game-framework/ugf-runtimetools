﻿using System;
using System.Collections.Generic;
using System.Reflection;
using UGF.RuntimeTools.Runtime.Tables;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace UGF.RuntimeTools.Editor.Tables
{
    internal static class TableTreeEditorInternalUtility
    {
        public static TableTreeViewState CreateState(TableAsset tableAsset)
        {
            if (tableAsset == null) throw new ArgumentNullException(nameof(tableAsset));

            return new TableTreeViewState
            {
                Header = CreateHeaderState(tableAsset)
            };
        }

        public static MultiColumnHeaderState CreateHeaderState(TableAsset tableAsset)
        {
            if (tableAsset == null) throw new ArgumentNullException(nameof(tableAsset));

            ITable table = tableAsset.Get();

            return CreateHeaderStateFromTableType(table.GetType());
        }

        public static MultiColumnHeaderState CreateHeaderStateFromTableType(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            Type[] genericArguments = type.GetGenericArguments();

            if (genericArguments.Length != 1)
            {
                throw new ArgumentException($"Table header state can be created from '{typeof(Table<>)}' generic type only.");
            }

            return CreateHeaderStateFromEntryType(genericArguments[0]);
        }

        public static MultiColumnHeaderState CreateHeaderStateFromEntryType(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            var columns = new List<MultiColumnHeaderState.Column>();

            FieldInfo[] fields = type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic);

            for (int i = 0; i < fields.Length; i++)
            {
                FieldInfo field = fields[i];

                if (field.IsDefined(typeof(SerializeField)))
                {
                    columns.Add(new TableTreeViewColumnState
                    {
                        PropertyName = field.Name,
                        headerContent = new GUIContent(ObjectNames.NicifyVariableName(field.Name)),
                    });
                }
            }

            return new MultiColumnHeaderState(columns.ToArray());
        }
    }
}
