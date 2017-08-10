using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace SwappableImplementation
{
    public class LightContext : DbContext
    {
        public LightContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Light> Lights { get; set; }
    }
}
