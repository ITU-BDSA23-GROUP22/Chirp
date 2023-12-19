using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using Chirp.Web.ViewModels;

namespace Chirp.Web.Pages
{
    public class PublicModel : PageModel
    {
        private const int DEFAULT_PAGE_NUMBER = 1;

        #region Mapped Razor properties 

        [FromForm(Name = "cheepText")]
        [Required(ErrorMessage = "A cheep message is required")]
        [MaxLength(160)]
        public string CheepText { get; set; } = string.Empty;

        public int PageNumber { get; set; }

        #endregion

        private readonly IPresentationService presentationService;

        public CheepListViewModel CheepsListViewModel { get; private set; }

        public PublicModel(IPresentationService presentationService)
        {
            this.presentationService = presentationService
                ?? throw new ArgumentNullException(nameof(presentationService));

            this.CheepsListViewModel = new CheepListViewModel();
        }

        public async Task<ActionResult> OnGet([FromQuery(Name = "page")] int? pageNumber)
        {
            this.CheepsListViewModel = await this.presentationService.GetAllCheepsViewModel(this.GetPageNumber(pageNumber));

            return Page();
        }

        public async Task<ActionResult> OnPostShare(int pageNumber)
        {
            if (!ModelState.IsValid)
            {
                this.CheepsListViewModel = await this.presentationService.GetAllCheepsViewModel(this.GetPageNumber(pageNumber));

                return Page();
            }

            await this.presentationService.CreateCheep(this.CheepText);

            return RedirectToPage("Public");
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
