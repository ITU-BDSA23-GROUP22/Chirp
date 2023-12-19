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

        [Fact]
        public async Task Create_With_NonExisting_Id_And_Name_Should_Create_And_Return_Author()
        {
            // Arrange
            var authorId = Guid.NewGuid();
            var authorName = "John Doe";


            var authorRepository = new AuthorRepository(this.chirpDbContext);

            // Act
            var author = await authorRepository.Create(authorId, authorName);
            await this.dbContext.SaveChanges();

            // Assert
            Assert.NotNull(author);
            Assert.Single(chirpDbContext.Authors);
            Assert.Equal(author.AuthorId, chirpDbContext.Authors.First().AuthorId);
            Assert.Equal(author.Name, chirpDbContext.Authors.First().Name);
            Assert.Equal(authorName, chirpDbContext.Authors.First().Name);
            Assert.Equal(authorId, chirpDbContext.Authors.First().AuthorId);
        }

        [Fact]
        public async Task Create_With_Existing_Id_Should_Throw_Exception()
        {
            // Arrange
            var authorId1 = Guid.NewGuid();
            var auhtorName1 = "John Doe";

            var auhtorName2 = "Not John Doe";

            var authorRepository = new AuthorRepository(this.chirpDbContext);
            var author = await authorRepository.Create(authorId1, auhtorName1);
            await this.dbContext.SaveChanges();

            // Act and Assert
            var exception = await Assert.ThrowsAsync<Exception>(async () => await authorRepository.Create(authorId1, auhtorName2));
            Assert.Equal("Failed to create author - an author with authorId already exists", exception.Message);
        }

        [Fact]
        public async Task FollowAuthor_With_None_FollowedAuthor_Should_Create_AuthorAuthorRelation()
        {
            // Arrange
            var author1Id = Guid.NewGuid();
            var author1Name = "author1";
            var author2Id = Guid.NewGuid();
            var author2Name = "author2";
            var timestamp = DateTime.Now;

            var authorRepository = new AuthorRepository(this.chirpDbContext);

            var author1 = await authorRepository.Create(author1Id, author1Name);
            var author2 = await authorRepository.Create(author2Id, author2Name);
            await this.dbContext.SaveChanges();

            // Act
            await authorRepository.FollowAuthor(author1, author2, timestamp);
            await this.dbContext.SaveChanges();

            // Assert
            Assert.Equal(2, chirpDbContext.Authors.Count());
            Assert.Single(chirpDbContext.AuthorAuthorRelations);
            Assert.Single(chirpDbContext.AuthorAuthorRelations.Where(x =>
                x.AuthorId == author1.AuthorId &&
                x.AuthorToFollowId == author2.AuthorId));

            Assert.Single(chirpDbContext.Authors.Single(x => x.AuthorId == author1.AuthorId).Following);

            var author = chirpDbContext.Authors.Single(x => x.AuthorId == author1.AuthorId);
            Assert.Equal(author1.AuthorId, author.Following.First().AuthorId);
            Assert.Equal(author2.AuthorId, author.Following.First().AuthorToFollowId);
            Assert.Equal(timestamp, author.Following.First().TimeStamp);
        }

        [Fact]
        public async Task UnfollowAuthor_With_FollowedAuthor_Should_Delete_AuthorAuthorRelation()
        {
            // Arrange
            var author1Id = Guid.NewGuid();
            var author1Name = "author1";
            var author2Id = Guid.NewGuid();
            var author2Name = "author2";
            var timestamp = DateTime.Now;

            var authorRepository = new AuthorRepository(this.chirpDbContext);

            var author1 = await authorRepository.Create(author1Id, author1Name);
            var author2 = await authorRepository.Create(author2Id, author2Name);
            await authorRepository.FollowAuthor(author1, author2, timestamp);
            await this.dbContext.SaveChanges();

            // Act
            await authorRepository.UnfollowAuthor(author1, author2);
            await this.dbContext.SaveChanges();

            // Assert
            Assert.Equal(2, chirpDbContext.Authors.Count());
            Assert.Empty(chirpDbContext.AuthorAuthorRelations);

            var author = chirpDbContext.Authors.Single(x => x.AuthorId == author1.AuthorId);
            Assert.Empty(author.Following);
        }
    }
}