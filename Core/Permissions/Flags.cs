using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using YellowMacaroni.Discord.Extentions;

namespace YellowMacaroni.Discord.Core
{
    /// <summary>
    /// For when an flags enum goes over the integer limit.
    /// </summary>
    /// <example>
    /// <code>
    /// [Flags]
    /// public enum MyFlags
    /// {
    ///     Item_1 = 1 &lt;&lt; 1,
    ///     Item_2 = 1 &lt;&lt; 2
    /// }
    /// 
    /// // Becomes
    /// 
    /// public enum MyFlagBits
    /// {
    ///     Item_1 = 1,
    ///     Item_2 = 2
    /// }
    /// 
    /// public static class MyClass
    /// {
    ///     public static Flags MyFlags = new MyFlags&lt;MyFlagBits&gt;();
    /// }
    /// </code>
    /// </example>
    /// <typeparam name="T">The enum to use for the flags where the values of each flag is its represented bit.</typeparam>
    public class Flags<T> where T : Enum
    {
        private readonly Dictionary<int, T> _values;

        public int Count => _values.Count;

        public Flags(T items)
        {
            _values = [];
            foreach (T item in items.ToList()) _values.TryAdd(Convert.ToInt32(item), item);
        }
        public Flags(Array items)
        {
            _values = [];
            foreach (T item in items) _values.TryAdd(Convert.ToInt32(item), item);
        }
        public Flags(IEnumerable<T> items)
        {
            _values = [];
            foreach (T item in items) _values.TryAdd(Convert.ToInt32(item), item);
        }
        public Flags(long value)
        {
            _values = [];
            string Binary = Convert.ToString(value, 2);
            Binary = Binary.Reverse().Join("");
            foreach (T item in Enum.GetValues(typeof(T)))
            {
                int thisindex = Convert.ToInt32(item);
                if (Binary.Length > thisindex) if (Binary[thisindex] == '1') _values.Add(thisindex, item);
            }
        }
        public Flags(string value)
        {
            _values = [];

            if (BigInteger.TryParse(value, out BigInteger bigInt))
            {
                foreach (T item in Enum.GetValues(typeof(T)))
                {
                    int bitPosition = Convert.ToInt32(item);

                    if ((bigInt & (BigInteger.One << bitPosition)) != BigInteger.Zero)
                    {
                        _values.Add(bitPosition, item);
                    }
                }
            }
        }
        public Flags(BigInteger value)
        {
            _values = [];

            foreach (T item in Enum.GetValues(typeof(T)))
            {
                int bitPosition = Convert.ToInt32(item);

                if ((value & (BigInteger.One << bitPosition)) != BigInteger.Zero)
                {
                    _values.Add(bitPosition, item);
                }
            }
        }

        public bool HasBit(int bit) => _values.ContainsKey(bit);

        public bool HasFlag(T item) => _values.ContainsKey(Convert.ToInt32(item));
        public bool HasFlag(T[] item)
        {
            foreach (T i in item) if (_values.ContainsKey(Convert.ToInt32(i))) return true;
            return false;
        }

        public static explicit operator long(Flags<T> Flag)
        {
            return Flag.Count;
        }
        public static explicit operator BigInteger(Flags<T> flags)
        {
            BigInteger result = BigInteger.Zero;

            foreach (var kvp in flags._values)
            {
                result |= BigInteger.One << kvp.Key;
            }

            return result;
        }

        public static explicit operator Flags<T>(long Value) => new(Value);
        public static explicit operator Flags<T>(BigInteger value) => new(value);
        public static implicit operator Flags<T>(string value) => new(value);
    }
}
