using DevWiki.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DevWiki.Application.Articles.Queries.GetArticleById
{
    public class GetArticleByIdQueryHandler : IRequestHandler<GetArticleByIdQuery, ArticleDto?>
    {
        private readonly IApplicationDbContext _context;

        public GetArticleByIdQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ArticleDto?> Handle(GetArticleByIdQuery request, CancellationToken cancellationToken)
        {
            var article = await _context.Articles
                .Include(a => a.ArticleTags)
                .ThenInclude(t => t.Tag)
                .FirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken);

            if (article == null)
            {
                return null;
            }

            return new ArticleDto
            {
                Id = article.Id,
                Title = article.Title,
                Content = article.Content,
                CreatedAt = article.CreatedAt,
                Tags = article.ArticleTags.Select(t => t.Tag.Name).ToList()
            };
        }
    }
}
