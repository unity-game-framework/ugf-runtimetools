using UGF.EditorTools.Runtime.Ids;
using UGF.RuntimeTools.Runtime.Tables;
using UnityEngine;

namespace UGF.RuntimeTools.Runtime.Tests.Tables
{
    [CreateAssetMenu(menuName = "Tests/TestTableEntryDropdownAsset")]
    public class TestTableEntryDropdownAsset : ScriptableObject
    {
        [TableEntryDropdown]
        [SerializeField] private GlobalId m_id;

        public GlobalId ID { get { return m_id; } set { m_id = value; } }
    }
}
