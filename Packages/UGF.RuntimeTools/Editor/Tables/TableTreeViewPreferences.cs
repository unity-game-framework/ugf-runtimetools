using System;
using UnityEditor;

namespace UGF.RuntimeTools.Editor.Tables
{
    internal class TableTreeViewPreferences
    {
        public TableTreeView TreeView { get; }
        public string Key { get; }

        private readonly string m_defaultData;

        public TableTreeViewPreferences(TableTreeView treeView, object target)
        {
            if (target == null) throw new ArgumentNullException(nameof(target));

            TreeView = treeView ?? throw new ArgumentNullException(nameof(treeView));

            Key = $"{nameof(TableTreeView)}:{target.GetType().FullName}";

            m_defaultData = EditorJsonUtility.ToJson(treeView.state);
        }

        public bool HasData()
        {
            return EditorPrefs.HasKey(Key);
        }

        public void Read()
        {
            if (EditorPrefs.HasKey(Key))
            {
                string data = EditorPrefs.GetString(Key);

                EditorJsonUtility.FromJsonOverwrite(data, TreeView.state);
            }
        }

        public void Write()
        {
            string data = EditorJsonUtility.ToJson(TreeView.state);

            EditorPrefs.SetString(Key, data);
        }

        public void Reset()
        {
            EditorJsonUtility.FromJsonOverwrite(m_defaultData, TreeView.state);
            EditorPrefs.DeleteKey(Key);
        }
    }
}
