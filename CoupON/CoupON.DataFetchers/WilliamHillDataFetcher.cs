using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

using CoupON.Interfaces.DataFetchers;

namespace CoupON.DataFetchers
{
    public class WilliamHillDataFetcher : IWilliamHillDataFetcher
    {
        // TODO: Move this string to config file
        private const string ukFootballMatchResultFeedUrl =
            "http://cachepricefeeds.williamhill.com/openbet_cdn?action=template&template=getHierarchyByMarketType&classId=1&marketSort=MR&filterBIR=N";

        public XDocument GetUkFootballMatchResultFeed()
        {
            var data = XDocument.Load(ukFootballMatchResultFeedUrl);

            return data;
        }
    }
}
