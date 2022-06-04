using APBD_09.DTO;
using APBD_09.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace APBD_09.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AccountController : ControllerBase
    {
        private readonly IAccountDbService _dbService;
        //private readonly string Secret;
        public AccountController(IAccountDbService accountDbService)
        {
        //    Secret = configuration["Secret"];
            _dbService = accountDbService;
        }


        [HttpPost("login")]
        //Authorize(Roles="doktor")]
        //[AllowAnonymous]
        public async Task<IActionResult> Login(LoginDTO loginDTO)
        {
            /*
             * Sprawdzamy czy w bazie danych istnieje podany Email (Email/login) jest unikalny w ramach bazy danych
             *            oraz czy hasło się zgadza
             *            
             * Jeśli e-mail nie istnieje lub hasła sie nie zgadzają zwracamy 400 badRequest
             * Jeśeli wszystko się zgadza  generujemy parę : access token + refresh token
             * 
             */
            var result = await _dbService.LoginAsync(loginDTO);

            return StatusCode((int)result.StatusCode, result.Message);

        }



        [HttpPost("register")]

        public async Task<IActionResult> Register(RegisterDTO registerDTO)
        {


            //Załózmy, że jesteśmy w serwisie bazodanowym
            //User user = new();
            //string hashedPassword = new PasswordHasher<User>().HashPassword(user, registerDTO.Password);

            //============= poniższa logika dla końcówki /login =================
            /*
            PasswordVerificationResult verifyPassword = new PasswordHasher<User>()
                                                            .VerifyHashedPassword(user, hashedPassword, registerDTO.Password);
            if (verifyPassword == PasswordVerificationResult.Failed)
            {
                return BadRequest("The password is incorrect.");
                // Hasła się nie zgadzają, zwracamy 400 BadRequst
            }

            */
            //Weryfikacja podczas logowania w serwisie bazodanowym
            /*User userFromDb = await _context....
           
            PasswordVerificationResult verifyPassword = new PasswordHasher<User>().VerifyHashedPassword(userFromDb, userFromDb.Password, loginDTO.Password);

            */
            var result = await _dbService.RegisterAsync(registerDTO);

            return StatusCode((int)result.StatusCode, result.Message);
        }
    }
}
