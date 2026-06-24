using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DevWiki.Client.Models;

namespace DevWiki.Client.Services
{
    public interface IArticleService
    {
        Task<List<ArticleDto>> GetArticlesAsync(string? searchTerm = null);
        Task<ArticleDto?> GetArticleByIdAsync(Guid id);
        Task<Guid?> CreateArticleAsync(CreateArticleDto dto);
        Task<bool> UpdateArticleAsync(Guid id, UpdateArticleDto dto);
        Task<bool> DeleteArticleAsync(Guid id);
    }
}
