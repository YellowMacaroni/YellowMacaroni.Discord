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

        public string Hex
        {
            get
            {
                return $"{r:X2}{g:X2}{b:X2}";
            }
            set
            { 
                if (value.StartsWith('#')) value = value.Substring(1);
                if (value.Length == 6)
                {
                    r = Convert.ToByte(value[..2], 16);
                    g = Convert.ToByte(value.Substring(2, 2), 16);
                    b = Convert.ToByte(value.Substring(4, 2), 16);
                }
                else if (value.Length == 3)
                {
                    r = Convert.ToByte(value[0].ToString() + value[0].ToString(), 16);
                    g = Convert.ToByte(value[1].ToString() + value[1].ToString(), 16);
                    b = Convert.ToByte(value[2].ToString() + value[2].ToString(), 16);
                }
                else
                {
                    throw new ArgumentException("Invalid hex format.");
                }
            }
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
