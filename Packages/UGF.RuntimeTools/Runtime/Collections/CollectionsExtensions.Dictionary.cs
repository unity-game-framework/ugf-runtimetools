﻿using System;
using System.Collections;
using System.Collections.Generic;

namespace UGF.RuntimeTools.Runtime.Collections
{
    public static partial class CollectionsExtensions
    {
        public static TKey Get<TKey, TValue, TArgument>(this Dictionary<TKey, TValue>.KeyCollection collection, TArgument argument, CollectionPredicate<TKey, TArgument> predicate)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));

            return CollectionsUtility.TryGet(collection.GetEnumerator(), argument, predicate, out TKey result) ? result : throw new CollectionValueNotFoundByArgumentException(argument);
        }

        public static bool TryGet<TKey, TValue, TArgument>(this Dictionary<TKey, TValue>.KeyCollection collection, TArgument argument, CollectionPredicate<TKey, TArgument> predicate, out TKey result)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));

            return CollectionsUtility.TryGet(collection.GetEnumerator(), argument, predicate, out result);
        }

        public static TValue Get<TKey, TValue, TArgument>(this Dictionary<TKey, TValue>.ValueCollection collection, TArgument argument, CollectionPredicate<TValue, TArgument> predicate)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));

            return CollectionsUtility.TryGet(collection.GetEnumerator(), argument, predicate, out TValue result) ? result : throw new CollectionValueNotFoundByArgumentException(argument);
        }

        public static bool TryGet<TKey, TValue, TArgument>(this Dictionary<TKey, TValue>.ValueCollection collection, TArgument argument, CollectionPredicate<TValue, TArgument> predicate, out TValue result)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));

            return CollectionsUtility.TryGet(collection.GetEnumerator(), argument, predicate, out result);
        }

        public static TValue Get<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> collection, TKey key)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));

            return collection.TryGetValue(key, out TValue value) ? value : throw new CollectionValueNotFoundByKeyException(key);
        }

        public static bool TryGet<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> collection, TKey key, out TValue result)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));

            return collection.TryGetValue(key, out result);
        }

        public static TValue Get<TKey, TValue>(this IDictionary<TKey, TValue> collection, TKey key)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));

            return collection.TryGetValue(key, out TValue value) ? value : throw new CollectionValueNotFoundByKeyException(key);
        }

        public static bool TryGet<TKey, TValue>(this IDictionary<TKey, TValue> collection, TKey key, out TValue result)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));

            return collection.TryGetValue(key, out result);
        }

        public static object Get(this IDictionary collection, object key)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            if (key == null) throw new ArgumentNullException(nameof(key));

            return collection[key] ?? throw new CollectionValueNotFoundByKeyException(key);
        }

        public static bool TryGet(this IDictionary collection, object key, out object result)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            if (key == null) throw new ArgumentNullException(nameof(key));

            result = collection[key];
            return result != null;
        }
    }
}
