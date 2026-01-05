using System;
using System.Collections.Generic;
using System.Text;

namespace Utils.Extensions
{
    /// <summary>
    /// Extension methods for Dictionary operations.
    /// </summary>
    public static class DictionaryExtensions
    {
        /// <summary>
        /// Adds or updates a key-value pair in the dictionary.
        /// </summary>
        /// <typeparam name="TKey">The type of keys in the dictionary.</typeparam>
        /// <typeparam name="TValue">The type of values in the dictionary.</typeparam>
        /// <param name="dict">The dictionary to modify.</param>
        /// <param name="key">The key to add or update.</param>
        /// <param name="value">The value to associate with the key.</param>
        public static void SafeAdd<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, TValue value)
        {
            if (dict.ContainsKey(key))
            {
                dict[key] = value;
            }
            else
            {
                dict.Add(key, value);
            }
        }
    }
}
