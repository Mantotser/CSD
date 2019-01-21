using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSD.Dal.Entity
{
    public class StatsPerDay
    {
        public int Id { get; set; }
        public double DriveTimePerDay { get; set; }
        public int PenaltyCountPerDay { get; set; }
        public int KilometersPerDay { get; set; }
        public int AccidentsPerDay { get; set; }
        public int DayCount { get; set; }
        public int DrivingTrips { get; set; }

        // Foreign Key
        public int DriverId { get; set; }

        // Navigation Property
        public Driver Driver { get; set; }


        public StatsPerDay(int dayCount, int driveTimePerDay, int penaltyCountPerDay, int kilometersPerDay, int accidentsPerDay, int drivingTrips)
        {
            DayCount = dayCount;
            DriveTimePerDay = driveTimePerDay;
            PenaltyCountPerDay = penaltyCountPerDay;
            KilometersPerDay = kilometersPerDay;
            AccidentsPerDay = accidentsPerDay;
            DrivingTrips = drivingTrips;
        }

        public StatsPerDay()
        {

        }
    }
}
