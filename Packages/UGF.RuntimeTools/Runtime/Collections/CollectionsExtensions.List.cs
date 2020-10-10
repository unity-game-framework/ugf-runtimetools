using System;
using System.Collections;
using System.Collections.Generic;

namespace UGF.RuntimeTools.Runtime.Collections
{
    public static partial class CollectionsExtensions
    {
        public static TValue Get<TValue, TArgument>(this List<TValue> collection, TArgument argument, CollectionPredicate<TValue, TArgument> predicate)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));

            return CollectionsUtility.TryGet(collection, argument, predicate, (list, index) => list[index], collection.Count, out TValue result) ? result : throw new CollectionValueNotFoundByArgumentException(argument);
        }

        public static bool TryGet<TValue, TArgument>(this List<TValue> collection, TArgument argument, CollectionPredicate<TValue, TArgument> predicate, out TValue result)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));

            return CollectionsUtility.TryGet(collection, argument, predicate, (list, index) => list[index], collection.Count, out result);
        }

        public static TValue Get<TValue, TArgument>(this IReadOnlyList<TValue> collection, TArgument argument, CollectionPredicate<TValue, TArgument> predicate)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));

            return CollectionsUtility.TryGet(collection, argument, predicate, (list, index) => list[index], collection.Count, out TValue result) ? result : throw new CollectionValueNotFoundByArgumentException(argument);
        }

        public static bool TryGet<TValue, TArgument>(this IReadOnlyList<TValue> collection, TArgument argument, CollectionPredicate<TValue, TArgument> predicate, out TValue result)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));

            return CollectionsUtility.TryGet(collection, argument, predicate, (list, index) => list[index], collection.Count, out result);
        }

        public static object Get<TArgument>(this IList collection, TArgument argument, CollectionPredicate<object, TArgument> predicate)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));

            return CollectionsUtility.TryGet(collection, argument, predicate, (list, index) => list[index], collection.Count, out object result) ? result : throw new CollectionValueNotFoundByArgumentException(argument);
        }

        public static bool TryGet<TArgument>(this IList collection, TArgument argument, CollectionPredicate<object, TArgument> predicate, out object result)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));

            return CollectionsUtility.TryGet(collection, argument, predicate, (list, index) => list[index], collection.Count, out result);
        }

        public static TValue Get<TValue, TArgument>(this IList<TValue> collection, TArgument argument, CollectionPredicate<TValue, TArgument> predicate)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));

            return CollectionsUtility.TryGet(collection, argument, predicate, (list, index) => list[index], collection.Count, out TValue result) ? result : throw new CollectionValueNotFoundByArgumentException(argument);
        }

        public static bool TryGet<TValue, TArgument>(this IList<TValue> collection, TArgument argument, CollectionPredicate<TValue, TArgument> predicate, out TValue result)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));

            return CollectionsUtility.TryGet(collection, argument, predicate, (list, index) => list[index], collection.Count, out result);
        }
    }
}
