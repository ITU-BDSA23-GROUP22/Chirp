using Xunit;
using Moq;
using Chirp.Infrastructure.Services;
using Chirp.Core;

namespace Chirp.Infrastructure.test.Services
{
    public class ChirpServiceUnitTests
    {
        private readonly Mock<ICheepRepository> cheepRepositoryMock;
        private readonly Mock<IAuthorRepository> authorRepositoryMock;
        private readonly Mock<IDbContext> dbContextMock;

        public ChirpServiceUnitTests()
        {
            this.cheepRepositoryMock = new Mock<ICheepRepository>();
            this.authorRepositoryMock = new Mock<IAuthorRepository>();
            this.dbContextMock = new Mock<IDbContext>();
        }

        [Fact]
        public async Task GetAuthor_With_Existing_Id_Sould_Return_Existing_Author()
        {
            // Arrange
            var authorId = Guid.NewGuid();
            var authorName = "name";
            var authorEmail = "email";

            this.authorRepositoryMock.Setup(x => x.Get(authorId))
                .Returns(
                    Task.FromResult(
                        (Author?)new Author
                        {
                            AuthorId = authorId,
                            Name = authorName,
                            Email = authorEmail,
                            Cheeps = Enumerable.Empty<Cheep>(),
                        }
                    ));

            var chirpService = new ChirpService(cheepRepositoryMock.Object, authorRepositoryMock.Object, dbContextMock.Object);

            // Act
            var authorDto = await chirpService.GetAuthor(authorId);

            // Assert
            Assert.NotNull(authorDto);
            Assert.Equal(authorId, authorDto.Id);
            Assert.Equal(authorName, authorDto.Name);
            Assert.Equal(authorEmail, authorDto.Email);
        }

        [Fact]
        public void GetAuthor_With_NonExisting_Id_Sould_Return_Throw()
        {
            // Arrange
            var authorId = Guid.NewGuid();

            this.authorRepositoryMock.Setup(x => x.Get(authorId))
                .Returns(
                    Task.FromResult(
                        (Author?)null
                    ));

            var chirpService = new ChirpService(cheepRepositoryMock.Object, authorRepositoryMock.Object, dbContextMock.Object);

            // Act + Assert
            var exception = Assert.ThrowsAsync<Exception>(async () => await chirpService.GetAuthor(authorId));
        }

        [Fact]
        public async Task CreateCheep_With_ExistingAuthor_Should_Create_Cheep()
        {
            // Arrange
            var authorId = Guid.NewGuid();
            var authorName = "name";
            var authorEmail = "email";

            var authorDto = new AuthorDTO(authorId, authorName, authorEmail, new List<Guid>());

            this.authorRepositoryMock.Setup(x => x.Get(authorEmail))
                .Returns(
                    Task.FromResult(
                        (Author?)new Author
                        {
                            AuthorId = authorId,
                            Name = authorName,
                            Email = authorEmail,
                            Cheeps = Enumerable.Empty<Cheep>(),
                        }
                    ));

            var cheepText = "hello";

            var chirpService = new ChirpService(cheepRepositoryMock.Object, authorRepositoryMock.Object, dbContextMock.Object);

            // Act
            await chirpService.CreateCheep(authorDto, cheepText);

            // Assert
            this.cheepRepositoryMock.Verify(x => x.Create(
                It.Is<Author>(x =>
                    x.AuthorId == authorId &&
                    x.Name == authorName &&
                    x.Email == authorEmail),
                cheepText,
                It.IsAny<DateTime>()), Times.Once);

            this.dbContextMock.Verify(x => x.SaveChanges(), Times.Once);
        }

        [Fact]
        public async Task CreateCheep_With_NoneExistingAuthor_Should_Create_Author_And_Cheep()
        {
            // Arrange
            var authorId = Guid.NewGuid();
            var authorName = "name";
            var authorEmail = "email";

            var authorDto = new AuthorDTO(authorId, authorName, authorEmail, new List<Guid>());

            this.authorRepositoryMock.Setup(x => x.Get(authorEmail))
                .Returns(
                    Task.FromResult(
                        (Author?)null
                    ));

            this.authorRepositoryMock.Setup(x => x.Create(authorName, authorEmail))
                .Returns(
                    Task.FromResult(
                        new Author
                        {
                            AuthorId = authorId,
                            Name = authorName,
                            Email = authorEmail,
                            Cheeps = Enumerable.Empty<Cheep>(),
                        }
                    ));

            var cheepText = "hello";

            var chirpService = new ChirpService(cheepRepositoryMock.Object, authorRepositoryMock.Object, dbContextMock.Object);

            // Act
            await chirpService.CreateCheep(authorDto, cheepText);

            // Assert
            this.authorRepositoryMock.Verify(x => x.Create(authorName, authorEmail), Times.Once);

            this.cheepRepositoryMock.Verify(x => x.Create(
                It.Is<Author>(x =>
                    x.AuthorId == authorId &&
                    x.Name == authorName &&
                    x.Email == authorEmail),
                cheepText,
                It.IsAny<DateTime>()), Times.Once);

            this.dbContextMock.Verify(x => x.SaveChanges(), Times.Once);
        }

        [Fact]
        public async Task GetAllCheeps_With_Existing_Cheep_And_Author_Should_Return_CheepDto_Enumeration()
        {
            // Arrange
            var pageNumber = 1;

            var author = new Author
            {
                AuthorId = Guid.NewGuid(),
                Name = "name",
                Email = "email",
                Cheeps = Enumerable.Empty<Cheep>(),
            };

            var cheep = new Cheep
            {
                CheepId = Guid.NewGuid(),
                AuthorId = author.AuthorId,
                Author = author,
                Text = "hello",
                TimeStamp = DateTime.Now,
            };

            this.authorRepositoryMock.Setup(x => x.Get(author.AuthorId))
                .Returns(
                    Task.FromResult(
                        (Author?)author)
                    );


            this.cheepRepositoryMock.Setup(x => x.GetAll(pageNumber))
                .Returns(
                Task.FromResult(new[] { cheep }
                    .AsEnumerable()));

            var chirpService = new ChirpService(cheepRepositoryMock.Object, authorRepositoryMock.Object, dbContextMock.Object);

            // Act
            var actual = await chirpService.GetAllCheeps(pageNumber);

            // Arrange
            Assert.Single(actual);
            Assert.Single(actual.Where(cheepDto =>
                cheepDto.Id == cheep.CheepId &&
                cheepDto.Message == cheep.Text &&
                cheepDto.Timestamp == cheep.TimeStamp.ToString() &&
                cheepDto.Author.Id == author.AuthorId &&
                cheepDto.Author.Name == author.Name &&
                cheepDto.Author.Email == author.Email));
        }

        [Fact]
        public void GetAllCheeps_With_Existing_Cheep_And_NoneExisting_Author_Should_Throw()
        {
            // Arrange
            var pageNumber = 1;

            var author = new Author
            {
                AuthorId = Guid.NewGuid(),
                Name = "name",
                Email = "email",
                Cheeps = Enumerable.Empty<Cheep>(),
            };

            var cheep = new Cheep
            {
                CheepId = Guid.NewGuid(),
                AuthorId = author.AuthorId,
                Author = author,
                Text = "hello",
                TimeStamp = DateTime.Now,
            };

            this.authorRepositoryMock.Setup(x => x.Get(author.AuthorId))
                .Throws<Exception>();

            this.cheepRepositoryMock.Setup(x => x.GetAll(pageNumber))
                .Returns(
                Task.FromResult(new[] { cheep }
                    .AsEnumerable()));

            var chirpService = new ChirpService(cheepRepositoryMock.Object, authorRepositoryMock.Object, dbContextMock.Object);

            // Act + Assert
            var exception = Assert.ThrowsAsync<Exception>(async () => await chirpService.GetAllCheeps(pageNumber));
        }

        [Fact]
        public async Task GetCheepsByAuthor_With_Existing_Cheep_And_Author_Should_Return_CheepDto_Enumeration()
        {
            // Arrange
            var pageNumber = 1;

            var author = new Author
            {
                AuthorId = Guid.NewGuid(),
                Name = "name",
                Email = "email",
                Cheeps = Enumerable.Empty<Cheep>(),
            };

            var cheep = new Cheep
            {
                CheepId = Guid.NewGuid(),
                AuthorId = author.AuthorId,
                Author = author,
                Text = "hello",
                TimeStamp = DateTime.Now,
            };

            this.authorRepositoryMock.Setup(x => x.Get(author.AuthorId))
                .Returns(
                    Task.FromResult(
                        (Author?)author)
                    );


            this.cheepRepositoryMock.Setup(x => x.GetByAuthor(author, pageNumber))
                .Returns(
                Task.FromResult(new[] { cheep }
                    .AsEnumerable()));

            var chirpService = new ChirpService(cheepRepositoryMock.Object, authorRepositoryMock.Object, dbContextMock.Object);

            // Act
            var actual = await chirpService.GetCheepsByAuthor(author.AuthorId, pageNumber);

            // Arrange
            Assert.Single(actual);
            Assert.Single(actual.Where(cheepDto =>
                cheepDto.Id == cheep.CheepId &&
                cheepDto.Message == cheep.Text &&
                cheepDto.Timestamp == cheep.TimeStamp.ToString() &&
                cheepDto.Author.Id == author.AuthorId &&
                cheepDto.Author.Name == author.Name &&
                cheepDto.Author.Email == author.Email));
        }

        [Fact]
        public void GetCheepsByAuthor_With_NoneExisting_Author_Should_Throw()
        {
            // Arrange
            var pageNumber = 1;

            var author = new Author
            {
                AuthorId = Guid.NewGuid(),
                Name = "name",
                Email = "email",
                Cheeps = Enumerable.Empty<Cheep>(),
            };

            this.authorRepositoryMock.Setup(x => x.Get(author.AuthorId))
                .Throws<Exception>();

            var chirpService = new ChirpService(cheepRepositoryMock.Object, authorRepositoryMock.Object, dbContextMock.Object);

            // Act + Assert
            var exception = Assert.ThrowsAsync<Exception>(async () => await chirpService.GetCheepsByAuthor(author.AuthorId, pageNumber));
        }
    }
}