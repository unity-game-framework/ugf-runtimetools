using UnityEditor;
using UnityEngine;

namespace UGF.RuntimeTools.Editor.Tables
{
    public class TableWindow : EditorWindow
    {
        private readonly TableTreeDrawer m_drawer = new TableTreeDrawer();

        private void OnEnable()
        {
            m_drawer.Enable();
        }

        private void OnDisable()
        {
            m_drawer.Disable();
        }

        private void OnGUI()
        {
            m_drawer.DrawGUI(new Rect(0F, 0F, position.width, position.height));
        }

        public void SetSerializedProperty(SerializedProperty serializedProperty)
        {
            m_drawer.SetSerializedProperty(serializedProperty);
        }

        public void ClearSerializedProperty()
        {
            m_drawer.ClearSerializedProperty();
        }
    }
}
