using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CoupON.Data;

namespace CoupON.Data.Entities
{
    public class CoupONContext : DbContext
    {
        public virtual DbSet<WilliamHillFixture> WilliamHillFixtures { get; set; }
        public virtual DbSet<WilliamHillFixtureOdds> WilliamHillFixtureOdds { get; set; } 
    }
}
