using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSD.Dal.Entity
{
    public class Driver
    {
        public int Id { get; set; }
        public string LicensePlate { get; set; }

        // Navigation
        public ICollection<StatsPerDay> StatsPerDays { get; set; }

        public Driver(string licensePlate)
        {
            LicensePlate = licensePlate;
        }
        public Driver()
        {

        }
    }
}
