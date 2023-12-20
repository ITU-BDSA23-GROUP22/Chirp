// ReferenceLink:
//  https://learn.microsoft.com/en-us/ef/core/testing/testing-without-the-database
//  https://learn.microsoft.com/en-us/dotnet/core/testing/unit-testing-best-practices

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
        public async Task Create_With_Existing_Author_And_Text_Timestamp_Should_Create_And_Return_Cheep()
        {
            // Arrange
            var authorId = Guid.NewGuid();
            var auhtorName = "John Doe";

            var authorRepository = new AuthorRepository(this.chirpDbContext);
            var author = await authorRepository.Create(authorId, auhtorName);

            var cheepText = "hello hello";
            var cheepTimeStamp = DateTime.Now;

            var cheepRepository = new CheepRepository(this.chirpDbContext);

            // Act
            var cheep = await cheepRepository.Create(author, cheepText, cheepTimeStamp);
            await dbContext.SaveChanges();

            // Aassert
            Assert.NotNull(cheep);
            Assert.Single(chirpDbContext.Authors);
            Assert.Single(chirpDbContext.Authors.
                Where(x => x.Name == auhtorName &&
                x.AuthorId == author.AuthorId));

            Assert.Single(chirpDbContext.Cheeps);
            Assert.Single(chirpDbContext.Cheeps
                .Where(x => x.AuthorId == author.AuthorId &&
                x.Text == cheepText &&
                x.TimeStamp == cheepTimeStamp));

        }

        [Fact]
        public async Task GetAll_With_Page_1_Should_Get_Cheeps_For_Page()
        {
            // Arrange
            var authorId = Guid.NewGuid();
            var authorName = "John Doe";

            var authorRepository = new AuthorRepository(this.chirpDbContext);
            var author = await authorRepository.Create(authorId, authorName);

            var cheepText = "hello hello";
            var cheepTimeStamp = DateTime.Now;

            var cheepRepository = new CheepRepository(this.chirpDbContext);

            var cheep = await cheepRepository.Create(author, cheepText, cheepTimeStamp);
            await dbContext.SaveChanges();

            // Act
            var cheeps = await cheepRepository.GetAll(1, 0, 10);


            // Aassert
            Assert.Single(cheeps);
            Assert.Equal(author.AuthorId, cheeps.First().Author.AuthorId);
            Assert.Equal(cheepText, cheeps.First().Text);

        }

        [Fact]
        public async Task GetByAuthors_With_Authors_Should_Return_Cheeps_From_Author()
        {
            // Arrange
            var authorId = Guid.NewGuid();
            var authorName = "John Doe";

            var authorRepository = new AuthorRepository(this.chirpDbContext);
            var author = await authorRepository.Create(authorId, authorName);

            var cheepText = "hello hello";
            var cheepTimeStamp = DateTime.Now;

            var cheepRepository = new CheepRepository(this.chirpDbContext);

            var cheep = await cheepRepository.Create(author, cheepText, cheepTimeStamp);
            await dbContext.SaveChanges();

            // Act
            var cheeps = await cheepRepository.GetByAuthors(new Guid[] { authorId }, 1, 0, 10);


            // Aassert
            Assert.Single(cheeps);
            Assert.Equal(author.AuthorId, cheeps.First().Author.AuthorId);
            Assert.Equal(cheepText, cheeps.First().Text);
        }

        [Fact]
        public async Task GetByAuthors_With_Authors_Should_Not_Return_Cheeps_From_Other_Authors()
        {
            // Arrange
            var authorId1 = Guid.NewGuid();
            var authorName1 = "John Doe";

            var authorId2 = Guid.NewGuid();
            var authorName2 = "John Doe";

            var authorRepository = new AuthorRepository(this.chirpDbContext);
            var author1 = await authorRepository.Create(authorId1, authorName1);
            var author2 = await authorRepository.Create(authorId2, authorName2);

            var cheepText1 = "hello hello";
            var cheepText2 = "bye bye";
            var cheepTimeStamp = DateTime.Now;

            var cheepRepository = new CheepRepository(this.chirpDbContext);

            var cheep1 = await cheepRepository.Create(author1, cheepText1, cheepTimeStamp);
            var cheep2 = await cheepRepository.Create(author2, cheepText2, cheepTimeStamp);
            await dbContext.SaveChanges();

            // Act
            var cheeps = await cheepRepository.GetByAuthors(new Guid[] { authorId1 }, 1, 0, 10);


            // Aassert
            Assert.Single(cheeps);
            Assert.Equal(author1.AuthorId, cheeps.First().Author.AuthorId);
            Assert.Equal(cheepText1, cheeps.First().Text);
            Assert.Empty(cheeps.Where(x => x.AuthorId != authorId1));
        }
    }
}