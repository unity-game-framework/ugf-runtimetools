using System.Collections.Generic;
using UGF.RuntimeTools.Editor.Tables;
using UGF.RuntimeTools.Runtime.Options;
using UnityEditor;
using UnityEngine;

namespace UGF.RuntimeTools.Editor.Tests.Tables
{
    public class TestTableTreeDrawer : TableTreeDrawer
    {
        public TestTableTreeDrawer(SerializedObject serializedObject, TableTreeOptions options) : base(serializedObject, options)
        {
        }

        protected override void OnDrawToolbar()
        {
            base.OnDrawToolbar();

            if (GUILayout.Button("Test"))
            {
                var items = new List<TableTreeViewItem>();

                TreeView.GetItems(items, TableTreeEntryType.Entry);

                if (items.Count > 0)
                {
                    AddChildren(items[0], TableTreeEditorClipboard.GetData().Children);
                }
            }
        }

        protected override void OnDrawTable()
        {
            base.OnDrawTable();

            GUILayout.Button("Test");
        }

        protected override void OnDrawFooter()
        {
            base.OnDrawFooter();

            GUILayout.Button("Test");
        }

        protected override void OnMenuCreate(GenericMenu menu)
        {
            base.OnMenuCreate(menu);

            menu.AddItem(new GUIContent("TEST"), false, () => { });
        }

        protected override void OnContextMenuHeaderCreate(GenericMenu menu, Optional<TableTreeColumnOptions> column)
        {
            base.OnContextMenuHeaderCreate(menu, column);

            menu.AddItem(new GUIContent("TEST"), false, () => { });
        }

        protected override void OnContextMenuCreate(GenericMenu menu)
        {
            base.OnContextMenuCreate(menu);

            menu.AddItem(new GUIContent("TEST"), false, () => { });
        }
    }
}
