using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YetAnotherEngine.Utils.Helpers
{
    class SortedListHelper
    {
        private static SortedListHelper _instance;
        public static SortedListHelper Instance => _instance ?? (_instance = new SortedListHelper());

        private SortedListHelper()
        {
        }

        public int RemoveAllFromSortedList<TKey, TValue>(SortedList<TKey, TValue> sortedList,
            Predicate<KeyValuePair<TKey, TValue>> match)
        {
            // The number of items removed.
            int itemsRemoved = 0;
            SortedList<TKey, TValue> itemsToBeRemoved = new SortedList<TKey, TValue>();
            // Cycle through each of the values in the sorted list.
            foreach (KeyValuePair<TKey, TValue> pair in sortedList)
            {
                // If the predicate returns true, then remove the item.
                if (match(pair))
                {
                    // Remove the item.
                    itemsToBeRemoved.Add(pair.Key, pair.Value);

                    // Increment the number of items removed.
                    itemsRemoved++;
                }
            }

            foreach (KeyValuePair<TKey, TValue> pair in itemsToBeRemoved)
            {
                sortedList.Remove(pair.Key);
            }


            // Return the items removed.
            return itemsRemoved;
        }
    }
}