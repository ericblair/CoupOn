using System;
using System.Collections.Generic;
using System.Data.Entity;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

using CoupON.Data;
using CoupON.Data.Entities;
using CoupON.DataFetchers;
using CoupON.DataParsers;
using CoupON.Interfaces.Data;
using CoupON.Repository;

namespace CoupON.Repositories.Tests
{
    [TestClass]
    public class WilliamHillRepositoryTests
    {
        //[TestMethod]
        public void InsertOrUpdateFixtures_SingleFixture_DetailsSaved()
        {
            var homeTeam = "Chelsea";
            var awayTeam = "Arsenal";

            var testFixture = new WilliamHillFixture
            {
                League = "English Premier League",
                MatchDateTime = new DateTime(2014, 1, 1, 12, 15, 30),
                HomeTeam = homeTeam,
                AwayTeam = awayTeam,

                HomeOdds = new WilliamHillFixtureOdds
                {
                    Prediction = homeTeam,
                    FractionalOdds = "2/1",
                    DecimalOdds = "3.00"
                },

                AwayOdds = new WilliamHillFixtureOdds
                {
                    Prediction = awayTeam,
                    FractionalOdds = "3/1",
                    DecimalOdds = "4.00"
                },

                DrawOdds = new WilliamHillFixtureOdds
                {
                    Prediction = "Draw",
                    FractionalOdds = "5/1",
                    DecimalOdds = "6.00"
                }
            };

            var testFixtures = new List<IFixture>();
            testFixtures.Add(testFixture);

            var mockContext = new Mock<CoupONContext>();
            //mockContext.Setup(x => x.WilliamHillFixtures.Add(testFixture));
            //mockContext.Setup(x => x.Entry(testFixture).GetDatabaseValues());
            mockContext.Setup(x => x.WilliamHillFixtures.Add(It.IsAny<WilliamHillFixture>()));
            mockContext.Setup(x => x.WilliamHillFixtureOdds.Add(It.IsAny<WilliamHillFixtureOdds>()));

            var testRepo = new WilliamHillRepository(mockContext.Object);

            // Test
            testRepo.InsertOrUpdateFixtures(testFixtures);

            mockContext.Verify(x => x.WilliamHillFixtures.Add(testFixture), Times.Once);
        }

        [TestMethod]
        public void IntegrationTest()
        {
            var dataFetcher = new WilliamHillDataFetcher();

            var dataParser = new WilliamHillDataParser(dataFetcher);

            var fixtures = dataParser.ExtractMatchBettingData();

            var context = new CoupONContext();
            var repository = new WilliamHillRepository(context);
            repository.InsertOrUpdateFixtures(fixtures);
        }
    }
}
