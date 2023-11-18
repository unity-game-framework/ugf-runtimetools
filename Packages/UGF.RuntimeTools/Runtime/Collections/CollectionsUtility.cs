using System;
using System.Collections;

namespace UGF.RuntimeTools.Runtime.Collections
{
    public static class CollectionsUtility
    {
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
