﻿using System;
using System.Collections;
using System.Collections.Generic;

namespace UGF.RuntimeTools.Runtime.Collections
{
    public static class CollectionsUtility
    {
        public static bool TryGet<TValue, TArgument, TCollection>(TCollection collection, TArgument argument, CollectionPredicate<TValue, TArgument> predicate, CollectionIndexer<TValue, TCollection> indexer, int count, out TValue result)
        {
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));
            if (indexer == null) throw new ArgumentNullException(nameof(indexer));

            for (int i = 0; i < count; i++)
            {
                TValue element = indexer(collection, i);

                if (predicate(element, argument))
                {
                    result = element;
                    return true;
                }
            }

            result = default;
            return false;
        }

        public static bool TryGet<TValue, TArgument>(IEnumerable<TValue> collection, TArgument argument, CollectionPredicate<TValue, TArgument> predicate, out TValue result)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));

            return TryGet(collection.GetEnumerator(), argument, predicate, out result);
        }

        public static bool TryGet<TValue, TArgument, TEnumerator>(TEnumerator enumerator, TArgument argument, CollectionPredicate<TValue, TArgument> predicate, out TValue result) where TEnumerator : IEnumerator<TValue>
        {
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

            while (enumerator.MoveNext())
            {
                TValue element = enumerator.Current;

                if (predicate(element, argument))
                {
                    result = element;
                    return true;
                }
            }

            result = default;
            return false;
        }

        public static int GetCount(IEnumerable enumerable)
        {
            if (enumerable == null) throw new ArgumentNullException(nameof(enumerable));

            if (enumerable is ICollection collection)
            {
                return collection.Count;
            }

            int count = 0;
            IEnumerator enumerator = enumerable.GetEnumerator();

            checked
            {
                while (enumerator.MoveNext())
                {
                    count++;
                }
            }

            return count;
        }
    }
}
