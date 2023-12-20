// ReferenceLink:
//  https://learn.microsoft.com/en-us/dotnet/core/testing/unit-testing-best-practices

using Xunit;
using Chirp.Web.ViewModels;
using Chirp.Core;

namespace Chirp.Web.Tests.ViewModels
{
    public class CheepViewModelUnitTests
    {
        public CheepViewModelUnitTests()
        {
        }

        [Fact]
        public void Constructor_With_Cheep_And_Author_Should_Return_ViewModel_For_Cheep_With_Author()
        {
            // Arrange
            var cheepId = Guid.NewGuid();
            var cheepText = "Hello";
            var cheepTimestamp = "15:10";

            var authenticatedAuthor = (AuthorDTO?)null;

            var authorViewModel = new AuthorViewModel(
                Guid.NewGuid(),
                "author",
                authenticatedAuthor
                );

            // Act
            var cheepViewModel = new CheepViewModel(cheepId, cheepText, cheepTimestamp, authorViewModel);

            // Assert
            Assert.Equal(cheepId, cheepViewModel.Id);
            Assert.Equal(cheepText, cheepViewModel.Message);
            Assert.Equal(cheepTimestamp, cheepViewModel.Timestamp);
            Assert.Equal(authorViewModel.Id, cheepViewModel.Author.Id);
            Assert.Equal(authorViewModel.Name, cheepViewModel.Author.Name);
        }
    }
}

