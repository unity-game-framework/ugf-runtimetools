using UGF.EditorTools.Editor.IMGUI;
using UGF.EditorTools.Editor.IMGUI.Scopes;
using UGF.RuntimeTools.Runtime.Tables;
using UnityEditor;

namespace UGF.RuntimeTools.Editor.Tables
{
    [CustomEditor(typeof(TableAsset<>), true)]
    internal class TableAssetDrawer : UnityEditor.Editor
    {
        private TableDrawer m_tableDrawer;

        private void OnEnable()
        {
            m_tableDrawer = new TableDrawer(serializedObject.FindProperty("m_table"));
            m_tableDrawer.Enable();
        }

        private void OnDisable()
        {
            m_tableDrawer.Disable();
        }

        public override void OnInspectorGUI()
        {
            using (new SerializedObjectUpdateScope(serializedObject))
            {
                EditorIMGUIUtility.DrawScriptProperty(serializedObject);

                m_tableDrawer.DrawGUILayout();
            }
        }
    }
}
