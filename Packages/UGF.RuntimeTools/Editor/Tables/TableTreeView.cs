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
        public SerializedProperty PropertyEntries { get; }

        private readonly List<TreeViewItem> m_items = new List<TreeViewItem>();

        public TableTreeView(SerializedObject serializedObject, TableTreeViewState state) : this(serializedObject.FindProperty("m_table"), state)
        {
        }

        public TableTreeView(SerializedProperty serializedProperty, TableTreeViewState state) : base(state, new MultiColumnHeader(state.Header))
        {
            SerializedProperty = serializedProperty ?? throw new ArgumentNullException(nameof(serializedProperty));
            PropertyEntries = SerializedProperty.FindPropertyRelative("m_entries");

            showAlternatingRowBackgrounds = true;
            cellMargin = EditorGUIUtility.standardVerticalSpacing;
            rowHeight = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing * 2F;
        }

        protected override TreeViewItem BuildRoot()
        {
            return new TreeViewItem(0, -1);
        }

        protected override IList<TreeViewItem> BuildRows(TreeViewItem root)
        {
            m_items.Clear();

            for (int i = 0; i < PropertyEntries.arraySize; i++)
            {
                SerializedProperty propertyElement = PropertyEntries.GetArrayElementAtIndex(i);

                var item = new TableTreeViewItem(i, propertyElement);

                m_items.Add(item);
            }

            SetupParentsAndChildrenFromDepths(root, m_items);

            return m_items;
        }

        protected override void RowGUI(RowGUIArgs args)
        {
            int count = args.GetNumVisibleColumns();
            float spacing = EditorGUIUtility.standardVerticalSpacing;

            for (int i = 0; i < count; i++)
            {
                Rect position = args.GetCellRect(i);
                var rowItem = (TableTreeViewItem)args.item;
                int rowIndex = args.row;
                int columnIndex = args.GetColumn(i);
                var columnState = (TableTreeViewColumnState)multiColumnHeader.GetColumn(columnIndex);

                position.xMin += spacing;
                position.xMax -= spacing;
                position.yMin += spacing;
                position.yMax -= spacing;

                OnDrawCellGUI(position, rowIndex, rowItem, columnIndex, columnState);
            }
        }

        private void OnDrawCellGUI(Rect position, int rowIndex, TableTreeViewItem rowItem, int columnIndex, TableTreeViewColumnState columnState)
        {
            SerializedProperty propertyValue = rowItem.SerializedProperty.FindPropertyRelative(columnState.PropertyName);

            OnDrawCellGUI(position, propertyValue);
        }

        private void OnDrawCellGUI(Rect position, SerializedProperty serializedProperty)
        {
            EditorGUI.PropertyField(position, serializedProperty, GUIContent.none, true);
        }
    }
}
