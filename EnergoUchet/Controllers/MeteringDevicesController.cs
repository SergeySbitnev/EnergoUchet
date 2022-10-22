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
    public class MeteringDevicesController : Controller
    {
        private EnergoUchetContext db = new EnergoUchetContext();

        // GET: MeteringDevices
        public ActionResult Index(int? building)
        {
            IQueryable<MeteringDevice> meteringDevices = db.MeteringDevices.Include(p => p.Building).Include(p => p.EnergyResourse);
            if (building != null && building != 0)
            {
                meteringDevices = meteringDevices.Where(p => p.BuildingId == building);
            }


            List<Building> buildings = db.Buildings.ToList();
            buildings.Insert(0, new Building { Address = "Все", Id = 0 });

            MeteringDeviceListViewModel mdlvm = new MeteringDeviceListViewModel
            {
                MeteringDevices = meteringDevices.ToList(),
                Buldings = new SelectList(buildings, "Id", "Address")
            };


            //var meteringDevices = db.MeteringDevices.Include(m => m.Building).Include(m => m.EnergyResourse);
            //return View(meteringDevices.ToList());
            return View(mdlvm);
        }

        public ActionResult Export()
        {
            IQueryable<MeteringDevice> meteringDevices2 = db.MeteringDevices.Include(p => p.Building).Include(p => p.EnergyResourse);
            List<MeteringDevice> meteringDevices = meteringDevices2.ToList();

            using (XLWorkbook workbook = new XLWorkbook(XLEventTracking.Disabled))
            {
                var worksheet = workbook.Worksheets.Add("Приборы учета");

                worksheet.Cell("A1").Value = "Объект";
                worksheet.Cell("B1").Value = "Тип учета";
                worksheet.Cell("C1").Value = "Прибор учета";
                

                worksheet.Row(1).Style.Font.Bold = true;

                for (int i = 0; i < meteringDevices.Count; i++)
                {
                    worksheet.Cell(i + 2, 1).Value = meteringDevices[i].Building.Address;
                    worksheet.Cell(i + 2, 2).Value = meteringDevices[i].EnergyResourse.Type;
                    worksheet.Cell(i + 2, 3).Value = meteringDevices[i].Model; 
                }
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    stream.Flush();

                    return new FileContentResult(stream.ToArray(),
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                    {
                        FileDownloadName = $"meteringDevices_{DateTime.UtcNow.ToShortDateString()}.xlsx"
                    };
                }
            }
        }

        // GET: MeteringDevices/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MeteringDevice meteringDevice = db.MeteringDevices.Find(id);
            if (meteringDevice == null)
            {
                return HttpNotFound();
            }
            return View(meteringDevice);
        }

        // GET: MeteringDevices/Create
        public ActionResult Create()
        {
            ViewBag.BuildingId = new SelectList(db.Buildings, "Id", "Address");
            ViewBag.EnergyResourseId = new SelectList(db.EnergyResourses, "Id", "Type");
            return View();
        }

        // POST: MeteringDevices/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Model,BuildingId,EnergyResourseId")] MeteringDevice meteringDevice)
        {
            if (ModelState.IsValid)
            {
                db.MeteringDevices.Add(meteringDevice);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.BuildingId = new SelectList(db.Buildings, "Id", "Address", meteringDevice.BuildingId);
            ViewBag.EnergyResourseId = new SelectList(db.EnergyResourses, "Id", "Type", meteringDevice.EnergyResourseId);
            return View(meteringDevice);
        }

        // GET: MeteringDevices/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MeteringDevice meteringDevice = db.MeteringDevices.Find(id);
            if (meteringDevice == null)
            {
                return HttpNotFound();
            }
            ViewBag.BuildingId = new SelectList(db.Buildings, "Id", "Address", meteringDevice.BuildingId);
            ViewBag.EnergyResourseId = new SelectList(db.EnergyResourses, "Id", "Type", meteringDevice.EnergyResourseId);
            return View(meteringDevice);
        }

        // POST: MeteringDevices/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Model,BuildingId,EnergyResourseId")] MeteringDevice meteringDevice)
        {
            if (ModelState.IsValid)
            {
                db.Entry(meteringDevice).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BuildingId = new SelectList(db.Buildings, "Id", "Address", meteringDevice.BuildingId);
            ViewBag.EnergyResourseId = new SelectList(db.EnergyResourses, "Id", "Type", meteringDevice.EnergyResourseId);
            return View(meteringDevice);
        }

        // GET: MeteringDevices/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MeteringDevice meteringDevice = db.MeteringDevices.Find(id);
            if (meteringDevice == null)
            {
                return HttpNotFound();
            }
            return View(meteringDevice);
        }

        // POST: MeteringDevices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MeteringDevice meteringDevice = db.MeteringDevices.Find(id);
            
            try
            {
                db.MeteringDevices.Remove(meteringDevice);
                db.SaveChanges();
            }
            catch
            {
                return View("ErrorDelete");
            }

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
