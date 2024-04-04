using System;
using System.Collections.Generic;
using UGF.CustomSettings.Runtime;
using UGF.EditorTools.Runtime.IMGUI.Types;
using UnityEngine;

namespace UGF.RuntimeTools.Editor.Tables
{
    public class TableTreeEditorUserSettingsData : CustomSettingsData
    {
        [SerializeField] private ClipboardData m_clipboard = new ClipboardData();
        [SerializeField] private List<StateData> m_states = new List<StateData>();

        public ClipboardData Clipboard { get { return m_clipboard; } }
        public List<StateData> States { get { return m_states; } }

        [Serializable]
        public class StateData
        {
            [SerializeField] private TypeReference m_type;
            [SerializeField] private TableTreeViewState m_state;

            public TypeReference Type { get { return m_type; } set { m_type = value; } }
            public TableTreeViewState State { get { return m_state; } set { m_state = value; } }
        }

        [Serializable]
        public class ClipboardData
        {
            [SerializeField] private TypeReference m_type;
            [SerializeReference] private List<object> m_entries = new List<object>();
            [SerializeReference] private List<object> m_children = new List<object>();

            public TypeReference Type { get { return m_type; } set { m_type = value; } }
            public List<object> Entries { get { return m_entries; } }
            public List<object> Children { get { return m_children; } }
        }
    }
}
