using System;
using System.Collections.Generic;
using System.Linq;
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

        public List<IFixture> ExtractMatchBettingData(XDocument dataFeed)
        {
            var bookieData = _dataFetcher.GetUkFootballMatchResultFeed();

            List<IFixture> fixtures = new List<IFixture>();
            var leagueNodes = getLeagueNodes(bookieData);

            foreach (var leagueNode in leagueNodes)
            {
                var leagueName = leagueNode.Attribute("name").Value;
                var leagueFixtures = getFixturesForBetType(leagueNode, _matchBettingNodeAttributePostFixString);

                foreach (var fixture in leagueFixtures)
                {
                    var match = fixture.Attribute("name").ToString().Split('"')[1].Split('-')[0].Trim();
                    var stringSeparator = new string[] { " v " };
                    var homeTeam = match.Split(stringSeparator, StringSplitOptions.None)[0].Trim();
                    var awayTeam = match.Split(stringSeparator, StringSplitOptions.None)[1].Trim();

                    var date = getDateOrTimeFromXmlAttribute(fixture, "date");
                    var time = getDateOrTimeFromXmlAttribute(fixture, "time");

                    var williamHillFixture = new WilliamHillFixture();
                    williamHillFixture.League = leagueName;
                    williamHillFixture.MatchDateTime = DateTime.Parse(date + " " + time);
                    williamHillFixture.HomeTeam = homeTeam;
                    williamHillFixture.AwayTeam = awayTeam;

                    foreach (var oddsNode in fixture.Elements("participant"))
                    {
                        var prediction = oddsNode.Attribute("name").Value;
                        var fractionalOdds = oddsNode.Attribute("odds").Value;
                        var decimalOdds = oddsNode.Attribute("oddsDecimal").Value;

                        var williamHillFixtureOdds = new WilliamHillFixtureOdds();
                        williamHillFixtureOdds.Prediction = prediction;
                        williamHillFixtureOdds.FractionalOdds = fractionalOdds;
                        williamHillFixtureOdds.DecimalOdds = decimalOdds;

                        if (williamHillFixtureOdds.Prediction == williamHillFixture.HomeTeam)
                        {
                            williamHillFixture.HomeOdds = williamHillFixtureOdds;
                        }
                        else if (williamHillFixtureOdds.Prediction == williamHillFixture.AwayTeam)
                        {
                            williamHillFixture.AwayOdds = williamHillFixtureOdds;
                        }
                        else
                        {
                            williamHillFixture.DrawOdds = williamHillFixtureOdds;
                        }
                    }

                    fixtures.Add(williamHillFixture);
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

        private string getDateOrTimeFromXmlAttribute(XElement fixtureElement, string attribute)
        {
            var dateOrTime = fixtureElement.Attribute(attribute).ToString().Split('"')[1];

            return dateOrTime;
        }
    }
}
