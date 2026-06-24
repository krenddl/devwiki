using MediatR;
using System;

namespace DevWiki.Application.Articles.Commands.CreateArticle
{
    public record CreateArticleCommand(string Title, string Content, string Tags) : IRequest<Guid>;
}
