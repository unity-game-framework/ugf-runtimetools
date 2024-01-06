using UnityEditor;
using UnityEngine;

namespace UGF.RuntimeTools.Editor.Tables
{
    internal static class TableEditorGUIInternalUtility
    {
        public static bool DrawToolbarButton(GUIContent content, float width = 50F)
        {
            return DrawToolbarButton(content, out _, width);
        }

        public static bool DrawToolbarButton(GUIContent content, out Rect position, float width = 50F)
        {
            position = GUILayoutUtility.GetRect(content, EditorStyles.toolbarButton, GUILayout.Width(width));

            return GUI.Button(position, content, EditorStyles.toolbarButton);
        }
    }
}
