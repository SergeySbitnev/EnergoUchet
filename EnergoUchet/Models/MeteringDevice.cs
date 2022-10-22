using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace EnergoUchet.Models
{
    public class MeteringDevice
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Модель")]
        public string Model { get; set; }

        public int? BuildingId { get; set; }
        public Building Building { get; set; }

        public int? EnergyResourseId { get; set; }
        public EnergyResourse EnergyResourse { get; set; }

        public ICollection<MeterReading> MeterReadings { get; set; }
        public MeteringDevice()
        {
            MeterReadings = new List<MeterReading>();
        }
    }
}