using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Helpers;
using EnergoUchet.Models;
using System.Data.Entity;
using System.IO;

namespace EnergoUchet.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private EnergoUchetContext db = new EnergoUchetContext();

        public ActionResult Index(int? meteringDevice)
        {
            IQueryable<MeterReading> meterReadings = db.MeterReadings.Include(p => p.MeteringDevice);
            meterReadings = meterReadings.Where(p => p.MeteringDeviceId == meteringDevice);
            List<MeterReading> mR = meterReadings.ToList();

            DateTime[] dates = new DateTime[30];
            double[] values = new double[30];
            int i = 0;
            foreach (var item in mR)
            {
                dates[i] = item.DateReadings;
                values[i] = item.Value;
                i++;
            }

            Array.Sort(dates);
            Array.Reverse(dates);
            Array.Sort(values);
            Array.Reverse(values);

            string[] x = new string[6];
            string[] y = new string[6];
            string[,] z = new string[2, 6];
            double sum = 0;
            for (i = 0; i < 5; i++)
            {
                z[0, 4-i] = x[4 - i] = dates[i].ToString("dd.MM.yyyy");
                sum += values[i] - values[i + 1];
                z[1, 4 - i] = y[4 - i] = (values[i] - values[i + 1]).ToString();
            }
            z[0, 5] = x[5] = "Прогноз";
            z[1, 5] = y[5] = (sum % 5).ToString();
            

            var filePathName = "~/Content/img/chart01.jpg";
            var chartImage = new Chart(800, 600);
            chartImage.AddTitle("Диаграмма потребления и прогноз");
            chartImage.AddSeries(
                    name: "MeterReading",
                    axisLabel: "Name",
                    xValue: x,
                    yValues: y);
            chartImage.Save(path: filePathName);

            List<MeteringDevice> meteringDevices = db.MeteringDevices.ToList();
            MeterReadingListViewModel mrlvm = new MeterReadingListViewModel
            {
                MeterReadings = meterReadings.ToList(),
                MeteringDevices = new SelectList(meteringDevices, "Id", "Model")
            };

            ViewBag.Message = z;
            return View(mrlvm);
        }
       
        public ActionResult About()
        {
            
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}