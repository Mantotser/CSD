using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CSD.Web.ViewModels
{
    public class DriverStats
    {
        public string LicensePlate { get; set; }
        public int TotalHours { get; set; }
        public int TotalKM { get; set; }
        public int TotalPenalties { get; set; }
        public int TotalAcidents { get; set; }

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
                return (double)TotalPenalties / TotalHours;
            }
        }
    }
}