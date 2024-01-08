using System;
using System.Collections.Generic;
using UGF.RuntimeTools.Runtime.Tables;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace UGF.RuntimeTools.Editor.Tables
{
    public class TableTreeWindow : EditorWindow
    {
        [SerializeField] private string m_assetId;

        private SerializedObject m_serializedObject;
        private TableTreeDrawer m_drawer;

        private void OnEnable()
        {
            var asset = AssetDatabase.LoadAssetAtPath<TableAsset>(AssetDatabase.GUIDToAssetPath(m_assetId));

            if (asset != null)
            {
                SetTarget(asset);
            }
        }

        private void OnDisable()
        {
            m_drawer?.Disable();
            m_drawer = null;
            m_serializedObject.Dispose();
            m_serializedObject = null;
        }

        private void OnGUI()
        {
            if (m_serializedObject?.targetObject == null)
            {
                m_serializedObject = null;
                m_drawer = null;
            }

            m_drawer?.DrawGUILayout();
        }

        public void SetTarget(TableAsset asset)
        {
            if (asset == null) throw new ArgumentNullException(nameof(asset));

            m_serializedObject = new SerializedObject(asset);
            m_assetId = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(asset));
            m_drawer?.Disable();
            m_drawer = new TableTreeDrawer(m_serializedObject, TableTreeEditorUtility.CreateTableTree(m_serializedObject));
            m_drawer.Enable();

            // m_tree = TableTreeEditorUtility.CreateTableTree(m_serializedObject);
            //
            // CreateTreeView(m_tree);
        }

        public void SetTarget(TableAsset asset, ITableTree tableTree)
        {
            if (asset == null) throw new ArgumentNullException(nameof(asset));
            if (tableTree == null) throw new ArgumentNullException(nameof(tableTree));

            m_serializedObject = new SerializedObject(asset);
            m_assetId = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(asset));
            m_drawer?.Disable();
            m_drawer = new TableTreeDrawer(m_serializedObject, tableTree);
            m_drawer.Enable();
        }

        public void ClearTarget()
        {
            m_assetId = string.Empty;
            m_drawer?.Disable();
            m_drawer = null;
        }

        private void CreateTreeView(ITableTree tableTree)
        {
            var treeView = new MultiColumnTreeView
            {
                showAlternatingRowBackgrounds = AlternatingRowBackground.All,
                sortingEnabled = true,
                selectionType = SelectionType.Multiple
            };

            var items = new List<ITableTreeItem>();
            var itemData = new List<TreeViewItemData<ITableTreeItem>>();

            tableTree.GetItems(items);

            for (int i = 0; i < items.Count; i++)
            {
                ITableTreeItem item = items[i];

                itemData.Add(new TreeViewItemData<ITableTreeItem>(i, item));
            }

            treeView.SetRootItems(itemData);

            foreach (ITableTreeColumn column in tableTree.Columns)
            {
                Columns columns = treeView.columns;

                columns.Add(new Column
                {
                    name = column.DisplayName.text,
                    title = column.DisplayName.text,
                    makeCell = () => new PropertyField
                    {
                        label = string.Empty
                    },
                    bindCell = (element, index) =>
                    {
                        var field = (PropertyField)element;
                        var item = treeView.GetItemDataForIndex<ITableTreeItem>(index);

                        field.BindProperty(item.GetProperty(column));
                    }
                });
            }

            rootVisualElement.Add(treeView);
        }
    }
}
