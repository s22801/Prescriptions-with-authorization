using APBD_09.Entities.Configurations;
using APBD_09.Services;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace APBD_09.Entities
{
    public class PrescriptionContext : DbContext, IPrescriptionContext
    {
        public virtual DbSet<Doctor> Doctors { get; set; }
        public virtual DbSet<Patient> Patients { get; set; }
        public virtual DbSet<Prescription> Prescriptions { get; set; }
        public virtual DbSet<Medicament> Medicaments { get; set; }
        public virtual DbSet<PrescriptionMedicament> PrescriptionMedicaments { get; set; }
        
        public virtual DbSet<User> Users { get; set; } 
        public PrescriptionContext(DbContextOptions<PrescriptionContext> options) : base(options)
        {

        }

        public PrescriptionContext() { }
        protected override void OnModelCreating(ModelBuilder model)
        {
            model.ApplyConfigurationsFromAssembly(typeof(DoctorEfConfiguration).GetTypeInfo().Assembly);
            model.Seed();
        }
    }
}
