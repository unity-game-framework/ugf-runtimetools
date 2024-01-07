using UGF.EditorTools.Runtime.Ids;
using UnityEditor;

namespace UGF.RuntimeTools.Editor.Tables
{
    public interface ITableTreeItem
    {
        int Depth { get; }

        GlobalId GetId();
        object GetValue();
        SerializedProperty GetProperty(ITableTreeColumn column);
    }
}
