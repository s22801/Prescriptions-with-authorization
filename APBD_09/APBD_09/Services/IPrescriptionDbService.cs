using APBD_09.DTO;
using APBD_09.Entities;
using APBD_09.Helpers;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace APBD_09.Services
{
    public interface IPrescriptionDbService
    {
        Task<ResponseHelper> GetPrescriptionAsync(int id);

        /*
        Task<IActionResult> GetDoctorsListAsync();
        Task<IActionResult> AddDoctorAsync(DoctorDTO dto);
        Task<IActionResult> ChangeDoctorAsync(int id, DoctorDTO dto);
        Task<IActionResult> DeleteDoctorAsync(int id);
        Task<IActionResult> GetPrescriptionAsync(int id);
        */
    }
}
