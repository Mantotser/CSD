using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CSD.Web.ViewModels
{
    public class DriverStats
    {
        public string LicensePlate { get; set; }
        public double TotalHours { get; set; }
        public int TotalKM { get; set; }
        public int TotalPenalties { get; set; }
        public int TotalAcidents { get; set; }
        public int TotalTrips { get; set; }

        public double TotalPenaltiesPerKm
        {
            get
            {
                return (double)TotalPenalties / TotalKM;
            }
        }

        public double TotalPenaltiesPerPeriod
        {
            get
            {
                return TotalPenalties / TotalHours;
            }
        }

        public double TotalPenaltiesPerTrip
        {
            get
            {
                return (double)TotalPenalties / TotalTrips;
            }
        }
    }
}