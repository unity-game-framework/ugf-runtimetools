using System;
using UnityEditor.IMGUI.Controls;

namespace UGF.RuntimeTools.Editor.Tables
{
    internal class TableTreeViewItem : TreeViewItem
    {
        public ITableTreeItem Item { get; }

        public TableTreeViewItem(int id, ITableTreeItem item) : base(id)
        {
            Item = item ?? throw new ArgumentNullException(nameof(item));
        }
    }
}
