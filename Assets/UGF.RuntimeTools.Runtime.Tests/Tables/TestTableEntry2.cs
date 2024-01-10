using System;
using System.Collections.Generic;
using UGF.RuntimeTools.Runtime.Tables;
using UnityEngine;

namespace UGF.RuntimeTools.Runtime.Tests.Tables
{
    [Serializable]
    [TableEntryChildren("m_values")]
    public class TestTableEntry2 : TableEntry
    {
        [SerializeField] private string m_text;
        [SerializeField] private int x;
        [SerializeField] private List<Vector4> m_values = new List<Vector4>();
        [SerializeField] private List<string> m_list = new List<string>();

        public string Text { get { return m_text; } set { m_text = value; } }
        public List<Vector4> Values { get { return m_values; } }
        public List<string> List { get { return m_list; } }
    }
}
