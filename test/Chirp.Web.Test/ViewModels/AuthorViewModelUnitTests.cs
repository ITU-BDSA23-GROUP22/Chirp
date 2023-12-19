using Xunit;
using Chirp.Web.ViewModels;
using Chirp.Core;

namespace Chirp.Web.Tests.ViewModels
{
    public class AuthorViewModelUnitTests
    {
        public AuthorViewModelUnitTests()
        {
        }

        [Fact]
        public void Constructor_With_Anonymous_Author_Should_Return_ViewModel_For_Author()
        {
            // Arrange
            var authorId = Guid.NewGuid();
            var authorName = "author";

            var authenticatedAuthor = (AuthorDTO?)null;

            // Act
            var actual = new AuthorViewModel(authorId, authorName, authenticatedAuthor);

            // Assert
            Assert.Equal(authorId, actual.Id);
            Assert.Equal(authorName, actual.Name);
            Assert.False(actual.CanFollow);
            Assert.False(actual.CanUnfollow);
        }

        [Fact]
        public void Constructor_With_Authenticated_Author_Same_Author_Should_Return_ViewModel_For_Author_Cannot_Follow_And_UnFollow()
        {
            // Arrange
            var authorId = Guid.NewGuid();
            var authorName = "author";

            var authenticatedAuthor = new AuthorDTO(
                authorId,
                authorName,
                Enumerable.Empty<Guid>());

            // Act
            var actual = new AuthorViewModel(authorId, authorName, authenticatedAuthor);

            // Assert
            Assert.False(actual.CanFollow);
            Assert.False(actual.CanUnfollow);
        }

        [Fact]
        public void Constructor_With_Authenticated_Author_Not_Following_Author_Should_Return_ViewModel_For_Author_Can_Follow()
        {
            // Arrange
            var authorId = Guid.NewGuid();
            var authorName = "author";

            var authenticatedAuthor = new AuthorDTO(
                Guid.NewGuid(),
                "authenticatedAuthor",
                Enumerable.Empty<Guid>());

            // Act
            var actual = new AuthorViewModel(authorId, authorName, authenticatedAuthor);

            // Assert
            Assert.True(actual.CanFollow);
        }

        [Fact]
        public void Constructor_With_Authenticated_Author_Following_Author_Should_Return_ViewModel_For_Author_Cannot_Follow()
        {
            // Arrange
            var authorId = Guid.NewGuid();
            var authorName = "author";

            var authenticatedAuthor = new AuthorDTO(
                Guid.NewGuid(),
                "authenticatedAuthor",
                new Guid[] { authorId });

            // Act
            var actual = new AuthorViewModel(authorId, authorName, authenticatedAuthor);

            // Assert
            Assert.False(actual.CanFollow);
        }

        [Fact]
        public void Constructor_With_Authenticated_Author_Not_Following_Author_Should_Return_ViewModel_For_Author_Cannot_Unfollow()
        {
            // Arrange
            var authorId = Guid.NewGuid();
            var authorName = "author";

            var authenticatedAuthor = new AuthorDTO(
                Guid.NewGuid(),
                "authenticatedAuthor",
                Enumerable.Empty<Guid>());

            // Act
            var actual = new AuthorViewModel(authorId, authorName, authenticatedAuthor);

            // Assert
            Assert.False(actual.CanUnfollow);
        }

        [Fact]
        public void Constructor_With_Authenticated_Author_Following_Author_Should_Return_ViewModel_For_Author_Can_Unfollow()
        {
            // Arrange
            var authorId = Guid.NewGuid();
            var authorName = "author";

            var authenticatedAuthor = new AuthorDTO(
                Guid.NewGuid(),
                "authenticatedAuthor",
                new Guid[] { authorId });

            // Act
            var actual = new AuthorViewModel(authorId, authorName, authenticatedAuthor);

            // Assert
            Assert.True(actual.CanUnfollow);
        }
    }
}

