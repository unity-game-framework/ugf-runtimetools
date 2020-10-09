using System.Collections.Generic;

namespace UGF.RuntimeTools.Runtime.Collections
{
    public static class CollectionsExtensions
    {
        public static TValue Get<TValue, TArgument>(this IEnumerable<TValue> collection, TArgument argument, CollectionPredicate<TValue, TArgument> predicate)
        {
            return CollectionsUtility.TryGet(collection, argument, predicate, out TValue result) ? result : throw new CollectionValueNotFoundByArgumentException(argument);
        }

        public static bool TryGet<TValue, TArgument>(this IEnumerable<TValue> collection, TArgument argument, CollectionPredicate<TValue, TArgument> predicate, out TValue result)
        {
            return CollectionsUtility.TryGet(collection, argument, predicate, out result);
        }

        public static TValue Get<TValue, TArgument>(this IReadOnlyList<TValue> collection, TArgument argument, CollectionPredicate<TValue, TArgument> predicate)
        {
            return CollectionsUtility.TryGet(collection, argument, predicate, out TValue result) ? result : throw new CollectionValueNotFoundByArgumentException(argument);
        }

        public static bool TryGet<TValue, TArgument>(this IReadOnlyList<TValue> collection, TArgument argument, CollectionPredicate<TValue, TArgument> predicate, out TValue result)
        {
            return CollectionsUtility.TryGet(collection, argument, predicate, out result);
        }

        public static TValue Get<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> collection, TKey key)
        {
            return collection.TryGetValue(key, out TValue value) ? value : throw new CollectionValueNotFoundByKeyException(key);
        }

        public static bool TryGet<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> collection, TKey key, out TValue result)
        {
            return collection.TryGetValue(key, out result);
        }

        public static TValue Get<TValue, TArgument>(this HashSet<TValue> collection, TArgument argument, CollectionPredicate<TValue, TArgument> predicate)
        {
            return CollectionsUtility.TryGet(collection, argument, predicate, out TValue result) ? result : throw new CollectionValueNotFoundByArgumentException(argument);
        }

        public static bool TryGet<TValue, TArgument>(this HashSet<TValue> collection, TArgument argument, CollectionPredicate<TValue, TArgument> predicate, out TValue result)
        {
            return CollectionsUtility.TryGet(collection, argument, predicate, out result);
        }
    }
}
