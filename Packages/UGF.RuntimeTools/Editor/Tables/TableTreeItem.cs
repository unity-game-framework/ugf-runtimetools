using System;
using System.Collections.Generic;
using UGF.EditorTools.Runtime.Ids;
using UnityEditor;

namespace UGF.RuntimeTools.Editor.Tables
{
    public abstract class TableTreeItem : ITableTreeItem
    {
        public List<ITableTreeItem> Children { get; }

        IReadOnlyList<ITableTreeItem> ITableTreeItem.Children { get { return Children; } }

        protected TableTreeItem() : this(new List<ITableTreeItem>())
        {
        }

        protected TableTreeItem(List<ITableTreeItem> children)
        {
            Children = children ?? throw new ArgumentNullException(nameof(children));
        }

        public GlobalId GetId()
        {
            return OnGetId();
        }

        public object GetValue()
        {
            return OnGetValue();
        }

        public bool TryGetProperty(ITableTreeColumn column, out SerializedProperty serializedProperty)
        {
            if (column == null) throw new ArgumentNullException(nameof(column));

            return OnTryGetProperty(column, out serializedProperty);
        }

        protected abstract GlobalId OnGetId();
        protected abstract object OnGetValue();
        protected abstract bool OnTryGetProperty(ITableTreeColumn column, out SerializedProperty serializedProperty);
    }
}
