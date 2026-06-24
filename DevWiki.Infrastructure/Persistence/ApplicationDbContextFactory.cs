using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevWiki.Infrastructure.Persistence
{
    // Этот класс нужен ТОЛЬКО для команд "dotnet ef..." в консоли
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

            
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=DevWikiDb;Username=postgres;Password=123");

            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}
