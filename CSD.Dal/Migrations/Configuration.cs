namespace CSD.Dal.Migrations
{
    using CSD.Dal.Entity;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Data.Entity.Validation;
    using System.Linq;

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
                        records.Add(new StatsPerDay(y, random.Next(25), random.Next(25), random.Next(1000), random.Next(2)));
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
