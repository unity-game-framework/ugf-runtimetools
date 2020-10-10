using System.Collections.Generic;

namespace UGF.RuntimeTools.Runtime.Collections
{
    public static partial class CollectionsExtensions
    {
        public static TValue Get<TValue, TArgument>(this IEnumerable<TValue> collection, TArgument argument, CollectionPredicate<TValue, TArgument> predicate)
        {
            return CollectionsUtility.TryGet(collection, argument, predicate, out TValue result) ? result : throw new CollectionValueNotFoundByArgumentException(argument);
        }

        public static bool TryGet<TValue, TArgument>(this IEnumerable<TValue> collection, TArgument argument, CollectionPredicate<TValue, TArgument> predicate, out TValue result)
        {
            return CollectionsUtility.TryGet(collection, argument, predicate, out result);
        }
    }
}
