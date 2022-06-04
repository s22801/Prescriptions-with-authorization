using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Collections.Generic;

namespace APBD_8.Entities.Configurations
{
    public class PrescriptionMedicamentEfConfiguration : IEntityTypeConfiguration<PrescriptionMedicament>
    {
        public void Configure(EntityTypeBuilder<PrescriptionMedicament> builder)
        {
            builder.HasKey(e => new
            {
                e.IdMedicament,
                e.IdPrescription
            }).HasName("PrescriptionMedicament_PK");

            builder.ToTable("Prescription_Medicament");

            builder.Property(e => e.Dose);
            builder.Property(e => e.Details).HasMaxLength(100).IsRequired();

            builder.HasOne(e => e.IdMedicamentNavigation)
                .WithMany(e => e.PrescriptionMedicaments)
                .HasForeignKey(e => e.IdMedicament)
                .HasConstraintName("Medicament_PrescriptionMedicament_FK")
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasOne(e => e.IdPrescriptionNavigation)
                .WithMany(e => e.PrescriptionMedicaments)
                .HasForeignKey(e => e.IdPrescription)
                .HasConstraintName("Prescription_PrescriptionMedicament_FK")
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.ToTable("Prescription_Medicament");

            // adding data
            /*
            var list = new List<PrescriptionMedicament>();

            
            list.Add(new PrescriptionMedicament
            {
                IdMedicament = 1,
                IdPrescription = 1,
                Dose = 200,
                Details = "2 pills in am and pm"
            });

            list.Add(new PrescriptionMedicament
            {
                IdMedicament = 2,
                IdPrescription = 1,
                Dose = 250,
                Details = "2 pills in am and pm"
            });

            list.Add(new PrescriptionMedicament
            {
                IdMedicament = 2,
                IdPrescription = 2,
                Dose = 250,
                Details = "2 pills in am and pm"
            });

            list.Add(new PrescriptionMedicament
            {
                IdMedicament = 3,
                IdPrescription = 3,
                Dose = 250,
                Details = "2 pills in am and pm"
            });

            builder.HasData(list);
            */
        }
    }
}
