using System;
using System.Collections;
using System.Collections.Generic;

namespace UGF.RuntimeTools.Runtime.Contexts
{
    public class Context : ContextBase
    {
        private readonly Dictionary<Type, List<object>> m_values = new Dictionary<Type, List<object>>();

        public new Dictionary<Type, List<object>>.Enumerator GetEnumerator()
        {
            return m_values.GetEnumerator();
        }

        protected override bool OnContains(object value)
        {
            Type type = value.GetType();

            if (m_values.TryGetValue(type, out List<object> values) && values.Contains(value))
            {
                return true;
            }

            foreach (KeyValuePair<Type, List<object>> pair in m_values)
            {
                if (type.IsAssignableFrom(pair.Key) && pair.Value.Contains(value))
                {
                    return true;
                }
            }

            return false;
        }

        protected override void OnAdd(object value)
        {
            Type type = value.GetType();

            if (!m_values.TryGetValue(type, out List<object> values))
            {
                values = new List<object>();

                m_values.Add(type, values);
            }

            values.Add(value);
        }

        protected override bool OnRemove(object value)
        {
            Type type = value.GetType();

            if (m_values.TryGetValue(type, out List<object> values) && values.Remove(value))
            {
                if (values.Count == 0)
                {
                    m_values.Remove(type);
                }

                return true;
            }

            return false;
        }

        protected override void OnClear()
        {
            m_values.Clear();
        }

        protected override bool OnTryGet(Type type, out object value)
        {
            if (m_values.TryGetValue(type, out List<object> values))
            {
                value = values[0];
                return true;
            }

            foreach (KeyValuePair<Type, List<object>> pair in m_values)
            {
                if (type.IsAssignableFrom(pair.Key))
                {
                    value = pair.Value[0];
                    return true;
                }
            }

            value = default;
            return false;
        }

        protected override bool OnTryGet<TArguments, T>(TArguments arguments, ContextPredicate<TArguments, T> predicate, out T value)
        {
            Type type = typeof(T);

            if (m_values.TryGetValue(type, out List<object> values))
            {
                for (int i = 0; i < values.Count; i++)
                {
                    value = (T)values[i];

                    if (predicate(arguments, value))
                    {
                        return true;
                    }
                }
            }

            foreach (KeyValuePair<Type, List<object>> pair in m_values)
            {
                if (type.IsAssignableFrom(pair.Key))
                {
                    for (int i = 0; i < pair.Value.Count; i++)
                    {
                        value = (T)pair.Value[i];

                        if (predicate(arguments, value))
                        {
                            return true;
                        }
                    }
                }
            }

            value = default;
            return false;
        }

        protected override bool OnTryGet<TArguments>(TArguments arguments, ContextPredicate<TArguments, object> predicate, out object value)
        {
            foreach (KeyValuePair<Type, List<object>> pair in m_values)
            {
                for (int i = 0; i < pair.Value.Count; i++)
                {
                    value = pair.Value[i];

                    if (predicate(arguments, value))
                    {
                        return true;
                    }
                }
            }

            value = default;
            return false;
        }

        protected override IEnumerator OnGetEnumerator()
        {
            foreach (KeyValuePair<Type, List<object>> pair in m_values)
            {
                for (int i = 0; i < pair.Value.Count; i++)
                {
                    object value = pair.Value[i];

                    yield return value;
                }
            }
        }
    }
}
