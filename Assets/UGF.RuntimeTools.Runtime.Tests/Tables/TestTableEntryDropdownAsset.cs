using UGF.EditorTools.Runtime.Ids;
using UGF.RuntimeTools.Runtime.Tables;
using UnityEngine;

namespace UGF.RuntimeTools.Runtime.Tests.Tables
{
    [CreateAssetMenu(menuName = "Tests/TestTableEntryDropdownAsset")]
    public class TestTableEntryDropdownAsset : ScriptableObject
    {
        [TableEntryDropdown(typeof(TestTableEntry2Asset))]
        [SerializeField] private GlobalId m_id;
        [TableEntryDropdown]
        [SerializeField] private GlobalId m_id2;

        public GlobalId ID { get { return m_id; } set { m_id = value; } }
        public GlobalId ID2 { get { return m_id2; } set { m_id2 = value; } }
    }
}
