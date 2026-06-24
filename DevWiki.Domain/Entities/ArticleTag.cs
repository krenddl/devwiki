using System;
using System.Collections.Generic;
using System.Text;

namespace DevWiki.Domain.Entities
{
    public class ArticleTag
    {
        public Guid ArticleId { get; set; }
        public Article Article { get; set; } = null!;

        public int TagId { get; set; }
        public Tag Tag { get; set; } = null!;
    }
}
