using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace APBD_8.Entities.Configurations
{
    public class DoctorEfConfiguration : IEntityTypeConfiguration<Doctor>
    {
        public void Configure(EntityTypeBuilder<Doctor> builder)
        {
            builder.HasKey(doctor => doctor.IdDoctor).HasName("Doctor_PK");
            builder.Property(doctor => doctor.IdDoctor).UseIdentityColumn();

            builder.Property(doctor => doctor.FirstName).IsRequired().HasMaxLength(100);
            builder.Property(doctor => doctor.LastName).IsRequired().HasMaxLength(100);
            builder.Property(doctor => doctor.Email).IsRequired().HasMaxLength(100);

            builder.ToTable(nameof(Doctor));
           
        }
    }
}
