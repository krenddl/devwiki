using DevWiki.Application.Interfaces;
using DevWiki.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DevWiki.Application.Articles.Queries
{
    public record GetArticlesQuery(string? SearchTerm = null) : IRequest<List<Article>>;

    // 2. Обработчик (Handler). Он знает, КАК достать этот список.
    public class GetArticlesQueryHandler : IRequestHandler<GetArticlesQuery, List<Article>>
    {
        private readonly IApplicationDbContext _context;

        public GetArticlesQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Article>> Handle(GetArticlesQuery request, CancellationToken cancellationToken)
        {
            var query = _context.Articles
                .Include(a => a.ArticleTags)
                .ThenInclude(at => at.Tag)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                var search = request.SearchTerm.ToLower();
                query = query.Where(a => 
                    a.Title.ToLower().Contains(search) || 
                    a.Content.ToLower().Contains(search) ||
                    a.ArticleTags.Any(at => at.Tag.Name.ToLower().Contains(search))
                );
            }

            return await query
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync(cancellationToken);
        }
    }
}
