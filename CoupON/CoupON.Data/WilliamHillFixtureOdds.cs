using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CoupON.Interfaces.Data;

namespace CoupON.Data
{
    public class WilliamHillFixtureOdds : IFixtureOdds
    {
        public string Prediction { get; set; }
        public string FractionalOdds { get; set; }
        public string DecimalOdds { get; set; }
    }
}
