using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Chirp.Infrastructure.test.Repositories
{
    public class CheepRepositoryUnitTests
    {
        private readonly ChirpDBContext chirpDbContext;
        private readonly IDbContext dbContext;

        public CheepRepositoryUnitTests()
        {
            var connection = new SqliteConnection($"DataSource=:memory:");
            connection.Open();

            this.chirpDbContext = new ChirpDBContext(
                new DbContextOptionsBuilder<ChirpDBContext>()
                    .UseSqlite(connection)
                    .Options
            );

            this.chirpDbContext.Database.EnsureCreated();

            this.dbContext = chirpDbContext;
        }

        [Fact]
        public async Task Create_With_NonExisting_Email_And_Name_Should_Create_And_Return_Author()
        {
            // Arrange
            var name = "John Doe";
            var email = "john.doe@example.com";

            var authorRepository = new AuthorRepository(this.chirpDbContext);

            // Act
            var author = await authorRepository.Create(name, email);
            await this.dbContext.SaveChanges();

            // Assert
            Assert.NotNull(author);
            Assert.Single(chirpDbContext.Authors);
            Assert.Equal(author.AuthorId, chirpDbContext.Authors.First().AuthorId);
            Assert.Equal(author.Name, chirpDbContext.Authors.First().Name);
            Assert.Equal(author.Email, chirpDbContext.Authors.First().Email);
            Assert.Equal(name, chirpDbContext.Authors.First().Name);
            Assert.Equal(email, chirpDbContext.Authors.First().Email);
        }

    }
}