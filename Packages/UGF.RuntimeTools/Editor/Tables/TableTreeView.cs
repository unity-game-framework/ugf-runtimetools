using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.IMGUI.Controls;

namespace UGF.RuntimeTools.Editor.Tables
{
    internal class TableTreeView : TreeView
    {
        public SerializedProperty SerializedProperty { get; }

        private readonly List<TreeViewItem> m_items = new List<TreeViewItem>();

        public TableTreeView(SerializedProperty serializedProperty, TableTreeViewState state) : base(state, new MultiColumnHeader(state.Header))
        {
            SerializedProperty = serializedProperty ?? throw new ArgumentNullException(nameof(serializedProperty));

            showAlternatingRowBackgrounds = true;
        }

        protected override TreeViewItem BuildRoot()
        {
            return new TreeViewItem(0, -1);
        }

        protected override IList<TreeViewItem> BuildRows(TreeViewItem root)
        {
            m_items.Clear();

            SerializedProperty propertyEntries = SerializedProperty.FindPropertyRelative("m_entries");

            for (int i = 0; i < propertyEntries.arraySize; i++)
            {
                SerializedProperty propertyElement = propertyEntries.GetArrayElementAtIndex(i);

                var item = new TableTreeViewItem(propertyElement);

                m_items.Add(item);
            }

            return m_items;
        }
    }
}
