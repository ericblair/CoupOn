using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CoupON.Data;
using CoupON.Data.Entities;
using CoupON.Interfaces.Data;
using CoupON.Interfaces.Repositories;

namespace CoupON.Repository
{
    public class WilliamHillRepository : IWilliamHillRepository
    {
        CoupONContext _context;

        public WilliamHillRepository(CoupONContext context)
        {
            _context = context;
        }

        public void InsertOrUpdateFixtures(List<IFixture> fixtures)
        {
            foreach (var fixture in fixtures)
            {
                var fixtureRecord = InsertOrUpdateFixture(fixture);

                fixtureRecord.HomeOdds = InsertOrUpdateFixtureOdds(fixtureRecord, fixture.HomeOdds);
                fixtureRecord.AwayOdds = InsertOrUpdateFixtureOdds(fixtureRecord, fixture.AwayOdds);
                fixtureRecord.DrawOdds = InsertOrUpdateFixtureOdds(fixtureRecord, fixture.DrawOdds);

                _context.WilliamHillFixtures.Add(fixtureRecord);
            }

            _context.SaveChanges();
        }

        public WilliamHillFixture InsertOrUpdateFixture(IFixture fixture)
        {
            var fixtureRecord = new WilliamHillFixture
            {
                League = fixture.League,
                MatchDateTime = fixture.MatchDateTime,
                HomeTeam = fixture.HomeTeam,
                AwayTeam = fixture.AwayTeam
            };

            return fixtureRecord;
        }

        public WilliamHillFixtureOdds InsertOrUpdateFixtureOdds(IFixture fixture, IFixtureOdds fixtureOdds)
        {
            var odds = new WilliamHillFixtureOdds
            {
                Prediction = fixtureOdds.Prediction,
                FractionalOdds = fixtureOdds.FractionalOdds,
                DecimalOdds = fixtureOdds.DecimalOdds,
                Fixture = fixture
            };

            //_context.WilliamHillFixtureOdds.Add(odds);
            return odds;
        }
    }
}
