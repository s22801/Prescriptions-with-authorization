using APBD_09.DTO;
using APBD_09.Helpers;
using System.Threading.Tasks;

namespace APBD_09.Services
{
    public interface IAccountDbService
    {
        Task<ResponseHelper> LoginAsync(LoginDTO loginDTO);
        Task<ResponseHelper> RegisterAsync(RegisterDTO registerDTO);
    }
}
