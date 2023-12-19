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

        public async Task<ActionResult> OnGet([FromQuery(Name = "page")] int? pageNumber)
        {
            this.AllowCheepShare = this.presentationService.GetAuthenticatedAuthor() != null;

            this.CheepsListViewModel = await this.presentationService.GetAllCheepsViewModel(this.GetPageNumber(pageNumber));

            return Page();
        }

        public async Task<ActionResult> OnPostShare(CheepShareViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return await OnGet(null);
            }

            await this.presentationService.CreateCheep(model.CheepText);

            return Redirect($"/?page=1");
        }

        public async Task<ActionResult> OnPostFollow(Guid authorToFollowId, int pageNumber)
        {
            await this.presentationService.FollowAuthor(authorToFollowId);

            return Redirect($"/?page={this.GetPageNumber(pageNumber)}");
        }

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
