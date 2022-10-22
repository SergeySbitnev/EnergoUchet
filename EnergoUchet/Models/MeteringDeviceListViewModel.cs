using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EnergoUchet.Models
{
    public class MeteringDeviceListViewModel
    {
        public IEnumerable<MeteringDevice> MeteringDevices { get; set; }
        public SelectList Buldings { get; set; }
        
    }
}