using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CoupON.Interfaces.Data;

namespace CoupON.Data
{
    public class WilliamHillFixtureOdds : IFixtureOdds
    {
        [Key]
        public int Id { get; set; }
        public string Prediction { get; set; }
        public string FractionalOdds { get; set; }
        public string DecimalOdds { get; set; }

        public int FixtureId { get; set; }
        public virtual IFixture Fixture { get; set; }
    }
}
