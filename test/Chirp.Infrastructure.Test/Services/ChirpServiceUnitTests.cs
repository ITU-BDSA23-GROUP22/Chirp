// ReferenceLink:
//  https://learn.microsoft.com/en-us/dotnet/core/testing/unit-testing-best-practices
//  https://softchris.github.io/pages/dotnet-moq.html#introduction


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
                            Cheeps = Enumerable.Empty<Cheep>(),
                            Following = Enumerable.Empty<AuthorAuthorRelation>()
                        }
                    ));

            var chirpService = new ChirpService(cheepRepositoryMock.Object, authorRepositoryMock.Object, dbContextMock.Object);

            // Act
            var authorDto = await chirpService.GetAuthor(authorId);

            // Assert
            Assert.NotNull(authorDto);
            Assert.Equal(authorId, authorDto.Id);
            Assert.Equal(authorName, authorDto.Name);
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

            var authorDto = new AuthorDTO(authorId, authorName, new List<Guid>());

            this.authorRepositoryMock.Setup(x => x.Get(authorId))
                .Returns(
                    Task.FromResult(
                        (Author?)new Author
                        {
                            AuthorId = authorId,
                            Name = authorName,
                            Cheeps = Enumerable.Empty<Cheep>(),
                            Following = Enumerable.Empty<AuthorAuthorRelation>()
                        }
                    ));

            var cheepText = "hello";

            var chirpService = new ChirpService(cheepRepositoryMock.Object, authorRepositoryMock.Object, dbContextMock.Object);

            // Act
            await chirpService.CreateCheep(authorId, cheepText);

            // Assert
            this.cheepRepositoryMock.Verify(x => x.Create(
                It.Is<Author>(x =>
                    x.AuthorId == authorId &&
                    x.Name == authorName),
                cheepText,
                It.IsAny<DateTime>()), Times.Once);

            this.dbContextMock.Verify(x => x.SaveChanges(), Times.Once);
        }

        [Fact]
        public async Task CreateCheep_With_NoneExistingAuthor_Should_Throw()
        {
            // Arrange
            var authorId = Guid.NewGuid();
            var authorName = "name";

            this.authorRepositoryMock.Setup(x => x.Get(authorId))
                .Returns(
                    Task.FromResult(
                        (Author?)null
                    ));

            this.authorRepositoryMock.Setup(x => x.Create(authorId, authorName))
                .Returns(
                    Task.FromResult(
                        new Author
                        {
                            AuthorId = authorId,
                            Name = authorName,
                        }
                    ));

            var cheepText = "hello";

            var chirpService = new ChirpService(cheepRepositoryMock.Object, authorRepositoryMock.Object, dbContextMock.Object);

            // Act + Assert
            await Assert.ThrowsAsync<Exception>(async () => await chirpService.CreateCheep(authorId, cheepText));
        }

        [Fact]
        public async Task GetAllCheeps_With_Existing_Cheep_And_Author_Should_Return_CheepDto_Enumeration()
        {
            // Arrange
            var pageNumber = 1;
            var cheepsPerPage = 10;

            var author = new Author
            {
                AuthorId = Guid.NewGuid(),
                Name = "name",
                Cheeps = Enumerable.Empty<Cheep>(),
                Following = Enumerable.Empty<AuthorAuthorRelation>()
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


            this.cheepRepositoryMock.Setup(x => x.GetAll(pageNumber, 0, cheepsPerPage))
                .Returns(
                Task.FromResult(new[] { cheep }
                    .AsEnumerable()));

            var chirpService = new ChirpService(cheepRepositoryMock.Object, authorRepositoryMock.Object, dbContextMock.Object);

            // Act
            var actual = await chirpService.GetAllCheeps(pageNumber, 0, cheepsPerPage);

            // Arrange
            Assert.Single(actual);
            Assert.Single(actual.Where(cheepDto =>
                cheepDto.Id == cheep.CheepId &&
                cheepDto.Message == cheep.Text &&
                cheepDto.Timestamp == cheep.TimeStamp.ToString() &&
                cheepDto.Author.Id == author.AuthorId &&
                cheepDto.Author.Name == author.Name));
        }

        [Fact]
        public void GetAllCheeps_With_Existing_Cheep_And_NoneExisting_Author_Should_Throw()
        {
            // Arrange
            var pageNumber = 1;
            var cheepsPerPage = 10;

            var author = new Author
            {
                AuthorId = Guid.NewGuid(),
                Name = "name",
                Cheeps = Enumerable.Empty<Cheep>(),
                Following = Enumerable.Empty<AuthorAuthorRelation>()
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

            this.cheepRepositoryMock.Setup(x => x.GetAll(pageNumber, 0, cheepsPerPage))
                .Returns(
                Task.FromResult(new[] { cheep }
                    .AsEnumerable()));

            var chirpService = new ChirpService(cheepRepositoryMock.Object, authorRepositoryMock.Object, dbContextMock.Object);

            // Act + Assert
            var exception = Assert.ThrowsAsync<Exception>(async () => await chirpService.GetAllCheeps(pageNumber, 0, cheepsPerPage));
        }

        [Fact]
        public async Task GetCheepsByAuthor_With_Existing_Cheep_And_Author_Should_Return_CheepDto_Enumeration()
        {
            // Arrange
            var pageNumber = 1;
            var cheepsPerPage = 10;

            var author = new Author
            {
                AuthorId = Guid.NewGuid(),
                Name = "name",
                Cheeps = Enumerable.Empty<Cheep>(),
                Following = Enumerable.Empty<AuthorAuthorRelation>()
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


            this.cheepRepositoryMock.Setup(x => x.GetByAuthors(new Guid[] { author.AuthorId }, pageNumber, 0, cheepsPerPage))
               .Returns(
               Task.FromResult(new[] { cheep }
                   .AsEnumerable()));

            var chirpService = new ChirpService(cheepRepositoryMock.Object, authorRepositoryMock.Object, dbContextMock.Object);

            // Act
            var actual = await chirpService.GetCheepsByAuthors(new Guid[] { author.AuthorId }, pageNumber, 0, cheepsPerPage);

            // Arrange
            Assert.Single(actual);
            Assert.Single(actual.Where(cheepDto =>
               cheepDto.Id == cheep.CheepId &&
               cheepDto.Message == cheep.Text &&
               cheepDto.Timestamp == cheep.TimeStamp.ToString() &&
               cheepDto.Author.Id == author.AuthorId &&
               cheepDto.Author.Name == author.Name));
        }

        [Fact]
        public void GetCheepsByAuthor_With_NoneExisting_Author_Should_Throw()
        {
            // Arrange
            var pageNumber = 1;
            var cheepsPerPage = 10;


            var author = new Author
            {
                AuthorId = Guid.NewGuid(),
                Name = "name",
                Cheeps = Enumerable.Empty<Cheep>(),
                Following = Enumerable.Empty<AuthorAuthorRelation>()
            };

            this.authorRepositoryMock.Setup(x => x.Get(author.AuthorId))
                .Throws<Exception>();

            var chirpService = new ChirpService(cheepRepositoryMock.Object, authorRepositoryMock.Object, dbContextMock.Object);

            // Act + Assert
            var exception = Assert.ThrowsAsync<Exception>(async () => await chirpService.GetCheepsByAuthors(new Guid[] { author.AuthorId }, pageNumber, 0, cheepsPerPage));
        }

        [Fact]
        public async Task SearchAuthors_With_SearchText_Should_Return_AuthorDto_Enumeration()
        {
            // Arrange
            var pageNumber = 1;
            var authorsPerPage = 10;

            var timeStamp = DateTime.Now;

            var author1 = new Author
            {
                AuthorId = Guid.NewGuid(),
                Name = "name1",
            };
            var author2 = new Author
            {
                AuthorId = Guid.NewGuid(),
                Name = "name2",
            };

            author1.Following = new List<AuthorAuthorRelation>{
                new AuthorAuthorRelation
                {
                    Author = author1,
                    AuthorId = author1.AuthorId,
                    AuthorToFollow = author2,
                    AuthorToFollowId = author2.AuthorId,
                    TimeStamp = timeStamp
                }
            };

            var searchText = "";
            this.authorRepositoryMock.Setup(x => x.SearchAuthor(searchText, pageNumber, 0, authorsPerPage))
               .Returns(
               Task.FromResult(new[] { author1 }
                   .AsEnumerable()));

            var chirpService = new ChirpService(cheepRepositoryMock.Object, authorRepositoryMock.Object, dbContextMock.Object);


            // Act
            var actual = await chirpService.SearchAuthors(searchText, pageNumber, 0, authorsPerPage);

            // Arrange
            Assert.Single(actual);
            Assert.Single(actual.Where(authorDto =>
                authorDto.Id == author1.AuthorId &&
                authorDto.Name == author1.Name &&
                authorDto.followingIds.Any(x => x == author2.AuthorId)));
        }
    }
}