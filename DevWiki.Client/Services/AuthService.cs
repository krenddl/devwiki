using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using DevWiki.Client.Auth;
using DevWiki.Client.Models;
using Microsoft.JSInterop;

namespace DevWiki.Client.Services
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly AuthenticationStateProvider _authStateProvider;
        private readonly IJSRuntime _jsRuntime;

        public AuthService(HttpClient httpClient, AuthenticationStateProvider authStateProvider, IJSRuntime jsRuntime)
        {
            _httpClient = httpClient;
            _authStateProvider = authStateProvider;
            _jsRuntime = jsRuntime;
        }

        public class LoginResponse { public string Token { get; set; } = string.Empty; }

        public async Task<bool> LoginAsync(LoginDto loginDto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/Auth/login", loginDto);
            
            if (response.IsSuccessStatusCode)
            {
                var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponse>();
                if (loginResponse != null && !string.IsNullOrEmpty(loginResponse.Token))
                {
                    await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "authToken", loginResponse.Token);
                }

                var meResponse = await _httpClient.GetAsync("api/Auth/me");
                if (meResponse.IsSuccessStatusCode)
                {
                    var userProfile = await meResponse.Content.ReadFromJsonAsync<UserDto>();
                    if (userProfile != null)
                    {
                        ((CustomAuthStateProvider)_authStateProvider).NotifyUserAuthentication(userProfile);
                        return true;
                    }
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
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "authToken");
            ((CustomAuthStateProvider)_authStateProvider).NotifyUserLogout();
        }
    }
}
