using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForEvolve.Blog.Samples.NinjaApi.Models
{
    public class NinjaEntity : TableEntity
    {
        public string Name { get; set; }
        public int Level { get; set; }
    }
}
