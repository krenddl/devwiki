using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using DevWiki.Client.Models;

namespace DevWiki.Client.Services
{
    public class ArticleService : IArticleService
    {
        private readonly HttpClient _httpClient;

        public ArticleService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<ArticleDto>> GetArticlesAsync(string? searchTerm = null)
        {
            var url = "api/Articles";
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                url += $"?searchTerm={System.Uri.EscapeDataString(searchTerm)}";
            }
            return await _httpClient.GetFromJsonAsync<List<ArticleDto>>(url) ?? new List<ArticleDto>();
        }

        public async Task<ArticleDto?> GetArticleByIdAsync(System.Guid id)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<ArticleDto>($"api/Articles/{id}");
            }
            catch
            {
                return null;
            }
        }

        public async Task<System.Guid?> CreateArticleAsync(CreateArticleDto dto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/Articles", dto);
            if (response.IsSuccessStatusCode)
            {
                var createdId = await response.Content.ReadFromJsonAsync<System.Guid>();
                return createdId;
            }
            return null;
        }

        public async Task<bool> UpdateArticleAsync(System.Guid id, UpdateArticleDto dto)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/Articles/{id}", dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteArticleAsync(System.Guid id)
        {
            var response = await _httpClient.DeleteAsync($"api/Articles/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}
