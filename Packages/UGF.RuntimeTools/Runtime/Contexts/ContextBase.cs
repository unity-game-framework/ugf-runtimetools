using System;
using System.Collections;
using System.Collections.Generic;

namespace UGF.RuntimeTools.Runtime.Contexts
{
    public abstract class ContextBase : IContext
    {
        public event ContextValueHandler Added;
        public event ContextValueHandler Removed;
        public event ContextHandler Cleared;

        public void Add(object value)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));

            OnAdd(value);

            Added?.Invoke(this, value);
        }

        public bool Remove(object value)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));

            if (OnRemove(value))
            {
                Removed?.Invoke(this, value);
                return true;
            }

            return false;
        }

        public void Clear()
        {
            OnClear();

            Cleared?.Invoke(this);
        }

        public T Get<T>() where T : class
        {
            return (T)Get(typeof(T));
        }

        public object Get(Type type)
        {
            return TryGet(type, out object value) ? value : throw new ContextValueNotFoundByTypeException(type);
        }

        public T Get<TArguments, T>(TArguments arguments, ContextPredicate<TArguments, T> predicate)
        {
            return TryGet(arguments, predicate, out T value) ? value : throw new ContextValueNotFoundByArgumentsException(arguments);
        }

        public object Get<TArguments>(TArguments arguments, ContextPredicate<TArguments, object> predicate)
        {
            return TryGet(arguments, predicate, out object value) ? value : throw new ContextValueNotFoundByArgumentsException(arguments);
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
            if (type == null) throw new ArgumentNullException(nameof(type));

            return OnTryGet(type, out value);
        }

        public bool TryGet<TArguments, T>(TArguments arguments, ContextPredicate<TArguments, T> predicate, out T value)
        {
            if (typeof(TArguments).IsClass && EqualityComparer<TArguments>.Default.Equals(arguments, default)) throw new ArgumentNullException(nameof(arguments));
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

            return OnTryGet(arguments, predicate, out value);
        }

        public bool TryGet<TArguments>(TArguments arguments, ContextPredicate<TArguments, object> predicate, out object value)
        {
            if (typeof(TArguments).IsClass && EqualityComparer<TArguments>.Default.Equals(arguments, default)) throw new ArgumentNullException(nameof(arguments));
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

            return OnTryGet(arguments, predicate, out value);
        }

        public IEnumerator GetEnumerator()
        {
            return OnGetEnumerator();
        }

        protected abstract void OnAdd(object value);
        protected abstract bool OnRemove(object value);
        protected abstract void OnClear();
        protected abstract bool OnTryGet(Type type, out object value);
        protected abstract bool OnTryGet<TArguments, T>(TArguments arguments, ContextPredicate<TArguments, T> predicate, out T value);
        protected abstract bool OnTryGet<TArguments>(TArguments arguments, ContextPredicate<TArguments, object> predicate, out object value);
        protected abstract IEnumerator OnGetEnumerator();
    }
}
