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

        [BindProperty(SupportsGet = true, Name = "author")]
        [Required()]
        public Guid AuthorId { get; set; }

        private readonly IPresentationService presentationService;

        public bool AllowCheepShare { get; private set; }

        public AuthorDTO? Author { get; private set; }

        public CheepListViewModel CheepsListViewModel { get; private set; }

        public UserTimelineModel(IPresentationService presentationService)
        {
            this.presentationService = presentationService
                ?? throw new ArgumentNullException(nameof(presentationService));

            this.CheepsListViewModel = new CheepListViewModel();
        }

        /// <summary>
        ///     Handles Get request for Cheep List for specified Auhtor for anonymous and authenticated users
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<ActionResult> OnGet([FromQuery(Name = "page")] int? pageNumber)
        {
            this.Author = await this.presentationService.GetAuthor(this.AuthorId)
                ?? throw new Exception("Get UserTimeline failed - Invalid authorId");

            var authorIds = new List<Guid> { this.AuthorId };

            var authenticatedAuthor = this.presentationService.GetAuthenticatedAuthor();
            if (authenticatedAuthor != null)
            {
                // Allow authenticated author on my-timeline
                this.AllowCheepShare = true;

                // For authenticated author also view cheeps from followed authors
                if (authenticatedAuthor.Id == this.Author.Id)
                {
                    authorIds.AddRange(this.Author.followingIds);
                }
            }

            this.CheepsListViewModel = await this.presentationService.GetCheepsByAuthorsViewModel(authorIds, this.GetPageNumber(pageNumber), $"/{this.AuthorId}");

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

            return Redirect($"/{this.AuthorId}?page=1");
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

            return Redirect($"/{this.AuthorId}?page={this.GetPageNumber(pageNumber)}");
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