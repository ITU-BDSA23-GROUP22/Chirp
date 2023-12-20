// ReferenceLink:
//  https://learn.microsoft.com/en-us/dotnet/core/testing/unit-testing-best-practices
//  https://softchris.github.io/pages/dotnet-moq.html#introduction

using Xunit;
using Moq;
using Microsoft.AspNetCore.Http;
using Chirp.Core;
using Chirp.Core.Services;

namespace Chirp.Web.Services
{
    public class PresentationServiceUnitTests
    {
        private readonly Mock<IHttpContextAccessor> httpContextAccessorMock;
        private readonly Mock<IChirpService> chirpServiceMock;
        private readonly HttpContext httpContext;

        public PresentationServiceUnitTests()
        {
            this.httpContextAccessorMock = new Mock<IHttpContextAccessor>();

            this.httpContext = new DefaultHttpContext();
            this.httpContextAccessorMock.SetupGet(x => x.HttpContext).Returns(httpContext);

            this.chirpServiceMock = new Mock<IChirpService>();
        }

        [Fact]
        public async Task GetAllCheepsViewModel_With_Invalid_PageNumber_Should_Throw()
        {
            // Arrange
            var pageNumber = -1;

            var chirpPresentationService = new PresentationService(httpContextAccessorMock.Object, chirpServiceMock.Object);

            // Act + Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(async () => await chirpPresentationService.GetAllCheepsViewModel(pageNumber));
        }

        [Fact]
        public async Task GetAllCheepsViewModel_With_Anonymous_Author_Should_Return_ViewModel()
        {
            // Arrange
            var pageNumber = 1;

            var authorId = Guid.NewGuid();
            var authorName = "author";

            var cheepId = Guid.NewGuid();
            var cheepText = "Hello";
            var cheepTimestamp = "15:30";

            var cheepDtos = new CheepDTO[]
            {
                new CheepDTO(
                    new AuthorDTO(authorId, authorName, Enumerable.Empty<Guid>()),
                    cheepId, cheepText, cheepTimestamp)
            };

            this.chirpServiceMock.Setup(x => x.GetAllCheeps(
                    pageNumber, 0, PresentationService.MAX_CHEEPS_PER_PAGE + 1
                )).Returns(Task.FromResult(cheepDtos.AsEnumerable()));

            var chirpPresentationService = new PresentationService(httpContextAccessorMock.Object, chirpServiceMock.Object);

            // Act
            var actual = await chirpPresentationService.GetAllCheepsViewModel(pageNumber);

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(pageNumber, actual.PageNumber);
            Assert.Equal(cheepDtos.Count(), actual.Cheeps.Count());
        }

        [Fact]
        public async Task GetCheepsByAuthorsViewModel_With_Invalid_PageNumber_Should_Throw()
        {
            // Arrange
            var pageNumber = -1;
            var pageUrl = "url";
            var authorIds = Enumerable.Empty<Guid>();

            var chirpPresentationService = new PresentationService(httpContextAccessorMock.Object, chirpServiceMock.Object);

            // Act + Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(async () => await chirpPresentationService.GetCheepsByAuthorsViewModel(authorIds, pageNumber, pageUrl));
        }

        [Fact]
        public async Task GetCheepsByAuthorsViewModel_With_Anonymous_Author_Should_Return_ViewModel()
        {
            // Arrange
            var pageNumber = 1;
            var pageUrl = "url";

            var authorId = Guid.NewGuid();
            var authorName = "author";

            var cheepId = Guid.NewGuid();
            var cheepText = "Hello";
            var cheepTimestamp = "15:30";

            var cheepDtos = new CheepDTO[]
            {
                new CheepDTO(
                    new AuthorDTO(authorId, authorName, Enumerable.Empty<Guid>()),
                    cheepId, cheepText, cheepTimestamp)
            };

            var authorIds = new Guid[] { authorId };

            this.chirpServiceMock.Setup(x => x.GetCheepsByAuthors(
                    authorIds, pageNumber, 0, PresentationService.MAX_CHEEPS_PER_PAGE + 1
                )).Returns(Task.FromResult(cheepDtos.AsEnumerable()));

            var chirpPresentationService = new PresentationService(httpContextAccessorMock.Object, chirpServiceMock.Object);

            // Act
            var actual = await chirpPresentationService.GetCheepsByAuthorsViewModel(authorIds, pageNumber, pageUrl);

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(pageUrl, actual.PageUrl);
            Assert.Equal(pageNumber, actual.PageNumber);
            Assert.Equal(cheepDtos.Count(), actual.Cheeps.Count());
        }

        [Fact]
        public async Task CreateCheep_With_Anonymous_Author_Should_Thorw()
        {
            // Arrange
            var cheepText = "Hello";

            var chirpPresentationService = new PresentationService(httpContextAccessorMock.Object, chirpServiceMock.Object);

            // Act + assert
            var exception = await Assert.ThrowsAsync<Exception>(async () => await chirpPresentationService.CreateCheep(cheepText));
        }

        [Fact]
        public async Task CreateCheep_With_Authenticated_Author_Should_Create_Cheep_For_Authenticated_Author()
        {
            // Arrange
            var cheepText = "Hello";

            var authorId = Guid.NewGuid();
            var authorName = "author";
            this.SetAuthenticatedUser(authorId, authorName);

            var chirpPresentationService = new PresentationService(httpContextAccessorMock.Object, chirpServiceMock.Object);

            // Act
            await chirpPresentationService.CreateCheep(cheepText);

            // Assert
            chirpServiceMock.Verify(x => x.CreateCheep(authorId, cheepText), Times.Once);
        }

        [Fact]
        public async Task FollowAuthor_With_Anonymous_Author_Should_Thorw()
        {
            // Arrange
            var authorToFollowId = Guid.NewGuid();

            var chirpPresentationService = new PresentationService(httpContextAccessorMock.Object, chirpServiceMock.Object);

            // Act + assert
            var exception = await Assert.ThrowsAsync<Exception>(async () => await chirpPresentationService.FollowAuthor(authorToFollowId));
        }

        [Fact]
        public async Task FollowAuthor_With_Authenticated_Author_Should_Follow_Author_For_Authenticated_Author()
        {
            // Arrange
            var authorToFollowId = Guid.NewGuid();

            var authorId = Guid.NewGuid();
            var authorName = "author";
            this.SetAuthenticatedUser(authorId, authorName);

            var chirpPresentationService = new PresentationService(httpContextAccessorMock.Object, chirpServiceMock.Object);

            // Act
            await chirpPresentationService.FollowAuthor(authorToFollowId);

            // Assert
            chirpServiceMock.Verify(x => x.FollowAuthor(authorId, authorToFollowId), Times.Once);
        }

        [Fact]
        public async Task UnfollowAuthor_With_Anonymous_Author_Should_Thorw()
        {
            // Arrange
            var authorToFollowId = Guid.NewGuid();

            var chirpPresentationService = new PresentationService(httpContextAccessorMock.Object, chirpServiceMock.Object);

            // Act + assert
            var exception = await Assert.ThrowsAsync<Exception>(async () => await chirpPresentationService.UnfollowAuthor(authorToFollowId));
        }

        [Fact]
        public async Task UnfollowAuthor_With_Authenticated_Author_Should_Unfollow_Author_For_Authenticated_Author()
        {
            // Arrange
            var authorToFollowId = Guid.NewGuid();

            var authorId = Guid.NewGuid();
            var authorName = "author";
            this.SetAuthenticatedUser(authorId, authorName);

            var chirpPresentationService = new PresentationService(httpContextAccessorMock.Object, chirpServiceMock.Object);

            // Act
            await chirpPresentationService.FollowAuthor(authorToFollowId);

            // Assert
            chirpServiceMock.Verify(x => x.FollowAuthor(authorId, authorToFollowId), Times.Once);
        }


        [Fact]
        public async Task GetAuthor_With_Id_Should_Return_Author()
        {
            // Arrange
            var authorId = Guid.NewGuid();
            var authorName = "author";

            this.chirpServiceMock.Setup(x => x.GetAuthor(authorId))
                .Returns(Task.FromResult((AuthorDTO?)
                    new AuthorDTO(authorId, authorName, Enumerable.Empty<Guid>())
                    ));

            var chirpPresentationService = new PresentationService(httpContextAccessorMock.Object, chirpServiceMock.Object);

            // Act
            var author = await chirpPresentationService.GetAuthor(authorId);

            // Assert
            Assert.NotNull(author);
            Assert.Equal(authorId, author.Id);
            Assert.Equal(authorName, author.Name);
        }

        [Fact]
        public async Task GetOrCreateAuthor_With_Existing_Author_Should_Return_Author()
        {
            // Arrange
            var authorId = Guid.NewGuid();
            var authorName = "author";

            this.chirpServiceMock.Setup(x => x.GetAuthor(authorId))
                .Returns(Task.FromResult((AuthorDTO?)
                    new AuthorDTO(authorId, authorName, Enumerable.Empty<Guid>())
                    ));

            var chirpPresentationService = new PresentationService(httpContextAccessorMock.Object, chirpServiceMock.Object);

            // Act
            var author = await chirpPresentationService.GetOrCreateAuthor(authorId, authorName);

            // Assert
            Assert.NotNull(author);
            Assert.Equal(authorId, author.Id);
            Assert.Equal(authorName, author.Name);
        }

        [Fact]
        public async Task GetOrCreateAuthor_With_None_Existing_Author_Should_Create_And_Return_Author()
        {
            // Arrange
            var authorId = Guid.NewGuid();
            var authorName = "author";

            this.chirpServiceMock.Setup(x => x.GetAuthor(authorId))
                .Returns(Task.FromResult((AuthorDTO?)null));

            this.chirpServiceMock.Setup(x => x.CreateAuthor(authorId, authorName))
                .Returns(Task.FromResult(
                    new AuthorDTO(authorId, authorName, Enumerable.Empty<Guid>())
                    ));

            var chirpPresentationService = new PresentationService(httpContextAccessorMock.Object, chirpServiceMock.Object);

            // Act
            var author = await chirpPresentationService.GetOrCreateAuthor(authorId, authorName);

            // Assert
            Assert.NotNull(author);
            Assert.Equal(authorId, author.Id);
            Assert.Equal(authorName, author.Name);
        }

        [Fact]
        public async Task GetOrCreateAuthor_With_None_Existing_Author_And_Failed_To_Create_Should_Throw()
        {
            // Arrange
            var authorId = Guid.NewGuid();
            var authorName = "author";

            var chirpPresentationService = new PresentationService(httpContextAccessorMock.Object, chirpServiceMock.Object);

            // Act + Assert
            var exception = await Assert.ThrowsAsync<Exception>(async () => await chirpPresentationService.GetOrCreateAuthor(authorId, authorName));
        }

        [Fact]
        public void GetAuthenticatedAuthor_With_Authenticated_Author_Should_Return_Author()
        {
            // Arrange
            var authorId = Guid.NewGuid();
            var authorName = "author";
            SetAuthenticatedUser(authorId, authorName);

            var chirpPresentationService = new PresentationService(httpContextAccessorMock.Object, chirpServiceMock.Object);

            // Act
            var author = chirpPresentationService.GetAuthenticatedAuthor();

            // Assert
            Assert.NotNull(author);
            Assert.Equal(authorId, author.Id);
            Assert.Equal(authorName, author.Name);
        }

        [Fact]
        public void GetAuthenticatedAuthor_With_Anonymous_Author_Should_Return_Null()
        {
            // Arrange
            var chirpPresentationService = new PresentationService(httpContextAccessorMock.Object, chirpServiceMock.Object);

            // Act
            var author = chirpPresentationService.GetAuthenticatedAuthor();

            // Assert
            Assert.Null(author);
        }

        private void SetAuthenticatedUser(Guid authorId, string authorName)
        {
            this.httpContext.User = new AuthorUser(this.httpContext.User, new AuthorDTO(authorId, authorName, Enumerable.Empty<Guid>()));
        }
    }
}

