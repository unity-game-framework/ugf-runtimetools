using System.IO;
using UnityEditor;
using UnityEngine;

namespace UGF.RuntimeTools.Runtime.Tests
{
    [CreateAssetMenu(menuName = "Tests/TestGlobalIdAsset")]
    public class TestGlobalIdAsset : ScriptableObject
    {
        [SerializeField] private GlobalId m_id;

        public GlobalId Id { get { return m_id; } set { m_id = value; } }

        [ContextMenu("SaveAsJsonRuntime", false, 2000)]
        private void SaveAsJsonRuntime()
        {
            string path = AssetDatabase.GetAssetPath(this);
            string text = JsonUtility.ToJson(this, true);

            File.WriteAllText($"{path}.runtime.json", text);
            AssetDatabase.Refresh();
        }

        [ContextMenu("SaveAsJsonEditor", false, 2000)]
        private void SaveAsJsonEditor()
        {
            string path = AssetDatabase.GetAssetPath(this);
            string text = EditorJsonUtility.ToJson(this, true);

            File.WriteAllText($"{path}.editor.json", text);
            AssetDatabase.Refresh();
        }
    }
}
