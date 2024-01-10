using System;
using UGF.RuntimeTools.Runtime.Tables;
using UnityEngine;

namespace UGF.RuntimeTools.Runtime.Tests.Tables
{
    [Serializable]
    public class TestTableEntry3 : TableEntry
    {
        [SerializeField] private string m_text;

        public string Text { get { return m_text; } set { m_text = value; } }
    }
}
