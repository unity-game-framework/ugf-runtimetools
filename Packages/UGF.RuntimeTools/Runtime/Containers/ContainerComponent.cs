using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UGF.RuntimeTools.Runtime.Containers
{
    [AddComponentMenu("Unity Game Framework/Containers/Container Component", 2000)]
    public class ContainerComponent : MonoBehaviour, IContainer
    {
        [SerializeField] private List<Object> m_values = new List<Object>();

        public List<Object> Values { get { return m_values; } }
        public int Count { get { return m_values.Count; } }

        public T Get<T>() where T : class
        {
            return (T)Get(typeof(T));
        }

        public object Get(Type type)
        {
            return TryGet(type, out object value) ? value : throw new ArgumentException($"Value not found by the specified type: '{type}'.");
        }

        public bool TryGet<T>(out T value) where T : class
        {
            if (TryGet(typeof(T), out object result))
            {
                value = (T)result;
                return true;
            }

            value = default;
            return false;
        }

        public bool TryGet(Type type, out object value)
        {
            for (int i = 0; i < m_values.Count; i++)
            {
                value = m_values[i];

                if (type.IsInstanceOfType(value))
                {
                    return true;
                }
            }

            value = default;
            return false;
        }

        public IEnumerator GetEnumerator()
        {
            return m_values.GetEnumerator();
        }
    }
}
