using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EnergoUchet.Models
{
    public class MeterReadingListViewModel
    {
        public IEnumerable<MeterReading> MeterReadings { get; set; }
        public SelectList MeteringDevices { get; set; }
    }
}