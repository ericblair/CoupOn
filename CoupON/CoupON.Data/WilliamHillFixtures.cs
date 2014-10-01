using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CoupON.Interfaces.Data;

namespace CoupON.Data
{
    public class WilliamHillFixture : IFixture
    {
        [Key]
        public int Id { get; set; }
        public string League { get; set; }
        public DateTime MatchDateTime { get; set; }
        public string HomeTeam { get; set; }
        public string AwayTeam { get; set; }

        public IFixtureOdds HomeOdds { get; set; }
        public IFixtureOdds AwayOdds { get; set; }
        public IFixtureOdds DrawOdds { get; set; }
    }
}
