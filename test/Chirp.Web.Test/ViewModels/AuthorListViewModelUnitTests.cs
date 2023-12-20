using Xunit;
using Chirp.Web.ViewModels;
using Chirp.Core;

namespace Chirp.Web.Tests.ViewModels
{
    public class AuthorListViewModelUnitTests
    {
        public AuthorListViewModelUnitTests()
        {

        }

        [Fact]
        public void Constructor_With_Anonymous_Author_And_Empty_Authors_Should_Return_ViewModel_With_Empty_Authors()
        {
            // Arrange
            var authenticatedAuthor = (AuthorDTO?)null;
            var pageNumber = 1;
            var authorsPerPage = 10;
            var pageUrl = "url";
            var searchText = "";

            var authors = new AuthorDTO[] { };

            // Act
            var actual = new AuthorListViewModel(authenticatedAuthor, authors, pageNumber, authorsPerPage, pageUrl, searchText);

            // Assert
            Assert.Empty(actual.Authors);
            Assert.Equal(pageUrl, actual.PageUrl);
            Assert.Equal(pageNumber, actual.PageNumber);
        }

        [Fact]
        public void Constructor_With_Anonymous_Author_And_Authors_Should_Return_ViewModel_With_Authors()
        {
            // Arrange
            var authenticatedAuthor = (AuthorDTO?)null;
            var pageNumber = 1;
            var authorsPerPage = 10;
            var pageUrl = "url";
            var searchText = "";

            var authorId = Guid.NewGuid();
            var authorName = "author";

            var authors = new AuthorDTO[]
            {                
                new AuthorDTO(
                    authorId,
                    authorName,
                    Enumerable.Empty<Guid>()
                    ),
            };

            // Act
            var actual = new AuthorListViewModel(authenticatedAuthor, authors, pageNumber, authorsPerPage, pageUrl, searchText);

            // Assert
            Assert.Equal(authors.Count(), actual.Authors.Count());
        }

        [Fact]
        public void Constructor_With_Anonymous_Author_And_More_Authors_Than_One_Page_Should_Return_ViewModel_With_Only_Authors_For_Page()
        {
            // Arrange
            var authenticatedAuthor = (AuthorDTO?)null;
            var pageNumber = 1;
            var authorsPerPage = 10;
            var pageUrl = "url";
            var searchText = "";

            var authorId = Guid.NewGuid();
            var authorName = "author";

            var authors = new List<AuthorDTO>();
            for (var i = 0; i < authorsPerPage + 1; i++)
            {
                authors.Add(new AuthorDTO(
                            authorId,
                            authorName,
                            Enumerable.Empty<Guid>()
                            ));
            }

            // Act
            var actual = new AuthorListViewModel(authenticatedAuthor, authors, pageNumber, authorsPerPage, pageUrl, searchText);

            // Assert
            Assert.Equal(authorsPerPage, actual.Authors.Count());
            Assert.Empty(actual.NavigateToPreviousPageUrl);
            Assert.Equal("url?page=2", actual.NavigateToNextPageUrl);
        }

        [Fact]
        public void Constructor_With_Anonymous_Author_And_Authors_For_One_Page_Should_Return_ViewModel_With_Authors_For_Page()
        {
            // Arrange
            var authenticatedAuthor = (AuthorDTO?)null;
            var pageNumber = 1;
            var authorsPerPage = 10;
            var pageUrl = "url";
            var searchText = "";

            var authorId = Guid.NewGuid();
            var authorName = "author";

            var authors = new List<AuthorDTO>();
            for (var i = 0; i < (authorsPerPage); i++)
            {
                authors.Add(new AuthorDTO(
                            authorId,
                            authorName,
                            Enumerable.Empty<Guid>()
                            ));
            }

            // Act
            var actual = new AuthorListViewModel(authenticatedAuthor, authors, pageNumber, authorsPerPage, pageUrl, searchText);

            // Assert
            Assert.Equal(authorsPerPage, actual.Authors.Count());
            Assert.Empty(actual.NavigateToPreviousPageUrl);
            Assert.Empty(actual.NavigateToNextPageUrl);
        }

        [Fact]
        public void Constructor_With_Anonymous_Author_And_Authors_For_One_Page_And_Not_PageNumber_One_Should_Return_ViewModel_With_Authors_For_Page()
        {
            // Arrange
            var authenticatedAuthor = (AuthorDTO?)null;
            var pageNumber = 2;
            var authorsPerPage = 10;
            var pageUrl = "url";
            var searchText = "";

            var authorId = Guid.NewGuid();
            var authorName = "author";

            var authors = new List<AuthorDTO>();
            for (var i = 0; i < (authorsPerPage); i++)
            {
                authors.Add(new AuthorDTO(
                            authorId,
                            authorName,
                            Enumerable.Empty<Guid>()
                        ));
            }

            // Act
            var actual = new AuthorListViewModel(authenticatedAuthor, authors, pageNumber, authorsPerPage, pageUrl, searchText);

            // Assert
            Assert.Equal(authorsPerPage, actual.Authors.Count());
            Assert.Equal("url?page=1", actual.NavigateToPreviousPageUrl);
            Assert.Empty(actual.NavigateToNextPageUrl);
        }

        [Fact]
        public void Constructor_With_Authenticated_Author_Should_Return_ViewModel_With_Authors_()
        {
            // Arrange
            var pageNumber = 1;
            var authorsPerPage = 10;
            var pageUrl = "url";
            var searchText = "";

            var authorId = Guid.NewGuid();
            var authorName = "author";

            var authenticatedAuthor = new AuthorDTO(
                Guid.NewGuid(),
                "authenticatedAuthor",
                new Guid[] { authorId });

            var authors = new AuthorDTO[]
            {
                new AuthorDTO(
                    authorId,
                    authorName,
                    Enumerable.Empty<Guid>()
                    )   
            };

            // Act
            var actual = new AuthorListViewModel(authenticatedAuthor, authors, pageNumber, authorsPerPage, pageUrl, searchText);

            // Assert
            Assert.Equal(authors.Count(), actual.Authors.Count());

            var author = authors.First();
            var authorViewModel = actual.Authors.First();
            Assert.Equal(author.Id, authorViewModel.Id);
            Assert.Equal(author.Name, authorViewModel.Name);
        }
    }
}
