using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YellowMacaroni.Discord.Core
{
    public class Reaction
    {
        public int? count;
        public CountDetails? count_details;
        public bool? me;
        public bool? me_burst;
        public Emoji? emoji;
        public List<string>? burst_colors;
    }

    public class CountDetails
    {
        public int burst;
        public int normal;
    }
}
