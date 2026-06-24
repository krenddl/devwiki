using System.Threading.Tasks;
using DevWiki.Client.Models;

namespace DevWiki.Client.Services
{
    public interface IAuthService
    {
        Task<bool> LoginAsync(LoginDto loginDto);
        Task<bool> RegisterAsync(RegisterDto registerDto);
        Task LogoutAsync();
    }
}
