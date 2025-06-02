using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YellowMacaroni.Discord.Core
{
    public class Color(byte r, byte g, byte b)
    {
        public byte r = r;
        public byte g = g;
        public byte b = b;

        public int ToInt()
        {
            return (r << 16) + (g << 8) + b;
        }

        public string ToHex()
        {
            return $"#{r:X2}{g:X2}{b:X2}";
        }

        public override string ToString() => ToHex();

        public static explicit operator int(Color color) => color.ToInt();
        public static explicit operator Color(int color)
        {
            byte r = (byte)((color >> 16) & 0xFF);
            byte g = (byte)((color >> 8) & 0xFF);
            byte b = (byte)(color & 0xFF);
            return new Color(r, g, b);
        }
    }
}
