using System;
using System.Collections.Generic;
using UGF.RuntimeTools.Runtime.Providers;

namespace UGF.RuntimeTools.Runtime.Objects
{
    public class ObjectRelativesProvider<TObject> : Provider<TObject, ObjectRelativeCollection<TObject>>, IObjectRelativeProvider where TObject : class
    {
        IReadOnlyDictionary<object, IObjectRelativesCollection> IProvider<object, IObjectRelativesCollection>.Entries { get { return m_entries; } }

        event ProviderEntryHandler<object, IObjectRelativesCollection> IProvider<object, IObjectRelativesCollection>.Added { add { Added += value; } remove { Added -= value; } }
        event ProviderEntryHandler<object, IObjectRelativesCollection> IProvider<object, IObjectRelativesCollection>.Removed { add { Removed += value; } remove { Removed -= value; } }

        private readonly Dictionary<object, IObjectRelativesCollection> m_entries = new Dictionary<object, IObjectRelativesCollection>();

        protected override void OnAdd(TObject id, ObjectRelativeCollection<TObject> entry)
        {
            base.OnAdd(id, entry);

            m_entries.Add(id, entry);
        }

        protected override bool OnRemove(TObject id, ObjectRelativeCollection<TObject> entry)
        {
            m_entries.Remove(id);

            return base.OnRemove(id, entry);
        }

        protected override void OnClear()
        {
            base.OnClear();

            m_entries.Clear();
        }

        public void Connect(TObject first, TObject second)
        {
            if (first == null) throw new ArgumentNullException(nameof(first));
            if (second == null) throw new ArgumentNullException(nameof(second));

            AddChild(first, second);
            AddChild(second, first);
        }

        public bool Disconnect(TObject first, TObject second)
        {
            if (first == null) throw new ArgumentNullException(nameof(first));
            if (second == null) throw new ArgumentNullException(nameof(second));

            return RemoveChild(first, second) || RemoveChild(second, first);
        }

        public void AddChild(TObject parent, TObject child)
        {
            if (parent == null) throw new ArgumentNullException(nameof(parent));
            if (child == null) throw new ArgumentNullException(nameof(child));

            if (!TryGet(parent, out ObjectRelativeCollection<TObject> collection))
            {
                collection = new ObjectRelativeCollection<TObject>();

                Add(parent, collection);
            }

            collection.Add(child);
        }

        public bool RemoveChild(TObject parent, TObject child)
        {
            if (parent == null) throw new ArgumentNullException(nameof(parent));
            if (child == null) throw new ArgumentNullException(nameof(child));

            if (TryGet(parent, out ObjectRelativeCollection<TObject> collection) && collection.Remove(child))
            {
                if (collection.Objects.Count == 0)
                {
                    Remove(parent);
                }

                return true;
            }

            return false;
        }

        void IObjectRelativeProvider.Connect(object first, object second)
        {
            Connect((TObject)first, (TObject)second);
        }

        bool IObjectRelativeProvider.Disconnect(object first, object second)
        {
            return Disconnect((TObject)first, (TObject)second);
        }

        void IObjectRelativeProvider.AddChild(object parent, object child)
        {
            AddChild((TObject)parent, (TObject)child);
        }

        bool IObjectRelativeProvider.RemoveChild(object parent, object child)
        {
            return RemoveChild((TObject)parent, (TObject)child);
        }

        void IProvider<object, IObjectRelativesCollection>.Add(object id, IObjectRelativesCollection entry)
        {
            Add((TObject)id, (ObjectRelativeCollection<TObject>)entry);
        }

        bool IProvider<object, IObjectRelativesCollection>.Remove(object id)
        {
            return Remove((TObject)id);
        }

        T IProvider<object, IObjectRelativesCollection>.Get<T>(object id)
        {
            return (T)(object)Get((TObject)id);
        }

        IObjectRelativesCollection IProvider<object, IObjectRelativesCollection>.Get(object id)
        {
            return Get((TObject)id);
        }

        bool IProvider<object, IObjectRelativesCollection>.TryGet<T>(object id, out T entry)
        {
            if (TryGet((TObject)id, out ObjectRelativeCollection<TObject> collection))
            {
                entry = (T)(object)collection;
                return true;
            }

            entry = default;
            return false;
        }

        bool IProvider<object, IObjectRelativesCollection>.TryGet(object id, out IObjectRelativesCollection entry)
        {
            if (TryGet((TObject)id, out ObjectRelativeCollection<TObject> collection))
            {
                entry = collection;
                return true;
            }

            entry = default;
            return false;
        }
    }
}
