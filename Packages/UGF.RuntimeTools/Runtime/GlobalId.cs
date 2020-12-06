using System;
using UnityEngine;

namespace UGF.RuntimeTools.Runtime
{
    [Serializable]
    public struct GlobalId
    {
        [SerializeField] private ulong m_first;
        [SerializeField] private ulong m_second;

        public void Test()
        {
        }
    }
}
