using System.Collections.Generic;
using UnityEngine;

namespace System.Linq
{
    public static partial class ExtensionLinq
    {
        public static Canvas Max(this IEnumerable<Canvas> self)
        {
            Canvas result = default;

            if (self.Count() > 0)
            {
                int maxSortingOrder = self.First().sortingOrder;

                foreach (var item in self)
                {
                    if (maxSortingOrder < item.sortingOrder)
                    {
                        result = item;

                        maxSortingOrder = item.sortingOrder;
                    }
                }
            }
            return result;
        }
        public static Canvas Min(this IEnumerable<Canvas> self)
        {
            Canvas result = default;

            if (self.Count() > 0)
            {
                int minSortingOrder = self.First().sortingOrder;

                foreach (var item in self)
                {
                    if (minSortingOrder > item.sortingOrder)
                    {
                        result = item;

                        minSortingOrder = item.sortingOrder;
                    }
                }
            }
            return result;
        }
    }
}
