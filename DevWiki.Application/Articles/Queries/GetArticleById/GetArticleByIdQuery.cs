using MediatR;
using System;
using DevWiki.Application.Articles.Queries;

namespace DevWiki.Application.Articles.Queries.GetArticleById
{
    public record GetArticleByIdQuery(Guid Id) : IRequest<ArticleDto?>;
}
