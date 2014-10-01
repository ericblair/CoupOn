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
                var fixtureId = InsertOrUpdateFixture(fixture);

                InsertOrUpdateFixtureOdds(fixtureId, fixture.HomeOdds);
                InsertOrUpdateFixtureOdds(fixtureId, fixture.AwayOdds);
                InsertOrUpdateFixtureOdds(fixtureId, fixture.DrawOdds);
            }

            _context.SaveChanges();
        }

        public int InsertOrUpdateFixture(IFixture fixture)
        {
            var fixtureRecord = new WilliamHillFixture
            {
                League = fixture.League,
                MatchDateTime = fixture.MatchDateTime,
                HomeTeam = fixture.HomeTeam,
                AwayTeam = fixture.AwayTeam
            };

            _context.WilliamHillFixtures.Add(fixtureRecord);
            _context.SaveChanges();
            _context.Entry(fixtureRecord).GetDatabaseValues();

            return fixtureRecord.Id;
        }

        public void InsertOrUpdateFixtureOdds(int fixtureId, IFixtureOdds fixtureOdds)
        {
            var odds = new WilliamHillFixtureOdds
            {
                Prediction = fixtureOdds.Prediction,
                FractionalOdds = fixtureOdds.FractionalOdds,
                DecimalOdds = fixtureOdds.DecimalOdds,
                FixtureId = fixtureId,
                Fixture = _context.WilliamHillFixtures.First(x => x.Id == fixtureId)
            };

            _context.WilliamHillFixtureOdds.Add(odds);
        }
    }
}
