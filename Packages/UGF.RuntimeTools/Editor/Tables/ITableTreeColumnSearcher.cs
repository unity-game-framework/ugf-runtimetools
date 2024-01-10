using UnityEditor;

namespace UGF.RuntimeTools.Editor.Tables
{
    public interface ITableTreeColumnSearcher
    {
        bool Check(SerializedProperty serializedProperty, string search);
    }
}
