using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EnergoUchet.Models
{
    public class BuildingListViewModel
    {
        public IEnumerable<Building> Buildings { get; set; }
        public SelectList Consumers { get; set; }
    }
}