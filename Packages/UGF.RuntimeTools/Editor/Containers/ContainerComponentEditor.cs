using UGF.EditorTools.Editor.IMGUI;
using UGF.EditorTools.Editor.IMGUI.Scopes;
using UGF.RuntimeTools.Runtime.Containers;
using UnityEditor;

namespace UGF.RuntimeTools.Editor.Containers
{
    [CustomEditor(typeof(ContainerComponent), true)]
    internal class ContainerComponentEditor : UnityEditor.Editor
    {
        private ReorderableListDrawer m_listValues;

        private void OnEnable()
        {
            m_listValues = new ReorderableListDrawer(serializedObject.FindProperty("m_values"));
            m_listValues.Enable();
        }

        private void OnDisable()
        {
            m_listValues.Disable();
        }

        public override void OnInspectorGUI()
        {
            using (new SerializedObjectUpdateScope(serializedObject))
            {
                EditorIMGUIUtility.DrawScriptProperty(serializedObject);

                m_listValues.DrawGUILayout();
            }
        }
    }
}
