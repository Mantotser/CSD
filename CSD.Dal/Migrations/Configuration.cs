namespace CSD.Dal.Migrations
{
    using CSD.Dal.Entity;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Data.Entity.Validation;
    using MathNet.Numerics.Distributions;
    using MathNet.Numerics.Random;
    using System.Linq;
    using MathNet.Numerics;

    internal sealed class Configuration : DbMigrationsConfiguration<CSD.Dal.DataConnection.CSDContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }


        private static readonly Random random = new Random();
        private static readonly object syncLock = new object();
        public static int RandomNumber(int min, int max)
        {
            lock (syncLock)
            { // synchronize
                return random.Next(min, max);
            }
        }

        private string LicensePlateGenerator()
        {
            lock (syncLock)
            { // synchronize
                return random.NextDouble().ToString("0.00000000").Substring(2, 8);
            }
        }


        protected override void Seed(CSD.Dal.DataConnection.CSDContext context)
        {
            try
            {
                for (int i = 1; i <= 100; i++)
                {
                    context.Drivers.AddOrUpdate(new Driver(LicensePlateGenerator()));
                }
                context.SaveChanges();

                var drivers = context.Drivers;
                foreach (var driver in drivers)
                {
                    var records = new List<StatsPerDay>();
                    for (int y = 1; y <= 365; y++)
                    {
                        // TODO: fix medians and deviations
                        //var ranDriveTime = new Normal(308, 100);
                        //var ranDrivePenalty = new Normal(0.1, 0.1);
                        //var ranDriveKm = new Normal(18488, 10000);
                        //var ranDriveAccidents = new Normal(1/30000, 1/30000);
                        //var ranDriveTrips = new Normal(818, 300);
                        //records.Add(
                        //    new StatsPerDay(y, 
                        //    ranDriveTime.RandomSource.Next(), 
                        //    ranDrivePenalty.RandomSource.Next(),
                        //    ranDriveKm.RandomSource.Next(),
                        //    ranDriveAccidents.RandomSource.Next(),
                        //    ranDriveTrips.RandomSource.Next()
                        //    ));
                        var ranDriveTime = Math.Abs((int)Generate.Normal(1, 308 / 365, 1.2).First());
                        var ranDrivePenalty = Math.Abs((int)Generate.Normal(1, 0.1, 0.6).First());
                        var ranDriveKm = Math.Abs((int)Generate.Normal(1, 18488 / 365, 10000 / 365).First());
                        var ranDriveAccidents = Math.Abs((int)Generate.Normal(1, 1 / 30000, 0.3).First());
                        var ranDriveTrips = Math.Abs((int)Generate.Normal(1, 818 / 365, 3).First());
                        records.Add(new StatsPerDay(y, ranDriveTime, ranDrivePenalty, ranDriveKm, ranDriveAccidents, ranDriveTrips));
                    }
                    driver.StatsPerDays = records;
                }
                context.SaveChanges();

            }
            catch (DbEntityValidationException exception)
            {
                foreach (var error in exception.EntityValidationErrors)
                {
                    foreach (var errorEntry in error.ValidationErrors)
                    {
                        Console.Error.WriteLine($"{error.Entry.Entity}: {errorEntry.PropertyName}: {errorEntry.ErrorMessage}");
                    }
                }

            }
        }
    }
}
