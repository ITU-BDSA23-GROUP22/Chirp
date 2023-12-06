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
        public string? page { get; set; } = null!;

        [FromForm(Name = "cheepText")]
        [Required(ErrorMessage = "Cheep text is required")]
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
                }
                else
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
                return Unauthorized();
            }

            if (!ModelState.IsValid)
            {
                this.Cheeps = await this.chirpService.GetAllCheeps(this.GetPageNumber());
                return Page();
            }

            await this.chirpService.CreateCheep(authorDto, this.CheepText);

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

        public bool ShouldShowValidation()
        {
            if (Request.Method.ToLower() == "post" && !ModelState.IsValid)
            {
                return true;
            }
            return false;
        }


        public bool IsUserAuthenticated()
        {
            return User.Identity?.IsAuthenticated == true;
        }

        #region Private methods

        private AuthorDTO? GetAuthenticatedAuthor()
        {

            if (IsUserAuthenticated())
            {
                return new AuthorDTO(
                    Guid.Empty,
                    User.Identity?.Name ?? string.Empty,
                    User.Claims?.SingleOrDefault(x => x.Type == "emails")?.Value ?? string.Empty,
                    new List<Guid>()
                );
            }
            return null;
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