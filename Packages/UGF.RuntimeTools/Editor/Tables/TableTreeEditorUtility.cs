using System;
using UGF.RuntimeTools.Runtime.Tables;
using UnityEditor;

namespace UGF.RuntimeTools.Editor.Tables
{
    public static class TableTreeEditorUtility
    {
        public static void ShowWindow(TableAsset tableAsset)
        {
            if (tableAsset == null) throw new ArgumentNullException(nameof(tableAsset));

            var window = EditorWindow.GetWindow<TableTreeWindow>(tableAsset.name, false);

            window.SetTarget(tableAsset);
            window.Show();
        }
    }
}
