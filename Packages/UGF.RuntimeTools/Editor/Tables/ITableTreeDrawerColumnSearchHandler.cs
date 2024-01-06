using UnityEditor;

namespace UGF.RuntimeTools.Editor.Tables
{
    public interface ITableTreeDrawerColumnSearchHandler
    {
        bool Check(SerializedProperty serializedProperty, string search);
    }
}
