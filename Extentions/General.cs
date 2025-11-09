using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YellowMacaroni.Discord.Extentions
{
    public static partial class Extentions
    {
        public static string ToJson(this object o, bool indented = false)
        {
            return JsonConvert.SerializeObject(o, indented ? Formatting.Indented : Formatting.None);
        }

        public static T? ToObject<T>(this string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }

        public static string Join<T>(this IEnumerable<T> list, string seperator = ", ")
        {
            return string.Join(seperator, list);
        }
        public static string Join<T>(this IEnumerable<T> list, string seperator, string lastSeperator)
        {
            if (!list.Any()) return "";
            if (list.Count() == 1) return list.First()?.ToString() ?? "";

            string Value = "";
            int i = 0;
            foreach (T item in list)
            {
                i++;
                if (i == list.Count() - 1) Value += $"{item}{lastSeperator}";
                else if (i == list.Count()) Value += $"{item}";
                else Value += $"{item}{seperator}";
            }
            return Value;
        }


        public static byte ToByte(this object o)
        {
            bool Success = byte.TryParse(o.ToString(), out byte result);
            return Success ? result : (byte)0;
        }
        public static int ToInt(this object o)
        {
            bool Success = int.TryParse(o.ToString(), out int result);
            return Success ? result : 0;
        }
        public static double ToDouble(this object o)
        {
            bool Success = double.TryParse(o.ToString(), out double result);
            return Success ? result : 0d;
        }
        public static float ToFloat(this object o)
        {
            bool Success = float.TryParse(o.ToString(), out float result);
            return Success ? result : 0f;
        }

        public static string ToFirstUpper(this string str)
        {
            if (str.Length == 0) return str;
            List<char> chars = [.. str];
            chars[0] = char.ToUpper(chars[0]);
            return chars.Join("");
        }

        public static List<T> ToList<T>(this T e) where T : Enum
        {
            List<T> Values = [];
            foreach (T t in Enum.GetValues(typeof(T))) if (e.HasFlag(t)) Values.Add(t);
            return Values;
        }

        public static bool ContainsRange<T>(this IEnumerable<T> a, IEnumerable<T> b)
        {
            foreach (T c in b) if (a.Contains(c)) return true;
            return false;
        }

        public static IEnumerable<R> ForAll<T, R>(this IEnumerable<T> list, Func<T, R> method)
        {
            List<R> Values = [];
            foreach (T t in list) Values.Add(method(t));
            return Values;
        }

        public static Dictionary<K,V> ToDictionary<K, V>(this IEnumerable<V> list, Func<V, K> keySelector) where K: notnull
        {
            Dictionary<K, V> dict = [];
            foreach (V v in list)
            {
                K key = keySelector(v);
                dict.TryAdd(key, v);
            }
            return dict;
        }

        public static Dictionary<K, V> ToDictionary<K ,V>(this IEnumerable<KeyValuePair<K, V>> list) where K: notnull
        {
            Dictionary<K, V> dict = [];
            foreach (KeyValuePair<K, V> kvp in list)
            {
                dict.TryAdd(kvp.Key, kvp.Value);
            }
            return dict;
        }

        public static T WaitFor<T>(this Task<T> task)
        {
            task.Wait();
            return task.Result;
        }
    }
}
