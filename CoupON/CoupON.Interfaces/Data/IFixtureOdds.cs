using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoupON.Interfaces.Data
{
    public interface IFixtureOdds
    {
        string Prediction { get; set; }
        string FractionalOdds { get; set; }
        string DecimalOdds { get; set; }

        IFixture Fixture { get; set; }
    }
}
