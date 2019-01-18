using CSD.Dal.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSD.Dal.DataConnection
{
    public class CSDContext :DbContext
    {
        public CSDContext() : base("CSDConnection")
        {

        }

        public DbSet<Driver> Drivers { get; set; }
        public DbSet<StatsPerDay> StatsPerDay { get; set; }
    }
}
