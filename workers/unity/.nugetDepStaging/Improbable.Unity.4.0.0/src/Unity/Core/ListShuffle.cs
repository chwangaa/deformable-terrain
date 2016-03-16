using System;
using System.Collections.Generic;
using System.Linq;

namespace Improbable.Unity.Core
{
    static class ListShuffle
    {
        private static readonly Random Rng = new Random();

        /// <summary>
        /// Creates a new list, with the elements of the provided list in random order
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">any list of items</param>
        /// <returns>new shuffled list of items</returns>
        public static IList<T> Shuffled<T>(this IList<T> list)
        {
            var output = list.ToList();

            var index = output.Count;
            while (index > 1)
            {
                index--;

                // pick somewhere random
                var newIndex = Rng.Next(index + 1);

                // save value in newIndex
                var value = output[newIndex];

                // perform the swap
                output[newIndex] = output[index];
                output[index] = value;
            }

            return output;
        }
    }
}
