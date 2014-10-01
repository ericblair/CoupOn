using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

using CoupON.Data;
using CoupON.Interfaces.Data;
using CoupON.Interfaces.DataFetchers;
using CoupON.Interfaces.DataParsers;

namespace CoupON.DataParsers
{
    public class WilliamHillDataParser : IWilliamHillDataParser
    {
        private readonly IWilliamHillDataFetcher _dataFetcher;

        public WilliamHillDataParser(IWilliamHillDataFetcher dataFetcher)
        {
            _dataFetcher = dataFetcher;
        }

        // String used to identify match betting odds from others like first scorer etc
        private const string _matchBettingNodeAttributePostFixString = "- Match Betting";

        public List<IFixture> ExtractMatchBettingData()
        {
            var bookieData = _dataFetcher.GetUkFootballMatchResultFeed();

            List<IFixture> fixtures = new List<IFixture>();
            var leagueNodes = getLeagueNodes(bookieData);

            foreach (var leagueNode in leagueNodes)
            {
                var leagueName = leagueNode.Attribute("name").Value;
                var leagueFixtures = getFixturesForBetType(leagueNode, _matchBettingNodeAttributePostFixString);

                foreach (var fixtureNode in leagueFixtures)
                {
                    var fixtureData = extractFixtureData(fixtureNode, leagueName);

                    fixtures.Add(fixtureData);
                }
            }

            return fixtures;
        }

        private IEnumerable<XElement> getLeagueNodes(XDocument bookieData)
        {
            var leagueNodes = bookieData.Descendants("type");

            return leagueNodes;
        }

        private IEnumerable<XElement> getFixturesForBetType(XElement leagueNode, string betType)
        {
            var fixtures = from fixture in leagueNode.Descendants()
                           where fixture.Attribute("name").Value.Contains(betType)
                           select fixture;

            return fixtures;
        }

        private WilliamHillFixture extractFixtureData(XElement fixtureNode, string leagueName)
        {
            var williamHillFixture = new WilliamHillFixture();
            williamHillFixture.League = leagueName;
            williamHillFixture.MatchDateTime = extractMatchDateTime(fixtureNode);
            williamHillFixture.HomeTeam = extractTeamName(fixtureNode, "Home");
            williamHillFixture.AwayTeam = extractTeamName(fixtureNode, "Away");

            williamHillFixture = extractFixtureOdds(fixtureNode, williamHillFixture);

            return williamHillFixture;
        }

        private WilliamHillFixture extractFixtureOdds(XElement fixtureNode, WilliamHillFixture fixtureData)
        {
            foreach (var oddsNode in fixtureNode.Elements("participant"))
            {
                var williamHillFixtureOdds = new WilliamHillFixtureOdds();
                williamHillFixtureOdds.Prediction = extractPrediction(oddsNode);
                williamHillFixtureOdds.FractionalOdds = extractOdds(oddsNode, "Fractional");
                williamHillFixtureOdds.DecimalOdds = extractOdds(oddsNode, "Decimal");

                if (williamHillFixtureOdds.Prediction == fixtureData.HomeTeam)
                {
                    fixtureData.HomeOdds = williamHillFixtureOdds;
                }
                else if (williamHillFixtureOdds.Prediction == fixtureData.AwayTeam)
                {
                    fixtureData.AwayOdds = williamHillFixtureOdds;
                }
                else
                {
                    fixtureData.DrawOdds = williamHillFixtureOdds;
                }
            }

            return fixtureData;
        }

        private DateTime extractMatchDateTime(XElement fixtureNode)
        {
            var date = getDateOrTimeFromXmlAttribute(fixtureNode, "date");
            var time = getDateOrTimeFromXmlAttribute(fixtureNode, "time");

            return DateTime.Parse(date + " " + time);
        }

        private string getDateOrTimeFromXmlAttribute(XElement fixtureElement, string attribute)
        {
            var dateOrTime = fixtureElement.Attribute(attribute).ToString().Split('"')[1];

            return dateOrTime;
        }

        private string extractTeamName(XElement fixtureNode, string homeOrAway)
        {
            var matchAttribute = fixtureNode.Attribute("name").Value;
            var matchAttDecoded = WebUtility.HtmlDecode(matchAttribute);
            var match = matchAttDecoded.Split('-')[0].Trim();

            var stringSeparator = new string[] { " v " };

            string teamName = null;

            if (homeOrAway == "Home")
            {
                teamName = match.Split(stringSeparator, StringSplitOptions.None)[0].Trim();
            }
            else if (homeOrAway == "Away")
            {
                teamName = match.Split(stringSeparator, StringSplitOptions.None)[1].Trim();
            }

            return teamName;
        }

        private string extractPrediction(XElement oddsNode)
        {
            var prediction = WebUtility.HtmlDecode(oddsNode.Attribute("name").Value);

            return prediction;
        }

        private string extractOdds(XElement oddsNode, string fractionalOrDecimal)
        {
            string odds = null;

            if (fractionalOrDecimal == "Fractional")
            {
                odds = oddsNode.Attribute("odds").Value;
            }
            else if (fractionalOrDecimal == "Decimal")
            {
                odds = oddsNode.Attribute("oddsDecimal").Value;
            }

            return odds;
        }
    }
}
