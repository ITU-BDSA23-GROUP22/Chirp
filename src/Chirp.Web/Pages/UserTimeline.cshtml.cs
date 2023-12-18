using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Chirp.Core;
using System.ComponentModel.DataAnnotations;
using Chirp.Web.ViewModels;

namespace Chirp.Web.Pages
{
    public class UserTimelineModel : PageModel
    {
        private const int DEFAULT_PAGE_NUMBER = 1;

        #region Mapped Razor properties 

        [BindProperty(SupportsGet = true, Name = "author")]
        [Required()]
        public Guid AuthorId { get; set; }

        #endregion

        private readonly IPresentationService presentationService;

        public AuthorDTO? Author { get; private set; }

        public CheepListViewModel CheepsListViewModel { get; private set; }


        public UserTimelineModel(IPresentationService presentationService)
        {
            this.presentationService = presentationService
                ?? throw new ArgumentNullException(nameof(presentationService));

            this.CheepsListViewModel = new CheepListViewModel();
        }

        public async Task<ActionResult> OnGet([FromQuery(Name = "page")] int? pageNumber)
        {
            this.Author = await this.presentationService.GetAuthor(this.AuthorId)
                ?? throw new Exception("Get UserTimeline failed - Invalid authorId");

            var authorIds = new List<Guid> { this.AuthorId };

            var authenticatedAuthor = this.presentationService.GetAuthenticatedAuthor();
            if (authenticatedAuthor != null)
            {

                // For authenticated author also view cheeps from followed authors
                if (authenticatedAuthor.Id == this.Author.Id)
                {
                    authorIds.AddRange(this.Author.followingIds);
                }
            }

            this.CheepsListViewModel = await this.presentationService.GetCheepsByAuthorsViewModel(authorIds, this.GetPageNumber(pageNumber), $"/{this.AuthorId}");

            return Page();
        }

        public async Task<ActionResult> OnPostFollow(Guid authorToFollowId, int pageNumber)
        {
            await this.presentationService.FollowAuthor(authorToFollowId);

            return Redirect($"/{this.AuthorId}?page={this.GetPageNumber(pageNumber)}");
        }

        public async Task<ActionResult> OnPostUnfollow(Guid authorToUnfollowId, int pageNumber)
        {
            await this.presentationService.UnfollowAuthor(authorToUnfollowId);

            return Redirect($"/{this.AuthorId}?page={this.GetPageNumber(pageNumber)}");
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