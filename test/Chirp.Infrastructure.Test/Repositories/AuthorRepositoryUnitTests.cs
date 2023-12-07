using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Chirp.Infrastructure.test.Repositories
{
    public class AuthorRepositoryUnitTests
    {
        private readonly ChirpDBContext chirpDbContext;
        private readonly IDbContext dbContext;

        public AuthorRepositoryUnitTests()
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

        [Fact(Skip = "Skal refaktoriseres")]
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

        [Fact(Skip = "Skal refaktoriseres")]
        public async Task Create_With_Existing_Email_Should_Throw_Exception()
        {
            // Arrange
            var name = "John Doe";
            var email = "john.doe@example.com";
            var name1 = "Not John Doe";
            var email1 = "john.doe@example.com";

            var authorRepository = new AuthorRepository(this.chirpDbContext);
            var author = await authorRepository.Create(name, email);
            await this.dbContext.SaveChanges();

            // Act and Assert
            var exception = await Assert.ThrowsAsync<Exception>(async () => await authorRepository.Create(name1, email1));
            Assert.Equal($"Failed to create author - an author with email [{email1}] already exists", exception.Message);
        }

        [Fact(Skip = "Skal refaktoriseres")]
        public async Task TestDBSETs()
        {
            // Arrange
            var name1 = "author1";
            var email1 = "author1@example.com";
            var name2 = "author2";
            var email2 = "author2@example.com";

            var authorRepository = new AuthorRepository(this.chirpDbContext);
            var author1 = await authorRepository.Create(name1, email1);
            var author2 = await authorRepository.Create(name2, email2);
            await this.dbContext.SaveChanges();

            // Act 
            //await this.chirpDbContext.AuthorAuthorRelations.AddAsync(new AuthorAuthorRelation
            //{
            //    Author = author1,
            //    AuthorId = author1.AuthorId,
            //    AuthorToFollow = author2,
            //    AuthorToFollowId = author2.AuthorId,
            //    TimeStamp = DateTime.UtcNow
            //});

            //await authorRepository.FollowAuthor(author1, author2);

            await this.dbContext.SaveChanges();

            var author3 = await this.chirpDbContext.Authors.SingleAsync(x => x.AuthorId == author1.AuthorId);

            //this.chirpDbContext.AuthorAuthorRelations.Remove(author3.Following.First());

            await authorRepository.UnfollowAuthor(author1, author2);

            await this.dbContext.SaveChanges();


            var author4 = await this.chirpDbContext.Authors.SingleAsync(x => x.AuthorId == author1.AuthorId);

            var etellernandet = this.chirpDbContext.AuthorAuthorRelations.ToArray();
        }
    }
}