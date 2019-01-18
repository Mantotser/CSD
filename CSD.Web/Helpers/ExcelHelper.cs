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
                worksheet.Cells[0, 3].PutValue("Total accidents per day");
                worksheet.Cells[0, 4].PutValue("Total penalties per day");

                var driverCount = 1;
                foreach (var driver in drivers)
                {
                    worksheet.Cells[driverCount, 0].PutValue(driver.LicensePlate);
                    worksheet.Cells[driverCount, 1].PutValue(driver.StatsPerDays.SingleOrDefault(s => s.DayCount == y).KilometersPerDay);
                    worksheet.Cells[driverCount, 2].PutValue(driver.StatsPerDays.SingleOrDefault(s => s.DayCount == y).DriveTimePerDay);
                    worksheet.Cells[driverCount, 3].PutValue(driver.StatsPerDays.SingleOrDefault(s => s.DayCount == y).AccidentsPerDay);
                    worksheet.Cells[driverCount, 4].PutValue(driver.StatsPerDays.SingleOrDefault(s => s.DayCount == y).PenaltyCountPerDay);
                    driverCount++;
                }
                worksheet.AutoFitColumn(0);
                worksheet.AutoFitColumn(1);
                worksheet.AutoFitColumn(2);
                worksheet.AutoFitColumn(3);
                worksheet.AutoFitColumn(4);
            }
            return workbook;
        }

        public static Workbook GetExcelTotalData(List<Driver> drivers, Workbook workbook)
        {
            var totalWorkBook = new Workbook();
            totalWorkBook.Worksheets.Add();
            var driverStatsList = new List<DriverStats>();

            foreach (var driver in drivers)
            {
                var totalKm = 0;
                var totalHours = 0;
                var totalAccidents = 0;
                var totalPenalties = 0;
                foreach (var worksheet in workbook.Worksheets)
                {
                    Cells cells = worksheet.Cells;
                    FindOptions findOptions = new FindOptions
                    {
                        CaseSensitive = false,
                        LookInType = LookInType.Values
                    };
                    Cell foundCell = cells.Find(driver.LicensePlate, null, findOptions);

                    if (foundCell != null)
                    {
                        int row = foundCell.Row;
                        string name = foundCell.Name;
                        totalKm += (int)worksheet.Cells[row, 1].Value;
                        totalHours += (int)worksheet.Cells[row, 2].Value;
                        totalAccidents += (int)worksheet.Cells[row, 3].Value;
                        totalPenalties += (int)worksheet.Cells[row, 4].Value;
                    }
                }
                driverStatsList.Add(new DriverStats
                {
                    LicensePlate = driver.LicensePlate,
                    TotalAcidents = totalAccidents,
                    TotalHours = totalHours,
                    TotalKM = totalKm,
                    TotalPenalties = totalPenalties
                });
            }

            Worksheet workSheet = totalWorkBook.Worksheets[0];
            var driverCount = 0;
            workSheet.Cells[0, 0].PutValue("Driver License Plate");
            workSheet.Cells[0, 1].PutValue("Total kilometers per period");
            workSheet.Cells[0, 2].PutValue("Total drive hours per period");
            workSheet.Cells[0, 3].PutValue("Total accidents per period");
            workSheet.Cells[0, 4].PutValue("Total penalties per period");
            foreach (var driverStat in driverStatsList)
            {
                driverCount += 1;
                workSheet.Cells[driverCount, 0].PutValue(driverStat.LicensePlate);
                workSheet.Cells[driverCount, 1].PutValue(driverStat.TotalKM);
                workSheet.Cells[driverCount, 2].PutValue(driverStat.TotalHours);
                workSheet.Cells[driverCount, 3].PutValue(driverStat.TotalAcidents);
                workSheet.Cells[driverCount, 4].PutValue(driverStat.TotalPenalties);
            }
            workSheet.AutoFitColumn(0);
            workSheet.AutoFitColumn(1);
            workSheet.AutoFitColumn(2);
            workSheet.AutoFitColumn(3);
            workSheet.AutoFitColumn(4);

            return totalWorkBook;
        }
    }
}