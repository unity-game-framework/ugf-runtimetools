using System;
using System.Collections.Generic;

namespace UGF.RuntimeTools.Runtime.Objects
{
    public interface IObjectRelativesCollection
    {
        IReadOnlyCollection<object> Objects { get; }

        void Add(object target);
        bool Remove(object target);
        void Clear();
        T Get<TArguments, T>(TArguments arguments, ObjectRelativePredicate<TArguments, T> predicate) where T : class;
        bool TryGet<TArguments, T>(TArguments arguments, ObjectRelativePredicate<TArguments, T> predicate, out T target) where T : class;
        T Get<T>() where T : class;
        object Get(Type targetType);
        bool TryGet<T>(out T target) where T : class;
        bool TryGet(Type targetType, out object target);
    }
}
