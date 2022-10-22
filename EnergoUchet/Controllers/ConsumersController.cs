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
    public class ConsumersController : Controller
    {
        private EnergoUchetContext db = new EnergoUchetContext();

        // GET: Consumers
        public ActionResult Index()
        {
            return View(db.Consumers.ToList());
        }

        public ActionResult Export()
        {
            List<Consumer> consumers = db.Consumers.ToList();

            using (XLWorkbook workbook = new XLWorkbook(XLEventTracking.Disabled))
            {
                var worksheet = workbook.Worksheets.Add("Потребители");
                worksheet.Cell("A1").Value = "Фамилия";
                worksheet.Cell("B1").Value = "Имя";
                worksheet.Cell("C1").Value = "Отчество";
                worksheet.Cell("D1").Value = "Организация";
                worksheet.Cell("E1").Value = "Email";
                worksheet.Cell("F1").Value = "Телефон";
                worksheet.Row(1).Style.Font.Bold = true;

                for (int i = 0; i < consumers.Count; i++)
                {
                    worksheet.Cell(i + 2, 1).Value = consumers[i].LastName;
                    worksheet.Cell(i + 2, 2).Value = consumers[i].FirstName;
                    worksheet.Cell(i + 2, 3).Value = consumers[i].SecondName;
                    worksheet.Cell(i + 2, 4).Value = consumers[i].Organization;
                    worksheet.Cell(i + 2, 5).Value = consumers[i].Email;
                    worksheet.Cell(i + 2, 6).Value = consumers[i].Phone;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    stream.Flush();

                    return new FileContentResult(stream.ToArray(),
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                    {
                        FileDownloadName = $"consumers_{DateTime.UtcNow.ToShortDateString()}.xlsx"
                    };
                }

            }
        }

        // GET: Consumers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Consumer consumer = db.Consumers.Find(id);
            if (consumer == null)
            {
                return HttpNotFound();
            }
            return View(consumer);
        }

        // GET: Consumers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Consumers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,LastName,FirstName,SecondName,Organization,Email,Phone")] Consumer consumer)
        {
            if (ModelState.IsValid)
            {
                db.Consumers.Add(consumer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(consumer);
        }

        // GET: Consumers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Consumer consumer = db.Consumers.Find(id);
            if (consumer == null)
            {
                return HttpNotFound();
            }
            return View(consumer);
        }

        // POST: Consumers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,LastName,FirstName,SecondName,Organization,Email,Phone")] Consumer consumer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(consumer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(consumer);
        }

        // GET: Consumers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Consumer consumer = db.Consumers.Find(id);
            if (consumer == null)
            {
                return HttpNotFound();
            }
            return View(consumer);
        }

        // POST: Consumers/Delete/5
        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [HandleError(ExceptionType = typeof(System.Data.Entity.Infrastructure.DbUpdateException), View = "ExceptionFound")]
        public ActionResult DeleteConfirmed(int id)
        {
            Consumer consumer = db.Consumers.Find(id);
            try
            {
                db.Consumers.Remove(consumer);
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
