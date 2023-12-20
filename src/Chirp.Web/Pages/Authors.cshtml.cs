using System.Net;
using Chirp.Web;
using Chirp.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.web.Pages
{
	public class AuthorsModel : PageModel
    {

        private const int DEFAULT_PAGE_NUMBER = 1;

        private readonly IPresentationService presentationService;

        public AuthorListViewModel AuthorListViewModel { get; private set; }

        public AuthorsModel(IPresentationService presentationService)
        {
            this.presentationService = presentationService
                ?? throw new ArgumentNullException(nameof(presentationService));

            this.AuthorListViewModel = new AuthorListViewModel();
        }

        public async Task<ActionResult> OnGet(
            [FromQuery(Name = "page")] int? pageNumber,
            [FromQuery(Name = "search")] string? searchText
           )
        {

            this.AuthorListViewModel = await presentationService.GetAuthorListViewModel(searchText, this.GetPageNumber(pageNumber));

            return Page();
        }

        public async Task<ActionResult> OnPostSearch(AuthorSearchViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return await OnGet(1, model.SearchText);
            }

            return Redirect($"/Authors?search={WebUtility.UrlEncode(model.SearchText)}&page=1");
        }

        public async Task<ActionResult> OnPostFollow(Guid authorToFollowId, int pageNumber, string? searchText)
        {
            await this.presentationService.FollowAuthor(authorToFollowId);

            return Redirect($"/Authors?search={WebUtility.UrlEncode(searchText)}&page={this.GetPageNumber(pageNumber)}");
        }

        public async Task<ActionResult> OnPostUnfollow(Guid authorToUnfollowId, int pageNumber, string? searchText)
        {
            await this.presentationService.UnfollowAuthor(authorToUnfollowId);

            return Redirect($"/Authors?search={WebUtility.UrlEncode(searchText)}&page={this.GetPageNumber(pageNumber)}");
        }

        private int GetPageNumber(int? pageNumber)
        {
            if (pageNumber != null && pageNumber > 0)
            {
                return (int)pageNumber;
            }
            return DEFAULT_PAGE_NUMBER;
        }
    }
}
