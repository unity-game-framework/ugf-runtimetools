using System;
using System.Collections.Generic;
using UGF.EditorTools.Runtime.Assets;
using UGF.EditorTools.Runtime.Ids;
using UGF.EditorTools.Runtime.IMGUI.References;
using UGF.RuntimeTools.Runtime.Tables;
using UnityEngine;

namespace UGF.RuntimeTools.Runtime.Tests.Tables
{
    [Serializable]
    public class TestTableEntry : TableEntry
    {
        [SerializeField] private string m_text;
        [SerializeField] private bool m_bool;
        [SerializeField] private int m_int;
        [SerializeField] private float m_float;
        [Range(-5F, 5F)]
        [SerializeField] private float m_range;
        [SerializeField] private TypeCode m_enum;
        [AssetId]
        [SerializeField] private GlobalId m_asset;
        [TableEntryDropdown(typeof(TestTableAsset))]
        [SerializeField] private GlobalId m_tableEntry;
        [SerializeField] private List<string> m_list = new List<string>();
        [ManagedReference(typeof(TestTableEntry))]
        [SerializeReference] private ITableEntry m_reference;

        public string Text { get { return m_text; } set { m_text = value; } }
        public bool Bool { get { return m_bool; } set { m_bool = value; } }
        public int Int { get { return m_int; } set { m_int = value; } }
        public float Float { get { return m_float; } set { m_float = value; } }
        public float Range { get { return m_range; } set { m_range = value; } }
        public TypeCode Enum { get { return m_enum; } set { m_enum = value; } }
        public GlobalId Asset { get { return m_asset; } set { m_asset = value; } }
        public GlobalId TableEntry { get { return m_tableEntry; } set { m_tableEntry = value; } }
        public List<string> List { get { return m_list; } set { m_list = value; } }
        public ITableEntry Reference { get { return m_reference; } set { m_reference = value; } }
    }
}
