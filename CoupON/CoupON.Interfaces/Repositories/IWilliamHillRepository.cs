using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CoupON.Interfaces.Data;

namespace CoupON.Interfaces.Repositories
{
    public interface IWilliamHillRepository
    {
        void InsertOrUpdateFixtures(List<IFixture> fixtures);
    }
}
