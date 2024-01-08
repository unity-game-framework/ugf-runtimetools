using System;
using UnityEditor;

namespace UGF.RuntimeTools.Editor.Tables
{
    public abstract class TableTreeItem<TColumn> : TableTreeItem where TColumn : class, ITableTreeColumn
    {
        protected override bool OnTryGetProperty(ITableTreeColumn column, out SerializedProperty serializedProperty)
        {
            if (column is not TColumn columnProperty) throw new ArgumentException($"Table tree colum must be type of '{typeof(TColumn).Name}'.");

            return OnTryGetProperty(columnProperty, out serializedProperty);
        }

        protected abstract bool OnTryGetProperty(TColumn column, out SerializedProperty serializedProperty);
    }
}
