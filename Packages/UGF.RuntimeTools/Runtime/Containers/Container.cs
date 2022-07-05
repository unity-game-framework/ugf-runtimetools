using System;
using System.Collections;
using System.Collections.Generic;

namespace UGF.RuntimeTools.Runtime.Containers
{
    public class Container<TValue> : IContainer where TValue : class
    {
        public int Count { get { return m_values.Count; } }

        private readonly List<TValue> m_values = new List<TValue>();

        public void Add(TValue value)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));

            m_values.Add(value);
        }

        public bool Remove(TValue value)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));

            return m_values.Remove(value);
        }

        public T Get<T>() where T : class
        {
            return (T)(object)Get(typeof(T));
        }

        public TValue Get(Type type)
        {
            return TryGet(type, out TValue value) ? value : throw new ArgumentException($"Value not found by the specified type: '{type}'.");
        }

        public bool TryGet<T>(out T value) where T : class
        {
            if (TryGet(typeof(T), out TValue result))
            {
                value = (T)(object)result;
                return true;
            }

            value = default;
            return false;
        }

        public bool TryGet(Type type, out TValue value)
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

        object IContainer.Get(Type type)
        {
            return Get(type);
        }

        bool IContainer.TryGet(Type type, out object value)
        {
            if (TryGet(type, out TValue result))
            {
                value = result;
                return true;
            }

            value = default;
            return false;
        }
    }
}
