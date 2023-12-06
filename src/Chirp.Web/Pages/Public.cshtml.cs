#define USE_FAKE_AUTHENTICATION // NOTE: allows for easily faking authenticated user for development support

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Chirp.Core;
using System.Security.Claims;
using Chirp.Core.Services;
using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;

namespace Chirp.Web.Pages
{

    public class PublicModel : PageModel
    {
        #region Mapped Razor properties 

        [FromQuery(Name = "page")]
        public string page { get; set; } = null!;

        [FromForm(Name = "cheepText")]
        [MaxLength(160)]
        public string CheepText { get; set; } = string.Empty;

        #endregion

        private readonly IChirpService chirpService;

        public IEnumerable<CheepDTO> Cheeps { get; set; } = null!;

        public CheepsPageModel cheepsModel { get; set; }

        public PublicModel(IChirpService chirpService)
        {
            this.chirpService = chirpService;
        }


        public async Task<ActionResult> OnGet()
        {
            this.Cheeps = await this.chirpService.GetAllCheeps(this.GetPageNumber());

            this.cheepsModel = new CheepsPageModel();
            this.cheepsModel.cheeps = this.Cheeps;

            var authorDto = this.GetAuthenticatedAuthor();
            if (authorDto == null)
            {
                this.cheepsModel.authorsFollowedByAuthenticatedUser = new List<Guid>();
            }
            else
            {   
                var authenticatedUser = await chirpService.GetAuthor(authorDto.Email);
                if (authenticatedUser == null)
                {
                    this.cheepsModel.authorsFollowedByAuthenticatedUser = new List<Guid>();
                } else
                {
                    this.cheepsModel.authorsFollowedByAuthenticatedUser = authenticatedUser.followingIds;
                }
            }

            

            return Page();
        }

        public async Task<ActionResult> OnPost()
        {
            var authorDto = this.GetAuthenticatedAuthor();
            if (authorDto == null)
            {
                return BadRequest();
            }

            var cheepText = this.CheepText
                ?? throw new Exception($"Failed to post cheep - no text specified");

            await this.chirpService.CreateCheep(authorDto, cheepText);

            return RedirectToPage("/Public");
        }

        public async Task<ActionResult> OnPostFollow(Guid authorToFollowId)
        {
            var authorDto = this.GetAuthenticatedAuthor();
            if (authorDto == null)
            {
                return BadRequest();
            }

            await this.chirpService.FollowAuthor(authorDto, authorToFollowId);

            return RedirectToPage("/Public");
        }

        public async Task<ActionResult> OnPostUnfollow(Guid authorToUnfollowId)
        {
            var authorDto = this.GetAuthenticatedAuthor();
            if (authorDto == null)
            {
                return BadRequest();
            }

            await this.chirpService.UnfollowAuthor(authorDto, authorToUnfollowId);

            return RedirectToPage("/Public");
        }

        public bool IsUserAuthenticated()
        {
#if         (USE_FAKE_AUTHENTICATION)
            return true;
#else
            return User.Identity?.IsAuthenticated == true;
#endif
        }

        #region Private methods

        private AuthorDTO? GetAuthenticatedAuthor()
        {
#if (USE_FAKE_AUTHENTICATION)
            return new AuthorDTO(Guid.Empty, "FAKENAME", "FAKE@EMAIL", new List<Guid>());
#else
            if (IsUserAuthenticated())
            {
                return new AuthorDTO(
                    Guid.Empty,
                    User.Identity?.Name ?? string.Empty,
                    User.Claims?.SingleOrDefault(x => x.Type == "emails")?.Value ?? string.Empty
                );
            }
            return null;
#endif
        }

        private int GetPageNumber()
        {
            if (int.TryParse(this.page, out var pageNumber))
            {
                return pageNumber;
            }
            return 1;
        }

        #endregion

    }

    public class CheepsPageModel
    {
        public IEnumerable<Chirp.Core.CheepDTO> cheeps { get; set; }

        public IEnumerable<Guid> authorsFollowedByAuthenticatedUser { get; set; }

    }
}