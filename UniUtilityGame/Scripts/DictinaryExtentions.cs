using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public static class DictinaryExtentions
    {
        //------------------------------------------------------------------------------------------------------------------------------------------
        // GetOrDefault
        public static TValue GetOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key)
        {
            return dict.TryGetValue(key, out TValue result) ? result : default(TValue);
        }

        public static TValue GetOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, TValue def)
        {
            return dict.TryGetValue(key, out TValue result) ? result : def;
        }

        //------------------------------------------------------------------------------------------------------------------------------------------
        // TryAddNew

        public static bool TryAddNew<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key) where TValue : new()
        {
            return dict.TryAdd(key, _ => new TValue());
        }

        public static bool TryAddDefault<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key)
        {
            return dict.TryAdd(key, default(TValue));
        }

        public static bool TryAdd<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, TValue addValue)
        {
            bool canAdd = !dict.ContainsKey(key);

            if (canAdd)
                dict.Add(key, addValue);

            return canAdd;
        }

        public static bool TryAdd<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, Func<TKey, TValue> addValueFactory)
        {
            bool canAdd = !dict.ContainsKey(key);

            if (canAdd)
                dict.Add(key, addValueFactory(key));

            return canAdd;
        }

        //------------------------------------------------------------------------------------------------------------------------------------------
        // GetOrAdd

        public static TValue GetOrAddNew<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key) where TValue : new()
        {
            dict.TryAddNew(key);
            return dict[key];
        }

        public static TValue GetOrAddDefault<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key)
        {
            dict.TryAddDefault(key);
            return dict[key];
        }

        public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, TValue addValue)
        {
            dict.TryAdd(key, addValue);
            return dict[key];
        }

        public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, Func<TKey, TValue> valueFactory)
        {
            dict.TryAdd(key, valueFactory);
            return dict[key];
        }

        //------------------------------------------------------------------------------------------------------------------------------------------

        public static void Add<TKey, TValue>(this IDictionary<TKey, TValue> source, KeyValuePair<TKey, TValue> addPair)
        {
            source.Add(addPair.Key, addPair.Value);
        }

        //------------------------------------------------------------------------------------------------------------------------------------------

    }
}

