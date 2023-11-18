using UGF.EditorTools.Editor.IMGUI;
using UGF.EditorTools.Editor.IMGUI.Scopes;
using UGF.RuntimeTools.Editor.Tables;
using UGF.RuntimeTools.Runtime.Tests.Tables;
using UnityEditor;

namespace UGF.RuntimeTools.Editor.Tests.Tables
{
    [CustomEditor(typeof(TestTableAsset))]
    public class TestTableAssetEditor : UnityEditor.Editor
    {
        private TableDrawer m_tableDrawer;
        private TableDrawer m_tableDrawer2;

        private void OnEnable()
        {
            m_tableDrawer = new TableDrawer(serializedObject.FindProperty("m_table"));
            m_tableDrawer.Enable();

            m_tableDrawer2 = new TableDrawer(serializedObject.FindProperty("m_table"));
            m_tableDrawer2.Enable();
            m_tableDrawer2.DrawingEntryProperties += OnTableDrawer2DrawingEntryProperties;
        }

        private void OnDisable()
        {
            m_tableDrawer.Disable();
            m_tableDrawer2.Disable();
            m_tableDrawer2.DrawingEntryProperties -= OnTableDrawer2DrawingEntryProperties;
        }

        public override void OnInspectorGUI()
        {
            using (new SerializedObjectUpdateScope(serializedObject))
            {
                EditorIMGUIUtility.DrawScriptProperty(serializedObject);

                m_tableDrawer.DrawGUILayout();
                m_tableDrawer2.DrawGUILayout();
            }
        }

        private void OnTableDrawer2DrawingEntryProperties(int index, SerializedProperty propertyEntry)
        {
            EditorGUILayout.HelpBox("Custom Entry Drawing", MessageType.Info);
        }
    }
}
