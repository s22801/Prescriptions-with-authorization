using APBD_8.DTO;
using APBD_8.Entities;
using APBD_8.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APBD_8.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrescriptionController : ControllerBase
    {
        private IPrescriptionDbService _dbService;

        public PrescriptionController(IPrescriptionDbService dbService)
        {
            _dbService = dbService;
        }


        [HttpGet("{idPrescription}")]
        public async Task<IActionResult> GetPrescriptionAsync([FromRoute] int idPrescription)
        {
            var result = await _dbService.GetPrescriptionAsync(idPrescription);
            return StatusCode((int)result.StatusCode, result.ResultDataCollection);
        }
    }

    /*
private readonly PrescriptionDbService _dbService;
public PrescriptionsController(PrescriptionDbService service)
{
_dbService = service;
}


[HttpGet]
public async Task<IActionResult> GetDoctorsListAsync()
{
var result = _dbService.GetDoctorsList();
if (result.Count == 0)
{
return NotFound();
}
return Ok(result);
}

[HttpPost]
public async Task<IActionResult> AddDoctor(DoctorDTO doctorDTO)
{
return Ok(_dbService.AddDoctorAsync(doctorDTO));
}

[HttpDelete("{id}")]
public async Task<IActionResult> DeleteDoctorAsync(int id)
{
await _dbService.DeleteDoctorAsync(id);
return new OkObjectResult($"Doctor with id = {id} was succesfully deleted.");
}

[HttpPut("{id}")]
public async Task<IActionResult> UpdateDoctorAsync(int id, DoctorDTO doctorDTO)
{
bool result = await _dbService.ChangeDoctorAsync(id, doctorDTO);

if (!result)
{
NotFound($"Doctor with id = {id} was not found.");
}

return Ok("Succesfully updated.");
}

[HttpGet("{id}")]
public async Task<IActionResult> GetPrescription(int id)
{
var result = _dbService.GetPrescription(id);

if (result == null)
{
return NotFound($"Prescription with given id = {id} was not found.");
}

return Ok(result);
}
*/
}