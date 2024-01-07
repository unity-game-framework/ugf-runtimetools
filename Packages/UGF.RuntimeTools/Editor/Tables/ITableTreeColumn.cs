using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace UGF.RuntimeTools.Editor.Tables
{
    public interface ITableTreeColumn
    {
        GUIContent DisplayName { get; }
        IComparer<SerializedProperty> Comparer { get; }
        ITableTreeColumnSearcher Searcher { get; }
    }
}
