using APBD_09.DTO;
using APBD_09.Helpers;
using System.Threading.Tasks;

namespace APBD_09.Services
{
    public interface IDoctorDbService
    {
        Task<ResponseHelper> GetDoctorsListAsync();
        Task<ResponseHelper> AddDoctorAsync(DoctorDTO doctor);
        Task<ResponseHelper> ChangeDoctorsAsync(int id, DoctorDTO doctor);
        Task<ResponseHelper> DeleteDoctorsAsync(int id);
    }
}
