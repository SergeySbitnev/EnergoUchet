using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace EnergoUchet.Models
{
    public class MeterReading
    {
        public int Id { get; set; }

        [DataType(DataType.Date), Required]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Дата")]
        public DateTime DateReadings { get; set; }

        [Required]
        [Display(Name = "Показания")]
        public double Value { get; set; }

        public int? MeteringDeviceId { get; set; }
        public MeteringDevice MeteringDevice { get; set; }
    }
}