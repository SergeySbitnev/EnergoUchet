using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace EnergoUchet.Models
{
    public class EnergyResourse
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Тип учета")]
        public string Type { get; set; }

        public ICollection<MeteringDevice> MeteringDevices { get; set; }

        public EnergyResourse()
        {
            MeteringDevices = new List<MeteringDevice>();
        }
    }
}