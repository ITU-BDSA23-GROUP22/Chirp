// the following code is adapted from the documentation 
// https://learn.microsoft.com/en-us/ef/core/get-started/overview/first-app?tabs=netcore-cli
using Microsoft.EntityFrameworkCore;

namespace Chirp.Infrastructure
{
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