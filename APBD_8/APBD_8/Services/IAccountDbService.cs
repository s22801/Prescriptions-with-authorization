using APBD_8.DTO;
using APBD_8.Helpers;
using System.Threading.Tasks;

namespace APBD_8.Services
{
    public interface IAccountDbService
    {
        Task<ResponseHelper> LoginAsync(LoginDTO loginDTO);
        Task<ResponseHelper> RegisterAsync(RegisterDTO registerDTO);
    }
}
