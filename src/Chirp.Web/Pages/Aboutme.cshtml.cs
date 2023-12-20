using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Chirp.Core;
using Chirp.Core.Services;
using Microsoft.AspNetCore.Authentication;
using System.Text;

namespace Chirp.Web.Pages
{
    public class AboutMeModel : PageModel
    {
        public const int MAX_DOWNLOAD_CHEEPS = 1000;

        private readonly IChirpService chirpService;

        private readonly IPresentationService presentationService;

        public IEnumerable<AuthorDTO> Authors { get; set; } = null!;

        public AboutMeModel(IChirpService chirpService, IPresentationService presentationService)
        {
            this.presentationService = presentationService
                ?? throw new ArgumentNullException(nameof(presentationService));

            this.chirpService = chirpService;
        }

        /// <summary>
        ///     Handles Get request for authenticated Author Information
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> OnGet()
        {
            var authenticatedUser = presentationService.GetAuthenticatedAuthor();

            if (authenticatedUser == null)
            {
                return Unauthorized();
            }

            this.Authors = await presentationService.GetFollowingAuthors(authenticatedUser.Id);


            return Page();
            
        }

        /// <summary>
        ///     Handles Post request for ForgetMe functionality
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> OnPostForgetMe()
        {
            await HttpContext.SignOutAsync();

            await chirpService.AnonymizeAuthor(presentationService.GetAuthenticatedAuthor().Id);

            return RedirectToPage("/Public");
        }

        /// <summary>
        ///     Handles Post request for DownloadMyInfo functionality
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> OnPostDownloadMyInfo()
        {

            var authenticatedUser = presentationService.GetAuthenticatedAuthor();

            if (authenticatedUser == null)
            {
                return Unauthorized();
            }

            StringBuilder sb = new StringBuilder();

            sb.AppendLine("Name:");
            sb.AppendLine(authenticatedUser.Name);
            sb.AppendLine();
            sb.AppendLine("Followed users:");

            var authors = await presentationService.GetFollowingAuthors(authenticatedUser.Id);

            foreach(var followedAuthor in authors)
            {
                sb.AppendLine($"{followedAuthor.Name}");
            }

            sb.AppendLine();
            sb.AppendLine("Cheeps:");

            var cheeps = await chirpService.GetCheepsByAuthors(new[] { authenticatedUser.Id }, 1, 0, MAX_DOWNLOAD_CHEEPS);

            foreach (var cheep in cheeps)
            {
                sb.AppendLine($"'{cheep.Message}' at {cheep.Timestamp}");
            }

            Response.Headers["Content-Disposition"] = "attachment;filename=information.txt";

            Response.ContentType = "text/plain";

            await Response.Body.WriteAsync(Encoding.UTF8.GetBytes(sb.ToString()));

            return new EmptyResult();
        }
    }
}