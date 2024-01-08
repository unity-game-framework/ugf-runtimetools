using System.Collections.Generic;
using UGF.EditorTools.Runtime.Ids;
using UnityEditor;

namespace UGF.RuntimeTools.Editor.Tables
{
    public interface ITableTreeItem
    {
        IReadOnlyList<ITableTreeItem> Children { get; }

        GlobalId GetId();
        object GetValue();
        bool TryGetProperty(ITableTreeColumn column, out SerializedProperty serializedProperty);
    }
}
