using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using Chirp.Web.ViewModels;
using Microsoft.IdentityModel.Tokens;

namespace Chirp.Web.Pages
{
    public class PublicModel : PageModel
    {
        private const int DEFAULT_PAGE_NUMBER = 1;

        [FromQuery(Name = "page")]
        public int? PageNumber { get; set; }

        private readonly IPresentationService presentationService;

        public bool AllowCheepShare { get; private set; }

        public CheepListViewModel CheepsListViewModel { get; private set; }

        public CheepShareViewModel CheepShareViewModel { get; private set; }

        public PublicModel(IPresentationService presentationService)
        {
            this.presentationService = presentationService
                ?? throw new ArgumentNullException(nameof(presentationService));

            this.CheepsListViewModel = new CheepListViewModel();
            this.CheepShareViewModel = new CheepShareViewModel();
        }

        public async Task<ActionResult> OnGet([FromQuery(Name = "page")] int? pageNumber)
        {
            this.AllowCheepShare = this.presentationService.GetAuthenticatedAuthor() != null;

            this.CheepsListViewModel = await this.presentationService.GetAllCheepsViewModel(this.GetPageNumber(this.PageNumber));

            return Page();
        }

        public async Task<ActionResult> OnPostShare(string cheepText)
        {
            if (!ModelState.IsValid)
            {
                return await OnGet(null);
            }

            await this.presentationService.CreateCheep(cheepText);

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
