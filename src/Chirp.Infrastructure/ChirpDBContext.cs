// ReferenceLink:
//  https://learn.microsoft.com/en-us/ef/core/get-started/overview/first-app?tabs=netcore-cli
//  https://learn.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/infrastructure-persistence-layer-design
//  https://learn.microsoft.com/en-us/archive/msdn-magazine/2009/june/the-unit-of-work-pattern-and-persistence-ignorance
//  https://learn.microsoft.com/en-us/aspnet/mvc/overview/older-versions/getting-started-with-ef-5-using-mvc-4/implementing-the-repository-and-unit-of-work-patterns-in-an-asp-net-mvc-application

using Microsoft.EntityFrameworkCore;

namespace Chirp.Infrastructure
{
    /// <inheritdoc/>
    public class ChirpDBContext : DbContext, IDbContext
    {
        public DbSet<Cheep> Cheeps { get; set; } = null!;
        public DbSet<Author> Authors { get; set; } = null!;
        public DbSet<AuthorAuthorRelation> AuthorAuthorRelations { get; set; }


        public ChirpDBContext(DbContextOptions<ChirpDBContext> options) : base(options)
        {
        }
        async Task IDbContext.SaveChanges()
        {
            await this.SaveChangesAsync();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Apply all EntityConfigurations defined in current assembly..
            builder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
        }
    }
}