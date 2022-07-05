using System;
using System.Collections;

namespace UGF.RuntimeTools.Runtime.Containers
{
    public interface IContainer : IEnumerable
    {
        int Count { get; }

        T Get<T>() where T : class;
        object Get(Type type);
        bool TryGet<T>(out T value) where T : class;
        bool TryGet(Type type, out object value);
    }
}
