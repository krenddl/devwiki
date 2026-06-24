using MediatR;
using System;

namespace DevWiki.Application.Articles.Commands.DeleteArticle
{
    public record DeleteArticleCommand(Guid Id) : IRequest<Unit>;
}
