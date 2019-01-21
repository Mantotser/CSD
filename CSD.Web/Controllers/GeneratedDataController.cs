using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Aspose.Cells;
using CSD.Dal.DataConnection;
using CSD.Dal.Entity;
using CSD.Web.Helpers;
using CSD.Web.ViewModels;

namespace CSD.Web.Controllers
{
    public class GeneratedDataController : Controller
    {
        private CSDContext db = new CSDContext();

        // GET: GeneratedData
        public ActionResult Index()
        {
            return View(db.Drivers.ToList());
        }

        // GET: GeneratedData/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Driver driver = db.Drivers.Find(id);
            if (driver == null)
            {
                return HttpNotFound();
            }

            driver.StatsPerDays = db.StatsPerDay.Where(w => w.DriverId == driver.Id).ToList();


            return View(driver);
        }

        // GET: GeneratedData/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: GeneratedData/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,LicensePlate")] Driver driver)
        {
            if (ModelState.IsValid)
            {
                db.Drivers.Add(driver);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(driver);
        }

        // GET: GeneratedData/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Driver driver = db.Drivers.Find(id);
            if (driver == null)
            {
                return HttpNotFound();
            }
            return View(driver);
        }

        // POST: GeneratedData/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,LicensePlate")] Driver driver)
        {
            if (ModelState.IsValid)
            {
                db.Entry(driver).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(driver);
        }

        // GET: GeneratedData/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Driver driver = db.Drivers.Find(id);
            if (driver == null)
            {
                return HttpNotFound();
            }
            return View(driver);
        }

        // POST: GeneratedData/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Driver driver = db.Drivers.Find(id);
            db.Drivers.Remove(driver);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        public ActionResult ExportExcelSheet()
        {
            try
            {
                var drivers = db.Drivers.ToList();

                Workbook workbook = ExcelHelper.GetExcelGeneratedData(drivers);
                // Saves the excel file
                var memoryStream = new MemoryStream();
                workbook.Save(memoryStream, SaveFormat.Xlsx);

                memoryStream.Seek(0, SeekOrigin.Begin);

                return File(memoryStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "GeneratedData.xlsx");
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        public ActionResult TotalGeneratedData()
        {
            try
            {
                var drivers = db.Drivers.ToList();

                List<DriverStats> driverStatsList = ExcelHelper.GetTotalDriverStats(drivers);

                Workbook totalWorkBook = ExcelHelper.GetExcelTotalData(driverStatsList);

                // Saves the excel file
                var memoryStream = new MemoryStream();
                totalWorkBook.Save(memoryStream, SaveFormat.Xlsx);

                memoryStream.Seek(0, SeekOrigin.Begin);

                return File(memoryStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "TotalGeneratedData.xlsx");
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        public ActionResult WorstTenDrivers(string type)
        {
            try
            {
                var drivers = db.Drivers.ToList();

                List<DriverStats> driverStatsList = ExcelHelper.GetTotalDriverStats(drivers);
                Workbook workBook = new Workbook();
                string excelName = "Worst";
                switch (type)
                {
                    case "km":
                        excelName = "TenWorstDriversPerKm";
                        workBook = ExcelHelper.GetExcelTenWorst(driverStatsList.OrderByDescending(o => o.TotalPenaltiesPerKm).Take(10).ToList(), "Ratio of (Total Penalties) / (Total Km)", TypeOfCalculationEnum.PerKm);
                        break;
                    case "h":
                        excelName = "TenWorstDriversPerTrip";
                        workBook = ExcelHelper.GetExcelTenWorst(driverStatsList.OrderByDescending(o => o.TotalPenaltiesPerPeriod).Take(10).ToList(), "Ratio of (Total Penalties) / (Total Hr)", TypeOfCalculationEnum.PerHr);
                        break;
                    case "trip":
                        excelName = "TenWorstDriversPerTrip";
                        workBook = ExcelHelper.GetExcelTenWorst(driverStatsList.OrderByDescending(o => o.TotalPenaltiesPerTrip).Take(10).ToList(), "Ratio of (Total Penalties) / (Total Trips)", TypeOfCalculationEnum.PerTrip);
                        break;
                    default:
                        break;
                }


                // Saves the excel file
                var memoryStream = new MemoryStream();
                workBook.Save(memoryStream, SaveFormat.Xlsx);

                memoryStream.Seek(0, SeekOrigin.Begin);

                return File(memoryStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{excelName}.xlsx");
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
