using System;
using System.Collections.Generic;

namespace UGF.RuntimeTools.Runtime.Collections
{
    public static partial class CollectionsExtensions
    {
        public static TValue Get<TValue, TArgument>(this HashSet<TValue> collection, TArgument argument, CollectionPredicate<TValue, TArgument> predicate)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));

            return CollectionsUtility.TryGet(collection.GetEnumerator(), argument, predicate, out TValue result) ? result : throw new CollectionValueNotFoundByArgumentException(argument);
        }

        public static bool TryGet<TValue, TArgument>(this HashSet<TValue> collection, TArgument argument, CollectionPredicate<TValue, TArgument> predicate, out TValue result)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));

            return CollectionsUtility.TryGet(collection.GetEnumerator(), argument, predicate, out result);
        }

        public static TValue Get<TValue, TArgument>(this Stack<TValue> collection, TArgument argument, CollectionPredicate<TValue, TArgument> predicate)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));

            return CollectionsUtility.TryGet(collection.GetEnumerator(), argument, predicate, out TValue result) ? result : throw new CollectionValueNotFoundByArgumentException(argument);
        }

        public static bool TryGet<TValue, TArgument>(this Stack<TValue> collection, TArgument argument, CollectionPredicate<TValue, TArgument> predicate, out TValue result)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));

            return CollectionsUtility.TryGet(collection.GetEnumerator(), argument, predicate, out result);
        }

        public static TValue Get<TValue, TArgument>(this Queue<TValue> collection, TArgument argument, CollectionPredicate<TValue, TArgument> predicate)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));

            return CollectionsUtility.TryGet(collection.GetEnumerator(), argument, predicate, out TValue result) ? result : throw new CollectionValueNotFoundByArgumentException(argument);
        }

        public static bool TryGet<TValue, TArgument>(this Queue<TValue> collection, TArgument argument, CollectionPredicate<TValue, TArgument> predicate, out TValue result)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));

            return CollectionsUtility.TryGet(collection.GetEnumerator(), argument, predicate, out result);
        }

        public static TValue Get<TValue, TArgument>(this LinkedList<TValue> collection, TArgument argument, CollectionPredicate<TValue, TArgument> predicate)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));

            return CollectionsUtility.TryGet(collection.GetEnumerator(), argument, predicate, out TValue result) ? result : throw new CollectionValueNotFoundByArgumentException(argument);
        }

        public static bool TryGet<TValue, TArgument>(this LinkedList<TValue> collection, TArgument argument, CollectionPredicate<TValue, TArgument> predicate, out TValue result)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));

            return CollectionsUtility.TryGet(collection.GetEnumerator(), argument, predicate, out result);
        }
    }
}
