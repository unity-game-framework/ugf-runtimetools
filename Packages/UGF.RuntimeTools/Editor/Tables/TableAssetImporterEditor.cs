using UGF.EditorTools.Editor.IMGUI;
using UGF.EditorTools.Editor.IMGUI.Scopes;
using UnityEditor;
using UnityEditor.AssetImporters;
using UnityEngine;

namespace UGF.RuntimeTools.Editor.Tables
{
    [CustomEditor(typeof(TableAssetImporter), true)]
    internal class TableAssetImporterEditor : ScriptedImporterEditor
    {
        public override bool showImportedObject { get; } = false;

        private SerializedProperty m_propertyTablePropertyName;
        private SerializedProperty m_propertyTable;
        private EditorObjectReferenceDrawer m_tableDrawer;

        public override void OnEnable()
        {
            base.OnEnable();

            m_propertyTablePropertyName = serializedObject.FindProperty("m_tablePropertyName");
            m_propertyTable = serializedObject.FindProperty("m_table");

            m_tableDrawer = new EditorObjectReferenceDrawer(serializedObject.FindProperty("m_table"))
            {
                Drawer = { DisplayTitlebar = true }
            };

            m_tableDrawer.Enable();
        }

        public override void OnDisable()
        {
            base.OnDisable();

            m_tableDrawer.Disable();
        }

        public override void OnInspectorGUI()
        {
            using (new SerializedObjectUpdateScope(serializedObject))
            {
                EditorIMGUIUtility.DrawScriptProperty(serializedObject);

                EditorGUILayout.PropertyField(m_propertyTablePropertyName);
                EditorGUILayout.PropertyField(m_propertyTable);
            }

            ApplyRevertGUI();

            EditorGUILayout.Space();

            m_tableDrawer.DrawGUILayout();
        }

        protected override bool OnApplyRevertGUI()
        {
            var importer = (TableAssetImporter)serializedObject.targetObject;

            using (new EditorGUI.DisabledScope(!importer.IsValid()))
            {
                using (new EditorGUI.DisabledScope(!importer.CanImport))
                {
                    if (GUILayout.Button("Import", GUILayout.Width(75F)))
                    {
                        importer.Import();
                    }
                }

                using (new EditorGUI.DisabledScope(!importer.CanExport))
                {
                    if (GUILayout.Button("Export", GUILayout.Width(75F)))
                    {
                        importer.Export();
                    }
                }
            }

            return base.OnApplyRevertGUI();
        }
    }
}
