using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EnergoUchet.Models;
using ClosedXML.Excel;
using System.IO;

namespace EnergoUchet.Controllers
{
    [Authorize]
    public class MeterReadingsController : Controller
    {
        private EnergoUchetContext db = new EnergoUchetContext();

        // GET: MeterReadings
        public ActionResult Index()
        {
            var meterReadings = db.MeterReadings.Include(m => m.MeteringDevice);
            return View(meterReadings.ToList());
        }

        public ActionResult Export()
        {
            IQueryable<MeterReading> meterReadings2  = db.MeterReadings.Include(p => p.MeteringDevice);
            List<MeterReading> meterReadings = meterReadings2.ToList();

            using (XLWorkbook workbook = new XLWorkbook(XLEventTracking.Disabled))
            {
                var worksheet = workbook.Worksheets.Add("Показания");

                worksheet.Cell("A1").Value = "Прибор учета";
                worksheet.Cell("B1").Value = "Дата";
                worksheet.Cell("C1").Value = "Показания";

                worksheet.Row(1).Style.Font.Bold = true;

                for (int i = 0; i < meterReadings.Count; i++)
                {
                    worksheet.Cell(i + 2, 1).Value = meterReadings[i].MeteringDevice.Model;
                    worksheet.Cell(i + 2, 2).Value = meterReadings[i].DateReadings;
                    worksheet.Cell(i + 2, 3).Value = meterReadings[i].Value;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    stream.Flush();

                    return new FileContentResult(stream.ToArray(),
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                    {
                        FileDownloadName = $"meterReadings_{DateTime.UtcNow.ToShortDateString()}.xlsx"
                    };
                }
            }
        }
        // GET: MeterReadings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MeterReading meterReading = db.MeterReadings.Find(id);
            if (meterReading == null)
            {
                return HttpNotFound();
            }
            return View(meterReading);
        }

        // GET: MeterReadings/Create
        public ActionResult Create()
        {
            ViewBag.MeteringDeviceId = new SelectList(db.MeteringDevices, "Id", "Model");
            return View();
        }

        // POST: MeterReadings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,DateReadings,Value,MeteringDeviceId")] MeterReading meterReading)
        {
            if (ModelState.IsValid)
            {
                db.MeterReadings.Add(meterReading);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MeteringDeviceId = new SelectList(db.MeteringDevices, "Id", "Model", meterReading.MeteringDeviceId);
            return View(meterReading);
        }

        // GET: MeterReadings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MeterReading meterReading = db.MeterReadings.Find(id);
            if (meterReading == null)
            {
                return HttpNotFound();
            }
            ViewBag.MeteringDeviceId = new SelectList(db.MeteringDevices, "Id", "Model", meterReading.MeteringDeviceId);
            return View(meterReading);
        }

        // POST: MeterReadings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,DateReadings,Value,MeteringDeviceId")] MeterReading meterReading)
        {
            if (ModelState.IsValid)
            {
                db.Entry(meterReading).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MeteringDeviceId = new SelectList(db.MeteringDevices, "Id", "Model", meterReading.MeteringDeviceId);
            return View(meterReading);
        }

        // GET: MeterReadings/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MeterReading meterReading = db.MeterReadings.Find(id);
            if (meterReading == null)
            {
                return HttpNotFound();
            }
            return View(meterReading);
        }

        // POST: MeterReadings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MeterReading meterReading = db.MeterReadings.Find(id);
            db.MeterReadings.Remove(meterReading);
            db.SaveChanges();
            return RedirectToAction("Index");
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
