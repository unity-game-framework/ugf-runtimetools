using System;
using System.Collections;

namespace UGF.RuntimeTools.Runtime.Contexts
{
    public interface IContext : IEnumerable
    {
        event ContextValueHandler Added;
        event ContextValueHandler Removed;
        event ContextHandler Cleared;

        void Add(object value);
        bool Remove(object value);
        void Clear();
        T Get<T>() where T : class;
        object Get(Type type);
        T Get<TArguments, T>(TArguments arguments, ContextPredicate<TArguments, T> predicate);
        object Get<TArguments>(TArguments arguments, ContextPredicate<TArguments, object> predicate);
        bool TryGet<T>(out T value) where T : class;
        bool TryGet(Type type, out object value);
        bool TryGet<TArguments, T>(TArguments arguments, ContextPredicate<TArguments, T> predicate, out T value);
        bool TryGet<TArguments>(TArguments arguments, ContextPredicate<TArguments, object> predicate, out object value);
    }
}
