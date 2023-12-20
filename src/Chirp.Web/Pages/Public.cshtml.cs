using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Chirp.Web.ViewModels;

namespace Chirp.Web.Pages
{
    public class PublicModel : PageModel
    {
        private const int DEFAULT_PAGE_NUMBER = 1;

        private readonly IPresentationService presentationService;

        public bool AllowCheepShare { get; private set; }

        public CheepListViewModel CheepsListViewModel { get; private set; }

        public PublicModel(IPresentationService presentationService)
        {
            this.presentationService = presentationService
                ?? throw new ArgumentNullException(nameof(presentationService));

            this.CheepsListViewModel = new CheepListViewModel();
        }

        /// <summary>
        ///     Handles Get request for Cheep list for anonymous and authenticated users 
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <returns></returns>
        public async Task<ActionResult> OnGet([FromQuery(Name = "page")] int? pageNumber)
        {
            this.AllowCheepShare = this.presentationService.GetAuthenticatedAuthor() != null;

            this.CheepsListViewModel = await this.presentationService.GetAllCheepsViewModel(this.GetPageNumber(pageNumber));

            return Page();
        }

        /// <summary>
        ///     Handles Post request for Share Cheep for authenticated users
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<ActionResult> OnPostShare(CheepShareViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return await OnGet(null);
            }

            await this.presentationService.CreateCheep(model.CheepText);

            return Redirect($"/?page=1");
        }

        /// <summary>
        ///     Handles Post request for Follow Author for authenticated users
        /// </summary>
        /// <param name="authorToFollowId"></param>
        /// <param name="pageNumber"></param>
        /// <returns></returns>
        public async Task<ActionResult> OnPostFollow(Guid authorToFollowId, int pageNumber)
        {
            await this.presentationService.FollowAuthor(authorToFollowId);

            return Redirect($"/?page={this.GetPageNumber(pageNumber)}");
        }

        /// <summary>
        ///     Handles Post request for Unfollow Author for authenticated users
        /// </summary>
        /// <param name="authorToUnfollowId"></param>
        /// <param name="pageNumber"></param>
        /// <returns></returns>
        public async Task<ActionResult> OnPostUnfollow(Guid authorToUnfollowId, int pageNumber)
        {
            await this.presentationService.UnfollowAuthor(authorToUnfollowId);

            return Redirect($"/?page={this.GetPageNumber(pageNumber)}");
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
