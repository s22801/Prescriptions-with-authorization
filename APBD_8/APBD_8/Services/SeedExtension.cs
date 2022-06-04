using APBD_8.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace APBD_8.Services
{
    public static class SeedExtension
    {

        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Doctor>().HasData(
                new
                {
                    IdDoctor = 1,
                    FirstName = "Jan",
                    LastName = "Kowalski",
                    Email = "jkow@apbd.com"
                },
                new
                {
                    IdDoctor = 2,
                    FirstName = "Janina",
                    LastName = "Kowalska",
                    Email = "janinka@apbd.com"
                }
                );

            modelBuilder.Entity<Patient>().HasData(
                new
                {
                    IdPatient = 1,
                    FirstName = "Adam",
                    LastName = "Nowak",
                    Birthday = DateTime.Now.AddYears(-25).AddMonths(-2).AddDays(-15)
                },
                new
                {
                    IdPatient = 2,
                    FirstName = "Adrainna",
                    LastName = "Nowacka",
                    Birthday = DateTime.Now.AddYears(-38).AddMonths(-7).AddDays(-21)
                }
                );

            modelBuilder.Entity<Prescription>().HasData(
                new
                {
                    IdPrescription = 1,
                    Date = DateTime.Now.AddDays(-2).AddHours(7),
                    DueDate = DateTime.Now.AddDays(5),
                    IdDoctor = 1,
                    IdPatient = 1
                },
                new
                {
                    IdPrescription = 2,
                    Date = DateTime.Now.AddDays(-2).AddHours(7),
                    DueDate = DateTime.Now.AddDays(5),
                    IdDoctor = 1,
                    IdPatient = 2
                }
                );

            modelBuilder.Entity<Medicament>().HasData(
                new
                {
                    IdMedicament = 1,
                    Name = "Paracetamol",
                    Description = "For headaches.",
                    Type = "Type 1"
                },
                new
                {
                    IdMedicament = 2,
                    Name = "Rutinoscorbin",
                    Description = "For everything.",
                    Type = "Type 4"
                }
                );

            modelBuilder.Entity<PrescriptionMedicament>().HasData(
                new
                {
                    IdMedicament = 1,
                    IdPrescription = 1,
                    Details = "2 times a day."
                },
                new
                {
                    IdMedicament = 2,
                    IdPrescription = 1,
                    Details = "3 times a day."
                },
                new
                {
                    IdMedicament = 2,
                    IdPrescription = 2,
                    Details = "3 times a day."
                }
                );
        }
    }
}
