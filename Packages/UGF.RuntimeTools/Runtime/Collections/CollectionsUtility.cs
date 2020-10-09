using System;
using System.Collections.Generic;

namespace UGF.RuntimeTools.Runtime.Collections
{
    public static class CollectionsUtility
    {
        public static bool TryGet<TValue, TArgument>(IEnumerable<TValue> collection, TArgument argument, CollectionPredicate<TValue, TArgument> predicate, out TValue result)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

            foreach (TValue element in collection)
            {
                if (predicate(element, argument))
                {
                    result = element;
                    return true;
                }
            }

            result = default;
            return false;
        }

        public static bool TryGet<TValue, TArgument>(IReadOnlyList<TValue> collection, TArgument argument, CollectionPredicate<TValue, TArgument> predicate, out TValue result)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

            for (int i = 0; i < collection.Count; i++)
            {
                TValue element = collection[i];

                if (predicate(element, argument))
                {
                    result = element;
                    return true;
                }
            }

            result = default;
            return false;
        }

        public static bool TryGet<TKey, TValue, TArgument>(Dictionary<TKey, TValue> collection, TArgument argument, CollectionPredicate<TKey, TArgument> predicate, out TKey result)
        {
            foreach (KeyValuePair<TKey, TValue> pair in collection)
            {
                if (predicate(pair.Key, argument))
                {
                    result = pair.Key;
                    return true;
                }
            }

            result = default;
            return false;
        }

        public static bool TryGet<TKey, TValue, TArgument>(Dictionary<TKey, TValue> collection, TArgument argument, CollectionPredicate<TValue, TArgument> predicate, out TValue result)
        {
            foreach (KeyValuePair<TKey, TValue> pair in collection)
            {
                if (predicate(pair.Value, argument))
                {
                    result = pair.Value;
                    return true;
                }
            }

            result = default;
            return false;
        }

        public static bool TryGet<TValue, TArgument>(HashSet<TValue> collection, TArgument argument, CollectionPredicate<TValue, TArgument> predicate, out TValue result)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

            foreach (TValue element in collection)
            {
                if (predicate(element, argument))
                {
                    result = element;
                    return true;
                }
            }

            result = default;
            return false;
        }

        public static bool TryGet<TValue, TArgument>(Stack<TValue> collection, TArgument argument, CollectionPredicate<TValue, TArgument> predicate, out TValue result)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

            foreach (TValue element in collection)
            {
                if (predicate(element, argument))
                {
                    result = element;
                    return true;
                }
            }

            result = default;
            return false;
        }

        public static bool TryGet<TValue, TArgument>(Queue<TValue> collection, TArgument argument, CollectionPredicate<TValue, TArgument> predicate, out TValue result)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

            foreach (TValue element in collection)
            {
                if (predicate(element, argument))
                {
                    result = element;
                    return true;
                }
            }

            result = default;
            return false;
        }
    }
}
