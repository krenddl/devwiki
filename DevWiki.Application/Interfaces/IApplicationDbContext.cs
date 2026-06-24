using DevWiki.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DevWiki.Application.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<User> Users { get; }
        DbSet<Article> Articles { get; }
        DbSet<Tag> Tags { get; }
        DbSet<ArticleTag> ArticleTags { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
