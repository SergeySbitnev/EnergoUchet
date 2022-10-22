using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace EnergoUchet.Models
{
    public class Building
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Страна")]
        public string Country { get; set; }

        [Required]
        [Display(Name = "Город")]
        public string Town { get; set; }

        [Required]
        [Display(Name = "Адрес")]
        public string Address { get; set; }


        [Required]
        [Display(Name = "Площадь")]
        public double Square { get; set; }

        [Required]
        [Display(Name = "Штат")]
        public int Staff { get; set; }

        public int? ConsumerId { get; set; }
        public Consumer Consumer { get; set; }

        public ICollection<MeteringDevice> MeteringDevices { get; set; }

        public Building()
        {
            MeteringDevices = new List<MeteringDevice>();
        }
    }
}