using System;
using System.Xml;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

using CoupON.DataParsers;
using CoupON.Interfaces.DataFetchers;

namespace CoupON.DataParsers.Tests
{
    [TestClass]
    public class WilliamHillDataParserTests
    {
        [TestMethod]
        public void SingleLeagueNodeContainsSingleFixture_ExtractsCorrectDetails()
        {
            // Arrange
            var mockFetcher = new Mock<IWilliamHillDataFetcher>();
            var testData = XDocument.Load(@"C:\CoupOn_Source\TestData\WH_MatchBetting_SingleFixture.xml");
            mockFetcher.Setup(x => x.GetUkFootballMatchResultFeed()).Returns(testData);

            var parser = new WilliamHillDataParser(mockFetcher.Object);

            // Act
            var result = parser.ExtractMatchBettingData();

            // Assert
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("English Premier League", result[0].League);
            Assert.AreEqual(DateTime.Parse("2014-09-27" + " " + "12:45:00"), result[0].MatchDateTime);
            Assert.AreEqual("Liverpool", result[0].HomeTeam);
            Assert.AreEqual("Everton", result[0].AwayTeam);

            Assert.AreEqual("Liverpool", result[0].HomeOdds.Prediction);
            Assert.AreEqual("4/5", result[0].HomeOdds.FractionalOdds);
            Assert.AreEqual("1.80", result[0].HomeOdds.DecimalOdds);

            Assert.AreEqual("Everton", result[0].AwayOdds.Prediction);
            Assert.AreEqual("16/5", result[0].AwayOdds.FractionalOdds);
            Assert.AreEqual("4.20", result[0].AwayOdds.DecimalOdds);

            Assert.AreEqual("Draw", result[0].DrawOdds.Prediction);
            Assert.AreEqual("11/4", result[0].DrawOdds.FractionalOdds);
            Assert.AreEqual("3.75", result[0].DrawOdds.DecimalOdds);
        }
    }
}
