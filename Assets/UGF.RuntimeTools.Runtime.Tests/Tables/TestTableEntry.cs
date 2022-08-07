using System;
using UGF.RuntimeTools.Runtime.Tables;
using UnityEngine;

namespace UGF.RuntimeTools.Runtime.Tests.Tables
{
    [Serializable]
    public class TestTableEntry : TableEntry
    {
        [SerializeField] private string m_text;
        [SerializeField] private bool m_bool;

        public string Text { get { return m_text; } set { m_text = value; } }
        public bool Bool { get { return m_bool; } set { m_bool = value; } }
    }
}
