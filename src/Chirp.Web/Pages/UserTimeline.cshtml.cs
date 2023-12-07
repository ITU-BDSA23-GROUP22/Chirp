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

        private readonly IHelperService helperService;

        public AuthorDTO? Author { get; private set; }

        public CheepListViewModel CheepsListViewModel { get; private set; }


        public UserTimelineModel(IHelperService helperService)
        {
            this.helperService = helperService
                ?? throw new ArgumentNullException(nameof(helperService));

            this.CheepsListViewModel = new CheepListViewModel();
        }

        public async Task<ActionResult> OnGet([FromQuery(Name = "page")] int? pageNumber)
        {
            if (this.AuthorId == null && this.AuthorId == Guid.Empty)
            {

            }
            this.Author = await this.helperService.GetAuthor(this.AuthorId);

            var authorIds = new List<Guid> { this.AuthorId };

            var authenticatedAuthor = await this.helperService.GetAuthor();
            if (authenticatedAuthor?.Id == this.Author?.Id)
            {
                authorIds.AddRange(this.Author.followingIds);
            }

            this.CheepsListViewModel = await this.helperService.GetCheepsByAuthorsViewModel(authorIds, this.GetPageNumber(pageNumber), $"/{this.AuthorId}");

            return Page();
        }

        public async Task<ActionResult> OnPostFollow(Guid authorToFollowId, int pageNumber)
        {
            await this.helperService.FollowAuthor(authorToFollowId);

            return Redirect($"/{this.AuthorId}?page={this.GetPageNumber(pageNumber)}");
        }

        public async Task<ActionResult> OnPostUnfollow(Guid authorToUnfollowId, int pageNumber)
        {
            await this.helperService.UnfollowAuthor(authorToUnfollowId);

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