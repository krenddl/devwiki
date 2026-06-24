using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using DevWiki.Client.Auth;
using DevWiki.Client.Models;

namespace DevWiki.Client.Services
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly AuthenticationStateProvider _authStateProvider;

        public AuthService(HttpClient httpClient, AuthenticationStateProvider authStateProvider)
        {
            _httpClient = httpClient;
            _authStateProvider = authStateProvider;
        }

        public async Task<bool> LoginAsync(LoginDto loginDto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/Auth/login", loginDto);
            
            if (response.IsSuccessStatusCode)
            {
                var userProfile = await _httpClient.GetFromJsonAsync<UserDto>("api/Auth/me");
                if (userProfile != null)
                {
                    ((CustomAuthStateProvider)_authStateProvider).NotifyUserAuthentication(userProfile);
                    return true;
                }
            }

            return false;
        }

        public async Task<bool> RegisterAsync(RegisterDto registerDto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/Auth/register", new { registerDto.Username, registerDto.Password });
            return response.IsSuccessStatusCode;
        }

        public async Task LogoutAsync()
        {
            await _httpClient.PostAsync("api/Auth/logout", null);
            ((CustomAuthStateProvider)_authStateProvider).NotifyUserLogout();
        }
    }
}
