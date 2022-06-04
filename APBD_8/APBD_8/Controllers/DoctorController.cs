using APBD_8.DTO;
using APBD_8.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;


namespace APBD_8.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        private readonly IDoctorDbService _dbService;
        public DoctorController(IDoctorDbService dbService)
        {
            _dbService = dbService;
        }

        [HttpGet]
        public async Task<IActionResult> GetDoctorsAsync()
        {
            var result = await _dbService.GetDoctorsListAsync();
            return StatusCode((int)result.StatusCode, result.ResultDataCollection);
        }

        [HttpPost]
        public async Task<IActionResult> AddDoctorAsync(DoctorDTO doctorDTO)
        {
            var result = await _dbService.AddDoctorAsync(doctorDTO);
            return StatusCode((int)result.StatusCode, result.ResultObject);
        }

        [HttpDelete("{idDoctor}")]
        public async Task<IActionResult> DeleteDoctorAsync([FromRoute] int idDoctor)
        {
            var result = await _dbService.DeleteDoctorsAsync(idDoctor);
            return StatusCode((int)result.StatusCode, result.Message);
        }

        [HttpPut("{idDoctor}")]
        public async Task<IActionResult> UpdateDoctorAsync([FromRoute] int idDoctor, DoctorDTO doctorDTO)
        {
           var result = await _dbService.ChangeDoctorsAsync(idDoctor, doctorDTO);
            return StatusCode((int)result.StatusCode, result.Message);
        }

    }
}
