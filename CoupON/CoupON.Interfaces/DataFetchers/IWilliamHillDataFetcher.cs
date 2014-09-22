using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CoupON.Interfaces.DataFetchers
{
    public interface IWilliamHillDataFetcher
    {
        XDocument GetUkFootballMatchResultFeed();
    }
}
