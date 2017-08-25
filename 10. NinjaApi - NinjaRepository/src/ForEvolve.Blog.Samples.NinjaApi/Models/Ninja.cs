using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForEvolve.Blog.Samples.NinjaApi.Models
{
    public class Ninja
    {
        public string Key { get; set; }
        public Clan Clan { get; set; }
        public string Name { get; set; }
        public int Level { get; set; }
    }
}
