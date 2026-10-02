using System.Threading.Tasks;
using BLL.DTO;

namespace BLL.Services
{
    public interface ILoginService
    {
        Task<LoginResponseDto> LoginAsync(LoginRequestDto request);
    }
}
