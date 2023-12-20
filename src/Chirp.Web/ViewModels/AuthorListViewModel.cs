using System.Net;
using Chirp.Core;

namespace Chirp.Web.ViewModels
{
    public class AuthorListViewModel
    {
        public IEnumerable<AuthorViewModel> Authors { get; }

        public string PageUrl { get; }
        public int PageNumber { get; }
        public string NavigateToPreviousPageUrl { get; }
        public string NavigateToNextPageUrl { get; }
        public string SearchText { get;  }

        public AuthorListViewModel()
        {
            this.Authors = Enumerable.Empty<AuthorViewModel>();
            this.PageUrl = string.Empty;
            this.NavigateToNextPageUrl = string.Empty;
            this.NavigateToPreviousPageUrl = string.Empty;
            this.SearchText = string.Empty;
        }

        public AuthorListViewModel(AuthorDTO? authenticatedAuthor, IEnumerable<AuthorDTO> authors, int pageNumber, int authorsPerPage, string pageUrl, string? searchText)
        {
            this.Authors = authors
                .Take(authorsPerPage)
                .Select(authorDto =>
            {

                var authorViewModel = new AuthorViewModel(
                     authorDto.Id,
                     authorDto.Name,
                     authenticatedAuthor
                     );

                return authorViewModel;
            });

            var urlEncodedSearchParam = string.IsNullOrWhiteSpace(searchText) ? string.Empty : $"search={WebUtility.UrlEncode(searchText ?? string.Empty)}&";

            this.PageUrl = pageUrl;
            this.PageNumber = pageNumber;
            this.NavigateToPreviousPageUrl = pageNumber > 1 ? $"{pageUrl}?{urlEncodedSearchParam}page={pageNumber - 1}" : string.Empty;
            this.NavigateToNextPageUrl = authors.Count() > authorsPerPage ? $"{pageUrl}?{urlEncodedSearchParam}page={pageNumber + 1}" : string.Empty;
            this.SearchText = searchText ?? string.Empty;
        }
    }
}