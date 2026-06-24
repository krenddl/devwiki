using System;
using System.Collections.Generic;

namespace DevWiki.Application.Articles.Queries.GetArticleById
{
    public class ArticleDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public List<string> Tags { get; set; } = new();
    }
}
