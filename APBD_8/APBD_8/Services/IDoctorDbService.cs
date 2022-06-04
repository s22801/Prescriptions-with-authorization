using APBD_8.DTO;
using APBD_8.Helpers;
using System.Threading.Tasks;

namespace APBD_8.Services
{
    public interface IDoctorDbService
    {
        Task<ResponseHelper> GetDoctorsListAsync();
        Task<ResponseHelper> AddDoctorAsync(DoctorDTO doctor);
        Task<ResponseHelper> ChangeDoctorsAsync(int id, DoctorDTO doctor);
        Task<ResponseHelper> DeleteDoctorsAsync(int id);
    }
}
