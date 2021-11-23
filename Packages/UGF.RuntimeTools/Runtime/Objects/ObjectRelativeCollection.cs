using System;
using System.Collections.Generic;

namespace UGF.RuntimeTools.Runtime.Objects
{
    public class ObjectRelativeCollection<TObject> : IObjectRelativesCollection where TObject : class
    {
        public IReadOnlyCollection<TObject> Objects { get { return m_objects; } }

        IReadOnlyCollection<object> IObjectRelativesCollection.Objects { get { return Objects; } }

        private readonly HashSet<TObject> m_objects = new HashSet<TObject>();

        public void Add(TObject target)
        {
            if (target == null) throw new ArgumentNullException(nameof(target));

            m_objects.Add(target);
        }

        public bool Remove(TObject target)
        {
            if (target == null) throw new ArgumentNullException(nameof(target));

            return m_objects.Remove(target);
        }

        public void Clear()
        {
            m_objects.Clear();
        }

        public T Get<TArguments, T>(TArguments arguments, ObjectRelativePredicate<TArguments, T> predicate) where T : class
        {
            return TryGet(arguments, predicate, out T target) ? target : throw new ArgumentException($"Target not found by the specified type and arguments: '{typeof(T)}, '{arguments}'.");
        }

        public bool TryGet<TArguments, T>(TArguments arguments, ObjectRelativePredicate<TArguments, T> predicate, out T target) where T : class
        {
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

            foreach (TObject element in m_objects)
            {
                target = element as T;

                if (target != null && predicate(arguments, target))
                {
                    return true;
                }
            }

            target = default;
            return false;
        }

        public T Get<T>() where T : class
        {
            return (T)Get(typeof(T));
        }

        public object Get(Type targetType)
        {
            return TryGet(targetType, out object target) ? target : throw new ArgumentException($"Target not found by the specified type: '{targetType}'.");
        }

        public bool TryGet<T>(out T target) where T : class
        {
            if (TryGet(typeof(T), out object result))
            {
                target = (T)result;
                return true;
            }

            target = default;
            return false;
        }

        public bool TryGet(Type targetType, out object target)
        {
            if (targetType == null) throw new ArgumentNullException(nameof(targetType));

            foreach (TObject element in m_objects)
            {
                if (targetType.IsInstanceOfType(element))
                {
                    target = element;
                    return true;
                }
            }

            target = default;
            return false;
        }

        void IObjectRelativesCollection.Add(object target)
        {
            Add((TObject)target);
        }

        bool IObjectRelativesCollection.Remove(object target)
        {
            return Remove((TObject)target);
        }
    }
}
