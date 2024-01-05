using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace UGF.RuntimeTools.Editor.Tables
{
    internal class TableTreeView : TreeView
    {
        public SerializedProperty SerializedProperty { get; }

        public TableTreeView(SerializedProperty serializedProperty) : base(new TreeViewState())
        {
            SerializedProperty = serializedProperty ?? throw new ArgumentNullException(nameof(serializedProperty));

            multiColumnHeader = new MultiColumnHeader(CreateHeaderState(SerializedProperty));
            showAlternatingRowBackgrounds = true;
            showBorder = true;

            Reload();
        }

        protected override TreeViewItem BuildRoot()
        {
            var root = new TreeViewItem
            {
                id = 0,
                displayName = "Root",
                depth = -1
            };

            SerializedProperty propertyEntries = SerializedProperty.FindPropertyRelative("m_entries");

            for (int i = 0; i < propertyEntries.arraySize; i++)
            {
                SerializedProperty propertyElement = propertyEntries.GetArrayElementAtIndex(i);
                SerializedProperty propertyId = propertyElement.FindPropertyRelative("m_id");
                SerializedProperty propertyName = propertyElement.FindPropertyRelative("m_name");

                root.AddChild(new TreeViewItem
                {
                    id = i,
                    displayName = propertyName.stringValue
                });
            }

            return root;
        }

        protected override void RowGUI(RowGUIArgs args)
        {
            int count = args.GetNumVisibleColumns();
            
            for (int i = 0; i < count; i++)
            {
                
            }
        }

        protected MultiColumnHeaderState CreateHeaderState(SerializedProperty serializedProperty)
        {
            if (serializedProperty == null) throw new ArgumentNullException(nameof(serializedProperty));

            var columns = new List<MultiColumnHeaderState.Column>();

            columns.Add(new MultiColumnHeaderState.Column
            {
                headerContent = new GUIContent("Id")
            });

            columns.Add(new MultiColumnHeaderState.Column
            {
                headerContent = new GUIContent("Name")
            });

            return new MultiColumnHeaderState(columns.ToArray());
        }
    }
}
