using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoupON.Interfaces.Data
{
    public interface IFixture
    {
        int Id { get; set; }
        string League { get; set; }
        DateTime MatchDateTime { get; set; }
        string HomeTeam { get; set; }
        string AwayTeam { get; set; }

        IFixtureOdds HomeOdds { get; set; }
        IFixtureOdds AwayOdds { get; set; }
        IFixtureOdds DrawOdds { get; set; }
    }
}
