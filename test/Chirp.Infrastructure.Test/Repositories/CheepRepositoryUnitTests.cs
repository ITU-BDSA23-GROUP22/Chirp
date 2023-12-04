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
        public async Task Create_With_Existing_Author_And_Text_Timestamp_Should_Create_And_Return_Cheep()
        {
            // Arrange
            var name = "John Doe";
            var email = "john.doe@example.com";

            var authorRepository = new AuthorRepository(this.chirpDbContext);
            var author = await authorRepository.Create(name, email);

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
                Where(x => x.Name == name &&
                x.Email == email &&
                x.AuthorId == author.AuthorId));

            Assert.Single(chirpDbContext.Cheeps);
            Assert.Single(chirpDbContext.Cheeps
                .Where(x => x.AuthorId == author.AuthorId &&
                x.Text == cheepText &&
                x.TimeStamp == cheepTimeStamp));

        }


        /*
        [Fact]
        public void TestCheepsByAuthor()
        {
            CheepRepository repo = new CheepRepository();

            string name = "John Doe";
            string email = "john.doe@example.com";
            repo.AddAuthor(name, email);
            AuthorDTO author = repo.GetAuthor(name);
            repo.WriteCheep("hello hello", DateTime.Now, author);
            Assert.Single(repo.GetCheepsByAuthor(author));
        }

        [Fact]
        public void TestGetAllCheeps()
        {
            CheepRepository repo = new CheepRepository();

            string name = "John Doe";
            string email = "john.doe@example.com";
            repo.AddAuthor(name, email);
            AuthorDTO author = repo.GetAuthor(name);
            for (int i = 0; i < 32; i++)
            {
                repo.WriteCheep("Test cheep " + i, DateTime.Now, author);
            }

            int index = 0;
            foreach (CheepDTO loopCheep in repo.GetAllCheeps(0))
            {
                Assert.Equals(loopCheep.message, "Test cheep " + index);
                index++;
            }
            Assert.Single(repo.GetCheepsByAuthor(author));
        }

        public void TestCheepById()
        {

        }

        public void TestAuthorById()
        {

        }
        */

    }
}