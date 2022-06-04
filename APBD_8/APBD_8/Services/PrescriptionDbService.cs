using APBD_8.DTO;
using APBD_8.Entities;
using APBD_8.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace APBD_8.Services
{
    public class PrescriptionDbService : IPrescriptionDbService
    {
        private readonly PrescriptionContext _context;

        public PrescriptionDbService(PrescriptionContext context)
        {
            _context = context;
        }

        public async Task<ResponseHelper> GetPrescriptionAsync(int id)
        {
            var checkPrescription = await _context.Prescriptions.FindAsync(id);

            if(checkPrescription == null)
            {
                return new ResponseHelper(System.Net.HttpStatusCode.NotFound, "There is not prescription with given Id.");
            }

            var result = await _context.Prescriptions
                .Where(x => x.IdPrescription == id)
                .AsSplitQuery()
                .Select(p => new PrescriptionDTO
                {
                    Date = p.Date,
                    DueDate = p.DueDate,
                    Patient = _context.Patients
                                        .Where(e => e.IdPatient == p.IdPatient)
                                        .Select(e => new PatientDTO
                                        {
                                            FirstName = e.FirstName,
                                            LastName = e.LastName,
                                            Birthday = e.Birthday
                                        }).FirstOrDefault(),
                    Doctor = _context.Doctors
                                       .Where(d => d.IdDoctor == p.IdDoctor)
                                       .Select(d => new DoctorDTO
                                       {
                                           FirstName = d.FirstName,
                                           LastName = d.LastName,
                                           Email = d.Email
                                       }).FirstOrDefault(),
                    Medicaments = p.PrescriptionMedicaments
                                          .Select(e => new MedicamentDTO
                                          {
                                              Name = e.IdMedicamentNavigation.Name,
                                              Details = e.Details,
                                              Dose = e.Dose
                                          }).ToList()
                }).FirstAsync();

            return new ResponseHelper(HttpStatusCode.OK, result);
        }


        /*
        //private readonly string _connString = "Data Source=(LocalDb)\\MSSQLLocalDB;Initial Catalog=Prescription;Integrated Security=True";
        private readonly PrescriptionContext _context = new();
        private readonly IConfiguration _configuration;

        public PrescriptionDbService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<IActionResult> GetDoctorsListAsync()
        {
            List<Doctor> result = new();
            await using SqlConnection conn = new(_configuration.GetConnectionString("DbConn"));
            await conn.OpenAsync();
            string sql = "SELECT * FROM Doctor;";
            await using SqlCommand cmd = new(sql,conn);
            using SqlDataReader reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                result.Add(new()
                {
                    IdDoctor = int.Parse(reader["IdDoctor"].ToString()),
                    FirstName = reader["FirstName"].ToString(),
                    LastName = reader["LastName"].ToString(),
                    Email = reader["Email"].ToString()
                });
            }
            await reader.CloseAsync();
            return new OkObjectResult(result);
        }

        public async Task<IActionResult> AddDoctorAsync(DoctorDTO dto)
        {
            await using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DbConn"));
            await con.OpenAsync();
            string sql = "SELECT MAX(IdDoctor)+1 FROM Doctor;";
            await using SqlCommand cmd = new SqlCommand(sql, con);
            using SqlDataReader reader = await cmd.ExecuteReaderAsync();
            await reader.ReadAsync();
            int nextId = int.Parse(reader[0].ToString());
            Doctor doctor = new()
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                IdDoctor = nextId
            };
            await _context.Doctors.AddAsync(doctor);
            await _context.SaveChangesAsync();
            return new OkObjectResult($"Doctor {dto.FirstName} {dto.LastName} was succesfully added to database.");
        }

        public async Task<IActionResult> ChangeDoctorAsync(int id, DoctorDTO dto)
        {
            if (id < 0)
            {
                return new BadRequestObjectResult("The entered IdDoctor is not a proper number.");
            }

            await using SqlConnection con = new(_configuration.GetConnectionString("DbConn"));
            await using SqlCommand command = con.CreateCommand();
            await con.OpenAsync();
            await using SqlTransaction transaction = (SqlTransaction)await con.BeginTransactionAsync();
            command.Transaction = transaction;

            if(! await CheckIfExists(id, con, transaction, "doctor"))
            {
                await transaction.RollbackAsync();
                return new NotFoundObjectResult($"Doctor with id = {id} was not found.");
            }

            command.CommandText = "UPDATE Doctor SET FirstName=@fName, LastName=@lName, Email=@Email WHERE id=@id;";
            command.Parameters.AddWithValue("id", id);
            command.Parameters.AddWithValue("fName", dto.FirstName);
            command.Parameters.AddWithValue("lName", dto.LastName);
            command.Parameters.AddWithValue("Email", dto.Email);

            await command.ExecuteNonQueryAsync();

            return new OkObjectResult($"Doctor with id = {id} was succesfully updated.");
        }

        private async Task<bool> CheckIfExists(int id, SqlConnection con, SqlTransaction transaction, string checkingObject)
        {
            await using SqlCommand command = con.CreateCommand();
            command.Transaction = transaction;
            if (checkingObject.Equals("doctor"))
            {
                command.CommandText = "SELECT COUNT(1) FROM Doctor WHERE IdDoctor=@id";
            }else if (checkingObject.Equals("prescription"))
            {
                command.CommandText = "SELECT COUNT(1) FROM Prescription WHERE IdPrescription=@id";
            }
            command.Parameters.AddWithValue("id", id);
            SqlDataReader dr = await command.ExecuteReaderAsync();
            await dr.ReadAsync();
            if (!dr.HasRows || int.Parse(dr[0].ToString()) == 0)
            {
                dr.CloseAsync();
                return false;
            }
            dr.CloseAsync();
            return true;
        }

        public async Task<IActionResult> DeleteDoctorAsync(int id)
        {
            if(id < 0)
            {
                return new BadRequestObjectResult("The entered IdDoctor is not a proper number.");
            }

            await using SqlConnection con = new(_configuration.GetConnectionString("DbConn"));
            await using SqlCommand command = con.CreateCommand();
            await con.OpenAsync();
            await using SqlTransaction transaction = (SqlTransaction) con.BeginTransaction();
            command.Transaction = transaction;

            if(! await CheckIfExists(id, con, transaction, "doctor"))
            {
                await transaction.RollbackAsync();
                return new NotFoundObjectResult($"Doctor with id = {id} was not found.");
            }

            command.CommandText = "DELETE FROM Prescriptions WHERE IdDoctor=@id; DELETE FROM Doctor WHERE IdDoctor=@id";
            command.Parameters.AddWithValue("id", id);
            await command.ExecuteNonQueryAsync();
            await transaction.CommitAsync();
            return new OkObjectResult($"Doctor with id = {id} was succesfully deleted.");
        }


        public async Task<IActionResult> GetPrescriptionAsync(int id)
        {
            await using SqlConnection con = new(_configuration.GetConnectionString("DbConn"));
            await using SqlCommand command = con.CreateCommand();
            await con.OpenAsync();
            using SqlTransaction transaction = (SqlTransaction)await con.BeginTransactionAsync();
            command.Transaction = transaction;
            
            if(!await CheckIfExists(id, con, transaction, "prescription"))
            {
                await transaction.RollbackAsync();
                return new BadRequestObjectResult($"There is no Prescription with IdPrescription = {id} in database.");
            }
            PrescriptionDTO result = new();
            IList<MedicamentDTO> medicaments = new List<MedicamentDTO>();

            command.CommandText = "SELECT Name, Description, Type FROM Medicament WHERE IdMedicament IN (SELECT IdMedicament FROM PrescriptionMedicament WHERE IdPrescription=@id);";
            command.Parameters.AddWithValue("id", id);
            SqlDataReader dr = await command.ExecuteReaderAsync();
            await dr.ReadAsync();
            if (!dr.HasRows)
            {
                return new NotFoundObjectResult("No medicaments were found.");
            }
            while (await dr.ReadAsync())
            {
                medicaments.Add(new()
                {
                    Name = dr["Name"].ToString(),
                    Description = dr["Description"].ToString(),
                    Type = dr["Type"].ToString()
                });
            }
            await dr.DisposeAsync();

            command.CommandText = "SELECT Date, DueDate, p.FirstName, p.LastName, Birthday, d.FirstName, d.LastName, d.Email FROM Prescription INNER JOIN Patient p ON p.IdPatient=Prescription.IdPatient INNER JOIN Doctor d ON d.IdDoctor=Prescription.IdDoctor WHERE IdPrescription=@id";

            SqlDataReader dr2 = await command.ExecuteReaderAsync();
            await dr2.ReadAsync();
            if (dr2.HasRows)
            {
                result = new()
                {
                    Medicaments = medicaments,
                    Date = DateTime.Parse(dr2["Date"].ToString()),
                    DueDate = DateTime.Parse(dr2["DueDate"].ToString()),
                    Patient = new()
                    {
                        FirstName = dr2["p.FirstName"].ToString(),
                        LastName = dr2["p.LastName"].ToString(),
                        Birthday = DateTime.Parse(dr2["Birthday"].ToString())
                    },
                    Doctor = new()
                    {
                        FirstName = dr2["d.FirstName"].ToString(),
                        LastName = dr2["d.LastName"].ToString(),
                        Email = dr2["Email"].ToString()
                    }
                };
            }
            await dr2.CloseAsync();

            return new OkObjectResult(result);
        }
        */

    }
}
