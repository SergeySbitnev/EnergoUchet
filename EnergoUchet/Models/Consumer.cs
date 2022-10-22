using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace EnergoUchet.Models
{
    public class Consumer
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Фамилия")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Имя")]
        public string FirstName { get; set; }

        [Display(Name = "Отчество")]
        public string SecondName { get; set; }

        [Required]
        [Display(Name = "Организация")]
        public string Organization { get; set; }

        [Required]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Телефон")]
        public string Phone { get; set; }

        public ICollection<Building> Buildings { get; set; }

        public Consumer()
        {
            Buildings = new List<Building>();
        }
    }
}