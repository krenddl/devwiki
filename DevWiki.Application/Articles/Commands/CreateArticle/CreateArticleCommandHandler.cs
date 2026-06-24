using DevWiki.Application.Interfaces;
using DevWiki.Domain.Entities;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace DevWiki.Application.Articles.Commands.CreateArticle
{
    public class CreateArticleCommandHandler : IRequestHandler<CreateArticleCommand, Guid>
    {
        private readonly IApplicationDbContext _context;

        public CreateArticleCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> Handle(CreateArticleCommand request, CancellationToken cancellationToken)
        {
            var article = new Article
            {
                Id = Guid.NewGuid(),
                Title = request.Title,
                Content = request.Content,
                CreatedAt = DateTime.UtcNow
            };

            if (!string.IsNullOrWhiteSpace(request.Tags))
            {
                var tagNames = request.Tags.Split(',', StringSplitOptions.RemoveEmptyEntries)
                                           .Select(t => t.Trim().ToLower())
                                           .Distinct()
                                           .ToList();

                foreach (var tagName in tagNames)
                {
                    var existingTag = await _context.Tags.FirstOrDefaultAsync(t => t.Name == tagName, cancellationToken);
                    
                    if (existingTag == null)
                    {
                        existingTag = new Tag { Name = tagName };
                        _context.Tags.Add(existingTag);
                    }

                    article.ArticleTags.Add(new ArticleTag { Article = article, Tag = existingTag });
                }
            }

            _context.Articles.Add(article);
            await _context.SaveChangesAsync(cancellationToken);

            return article.Id;
        }
    }
}
