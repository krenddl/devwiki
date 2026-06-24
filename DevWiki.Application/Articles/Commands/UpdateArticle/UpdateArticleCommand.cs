using MediatR;
using System;

namespace DevWiki.Application.Articles.Commands.UpdateArticle
{
    public class UpdateArticleCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string Tags { get; set; } = string.Empty;
    }
}
