using APBD_8.DTO;
using APBD_8.Entities;
using APBD_8.Helpers;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace APBD_8.Services
{
    public class AccountDbService : IAccountDbService
    {
        private readonly PrescriptionContext _context;
        private readonly string Secret;

        public AccountDbService(IConfiguration configuration, PrescriptionContext context)
        {
            Secret = configuration["Secret"];
            _context = context;
        }

        public async Task<ResponseHelper> LoginAsync(LoginDTO loginDTO)
        {
            /*
             * Sprawdzamy czy w bazie danych istnieje podany Email (Email/login) jest unikalny w ramach bazy danych
             *            oraz czy hasło się zgadza
             *            
             * Jeśli e-mail nie istnieje lub hasła sie nie zgadzają zwracamy 400 badRequest
             * Jeśeli wszystko się zgadza  generujemy parę : access token + refresh token
             * 
             */

            var user = await _context.Users.FindAsync(loginDTO.Email);

            if (user == null)
            {
                return new ResponseHelper(System.Net.HttpStatusCode.BadRequest,"Entered login is not found.");
            }

            var saltBytes = new byte[16];

            var hashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2( loginDTO.Password, saltBytes, KeyDerivationPrf.HMACSHA256, 10000, 32));

            PasswordVerificationResult verifyPassword = new PasswordHasher<User>()
                                                            .VerifyHashedPassword(user, hashedPassword, loginDTO.Password);
            if (verifyPassword == PasswordVerificationResult.Failed)
            {
                return new ResponseHelper(System.Net.HttpStatusCode.BadRequest, "The password is incorrect.");
                // Hasła się nie zgadzają, zwracamy 400 BadRequst
            }

            JwtSecurityToken token = AuthHelper.GenerateToken(Secret);

            Guid refreshToken = Guid.NewGuid(); // <- wartość zostaje zapisana w tabelu User w rekordzie dla danego użytkownika dodatkowo dodajemy (w bazie) czas wygaśnięcia refresh tokena


            return new ResponseHelper(System.Net.HttpStatusCode.OK, new JwtSecurityTokenHandler().WriteToken(token), refreshToken);
        }

        public async Task<ResponseHelper> RegisterAsync(RegisterDTO registerDTO)
        {
            if (registerDTO.Password.Length < 10)
            {
                return new ResponseHelper(System.Net.HttpStatusCode.BadRequest, "Password should contain at least 10 characters.");
            }

            var checkRegister = await _context.Users.FindAsync(registerDTO.Email);

            if (checkRegister != null)
            {
                return new ResponseHelper(System.Net.HttpStatusCode.BadRequest,"There is already an account associated with entered email.");
            }

            var saltBytes = new byte[16];

            RandomNumberGenerator.Create().GetBytes(saltBytes);

            var hashedPassword = Convert.ToBase64String(
                KeyDerivation.Pbkdf2(registerDTO.Password, saltBytes, KeyDerivationPrf.HMACSHA256, 10000, 32));


            var user = new User
            {
                Email = registerDTO.Email,
                Password = hashedPassword,
                Salt = Convert.ToBase64String(saltBytes),
                RefreshToken = Guid.NewGuid().ToString(),
                TokenExpirationTime = DateTime.Now.AddMinutes(30)
            };

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return new ResponseHelper(System.Net.HttpStatusCode.OK, "Registered succesfully");
        }
    }
}
