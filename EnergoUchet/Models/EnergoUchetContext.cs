using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace EnergoUchet.Models
{
    public class EnergoUchetContext : DbContext
    {
        public EnergoUchetContext(): base("DefaultConnection")
        {

        }
        public DbSet<Consumer> Consumers { get; set; }
        public DbSet<Building> Buildings { get; set; }
        public DbSet<MeteringDevice> MeteringDevices { get; set; }
        public DbSet<EnergyResourse> EnergyResourses { get; set; }
        public DbSet<MeterReading> MeterReadings { get; set; }
    }
}