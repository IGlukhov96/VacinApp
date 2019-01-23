using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace VaccinationRecord.Models
{
    public class VaccinationContext: DbContext
    {
        public DbSet<Drug> Drugs { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Vaccination> Vaccinations { get; set; }
    }
}