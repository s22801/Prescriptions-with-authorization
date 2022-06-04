using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;

namespace APBD_8.Entities.Configurations
{
    public class PrescriptionEfConfiguration : IEntityTypeConfiguration<Prescription>
    {
        public void Configure(EntityTypeBuilder<Prescription> builder)
        {
            builder.HasKey(e => e.IdPrescription).HasName("IdPrescription_PK");
            builder.Property(e => e.IdPrescription).UseIdentityColumn();

            builder.Property(e => e.Date).IsRequired();
            builder.Property(e => e.DueDate).IsRequired();

            builder.HasOne(e => e.IdDoctorNavigation)
                .WithMany(e => e.Prescriptions)
                .HasForeignKey(e => e.IdDoctor)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Doctor_Prescription_FK");

            builder.HasOne(e => e.IdPatientNavigation)
                .WithMany(e => e.Prescriptions)
                .HasForeignKey(e => e.IdPatient)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Patient_Prescription_FK");

            builder.ToTable(nameof(Prescription));

            /*
            var prescriptions = new List<Prescription>();

            
            prescriptions.Add(new Prescription
            {
                IdPrescription = 1,
                Date = DateTime.Now,
                DueDate = DateTime.Now.AddDays(90),
                IdPatientNavigation = 1,
                IdDoctorNavigation = 1
            });

            prescriptions.Add(new Prescription
            {
                IdPrescription = 2,
                Date = DateTime.Now,
                DueDate = DateTime.Now.AddDays(90),
                IdPatient = 2,
                IdDoctor = 1
            });

            prescriptions.Add(new Prescription
            {
                IdPrescription = 3,
                Date = DateTime.Now,
                DueDate = DateTime.Now.AddDays(90),
                IdPatient = 1,
                IdDoctor = 2
            });

            prescriptions.Add(new Prescription
            {
                IdPrescription = 4,
                Date = DateTime.Now,
                DueDate = DateTime.Now.AddDays(90),
                IdPatient = 1,
                IdDoctor = 3
            });

            prescriptions.Add(new Prescription
            {
                IdPrescription = 5,
                Date = DateTime.Now,
                DueDate = DateTime.Now.AddDays(90),
                IdPatient = 2,
                IdDoctor = 3
            });

            builder.HasData(prescriptions);
            */
        }
    }
}
