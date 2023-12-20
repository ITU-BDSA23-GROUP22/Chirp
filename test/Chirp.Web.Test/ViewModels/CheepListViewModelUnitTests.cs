// ReferenceLink:
//  https://learn.microsoft.com/en-us/dotnet/core/testing/unit-testing-best-practices

using Xunit;
using Chirp.Web.ViewModels;
using Chirp.Core;

namespace Chirp.Web.Tests.ViewModels
{
    public class CheepListViewModelUnitTests
    {
        public CheepListViewModelUnitTests()
        {

        }

        [Fact]
        public void Constructor_With_Anonymous_Author_And_Empty_Cheeps_Should_Return_ViewModel_With_Empty_Cheeps()
        {
            // Arrange
            var authenticatedAuthor = (AuthorDTO?)null;
            var pageNumber = 1;
            var cheepsPerPage = 10;
            var pageUrl = "url";

            var cheeps = new CheepDTO[] { };

            // Act
            var actual = new CheepListViewModel(authenticatedAuthor, cheeps, pageNumber, cheepsPerPage, pageUrl);

            // Assert
            Assert.Empty(actual.Cheeps);
            Assert.Equal(pageUrl, actual.PageUrl);
            Assert.Equal(pageNumber, actual.PageNumber);
        }

        [Fact]
        public void Constructor_With_Anonymous_Author_And_Cheeps_Should_Return_ViewModel_With_Cheeps()
        {
            // Arrange
            var authenticatedAuthor = (AuthorDTO?)null;
            var pageNumber = 1;
            var cheepsPerPage = 10;
            var pageUrl = "url";

            var authorId = Guid.NewGuid();
            var authorName = "author";

            var cheeps = new CheepDTO[]
            {
                new CheepDTO(
                    new AuthorDTO(
                        authorId,
                        authorName,
                        Enumerable.Empty<Guid>()
                        ),
                    Guid.NewGuid(),
                    "Hello",
                    "15:10")
            };

            // Act
            var actual = new CheepListViewModel(authenticatedAuthor, cheeps, pageNumber, cheepsPerPage, pageUrl);

            // Assert
            Assert.Equal(cheeps.Count(), actual.Cheeps.Count());
        }

        [Fact]
        public void Constructor_With_Anonymous_Author_And_More_Cheeps_Than_One_Page_Should_Return_ViewModel_With_Only_Cheeps_For_Page()
        {
            // Arrange
            var authenticatedAuthor = (AuthorDTO?)null;
            var pageNumber = 1;
            var cheepsPerPage = 10;
            var pageUrl = "url";

            var authorId = Guid.NewGuid();
            var authorName = "author";

            var cheeps = new List<CheepDTO>();
            for (var i = 0; i < cheepsPerPage + 1; i++)
            {
                cheeps.Add(new CheepDTO(
                    new AuthorDTO(
                        authorId,
                        authorName,
                        Enumerable.Empty<Guid>()
                        ),
                    Guid.NewGuid(),
                    $"Hello{i}",
                    $"15:{i}"
                    ));
            }

            // Act
            var actual = new CheepListViewModel(authenticatedAuthor, cheeps, pageNumber, cheepsPerPage, pageUrl);

            // Assert
            Assert.Equal(cheepsPerPage, actual.Cheeps.Count());
            Assert.Empty(actual.NavigateToPreviousPageUrl);
            Assert.Equal("url?page=2", actual.NavigateToNextPageUrl);
        }

        [Fact]
        public void Constructor_With_Anonymous_Author_And_Cheeps_For_One_Page_Should_Return_ViewModel_With_Cheeps_For_Page()
        {
            // Arrange
            var authenticatedAuthor = (AuthorDTO?)null;
            var pageNumber = 1;
            var cheepsPerPage = 10;
            var pageUrl = "url";

            var authorId = Guid.NewGuid();
            var authorName = "author";

            var cheeps = new List<CheepDTO>();
            for (var i = 0; i < (cheepsPerPage); i++)
            {
                cheeps.Add(new CheepDTO(
                    new AuthorDTO(
                        authorId,
                        authorName,
                        Enumerable.Empty<Guid>()
                        ),
                    Guid.NewGuid(),
                    $"Hello{i}",
                    $"15:{i}"
                    ));
            }

            // Act
            var actual = new CheepListViewModel(authenticatedAuthor, cheeps, pageNumber, cheepsPerPage, pageUrl);

            // Assert
            Assert.Equal(cheepsPerPage, actual.Cheeps.Count());
            Assert.Empty(actual.NavigateToPreviousPageUrl);
            Assert.Empty(actual.NavigateToNextPageUrl);
        }

        [Fact]
        public void Constructor_With_Anonymous_Author_And_Cheeps_For_One_Page_And_Not_PageNumber_One_Should_Return_ViewModel_With_Cheeps_For_Page()
        {
            // Arrange
            var authenticatedAuthor = (AuthorDTO?)null;
            var pageNumber = 2;
            var cheepsPerPage = 10;
            var pageUrl = "url";

            var authorId = Guid.NewGuid();
            var authorName = "author";

            var cheeps = new List<CheepDTO>();
            for (var i = 0; i < (cheepsPerPage); i++)
            {
                cheeps.Add(new CheepDTO(
                    new AuthorDTO(
                        authorId,
                        authorName,
                        Enumerable.Empty<Guid>()
                        ),
                    Guid.NewGuid(),
                    $"Hello{i}",
                    $"15:{i}"
                    ));
            }

            // Act
            var actual = new CheepListViewModel(authenticatedAuthor, cheeps, pageNumber, cheepsPerPage, pageUrl);

            // Assert
            Assert.Equal(cheepsPerPage, actual.Cheeps.Count());
            Assert.Equal("url?page=1", actual.NavigateToPreviousPageUrl);
            Assert.Empty(actual.NavigateToNextPageUrl);
        }

        [Fact]
        public void Constructor_With_Authenticated_Author_Should_Return_ViewModel_With_Cheeps_()
        {
            // Arrange
            var authenticatedAuthor = (AuthorDTO?)null;
            var pageNumber = 1;
            var cheepsPerPage = 10;
            var pageUrl = "url";

            var authorId = Guid.NewGuid();
            var authorName = "author";

            var cheeps = new CheepDTO[]
            {
                new CheepDTO(
                    new AuthorDTO(
                        authorId,
                        authorName,
                        Enumerable.Empty<Guid>()
                        ),
                    Guid.NewGuid(),
                    "Hello",
                    "15:10")
            };

            // Act
            var actual = new CheepListViewModel(authenticatedAuthor, cheeps, pageNumber, cheepsPerPage, pageUrl);

            // Assert
            Assert.Equal(cheeps.Count(), actual.Cheeps.Count());

            var cheep = cheeps.First();
            var cheepViewModel = actual.Cheeps.First();
            Assert.Equal(cheep.Id, cheepViewModel.Id);
            Assert.Equal(cheep.Message, cheepViewModel.Message);
            Assert.Equal(cheep.Timestamp, cheepViewModel.Timestamp);
            Assert.Equal(cheep.Author.Id, cheepViewModel.Author.Id);
            Assert.Equal(cheep.Author.Name, cheepViewModel.Author.Name);
        }
    }
}
