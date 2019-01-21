using Aspose.Cells;
using CSD.Dal.Entity;
using CSD.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CSD.Web.Helpers
{
    public static class ExcelHelper
    {
        public static Workbook GetExcelGeneratedData(List<Driver> drivers)
        {
            Workbook workbook = new Workbook();

            for (int y = 1; y <= 365; y++)
            {
                workbook.Worksheets.Add();
                workbook.Worksheets[y].Name = $"Day {y}";
                Worksheet worksheet = workbook.Worksheets[y];
                worksheet.Cells[0, 0].PutValue("Driver License Plate");
                worksheet.Cells[0, 1].PutValue("Total kilometers per day");
                worksheet.Cells[0, 2].PutValue("Total drive hours per Day");
                worksheet.Cells[0, 3].PutValue("Total trips per day");
                worksheet.Cells[0, 4].PutValue("Total accidents per day");
                worksheet.Cells[0, 5].PutValue("Total penalties per day");

                var driverCount = 1;
                foreach (var driver in drivers)
                {
                    worksheet.Cells[driverCount, 0].PutValue(driver.LicensePlate);
                    worksheet.Cells[driverCount, 1].PutValue(driver.StatsPerDays.SingleOrDefault(s => s.DayCount == y).KilometersPerDay);
                    worksheet.Cells[driverCount, 2].PutValue(driver.StatsPerDays.SingleOrDefault(s => s.DayCount == y).DriveTimePerDay);
                    worksheet.Cells[driverCount, 3].PutValue(driver.StatsPerDays.SingleOrDefault(s => s.DayCount == y).DrivingTrips);
                    worksheet.Cells[driverCount, 4].PutValue(driver.StatsPerDays.SingleOrDefault(s => s.DayCount == y).AccidentsPerDay);
                    worksheet.Cells[driverCount, 5].PutValue(driver.StatsPerDays.SingleOrDefault(s => s.DayCount == y).PenaltyCountPerDay);
                    driverCount++;
                }
                AutoFitColumns(worksheet);
            }
            return workbook;
        }

        public static Workbook GetExcelTotalData(List<DriverStats> driverStatsList)
        {
            var totalWorkBook = new Workbook();
            totalWorkBook.Worksheets.Add();
            Worksheet workSheet = totalWorkBook.Worksheets[0];
            var driverCount = 0;
            workSheet.Cells[0, 0].PutValue("Driver License Plate");
            workSheet.Cells[0, 1].PutValue("Total kilometers per period");
            workSheet.Cells[0, 2].PutValue("Total drive hours per period");
            workSheet.Cells[0, 3].PutValue("Total trips per period");
            workSheet.Cells[0, 4].PutValue("Total accidents per period");
            workSheet.Cells[0, 5].PutValue("Total penalties per period");
            foreach (var driverStat in driverStatsList)
            {
                driverCount += 1;
                workSheet.Cells[driverCount, 0].PutValue(driverStat.LicensePlate);
                workSheet.Cells[driverCount, 1].PutValue(driverStat.TotalKM);
                workSheet.Cells[driverCount, 2].PutValue(driverStat.TotalHours);
                workSheet.Cells[driverCount, 3].PutValue(driverStat.TotalTrips);
                workSheet.Cells[driverCount, 4].PutValue(driverStat.TotalAcidents);
                workSheet.Cells[driverCount, 5].PutValue(driverStat.TotalPenalties);
            }
            AutoFitColumns(workSheet);
            return totalWorkBook;
        }

        private static void AutoFitColumns(Worksheet worksheet)
        {
            for (int i = 0; i <= 6; i++)
            {
                worksheet.AutoFitColumn(i);
            }
        }

        public static List<DriverStats> GetTotalDriverStats(List<Driver> drivers)
        {
            var driverStatsList = new List<DriverStats>();

            foreach (var driver in drivers)
            {
                driverStatsList.Add(
                    new DriverStats
                    {
                        LicensePlate = driver.LicensePlate,
                        TotalKM = driver.StatsPerDays.Sum(s => s.KilometersPerDay),
                        TotalPenalties = driver.StatsPerDays.Sum(s => s.PenaltyCountPerDay),
                        TotalHours = driver.StatsPerDays.Sum(s => s.DriveTimePerDay),
                        TotalAcidents = driver.StatsPerDays.Sum(s => s.AccidentsPerDay),
                        TotalTrips = driver.StatsPerDays.Sum(s => s.DrivingTrips)
                    }
                );
            }
            return driverStatsList;
        }

        public static Workbook GetExcelTenWorstPerKm(List<DriverStats> driverStatsList)
        {
            var totalWorkBook = new Workbook();
            totalWorkBook.Worksheets.Add();
            Worksheet workSheet = totalWorkBook.Worksheets[0];
            var driverCount = 0;
            workSheet.Cells[0, 0].PutValue("Driver License Plate");
            workSheet.Cells[0, 1].PutValue("Total kilometers per period");
            workSheet.Cells[0, 2].PutValue("Total drive hours per period");
            workSheet.Cells[0, 3].PutValue("Total accidents per period");
            workSheet.Cells[0, 4].PutValue("Total penalties per period");
            workSheet.Cells[0, 5].PutValue("Ratio of (Total Penalties)/(Total Km)");
            foreach (var driverStat in driverStatsList)
            {
                driverCount += 1;
                workSheet.Cells[driverCount, 0].PutValue(driverStat.LicensePlate);
                workSheet.Cells[driverCount, 1].PutValue(driverStat.TotalKM);
                workSheet.Cells[driverCount, 2].PutValue(driverStat.TotalHours);
                workSheet.Cells[driverCount, 3].PutValue(driverStat.TotalAcidents);
                workSheet.Cells[driverCount, 4].PutValue(driverStat.TotalPenalties);
                workSheet.Cells[driverCount, 5].PutValue(driverStat.TotalPenaltiesPerKm);
            }
            AutoFitColumns(workSheet);
            return totalWorkBook;
        }

        public static Workbook GetExcelTenWorst(List<DriverStats> driverStatsList, string perWhatTitle, TypeOfCalculationEnum typeOfCalculationEnum)
        {
            var totalWorkBook = new Workbook();
            Worksheet workSheet = totalWorkBook.Worksheets[0];
            var driverCount = 0;
            workSheet.Cells[0, 0].PutValue("Driver License Plate");
            workSheet.Cells[0, 1].PutValue("Total kilometers per period");
            workSheet.Cells[0, 2].PutValue("Total drive hours per period");
            workSheet.Cells[0, 3].PutValue("Total trips per period");
            workSheet.Cells[0, 4].PutValue("Total accidents per period");
            workSheet.Cells[0, 5].PutValue("Total penalties per period");
            workSheet.Cells[0, 6].PutValue(perWhatTitle);
            foreach (var driverStat in driverStatsList)
            {
                driverCount += 1;
                workSheet.Cells[driverCount, 0].PutValue(driverStat.LicensePlate);
                workSheet.Cells[driverCount, 1].PutValue(driverStat.TotalKM);
                workSheet.Cells[driverCount, 2].PutValue(driverStat.TotalHours);
                workSheet.Cells[driverCount, 3].PutValue(driverStat.TotalTrips);
                workSheet.Cells[driverCount, 4].PutValue(driverStat.TotalAcidents);
                workSheet.Cells[driverCount, 5].PutValue(driverStat.TotalPenalties);

                switch (typeOfCalculationEnum)
                {
                    case TypeOfCalculationEnum.PerHr:
                        workSheet.Cells[driverCount, 6].PutValue(driverStat.TotalPenaltiesPerPeriod);
                        break;
                    case TypeOfCalculationEnum.PerKm:
                        workSheet.Cells[driverCount, 6].PutValue(driverStat.TotalPenaltiesPerKm);
                        break;
                    case TypeOfCalculationEnum.PerTrip:
                        workSheet.Cells[driverCount, 6].PutValue(driverStat.TotalPenaltiesPerTrip);
                        break;
                    default:
                        break;
                }
            }
            AutoFitColumns(workSheet);
            return totalWorkBook;
        }

    }
}