using System;
using System.Collections.Generic;

namespace Assets.Scripts.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="IEnumerable{T}" />.
    /// </summary>
    public static class ExArray
    {
        private static Random _random = new Random();

        /// <summary>
        /// Shuffle the array.
        /// </summary>
        /// <typeparam name="T">Array element type.</typeparam>
        /// <param name="array">Array to shuffle.</param>
        public static void Shuffle<T>(this T[] array)
        {
            if (array is null)
                throw new ArgumentNullException(nameof(array));

            int n = array.Length;
            for (int i = 0; i < (n - 1); i++)
            {
                int r = i + _random.Next(n - i);
                T t = array[r];
                array[r] = array[i];
                array[i] = t;
            }
        }
    }
}
