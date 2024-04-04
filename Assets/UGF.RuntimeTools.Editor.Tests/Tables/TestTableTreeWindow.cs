using UGF.RuntimeTools.Editor.Tables;
using UGF.RuntimeTools.Runtime.Tests.Tables;
using UnityEditor;

namespace UGF.RuntimeTools.Editor.Tests.Tables
{
    [TableTreeWindow(typeof(TestTableEntry3Asset))]
    public class TestTableTreeWindow : TableTreeWindow
    {
        protected override TableTreeDrawer OnCreateDrawer(SerializedObject serializedObject, TableTreeOptions options)
        {
            return new TestTableTreeDrawer(serializedObject, options);
        }
    }
}
