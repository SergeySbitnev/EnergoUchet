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
    public class BuildingsController : Controller
    {
        private EnergoUchetContext db = new EnergoUchetContext();

        // GET: Buildings
        public ActionResult Index(int? consumer)
        {
            IQueryable<Building> buildings = db.Buildings.Include(p => p.Consumer);
            if (consumer != null && consumer != 0)
            {
                buildings = buildings.Where(p => p.ConsumerId == consumer);
            }

            List<Consumer> consumers = db.Consumers.ToList();
            consumers.Insert(0, new Consumer { Organization = "Все", Id = 0 });

            BuildingListViewModel blvm = new BuildingListViewModel
            {
                Buildings = buildings.ToList(),
                Consumers = new SelectList(consumers, "Id", "Organization")
            };

            return View(blvm);
        }

        public ActionResult Export()
        {
            IQueryable<Building> buildings2 = db.Buildings.Include(p => p.Consumer);
            List<Building> buildings = buildings2.ToList();

            using (XLWorkbook workbook = new XLWorkbook(XLEventTracking.Disabled))
            {
                var worksheet = workbook.Worksheets.Add("Объекты");

                worksheet.Cell("A1").Value = "Организация";
                worksheet.Cell("B1").Value = "Страна";
                worksheet.Cell("C1").Value = "Город";
                worksheet.Cell("D1").Value = "Адрес";
                worksheet.Cell("E1").Value = "Площадь";
                worksheet.Cell("F1").Value = "Штат";
                
                worksheet.Row(1).Style.Font.Bold = true;

                for (int i = 0; i < buildings.Count; i++)
                {
                    worksheet.Cell(i + 2, 1).Value = buildings[i].Consumer.Organization;
                    worksheet.Cell(i + 2, 2).Value = buildings[i].Country;
                    worksheet.Cell(i + 2, 3).Value = buildings[i].Town;
                    worksheet.Cell(i + 2, 4).Value = buildings[i].Address;
                    worksheet.Cell(i + 2, 5).Value = buildings[i].Square;
                    worksheet.Cell(i + 2, 6).Value = buildings[i].Staff;
                    
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    stream.Flush();

                    return new FileContentResult(stream.ToArray(),
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                    {
                        FileDownloadName = $"buildings_{DateTime.UtcNow.ToShortDateString()}.xlsx"
                    };
                }
            }
        }

        // GET: Buildings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Building building = db.Buildings.Find(id);
            if (building == null)
            {
                return HttpNotFound();
            }
            return View(building);
        }

        // GET: Buildings/Create
        public ActionResult Create()
        {
            ViewBag.ConsumerId = new SelectList(db.Consumers, "Id", "Organization");
            return View();
        }

        // POST: Buildings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Country,Town,Address,Square,Staff,ConsumerId")] Building building)
        {
            if (ModelState.IsValid)
            {
                db.Buildings.Add(building);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ConsumerId = new SelectList(db.Consumers, "Id", "Organization", building.ConsumerId);
            return View(building);
        }

        // GET: Buildings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Building building = db.Buildings.Find(id);
            if (building == null)
            {
                return HttpNotFound();
            }
            ViewBag.ConsumerId = new SelectList(db.Consumers, "Id", "Organization", building.ConsumerId);
            return View(building);
        }

        // POST: Buildings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Country,Town,Address,Square,Staff,ConsumerId")] Building building)
        {
            if (ModelState.IsValid)
            {
                db.Entry(building).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ConsumerId = new SelectList(db.Consumers, "Id", "Organization", building.ConsumerId);
            return View(building);
        }

        // GET: Buildings/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Building building = db.Buildings.Find(id);
            if (building == null)
            {
                return HttpNotFound();
            }
            return View(building);
        }

        // POST: Buildings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Building building = db.Buildings.Find(id);
            
            try
            {
                db.Buildings.Remove(building);
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
