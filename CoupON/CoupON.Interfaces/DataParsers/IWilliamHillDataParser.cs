using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

using CoupON.Interfaces.Data;
using CoupON.Interfaces.DataParsers;

namespace CoupON.Interfaces.DataParsers
{
    public interface IWilliamHillDataParser
    {
        List<IFixture> ExtractMatchBettingData(XDocument dataFeed);
    }
}
