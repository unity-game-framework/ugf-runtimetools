using System;
using UnityEngine;

namespace UGF.RuntimeTools.Runtime.Scopes
{
    public readonly struct BehaviourEnableScope : IDisposable
    {
        private readonly Behaviour m_behaviour;
        private readonly bool m_enable;

        public BehaviourEnableScope(Behaviour behaviour, bool enable)
        {
            if (behaviour == null) throw new ArgumentNullException(nameof(behaviour));

            m_behaviour = behaviour;
            m_enable = m_behaviour.enabled;

            m_behaviour.enabled = enable;
        }

        public void Dispose()
        {
            m_behaviour.enabled = m_enable;
        }
    }
}
