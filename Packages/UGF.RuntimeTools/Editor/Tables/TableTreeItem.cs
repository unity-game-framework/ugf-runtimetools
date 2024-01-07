using System;
using UGF.EditorTools.Runtime.Ids;
using UnityEditor;

namespace UGF.RuntimeTools.Editor.Tables
{
    public abstract class TableTreeItem : ITableTreeItem
    {
        public int Depth { get; set; }

        public GlobalId GetId()
        {
            return OnGetId();
        }

        public object GetValue()
        {
            return OnGetValue();
        }

        public SerializedProperty GetProperty(ITableTreeColumn column)
        {
            if (column == null) throw new ArgumentNullException(nameof(column));

            return OnGetProperty(column);
        }

        protected abstract GlobalId OnGetId();
        protected abstract object OnGetValue();
        protected abstract SerializedProperty OnGetProperty(ITableTreeColumn column);
    }
}
