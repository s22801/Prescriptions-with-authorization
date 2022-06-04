using APBD_8.DTO;
using APBD_8.Entities;
using APBD_8.Helpers;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace APBD_8.Services
{
    public class DoctorDbService : IDoctorDbService
    {
        private readonly PrescriptionContext _context;

        public DoctorDbService(PrescriptionContext context)
        {
            _context = context;
        }

        public async Task<ResponseHelper> GetDoctorsListAsync()
        {
            var result = await _context.Doctors.Select(x => new DoctorDTO
            {
                FirstName = x.FirstName,
                LastName = x.LastName,
                Email = x.Email
            }).ToListAsync();

            if (result.Count == 0)
            {
                return new ResponseHelper(System.Net.HttpStatusCode.NotFound, "Database is empty.");
            }

            return new ResponseHelper(System.Net.HttpStatusCode.OK, result);
        }


        public async Task<ResponseHelper> AddDoctorAsync(DoctorDTO doctorDTO)
        {
            Doctor doctor = await _context.Doctors.Where(x => x.Email == doctorDTO.Email && x.FirstName == doctorDTO.FirstName && x.LastName == doctorDTO.FirstName).FirstOrDefaultAsync();

            if (doctor != null)
            {
                return new ResponseHelper(System.Net.HttpStatusCode.BadRequest, "There is a doctor with given email in database.");
            }

            int id = _context.Doctors.Select(doctor => doctor.IdDoctor).Max();

            await _context.Doctors.AddAsync(new()
            {
                FirstName = doctorDTO.FirstName,
                LastName = doctorDTO.LastName,
                Email = doctorDTO.Email
            });

            await _context.SaveChangesAsync();

            return new ResponseHelper(System.Net.HttpStatusCode.OK, doctor);
        }


        public async Task<ResponseHelper> ChangeDoctorsAsync(int id, DoctorDTO doctorDTO)
        {
            Doctor doctor = await _context.Doctors.FindAsync(id);

            if (doctor == null)
            {
                return new ResponseHelper(System.Net.HttpStatusCode.NotFound, "There is no doctor with given id in database.");
            }
            doctor.FirstName = doctorDTO.FirstName;
            doctor.LastName = doctorDTO.LastName;
            doctor.Email = doctorDTO.Email;

            await _context.SaveChangesAsync();

            return new ResponseHelper(System.Net.HttpStatusCode.OK, "Doctor succesfully updated.");
        }

        public async Task<ResponseHelper> DeleteDoctorsAsync(int id)
        {
            Doctor doctor = await _context.Doctors.FindAsync(id);

            if (doctor == null)
            {
                return new ResponseHelper(System.Net.HttpStatusCode.BadRequest, "Doctor with given Id was not found.");
            }

            _context.Remove(doctor);

            await _context.SaveChangesAsync();

            return new ResponseHelper(System.Net.HttpStatusCode.OK, "Succesfully deleted");
        }
    }
}
