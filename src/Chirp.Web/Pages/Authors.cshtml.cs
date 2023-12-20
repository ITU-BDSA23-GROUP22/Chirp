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

        /// <summary>
        ///  Handles Get request for Author list for anonymous and authenticated users
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="searchText"></param>
        /// <returns></returns>
        public async Task<ActionResult> OnGet(
            [FromQuery(Name = "page")] int? pageNumber,
            [FromQuery(Name = "search")] string? searchText
           )
        {

            this.AuthorListViewModel = await presentationService.GetAuthorListViewModel(searchText, this.GetPageNumber(pageNumber));

            return Page();
        }

        /// <summary>
        ///     Handles Post request for Search Authors for anonymous and authenticated users
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<ActionResult> OnPostSearch(AuthorSearchViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return await OnGet(1, model.SearchText);
            }

            return Redirect($"/Authors?search={WebUtility.UrlEncode(model.SearchText)}&page=1");
        }

        /// <summary>
        ///     Handles Post request for Follow Author for authenticated users
        /// </summary>
        /// <param name="authorToFollowId"></param>
        /// <param name="pageNumber"></param>
        /// <param name="searchText"></param>
        /// <returns></returns>
        public async Task<ActionResult> OnPostFollow(Guid authorToFollowId, int pageNumber, string? searchText)
        {
            await this.presentationService.FollowAuthor(authorToFollowId);

            return Redirect($"/Authors?search={WebUtility.UrlEncode(searchText)}&page={this.GetPageNumber(pageNumber)}");
        }

        /// <summary>
        ///     Handles Post request for Unfollow Author for authenticated users
        /// </summary>
        /// <param name="authorToUnfollowId"></param>
        /// <param name="pageNumber"></param>
        /// <param name="searchText"></param>
        /// <returns></returns>
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
