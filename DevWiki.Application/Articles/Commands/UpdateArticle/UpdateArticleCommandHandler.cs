using DevWiki.Application.Interfaces;
using DevWiki.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DevWiki.Application.Articles.Commands.UpdateArticle
{
    public class UpdateArticleCommandHandler : IRequestHandler<UpdateArticleCommand, Unit>
    {
        private readonly IApplicationDbContext _context;

        public UpdateArticleCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(UpdateArticleCommand request, CancellationToken cancellationToken)
        {
            var article = await _context.Articles
                .Include(a => a.ArticleTags)
                .FirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken);

            if (article == null)
            {
                throw new Exception("Article not found."); // TODO: Use proper NotFoundException
            }

            article.Title = request.Title;
            article.Content = request.Content;
            article.UpdatedAt = DateTime.UtcNow;

            // Очищаем старые теги
            _context.ArticleTags.RemoveRange(article.ArticleTags);
            article.ArticleTags.Clear();

            if (!string.IsNullOrWhiteSpace(request.Tags))
            {
                var tagNames = request.Tags.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                foreach (var tagName in tagNames)
                {
                    var existingTag = await _context.Tags.FirstOrDefaultAsync(t => t.Name.ToLower() == tagName.ToLower(), cancellationToken);
                    if (existingTag == null)
                    {
                        existingTag = new Tag { Name = tagName };
                        _context.Tags.Add(existingTag);
                    }

                    article.ArticleTags.Add(new ArticleTag { Article = article, Tag = existingTag });
                }
            }

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
